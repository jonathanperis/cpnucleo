namespace Cpnucleo.Application.Common.Mappings;

[Mapper]
internal static partial class EntityToDTOMapper
{
    public static partial ApontamentoDTO MapToDto(this Apontamento item);
    public static partial ImpedimentoDTO MapToDto(this Impedimento item);
    public static partial ImpedimentoTarefaDTO MapToDto(this ImpedimentoTarefa item);
    public static partial ProjetoDTO MapToDto(this Projeto item);
    
    [MapperIgnoreSource(nameof(Recurso.Senha))]
    public static partial RecursoDTO MapToDto(this Recurso item);
    
    public static partial RecursoProjetoDTO MapToDto(this RecursoProjeto item);
    public static partial RecursoTarefaDTO MapToDto(this RecursoTarefa item);
    public static partial SistemaDTO MapToDto(this Sistema item);
    public static partial TarefaDTO MapToDto(this Tarefa item);
    public static partial TipoTarefaDTO MapToDto(this TipoTarefa item);
    public static partial WorkflowDTO MapToDto(this Workflow item);
}