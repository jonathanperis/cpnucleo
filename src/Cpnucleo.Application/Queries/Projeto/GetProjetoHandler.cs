namespace Cpnucleo.Application.Queries.Projeto;

public sealed class GetProjetoHandler : IRequestHandler<GetProjetoQuery, GetProjetoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetProjetoViewModel> Handle(GetProjetoQuery request, CancellationToken cancellationToken)
    {
        ProjetoDTO projeto = await _unitOfWork.ProjetoRepository.Get(request.Id)
            .ProjectTo<ProjetoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (projeto is null)
        {
            return new GetProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetProjetoViewModel { Projeto = projeto, OperationResult = OperationResult.Success };
    }
}
