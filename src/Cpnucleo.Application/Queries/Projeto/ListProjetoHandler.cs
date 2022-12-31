namespace Cpnucleo.Application.Queries.Projeto;

public sealed class ListProjetoHandler : IRequestHandler<ListProjetoQuery, ListProjetoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListProjetoViewModel> Handle(ListProjetoQuery request, CancellationToken cancellationToken)
    {
        List<ProjetoDTO> projetos = await _unitOfWork.ProjetoRepository.All(request.GetDependencies)
            .ProjectTo<ProjetoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (projetos is null)
        {
            return new ListProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListProjetoViewModel { Projetos = projetos, OperationResult = OperationResult.Success };
    }
}
