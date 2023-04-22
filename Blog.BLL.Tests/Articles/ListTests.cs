using AutoFixture;
using Blog.BLL.Contracts;
using Blog.BLL.Core;
using Blog.BLL.Services.Articles;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentAssertions;
using NSubstitute;

namespace Blog.BLL.Tests.Articles;

public class ListTests
{
    private readonly IFixture _fixture;
    private readonly IUnitOfWork _unitOfWork;
    private readonly MapperlyMapper _mapper;
    private readonly IUserAccessor _userAccessor;
    private readonly List.Handler _handler;

    public ListTests()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = new MapperlyMapper();
        _userAccessor = Substitute.For<IUserAccessor>();

        _handler = new List.Handler(_unitOfWork, _mapper, _userAccessor);
    }

    [Theory]
    [MemberData(nameof(GetParameters))]
    public async Task Handle_ShouldReturnSuccessResultWithListOfArticles_111(Guid rubricId, bool isAuthor, Guid authorId, string authorUserName)
    {
        // Arrange
        var query = Substitute.For<List.Query>();
        var articleParams = _fixture.Build<ArticleParams>()
            .With(x => x.PageSize, 1)
            .With(x => x.PageNumber, 1)
            .With(x => x.RubricId, rubricId)
            .With(x => x.IsAuthor, isAuthor)
            .Create();
        query.ArticleParams = articleParams;

        var article = _fixture.Build<Article>()
            .With(x => x.Author, _fixture.Build<User>()
                .With(x => x.Id, authorId.ToString)
                .With(x => x.UserName, authorUserName)
                .Create())
            .With(x => x.Rubric, _fixture.Build<Rubric>()
                .With(x => x.Id, rubricId)
                .Create())
            .Without(x => x.Comments)
            .Create();
        var articles = Substitute.For<PagedList<Article>>(new[] { article }, 1, articleParams.PageNumber, articleParams.PageSize);

        _unitOfWork.GetRepository<Article>().GetQueryable().Returns(new[] { article }.AsAsyncQueryable());

        var articleDto = _mapper.Map(article);
        var articlesDto = articles.Map(_ => articleDto);

        _userAccessor.GetUsername().Returns(authorUserName);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(articlesDto);
    }

    public static IEnumerable<object[]> GetParameters()
    {
        yield return new object[] { Guid.Empty, false, Guid.Empty, string.Empty};
        yield return new object[] { Guid.NewGuid(), false, Guid.Empty, string.Empty};
        yield return new object[] { Guid.Empty, true, Guid.NewGuid(), "UserName"};
        yield return new object[] { Guid.NewGuid(), true, Guid.NewGuid(), "UserName"};
    }
}