using Blog.BLL.ModelsDTO;
using Blog.BLL.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    public class UsersController : BaseApiController
    {
        [HttpGet] // api/users
        public async Task<ActionResult<List<UserDto>>> GetUsers()
        {
            return await Mediator.Send(new List.Query());
        }

        [HttpGet("{id:guid}")] // api/users/id
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            return await Mediator.Send(new Details.Query { Id = id });
        }

        [HttpPost] // api/users
        public async Task<IActionResult> CreateUser(UserDto user)
        {
            await Mediator.Send(new Create.Command { User = user });
            return Ok();
        }

        [HttpPut("{id:Guid}")] // api/users/id
        public async Task<IActionResult> EditUser(Guid id, UserDto user)
        {
            user.Id = id;
            await Mediator.Send(new Edit.Command { User = user });
            return Ok();
        }

        [HttpDelete("{id:guid}")] // app/users/id
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await Mediator.Send(new Delete.Command { Id = id });
            return Ok();
        }
    }
}
