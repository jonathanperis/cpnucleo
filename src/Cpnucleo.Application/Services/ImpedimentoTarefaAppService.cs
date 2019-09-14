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

        public ImpedimentoTarefaAppService(IMapper mapper, IRepository<ImpedimentoTarefa> repository, IUnitOfWork unitOfWork, IImpedimentoTarefaRepository impedimentoTarefaRepository)
            : base(mapper, repository, unitOfWork)
        {
            _impedimentoTarefaRepository = impedimentoTarefaRepository;
        }

        public IEnumerable<ImpedimentoTarefaViewModel> ListarPorTarefa(Guid idTarefa)
        {
            return _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(_impedimentoTarefaRepository.ListarPorTarefa(idTarefa));
        }
    }
}
