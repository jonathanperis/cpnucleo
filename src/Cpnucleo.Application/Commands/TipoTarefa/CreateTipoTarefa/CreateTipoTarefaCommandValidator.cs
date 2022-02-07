namespace Cpnucleo.Application.Commands.TipoTarefa.CreateTipoTarefa;

public class CreateTipoTarefaCommandValidator : AbstractValidator<CreateTipoTarefaCommand>
{
    public CreateTipoTarefaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.Image).MaximumLength(50);
    }
}
