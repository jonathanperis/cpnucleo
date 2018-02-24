using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.Impedimento
{
    public class ImpedimentoRepository : IRepository<ImpedimentoItem>
    {
        private readonly ImpedimentoContext _context;

        public ImpedimentoRepository(ImpedimentoContext context)
        {
            _context = context;
        }        

        public async Task Incluir(ImpedimentoItem impedimento)
        {
            impedimento.DataInclusao = DateTime.Now;
            
            _context.Impedimentos.Add(impedimento);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(ImpedimentoItem impedimento)
        {
            var ImpedimentoItem = _context.Impedimentos.Find(impedimento.IdImpedimento);
            ImpedimentoItem.Nome = impedimento.Nome;

            ImpedimentoItem.DataAlteracao = DateTime.Now;

            _context.Impedimentos.Update(ImpedimentoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ImpedimentoItem> Consultar(int idImpedimento)
        {
            return await _context.Impedimentos
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.IdImpedimento == idImpedimento);
        }

        public async Task<IList<ImpedimentoItem>> Listar()
        {
            return await _context.Impedimentos
                .AsNoTracking()
                .OrderBy(y => y.DataInclusao)
                .ToListAsync();
        }

        public async Task Remover(ImpedimentoItem impedimento)
        {    
            var ImpedimentoItem = _context.Impedimentos.Find(impedimento.IdImpedimento);            

            _context.Impedimentos.Remove(ImpedimentoItem);
            await _context.SaveChangesAsync();
        }
    }
}