using Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa;
using Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

namespace Cpnucleo.Application.Handlers;

public class ImpedimentoTarefaHandler :
    IRequestHandler<CreateImpedimentoTarefaCommand, OperationResult>,
    IRequestHandler<GetImpedimentoTarefaQuery, ImpedimentoTarefaViewModel>,
    IRequestHandler<ListImpedimentoTarefaQuery, IEnumerable<ImpedimentoTarefaViewModel>>,
    IRequestHandler<RemoveImpedimentoTarefaCommand, OperationResult>,
    IRequestHandler<UpdateImpedimentoTarefaCommand, OperationResult>,
    IRequestHandler<GetByTarefaQuery, IEnumerable<ImpedimentoTarefaViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ImpedimentoTarefaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ImpedimentoTarefaRepository.AddAsync(_mapper.Map<ImpedimentoTarefa>(request.ImpedimentoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<ImpedimentoTarefaViewModel> Handle(GetImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        ImpedimentoTarefaViewModel result = _mapper.Map<ImpedimentoTarefaViewModel>(await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id));

        return result;
    }

    public async Task<IEnumerable<ImpedimentoTarefaViewModel>> Handle(ListImpedimentoTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ImpedimentoTarefaViewModel> result = _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(await _unitOfWork.ImpedimentoTarefaRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async Task<OperationResult> Handle(RemoveImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        ImpedimentoTarefa obj = await _unitOfWork.ImpedimentoTarefaRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ImpedimentoTarefaRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<OperationResult> Handle(UpdateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.ImpedimentoTarefaRepository.Update(_mapper.Map<ImpedimentoTarefa>(request.ImpedimentoTarefa));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<IEnumerable<ImpedimentoTarefaViewModel>> Handle(GetByTarefaQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ImpedimentoTarefaViewModel> result = _mapper.Map<IEnumerable<ImpedimentoTarefaViewModel>>(await _unitOfWork.ImpedimentoTarefaRepository.GetByTarefaAsync(request.IdTarefa));

        return result;
    }
}
