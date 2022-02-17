namespace Cpnucleo.Application.Commands.Tarefa;

public class CreateTarefaHandler : IRequestHandler<CreateTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateTarefaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.TarefaRepository.AddAsync(_mapper.Map<Domain.Entities.Tarefa>(request));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
