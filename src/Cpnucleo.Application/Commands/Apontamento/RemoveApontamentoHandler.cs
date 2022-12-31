namespace Cpnucleo.Application.Commands.Apontamento;

public sealed class RemoveApontamentoHandler : IRequestHandler<RemoveApontamentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveApontamentoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveApontamentoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Apontamento apontamento = await _unitOfWork.ApontamentoRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (apontamento is null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ApontamentoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
