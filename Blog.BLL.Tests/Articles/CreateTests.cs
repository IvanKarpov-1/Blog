using AutoFixture;
using Blog.BLL.Contracts;
using Blog.BLL.Core;
using Blog.BLL.Services.Articles;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace Blog.BLL.Tests.Articles;

public class CreateTests
{
    private readonly IFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserAccessor _userAccessor;
    private readonly UserManager<User> _userManager;
    private readonly Create.Handler _handler;

    public CreateTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWork = Substitute.For<IUnitOfWork>();
        var mapper = new MapperlyMapper();
        _userAccessor = Substitute.For<IUserAccessor>();
        _userManager = Substitute.For<UserManager<User>>(Substitute.For<IUserStore<User>>(), null, null, null, null,
            null, null, null, null);

        _handler = new Create.Handler(_unitOfWork, mapper, _userAccessor, _userManager);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenCreateArticle()
    {
        // Arrange
        var command = _fixture.Create<Create.Command>();
        var user = _fixture.Create<User>();
        var rubric = _fixture.Create<Rubric>();

        _unitOfWork.GetRepository<Rubric>().GetByIdAsync(Arg.Any<Guid>()).Returns(rubric);
        _userManager.Users.Returns(new[] { user }.AsQueryable());
        _userAccessor.GetUsername().Returns(user.UserName);
        _unitOfWork.CommitAsync().Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _unitOfWork.GetRepository<Article>().Received(1).Add(Arg.Any<Article>());

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(Unit.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFailToCreateArticle()
    {
        // Arrange
        var command = _fixture.Create<Create.Command>();

        _unitOfWork.CommitAsync().Returns(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Received().CommitAsync();

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo("Не вдалося створити статтю");
    }
}