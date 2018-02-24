using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.Projeto
{
    public class ProjetoRepository : IRepository<ProjetoItem>
    {
        private readonly ProjetoContext _context;

        public ProjetoRepository(ProjetoContext context)
        {
            _context = context;
        }        

        public async Task Incluir(ProjetoItem projeto)
        {
            projeto.DataInclusao = DateTime.Now;
            
            _context.Projetos.Add(projeto);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(ProjetoItem projeto)
        {
            var projetoItem = _context.Projetos.Find(projeto.IdProjeto);
            projetoItem.Nome = projeto.Nome;
            projetoItem.IdSistema = projeto.IdSistema;

            projetoItem.DataAlteracao = DateTime.Now;

            _context.Projetos.Update(projetoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ProjetoItem> Consultar(int idProjeto)
        {
            return await _context.Projetos
                .AsNoTracking()
                .Include(x => x.Sistema)
                .SingleOrDefaultAsync(x => x.IdProjeto == idProjeto);
        }

        public async Task<IList<ProjetoItem>> Listar()
        {
            return await _context.Projetos
                .AsNoTracking()
                .Include(x => x.Sistema)
                .OrderBy(x => x.DataInclusao)
                .ToListAsync();
        }

        public async Task Remover(ProjetoItem projeto)
        {    
            var projetoItem = _context.Projetos.Find(projeto.IdProjeto);            

            _context.Projetos.Remove(projetoItem);
            await _context.SaveChangesAsync();
        }
    }
}