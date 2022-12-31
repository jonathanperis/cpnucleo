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
        List<RecursoProjetoDTO> recursoProjetos = await _unitOfWork.RecursoProjetoRepository.List(request.GetDependencies)
            .ProjectTo<RecursoProjetoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursoProjetos is null)
        {
            return new ListRecursoProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoProjetoViewModel { RecursoProjetos = recursoProjetos, OperationResult = OperationResult.Success };
    }
}
