namespace Cpnucleo.Application.Commands.Projeto;

public sealed class RemoveProjetoHandler : IRequestHandler<RemoveProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveProjetoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveProjetoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Projeto projeto = await _unitOfWork.ProjetoRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (projeto is null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ProjetoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
