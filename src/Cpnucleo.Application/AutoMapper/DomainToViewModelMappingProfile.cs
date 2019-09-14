using AutoMapper;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Models;

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
