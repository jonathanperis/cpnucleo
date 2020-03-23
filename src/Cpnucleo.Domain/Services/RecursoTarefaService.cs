using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Linq;

namespace Cpnucleo.Domain.Services
{
    public class RecursoTarefaService : CrudService<RecursoTarefa>, IRecursoTarefaService
    {
        private readonly IRecursoTarefaRepository _recursoTarefaRepository;

        public RecursoTarefaService(IRecursoTarefaRepository recursoTarefaRepository, IUnitOfWork unitOfWork)
            : base(recursoTarefaRepository, unitOfWork)
        {
            _recursoTarefaRepository = recursoTarefaRepository;
        }

        public IQueryable<RecursoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return _recursoTarefaRepository.ListarPorTarefa(idTarefa);
        }
    }
}
