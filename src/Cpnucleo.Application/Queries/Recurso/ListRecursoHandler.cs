namespace Cpnucleo.Application.Queries.Recurso;

public sealed class ListRecursoHandler : IRequestHandler<ListRecursoQuery, ListRecursoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListRecursoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListRecursoViewModel> Handle(ListRecursoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Recurso> recursos = await _unitOfWork.RecursoRepository.AllAsync(request.GetDependencies);

        if (recursos == null)
        {
            return new ListRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<RecursoDTO> result = _mapper.Map<IEnumerable<RecursoDTO>>(recursos);

        return new ListRecursoViewModel { Recursos = result, OperationResult = OperationResult.Success };
    }
}
