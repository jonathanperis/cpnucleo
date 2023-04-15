namespace Cpnucleo.Application.Queries.ListTarefaByRecurso;

public sealed class ListTarefaByRecursoQueryHandler : IRequestHandler<ListTarefaByRecursoQuery, ListTarefaByRecursoViewModel>
{
    private readonly IApplicationDbContext _context;

    public ListTarefaByRecursoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<ListTarefaByRecursoViewModel> Handle(ListTarefaByRecursoQuery request, CancellationToken cancellationToken)
    {
        var tarefas = await _context.Tarefas
            .AsNoTracking()
            .Include(x => x.Projeto)
            .Include(x => x.Recurso)
            .Include(x => x.Workflow)
            .Include(x => x.TipoTarefa)
            .Where(x => x.IdRecurso == request.IdRecurso && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        if (tarefas is null)
        {
            return new ListTarefaByRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        await PreencherDadosAdicionaisAsync(tarefas, cancellationToken);

        return new ListTarefaByRecursoViewModel { Tarefas = tarefas, OperationResult = OperationResult.Success };
    }

    private async Task PreencherDadosAdicionaisAsync(List<TarefaDTO> lista, CancellationToken cancellationToken)
    {
        var colunas = _context.Workflows.Where(x => x.Ativo).Count();

        foreach (TarefaDTO item in lista)
        {
            item.Workflow.TamanhoColuna = Workflow.GetTamanhoColuna(colunas);

            item.HorasConsumidas = _context.Apontamentos
                .Where(x => x.IdRecurso == item.IdRecurso && x.IdTarefa == item.Id && x.Ativo)
                .Sum(x => x.QtdHoras);

            item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

            var impedimentos = await _context.ImpedimentoTarefas
                .Where(x => x.IdTarefa == item.Id && x.Ativo)
                .OrderBy(x => x.DataInclusao)
                .Select(x => x.MapToDto())
                .ToListAsync(cancellationToken);

            if (impedimentos.Any())
            {
                item.TipoTarefa.Element = TipoTarefaConstants.WarningElement;
            }
            else if (DateTime.UtcNow.Date >= item.DataInicio && DateTime.UtcNow.Date <= item.DataTermino)
            {
                item.TipoTarefa.Element = TipoTarefaConstants.SuccessElement;
            }
            else if (DateTime.UtcNow.Date > item.DataTermino)
            {
                item.TipoTarefa.Element = TipoTarefaConstants.DangerElement;
            }
            else
            {
                item.TipoTarefa.Element = TipoTarefaConstants.InfoElement;
            }
        }
    }
}