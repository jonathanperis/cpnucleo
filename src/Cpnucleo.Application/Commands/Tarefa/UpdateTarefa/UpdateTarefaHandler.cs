namespace Cpnucleo.Application.Commands.Tarefa.UpdateTarefa;

public class UpdateTarefaHandler : IRequestHandler<UpdateTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateTarefaCommand request, CancellationToken cancellationToken)
    {
        var tarefa = await _unitOfWork.TarefaRepository.GetAsync(request.Id);

        if (tarefa == null)
        {
            return OperationResult.NotFound;
        }

        tarefa.Nome = request.Nome;
        tarefa.DataInicio = request.DataInicio;
        tarefa.DataTermino = request.DataTermino;
        tarefa.QtdHoras = request.QtdHoras;
        tarefa.Ativo = request.Ativo;
        tarefa.IdProjeto = request.IdProjeto;
        tarefa.IdWorkflow = request.IdWorkflow;
        tarefa.IdRecurso = request.IdRecurso;
        tarefa.IdTipoTarefa = request.IdTipoTarefa;

        _unitOfWork.TarefaRepository.Update(tarefa);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
