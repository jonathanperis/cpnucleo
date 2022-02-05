namespace Cpnucleo.Application.Commands.RecursoTarefa.UpdateRecursoTarefa;

public class UpdateRecursoTarefaHandler : IRequestHandler<UpdateRecursoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRecursoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        var recursoTarefa = await _unitOfWork.RecursoTarefaRepository.GetAsync(request.Id);

        if (recursoTarefa == null)
        {
            return OperationResult.NotFound;
        }

        recursoTarefa.IdRecurso = request.IdRecurso;
        recursoTarefa.IdTarefa = request.IdTarefa;

        _unitOfWork.RecursoTarefaRepository.Update(recursoTarefa);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
