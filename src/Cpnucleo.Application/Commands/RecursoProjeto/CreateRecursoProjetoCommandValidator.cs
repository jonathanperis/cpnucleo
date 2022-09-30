namespace Cpnucleo.Application.Commands.RecursoProjeto;

public sealed class CreateRecursoProjetoCommandValidator : AbstractValidator<CreateRecursoProjetoCommand>
{
    public CreateRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.IdRecurso).NotEmpty();
        RuleFor(x => x.IdProjeto).NotEmpty();
    }
}
