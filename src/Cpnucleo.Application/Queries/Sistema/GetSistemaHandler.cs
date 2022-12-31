namespace Cpnucleo.Application.Queries.Sistema;

public sealed class GetSistemaHandler : IRequestHandler<GetSistemaQuery, GetSistemaViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetSistemaHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetSistemaViewModel> Handle(GetSistemaQuery request, CancellationToken cancellationToken)
    {
        SistemaDTO sistema = await _unitOfWork.SistemaRepository.Get(request.Id)
            .ProjectTo<SistemaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (sistema is null)
        {
            return new GetSistemaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetSistemaViewModel { Sistema = sistema, OperationResult = OperationResult.Success };
    }
}
