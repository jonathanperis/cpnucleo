using Cpnucleo.Shared.Commands.UpdateTipoTarefa;

namespace Cpnucleo.Application.Commands.UpdateTipoTarefa;

public sealed class UpdateTipoTarefaHandler : IRequestHandler<UpdateTipoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTipoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        TipoTarefa tipoTarefa = await _unitOfWork.TipoTarefaRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (tipoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        tipoTarefa.Nome = request.Nome;
        tipoTarefa.Image = request.Image;

        _unitOfWork.TipoTarefaRepository.Update(tipoTarefa);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
