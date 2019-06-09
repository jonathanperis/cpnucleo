using Cpnucleo.Pages.Data;
using Cpnucleo.Pages.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cpnucleo.Pages.Repository
{
    public class RecursoProjetoRepository : IRecursoProjetoRepository
    {
        private readonly Context _context;

        public RecursoProjetoRepository(Context context) => _context = context;

        public async Task Incluir(RecursoProjetoItem recursoProjeto)
        {
            recursoProjeto.DataInclusao = DateTime.Now;
            
            _context.RecursoProjetos.Add(recursoProjeto);
            await _context.SaveChangesAsync();
        }

        public async Task<RecursoProjetoItem> Consultar(int idRecursoProjeto)
        {
            return await _context.RecursoProjetos
                .Include(x => x.Projeto)
                .Include(x => x.Recurso)
                .SingleOrDefaultAsync(x => x.IdRecursoProjeto == idRecursoProjeto);
        }

        public async Task Remover(RecursoProjetoItem recursoProjeto)
        {
            var recursoProjetoItem = await Consultar(recursoProjeto.IdRecursoProjeto);

            _context.RecursoProjetos.Remove(recursoProjetoItem);
            await _context.SaveChangesAsync();
        }

        public Task Alterar(RecursoProjetoItem item)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RecursoProjetoItem>> Listar()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RecursoProjetoItem>> ListarPoridProjeto(int idProjeto)
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