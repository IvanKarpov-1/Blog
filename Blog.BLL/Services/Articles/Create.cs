using Blog.BLL.Contracts;
using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.Articles;

public class Create
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
        private readonly IUserAccessor _userAccessor;
        private readonly UserManager<User> _userManager;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper, IUserAccessor userAccessor,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _userManager = userManager;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = _userManager.Users.FirstOrDefault(x=> x.UserName == _userAccessor.GetUsername());

            var article = _mapper.Map(request.Article);

            article.Rubric = await _unitOfWork.GetRepository<Rubric>().GetByIdAsync(request.Article.RubricId);

            article.Author = user;

            _unitOfWork.GetRepository<Article>().Add(article);

            var result = await _unitOfWork.CommitAsync() > 0;

            if (!result) return Result<Unit>.Failure("Не вдалося створити статтю");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}