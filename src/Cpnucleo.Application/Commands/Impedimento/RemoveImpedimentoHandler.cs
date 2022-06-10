namespace Cpnucleo.Application.Commands.Impedimento;

public class RemoveImpedimentoHandler : IRequestHandler<RemoveImpedimentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveImpedimentoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveImpedimentoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Impedimento impedimento = await _unitOfWork.ImpedimentoRepository.GetAsync(request.Id);

        if (impedimento == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ImpedimentoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
