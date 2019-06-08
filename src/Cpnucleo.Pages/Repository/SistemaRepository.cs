using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public class SistemaRepository : IRepository<SistemaItem>
    {
        private readonly Context _context;

        public SistemaRepository(Context context) => _context = context;

        public async Task Incluir(SistemaItem sistema)
        {
            sistema.DataInclusao = DateTime.Now;
            
            _context.Sistemas.Add(sistema);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(SistemaItem sistema)
        {
            var sistemaItem = _context.Sistemas.Find(sistema.IdSistema);
            sistemaItem.Nome = sistema.Nome;
            sistemaItem.Descricao = sistema.Descricao;

            sistemaItem.DataAlteracao = DateTime.Now;

            _context.Sistemas.Update(sistemaItem);
            await _context.SaveChangesAsync();
        }

        public async Task<SistemaItem> Consultar(int idSistema)
        {
            return await _context.Sistemas
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.IdSistema == idSistema);
        }

        public async Task<IEnumerable<SistemaItem>> Listar()
        {
            return await _context.Sistemas
                .AsNoTracking()
                .OrderBy(y => y.DataInclusao)
                .ToListAsync();
        }

        public async Task Remover(SistemaItem sistema)
        {    
            var sistemaItem = _context.Sistemas.Find(sistema.IdSistema);            

            _context.Sistemas.Remove(sistemaItem);
            await _context.SaveChangesAsync();
        }
    }
}