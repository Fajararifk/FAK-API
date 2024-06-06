using FAK.Common.Model;
using FAK.Domain;
using FAK.Domain.Entities;
using FAK.Persistance;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App
{
    public class LoginCommandHandler : AppBase, IRequestHandler<CommandsModel<LoginCommand, User>, User>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly JWTConfig _jWTConfig;
        private readonly UserManager<IdentityUser> _userManager;
        public LoginCommandHandler(
            AppDbContext context, IAuthenticationService authenticationService,
            IConfiguration configuration, IOptions<JWTConfig> jwtConfig, UserManager<IdentityUser> userManager) : base(context)
        {
            _context = context;
            _authenticationService = authenticationService;
            _configuration = configuration;
            _jWTConfig = jwtConfig.Value;
            _userManager = userManager;
        }


        public async Task<User> Handle(CommandsModel<LoginCommand, User> req, CancellationToken cancellationToken)

        {
            bool isNull = false;
            var identityUser = await _userManager.FindByNameAsync(req.CommandModel.UserName);
            var user = new User();
            if (identityUser is null)
            {
                isNull = true;
            }
            else
            {
                var checkPassword = await _userManager.CheckPasswordAsync(identityUser, req.CommandModel.Password);
                if (checkPassword)
                {
                    isNull = false;
                    user.Email = identityUser.Email;
                    var token = GenerateToken(user);
                    user.Token = token;
                    user.UserName = identityUser.UserName;
                    user.Password = req.CommandModel.Password;
                }

            }
            
            if (isNull)
            {
                return user;
            }
            else
            {
                return user;
            }
            //LoginModel loginInfo = new LoginModel();
            //bool logged = false;
            //if (req == null)
            //    loginInfo.sts = "Invalid Username or Password";

            //var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == req.CommandModel.UserName);
            //if (user != null && BCrypt.Net.BCrypt.Verify(req.CommandModel.Password, user.Password))
            //{
            //    user.Token = GenerateToken(user);
            //    loginInfo.sts = "Login";
            //}
            //else
            //{
            //    loginInfo.sts = "User Not Found";

            //}
            //return user;

        }

        private string GenerateToken(User user)
        {
            IEnumerable<System.Security.Claims.Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Password", user.Password),
                new Claim("UserName", user.UserName)
            };
            var securityKeys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWTConfig:Key").Value));
            //Guid guid = Guid.Parse(_configuration.GetSection("JWTConfig:Key").Value);
            //byte[] keyByte = guid.ToByteArray();
            //var bitConverter = BitConverter.ToString(keyByte).Replace("-", "");
            //byte[] keyBytes = new byte[64]; // 512 bits
            //using (var rng = new RNGCryptoServiceProvider())
            //{
            //    rng.GetBytes(keyBytes);
            //}


            //var securityKey = new SymmetricSecurityKey(keyByte);
            var signingCred = new SigningCredentials(securityKeys, SecurityAlgorithms.HmacSha256Signature);
            
            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                issuer: _configuration.GetSection("JWTConfig:Issuer").Value,
                audience: _configuration.GetSection("JWTConfig:Audience").Value,
                signingCredentials: signingCred);

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
            //var jwtTokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.UTF8.GetBytes(_jWTConfig.Key);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new System.Security.Claims.ClaimsIdentity(new[]
            //    {
            //        new System.Security.Claims.Claim(JwtRegisteredClaimNames.Name, user.UserName),
            //        new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email),
            //        new System.Security.Claims.Claim(JwtRegisteredClaimNames.Birthdate, user.Role)

            //    }),
            //    Expires = DateTime.UtcNow.AddHours(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),

            //};

            //var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            //return jwtTokenHandler.WriteToken(token);
        }
    }
}
