using Cpnucleo.Shared.Commands.Apontamento;
using Cpnucleo.Shared.Commands.Impedimento;
using Cpnucleo.Shared.Commands.ImpedimentoTarefa;
using Cpnucleo.Shared.Commands.Projeto;
using Cpnucleo.Shared.Commands.Recurso;
using Cpnucleo.Shared.Commands.RecursoProjeto;
using Cpnucleo.Shared.Commands.RecursoTarefa;
using Cpnucleo.Shared.Commands.Sistema;
using Cpnucleo.Shared.Commands.Tarefa;
using Cpnucleo.Shared.Commands.TipoTarefa;
using Cpnucleo.Shared.Commands.Workflow;

namespace Cpnucleo.Application.Common.Mappings
{
    public class CommandToEntityProfile : Profile
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
