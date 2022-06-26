namespace Cpnucleo.Application.Queries.Tarefa;

public class GetTarefaHandler : IRequestHandler<GetTarefaQuery, GetTarefaViewModel>
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
        Domain.Entities.Tarefa tarefa = await _unitOfWork.TarefaRepository.GetAsync(request.Id);

        if (tarefa == null)
        {
            return new GetTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        TarefaDTO result = _mapper.Map<TarefaDTO>(tarefa);

        return new GetTarefaViewModel { Tarefa = result, OperationResult = OperationResult.Success };
    }
}
