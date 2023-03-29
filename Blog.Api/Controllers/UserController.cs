using Blog.BLL.Services;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet] // api/user
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            return await _userService.GetAsync();
        }

        [HttpGet("{id}")] // api/user/id
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            return await _userService.GetById(id);
        }
    }
}
