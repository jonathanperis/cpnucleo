using Cpnucleo.Shared.Commands.RecursoProjeto;

namespace Cpnucleo.Application.Commands.RecursoProjeto;

public class CreateRecursoProjetoCommandValidator : AbstractValidator<CreateRecursoProjetoCommand>
{
    public CreateRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
        RuleFor(x => x.IdProjeto).NotEmpty();
    }
}
