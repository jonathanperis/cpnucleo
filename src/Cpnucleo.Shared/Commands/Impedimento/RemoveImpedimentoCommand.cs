using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Impedimento;

public record RemoveImpedimentoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;