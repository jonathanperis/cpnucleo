namespace Cpnucleo.Application.Commands.RecursoProjeto;

public sealed class UpdateRecursoProjetoHandler : IRequestHandler<UpdateRecursoProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRecursoProjetoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.RecursoProjeto recursoProjeto = await _unitOfWork.RecursoProjetoRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (recursoProjeto is null)
        {
            return OperationResult.NotFound;
        }

        recursoProjeto.IdRecurso = request.IdRecurso;
        recursoProjeto.IdProjeto = request.IdProjeto;

        _unitOfWork.RecursoProjetoRepository.Update(recursoProjeto);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
