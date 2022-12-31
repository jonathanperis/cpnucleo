namespace Cpnucleo.Application.Commands.TipoTarefa;

public sealed class RemoveTipoTarefaHandler : IRequestHandler<RemoveTipoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveTipoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.TipoTarefa tipoTarefa = await _unitOfWork.TipoTarefaRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (tipoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.TipoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
