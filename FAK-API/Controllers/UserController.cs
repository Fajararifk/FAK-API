using FAK.Common.Enumerator;
using FAK.Domain.Entities;
using FAK.GLOBAL.App;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FAK_API.Controllers
{
    public class UserController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            command.Browser = HttpContext.Request.Headers["User-Agent"].ToString();
            User mLogin = await Mediator.Send(GetCommand<LoginCommand, User>(command));
            if (!string.IsNullOrEmpty(mLogin.UserName) && !string.IsNullOrEmpty(mLogin.Token) && !string.IsNullOrEmpty(mLogin.Password))
            {
                return Ok(new
                {
                    Token = mLogin.Token,
                    Message = "Login Successfully",
                });
            }
            else
            {
                return BadRequest(new 
                { 
                    errors = "User Not Found!" 
                });
            }
        }

        [HttpPost()]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterCommand command)
        {
            
            command.Browser = HttpContext.Request.Headers["User-Agent"].ToString();
            if (!IsValidEmail(command.Email))
            {
                return BadRequest(new
                {
                    errors = "Invalid email format"
                });
            }
            if (!IsValidPassword(command.Password))
            {
                return BadRequest(new
                {
                    errors = "Invalid password format. Password must contain at least one uppercase letter, one lowercase letter, and one digit."
                });
            }

            User mLogin = await Mediator.Send(GetCommand<RegisterCommand, User>(command));
            if (!string.IsNullOrEmpty(mLogin.UserName) && !string.IsNullOrEmpty(mLogin.Password) && mLogin.Password == mLogin.ConfirmPassword)
            {
                return Ok(new
                {
                    Message = "Register Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    errors = "Register Failed!"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            command.Browser = HttpContext.Request.Headers["User-Agent"].ToString();
            if (!IsValidEmail(command.Email))
            {
                return BadRequest(new
                {
                    errors = "Invalid email format"
                });
            }
            User mLogin = await Mediator.Send(GetCommand<ForgotPasswordCommand, User>(command));
            if (!string.IsNullOrEmpty(mLogin.Email))
            {
                return Ok(new
                {
                    Message = "Forgot Password Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    errors = "Forgot Password Failed!"
                });
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> RequestResetPassword([FromBody] RequestResetPasswordCommand command)
        {
            command.Browser = HttpContext.Request.Headers["User-Agent"].ToString();
            if (!IsValidEmail(command.Email))
            {
                return BadRequest(new
                {
                    errors = "Invalid email format"
                });
            }
            User mLogin = await Mediator.Send(GetCommand<RequestResetPasswordCommand, User>(command));
            if (!string.IsNullOrEmpty(mLogin.Email))
            {
                return Ok(new
                {
                    Message = "Request Reset Password Successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    errors = "Request Reset Password Failed!"
                });
            }

        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            // This regex pattern checks for a basic email format with @
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            // This regex pattern checks for at least one uppercase letter, one lowercase letter, one digit, and one symbol
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
            return passwordRegex.IsMatch(password);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand cmd)
        {
            cmd.Browser = HttpContext.Request.Headers["User-Agent"].ToString();
            User mLogin = await Mediator.Send(GetCommand<ChangePasswordCommand, User>(cmd));
            if (!string.IsNullOrEmpty(mLogin.Email))
            {
                return Ok(new
                {
                    Message = "Change Password Succesfully",
                    Status = 200
                });
            }
            else
            {
                return BadRequest(new
                {
                    Message = new List<string>() { mLogin.Email },
                    Status = 400
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand reset)
        {
            reset.Browser = HttpContext.Request.Headers["User-Agent"].ToString();
            if (reset.newPassword == reset.confirmNewPassword)
            {
                User mLogin = await Mediator.Send(GetCommand<ResetPasswordCommand, User>(reset));
                if (!string.IsNullOrEmpty(mLogin.Email))
                {
                    return Ok(new
                    {
                        Message = "Change Password Succesfully",
                        Status = 200
                    });
                }
                else
                {
                    return BadRequest( new
                    {
                        Message = new List<string>() { mLogin.Email },
                        Status = 400
                    });
                }
            }
            return BadRequest("Password Tidak Cocok");
            
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            return Ok(await Mediator.Send(GetCommand<LogoutCommand, StatusModel>(null)));
        }

        [HttpGet]
        public async Task<IActionResult> GetIdleSession()
        {
            ParamHeaderModel mdl = new ParamHeaderModel()
            {
                parName = "Idle Timeout",
                parValue = "10000",
            };//await Mediator.Send(GetQuery<GetSysParByCodeQuery, ParamHeaderModel>(new GetSysParByCodeQuery { code = "GB300" }));
            return Ok(mdl);
        }
    }
}
