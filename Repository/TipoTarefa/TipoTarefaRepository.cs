using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_cpnucleo_pages.Repository.TipoTarefa
{
    public class TipoTarefaRepository : IRepository<TipoTarefaItem>
    {
        private readonly TipoTarefaContext _context;

        public TipoTarefaRepository(TipoTarefaContext context)
        {
            _context = context;
        }        

        public async Task Incluir(TipoTarefaItem tipoTarefa)
        {
            tipoTarefa.DataInclusao = DateTime.Now;
            
            _context.TipoTarefas.Add(tipoTarefa);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(TipoTarefaItem tipoTarefa)
        {
            var tipoTarefaItem = _context.TipoTarefas.Find(tipoTarefa.IdTipoTarefa);
            tipoTarefaItem.Nome = tipoTarefa.Nome;

            tipoTarefaItem.DataAlteracao = DateTime.Now;

            _context.TipoTarefas.Update(tipoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public async Task<TipoTarefaItem> Consultar(int idTipoTarefa)
        {
            return await _context.TipoTarefas
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.IdTipoTarefa == idTipoTarefa);
        }

        public async Task<IEnumerable<TipoTarefaItem>> Listar()
        {
            return await _context.TipoTarefas
                .AsNoTracking()
                .OrderBy(y => y.DataInclusao)
                .ToListAsync();
        }

        public async Task Remover(TipoTarefaItem tipoTarefa)
        {    
            var tipoTarefaItem = _context.TipoTarefas.Find(tipoTarefa.IdTipoTarefa);            

            _context.TipoTarefas.Remove(tipoTarefaItem);
            await _context.SaveChangesAsync();
        }
    }
}