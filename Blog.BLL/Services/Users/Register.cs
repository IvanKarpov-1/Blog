using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Services.Users;

public class Register
{
    public class Command : IRequest<Result<UserDto>>
    {
        public RegisterDto RegisterDto { get; set; }
    }

    public class Handler : IRequestHandler<Command, Result<UserDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;

        public Handler(UserManager<User> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<Result<UserDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == request.RegisterDto.Username))
            {
                return Result<UserDto>.Failure("Ім'я користувача вже зайняте").IsValidationError();
            }

            if (await _userManager.Users.AnyAsync(x => x.Email == request.RegisterDto.Email))
            {
                return Result<UserDto>.Failure("Електронна пошта користувача вже зайнята").IsValidationError();
            }

            var user = new User
            {
                DisplayName = request.RegisterDto.DisplayName,
                Email = request.RegisterDto.Email,
                UserName = request.RegisterDto.Username,
            };

            var result = await _userManager.CreateAsync(user, request.RegisterDto.Password);

            if (result.Succeeded)
            {
                return Result<UserDto>.Success(new UserDto
                {
                    DisplayName = user.DisplayName,
                    Image = null,
                    Token = _tokenService.CreateToken(user),
                    UserName = user.UserName,
                });
            }

            return Result<UserDto>.Failure(result.Errors.ToString());
        }
    }
}