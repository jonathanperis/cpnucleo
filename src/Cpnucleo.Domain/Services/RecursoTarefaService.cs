using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Services
{
    internal class RecursoTarefaService : CrudService<RecursoTarefa>, IRecursoTarefaService
    {
        private readonly IRecursoTarefaRepository _recursoTarefaRepository;

        public RecursoTarefaService(IRecursoTarefaRepository recursoTarefaRepository, IUnitOfWork unitOfWork)
            : base(recursoTarefaRepository, unitOfWork)
        {
            _recursoTarefaRepository = recursoTarefaRepository;
        }

        public IEnumerable<RecursoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return _recursoTarefaRepository.ListarPorTarefa(idTarefa);
        }
    }
}
