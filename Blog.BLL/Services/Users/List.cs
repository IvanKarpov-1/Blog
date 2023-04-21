using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;

namespace Blog.BLL.Services.Users;

public class List
{
    public class Query : IRequest<List<ProfileDto>> { }

    public class Handler : IRequestHandler<Query, List<ProfileDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperlyMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProfileDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.GetRepository<User>().GetAllAsync();
            return users.Select(_mapper.Map).ToList();
        }
    }
}