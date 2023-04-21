using Blog.BLL.Contracts;
using Blog.BLL.Core;
using Blog.DAL.Contracts;
using Blog.DAL.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blog.BLL.Services.Profiles;

public class Edit
{
    public class Command : IRequest<Result<Unit>>
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }

    public class Handler : IRequestHandler<Command, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserAccessor _userAccessor;

        public Handler(IUnitOfWork unitOfWork, IUserAccessor userAccessor)
        {
            _unitOfWork = unitOfWork;
            _userAccessor = userAccessor;
        }

        public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.GetRepository<User>()
                .GetQueryable()
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(),
                    cancellationToken);

            if (user == null) return null;

            user.DisplayName = request.DisplayName ?? user.DisplayName;
            user.Bio = request.Bio ?? user.Bio;

            var result = await _unitOfWork.CommitAsync() > 0;

            if (!result) return Result<Unit>.Failure("Не вдалося оновити профіль користувача");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}