using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public class ImpedimentoRepository : IRepository<ImpedimentoItem>
    {
        private readonly Context _context;

        public ImpedimentoRepository(Context context) => _context = context;

        public async Task Incluir(ImpedimentoItem impedimento)
        {
            impedimento.DataInclusao = DateTime.Now;
            
            _context.Impedimentos.Add(impedimento);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(ImpedimentoItem impedimento)
        {
            var impedimentoItem = _context.Impedimentos.Find(impedimento.IdImpedimento);
            impedimentoItem.Nome = impedimento.Nome;

            impedimentoItem.DataAlteracao = DateTime.Now;

            _context.Impedimentos.Update(impedimentoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ImpedimentoItem> Consultar(int idImpedimento)
        {
            return await _context.Impedimentos
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.IdImpedimento == idImpedimento);
        }

        public async Task<IEnumerable<ImpedimentoItem>> Listar()
        {
            return await _context.Impedimentos
                .AsNoTracking()
                .OrderBy(y => y.DataInclusao)
                .ToListAsync();
        }

        public async Task Remover(ImpedimentoItem impedimento)
        {    
            var impedimentoItem = _context.Impedimentos.Find(impedimento.IdImpedimento);            

            _context.Impedimentos.Remove(impedimentoItem);
            await _context.SaveChangesAsync();
        }
    }
}