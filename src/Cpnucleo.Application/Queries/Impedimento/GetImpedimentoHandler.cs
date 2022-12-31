namespace Cpnucleo.Application.Queries.Impedimento;

public sealed class GetImpedimentoHandler : IRequestHandler<GetImpedimentoQuery, GetImpedimentoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetImpedimentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetImpedimentoViewModel> Handle(GetImpedimentoQuery request, CancellationToken cancellationToken)
    {
        ImpedimentoDTO impedimento = await _unitOfWork.ImpedimentoRepository.Get(request.Id)
            .ProjectTo<ImpedimentoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (impedimento is null)
        {
            return new GetImpedimentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetImpedimentoViewModel { Impedimento = impedimento, OperationResult = OperationResult.Success };
    }
}
