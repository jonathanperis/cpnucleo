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
        IEnumerable<Domain.Entities.Apontamento> apontamentos = await _unitOfWork.ApontamentoRepository.AllAsync(request.GetDependencies);

        if (apontamentos == null)
        {
            return new ListApontamentoViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<ApontamentoDTO> result = _mapper.Map<IEnumerable<ApontamentoDTO>>(apontamentos);

        return new ListApontamentoViewModel { Apontamentos = result, OperationResult = OperationResult.Success };
    }
}
