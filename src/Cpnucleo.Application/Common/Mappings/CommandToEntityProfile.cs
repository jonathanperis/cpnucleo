using Cpnucleo.Application.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Application.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Application.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;
using Cpnucleo.Application.Commands.Projeto.CreateProjeto;
using Cpnucleo.Application.Commands.Recurso.CreateRecurso;
using Cpnucleo.Application.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Application.Commands.RecursoTarefa.CreateRecursoTarefa;
using Cpnucleo.Application.Commands.Sistema.CreateSistema;
using Cpnucleo.Application.Commands.Tarefa.CreateTarefa;
using Cpnucleo.Application.Commands.TipoTarefa.CreateTipoTarefa;
using Cpnucleo.Application.Commands.Workflow.CreateWorkflow;

namespace Cpnucleo.Application.Configuration
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
