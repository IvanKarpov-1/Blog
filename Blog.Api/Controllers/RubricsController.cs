using Blog.BLL.Services.Rubrics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

public class RubricsController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{parentId:guid?}")]
    public async Task<IActionResult> GetRubrics(Guid? parentId = null)
    {
        return HandleResult(await Mediator.Send(new List.Query { ParentId = parentId }));
    }
}   