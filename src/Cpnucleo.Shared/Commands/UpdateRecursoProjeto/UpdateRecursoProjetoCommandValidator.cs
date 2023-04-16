namespace Cpnucleo.Shared.Commands.UpdateRecursoProjeto;

public sealed class UpdateRecursoProjetoCommandValidator : AbstractValidator<UpdateRecursoProjetoCommand>
{
    public UpdateRecursoProjetoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Recurso Projeto");

        RuleFor(x => x.IdRecurso)
            .NotEmpty()
            .WithMessage("Recurso Projeto deve conter um Recurso");

        RuleFor(x => x.IdProjeto)
            .NotEmpty()
            .WithMessage("Recurso Projeto deve conter um Projeto");
    }
}
