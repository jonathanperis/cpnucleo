using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;

namespace Cpnucleo.Infra.Data.Repository
{
    public class TipoTarefaRepository : CrudRepository<TipoTarefa>, ITipoTarefaRepository
    {
        public TipoTarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }
    }
}
