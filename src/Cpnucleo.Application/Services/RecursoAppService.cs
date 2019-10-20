using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using Cpnucleo.Infra.Security.Interfaces;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class RecursoAppService : CrudAppService<Recurso, RecursoViewModel>, IRecursoAppService
    {
        protected readonly IRecursoRepository _recursoRepository;
        protected readonly ICryptographyManager _cryptographyManager;

        public RecursoAppService(IMapper mapper, ICrudRepository<Recurso> repository, IUnitOfWork unitOfWork, IRecursoRepository recursoRepository, ICryptographyManager cryptographyManager)
            : base(mapper, repository, unitOfWork)
        {
            _recursoRepository = recursoRepository;
            _cryptographyManager = cryptographyManager;
        }

        public new bool Incluir(RecursoViewModel recurso)
        {
            _cryptographyManager.CryptPbkdf2(recurso.Senha, out string senhaCrypt, out string salt);

            recurso.Senha = senhaCrypt;
            recurso.Salt = salt;

            return base.Incluir(recurso);
        }

        public new bool Alterar(RecursoViewModel recurso)
        {
            _cryptographyManager.CryptPbkdf2(recurso.Senha, out string senhaCrypt, out string salt);

            recurso.Senha = senhaCrypt;
            recurso.Salt = salt;

            return base.Alterar(recurso);
        }

        public RecursoViewModel Autenticar(string login, string senha, out bool valido)
        {
            valido = false;

            RecursoViewModel recurso = _recursoRepository.ConsultarPorLogin(login).ProjectTo<RecursoViewModel>(_mapper.ConfigurationProvider).FirstOrDefault();

            if (recurso == null)
            {
                return null;
            }

            valido = _cryptographyManager.VerifyPbkdf2(senha, recurso.Senha, recurso.Salt);

            return recurso;
        }
    }
}
