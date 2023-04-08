using Cpnucleo.Shared.Commands.UpdateImpedimentoTarefa;

namespace Cpnucleo.Application.Commands.UpdateImpedimentoTarefa;

public sealed class UpdateImpedimentoTarefaCommandValidator : AbstractValidator<UpdateImpedimentoTarefaCommand>
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
