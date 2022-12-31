namespace Cpnucleo.Application.Commands.Impedimento;

public sealed class UpdateImpedimentoHandler : IRequestHandler<UpdateImpedimentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateImpedimentoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateImpedimentoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Impedimento impedimento = await _unitOfWork.ImpedimentoRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (impedimento is null)
        {
            return OperationResult.NotFound;
        }

        impedimento.Nome = request.Nome;

        _unitOfWork.ImpedimentoRepository.Update(impedimento);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
