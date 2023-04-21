using Blog.BLL.ModelsDTOs;
using Blog.BLL.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    public class AccountController : BaseApiController
    {
        [AllowAnonymous]
        [HttpPost("login")] // api/account/login
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            return HandleResult(await Mediator.Send(new Login.Command { LoginDto = loginDto}));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            return HandleResult(await Mediator.Send(new Register.Command { RegisterDto = registerDto }));
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            return HandleResult(await Mediator.Send(new Details.Query {User = User}));
        }
    }
}
