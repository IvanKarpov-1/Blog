using AutoFixture;
using Blog.BLL.Contracts;
using Blog.BLL.ModelsDTOs;
using Blog.BLL.Services.Users;
using Blog.DAL.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Blog.BLL.Tests.Users;

public class RegisterTests
{
    private readonly IFixture _fixture;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly Register.Handler _handler;

    public RegisterTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null,
            null, null, null, null);
        _tokenService = Substitute.For<ITokenService>();

        _handler = new Register.Handler(_userManager, _tokenService);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessWithUserDto_WhenUserIsSuccessfullyRegistered()
    {
        // Arrange
        var registerDto = _fixture.Build<RegisterDto>()
            .With(x => x.Password, "Pa$w0rd")
            .Create();
        var command = _fixture.Build<Register.Command>()
            .With(x => x.RegisterDto, registerDto)
            .Create();
        var token = _fixture.Create<string>();
        var userDto = _fixture.Build<UserDto>()
            .With(x => x.DisplayName, registerDto.DisplayName)
            .With(x => x.Token, token)
            .With(x => x.UserName, registerDto.Username)
            .With(x => x.Image, () => null!)
            .Create();
        var user = _fixture.Create<User>();

        _userManager.Users.Returns(new[] { user }.AsAsyncQueryable());
        _userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(Task.FromResult(IdentityResult.Success));
        _tokenService.CreateToken(Arg.Any<User>()).Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(userDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserNameIsAlreadyTaken()
    {
        // Arrange
        var userName = _fixture.Create<string>();
        var registerDto = _fixture.Build<RegisterDto>()
            .With(x => x.Password, "Pa$w0rd")
            .With(x => x.Username, userName)
            .Create();
        var user = _fixture.Build<User>()
            .With(x => x.UserName, userName)
            .Create();
        var command = _fixture.Build<Register.Command>()
            .With(x => x.RegisterDto, registerDto)
            .Create();

        _userManager.Users.Returns(new[] { user }.AsAsyncQueryable());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Ім'я користувача вже зайняте");
        result.ValidationError.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmailIsAlreadyTaken()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var registerDto = _fixture.Build<RegisterDto>()
            .With(x => x.Password, "Pa$w0rd")
            .With(x => x.Email, email)
            .Create();
        var user = _fixture.Build<User>()
            .With(x => x.Email, email)
            .Create();
        var command = _fixture.Build<Register.Command>()
            .With(x => x.RegisterDto, registerDto)
            .Create();

        _userManager.Users.Returns(new[] { user }.AsAsyncQueryable());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Електронна пошта користувача вже зайнята");
        result.ValidationError.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCreatingUserIsUnsuccessful()
    {
        // Arrange
        var registerDto = _fixture.Build<RegisterDto>()
            .With(x => x.Password, "Pa$w0rd")
            .Create();
        var command = _fixture.Build<Register.Command>()
            .With(x => x.RegisterDto, registerDto)
            .Create();
        var user = _fixture.Create<User>();

        _userManager.Users.Returns(new[] { user }.AsAsyncQueryable());
        _userManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(Task.FromResult(IdentityResult.Failed()));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }
}