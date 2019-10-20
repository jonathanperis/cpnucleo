using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class ApontamentoRepository : CrudRepository<Apontamento>, IApontamentoRepository
    {
        public ApontamentoRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<Apontamento> ListarPorRecurso(Guid idRecurso)
        {
            return Listar()
                .Where(x => x.IdRecurso == idRecurso && x.DataApontamento.Value > DateTime.Now.AddDays(-30));
        }

        public int ObterTotalHorasPorRecurso(Guid idRecurso, Guid idTarefa)
        {
            return Listar()
                .Where(x => x.IdRecurso == idRecurso && x.IdTarefa == idTarefa)
                .Sum(x => x.QtdHoras);
        }
    }
}
