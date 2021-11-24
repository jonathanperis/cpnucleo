using Cpnucleo.Infra.CrossCutting.Util.Commands.Impedimento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

namespace Cpnucleo.Application.Handlers;

public class ImpedimentoHandler :
    IRequestHandler<CreateImpedimentoCommand, OperationResult>,
    IRequestHandler<GetImpedimentoQuery, ImpedimentoViewModel>,
    IRequestHandler<ListImpedimentoQuery, IEnumerable<ImpedimentoViewModel>>,
    IRequestHandler<RemoveImpedimentoCommand, OperationResult>,
    IRequestHandler<UpdateImpedimentoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ImpedimentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateImpedimentoCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ImpedimentoRepository.AddAsync(_mapper.Map<Impedimento>(request.Impedimento));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<ImpedimentoViewModel> Handle(GetImpedimentoQuery request, CancellationToken cancellationToken)
    {
        ImpedimentoViewModel result = _mapper.Map<ImpedimentoViewModel>(await _unitOfWork.ImpedimentoRepository.GetAsync(request.Id));

        return result;
    }

    public async Task<IEnumerable<ImpedimentoViewModel>> Handle(ListImpedimentoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ImpedimentoViewModel> result = _mapper.Map<IEnumerable<ImpedimentoViewModel>>(await _unitOfWork.ImpedimentoRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async Task<OperationResult> Handle(RemoveImpedimentoCommand request, CancellationToken cancellationToken)
    {
        Impedimento obj = await _unitOfWork.ImpedimentoRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ImpedimentoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<OperationResult> Handle(UpdateImpedimentoCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.ImpedimentoRepository.Update(_mapper.Map<Impedimento>(request.Impedimento));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
