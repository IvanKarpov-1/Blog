using Blog.BLL.Core;
using Blog.BLL.ModelsDTO;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;

namespace Blog.BLL.Services.Users;

public class Create
{
    public class Command : IRequest
    {
        public UserDto User { get; set; }
    }

    public class Handler : IRequestHandler<Command>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperlyMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            _unitOfWork.GetRepository<User>().Add(_mapper.Map(request.User));

            await _unitOfWork.CommitAsync();
        }
    }
}