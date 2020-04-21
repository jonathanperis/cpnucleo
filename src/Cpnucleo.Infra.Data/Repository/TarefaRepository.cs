using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cpnucleo.Infra.Data.Repository
{
    internal class TarefaRepository : CrudRepository<Tarefa>, ITarefaRepository
    {
        public TarefaRepository(CpnucleoContext context)
            : base(context)
        {

        }

        public IEnumerable<Tarefa> ListarPorRecurso(Guid idRecurso)
        {
            return Listar()
                .Include(_context.GetIncludePaths(typeof(Tarefa)))
                .Select(Tarefa => new
                {
                    Tarefa,
                    ListaRecursoTarefas = Tarefa.ListaRecursoTarefas
                        .Where(p => p.IdRecurso == idRecurso)
                })
                .Select(x => x.Tarefa)
                .ToList();
        }
    }
}
