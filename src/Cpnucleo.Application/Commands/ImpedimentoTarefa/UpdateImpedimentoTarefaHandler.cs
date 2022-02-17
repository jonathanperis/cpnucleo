namespace Cpnucleo.Application.Commands.ImpedimentoTarefa;

public class UpdateImpedimentoTarefaHandler : IRequestHandler<UpdateImpedimentoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateImpedimentoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        var impedimentoTarefa = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id);

        if (impedimentoTarefa == null)
        {
            return OperationResult.NotFound;
        }

        impedimentoTarefa.Descricao = request.Descricao;
        impedimentoTarefa.IdTarefa = request.IdTarefa;
        impedimentoTarefa.IdImpedimento = request.IdImpedimento;

        _unitOfWork.ImpedimentoTarefaRepository.Update(impedimentoTarefa);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
