using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Requests.Auth;

namespace Cpnucleo.Application.Handlers;

public class AuthHandler :
    IRequestHandler<AuthRequest, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICryptographyManager _cryptographyManager;

    public AuthHandler(IUnitOfWork unitOfWork, IMapper mapper, ICryptographyManager cryptographyManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cryptographyManager = cryptographyManager;
    }

    public async Task<AuthResponse> Handle(AuthRequest request, CancellationToken cancellationToken)
    {
        AuthResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.Recurso = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetByLoginAsync(request.Auth.Usuario));

        if (result.Recurso == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        bool success = _cryptographyManager.VerifyPbkdf2(request.Auth.Senha, result.Recurso.Senha, result.Recurso.Salt);

        result.Recurso.Senha = null;
        result.Recurso.Salt = null;

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
