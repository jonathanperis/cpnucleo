using Cpnucleo.Application.Common.Repositories.Interfaces;

namespace Cpnucleo.Application.Common.Repositories.UoW;

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
