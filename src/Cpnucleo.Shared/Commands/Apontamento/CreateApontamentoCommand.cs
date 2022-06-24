using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Shared.Commands.Apontamento;

public record CreateApontamentoCommand(Guid Id, string Descricao, DateTime DataApontamento, int QtdHoras, Guid IdTarefa, Guid IdRecurso) : BaseCommand, IRequest<OperationResult>;