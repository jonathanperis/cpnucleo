using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.ImpedimentoTarefa;

public record UpdateImpedimentoTarefaCommand(Guid Id, string Descricao, Guid IdTarefa, Guid IdImpedimento) : BaseCommand, IRequest<OperationResult>;