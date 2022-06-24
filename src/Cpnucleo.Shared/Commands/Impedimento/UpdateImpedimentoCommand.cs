using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Impedimento;

public record UpdateImpedimentoCommand(Guid Id, string Nome) : BaseCommand, IRequest<OperationResult>;