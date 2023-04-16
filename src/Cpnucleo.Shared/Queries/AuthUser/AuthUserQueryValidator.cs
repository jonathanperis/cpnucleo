namespace Cpnucleo.Shared.Queries.AuthUser;

public sealed class AuthUserQueryValidator : AbstractValidator<AuthUserQuery>
{
    public AuthUserQueryValidator()
    {
        RuleFor(x => x.Usuario)
            .NotEmpty()
            .WithMessage("Necessário informar o Usuário");

        RuleFor(x => x.Senha)
            .NotEmpty()
            .WithMessage("Necessário informar a Senha");
    }
}
