using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;

namespace Cpnucleo.Infra.Data.Repository
{
    public class SistemaRepository : CrudRepository<Sistema>, ISistemaRepository
    {
        public SistemaRepository(CpnucleoContext context)
            : base(context)
        {

        }
    }
}
