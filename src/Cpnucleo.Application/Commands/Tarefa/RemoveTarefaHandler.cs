namespace Cpnucleo.Application.Commands.Tarefa;

public sealed class RemoveTarefaHandler : IRequestHandler<RemoveTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveTarefaCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Tarefa tarefa = await _unitOfWork.TarefaRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (tarefa is null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.TarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
