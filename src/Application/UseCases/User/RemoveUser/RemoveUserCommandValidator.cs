namespace Application.UseCases.User.RemoveUser;

public sealed class RemoveUserCommandValidator : AbstractValidator<RemoveUserCommand>
{
    public RemoveUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}
