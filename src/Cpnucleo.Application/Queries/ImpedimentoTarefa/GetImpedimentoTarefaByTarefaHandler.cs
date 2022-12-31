namespace Cpnucleo.Application.Queries.ImpedimentoTarefa;

public sealed class GetImpedimentoTarefaByTarefaHandler : IRequestHandler<GetImpedimentoTarefaByTarefaQuery, GetImpedimentoTarefaByTarefaViewModel>
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
        List<ImpedimentoTarefaDTO> impedimentoTarefas = await _unitOfWork.ImpedimentoTarefaRepository.ListImpedimentoTarefaByTarefa(request.IdTarefa)
            .ProjectTo<ImpedimentoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (impedimentoTarefas is null)
        {
            return new GetImpedimentoTarefaByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetImpedimentoTarefaByTarefaViewModel { ImpedimentoTarefas = impedimentoTarefas, OperationResult = OperationResult.Success };
    }
}
