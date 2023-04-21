using Blog.BLL.Core;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;

namespace Blog.BLL.Services.Articles;

public class Delete
{
    public class Command : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetByIdAsync(request.Id);

            if (article == null) return null;

            _unitOfWork.GetRepository<Article>().Remove(article);

            var result = await _unitOfWork.CommitAsync() > 0;

            if (!result) return Result<Unit>.Failure("Не вдалося видалити статтю");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}