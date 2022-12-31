namespace Cpnucleo.Application.Queries.RecursoProjeto;

public sealed class GetRecursoProjetoHandler : IRequestHandler<GetRecursoProjetoQuery, GetRecursoProjetoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecursoProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetRecursoProjetoViewModel> Handle(GetRecursoProjetoQuery request, CancellationToken cancellationToken)
    {
        RecursoProjetoDTO recursoProjeto = await _unitOfWork.RecursoProjetoRepository.Get(request.Id)
            .ProjectTo<RecursoProjetoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (recursoProjeto is null)
        {
            return new GetRecursoProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoProjetoViewModel { RecursoProjeto = recursoProjeto, OperationResult = OperationResult.Success };
    }
}
