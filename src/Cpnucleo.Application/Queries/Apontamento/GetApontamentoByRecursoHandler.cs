namespace Cpnucleo.Application.Queries.Apontamento;

public sealed class GetApontamentoByRecursoHandler : IRequestHandler<GetApontamentoByRecursoQuery, GetApontamentoByRecursoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetApontamentoByRecursoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetApontamentoByRecursoViewModel> Handle(GetApontamentoByRecursoQuery request, CancellationToken cancellationToken)
    {
        List<ApontamentoDTO> apontamentos = await _unitOfWork.ApontamentoRepository.ListApontamentoByRecurso(request.IdRecurso)
            .ProjectTo<ApontamentoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (apontamentos is null)
        {
            return new GetApontamentoByRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetApontamentoByRecursoViewModel { Apontamentos = apontamentos, OperationResult = OperationResult.Success };
    }
}
