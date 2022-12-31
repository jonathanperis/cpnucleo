namespace Cpnucleo.Application.Commands.RecursoTarefa;

public sealed class RemoveRecursoTarefaHandler : IRequestHandler<RemoveRecursoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRecursoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.RecursoTarefa recursoTarefa = await _unitOfWork.RecursoTarefaRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (recursoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.RecursoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
