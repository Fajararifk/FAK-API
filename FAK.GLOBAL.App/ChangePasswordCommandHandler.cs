using FAK.Common.Model;
using FAK.Domain.Entities;
using FAK.Infrastructure.Services;
using FAK.Persistance;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App
{
    public class ChangePasswordCommandHandler : AppBase, IRequestHandler<CommandsModel<ChangePasswordCommand, User>, User>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        public ChangePasswordCommandHandler(
            AppDbContext context, IAuthenticationService authenticationService,
            IConfiguration configuration, UserManager<IdentityUser> userManager) : base(context)
        {
            _context = context;
            _authenticationService = authenticationService;
            _configuration = configuration;
            _userManager = userManager;
        }


        public async Task<User> Handle(CommandsModel<ChangePasswordCommand, User> req, CancellationToken cancellationToken)

        {

            bool isNull = false;
            var userForgot = _context.Users.OrderBy(x=>x.ForgotTime).FirstOrDefault().Email;
            var identityUser = await _userManager.FindByEmailAsync(userForgot);
            var user = new User();
            if (identityUser is null)
            {
                isNull = true;
            }
            else
            {
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                var passwordChanged = await _userManager.ChangePasswordAsync(identityUser, req.CommandModel.Password, req.CommandModel.newPassword);
                if (passwordChanged.Succeeded)
                {
                    user.Email = identityUser.Email;
                    //_emailSender.SendEmail(_configuration.GetSection("Email").Value, user.Email, "Password Telah Diganti", "Ingat Password Terbarumu");
                }
                

            }
            return user;
            //LoginModel loginInfo = new LoginModel();
            //bool logged = false;
            //if (req == null)
            //    loginInfo.sts = "Invalid Username or Password";

            //var user = await _context.Users.FirstOrDefaultAsync(x => x.Password == req.CommandModel.Password);
            //if (user == null)
            //{
            //    user.Message = "User Not Found";
            //}
            //return user;

        }
    }
}
