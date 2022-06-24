using Cpnucleo.Shared.Commands.ImpedimentoTarefa;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Application.Commands.ImpedimentoTarefa;

public class RemoveImpedimentoTarefaHandler : IRequestHandler<RemoveImpedimentoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveImpedimentoTarefaHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.ImpedimentoTarefa impedimentoTarefa = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id);

        if (impedimentoTarefa == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ImpedimentoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
