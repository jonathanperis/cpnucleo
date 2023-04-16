namespace Cpnucleo.Shared.Commands.UpdateSistema;

public sealed class UpdateSistemaCommandValidator : AbstractValidator<UpdateSistemaCommand>
{
    public UpdateSistemaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Necessário informar o Id do Sistema");

        RuleFor(x => x.Nome)
            .NotEmpty()
            .WithMessage("Necessário informar a Nome do Sistema");

        RuleFor(x => x.Nome)
            .MaximumLength(50)
            .WithMessage("Nome pode conter no máximo 50 caractéres");

        RuleFor(x => x.Descricao)
            .NotEmpty()
            .WithMessage("Necessário informar a Descrição do Sistema");

        RuleFor(x => x.Descricao)
            .MaximumLength(450)
            .WithMessage("Descrição pode conter no máximo 450 caractéres");
    }
}
