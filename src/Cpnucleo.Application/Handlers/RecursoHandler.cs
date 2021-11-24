using Cpnucleo.Infra.CrossCutting.Security.Interfaces;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

namespace Cpnucleo.Application.Handlers;

public class RecursoHandler :
    IRequestHandler<CreateRecursoCommand, OperationResult>,
    IRequestHandler<GetRecursoQuery, RecursoViewModel>,
    IRequestHandler<ListRecursoQuery, IEnumerable<RecursoViewModel>>,
    IRequestHandler<RemoveRecursoCommand, OperationResult>,
    IRequestHandler<UpdateRecursoCommand, OperationResult>
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

    public async Task<OperationResult> Handle(CreateRecursoCommand request, CancellationToken cancellationToken)
    {
        _cryptographyManager.CryptPbkdf2(request.Recurso.Senha, out string senhaCrypt, out string salt);

        request.Recurso.Senha = senhaCrypt;
        request.Recurso.Salt = salt;

        await _unitOfWork.RecursoRepository.AddAsync(_mapper.Map<Recurso>(request.Recurso));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<RecursoViewModel> Handle(GetRecursoQuery request, CancellationToken cancellationToken)
    {
        RecursoViewModel result = _mapper.Map<RecursoViewModel>(await _unitOfWork.RecursoRepository.GetAsync(request.Id));

        if (result != null)
        {
            result.Senha = null;
            result.Salt = null;
        }

        return result;
    }

    public async Task<IEnumerable<RecursoViewModel>> Handle(ListRecursoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<RecursoViewModel> result = _mapper.Map<IEnumerable<RecursoViewModel>>(await _unitOfWork.RecursoRepository.AllAsync(request.GetDependencies));

        foreach (RecursoViewModel item in result)
        {
            item.Senha = null;
            item.Salt = null;
        }

        return result;
    }

    public async Task<OperationResult> Handle(RemoveRecursoCommand request, CancellationToken cancellationToken)
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

    public async Task<OperationResult> Handle(UpdateRecursoCommand request, CancellationToken cancellationToken)
    {
        _cryptographyManager.CryptPbkdf2(request.Recurso.Senha, out string senhaCrypt, out string salt);

        request.Recurso.Senha = senhaCrypt;
        request.Recurso.Salt = salt;

        _unitOfWork.RecursoRepository.Update(_mapper.Map<Recurso>(request.Recurso));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
