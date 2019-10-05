using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;

namespace Cpnucleo.Infra.Data.Repository
{
    public class ImpedimentoRepository : CrudRepository<Impedimento>, IImpedimentoRepository
    {
        public ImpedimentoRepository(CpnucleoContext context)
            : base(context)
        {

        }
    }
}
