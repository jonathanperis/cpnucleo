using Cpnucleo.Shared.Commands.Apontamento;
using Cpnucleo.Shared.Common.Models;

namespace Cpnucleo.Application.Commands.Apontamento;

public class RemoveApontamentoHandler : IRequestHandler<RemoveApontamentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveApontamentoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveApontamentoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Apontamento apontamento = await _unitOfWork.ApontamentoRepository.GetAsync(request.Id);

        if (apontamento == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ApontamentoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
