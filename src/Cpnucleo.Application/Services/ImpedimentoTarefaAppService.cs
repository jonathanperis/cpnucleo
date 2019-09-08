using AutoMapper;
using Cpnucleo.Application.Interfaces;
using Cpnucleo.Application.ViewModels;
using Cpnucleo.Domain.Interfaces;
using Cpnucleo.Domain.Models;
using System;
using System.Linq;

namespace Cpnucleo.Application.Services
{
    public class ImpedimentoTarefaAppService : AppService<ImpedimentoTarefa, ImpedimentoTarefaViewModel>, IImpedimentoTarefaAppService
    {
        public ImpedimentoTarefaAppService(IMapper mapper, IRepository<ImpedimentoTarefa> repository)
            : base(mapper, repository)
        {

        }

        public IQueryable<ImpedimentoTarefaViewModel> ListarPoridTarefa(Guid idTarefa)
        {
            throw new NotImplementedException();
        }
    }
}
