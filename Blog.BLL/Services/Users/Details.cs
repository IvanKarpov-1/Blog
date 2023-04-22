using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Blog.BLL.Contracts;
using Blog.DAL.Models;

namespace Blog.BLL.Services.Users;

public class Details
{
    public class Query : IRequest<Result<UserDto>>
    {
        public ClaimsPrincipal User { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<UserDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public Handler(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<UserDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.User.FindFirstValue(ClaimTypes.Email)!);

            return Result<UserDto>.Success(new UserDto
            {
                DisplayName = user.DisplayName,
                Image = null,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName,
            });
        }
    }
}