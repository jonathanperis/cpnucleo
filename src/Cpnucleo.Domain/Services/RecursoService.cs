using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Repositories;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using System.Linq;

namespace Cpnucleo.Domain.Services
{
    public class RecursoService : CrudService<Recurso>, IRecursoService
    {
        private readonly IRecursoRepository _recursoRepository;
        private readonly ICryptographyManager _cryptographyManager;

        public RecursoService(IRecursoRepository recursoRepository, IUnitOfWork unitOfWork, ICryptographyManager cryptographyManager)
            : base(recursoRepository, unitOfWork)
        {
            _recursoRepository = recursoRepository;
            _cryptographyManager = cryptographyManager;
        }

        public new bool Incluir(Recurso recurso)
        {
            _cryptographyManager.CryptPbkdf2(recurso.Senha, out string senhaCrypt, out string salt);

            recurso.Senha = senhaCrypt;
            recurso.Salt = salt;

            return base.Incluir(recurso);
        }

        public new bool Alterar(Recurso recurso)
        {
            _cryptographyManager.CryptPbkdf2(recurso.Senha, out string senhaCrypt, out string salt);

            recurso.Senha = senhaCrypt;
            recurso.Salt = salt;

            return base.Alterar(recurso);
        }

        public Recurso Autenticar(string login, string senha, out bool valido)
        {
            valido = false;

            Recurso recurso = _recursoRepository.ConsultarPorLogin(login).FirstOrDefault();

            if (recurso == null)
            {
                return null;
            }

            valido = _cryptographyManager.VerifyPbkdf2(senha, recurso.Senha, recurso.Salt);

            return recurso;
        }
    }
}
