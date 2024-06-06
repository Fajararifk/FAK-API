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
    public class RequestResetPasswordCommandHandler : AppBase, IRequestHandler<CommandsModel<RequestResetPasswordCommand, User>, User>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly JWTConfig _jWTConfig;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        public RequestResetPasswordCommandHandler(
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


        public async Task<User> Handle(CommandsModel<RequestResetPasswordCommand, User> req, CancellationToken cancellationToken)

        {
            bool isNull = false;
            var identityUser = await _userManager.FindByEmailAsync(req.CommandModel.Email);
            var user = new User();
            if (identityUser is null)
            {
                isNull = true;
            }
            else
            {
                user.Email = identityUser.Email;
                user.RequestResetTime = DateTime.Now;
                _context.Add(user);
                await _context.SaveChangesAsync();
                _emailSender.SendEmail(_configuration.GetSection("Email").Value, user.Email, "Request Reset Password", "Reset Password");

            }
            return user;
        }
    }
}
