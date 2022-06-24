using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Projeto;

public record CreateProjetoCommand(Guid Id, string Nome, Guid IdSistema) : BaseCommand, IRequest<OperationResult>;