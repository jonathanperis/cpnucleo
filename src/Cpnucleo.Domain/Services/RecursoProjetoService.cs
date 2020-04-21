using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace Cpnucleo.Domain.Services
{
    internal class RecursoProjetoService : CrudService<RecursoProjeto>, IRecursoProjetoService
    {
        private readonly IRecursoProjetoRepository _recursoProjetoRepository;

        public RecursoProjetoService(IRecursoProjetoRepository recursoProjetoRepository, IUnitOfWork unitOfWork)
            : base(recursoProjetoRepository, unitOfWork)
        {
            _recursoProjetoRepository = recursoProjetoRepository;
        }

        public IEnumerable<RecursoProjeto> ListarPorProjeto(Guid idProjeto)
        {
            return _recursoProjetoRepository.ListarPorProjeto(idProjeto);
        }
    }
}
