namespace Cpnucleo.Application.Queries.Tarefa;

public class ListTarefaHandler : IRequestHandler<ListTarefaQuery, ListTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListTarefaViewModel> Handle(ListTarefaQuery request, CancellationToken cancellationToken)
    {
        var tarefas = await _unitOfWork.TarefaRepository.AllAsync(request.GetDependencies);

        if (tarefas == null)
        {
            return new ListTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<TarefaDTO> result = _mapper.Map<IEnumerable<TarefaDTO>>(tarefas);

        await PreencherDadosAdicionaisAsync(result);

        return new ListTarefaViewModel { Tarefas = result, OperationResult = OperationResult.Success };
    }

    private async Task PreencherDadosAdicionaisAsync(IEnumerable<TarefaDTO> lista)
    {
        int colunas = await _unitOfWork.WorkflowRepository.GetQuantidadeColunasAsync();

        foreach (TarefaDTO item in lista)
        {
            item.Workflow.TamanhoColuna = _unitOfWork.WorkflowRepository.GetTamanhoColuna(colunas);

            item.HorasConsumidas = await _unitOfWork.ApontamentoRepository.GetTotalHorasByRecursoAsync(item.IdRecurso, item.Id);
            item.HorasRestantes = item.QtdHoras - item.HorasConsumidas;

            IEnumerable<Domain.Entities.ImpedimentoTarefa> impedimentos = await _unitOfWork.ImpedimentoTarefaRepository.GetImpedimentoTarefaByTarefaAsync(item.Id);

            if (impedimentos.Any())
            {
                item.TipoTarefa.Element = "warning-element";
            }
            else if (DateTime.Now.Date >= item.DataInicio && DateTime.Now.Date <= item.DataTermino)
            {
                item.TipoTarefa.Element = "success-element";
            }
            else if (DateTime.Now.Date > item.DataTermino)
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
