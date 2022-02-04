namespace Cpnucleo.Application.Commands.Recurso.RemoveRecurso;

public class RemoveRecursoHandler : IRequestHandler<RemoveRecursoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRecursoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveRecursoCommand request, CancellationToken cancellationToken)
    {
        var recurso = await _unitOfWork.RecursoRepository.GetAsync(request.Id);

        if (recurso == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.RecursoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
