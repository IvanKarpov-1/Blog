using Blog.BLL.ModelsDTOs;
using FluentValidation;

namespace Blog.BLL.Services.Articles;

public class ArticleValidator : AbstractValidator<ArticleDto>
{
    public ArticleValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.Content).NotEmpty();
        RuleFor(x => x.RubricId).NotEmpty();
    }
}