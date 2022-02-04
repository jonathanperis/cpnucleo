namespace Cpnucleo.Application.Commands.RecursoTarefa.RemoveRecursoTarefa;

public class RemoveRecursoTarefaHandler : IRequestHandler<RemoveRecursoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRecursoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        var recursoTarefa = await _unitOfWork.RecursoTarefaRepository.GetAsync(request.Id);

        if (recursoTarefa == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.RecursoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
