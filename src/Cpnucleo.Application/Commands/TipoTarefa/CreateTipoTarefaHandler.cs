namespace Cpnucleo.Application.Commands.TipoTarefa;

public sealed class CreateTipoTarefaHandler : IRequestHandler<CreateTipoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateTipoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.TipoTarefaRepository.AddAsync(_mapper.Map<Domain.Entities.TipoTarefa>(request));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
