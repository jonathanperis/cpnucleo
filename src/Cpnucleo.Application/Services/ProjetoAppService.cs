using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;

namespace Cpnucleo.Application.Services
{
    public class ProjetoAppService : CrudAppService<Projeto, ProjetoViewModel>, IProjetoAppService
    {
        public ProjetoAppService(IMapper mapper, ICrudRepository<Projeto> repository, IUnitOfWork unitOfWork)
            : base(mapper, repository, unitOfWork)
        {

        }
    }
}
