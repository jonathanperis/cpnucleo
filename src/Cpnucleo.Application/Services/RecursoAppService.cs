using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Security.Interfaces;

namespace Cpnucleo.Application.Services
{
    public class RecursoAppService : AppService<Recurso, RecursoViewModel>, IRecursoAppService
    {
        protected readonly IRecursoRepository _recursoRepository;
        protected readonly ICryptographyManager _cryptographyManager;

        public RecursoAppService(IMapper mapper, IRepository<Recurso> repository, IUnitOfWork unitOfWork, IRecursoRepository recursoRepository, ICryptographyManager cryptographyManager)
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

        public RecursoViewModel ConsultarPorLogin(string login, string senha, out bool valido)
        {
            valido = false;

            RecursoViewModel recurso = _mapper.Map<RecursoViewModel>(_recursoRepository.ConsultarPorLogin(login));

            if (recurso == null)
            {
                return null;
            }

            if (!recurso.Ativo)
            {
                return null;
            }

            valido = _cryptographyManager.VerifyPbkdf2(senha, recurso.Senha, recurso.Salt);

            return recurso;
        }
    }
}
