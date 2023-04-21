using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Services.Articles;

public class Details
{
    public class Query : IRequest<Result<ArticleDto>>
    {
        public Guid Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<ArticleDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperlyMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<Result<ArticleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var article = await _unitOfWork
                .GetRepository<Article>()
                .GetQueryable()
                .Include(x => x.Author)
                .Include(x => x.Rubric)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

            return Result<ArticleDto>.Success(_mapper.Map(article));
        }
    }
}