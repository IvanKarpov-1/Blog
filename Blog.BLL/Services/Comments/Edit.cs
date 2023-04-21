using Blog.BLL.Core;
using Blog.BLL.ModelsDTOs;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentValidation;
using MediatR;

namespace Blog.BLL.Services.Comments;

public class Edit
{
    public class Command : IRequest<Result<CommentDto>>
    {
        public CommentDto Comment { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.Comment.Content).NotEmpty();
            RuleFor(x => x.Comment.Id).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<CommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MapperlyMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, MapperlyMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(request.Comment.Id);

            if (comment == null) return null;

            comment.Content = request.Comment.Content;

            var result = await _unitOfWork.CommitAsync() > 0;

            if (!result) return Result<CommentDto>.Failure("Не вдалося відредагувати коментар");

            return Result<CommentDto>.Success(_mapper.Map(comment));
        }
    }
}