using Blog.BLL.Contracts;
using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Blog.BLL.Services.Users;

public class Login
{
    public class Command : IRequest<Result<UserDto>>
    {
        public LoginDto LoginDto { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<UserDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public Handler(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<UserDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.LoginDto.Email);

            if (user == null) return Result<UserDto>.Failure(string.Empty);

            var result = await _userManager.CheckPasswordAsync(user, request.LoginDto.Password);

            if (!result) return Result<UserDto>.Failure(string.Empty);
            
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