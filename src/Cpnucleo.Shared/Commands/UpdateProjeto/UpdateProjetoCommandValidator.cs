namespace Cpnucleo.Shared.Commands.UpdateProjeto;

public sealed class UpdateProjetoCommandValidator : AbstractValidator<UpdateProjetoCommand>
{
    public UpdateProjetoCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Projeto");

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
