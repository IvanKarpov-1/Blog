using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;

namespace Blog.BLL.Services.Users;

public class Delete
{
    public class Command : IRequest
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(request.Id);

            _unitOfWork.GetRepository<User>().Remove(user);

            await _unitOfWork.CommitAsync();
        }
    }
}