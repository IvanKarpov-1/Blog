using AutoFixture;
using Blog.BLL.Contracts;
using Blog.BLL.ModelsDTOs;
using Blog.BLL.Services.Users;
using Blog.DAL.Models;
using Microsoft.AspNetCore.Identity;
using FluentAssertions;
using NSubstitute;

namespace Blog.BLL.Tests.Users;

public class DetailsTests
{
    private readonly IFixture _fixture;
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly Details.Handler _handler;

    public DetailsTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null,
            null, null, null, null);
        _tokenService = Substitute.For<ITokenService>();

        _handler = new Details.Handler(_userManager, _tokenService);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessWithUserDto()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var query = _fixture.Create<Details.Query>();
        var token = _fixture.Create<string>();
        var userDto = _fixture.Build<UserDto>()
            .With(x => x.DisplayName, user.DisplayName)
            .With(x => x.Image, () => null!)
            .With(x => x.UserName, user.UserName)
            .With(x => x.Token, token)
            .Create();

        _userManager.FindByEmailAsync(Arg.Any<string>()).Returns(user);
        _tokenService.CreateToken(user).Returns(token);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(userDto);
    }
}