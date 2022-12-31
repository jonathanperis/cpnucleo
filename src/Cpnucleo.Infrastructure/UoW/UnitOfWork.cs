using Cpnucleo.Domain.Common.Repositories.UoW;
using Cpnucleo.Infrastructure.Context;
using Cpnucleo.Infrastructure.Repositories;

namespace Cpnucleo.Infrastructure.UoW;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly CpnucleoDbContext _context;

    public UnitOfWork(CpnucleoDbContext context)
    {
        _context = context;
    }

    private IApontamentoRepository _apontamentoRepository;
    private IGenericRepository<Impedimento> _impedimentoRepository;
    private IImpedimentoTarefaRepository _impedimentoTarefaRepository;
    private IGenericRepository<Projeto> _projetoRepository;
    private IRecursoRepository _recursoRepository;
    private IRecursoProjetoRepository _recursoProjetoRepository;
    private IRecursoTarefaRepository _recursoTarefaRepository;
    private IGenericRepository<Sistema> _sistemaRepository;
    private ITarefaRepository _tarefaRepository;
    private IGenericRepository<TipoTarefa> _tipoTarefaRepository;
    private IWorkflowRepository _workflowRepository;

    public IApontamentoRepository ApontamentoRepository
    {
        get
        {
            if (_apontamentoRepository == null)
            {
                _apontamentoRepository = new ApontamentoRepository(_context);
            }

            return _apontamentoRepository;
        }
    }

    public IGenericRepository<Impedimento> ImpedimentoRepository
    {
        get
        {
            if (_impedimentoRepository == null)
            {
                _impedimentoRepository = new GenericRepository<Impedimento>(_context);
            }

            return _impedimentoRepository;
        }
    }

    public IImpedimentoTarefaRepository ImpedimentoTarefaRepository
    {
        get
        {
            if (_impedimentoTarefaRepository == null)
            {
                _impedimentoTarefaRepository = new ImpedimentoTarefaRepository(_context);
            }

            return _impedimentoTarefaRepository;
        }
    }

    public IGenericRepository<Projeto> ProjetoRepository
    {
        get
        {
            if (_projetoRepository == null)
            {
                _projetoRepository = new GenericRepository<Projeto>(_context);
            }

            return _projetoRepository;
        }
    }

    public IRecursoRepository RecursoRepository
    {
        get
        {
            if (_recursoRepository == null)
            {
                _recursoRepository = new RecursoRepository(_context);
            }

            return _recursoRepository;
        }
    }

    public IRecursoProjetoRepository RecursoProjetoRepository
    {
        get
        {
            if (_recursoProjetoRepository == null)
            {
                _recursoProjetoRepository = new RecursoProjetoRepository(_context);
            }

            return _recursoProjetoRepository;
        }
    }

    public IRecursoTarefaRepository RecursoTarefaRepository
    {
        get
        {
            if (_recursoTarefaRepository == null)
            {
                _recursoTarefaRepository = new RecursoTarefaRepository(_context);
            }

            return _recursoTarefaRepository;
        }
    }

    public IGenericRepository<Sistema> SistemaRepository
    {
        get
        {
            if (_sistemaRepository == null)
            {
                _sistemaRepository = new GenericRepository<Sistema>(_context);
            }

            return _sistemaRepository;
        }
    }

    public ITarefaRepository TarefaRepository
    {
        get
        {
            if (_tarefaRepository == null)
            {
                _tarefaRepository = new TarefaRepository(_context);
            }

            return _tarefaRepository;
        }
    }

    public IGenericRepository<TipoTarefa> TipoTarefaRepository
    {
        get
        {
            if (_tipoTarefaRepository == null)
            {
                _tipoTarefaRepository = new GenericRepository<TipoTarefa>(_context);
            }

            return _tipoTarefaRepository;
        }
    }

    public IWorkflowRepository WorkflowRepository
    {
        get
        {
            if (_workflowRepository == null)
            {
                _workflowRepository = new WorkflowRepository(_context);
            }

            return _workflowRepository;
        }
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken) > 0;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
