using Cpnucleo.Shared.Commands.RecursoProjeto;

namespace Cpnucleo.Application.Commands.RecursoProjeto;

public class UpdateRecursoProjetoCommandValidator : AbstractValidator<UpdateRecursoProjetoCommand>
{
    public UpdateRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdRecurso).NotEmpty();
        RuleFor(x => x.IdProjeto).NotEmpty();
    }
}
