using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces.Services;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Services
{
    internal class RecursoAppService : CrudAppService<Recurso, RecursoViewModel>, IRecursoAppService
    {
        private readonly IRecursoService _recursoService;

        public RecursoAppService(IMapper mapper, IRecursoService recursoService)
            : base(mapper, recursoService)
        {
            _recursoService = recursoService;
        }

        public RecursoViewModel Autenticar(string login, string senha, out bool valido)
        {
            return _mapper.Map<RecursoViewModel>(_recursoService.Autenticar(login, senha, out valido));
        }
    }
}
