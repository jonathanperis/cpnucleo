using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Infra.Data.Repositories
{
    internal class TarefaRepository : GenericRepository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public async Task<IEnumerable<Tarefa>> GetByRecursoAsync(Guid idRecurso)
        {
            IEnumerable<Tarefa> result = await AllAsync(true);

            return result
                .Select(Tarefa => new
                {
                    Tarefa,
                    // ListaRecursoTarefas = Tarefa.ListaRecursoTarefas
                    //     .Where(p => p.IdRecurso == idRecurso)
                })
                .Select(x => x.Tarefa)
                .ToList();
        }
    }
}
