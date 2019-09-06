using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    class SistemaRepository : IRepository<SistemaModel>
    {
        private readonly Context _context;

        public SistemaRepository(Context context) => _context = context;

        public async Task IncluirAsync(SistemaModel sistema)
        {
            sistema.DataInclusao = DateTime.Now;
            
            _context.Sistemas.Add(sistema);
            await _context.SaveChangesAsync();
        }

        public async Task AlterarAsync(SistemaModel sistema)
        {
            var sistemaItem = await ConsultarAsync(sistema.IdSistema);

            sistemaItem.Nome = sistema.Nome;
            sistemaItem.Descricao = sistema.Descricao;
            sistemaItem.DataAlteracao = DateTime.Now;

            _context.Sistemas.Update(sistemaItem);
            await _context.SaveChangesAsync();
        }

        public async Task<SistemaModel> ConsultarAsync(int idSistema)
        {
            return await _context.Sistemas
                .SingleOrDefaultAsync(x => x.IdSistema == idSistema);
        }

        public async Task<IEnumerable<SistemaModel>> ListarAsync()
        {
            return await _context.Sistemas
                .AsNoTracking()
                .OrderBy(y => y.DataInclusao)
                .ToListAsync();
        }

        public async Task RemoverAsync(SistemaModel sistema)
        {    
            var sistemaItem = await ConsultarAsync(sistema.IdSistema);            

            _context.Sistemas.Remove(sistemaItem);
            await _context.SaveChangesAsync();
        }
    }
}