using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Application.Services
{
    public class ImpedimentoTarefaAppService : AppService<ImpedimentoTarefa, ImpedimentoTarefaViewModel>, IImpedimentoTarefaAppService
    {
        protected readonly IImpedimentoTarefaRepository _impedimentoTarefaRepository;

        public ImpedimentoTarefaAppService(IMapper mapper, IRepository<ImpedimentoTarefa> repository, IImpedimentoTarefaRepository impedimentoTarefaRepository)
            : base(mapper, repository)
        {
            _impedimentoTarefaRepository = impedimentoTarefaRepository;
        }

        public IEnumerable<ImpedimentoTarefaViewModel> ListarPoridTarefa(Guid idTarefa)
        {
            return _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(_impedimentoTarefaRepository.ListarPoridTarefa(idTarefa));
        }
    }
}
