namespace Cpnucleo.Application.Queries.RecursoTarefa.ListRecursoTarefa;

public class ListRecursoTarefaHandler : IRequestHandler<ListRecursoTarefaQuery, ListRecursoTarefaViewModel>
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
        var recursoTarefas = await _unitOfWork.RecursoTarefaRepository.AllAsync(request.GetDependencies);

        if (recursoTarefas == null)
        {
            return new ListRecursoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<RecursoTarefaDTO> result = _mapper.Map<IEnumerable<RecursoTarefaDTO>>(recursoTarefas);

        return new ListRecursoTarefaViewModel { RecursoTarefas = result, OperationResult = OperationResult.Success };
    }
}
