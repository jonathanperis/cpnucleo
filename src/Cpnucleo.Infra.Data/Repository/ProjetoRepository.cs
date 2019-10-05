using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;

namespace Cpnucleo.Infra.Data.Repository
{
    public class ProjetoRepository : CrudRepository<Projeto>, IProjetoRepository
    {
        public ProjetoRepository(CpnucleoContext context)
            : base(context)
        {

        }
    }
}
