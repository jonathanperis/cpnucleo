namespace Cpnucleo.Application.Queries.TipoTarefa;

public sealed class GetTipoTarefaHandler : IRequestHandler<GetTipoTarefaQuery, GetTipoTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTipoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetTipoTarefaViewModel> Handle(GetTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        TipoTarefaDTO tipoTarefa = await _unitOfWork.TipoTarefaRepository.Get(request.Id)
            .ProjectTo<TipoTarefaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (tipoTarefa is null)
        {
            return new GetTipoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetTipoTarefaViewModel { TipoTarefa = tipoTarefa, OperationResult = OperationResult.Success };
    }
}
