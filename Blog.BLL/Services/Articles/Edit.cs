using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentValidation;
using MediatR;

namespace Blog.BLL.Services.Articles;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public ArticleDto Article { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Article).SetValidator(new ArticleValidator());
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperlyMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetByIdAsync(request.Article.Id);

            if (article == null) return null;

            _mapper.Map(request.Article, article);

            var result = await _unitOfWork.CommitAsync() > 0;

            if (!result) return Result<Unit>.Failure("Не вдалося оновити статтю");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}