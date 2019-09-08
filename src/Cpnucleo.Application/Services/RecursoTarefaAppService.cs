using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class RecursoTarefaAppService : AppService<RecursoTarefa, RecursoTarefaViewModel>, IRecursoTarefaAppService
    {
        public RecursoTarefaAppService(IMapper mapper, IRepository<RecursoTarefa> repository)
            : base(mapper, repository)
        {

        }

        public IQueryable<RecursoTarefaViewModel> ListarPoridRecurso(Guid idRecurso)
        {
            throw new NotImplementedException();
        }

        public IQueryable<RecursoTarefaViewModel> ListarPoridTarefa(Guid idTarefa)
        {
            throw new NotImplementedException();
        }
    }
}
