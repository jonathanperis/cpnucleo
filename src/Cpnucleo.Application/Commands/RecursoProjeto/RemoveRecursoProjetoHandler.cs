namespace Cpnucleo.Application.Commands.RecursoProjeto;

public sealed class RemoveRecursoProjetoHandler : IRequestHandler<RemoveRecursoProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemoveRecursoProjetoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(RemoveRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.RecursoProjeto recursoProjeto = await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id);

        if (recursoProjeto == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.RecursoProjetoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
