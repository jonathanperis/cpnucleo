namespace Cpnucleo.Application.Commands.TipoTarefa;

public sealed class UpdateTipoTarefaCommandValidator : AbstractValidator<UpdateTipoTarefaCommand>
{
    public UpdateTipoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Nome).NotEmpty();
        RuleFor(x => x.Nome).MaximumLength(50);
        RuleFor(x => x.Image).NotEmpty();
        RuleFor(x => x.Image).MaximumLength(50);
    }
}
