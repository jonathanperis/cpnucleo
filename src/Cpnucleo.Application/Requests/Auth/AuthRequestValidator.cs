using Cpnucleo.Shared.Requests.Auth;

namespace Cpnucleo.Application.Requests.Auth;

public sealed class GetAuthQueryValidator : AbstractValidator<AuthRequest>
{
    public GetAuthQueryValidator()
    {
        RuleFor(x => x.Usuario).NotEmpty();
        RuleFor(x => x.Senha).NotEmpty();
    }
}
