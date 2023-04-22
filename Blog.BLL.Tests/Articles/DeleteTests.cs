using AutoFixture;
using Blog.BLL.Services.Articles;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace Blog.BLL.Tests.Articles;

public class DeleteTests
{
    private readonly IFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Delete.Handler _handler;

    public DeleteTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new Delete.Handler(_unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldDeleteArticle_WithSpecificId()
    {
        // Arrange
        var command = _fixture.Create<Delete.Command>();
        var article = _fixture.Create<Article>();

        _unitOfWork.GetRepository<Article>().GetByIdAsync(Arg.Any<Guid>()).Returns(article);
        _unitOfWork.CommitAsync().Returns(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Received().CommitAsync();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(Unit.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenArticleIsNotFound()
    {
        // Arrange
        var command = _fixture.Create<Delete.Command>();

        _unitOfWork.GetRepository<Article>().GetByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<Article>(null!));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenFailToDeleteArticle()
    {
        // Arrange
        var command = _fixture.Create<Delete.Command>();
        var article = _fixture.Create<Article>();

        _unitOfWork.GetRepository<Article>().GetByIdAsync(Arg.Any<Guid>()).Returns(article);
        _unitOfWork.CommitAsync().Returns(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        await _unitOfWork.Received().CommitAsync();

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo("Не вдалося видалити статтю");
    }
}