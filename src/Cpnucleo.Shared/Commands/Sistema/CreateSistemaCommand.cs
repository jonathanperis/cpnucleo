namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Sistema;

public record CreateSistemaCommand(Guid Id, string Nome, string Descricao) : BaseCommand, IRequest<OperationResult>;