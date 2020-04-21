using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Services
{
    internal class ApontamentoService : CrudService<Apontamento>, IApontamentoService
    {
        private readonly IApontamentoRepository _apontamentoRepository;

        public ApontamentoService(IApontamentoRepository apontamentoRepository, IUnitOfWork unitOfWork)
            : base(apontamentoRepository, unitOfWork)
        {
            _apontamentoRepository = apontamentoRepository;
        }

        public IEnumerable<Apontamento> ListarPorRecurso(Guid idRecurso)
        {
            return _apontamentoRepository.ListarPorRecurso(idRecurso);
        }

        public int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa)
        {
            return _apontamentoRepository.ObterTotalHorasPorRecurso(idRecurso, idTarefa);
        }
    }
}
