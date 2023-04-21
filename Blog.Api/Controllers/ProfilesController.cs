using Blog.BLL.Services.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

public class ProfilesController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{username}")]
    public async Task<IActionResult> GetProfile(string username)
    {
        return HandleResult(await Mediator.Send(new Details.Query { UserName = username }));
    }

    [HttpPut]
    public async Task<IActionResult> EditProfile(Edit.Command command)
    {
        return HandleResult(await Mediator.Send(command));
    }
}