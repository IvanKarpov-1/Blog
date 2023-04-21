using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.Profiles;

public class Details
{
    public class Query : IRequest<Result<ProfileDto>>
    {
        public string UserName { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ProfileDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly MapperlyMapper _mapper;

        public Handler(UserManager<User> userManager, MapperlyMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result<ProfileDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null) return null;
;
            return Result<ProfileDto>.Success(_mapper.Map(user));
        }
    }
}