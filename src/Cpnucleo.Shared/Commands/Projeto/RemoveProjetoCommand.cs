using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Projeto;

public record RemoveProjetoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;