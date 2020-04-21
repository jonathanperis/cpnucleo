using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Services
{
    internal class ImpedimentoTarefaService : CrudService<ImpedimentoTarefa>, IImpedimentoTarefaService
    {
        private readonly IImpedimentoTarefaRepository _impedimentoTarefaRepository;

        public ImpedimentoTarefaService(IImpedimentoTarefaRepository impedimentoTarefaRepository, IUnitOfWork unitOfWork)
            : base(impedimentoTarefaRepository, unitOfWork)
        {
            _impedimentoTarefaRepository = impedimentoTarefaRepository;
        }

        public IEnumerable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return _impedimentoTarefaRepository.ListarPorTarefa(idTarefa);
        }
    }
}
