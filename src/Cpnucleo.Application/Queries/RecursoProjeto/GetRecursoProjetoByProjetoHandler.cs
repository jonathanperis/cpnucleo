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
        IEnumerable<Domain.Entities.RecursoProjeto> recursoProjetos = await _unitOfWork.RecursoProjetoRepository.GetRecursoProjetoByProjetoAsync(request.IdProjeto);

        if (recursoProjetos == null)
        {
            return new GetRecursoProjetoByProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<RecursoProjetoDTO> result = _mapper.Map<IEnumerable<RecursoProjetoDTO>>(recursoProjetos);

        return new GetRecursoProjetoByProjetoViewModel { RecursoProjetos = result, OperationResult = OperationResult.Success };
    }
}
