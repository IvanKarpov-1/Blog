using Blog.BLL.ModelsDTOs;
using Blog.BLL.Services.Comments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Blog.API.SignalR;

public class CommentHub : Hub
{
    private readonly IMediator _mediator;

    public CommentHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    public async Task SendComment(Create.Command command, Guid articleId)
    {
        var comment = await _mediator.Send(command);

        await Clients.Group(articleId.ToString())
            .SendAsync("ReceiveCreatedComment", comment.Value);
    }

    [Authorize(Policy = "IsCommentAuthor")]
    public async Task EditComment(CommentDto commentDto, Guid articleId)
    {
        var comment = await _mediator.Send(new Edit.Command { Comment = commentDto });

        await Clients.Group(articleId.ToString())
            .SendAsync("ReceiveUpdatedComment", comment.Value);
    }

    [Authorize(Policy = "IsCommentAuthor")]
    public async Task DeleteComment(CommentDto commentDto, Guid articleId)
    {
        var id = await _mediator.Send(new Delete.Command { Id = commentDto.Id });

        await Clients.Group(articleId.ToString())
            .SendAsync("ReceiveDeletedCommentId", id.Value);
    }

    public override async Task OnConnectedAsync()
    {
        var httpContext = Context.GetHttpContext();
        if (httpContext != null)
        {
            var commentableId = httpContext.Request.Query["commentableId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, commentableId);
            var result = await _mediator.Send(new List.Query { CommentableId = Guid.Parse(commentableId) });
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}