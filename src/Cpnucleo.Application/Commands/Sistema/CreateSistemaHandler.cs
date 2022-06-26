namespace Cpnucleo.Application.Commands.Sistema;

public class CreateSistemaHandler : IRequestHandler<CreateSistemaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSistemaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateSistemaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.SistemaRepository.AddAsync(_mapper.Map<Domain.Entities.Sistema>(request));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
