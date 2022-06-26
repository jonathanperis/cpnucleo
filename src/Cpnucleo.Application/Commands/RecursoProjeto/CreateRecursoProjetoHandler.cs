namespace Cpnucleo.Application.Commands.RecursoProjeto;

public class CreateRecursoProjetoHandler : IRequestHandler<CreateRecursoProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateRecursoProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.RecursoProjetoRepository.AddAsync(_mapper.Map<Domain.Entities.RecursoProjeto>(request));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
