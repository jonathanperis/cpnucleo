namespace Cpnucleo.Shared.Commands.CreateRecursoProjeto;

public sealed class CreateRecursoProjetoCommandValidator : AbstractValidator<CreateRecursoProjetoCommand>
{
    public CreateRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.IdRecurso)
            .NotEmpty()
            .WithMessage("Recurso Projeto deve conter um Recurso");

        RuleFor(x => x.IdProjeto)
            .NotEmpty()
            .WithMessage("Recurso Projeto deve conter um Projeto");
    }
}
