using Cpnucleo.Shared.Queries.AuthUser;

namespace Cpnucleo.Application.Queries.AuthUser;

public sealed class AuthUserQueryValidator : AbstractValidator<AuthUserQuery>
{
    public AuthUserQueryValidator()
    {
        RuleFor(x => x.Usuario).NotEmpty();
        RuleFor(x => x.Senha).NotEmpty();
    }
}
