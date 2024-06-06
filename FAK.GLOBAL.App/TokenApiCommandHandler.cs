using FAK.Common.Model;
using FAK.Domain.Entities;
using FAK.Domain;
using FAK.Persistance;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace FAK.GLOBAL.App
{
    public class TokenApiCommandHandler : AppBase, IRequestHandler<CommandsModel<TokenApiCommand, User>, User>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly JWTConfig _jWTConfig;
        private readonly UserManager<IdentityUser> _userManager;
        public TokenApiCommandHandler(
            AppDbContext context, IAuthenticationService authenticationService,
            IConfiguration configuration, IOptions<JWTConfig> jwtConfig, UserManager<IdentityUser> userManager) : base(context)
        {
            _context = context;
            _authenticationService = authenticationService;
            _configuration = configuration;
            _jWTConfig = jwtConfig.Value;
            _userManager = userManager;
        }


        public async Task<User> Handle(CommandsModel<TokenApiCommand, User> req, CancellationToken cancellationToken)

        {
            string accessToken = req.CommandModel.AccessToken;
            string refreshToken = req.CommandModel.RefreshToken;


            var principal = GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = _context.Users.SingleOrDefault(u => u.UserName == username);
            if (user is null)
                return user;
            var newAccessToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = GenerateRefreshToken();
            user.Token = newAccessToken;
            user.RefreshToken = newRefreshToken;
            return user;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWTConfig:Key").Value));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: _configuration.GetSection("JWTConfig:Issuer").Value,
                audience: _configuration.GetSection("JWTConfig:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWTConfig:Key").Value)),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
