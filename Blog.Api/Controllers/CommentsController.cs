using Blog.BLL.Services.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers;

public class CommentsController : BaseApiController
{
    [AllowAnonymous]
    [HttpGet("{commentableId:guid}")]
    public async Task<IActionResult> GetComments(Guid commentableId)
    {
        return HandleResult(await Mediator.Send(new List.Query { CommentableId = commentableId }));
    }
}