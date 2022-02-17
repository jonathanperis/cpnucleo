namespace Cpnucleo.Application.Queries.Apontamento;

public class GetApontamentoHandler : IRequestHandler<GetApontamentoQuery, GetApontamentoViewModel>
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
        var apontamento = await _unitOfWork.ApontamentoRepository.GetAsync(request.Id);

        if (apontamento == null)
        {
            return new GetApontamentoViewModel { OperationResult = OperationResult.NotFound };
        }

        ApontamentoDTO result = _mapper.Map<ApontamentoDTO>(apontamento);

        return new GetApontamentoViewModel { Apontamento = result, OperationResult = OperationResult.Success };
    }
}
