namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.GetByTarefa;

public class GetImpedimentoTarefaByTarefaHandler : IRequestHandler<GetImpedimentoTarefaByTarefaQuery, GetImpedimentoTarefaByTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetImpedimentoTarefaByTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetImpedimentoTarefaByTarefaViewModel> Handle(GetImpedimentoTarefaByTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefas = await _unitOfWork.ImpedimentoTarefaRepository.GetImpedimentoTarefaByTarefaAsync(request.IdTarefa);

        if (impedimentoTarefas == null)
        {
            return new GetImpedimentoTarefaByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<ImpedimentoTarefaDTO> result = _mapper.Map<IEnumerable<ImpedimentoTarefaDTO>>(impedimentoTarefas);

        return new GetImpedimentoTarefaByTarefaViewModel { ImpedimentoTarefas = result, OperationResult = OperationResult.Success };
    }
}
