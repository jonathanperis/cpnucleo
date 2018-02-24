using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.ImpedimentoTarefa
{
    public class ImpedimentoTarefaRepository : IImpedimentoTarefaRepository
    {
        private readonly ImpedimentoTarefaContext _context;

        public ImpedimentoTarefaRepository(ImpedimentoTarefaContext context)
        {
            _context = context;
        }        

        public async Task Incluir(ImpedimentoTarefaItem impedimentoTarefa)
        {
            impedimentoTarefa.DataInclusao = DateTime.Now;
            
            _context.ImpedimentoTarefas.Add(impedimentoTarefa);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(ImpedimentoTarefaItem impedimentoTarefa)
        {
            var ImpedimentoTarefaItem = _context.ImpedimentoTarefas.Find(impedimentoTarefa.IdImpedimentoTarefa);
            ImpedimentoTarefaItem.Descricao = impedimentoTarefa.Descricao;
            ImpedimentoTarefaItem.IdImpedimento = impedimentoTarefa.IdImpedimento;
            ImpedimentoTarefaItem.Ativo = impedimentoTarefa.Ativo;            

            ImpedimentoTarefaItem.DataAlteracao = DateTime.Now;

            _context.ImpedimentoTarefas.Update(ImpedimentoTarefaItem);
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
            var ImpedimentoTarefaItem = _context.ImpedimentoTarefas.Find(impedimentoTarefa.IdImpedimentoTarefa);            

            _context.ImpedimentoTarefas.Remove(ImpedimentoTarefaItem);
            await _context.SaveChangesAsync();
        }

        public Task<IList<ImpedimentoTarefaItem>> Listar()
        {
            throw new NotImplementedException();
        }

        public async Task<IList<ImpedimentoTarefaItem>> ListarPoridTarefa(int idTarefa)
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