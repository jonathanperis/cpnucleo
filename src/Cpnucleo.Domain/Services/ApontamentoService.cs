using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Services
{
    public class ApontamentoService : CrudService<Apontamento>, IApontamentoService
    {
        private readonly IApontamentoRepository _apontamentoRepository;

        public ApontamentoService(IApontamentoRepository apontamentoRepository, IUnitOfWork unitOfWork)
            : base(apontamentoRepository, unitOfWork)
        {
            _apontamentoRepository = apontamentoRepository;
        }

        public IQueryable<Apontamento> ListarPorRecurso(Guid idRecurso)
        {
            return _apontamentoRepository.ListarPorRecurso(idRecurso);
        }

        public int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa)
        {
            return _apontamentoRepository.ObterTotalHorasPorRecurso(idRecurso, idTarefa);
        }
    }
}
