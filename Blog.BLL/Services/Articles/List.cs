using Blog.BLL.Contracts;
using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Services.Articles;

public class List
{
    public class Query : IRequest<Result<PagedList<ArticleDto>>>
    {
        public ArticleParams ArticleParams { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<PagedList<ArticleDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperlyMapper _mapper;
        private readonly IUserAccessor _userAccessor;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper, IUserAccessor userAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<Result<PagedList<ArticleDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork
                .GetRepository<Article>()
                .GetQueryable()
                .OrderBy(d => d.Title)
                .Include(x => x.Author)
                .Include(x => x.Rubric)
                .AsQueryable();

            if (request.ArticleParams.RubricId != Guid.Empty)
            {
                query = query.Where(x => x.Rubric.Id == request.ArticleParams.RubricId);
            }

            if (request.ArticleParams.IsAuthor)
            {
                query = query.Where(x => x.Author.UserName == _userAccessor.GetUsername());
            }

            var articles = await PagedList<Article>.CreateAsync(query, request.ArticleParams.PageNumber,
                request.ArticleParams.PageSize);

            var articlesDto = articles.Map(_mapper.Map);

            return Result<PagedList<ArticleDto>>.Success(articlesDto);
        }
    }
}