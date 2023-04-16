namespace Cpnucleo.Shared.Commands.UpdateRecurso;

public sealed class UpdateRecursoCommandValidator : AbstractValidator<UpdateRecursoCommand>
{
    public UpdateRecursoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Recurso");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar o Nome do Recurso");

        RuleFor(x => x.Nome)
            .MaximumLength(50)
            .WithMessage("Nome pode conter no máximo 50 caractéres");

        RuleFor(x => x.Senha)
            .NotEmpty()
            .WithMessage("Necessário informar a Senha do Recurso");

        RuleFor(x => x.Senha)
            .MaximumLength(50)
            .WithMessage("Senha pode conter no máximo 50 caractéres");
    }
}
