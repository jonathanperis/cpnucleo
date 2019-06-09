using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public class ImpedimentoTarefaRepository : IImpedimentoTarefaRepository
    {
        private readonly Context _context;

        public ImpedimentoTarefaRepository(Context context) => _context = context;

        public async Task IncluirAsync(ImpedimentoTarefaItem impedimentoTarefa)
        {
            impedimentoTarefa.DataInclusao = DateTime.Now;
            
            _context.ImpedimentoTarefas.Add(impedimentoTarefa);
            await _context.SaveChangesAsync();
        }

        public async Task AlterarAsync(ImpedimentoTarefaItem impedimentoTarefa)
        {
            var impedimentoTarefaItem = await ConsultarAsync(impedimentoTarefa.IdImpedimentoTarefa);

            impedimentoTarefaItem.Descricao = impedimentoTarefa.Descricao;
            impedimentoTarefaItem.IdImpedimento = impedimentoTarefa.IdImpedimento;
            impedimentoTarefaItem.Ativo = impedimentoTarefa.Ativo;            

            impedimentoTarefaItem.DataAlteracao = DateTime.Now;

            _context.ImpedimentoTarefas.Update(impedimentoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ImpedimentoTarefaItem> ConsultarAsync(int idImpedimentoTarefa)
        {
            return await _context.ImpedimentoTarefas
                .Include(x => x.Tarefa)
                .Include(x => x.Impedimento)
                .SingleOrDefaultAsync(x => x.IdImpedimentoTarefa == idImpedimentoTarefa);
        }

        public async Task RemoverAsync(ImpedimentoTarefaItem impedimentoTarefa)
        {    
            var impedimentoTarefaItem = await ConsultarAsync(impedimentoTarefa.IdImpedimentoTarefa);            

            _context.ImpedimentoTarefas.Remove(impedimentoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<ImpedimentoTarefaItem>> ListarAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ImpedimentoTarefaItem>> ListarPoridTarefaAsync(int idTarefa)
        {
            return await _context.ImpedimentoTarefas
                .AsNoTracking()
                .Include(x => x.Tarefa)
                .Include(x => x.Impedimento)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdTarefa == idTarefa)
                .ToListAsync();
        }
    }
}