namespace Cpnucleo.Application.Commands.ImpedimentoTarefa;

public sealed class UpdateImpedimentoTarefaHandler : IRequestHandler<UpdateImpedimentoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateImpedimentoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ImpedimentoTarefa impedimentoTarefa = await _unitOfWork.ImpedimentoTarefaRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (impedimentoTarefa is null)
        {
            return OperationResult.NotFound;
        }

        impedimentoTarefa.Descricao = request.Descricao;
        impedimentoTarefa.IdTarefa = request.IdTarefa;
        impedimentoTarefa.IdImpedimento = request.IdImpedimento;

        _unitOfWork.ImpedimentoTarefaRepository.Update(impedimentoTarefa);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
