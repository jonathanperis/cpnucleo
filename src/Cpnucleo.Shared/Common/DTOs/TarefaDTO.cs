namespace Cpnucleo.Shared.Common.Dtos;

public sealed record TarefaDto(string? Nome,
                               DateTime DataInicio,
                               DateTime DataTermino,
                               int QtdHoras,
                               string? Detalhe,
                               int HorasConsumidas,
                               int HorasRestantes,
                               Guid IdProjeto,
                               Guid IdWorkflow,
                               Guid IdRecurso,
                               Guid IdTipoTarefa,
                               ProjetoDto? Projeto,
                               WorkflowDto? Workflow,
                               RecursoDto? Recurso,
                               TipoTarefaDto? TipoTarefa) : BaseDto;