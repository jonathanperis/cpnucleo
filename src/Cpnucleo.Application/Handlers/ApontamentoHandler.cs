using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.RemoveApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.UpdateApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetByRecurso;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;

namespace Cpnucleo.Application.Handlers;

public class ApontamentoHandler :
    IAsyncRequestHandler<CreateApontamentoCommand, CreateApontamentoResponse>,
    IAsyncRequestHandler<GetApontamentoQuery, GetApontamentoResponse>,
    IAsyncRequestHandler<ListApontamentoQuery, ListApontamentoResponse>,
    IAsyncRequestHandler<RemoveApontamentoCommand, RemoveApontamentoResponse>,
    IAsyncRequestHandler<UpdateApontamentoCommand, UpdateApontamentoResponse>,
    IAsyncRequestHandler<GetByRecursoQuery, GetByRecursoResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApontamentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<CreateApontamentoResponse> InvokeAsync(CreateApontamentoCommand request, CancellationToken cancellationToken = default)
    {
        CreateApontamentoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        Apontamento obj = await _unitOfWork.ApontamentoRepository.AddAsync(_mapper.Map<Apontamento>(request.Apontamento));
        result.Apontamento = _mapper.Map<ApontamentoViewModel>(obj);

        await _unitOfWork.SaveChangesAsync();

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<GetApontamentoResponse> InvokeAsync(GetApontamentoQuery request, CancellationToken cancellationToken = default)
    {
        GetApontamentoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.Apontamento = _mapper.Map<ApontamentoViewModel>(await _unitOfWork.ApontamentoRepository.GetAsync(request.Id));

        if (result.Apontamento == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<ListApontamentoResponse> InvokeAsync(ListApontamentoQuery request, CancellationToken cancellationToken)
    {
        ListApontamentoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.Apontamentos = _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.AllAsync(request.GetDependencies));
        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<RemoveApontamentoResponse> InvokeAsync(RemoveApontamentoCommand request, CancellationToken cancellationToken = default)
    {
        RemoveApontamentoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        Apontamento obj = await _unitOfWork.ApontamentoRepository.GetAsync(request.Id);

        if (obj == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        await _unitOfWork.ApontamentoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<UpdateApontamentoResponse> InvokeAsync(UpdateApontamentoCommand request, CancellationToken cancellationToken = default)
    {
        UpdateApontamentoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        _unitOfWork.ApontamentoRepository.Update(_mapper.Map<Apontamento>(request.Apontamento));

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<GetByRecursoResponse> InvokeAsync(GetByRecursoQuery request, CancellationToken cancellationToken)
    {
        GetByRecursoResponse result = new()
        {
            Status = OperationResult.Failed
        };

        result.Apontamentos = _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.GetByRecursoAsync(request.IdRecurso));
        result.Status = OperationResult.Success;

        return result;
    }
}
