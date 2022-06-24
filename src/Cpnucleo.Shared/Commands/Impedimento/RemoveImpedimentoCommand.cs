namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Impedimento;

public record RemoveImpedimentoCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;