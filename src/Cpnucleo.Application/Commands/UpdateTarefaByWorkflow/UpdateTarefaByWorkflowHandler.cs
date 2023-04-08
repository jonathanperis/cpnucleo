using Cpnucleo.Shared.Commands.UpdateTarefaByWorkflow;

namespace Cpnucleo.Application.Commands.UpdateTarefaByWorkflow;

public sealed class UpdateTarefaByWorkflowHandler : IRequestHandler<UpdateTarefaByWorkflowCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTarefaByWorkflowHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateTarefaByWorkflowCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Tarefa tarefa = await _unitOfWork.TarefaRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (tarefa is null)
        {
            return OperationResult.NotFound;
        }

        Workflow workflow = await _unitOfWork.WorkflowRepository.Get(request.IdWorkflow).FirstOrDefaultAsync(cancellationToken);

        if (workflow is null)
        {
            return OperationResult.Failed;
        }

        tarefa.IdWorkflow = workflow.Id;
        tarefa.Workflow = workflow; //TODO: - VERIFICAR NECESSIDADE.

        _unitOfWork.TarefaRepository.Update(tarefa);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
