using Cpnucleo.Shared.Commands.RemoveRecursoProjeto;

namespace Cpnucleo.Application.Commands.RemoveRecursoProjeto;

public sealed class RemoveRecursoProjetoHandler : IRequestHandler<RemoveRecursoProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRecursoProjetoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.RecursoProjeto recursoProjeto = await _unitOfWork.RecursoProjetoRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (recursoProjeto is null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.RecursoProjetoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
