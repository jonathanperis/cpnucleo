using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Services
{
    public class SistemaAppService : CrudAppService<Sistema, SistemaViewModel>, ISistemaAppService
    {
        public SistemaAppService(IMapper mapper, ICrudRepository<Sistema> repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {

        }
    }
}
