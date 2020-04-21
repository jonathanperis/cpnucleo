using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    internal class TarefaAppService : CrudAppService<Tarefa, TarefaViewModel>, ITarefaAppService
    {
        private readonly ITarefaService _tarefaService;

        public TarefaAppService(IMapper mapper, ITarefaService tarefaService)
            : base(mapper, tarefaService)
        {
            _tarefaService = tarefaService;
        }

        public IEnumerable<TarefaViewModel> ListarPorRecurso(Guid idRecurso)
        {
            return _mapper.Map<IEnumerable<TarefaViewModel>>(_tarefaService.ListarPorRecurso(idRecurso));
        }

        public bool AlterarPorWorkflow(Guid idTarefa, Guid idWorkflow)
        {
            return _tarefaService.AlterarPorWorkflow(idTarefa, idWorkflow);
        }
    }
}
