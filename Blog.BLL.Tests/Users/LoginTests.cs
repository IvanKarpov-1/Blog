using AutoFixture;
using Blog.BLL.Contracts;
using Blog.BLL.ModelsDTOs;
using Blog.BLL.Services.Users;
using Blog.DAL.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Blog.BLL.Tests.Users;

public class LoginTests
{
    private readonly IFixture _fixture;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly Login.Handler _handler;

    public LoginTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null,
            null, null, null, null);
        _tokenService = Substitute.For<ITokenService>();

        _handler = new Login.Handler(_userManager, _tokenService);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessWithUserDto_WhenUserIsSuccessfullyLoggedIn()
    {
        // Arrange
        var command = _fixture.Create<Login.Command>();
        var user = _fixture.Create<User>();
        var token = _fixture.Create<string>();
        var userDto = _fixture.Build<UserDto>()
            .With(x => x.DisplayName, user.DisplayName)
            .With(x => x.Token, token)
            .With(x => x.UserName, user.UserName)
            .With(x => x.Image, () => null!)
            .Create();

        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(user);
        _userManager.CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(true);
        _tokenService.CreateToken(user).Returns(token);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(userDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenUserIsNotFound()
    {
        // Arrange
        var command = _fixture.Create<Login.Command>();

        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(_ => Task.FromResult<User?>(null));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(string.Empty);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPasswordIsWrong()
    {
        // Arrange
        var command = _fixture.Create<Login.Command>();
        var user = _fixture.Create<User>();

        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(user);
        _userManager.CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(string.Empty);
    }
}