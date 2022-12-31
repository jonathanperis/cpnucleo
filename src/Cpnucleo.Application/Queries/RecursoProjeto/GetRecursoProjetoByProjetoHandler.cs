namespace Cpnucleo.Application.Queries.RecursoProjeto;

public sealed class GetRecursoProjetoByProjetoHandler : IRequestHandler<ListRecursoProjetoByProjetoQuery, ListRecursoProjetoByProjetoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecursoProjetoByProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListRecursoProjetoByProjetoViewModel> Handle(ListRecursoProjetoByProjetoQuery request, CancellationToken cancellationToken)
    {
        List<RecursoProjetoDTO> recursoProjetos = await _unitOfWork.RecursoProjetoRepository.ListRecursoProjetoByProjeto(request.IdProjeto)
            .ProjectTo<RecursoProjetoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursoProjetos is null)
        {
            return new ListRecursoProjetoByProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoProjetoByProjetoViewModel { RecursoProjetos = recursoProjetos, OperationResult = OperationResult.Success };
    }
}
