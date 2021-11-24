using Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

namespace Cpnucleo.Application.Handlers;

public class ProjetoHandler :
    IRequestHandler<CreateProjetoCommand, OperationResult>,
    IRequestHandler<GetProjetoQuery, ProjetoViewModel>,
    IRequestHandler<ListProjetoQuery, IEnumerable<ProjetoViewModel>>,
    IRequestHandler<RemoveProjetoCommand, OperationResult>,
    IRequestHandler<UpdateProjetoCommand, OperationResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateProjetoCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.ProjetoRepository.AddAsync(_mapper.Map<Projeto>(request.Projeto));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<ProjetoViewModel> Handle(GetProjetoQuery request, CancellationToken cancellationToken)
    {
        ProjetoViewModel result = _mapper.Map<ProjetoViewModel>(await _unitOfWork.ProjetoRepository.GetAsync(request.Id));

        return result;
    }

    public async Task<IEnumerable<ProjetoViewModel>> Handle(ListProjetoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<ProjetoViewModel> result = _mapper.Map<IEnumerable<ProjetoViewModel>>(await _unitOfWork.ProjetoRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async Task<OperationResult> Handle(RemoveProjetoCommand request, CancellationToken cancellationToken)
    {
        Projeto obj = await _unitOfWork.ProjetoRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.ProjetoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<OperationResult> Handle(UpdateProjetoCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.ProjetoRepository.Update(_mapper.Map<Projeto>(request.Projeto));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
