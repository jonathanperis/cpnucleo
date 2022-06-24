using Cpnucleo.Shared.Commands;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.RecursoProjeto;

public record UpdateRecursoProjetoCommand(Guid Id, Guid IdRecurso, Guid IdProjeto) : BaseCommand, IRequest<OperationResult>;