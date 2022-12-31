namespace Cpnucleo.Application.Queries.Apontamento;

public sealed class GetApontamentoHandler : IRequestHandler<GetApontamentoQuery, GetApontamentoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetApontamentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetApontamentoViewModel> Handle(GetApontamentoQuery request, CancellationToken cancellationToken)
    {
        ApontamentoDTO apontamento = await _unitOfWork.ApontamentoRepository.Get(request.Id)
            .ProjectTo<ApontamentoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (apontamento is null)
        {
            return new GetApontamentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetApontamentoViewModel { Apontamento = apontamento, OperationResult = OperationResult.Success };
    }
}
