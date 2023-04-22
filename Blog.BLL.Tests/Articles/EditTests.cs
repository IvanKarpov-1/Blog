using AutoFixture;
using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.BLL.Services.Articles;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace Blog.BLL.Tests.Articles;

public class EditTests
{
    private readonly IFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly MapperlyMapper _mapper;
    private readonly Edit.Handler _handler;

    public EditTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = new MapperlyMapper();

        _handler = new Edit.Handler(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldEditArticle_WhenNewArticleIdIsMatchingOldArticleId()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var editedArticle = _fixture.Build<ArticleDto>()
            .With(x => x.Id, articleId)
            .Without(x => x.Author)
            .Without(x => x.Comments)
            .Create(); 
        var oldArticle = _fixture.Build<Article>()
            .With(x => x.Id, articleId)
            .Without(x => x.Author)
            .Without(x => x.Comments)
            .Create();
        var command = _fixture.Build<Edit.Command>()
            .With(x => x.Article, editedArticle)
            .Create();

        _unitOfWork.GetRepository<Article>().GetByIdAsync(Arg.Any<Guid>()).Returns(oldArticle);
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
        var editedArticle = _fixture.Create<ArticleDto>();
        var command = _fixture.Build<Edit.Command>()
            .With(x => x.Article, editedArticle)
            .Create();

        _unitOfWork.GetRepository<Article>().GetByIdAsync(Arg.Any<Guid>()).Returns(Task.FromResult<Article>(null!));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenTryingToUpdateArticleWithNoChanges()
    {
        // Arrange
        var article = _fixture.Build<Article>()
            .Without(x => x.Author)
            .Without(x => x.Comments)
            .Create();
        var unchangedArticle = _mapper.Map(article);
        var command = _fixture.Build<Edit.Command>()
            .With(x => x.Article, unchangedArticle)
            .Create();

        _unitOfWork.GetRepository<Article>().GetByIdAsync(Arg.Any<Guid>()).Returns(article);
        _unitOfWork.CommitAsync().Returns(0);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo("Не вдалося оновити статтю");
    }
}