using FAK.Persistance;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        private readonly AppDbContext _context;
        public LoginCommandValidator(AppDbContext context)
        {
            _context = context;

            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");

            //RuleFor(x => x).Must(x => x.Equals(ValidatePassword(x.UserName, x.Password))).WithMessage("Invalid Username or Password");
        }
    }
}
