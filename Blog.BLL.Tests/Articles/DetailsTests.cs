using AutoFixture;
using Blog.BLL.Core;
using Blog.BLL.Services.Articles;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using NSubstitute;

namespace Blog.BLL.Tests.Articles;

public class DetailsTests
{
    private readonly IFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly MapperlyMapper _mapper;
    private readonly Details.Handler _handler;

    public DetailsTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = new MapperlyMapper();

        _handler = new Details.Handler(_unitOfWork, _mapper);
    }

    [Fact]
    public async Task Handle_ShouldReturnArticle_WithSpecificId()
    {
        // Arrange
        var articleId = Guid.NewGuid();
        var query = _fixture.Build<Details.Query>()
            .With(x => x.Id, articleId)
            .Create();
        var article = _fixture.Build<Article>()
            .With(x => x.Id, articleId)
            .Without(x => x.Author)
            .Without(x => x.Comments)
            .Create();

        _unitOfWork.GetRepository<Article>().GetQueryable().Returns(new[] { article }.AsAsyncQueryable());

        var articleDto = _mapper.Map(article);
        
        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(articleDto);
    }

    [Fact]
    public async Task Handle_ShouldReturnNull_WhenArticleIsNotFound()
    {
        // Arrange
        var query = _fixture.Create<Details.Query>();
        var article = _fixture.Create<Article>();

        _unitOfWork.GetRepository<Article>().GetQueryable().Returns(new[] { article }.AsAsyncQueryable());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }
}