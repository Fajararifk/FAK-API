using FAK.Common.Model;
using FAK.Domain.Entities;
using FAK.Domain;
using FAK.Persistance;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FAK.Infrastructure.Services;

namespace FAK.GLOBAL.App
{
    public class ResetPasswordCommandHandler : AppBase, IRequestHandler<CommandsModel<ResetPasswordCommand, User>, User>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly JWTConfig _jWTConfig;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        public ResetPasswordCommandHandler(
            AppDbContext context, IAuthenticationService authenticationService,
            IConfiguration configuration, IOptions<JWTConfig> jwtConfig, UserManager<IdentityUser> userManager, IEmailSender emailSender) : base(context)
        {
            _context = context;
            _authenticationService = authenticationService;
            _configuration = configuration;
            _jWTConfig = jwtConfig.Value;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        public async Task<User> Handle(CommandsModel<ResetPasswordCommand, User> req, CancellationToken cancellationToken)

        {
            bool isNull = false;
            var userForgot = _context.Users.OrderBy(x => x.RequestResetTime).FirstOrDefault().Email;
            var identityUser = await _userManager.FindByEmailAsync(userForgot);
            var user = new User();
            if (identityUser is null)
            {
                isNull = true;
            }
            else
            {
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                var resetPassword = await _userManager.ResetPasswordAsync(identityUser, passwordResetToken, req.CommandModel.newPassword);
                if (resetPassword.Succeeded)
                {
                    user.Email = identityUser.Email;
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    _emailSender.SendEmail(_configuration.GetSection("Email").Value, user.Email, "Forgot Password", "Lupa Password");
                }
                return user;

            }
            return user;
        }
    }
}
