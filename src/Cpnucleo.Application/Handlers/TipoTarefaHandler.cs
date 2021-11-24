using Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.TipoTarefa;

namespace Cpnucleo.Application.Handlers;

public class TipoTarefaHandler :
    IRequestHandler<CreateTipoTarefaCommand, OperationResult>,
    IRequestHandler<GetTipoTarefaQuery, TipoTarefaViewModel>,
    IRequestHandler<ListTipoTarefaQuery, IEnumerable<TipoTarefaViewModel>>,
    IRequestHandler<RemoveTipoTarefaCommand, OperationResult>,
    IRequestHandler<UpdateTipoTarefaCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TipoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.TipoTarefaRepository.AddAsync(_mapper.Map<TipoTarefa>(request.TipoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<TipoTarefaViewModel> Handle(GetTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        TipoTarefaViewModel result = _mapper.Map<TipoTarefaViewModel>(await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id));

        return result;
    }

    public async Task<IEnumerable<TipoTarefaViewModel>> Handle(ListTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<TipoTarefaViewModel> result = _mapper.Map<IEnumerable<TipoTarefaViewModel>>(await _unitOfWork.TipoTarefaRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async Task<OperationResult> Handle(RemoveTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        TipoTarefa obj = await _unitOfWork.TipoTarefaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.TipoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<OperationResult> Handle(UpdateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.TipoTarefaRepository.Update(_mapper.Map<TipoTarefa>(request.TipoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
