using AutoFixture;
using Blog.BLL.Core;
using Blog.BLL.Services.Profiles;
using Blog.DAL.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Blog.BLL.Tests.Profiles;

public class DetailsTests
{
    private readonly IFixture _fixture;
    private readonly UserManager<User> _userManager;
    private readonly MapperlyMapper _mapper;
    private readonly Details.Handler _handler;

    public DetailsTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _mapper = new MapperlyMapper();
        _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null,
            null, null, null, null);

        _handler = new Details.Handler(_userManager, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessWithProfileDto_ForSpecificUserName()
    {
        // Arrange
        var userName = _fixture.Create<string>();
        var query = _fixture.Build<Details.Query>()
            .With(x => x.UserName, userName)
            .Create();
        var user = _fixture.Build<User>()
            .With(x => x.Id, Guid.NewGuid().ToString)
            .With(x => x.UserName, userName)
            .Create();
        var profileDto = _mapper.Map(user);

        _userManager.FindByNameAsync(userName).Returns(user);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(profileDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenUserIsNotFound()
    {
        // Arrange
        var query = _fixture.Create<Details.Query>();

        _userManager.FindByNameAsync(Arg.Any<string>()).Returns(_ => Task.FromResult<User?>(null));

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}