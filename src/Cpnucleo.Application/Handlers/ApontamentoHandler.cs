using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

namespace Cpnucleo.Application.Handlers;

public class ApontamentoHandler :
    IAsyncRequestHandler<CreateApontamentoCommand, OperationResult>,
    IAsyncRequestHandler<GetApontamentoQuery, ApontamentoViewModel>,
    IAsyncRequestHandler<ListApontamentoQuery, IEnumerable<ApontamentoViewModel>>,
    IAsyncRequestHandler<RemoveApontamentoCommand, OperationResult>,
    IAsyncRequestHandler<UpdateApontamentoCommand, OperationResult>,
    IAsyncRequestHandler<GetByRecursoQuery, IEnumerable<ApontamentoViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApontamentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async ValueTask<OperationResult> InvokeAsync(CreateApontamentoCommand request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.ApontamentoRepository.AddAsync(_mapper.Map<Apontamento>(request.Apontamento));
        
        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<ApontamentoViewModel> InvokeAsync(GetApontamentoQuery request, CancellationToken cancellationToken = default)
    {
        ApontamentoViewModel result = _mapper.Map<ApontamentoViewModel>(await _unitOfWork.ApontamentoRepository.GetAsync(request.Id));

        return result;
    }

    public async ValueTask<IEnumerable<ApontamentoViewModel>> InvokeAsync(ListApontamentoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ApontamentoViewModel> result = _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(RemoveApontamentoCommand request, CancellationToken cancellationToken = default)
    {
        Apontamento obj = await _unitOfWork.ApontamentoRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ApontamentoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<OperationResult> InvokeAsync(UpdateApontamentoCommand request, CancellationToken cancellationToken = default)
    {
        _unitOfWork.ApontamentoRepository.Update(_mapper.Map<Apontamento>(request.Apontamento));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async ValueTask<IEnumerable<ApontamentoViewModel>> InvokeAsync(GetByRecursoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ApontamentoViewModel> result = _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.GetByRecursoAsync(request.IdRecurso));

        return result;
    }
}
