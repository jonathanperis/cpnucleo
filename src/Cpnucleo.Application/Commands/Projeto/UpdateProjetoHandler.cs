namespace Cpnucleo.Application.Commands.Projeto;

public sealed class UpdateProjetoHandler : IRequestHandler<UpdateProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjetoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateProjetoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Projeto projeto = await _unitOfWork.ProjetoRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (projeto is null)
        {
            return OperationResult.NotFound;
        }

        projeto.Nome = request.Nome;
        projeto.IdSistema = request.IdSistema;

        _unitOfWork.ProjetoRepository.Update(projeto);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
