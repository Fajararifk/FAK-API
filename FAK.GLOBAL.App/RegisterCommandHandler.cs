using BCrypt.Net;
using FAK.Common.Model;
using FAK.Domain.Entities;
using FAK.Persistance;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App
{
    public class RegisterCommandHandler : AppBase, IRequestHandler<CommandsModel<RegisterCommand, User>, User>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly UserManager<IdentityUser> _userManager;
        public RegisterCommandHandler(
            AppDbContext context, IAuthenticationService authenticationService,
            IConfiguration configuration, IPasswordHasher<User> passwordHasher, UserManager<IdentityUser> userManager) : base(context)
        {
            _context = context;
            _authenticationService = authenticationService;
            _configuration = configuration;
            _userManager = userManager;
        }


        public async Task<User> Handle(CommandsModel<RegisterCommand, User> req, CancellationToken cancellationToken)

        {

            if (req.CommandModel == null)
                throw new ArgumentNullException(nameof(req));
            var user = new User()
            {
                Email = req.CommandModel.Email,
                UserName = req.CommandModel.UserName,
                ConfirmPassword = req.CommandModel.ConfirmPassword
                //role = req.commandmodel.role,
                //token = req.CommandModel.token,
                //password = req.commandmodel.password,
                //confirmpassword = req.commandmodel.confirmpassword,
            };

            var userIdentity = new IdentityUser
            {
                UserName = req.CommandModel.UserName,
                Email = req.CommandModel.Email,
            };

            var result = await _userManager.CreateAsync(userIdentity, req.CommandModel.Password);
            if(result.Succeeded && req.CommandModel.Password == req.CommandModel.ConfirmPassword)
            {
                user.Password = req.CommandModel.Password;            
                return user;
            }
            else
            {
                return user;
            }
            
        }
    }
}
