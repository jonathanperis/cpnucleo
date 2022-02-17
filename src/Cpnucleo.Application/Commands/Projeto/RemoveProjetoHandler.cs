namespace Cpnucleo.Application.Commands.Projeto;

public class RemoveProjetoHandler : IRequestHandler<RemoveProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveProjetoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = await _unitOfWork.ProjetoRepository.GetAsync(request.Id);

        if (projeto == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ProjetoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
