using Cpnucleo.Shared.Commands.UpdateRecursoTarefa;

namespace Cpnucleo.Application.Commands.UpdateRecursoTarefa;

public sealed class UpdateRecursoTarefaHandler : IRequestHandler<UpdateRecursoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRecursoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateRecursoTarefaCommand request, CancellationToken cancellationToken)
    {
        RecursoTarefa recursoTarefa = await _unitOfWork.RecursoTarefaRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (recursoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        recursoTarefa.IdRecurso = request.IdRecurso;
        recursoTarefa.IdTarefa = request.IdTarefa;

        _unitOfWork.RecursoTarefaRepository.Update(recursoTarefa);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
