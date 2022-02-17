namespace Cpnucleo.Application.Queries.Sistema;

public class GetSistemaHandler : IRequestHandler<GetSistemaQuery, GetSistemaViewModel>
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
        var sistema = await _unitOfWork.SistemaRepository.GetAsync(request.Id);

        if (sistema == null)
        {
            return new GetSistemaViewModel { OperationResult = OperationResult.NotFound };
        }

        SistemaDTO result = _mapper.Map<SistemaDTO>(sistema);

        return new GetSistemaViewModel { Sistema = result, OperationResult = OperationResult.Success };
    }
}
