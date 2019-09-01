using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    class TipoTarefaRepository : IRepository<TipoTarefaItem>
    {
        private readonly Context _context;

        public TipoTarefaRepository(Context context) => _context = context;

        public async Task IncluirAsync(TipoTarefaItem tipoTarefa)
        {
            tipoTarefa.DataInclusao = DateTime.Now;
            
            _context.TipoTarefas.Add(tipoTarefa);
            await _context.SaveChangesAsync();
        }

        public async Task AlterarAsync(TipoTarefaItem tipoTarefa)
        {
            var tipoTarefaItem = await ConsultarAsync(tipoTarefa.IdTipoTarefa);

            tipoTarefaItem.Nome = tipoTarefa.Nome;
            tipoTarefaItem.DataAlteracao = DateTime.Now;

            _context.TipoTarefas.Update(tipoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public async Task<TipoTarefaItem> ConsultarAsync(int idTipoTarefa)
        {
            return await _context.TipoTarefas
                .SingleOrDefaultAsync(x => x.IdTipoTarefa == idTipoTarefa);
        }

        public async Task<IEnumerable<TipoTarefaItem>> ListarAsync()
        {
            return await _context.TipoTarefas
                .AsNoTracking()
                .OrderBy(y => y.DataInclusao)
                .ToListAsync();
        }

        public async Task RemoverAsync(TipoTarefaItem tipoTarefa)
        {    
            var tipoTarefaItem = await ConsultarAsync(tipoTarefa.IdTipoTarefa);            

            _context.TipoTarefas.Remove(tipoTarefaItem);
            await _context.SaveChangesAsync();
        }
    }
}