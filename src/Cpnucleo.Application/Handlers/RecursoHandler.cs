using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

namespace Cpnucleo.Application.Handlers;

public class RecursoHandler :
    IAsyncRequestHandler<CreateRecursoCommand, OperationResult>,
    IAsyncRequestHandler<GetRecursoQuery, RecursoViewModel>,
    IAsyncRequestHandler<ListRecursoQuery, IEnumerable<RecursoViewModel>>,
    IAsyncRequestHandler<RemoveRecursoCommand, OperationResult>,
    IAsyncRequestHandler<UpdateRecursoCommand, OperationResult>,
    IAsyncRequestHandler<AuthQuery, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICryptographyManager _cryptographyManager;

    public RecursoHandler(IUnitOfWork unitOfWork, IMapper mapper, ICryptographyManager cryptographyManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cryptographyManager = cryptographyManager;
    }

    public async ValueTask<OperationResult> InvokeAsync(CreateRecursoCommand request, CancellationToken cancellationToken)
    {
        _cryptographyManager.CryptPbkdf2(request.Recurso.Senha, out string senhaCrypt, out string salt);

        request.Recurso.Senha = senhaCrypt;
        request.Recurso.Salt = salt;

        await _unitOfWork.RecursoRepository.AddAsync(_mapper.Map<Recurso>(request.Recurso));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<RecursoViewModel> InvokeAsync(GetRecursoQuery request, CancellationToken cancellationToken)
    {
        RecursoViewModel result = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetAsync(request.Id));

        result.Senha = null;
        result.Salt = null;

        return result;
    }

    public async ValueTask<IEnumerable<RecursoViewModel>> InvokeAsync(ListRecursoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<RecursoViewModel> result = _mapper.Map<IEnumerable<RecursoViewModel>>(await _unitOfWork.RecursoRepository.AllAsync(request.GetDependencies));

        foreach (RecursoViewModel item in result)
        {
            item.Senha = null;
            item.Salt = null;
        }

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(RemoveRecursoCommand request, CancellationToken cancellationToken)
    {
        Recurso obj = await _unitOfWork.RecursoRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.RecursoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(UpdateRecursoCommand request, CancellationToken cancellationToken)
    {
        _cryptographyManager.CryptPbkdf2(request.Recurso.Senha, out string senhaCrypt, out string salt);

        request.Recurso.Senha = senhaCrypt;
        request.Recurso.Salt = salt;

        _unitOfWork.RecursoRepository.Update(_mapper.Map<Recurso>(request.Recurso));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<AuthResponse> InvokeAsync(AuthQuery request, CancellationToken cancellationToken)
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
