namespace Cpnucleo.Application.Queries.TipoTarefa;

public sealed class ListTipoTarefaHandler : IRequestHandler<ListTipoTarefaQuery, ListTipoTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListTipoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListTipoTarefaViewModel> Handle(ListTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        List<TipoTarefaDTO> tipoTarefas = await _unitOfWork.TipoTarefaRepository.All(request.GetDependencies)
            .ProjectTo<TipoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (tipoTarefas is null)
        {
            return new ListTipoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListTipoTarefaViewModel { TipoTarefas = tipoTarefas, OperationResult = OperationResult.Success };
    }
}
