namespace Cpnucleo.Application.Commands.Projeto.UpdateProjeto;

public class UpdateProjetoHandler : IRequestHandler<UpdateProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProjetoHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<OperationResult> Handle(UpdateProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = await _unitOfWork.ProjetoRepository.GetAsync(request.Id);

        if (projeto == null)
        {
            return OperationResult.NotFound;
        }

        projeto.Nome = request.Nome;
        projeto.IdSistema = request.IdSistema;

        _unitOfWork.ProjetoRepository.Update(projeto);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
