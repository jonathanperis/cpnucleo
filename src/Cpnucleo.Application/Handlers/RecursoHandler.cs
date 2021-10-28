using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.RemoveRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.GetRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.ListRecurso;

namespace Cpnucleo.Application.Handlers;

public class RecursoHandler :
    IAsyncRequestHandler<CreateRecursoCommand, CreateRecursoResponse>,
    IAsyncRequestHandler<GetRecursoQuery, GetRecursoResponse>,
    IAsyncRequestHandler<ListRecursoQuery, ListRecursoResponse>,
    IAsyncRequestHandler<RemoveRecursoCommand, RemoveRecursoResponse>,
    IAsyncRequestHandler<UpdateRecursoCommand, UpdateRecursoResponse>,
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

    public async ValueTask<CreateRecursoResponse> InvokeAsync(CreateRecursoCommand request, CancellationToken cancellationToken)
    {
        CreateRecursoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        _cryptographyManager.CryptPbkdf2(request.Recurso.Senha, out string senhaCrypt, out string salt);

        request.Recurso.Senha = senhaCrypt;
        request.Recurso.Salt = salt;

        Recurso obj = await _unitOfWork.RecursoRepository.AddAsync(_mapper.Map<Recurso>(request.Recurso));
        result.Recurso = _mapper.Map<RecursoViewModel>(obj);

        await _unitOfWork.SaveChangesAsync();

        result.Recurso.Senha = null;
        result.Recurso.Salt = null;

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<GetRecursoResponse> InvokeAsync(GetRecursoQuery request, CancellationToken cancellationToken)
    {
        GetRecursoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.Recurso = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetAsync(request.Id));

        if (result.Recurso == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        result.Recurso.Senha = null;
        result.Recurso.Salt = null;

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<ListRecursoResponse> InvokeAsync(ListRecursoQuery request, CancellationToken cancellationToken)
    {
        ListRecursoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.Recursos = _mapper.Map<IEnumerable<RecursoViewModel>>(await _unitOfWork.RecursoRepository.AllAsync(request.GetDependencies));

        foreach (RecursoViewModel item in result.Recursos)
        {
            item.Senha = null;
            item.Salt = null;
        }

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<RemoveRecursoResponse> InvokeAsync(RemoveRecursoCommand request, CancellationToken cancellationToken)
    {
        RemoveRecursoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        Recurso obj = await _unitOfWork.RecursoRepository.GetAsync(request.Id);

        if (obj == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        await _unitOfWork.RecursoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<UpdateRecursoResponse> InvokeAsync(UpdateRecursoCommand request, CancellationToken cancellationToken)
    {
        UpdateRecursoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        _cryptographyManager.CryptPbkdf2(request.Recurso.Senha, out string senhaCrypt, out string salt);

        request.Recurso.Senha = senhaCrypt;
        request.Recurso.Salt = salt;

        _unitOfWork.RecursoRepository.Update(_mapper.Map<Recurso>(request.Recurso));

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<AuthResponse> InvokeAsync(AuthQuery request, CancellationToken cancellationToken)
    {
        AuthResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.Recurso = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetByLoginAsync(request.Login));

        if (result.Recurso == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        bool success = _cryptographyManager.VerifyPbkdf2(request.Senha, result.Recurso.Senha, result.Recurso.Salt);

        result.Recurso.Senha = null;
        result.Recurso.Salt = null;

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
