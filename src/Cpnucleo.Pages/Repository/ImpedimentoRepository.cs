using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    class ImpedimentoRepository : IRepository<ImpedimentoModel>
    {
        private readonly Context _context;

        public ImpedimentoRepository(Context context) => _context = context;

        public async Task IncluirAsync(ImpedimentoModel impedimento)
        {
            impedimento.DataInclusao = DateTime.Now;
            
            _context.Impedimentos.Add(impedimento);
            await _context.SaveChangesAsync();
        }

        public async Task AlterarAsync(ImpedimentoModel impedimento)
        {
            var impedimentoItem = await ConsultarAsync(impedimento.IdImpedimento);

            impedimentoItem.Nome = impedimento.Nome;
            impedimentoItem.DataAlteracao = DateTime.Now;

            _context.Impedimentos.Update(impedimentoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ImpedimentoModel> ConsultarAsync(int idImpedimento)
        {
            return await _context.Impedimentos
                .SingleOrDefaultAsync(x => x.IdImpedimento == idImpedimento);
        }

        public async Task<IEnumerable<ImpedimentoModel>> ListarAsync()
        {
            return await _context.Impedimentos
                .AsNoTracking()
                .OrderBy(y => y.DataInclusao)
                .ToListAsync();
        }

        public async Task RemoverAsync(ImpedimentoModel impedimento)
        {    
            var impedimentoItem = await ConsultarAsync(impedimento.IdImpedimento);            

            _context.Impedimentos.Remove(impedimentoItem);
            await _context.SaveChangesAsync();
        }
    }
}