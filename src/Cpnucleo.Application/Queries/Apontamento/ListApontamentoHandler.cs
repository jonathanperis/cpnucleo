namespace Cpnucleo.Application.Queries.Apontamento;

public sealed class ListApontamentoHandler : IRequestHandler<ListApontamentoQuery, ListApontamentoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListApontamentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListApontamentoViewModel> Handle(ListApontamentoQuery request, CancellationToken cancellationToken)
    {
        List<ApontamentoDTO> apontamentos = await _unitOfWork.ApontamentoRepository.List(request.GetDependencies)
            .ProjectTo<ApontamentoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (apontamentos is null)
        {
            return new ListApontamentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListApontamentoViewModel { Apontamentos = apontamentos, OperationResult = OperationResult.Success };
    }
}
