using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.CreateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.RemoveImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento.UpdateImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;

namespace Cpnucleo.Application.Handlers;

public class ImpedimentoHandler :
    IAsyncRequestHandler<CreateImpedimentoCommand, CreateImpedimentoResponse>,
    IAsyncRequestHandler<GetImpedimentoQuery, GetImpedimentoResponse>,
    IAsyncRequestHandler<ListImpedimentoQuery, ListImpedimentoResponse>,
    IAsyncRequestHandler<RemoveImpedimentoCommand, RemoveImpedimentoResponse>,
    IAsyncRequestHandler<UpdateImpedimentoCommand, UpdateImpedimentoResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ImpedimentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<CreateImpedimentoResponse> InvokeAsync(CreateImpedimentoCommand request, CancellationToken cancellationToken)
    {
        CreateImpedimentoResponse result = new CreateImpedimentoResponse
        {
            Status = OperationResult.Failed
        };

        Impedimento obj = await _unitOfWork.ImpedimentoRepository.AddAsync(_mapper.Map<Impedimento>(request.Impedimento));
        result.Impedimento = _mapper.Map<ImpedimentoViewModel>(obj);

        await _unitOfWork.SaveChangesAsync();

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<GetImpedimentoResponse> InvokeAsync(GetImpedimentoQuery request, CancellationToken cancellationToken)
    {
        GetImpedimentoResponse result = new GetImpedimentoResponse
        {
            Status = OperationResult.Failed
        };

        result.Impedimento = _mapper.Map<ImpedimentoViewModel>(await _unitOfWork.ImpedimentoRepository.GetAsync(request.Id));

        if (result.Impedimento == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<ListImpedimentoResponse> InvokeAsync(ListImpedimentoQuery request, CancellationToken cancellationToken)
    {
        ListImpedimentoResponse result = new ListImpedimentoResponse
        {
            Status = OperationResult.Failed
        };

        result.Impedimentos = _mapper.Map<IEnumerable<ImpedimentoViewModel>>(await _unitOfWork.ImpedimentoRepository.AllAsync(request.GetDependencies));
        result.Status = OperationResult.Success;

        return result;
    }

    public async ValueTask<RemoveImpedimentoResponse> InvokeAsync(RemoveImpedimentoCommand request, CancellationToken cancellationToken)
    {
        RemoveImpedimentoResponse result = new RemoveImpedimentoResponse
        {
            Status = OperationResult.Failed
        };

        Impedimento obj = await _unitOfWork.ImpedimentoRepository.GetAsync(request.Id);

        if (obj == null)
        {
            result.Status = OperationResult.NotFound;

            return result;
        }

        await _unitOfWork.ImpedimentoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<UpdateImpedimentoResponse> InvokeAsync(UpdateImpedimentoCommand request, CancellationToken cancellationToken)
    {
        UpdateImpedimentoResponse result = new UpdateImpedimentoResponse
        {
            Status = OperationResult.Failed
        };

        _unitOfWork.ImpedimentoRepository.Update(_mapper.Map<Impedimento>(request.Impedimento));

        bool success = await _unitOfWork.SaveChangesAsync();

        result.Status = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
