using Cpnucleo.Domain.Entities;
using Cpnucleo.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Cpnucleo.Domain.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IApontamentoRepository ApontamentoRepository { get; }
        IGenericRepository<Impedimento> ImpedimentoRepository { get; }
        IImpedimentoTarefaRepository ImpedimentoTarefaRepository { get; }
        IGenericRepository<Projeto> ProjetoRepository { get; }
        IRecursoRepository RecursoRepository { get; }
        IRecursoProjetoRepository RecursoProjetoRepository { get; }
        IRecursoTarefaRepository RecursoTarefaRepository { get; }
        IGenericRepository<Sistema> SistemaRepository { get; }
        ITarefaRepository TarefaRepository { get; }
        IGenericRepository<TipoTarefa> TipoTarefaRepository { get; }
        IWorkflowRepository WorkflowRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
