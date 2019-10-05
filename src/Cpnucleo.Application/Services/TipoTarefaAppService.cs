using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Services
{
    public class TipoTarefaAppService : CrudAppService<TipoTarefa, TipoTarefaViewModel>, ITipoTarefaAppService
    {
        public TipoTarefaAppService(IMapper mapper, ICrudRepository<TipoTarefa> repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {

        }
    }
}
