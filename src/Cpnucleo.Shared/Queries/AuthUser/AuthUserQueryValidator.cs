namespace Cpnucleo.Shared.Queries.AuthUser;

public sealed class AuthUserQueryValidator : AbstractValidator<AuthUserQuery>
{
    public AuthUserQueryValidator()
    {
        RuleFor(x => x.Usuario).NotEmpty();
        RuleFor(x => x.Senha).NotEmpty();
    }
}
