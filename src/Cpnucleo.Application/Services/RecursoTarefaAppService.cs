using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class RecursoTarefaAppService : CrudAppService<RecursoTarefa, RecursoTarefaViewModel>, IRecursoTarefaAppService
    {
        private readonly IRecursoTarefaRepository _recursoTarefaRepository;
        private readonly IApontamentoAppService _apontamentoAppService;

        public RecursoTarefaAppService(IMapper mapper, IRecursoTarefaRepository repository, IUnitOfWork unitOfWork, IRecursoTarefaRepository recursoTarefaRepository, IApontamentoAppService apontamentoAppService)
            : base(mapper, repository, unitOfWork)
        {
            _recursoTarefaRepository = recursoTarefaRepository;
            _apontamentoAppService = apontamentoAppService;
        }

        public IEnumerable<RecursoTarefaViewModel> ListarPorTarefa(Guid idTarefa)
        {
            return _recursoTarefaRepository.ListarPorTarefa(idTarefa).ProjectTo<RecursoTarefaViewModel>(_mapper.ConfigurationProvider).ToList();
        }
    }
}
