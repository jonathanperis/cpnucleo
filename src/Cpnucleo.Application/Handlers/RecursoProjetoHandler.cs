using Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto;
using Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

namespace Cpnucleo.Application.Handlers;

public class RecursoProjetoHandler :
    IRequestHandler<CreateRecursoProjetoCommand, OperationResult>,
    IRequestHandler<GetRecursoProjetoQuery, RecursoProjetoViewModel>,
    IRequestHandler<ListRecursoProjetoQuery, IEnumerable<RecursoProjetoViewModel>>,
    IRequestHandler<RemoveRecursoProjetoCommand, OperationResult>,
    IRequestHandler<UpdateRecursoProjetoCommand, OperationResult>,
    IRequestHandler<GetByProjetoQuery, IEnumerable<RecursoProjetoViewModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RecursoProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<OperationResult> Handle(CreateRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.RecursoProjetoRepository.AddAsync(_mapper.Map<RecursoProjeto>(request.RecursoProjeto));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<RecursoProjetoViewModel> Handle(GetRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        RecursoProjetoViewModel result = _mapper.Map<RecursoProjetoViewModel>(await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id));

        return result;
    }

    public async Task<IEnumerable<RecursoProjetoViewModel>> Handle(ListRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<RecursoProjetoViewModel> result = _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(await _unitOfWork.RecursoProjetoRepository.AllAsync(request.GetDependencies));

        return result;
    }

    public async Task<OperationResult> Handle(RemoveRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        RecursoProjeto obj = await _unitOfWork.RecursoProjetoRepository.GetAsync(request.Id);

        if (obj == null)
        {
            return OperationResult.NotFound;
        }

        await _unitOfWork.RecursoProjetoRepository.RemoveAsync(request.Id);

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<OperationResult> Handle(UpdateRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        _unitOfWork.RecursoProjetoRepository.Update(_mapper.Map<RecursoProjeto>(request.RecursoProjeto));

        bool success = await _unitOfWork.SaveChangesAsync();

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }

    public async Task<IEnumerable<RecursoProjetoViewModel>> Handle(GetByProjetoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<RecursoProjetoViewModel> result = _mapper.Map<IEnumerable<RecursoProjetoViewModel>>(await _unitOfWork.RecursoProjetoRepository.GetByProjetoAsync(request.IdProjeto));

        return result;
    }
}
