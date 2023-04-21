using Blog.DAL.Contracts;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Blog.Infrastructure.Security;

public class IsArticleAuthorRequirement : IAuthorizationRequirement
{
}

public class IsArticleAuthorRequirementHandler : AuthorizationHandler<IsArticleAuthorRequirement>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IsArticleAuthorRequirementHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsArticleAuthorRequirement requirement)
    {
        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return Task.CompletedTask;

        var articleId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues
            .SingleOrDefault(x => x.Key == "id").Value?.ToString()!);

        var article = _unitOfWork.GetRepository<Article>()
            .GetQueryable()
            .AsNoTracking()
            .Include(x => x.Author)
            .SingleOrDefaultAsync(x => x.Id == articleId)
            .Result;

        if (article?.Author != null && article.Author.Id == userId) context.Succeed(requirement);

        return Task.CompletedTask;
    }
}