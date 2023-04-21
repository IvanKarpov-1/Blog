using Blog.BLL.Core;

namespace Blog.BLL.Services.Articles;

public class ArticleParams : PagingParams
{
    public bool IsAuthor { get; set; }

    public Guid RubricId { get; set; }
}