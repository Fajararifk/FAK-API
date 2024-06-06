using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System;
using MediatR;
using FAK.Common;
using FAK.Infrastructure;
using FAK.Common.Model;
using System.Diagnostics.CodeAnalysis;

namespace FAK_API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    public abstract class BaseController : Controller
    {
        private IMediator _mediator;
        protected IDateTime _currDate = new MachineDateTime();
        protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

        protected CommandsModel<TReq, TRes> GetCommand<TReq, TRes>(TReq command) where TReq : class
            where TRes : class
        {
            CommandsModel<TReq, TRes> m = new CommandsModel<TReq, TRes>();
            if (command.ToString().Contains("Register"))
            {
                m.UserIdentity = new UserIdentityModel
                {
                    UserId = "UserId",
                    GroupId = "GroupAccess",
                    Host = LoginHost,
                    BranchGroup = "BranchGroup",
                    Modules = "UserModule"
                };
            }
            else
            {
                m.UserIdentity = new UserIdentityModel
                {
                    UserId = LoginId,
                    GroupId = LoginRole,
                    Host = LoginHost,
                    BranchGroup = LoginBranchGroup,
                    Modules = LoginUserModule
                };
            }
            
            m.CurrentDateTime = _currDate.Now;
            m.CommandModel = command;

            return m;
        }

        protected QueriesModel<TReq, TRes> GetQuery<TReq, TRes>(TReq query) where TReq : class
            where TRes : class
        {
            QueriesModel<TReq, TRes> m = new QueriesModel<TReq, TRes>();
            m.UserIdentity = new UserIdentityModel
            {
                UserId = LoginId,
                GroupId = LoginRole,
                Host = LoginHost,
                BranchGroup = LoginBranchGroup,
                Modules = LoginUserModule
            };
            m.AccessMatrix = this.AccessMatrix;
            m.CurrentDateTime = _currDate.Now;
            m.QueryModel = query;
            return m;
        }

        protected AccessMatrixModel AccessMatrix
        {
            get { return (AccessMatrixModel)HttpContext.Items["screeninfo"]; }
        }

        protected IActionResult UnAuthorizedRequest()
        {
            HttpContext.Response.ContentType = "application/json";
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return new JsonResult(new
            {
                errors = "Unauthorized to Do This Action"
            }
            );
        }

        protected IActionResult BadRequestRequest(List<string> errors)
        {
            HttpContext.Response.ContentType = "application/json";
            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return new JsonResult(new
            {
                errors = errors,
                status = HttpStatusCode.BadRequest
            }
            );
        }

        protected string LoginId
        {
            get
            {
                //return "admin";
                Claim authInfo = ((ClaimsIdentity)User.Identity).Claims.Where(x => x.Type == "UserId").FirstOrDefault();
                if (authInfo != null)
                {
                    return authInfo.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected string LoginRole
        {
            get
            {
                Claim authInfo = ((ClaimsIdentity)User.Identity).Claims.Where(x => x.Type == "GroupAccess").FirstOrDefault();
                if (authInfo != null)
                {
                    return authInfo.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        protected string LoginUserModule
        {
            get
            {
                Claim authInfo = ((ClaimsIdentity)User.Identity).Claims.Where(x => x.Type == "UserModule").FirstOrDefault();
                if (authInfo != null)
                {
                    return authInfo.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected string LoginBranchGroup
        {
            get
            {
                Claim authInfo = ((ClaimsIdentity)User.Identity).Claims.Where(x => x.Type == "BranchGroup").FirstOrDefault();
                if (authInfo != null)
                {
                    return authInfo.Value;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        protected string LoginHost
        {
            get
            {
                string str = HttpContext.Connection.RemoteIpAddress.ToString();

                return str;
            }
        }

        [Route("api/[controller]")]
        [ApiController]
        public abstract class BaseApiController : Controller
        {
            [ExcludeFromCodeCoverage]
            [HttpGet("GetUserId")]
            public string GetUserId()
            {
                var CurrentUserId = "N/A|0";
                try
                {
                    CurrentUserId = User.Claims.FirstOrDefault(x => x.Type == "userId").Value;
                }
                catch (Exception)
                {

                }
                return CurrentUserId;
            }

            [ExcludeFromCodeCoverage]
            [HttpGet("GetRoleId")]
            public string GetRoleId()
            {
                var CurrentRoleId = "N/A|0";
                try
                {
                    CurrentRoleId = User.Claims.FirstOrDefault(x => x.Type == "Role").Value;
                }
                catch (Exception)
                {

                }
                return CurrentRoleId;
            }

            [ExcludeFromCodeCoverage]
            protected virtual bool ValidateCreate<T>(T entity)
            {
                return true;
            }

            [ExcludeFromCodeCoverage]
            protected virtual bool ValidateUpdate<T>(T entity)
            {
                return true;
            }

            [ExcludeFromCodeCoverage]
            protected virtual (bool Result, Int64 Id) ValidateReenable<T>(T entity)
            {
                return (true, 0);
            }

            [ExcludeFromCodeCoverage]
            protected virtual bool ValidateDelete(long id)
            {
                return true;
            }

            [ExcludeFromCodeCoverage]
            protected virtual bool ValidateDelete(string id)
            {
                return true;
            }
        }


    }
}
