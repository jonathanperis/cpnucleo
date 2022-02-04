namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.ListImpedimentoTarefa;

public class ListImpedimentoTarefaHandler : IRequestHandler<ListImpedimentoTarefaQuery, ListImpedimentoTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListImpedimentoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListImpedimentoTarefaViewModel> Handle(ListImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefas = await _unitOfWork.ImpedimentoTarefaRepository.AllAsync(request.GetDependencies);

        if (impedimentoTarefas == null)
        {
            return new ListImpedimentoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<ImpedimentoTarefaDTO> result = _mapper.Map<IEnumerable<ImpedimentoTarefaDTO>>(impedimentoTarefas);

        return new ListImpedimentoTarefaViewModel { ImpedimentoTarefas = result, OperationResult = OperationResult.Success };
    }
}
