namespace Cpnucleo.Application.Queries.RecursoTarefa;

public sealed class GetRecursoTarefaByTarefaHandler : IRequestHandler<GetRecursoTarefaByTarefaQuery, GetRecursoTarefaByTarefaViewModel>
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
        List<RecursoTarefaDTO> recursoTarefas = await _unitOfWork.RecursoTarefaRepository.ListRecursoTarefaByTarefa(request.IdTarefa)
            .ProjectTo<RecursoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursoTarefas is null)
        {
            return new GetRecursoTarefaByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoTarefaByTarefaViewModel { RecursoTarefas = recursoTarefas, OperationResult = OperationResult.Success };
    }
}
