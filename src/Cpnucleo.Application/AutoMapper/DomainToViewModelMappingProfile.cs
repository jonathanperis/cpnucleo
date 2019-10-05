using AutoMapper;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.AutoMapper
{
    internal class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Sistema, SistemaViewModel>();
            CreateMap<Projeto, ProjetoViewModel>();
            CreateMap<Tarefa, TarefaViewModel>();
            CreateMap<Apontamento, ApontamentoViewModel>();
            CreateMap<Workflow, WorkflowViewModel>();
            CreateMap<Recurso, RecursoViewModel>();
            CreateMap<Impedimento, ImpedimentoViewModel>();
            CreateMap<ImpedimentoTarefa, ImpedimentoTarefaViewModel>();
            CreateMap<RecursoProjeto, RecursoProjetoViewModel>();
            CreateMap<RecursoTarefa, RecursoTarefaViewModel>();
            CreateMap<TipoTarefa, TipoTarefaViewModel>();
        }
    }
}
