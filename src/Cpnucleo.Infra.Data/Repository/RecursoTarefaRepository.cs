using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class RecursoTarefaRepository : CrudRepository<RecursoTarefa>, IRecursoTarefaRepository
    {
        public RecursoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<RecursoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return Listar()
                .Where(x => x.IdTarefa == idTarefa);
        }
    }
}
