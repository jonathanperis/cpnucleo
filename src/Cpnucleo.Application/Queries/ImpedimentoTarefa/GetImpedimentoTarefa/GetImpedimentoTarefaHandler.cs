namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;

public class GetImpedimentoTarefaHandler : IRequestHandler<GetImpedimentoTarefaQuery, GetImpedimentoTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetImpedimentoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetImpedimentoTarefaViewModel> Handle(GetImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefa = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id);

        if (impedimentoTarefa == null)
        {
            return new GetImpedimentoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        ImpedimentoTarefaDTO result = _mapper.Map<ImpedimentoTarefaDTO>(impedimentoTarefa);

        return new GetImpedimentoTarefaViewModel { ImpedimentoTarefa = result, OperationResult = OperationResult.Success };
    }
}
