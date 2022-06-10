namespace Cpnucleo.Application.Queries.RecursoTarefa;

public class GetRecursoTarefaHandler : IRequestHandler<GetRecursoTarefaQuery, GetRecursoTarefaViewModel>
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
        Domain.Entities.RecursoTarefa recursoTarefa = await _unitOfWork.RecursoTarefaRepository.GetAsync(request.Id);

        if (recursoTarefa == null)
        {
            return new GetRecursoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        RecursoTarefaDTO result = _mapper.Map<RecursoTarefaDTO>(recursoTarefa);

        return new GetRecursoTarefaViewModel { RecursoTarefa = result, OperationResult = OperationResult.Success };
    }
}
