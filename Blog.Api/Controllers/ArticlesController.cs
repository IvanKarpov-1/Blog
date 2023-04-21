using Blog.BLL.ModelsDTOs;
using Blog.BLL.Services.Articles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    public class ArticlesController : BaseApiController
    {
        [AllowAnonymous]
        [HttpGet] // api/articles
        public async Task<IActionResult> GetArticles([FromQuery] ArticleParams param)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query { ArticleParams = param }));
        }

        [AllowAnonymous]
        [HttpGet("{id:guid}")] // api/articles/id
        public async Task<IActionResult> GetArticle(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        [HttpPost] // api/articles
        public async Task<IActionResult> CreateArticle(ArticleDto article)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Article = article }));
        }

        [Authorize(Policy = "IsArticleAuthor")]
        [HttpPut("{id:Guid}")] // api/articles/id
        public async Task<IActionResult> EditArticle(Guid id, ArticleDto article)
        {
            article.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Article = article }));
        }

        [Authorize(Policy = "IsArticleAuthor")]
        [HttpDelete("{id:guid}")] // api/articles/id
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }
    }
}
