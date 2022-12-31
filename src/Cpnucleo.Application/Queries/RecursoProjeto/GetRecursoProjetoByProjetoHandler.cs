namespace Cpnucleo.Application.Queries.RecursoProjeto;

public sealed class GetRecursoProjetoByProjetoHandler : IRequestHandler<GetRecursoProjetoByProjetoQuery, GetRecursoProjetoByProjetoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecursoProjetoByProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetRecursoProjetoByProjetoViewModel> Handle(GetRecursoProjetoByProjetoQuery request, CancellationToken cancellationToken)
    {
        List<RecursoProjetoDTO> recursoProjetos = await _unitOfWork.RecursoProjetoRepository.GetRecursoProjetoByProjeto(request.IdProjeto)
            .ProjectTo<RecursoProjetoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursoProjetos is null)
        {
            return new GetRecursoProjetoByProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoProjetoByProjetoViewModel { RecursoProjetos = recursoProjetos, OperationResult = OperationResult.Success };
    }
}
