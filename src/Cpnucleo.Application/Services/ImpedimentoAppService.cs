using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Services
{
    public class ImpedimentoAppService : CrudAppService<Impedimento, ImpedimentoViewModel>, IImpedimentoAppService
    {
        public ImpedimentoAppService(IMapper mapper, ICrudRepository<Impedimento> repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {

        }
    }
}
