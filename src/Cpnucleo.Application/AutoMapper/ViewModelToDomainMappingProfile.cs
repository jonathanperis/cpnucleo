using AutoMapper;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Models;

namespace Cpnucleo.Application.AutoMapper
{
    internal class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<SistemaViewModel, Sistema>();
            CreateMap<ProjetoViewModel, Projeto>();
            CreateMap<TarefaViewModel, Tarefa>();
            CreateMap<ApontamentoViewModel, Apontamento>();
            CreateMap<WorkflowViewModel, Workflow>();
            CreateMap<RecursoViewModel, Recurso>();
            CreateMap<ImpedimentoViewModel, Impedimento>();
            CreateMap<ImpedimentoTarefaViewModel, ImpedimentoTarefa>();
            CreateMap<RecursoProjetoViewModel, RecursoProjeto>();
            CreateMap<RecursoTarefaViewModel, RecursoTarefa>();
            CreateMap<TipoTarefaViewModel, TipoTarefa>();
        }
    }
}
