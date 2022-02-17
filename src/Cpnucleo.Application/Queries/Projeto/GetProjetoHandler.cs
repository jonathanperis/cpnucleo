namespace Cpnucleo.Application.Queries.Projeto;

public class GetProjetoHandler : IRequestHandler<GetProjetoQuery, GetProjetoViewModel>
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
        var projeto = await _unitOfWork.ProjetoRepository.GetAsync(request.Id);

        if (projeto == null)
        {
            return new GetProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        ProjetoDTO result = _mapper.Map<ProjetoDTO>(projeto);

        return new GetProjetoViewModel { Projeto = result, OperationResult = OperationResult.Success };
    }
}
