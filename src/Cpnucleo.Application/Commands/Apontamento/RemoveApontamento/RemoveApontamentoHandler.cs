namespace Cpnucleo.Application.Commands.Apontamento.RemoveApontamento;

public class RemoveApontamentoHandler : IRequestHandler<RemoveApontamentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveApontamentoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveApontamentoCommand request, CancellationToken cancellationToken)
    {
        var apontamento = await _unitOfWork.ApontamentoRepository.GetAsync(request.Id);

        if (apontamento == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ApontamentoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
