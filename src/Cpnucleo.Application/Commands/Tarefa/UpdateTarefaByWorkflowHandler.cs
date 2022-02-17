namespace Cpnucleo.Application.Commands.Tarefa;

public class UpdateTarefaByWorkflowHandler : IRequestHandler<UpdateTarefaByWorkflowCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTarefaByWorkflowHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateTarefaByWorkflowCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _unitOfWork.TarefaRepository.GetAsync(request.Id);

        if (tarefa == null)
        {
            return OperationResult.NotFound;
        }

        var workflow = await _unitOfWork.WorkflowRepository.GetAsync(request.IdWorkflow);

        if (workflow == null)
        {
            return OperationResult.Failed;
        }

        tarefa.IdWorkflow = workflow.Id;
        tarefa.Workflow = workflow; //TODO: - VERIFICAR NECESSIDADE.

        _unitOfWork.TarefaRepository.Update(tarefa);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
