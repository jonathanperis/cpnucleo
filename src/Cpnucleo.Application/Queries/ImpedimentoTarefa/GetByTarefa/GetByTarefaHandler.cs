namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.GetByTarefa;

public class GetByTarefaHandler : IRequestHandler<GetByTarefaQuery, GetByTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetByTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetByTarefaViewModel> Handle(GetByTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefas = await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(request.IdTarefa);

        if (impedimentoTarefas == null)
        {
            return new GetByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<ImpedimentoTarefaDTO> result = _mapper.Map<IEnumerable<ImpedimentoTarefaDTO>>(impedimentoTarefas);

        return new GetByTarefaViewModel { ImpedimentoTarefas = result, OperationResult = OperationResult.Success };
    }
}
