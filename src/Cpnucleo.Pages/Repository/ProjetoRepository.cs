using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    class ProjetoRepository : IRepository<ProjetoItem>
    {
        private readonly Context _context;

        public ProjetoRepository(Context context) => _context = context;

        public async Task IncluirAsync(ProjetoItem projeto)
        {
            projeto.DataInclusao = DateTime.Now;
            
            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
        }

        public async Task AlterarAsync(ProjetoItem projeto)
        {
            var projetoItem = await ConsultarAsync(projeto.IdProjeto);

            projetoItem.Nome = projeto.Nome;
            projetoItem.IdSistema = projeto.IdSistema;
            projetoItem.DataAlteracao = DateTime.Now;

            _context.Projetos.Update(projetoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ProjetoItem> ConsultarAsync(int idProjeto)
        {
            return await _context.Projetos
                .Include(x => x.Sistema)
                .SingleOrDefaultAsync(x => x.IdProjeto == idProjeto);
        }

        public async Task<IEnumerable<ProjetoItem>> ListarAsync()
        {
            return await _context.Projetos
                .AsNoTracking()
                .Include(x => x.Sistema)
                .OrderBy(x => x.DataInclusao)
                .ToListAsync();
        }

        public async Task RemoverAsync(ProjetoItem projeto)
        {    
            var projetoItem = await ConsultarAsync(projeto.IdProjeto);            

            _context.Projetos.Remove(projetoItem);
            await _context.SaveChangesAsync();
        }
    }
}