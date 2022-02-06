namespace Cpnucleo.Application.Commands.Sistema.CreateSistema;

public class CreateSistemaCommandValidator : AbstractValidator<CreateSistemaCommand>
{
    public CreateSistemaCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Nome)
            .NotEmpty();

        RuleFor(x => x.Descricao)
            .NotEmpty();
    }
}
