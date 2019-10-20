using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
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

        public IQueryable<RecursoTarefa> ListarPorRecurso(Guid idRecurso)
        {
            return Listar()
                .Where(x => x.IdRecurso == idRecurso);
        }

        public IQueryable<RecursoTarefa> ListarPorTarefa(Guid idTarefa)
        {
            return Listar()
                .Where(x => x.IdTarefa == idTarefa);
        }
    }
}
