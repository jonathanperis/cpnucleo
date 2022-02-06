namespace Cpnucleo.Application.Configuration;

public class DTOToEntityProfile : Profile
{
    public DTOToEntityProfile()
    {
        CreateMap<ApontamentoDTO, Apontamento>();
        CreateMap<ImpedimentoDTO, Impedimento>();
        CreateMap<ImpedimentoTarefaDTO, ImpedimentoTarefa>();
        CreateMap<ProjetoDTO, Projeto>();
        CreateMap<RecursoDTO, Recurso>();
        CreateMap<RecursoProjetoDTO, RecursoProjeto>();
        CreateMap<RecursoTarefaDTO, RecursoTarefa>();
        CreateMap<SistemaDTO, Sistema>();
        CreateMap<TarefaDTO, Tarefa>();
        CreateMap<TipoTarefaDTO, TipoTarefa>();
        CreateMap<WorkflowDTO, Workflow>();
    }
}
