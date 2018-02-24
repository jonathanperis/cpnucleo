using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using dotnet_cpnucleo_pages.Security;

namespace dotnet_cpnucleo_pages.Repository.Recurso
{
    public class RecursoRepository : IRecursoRepository
    {
        private readonly RecursoContext _context;

        public RecursoRepository(RecursoContext context)
        {
            _context = context;
        }        

        public async Task Incluir(RecursoItem recurso)
        {
            CryptographyManager.CryptPBKDF2(recurso.Senha, out string itemCriptografado, out string salt);

            recurso.SenhaCriptografada = itemCriptografado;
            recurso.Salt = salt;

            recurso.DataInclusao = DateTime.Now;
            
            _context.Recursos.Add(recurso);
            await _context.SaveChangesAsync();
        }

        public async Task Alterar(RecursoItem recurso)
        {
            var RecursoItem = _context.Recursos.Find(recurso.IdRecurso);

            CryptographyManager.CryptPBKDF2(recurso.Senha, out string itemCriptografado, out string salt);

            RecursoItem.SenhaCriptografada = itemCriptografado;
            RecursoItem.Salt = salt;

            RecursoItem.Nome = recurso.Nome;
            RecursoItem.Ativo = recurso.Ativo;

            RecursoItem.DataAlteracao = DateTime.Now;

            _context.Recursos.Update(RecursoItem);
            await _context.SaveChangesAsync();
        }

        public async Task<RecursoItem> Consultar(int idRecurso)
        {
            return await _context.Recursos
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.IdRecurso == idRecurso);
        }

        public async Task<IList<RecursoItem>> Listar()
        {
            return await _context.Recursos
                .AsNoTracking()
                .OrderBy(y => y.DataInclusao)
                .ToListAsync();
        }

        public async Task Remover(RecursoItem recurso)
        {    
            var RecursoItem = _context.Recursos.Find(recurso.IdRecurso);            

            _context.Recursos.Remove(RecursoItem);
            await _context.SaveChangesAsync();
        }

        public RecursoItem ValidarRecurso(string login, string senha, out bool valido)
        {
            valido = false;

            var RecursoItem = _context.Recursos.SingleOrDefault(x => x.Login == login);

            if (RecursoItem == null) return RecursoItem;

            valido = CryptographyManager.VerifyPBKDF2(senha, RecursoItem.SenhaCriptografada, RecursoItem.Salt);

            return RecursoItem;
        }        
    }
}