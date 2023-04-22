using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Services.Rubrics;

public class List
{
    public class Query : IRequest<Result<List<RubricDto>>>
    {
        public Guid? ParentId { get; set; }
    }

    public class Handler : IRequestHandler<Query, Result<List<RubricDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperlyMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<RubricDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var queryable = _unitOfWork
                .GetRepository<Rubric>()
                .GetQueryable()
                .Include(x => x.Rubrics)
                .ThenInclude(x => x.Rubrics)
                .ThenInclude(x => x.Rubrics)
                .ThenInclude(x => x.Rubrics);

            List<Rubric> rubrics;

            if (request.ParentId == null)
            {
                rubrics = await queryable
                    .Where(x => x.Parent == null)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                rubrics = new List<Rubric>
                {
                    await queryable
                    .SingleOrDefaultAsync(x => x.Id == request.ParentId, cancellationToken)
                };
            }

            var rubricsDto = rubrics.Select(_mapper.Map).ToList();

            return Result<List<RubricDto>>.Success(rubricsDto);
        }
    }
}