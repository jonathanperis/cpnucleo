using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnet_cpnucleo_pages.Repository.RecursoProjeto
{
    public class RecursoProjetoRepository : IRecursoProjetoRepository
    {
        private readonly RecursoProjetoContext _context;

        public RecursoProjetoRepository(RecursoProjetoContext context)
        {
            _context = context;
        }        

        public async Task Incluir(RecursoProjetoItem recursoProjeto)
        {
            recursoProjeto.DataInclusao = DateTime.Now;
            
            _context.RecursoProjetos.Add(recursoProjeto);
            await _context.SaveChangesAsync();
        }

        public async Task<RecursoProjetoItem> Consultar(int idRecursoProjeto)
        {
            return await _context.RecursoProjetos
                .AsNoTracking()
                .Include(x => x.Projeto)
                .Include(x => x.Recurso)
                .SingleOrDefaultAsync(x => x.IdRecursoProjeto == idRecursoProjeto);
        }

        public async Task Remover(RecursoProjetoItem recursoProjeto)
        {
            var RecursoProjetoItem = _context.RecursoProjetos.Find(recursoProjeto.IdRecursoProjeto);

            _context.RecursoProjetos.Remove(RecursoProjetoItem);
            await _context.SaveChangesAsync();
        }

        public Task Alterar(RecursoProjetoItem item)
        {
            throw new NotImplementedException();
        }

        public Task<IList<RecursoProjetoItem>> Listar()
        {
            throw new NotImplementedException();
        }

        public async Task<IList<RecursoProjetoItem>> ListarPoridProjeto(int idProjeto)
        {
            return await _context.RecursoProjetos
                .AsNoTracking()
                .Include(x => x.Projeto)
                .Include(x => x.Recurso)
                .OrderBy(x => x.DataInclusao)
                .Where(x => x.IdProjeto == idProjeto)                                
                .ToListAsync();
        }
    }
}