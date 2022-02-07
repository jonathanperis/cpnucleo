namespace Cpnucleo.Application.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;

public class UpdateImpedimentoTarefaCommandValidator : AbstractValidator<UpdateImpedimentoTarefaCommand>
{
    public UpdateImpedimentoTarefaCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Descricao).NotEmpty();
        RuleFor(x => x.Descricao).MaximumLength(450);
        RuleFor(x => x.IdTarefa).NotEmpty();
        RuleFor(x => x.IdImpedimento).NotEmpty();
    }
}
