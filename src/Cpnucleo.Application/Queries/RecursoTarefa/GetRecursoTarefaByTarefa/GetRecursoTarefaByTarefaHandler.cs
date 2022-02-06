namespace Cpnucleo.Application.Queries.RecursoTarefa.GetByTarefa;

public class GetRecursoTarefaByTarefaHandler : IRequestHandler<GetRecursoTarefaByTarefaQuery, GetRecursoTarefaByTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecursoTarefaByTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetRecursoTarefaByTarefaViewModel> Handle(GetRecursoTarefaByTarefaQuery request, CancellationToken cancellationToken)
    {
        var recursoTarefas = await _unitOfWork.RecursoTarefaRepository.GetRecursoTarefaByTarefaAsync(request.IdTarefa);

        if (recursoTarefas == null)
        {
            return new GetRecursoTarefaByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<RecursoTarefaDTO> result = _mapper.Map<IEnumerable<RecursoTarefaDTO>>(recursoTarefas);

        return new GetRecursoTarefaByTarefaViewModel { RecursoTarefas = result, OperationResult = OperationResult.Success };
    }
}
