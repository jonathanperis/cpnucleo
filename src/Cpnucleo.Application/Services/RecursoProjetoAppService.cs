using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class RecursoProjetoAppService : AppService<RecursoProjeto, RecursoProjetoViewModel>, IRecursoProjetoAppService
    {
        public RecursoProjetoAppService(IMapper mapper, IRepository<RecursoProjeto> repository)
            : base(mapper, repository)
        {

        }

        public IQueryable<RecursoProjetoViewModel> ListarPoridProjeto(Guid idProjeto)
        {
            throw new NotImplementedException();
        }
    }
}
