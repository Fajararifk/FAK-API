using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FAK_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : BaseController
    {
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public Task<bool> GetBool()
        {
            return Task.FromResult(true);
        }
    }
}
