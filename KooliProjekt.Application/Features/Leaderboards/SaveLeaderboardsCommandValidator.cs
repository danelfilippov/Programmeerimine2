using FluentValidation;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Features.Leaderboards
{
    public class SaveLeaderboardsCommandValidator : AbstractValidator<SaveLeaderboardsCommand>
    {
        public SaveLeaderboardsCommandValidator(ApplicationDbContext context)
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(50).WithMessage("Title cannot exceed 50 characters")
                .Custom((s, context) =>
                {
                    var command = context.InstanceToValidate;
                });
        }
    }
}