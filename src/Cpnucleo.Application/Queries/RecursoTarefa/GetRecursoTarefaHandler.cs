namespace Cpnucleo.Application.Queries.RecursoTarefa;

public sealed class GetRecursoTarefaHandler : IRequestHandler<GetRecursoTarefaQuery, GetRecursoTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecursoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetRecursoTarefaViewModel> Handle(GetRecursoTarefaQuery request, CancellationToken cancellationToken)
    {
        RecursoTarefaDTO recursoTarefa = await _unitOfWork.RecursoTarefaRepository.Get(request.Id)
            .ProjectTo<RecursoTarefaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (recursoTarefa is null)
        {
            return new GetRecursoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoTarefaViewModel { RecursoTarefa = recursoTarefa, OperationResult = OperationResult.Success };
    }
}
