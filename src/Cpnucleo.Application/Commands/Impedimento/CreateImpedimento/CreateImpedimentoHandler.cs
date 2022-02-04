namespace Cpnucleo.Application.Commands.Impedimento.CreateImpedimento;

public class CreateImpedimentoHandler : IRequestHandler<CreateImpedimentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateImpedimentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateImpedimentoCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ImpedimentoRepository.AddAsync(_mapper.Map<Domain.Entities.Impedimento>(request));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
