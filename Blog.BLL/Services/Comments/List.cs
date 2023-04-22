using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Services.Comments;

public class List
{
    public class Query : IRequest<Result<List<CommentDto>>>
    {
        public Guid CommentableId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<CommentDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperlyMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<CommentDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var comments = await _unitOfWork.GetRepository<Comment>()
                .GetQueryable()
                .Where(x => x.Parent.Id == request.CommentableId)
                .Include(x => x.Parent)
                .Include(x => x.Author)
                .Include(x => x.Comments)
                .ThenInclude(x => x.Comments)
                .ThenInclude(x => x.Comments)
                .ThenInclude(x => x.Comments)
                .ThenInclude(x => x.Comments)
                .ThenInclude(x => x.Comments)
                .ThenInclude(x => x.Comments)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync(cancellationToken);

            var commentsDto = comments.Select(_mapper.Map).ToList();

            return Result<List<CommentDto>>.Success(commentsDto);
        }
    }
}
