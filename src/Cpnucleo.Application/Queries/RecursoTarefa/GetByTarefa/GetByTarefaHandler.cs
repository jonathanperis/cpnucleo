namespace Cpnucleo.Application.Queries.RecursoTarefa.GetByTarefa;

public class GetByTarefaHandler : IRequestHandler<GetByTarefaQuery, GetByTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetByTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetByTarefaViewModel> Handle(GetByTarefaQuery request, CancellationToken cancellationToken)
    {
        var recursoTarefas = await _unitOfWork.RecursoTarefaRepository.GetByTarefaAsync(request.IdTarefa);

        if (recursoTarefas == null)
        {
            return new GetByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<RecursoTarefaDTO> result = _mapper.Map<IEnumerable<RecursoTarefaDTO>>(recursoTarefas);

        return new GetByTarefaViewModel { RecursoTarefas = result, OperationResult = OperationResult.Success };
    }
}
