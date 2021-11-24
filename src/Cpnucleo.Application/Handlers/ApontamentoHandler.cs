using Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

namespace Cpnucleo.Application.Handlers;

public class ApontamentoHandler :
    IRequestHandler<CreateApontamentoCommand, OperationResult>,
    IRequestHandler<GetApontamentoQuery, ApontamentoViewModel>,
    IRequestHandler<ListApontamentoQuery, IEnumerable<ApontamentoViewModel>>,
    IRequestHandler<RemoveApontamentoCommand, OperationResult>,
    IRequestHandler<UpdateApontamentoCommand, OperationResult>,
    IRequestHandler<GetByRecursoQuery, IEnumerable<ApontamentoViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApontamentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateApontamentoCommand request, CancellationToken cancellationToken = default)
    {
        await _unitOfWork.ApontamentoRepository.AddAsync(_mapper.Map<Apontamento>(request.Apontamento));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<ApontamentoViewModel> Handle(GetApontamentoQuery request, CancellationToken cancellationToken = default)
    {
        ApontamentoViewModel result = _mapper.Map<ApontamentoViewModel>(await _unitOfWork.ApontamentoRepository.GetAsync(request.Id));

        return result;
    }

    public async Task<IEnumerable<ApontamentoViewModel>> Handle(ListApontamentoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ApontamentoViewModel> result = _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async Task<OperationResult> Handle(RemoveApontamentoCommand request, CancellationToken cancellationToken = default)
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

    public async Task<OperationResult> Handle(UpdateApontamentoCommand request, CancellationToken cancellationToken = default)
    {
        _unitOfWork.ApontamentoRepository.Update(_mapper.Map<Apontamento>(request.Apontamento));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<IEnumerable<ApontamentoViewModel>> Handle(GetByRecursoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ApontamentoViewModel> result = _mapper.Map<IEnumerable<ApontamentoViewModel>>(await _unitOfWork.ApontamentoRepository.GetByRecursoAsync(request.IdRecurso));

        return result;
    }
}
