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

        public async Task IncluirAsync(ImpedimentoItem impedimento)
        {
            impedimento.DataInclusao = DateTime.Now;
            
            _context.Impedimentos.Add(impedimento);
            await _context.SaveChangesAsync();
        }

        public async Task AlterarAsync(ImpedimentoItem impedimento)
        {
            var impedimentoItem = await ConsultarAsync(impedimento.IdImpedimento);

            impedimentoItem.Nome = impedimento.Nome;

            impedimentoItem.DataAlteracao = DateTime.Now;

            _context.Impedimentos.Update(impedimentoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ImpedimentoItem> ConsultarAsync(int idImpedimento)
        {
            return await _context.Impedimentos
                .SingleOrDefaultAsync(x => x.IdImpedimento == idImpedimento);
        }

        public async Task<IEnumerable<ImpedimentoItem>> ListarAsync()
        {
            return await _context.Impedimentos
                .AsNoTracking()
                .OrderBy(y => y.DataInclusao)
                .ToListAsync();
        }

        public async Task RemoverAsync(ImpedimentoItem impedimento)
        {    
            var impedimentoItem = await ConsultarAsync(impedimento.IdImpedimento);            

            _context.Impedimentos.Remove(impedimentoItem);
            await _context.SaveChangesAsync();
        }
    }
}