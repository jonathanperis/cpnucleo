namespace Cpnucleo.Shared.Commands.Apontamento;

public sealed record CreateApontamentoCommand(Guid Id, string Descricao, DateTime DataApontamento, int QtdHoras, Guid IdTarefa, Guid IdRecurso) : BaseCommand, IRequest<OperationResult>;