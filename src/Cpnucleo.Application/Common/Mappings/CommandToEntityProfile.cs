namespace Cpnucleo.Application.Common.Mappings
{
    internal class CommandToEntityProfile : Profile
    {
        public CommandToEntityProfile()
        {
            CreateMap<CreateApontamentoCommand, Apontamento>();
            CreateMap<CreateImpedimentoCommand, Impedimento>();
            CreateMap<CreateImpedimentoTarefaCommand, ImpedimentoTarefa>();
            CreateMap<CreateProjetoCommand, Projeto>();
            CreateMap<CreateRecursoCommand, Recurso>();
            CreateMap<CreateRecursoProjetoCommand, RecursoProjeto>();
            CreateMap<CreateRecursoTarefaCommand, RecursoTarefa>();
            CreateMap<CreateSistemaCommand, Sistema>();
            CreateMap<CreateTarefaCommand, Tarefa>();
            CreateMap<CreateTipoTarefaCommand, TipoTarefa>();
            CreateMap<CreateWorkflowCommand, Workflow>();
        }
    }
}
