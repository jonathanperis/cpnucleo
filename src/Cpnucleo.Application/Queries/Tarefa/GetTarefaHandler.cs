namespace Cpnucleo.Application.Queries.Tarefa;

public sealed class GetTarefaHandler : IRequestHandler<GetTarefaQuery, GetTarefaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetTarefaViewModel> Handle(GetTarefaQuery request, CancellationToken cancellationToken)
    {
        TarefaDTO tarefa = await _unitOfWork.TarefaRepository.Get(request.Id)
            .ProjectTo<TarefaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (tarefa is null)
        {
            return new GetTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetTarefaViewModel { Tarefa = tarefa, OperationResult = OperationResult.Success };
    }
}
