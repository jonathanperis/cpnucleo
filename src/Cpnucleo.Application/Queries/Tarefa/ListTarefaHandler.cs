namespace Cpnucleo.Application.Queries.Tarefa;

public sealed class ListTarefaHandler : IRequestHandler<ListTarefaQuery, ListTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IWorkflowService _workflowService;

    public ListTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper, IWorkflowService workflowService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _workflowService = workflowService;
    }

    public async Task<ListTarefaViewModel> Handle(ListTarefaQuery request, CancellationToken cancellationToken)
    {
        List<TarefaDTO> tarefas = await _unitOfWork.TarefaRepository.All(request.GetDependencies)
            .ProjectTo<TarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (tarefas is null)
        {
            return new ListTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        await PreencherDadosAdicionaisAsync(tarefas, cancellationToken);

        return new ListTarefaViewModel { Tarefas = tarefas, OperationResult = OperationResult.Success };
    }

    private async Task PreencherDadosAdicionaisAsync(List<TarefaDTO> lista, CancellationToken cancellationToken)
    {
        int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

        foreach (TarefaDTO item in lista)
        {
            item.Workflow.TamanhoColuna = _workflowService.GetTamanhoColuna(colunas);

            item.HorasConsumidas = await _unitOfWork.ApontamentoRepository.GetTotalHorasByRecursoAsync(item.IdRecurso, item.Id);
            item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

            List<ImpedimentoTarefaDTO> impedimentos = await _unitOfWork.ImpedimentoTarefaRepository.GetImpedimentoTarefaByTarefa(item.Id)
                .ProjectTo<ImpedimentoTarefaDTO>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (impedimentos.Any())
            {
                item.TipoTarefa.Element = "warning-element";
            }
            else if (DateTime.UtcNow.Date >= item.DataInicio && DateTime.UtcNow.Date <= item.DataTermino)
            {
                item.TipoTarefa.Element = "success-element";
            }
            else if (DateTime.UtcNow.Date > item.DataTermino)
            {
                item.TipoTarefa.Element = "danger-element";
            }
            else
            {
                item.TipoTarefa.Element = "info-element";
            }
        }
    }
}
