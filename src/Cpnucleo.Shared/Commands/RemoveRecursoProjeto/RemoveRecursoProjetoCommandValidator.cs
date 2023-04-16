namespace Cpnucleo.Shared.Commands.RemoveRecursoProjeto;

public sealed class RemoveRecursoProjetoCommandValidator : AbstractValidator<RemoveRecursoProjetoCommand>
{
    public RemoveRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Recurso Projeto");
    }
}
