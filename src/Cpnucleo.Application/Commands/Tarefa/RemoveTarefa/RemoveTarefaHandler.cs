namespace Cpnucleo.Application.Commands.Tarefa.RemoveTarefa;

public class RemoveTarefaHandler : IRequestHandler<RemoveTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _unitOfWork.TarefaRepository.GetAsync(request.Id);

        if (tarefa == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.TarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
