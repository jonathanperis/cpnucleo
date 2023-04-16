namespace Cpnucleo.Shared.Commands.CreateProjeto;

public sealed class CreateProjetoCommandValidator : AbstractValidator<CreateProjetoCommand>
{
    public CreateProjetoCommandValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar o Nome do Projeto");

        RuleFor(x => x.Nome)
            .MaximumLength(50)
            .WithMessage("Nome pode conter no máximo 50 caractéres");

        RuleFor(x => x.IdSistema)
            .NotEmpty()
            .WithMessage("Projeto deve conter um Sistema");
    }
}
