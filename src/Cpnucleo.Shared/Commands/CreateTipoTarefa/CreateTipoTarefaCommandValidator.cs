namespace Cpnucleo.Shared.Commands.CreateTipoTarefa;

public sealed class CreateTipoTarefaCommandValidator : AbstractValidator<CreateTipoTarefaCommand>
{
    public CreateTipoTarefaCommandValidator()
    {
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.Image).MaximumLength(50);
    }
}
