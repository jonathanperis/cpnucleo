using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class RecursoTarefaRepository : Repository<RecursoTarefa>, IRecursoTarefaRepository
    {
        public RecursoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<RecursoTarefa> ListarPoridRecurso(Guid idRecurso)
        {
            throw new NotImplementedException();
        }

        public IQueryable<RecursoTarefa> ListarPoridTarefa(Guid idTarefa)
        {
            throw new NotImplementedException();
        }
    }
}
