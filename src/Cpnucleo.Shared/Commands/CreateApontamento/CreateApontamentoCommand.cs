namespace Cpnucleo.Shared.Commands.CreateApontamento;

public sealed record CreateApontamentoCommand(string Descricao, DateTime DataApontamento, int QtdHoras, Guid IdTarefa, Guid IdRecurso) : BaseCommand, IRequest<OperationResult>;