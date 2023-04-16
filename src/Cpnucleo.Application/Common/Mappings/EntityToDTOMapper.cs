namespace Cpnucleo.Application.Common.Mappings;

[Mapper]
internal static partial class EntityToDtoMapper
{
    public static partial ApontamentoDto MapToDto(this Apontamento item);
    public static partial ImpedimentoDto MapToDto(this Impedimento item);
    public static partial ImpedimentoTarefaDto MapToDto(this ImpedimentoTarefa item);
    public static partial ProjetoDto MapToDto(this Projeto item);
    
    [MapperIgnoreSource(nameof(Recurso.Senha))]
    public static partial RecursoDto MapToDto(this Recurso item);
    
    public static partial RecursoProjetoDto MapToDto(this RecursoProjeto item);
    public static partial RecursoTarefaDto MapToDto(this RecursoTarefa item);
    public static partial SistemaDto MapToDto(this Sistema item);
    public static partial TarefaDto MapToDto(this Tarefa item);
    public static partial TipoTarefaDto MapToDto(this TipoTarefa item);
    public static partial WorkflowDto MapToDto(this Workflow item);
}