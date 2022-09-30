namespace Cpnucleo.Application.Queries.RecursoProjeto;

public sealed class ListRecursoProjetoHandler : IRequestHandler<ListRecursoProjetoQuery, ListRecursoProjetoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListRecursoProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListRecursoProjetoViewModel> Handle(ListRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.RecursoProjeto> recursoProjetos = await _unitOfWork.RecursoProjetoRepository.AllAsync(request.GetDependencies);

        if (recursoProjetos == null)
        {
            return new ListRecursoProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<RecursoProjetoDTO> result = _mapper.Map<IEnumerable<RecursoProjetoDTO>>(recursoProjetos);

        return new ListRecursoProjetoViewModel { RecursoProjetos = result, OperationResult = OperationResult.Success };
    }
}
