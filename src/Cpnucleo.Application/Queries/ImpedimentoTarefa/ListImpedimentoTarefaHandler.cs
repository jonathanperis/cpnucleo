namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public sealed class ListImpedimentoTarefaHandler : IRequestHandler<ListImpedimentoTarefaQuery, ListImpedimentoTarefaViewModel>
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
        List<ImpedimentoTarefaDTO> impedimentoTarefas = await _unitOfWork.ImpedimentoTarefaRepository.All(request.GetDependencies)
            .ProjectTo<ImpedimentoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (impedimentoTarefas is null)
        {
            return new ListImpedimentoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListImpedimentoTarefaViewModel { ImpedimentoTarefas = impedimentoTarefas, OperationResult = OperationResult.Success };
    }
}
