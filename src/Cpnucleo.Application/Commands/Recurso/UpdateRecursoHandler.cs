using Cpnucleo.Domain.Common.Security.Interfaces;

namespace Cpnucleo.Application.Commands.Recurso;

public sealed class UpdateRecursoHandler : IRequestHandler<UpdateRecursoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICryptographyManager _cryptographyManager;

    public UpdateRecursoHandler(IUnitOfWork unitOfWork, ICryptographyManager cryptographyManager)
    {
        _unitOfWork = unitOfWork;
        _cryptographyManager = cryptographyManager;
    }

    public async Task<OperationResult> Handle(UpdateRecursoCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.Recurso recurso = await _unitOfWork.RecursoRepository.Get(request.Id).FirstOrDefaultAsync(cancellationToken);

        if (recurso is null)
        {
            return OperationResult.NotFound;
        }

        recurso.Nome = request.Nome;

        _cryptographyManager.CryptPbkdf2(request.Senha, out string senhaCrypt, out string salt);

        recurso.Senha = senhaCrypt;
        recurso.Salt = salt;

        _unitOfWork.RecursoRepository.Update(recurso);

        bool success = await _unitOfWork.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
