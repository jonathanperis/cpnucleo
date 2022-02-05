namespace Cpnucleo.Application.Commands.TipoTarefa.UpdateTipoTarefa;

public class UpdateTipoTarefaHandler : IRequestHandler<UpdateTipoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateTipoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        var tipoTarefa = await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id);

        if (tipoTarefa == null)
        {
            return OperationResult.NotFound;
        }

        tipoTarefa.Nome = request.Nome;
        tipoTarefa.Image = request.Image;

        _unitOfWork.TipoTarefaRepository.Update(tipoTarefa);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
