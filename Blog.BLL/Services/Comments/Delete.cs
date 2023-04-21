using Blog.BLL.Core;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;

namespace Blog.BLL.Services.Comments;

public class Delete
{
    public class Command : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(request.Id);

            if (comment == null) return null;

            _unitOfWork.GetRepository<Comment>().Remove(comment);

            var result = await _unitOfWork.CommitAsync() > 0;

            if (!result) return Result<Guid>.Failure("Не вдалося видалити коментар");

            return Result<Guid>.Success(request.Id);
        }
    }
}