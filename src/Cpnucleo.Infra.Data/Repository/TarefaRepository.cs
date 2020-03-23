using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    public class TarefaRepository : CrudRepository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IQueryable<Tarefa> ListarPorRecurso(Guid idRecurso)
        {
            return Listar()
                .Include(Tarefa => Tarefa.Workflow)
                .Include(Tarefa => Tarefa.TipoTarefa)
                .Select(Tarefa => new
                {
                    Tarefa,
                    ListaRecursoTarefas = Tarefa.ListaRecursoTarefas
                        .Where(p => p.IdRecurso == idRecurso)
                })
                .Select(x => x.Tarefa);
        }
    }
}
