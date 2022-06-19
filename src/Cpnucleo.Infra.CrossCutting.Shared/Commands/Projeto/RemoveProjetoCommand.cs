namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Projeto;

public record RemoveProjetoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;