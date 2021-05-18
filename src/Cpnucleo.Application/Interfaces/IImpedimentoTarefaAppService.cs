using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.Interfaces
{
    public interface IImpedimentoTarefaRepository : IGenericRepository<ImpedimentoTarefaViewModel>
    {
        Task<IEnumerable<ImpedimentoTarefaViewModel>> GetByTarefaAsync(Guid idTarefa);
    }
}