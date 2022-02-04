namespace Cpnucleo.Application.Configuration;

public class EntityToDTOProfile : Profile
{
    public EntityToDTOProfile()
    {
        CreateMap<Apontamento, ApontamentoDTO>();
        CreateMap<Impedimento, ImpedimentoDTO>();
        CreateMap<ImpedimentoTarefa, ImpedimentoTarefaDTO>();
        CreateMap<Projeto, ProjetoDTO>();
        CreateMap<Recurso, RecursoDTO>();
        CreateMap<RecursoProjeto, RecursoProjetoDTO>();
        CreateMap<RecursoTarefa, RecursoTarefaDTO>();
        CreateMap<Sistema, SistemaDTO>();
        CreateMap<Tarefa, TarefaDTO>();
        CreateMap<TipoTarefa, TipoTarefaDTO>();
        CreateMap<Workflow, WorkflowDTO>();
    }
}
