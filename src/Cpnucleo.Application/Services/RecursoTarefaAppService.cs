using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class RecursoTarefaAppService : CrudAppService<RecursoTarefa, RecursoTarefaViewModel>, IRecursoTarefaAppService
    {
        private readonly IRecursoTarefaService _recursoTarefaService;

        public RecursoTarefaAppService(IMapper mapper, IRecursoTarefaService recursoTarefaService)
            : base(mapper, recursoTarefaService)
        {
            _recursoTarefaService = recursoTarefaService;
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(Guid idTarefa)
        {
            return _recursoTarefaService.ListarPorTarefa(idTarefa).ProjectTo<RecursoTarefaViewModel>(_mapper.ConfigurationProvider).ToList();
        }
    }
}
