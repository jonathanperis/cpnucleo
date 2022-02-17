namespace Cpnucleo.Application.Commands.Impedimento;

public class UpdateImpedimentoHandler : IRequestHandler<UpdateImpedimentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateImpedimentoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateImpedimentoCommand request, CancellationToken cancellationToken)
    {
        var impedimento = await _unitOfWork.ImpedimentoRepository.GetAsync(request.Id);

        if (impedimento == null)
        {
            return OperationResult.NotFound;
        }

        impedimento.Nome = request.Nome;

        _unitOfWork.ImpedimentoRepository.Update(impedimento);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
