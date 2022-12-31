namespace Cpnucleo.Application.Queries.Impedimento;

public sealed class ListImpedimentoHandler : IRequestHandler<ListImpedimentoQuery, ListImpedimentoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListImpedimentoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListImpedimentoViewModel> Handle(ListImpedimentoQuery request, CancellationToken cancellationToken)
    {
        List<ImpedimentoDTO> impedimentos = await _unitOfWork.ImpedimentoRepository.All(request.GetDependencies)
            .ProjectTo<ImpedimentoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (impedimentos is null)
        {
            return new ListImpedimentoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListImpedimentoViewModel { Impedimentos = impedimentos, OperationResult = OperationResult.Success };
    }
}
