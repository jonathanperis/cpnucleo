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
        Domain.Entities.Impedimento impedimento = await _unitOfWork.ImpedimentoRepository.GetAsync(request.Id);

        if (impedimento == null)
        {
            return new GetImpedimentoViewModel { OperationResult = OperationResult.NotFound };
        }

        ImpedimentoDTO result = _mapper.Map<ImpedimentoDTO>(impedimento);

        return new GetImpedimentoViewModel { Impedimento = result, OperationResult = OperationResult.Success };
    }
}
