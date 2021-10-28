using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.RemoveRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetRecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.ListRecursoProjeto;

namespace Cpnucleo.Application.Handlers;

public class RecursoProjetoHandler :
    IAsyncRequestHandler<CreateRecursoProjetoCommand, CreateRecursoProjetoResponse>,
    IAsyncRequestHandler<GetRecursoProjetoQuery, GetRecursoProjetoResponse>,
    IAsyncRequestHandler<ListRecursoProjetoQuery, ListRecursoProjetoResponse>,
    IAsyncRequestHandler<RemoveRecursoProjetoCommand, RemoveRecursoProjetoResponse>,
    IAsyncRequestHandler<UpdateRecursoProjetoCommand, UpdateRecursoProjetoResponse>,
    IAsyncRequestHandler<GetByProjetoQuery, GetByProjetoResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RecursoProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<CreateRecursoProjetoResponse> InvokeAsync(CreateRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        CreateRecursoProjetoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        RecursoProjeto obj = await _unitOfWork.RecursoProjetoRepository.AddAsync(_mapper.Map<RecursoProjeto>(request.RecursoProjeto));
        result.RecursoProjeto = _mapper.Map<RecursoProjetoViewModel>(obj);

        await _unitOfWork.SaveChangesAsync();

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<GetRecursoProjetoResponse> InvokeAsync(GetRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        GetRecursoProjetoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.RecursoProjeto = _mapper.Map<RecursoProjetoViewModel>(await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id));

        if (result.RecursoProjeto == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<ListRecursoProjetoResponse> InvokeAsync(ListRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        ListRecursoProjetoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.RecursoProjetos = _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(await _unitOfWork.RecursoProjetoRepository.AllAsync(request.GetDependencies));
        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<RemoveRecursoProjetoResponse> InvokeAsync(RemoveRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        RemoveRecursoProjetoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        RecursoProjeto obj = await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id);

        if (obj == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        await _unitOfWork.RecursoProjetoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<UpdateRecursoProjetoResponse> InvokeAsync(UpdateRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        UpdateRecursoProjetoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        _unitOfWork.RecursoProjetoRepository.Update(_mapper.Map<RecursoProjeto>(request.RecursoProjeto));

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<GetByProjetoResponse> InvokeAsync(GetByProjetoQuery request, CancellationToken cancellationToken)
    {
        GetByProjetoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.RecursoProjetos = _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(await _unitOfWork.RecursoProjetoRepository.GetByProjetoAsync(request.IdProjeto));
        result.Status = OperationResult.Success;

        return result;
    }
}
