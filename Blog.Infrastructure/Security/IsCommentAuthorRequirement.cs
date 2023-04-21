using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog.Infrastructure.Security;

public class IsCommentAuthorRequirement : IAuthorizationRequirement
{
}

public class IsCommentAuthorRequirementHandler : AuthorizationHandler<IsCommentAuthorRequirement>
{
    private readonly IUnitOfWork _unitOfWork;

    public IsCommentAuthorRequirementHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCommentAuthorRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return Task.CompletedTask;

        if ((context.Resource as HubInvocationContext)?.HubMethodArguments.SingleOrDefault(x =>
                x != null && x.GetType() == typeof(CommentDto)) is not CommentDto commentDto) return Task.CompletedTask;

        var comment = _unitOfWork.GetRepository<Comment>()
            .GetQueryable()
            .AsNoTracking()
            .Include(x => x.Author)
            .SingleOrDefaultAsync(x => x.Id == commentDto.Id)
            .Result;

        if (comment?.Author != null && comment.Author.Id == userId) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}