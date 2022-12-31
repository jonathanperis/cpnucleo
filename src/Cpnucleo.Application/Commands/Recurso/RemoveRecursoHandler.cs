namespace Cpnucleo.Application.Commands.Recurso;

public sealed class RemoveRecursoHandler : IRequestHandler<RemoveRecursoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRecursoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveRecursoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Recurso recurso = await _unitOfWork.RecursoRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (recurso is null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.RecursoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
