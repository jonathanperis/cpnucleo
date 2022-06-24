using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Sistema;

public record CreateSistemaCommand(Guid Id, string Nome, string Descricao) : BaseCommand, IRequest<OperationResult>;