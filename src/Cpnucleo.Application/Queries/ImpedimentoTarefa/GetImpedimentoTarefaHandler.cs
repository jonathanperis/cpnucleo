namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public sealed class GetImpedimentoTarefaHandler : IRequestHandler<GetImpedimentoTarefaQuery, GetImpedimentoTarefaViewModel>
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
        ImpedimentoTarefaDTO impedimentoTarefa = await _unitOfWork.ImpedimentoTarefaRepository.Get(request.Id)
            .ProjectTo<ImpedimentoTarefaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (impedimentoTarefa is null)
        {
            return new GetImpedimentoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetImpedimentoTarefaViewModel { ImpedimentoTarefa = impedimentoTarefa, OperationResult = OperationResult.Success };
    }
}
