using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System;

namespace Cpnucleo.Infra.Data.Repository
{
    public class RecursoRepository : Repository<Recurso>, IRecursoRepository
    {
        public RecursoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public Recurso ValidarRecurso(string usuario, string senha, out bool valido)
        {
            throw new NotImplementedException();
        }
    }
}
