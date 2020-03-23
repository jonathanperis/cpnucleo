using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Services
{
    public class ImpedimentoTarefaService : CrudService<ImpedimentoTarefa>, IImpedimentoTarefaService
    {
        private readonly IImpedimentoTarefaRepository _impedimentoTarefaRepository;

        public ImpedimentoTarefaService(IImpedimentoTarefaRepository impedimentoTarefaRepository, IUnitOfWork unitOfWork)
            : base(impedimentoTarefaRepository, unitOfWork)
        {
            _impedimentoTarefaRepository = impedimentoTarefaRepository;
        }

        public IQueryable<ImpedimentoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return _impedimentoTarefaRepository.ListarPorTarefa(idTarefa);
        }
    }
}
