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
        IEnumerable<Domain.Entities.TipoTarefa> tipoTarefas = await _unitOfWork.TipoTarefaRepository.AllAsync(request.GetDependencies);

        if (tipoTarefas == null)
        {
            return new ListTipoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<TipoTarefaDTO> result = _mapper.Map<IEnumerable<TipoTarefaDTO>>(tipoTarefas);

        return new ListTipoTarefaViewModel { TipoTarefas = result, OperationResult = OperationResult.Success };
    }
}
