namespace Cpnucleo.Shared.Commands.UpdateRecursoProjeto;

public sealed class UpdateRecursoProjetoCommandValidator : AbstractValidator<UpdateRecursoProjetoCommand>
{
    public UpdateRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.IdRecurso).NotEmpty();
        RuleFor(x => x.IdProjeto).NotEmpty();
    }
}
