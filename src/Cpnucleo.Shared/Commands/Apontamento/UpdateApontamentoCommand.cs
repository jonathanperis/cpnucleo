namespace Cpnucleo.Shared.Commands.Apontamento;

public record UpdateApontamentoCommand(Guid Id, string Descricao, DateTime DataApontamento, int QtdHoras, Guid IdTarefa, Guid IdRecurso) : BaseCommand, IRequest<OperationResult>;