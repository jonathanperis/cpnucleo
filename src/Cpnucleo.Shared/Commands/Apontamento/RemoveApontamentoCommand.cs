using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Apontamento;

public record RemoveApontamentoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;