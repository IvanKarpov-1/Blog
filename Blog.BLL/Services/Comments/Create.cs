using Blog.BLL.Contracts;
using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Services.Comments;

public class Create
{
    public class Command : IRequest<Result<CommentDto>>
    {
        public string Content { get; set; }
        public Guid ParentId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Content).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<CommentDto>>
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

        public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var commentable = await _unitOfWork.GetRepository<DAL.Models.Commentable>()
                .GetByIdAsync(request.ParentId);

            if (commentable == null) return null;

            var user = await _unitOfWork.GetRepository<User>()
                .GetQueryable()
                .SingleOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

            var comment = new Comment
            {
                Author = user,
                Content = request.Content,
                Parent = commentable
            };

            commentable.Comments.Add(comment);

            var result = await _unitOfWork.CommitAsync() > 0;

            if (result) return Result<CommentDto>.Success(_mapper.Map(comment));

            return Result<CommentDto>.Failure("Не вдалося додати коментар");
        }
    }
}