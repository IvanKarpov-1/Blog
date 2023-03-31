using Blog.BLL.Core;
using Blog.BLL.ModelsDTO;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;

namespace Blog.BLL.Services.Users;

public class Details
{
    public class Query : IRequest<UserDto>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperlyMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(request.Id);
            return _mapper.Map(user);
        }
    }
}