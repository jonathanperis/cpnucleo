using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository.ImpedimentoTarefa
{
    public class ImpedimentoTarefaRepository : IImpedimentoTarefaRepository
    {
        private readonly Context _context;

        public ImpedimentoTarefaRepository(Context context) => _context = context;

        public async Task Incluir(ImpedimentoTarefaItem impedimentoTarefa)
        {
            impedimentoTarefa.DataInclusao = DateTime.Now;
            
            _context.ImpedimentoTarefas.Add(impedimentoTarefa);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(ImpedimentoTarefaItem impedimentoTarefa)
        {
            var impedimentoTarefaItem = _context.ImpedimentoTarefas.Find(impedimentoTarefa.IdImpedimentoTarefa);
            impedimentoTarefaItem.Descricao = impedimentoTarefa.Descricao;
            impedimentoTarefaItem.IdImpedimento = impedimentoTarefa.IdImpedimento;
            impedimentoTarefaItem.Ativo = impedimentoTarefa.Ativo;            

            impedimentoTarefaItem.DataAlteracao = DateTime.Now;

            _context.ImpedimentoTarefas.Update(impedimentoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ImpedimentoTarefaItem> Consultar(int idImpedimentoTarefa)
        {
            return await _context.ImpedimentoTarefas
                .AsNoTracking()
                .Include(x => x.Tarefa)
                .Include(x => x.Impedimento)
                .SingleOrDefaultAsync(x => x.IdImpedimentoTarefa == idImpedimentoTarefa);
        }

        public async Task Remover(ImpedimentoTarefaItem impedimentoTarefa)
        {    
            var impedimentoTarefaItem = _context.ImpedimentoTarefas.Find(impedimentoTarefa.IdImpedimentoTarefa);            

            _context.ImpedimentoTarefas.Remove(impedimentoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public Task<IEnumerable<ImpedimentoTarefaItem>> Listar()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ImpedimentoTarefaItem>> ListarPoridTarefa(int idTarefa)
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