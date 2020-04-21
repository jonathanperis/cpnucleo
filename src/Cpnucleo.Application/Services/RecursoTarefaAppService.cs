using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    internal class RecursoTarefaAppService : CrudAppService<RecursoTarefa, RecursoTarefaViewModel>, IRecursoTarefaAppService
    {
        private readonly IRecursoTarefaService _recursoTarefaService;

        public RecursoTarefaAppService(IMapper mapper, IRecursoTarefaService recursoTarefaService)
            : base(mapper, recursoTarefaService)
        {
            _recursoTarefaService = recursoTarefaService;
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(Guid idTarefa)
        {
            return _mapper.Map<IEnumerable<RecursoTarefaViewModel>>(_recursoTarefaService.ListarPorTarefa(idTarefa));
        }
    }
}
