namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public sealed class GetImpedimentoTarefaByTarefaHandler : IRequestHandler<ListImpedimentoTarefaByTarefaQuery, ListImpedimentoTarefaByTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetImpedimentoTarefaByTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListImpedimentoTarefaByTarefaViewModel> Handle(ListImpedimentoTarefaByTarefaQuery request, CancellationToken cancellationToken)
    {
        List<ImpedimentoTarefaDTO> impedimentoTarefas = await _unitOfWork.ImpedimentoTarefaRepository.ListImpedimentoTarefaByTarefa(request.IdTarefa)
            .ProjectTo<ImpedimentoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (impedimentoTarefas is null)
        {
            return new ListImpedimentoTarefaByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListImpedimentoTarefaByTarefaViewModel { ImpedimentoTarefas = impedimentoTarefas, OperationResult = OperationResult.Success };
    }
}
