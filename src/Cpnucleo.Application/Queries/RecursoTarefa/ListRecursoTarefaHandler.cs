namespace Cpnucleo.Application.Queries.RecursoTarefa;

public sealed class ListRecursoTarefaHandler : IRequestHandler<ListRecursoTarefaQuery, ListRecursoTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListRecursoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListRecursoTarefaViewModel> Handle(ListRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        List<RecursoTarefaDTO> recursoTarefas = await _unitOfWork.RecursoTarefaRepository.List(request.GetDependencies)
            .ProjectTo<RecursoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursoTarefas is null)
        {
            return new ListRecursoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoTarefaViewModel { RecursoTarefas = recursoTarefas, OperationResult = OperationResult.Success };
    }
}
