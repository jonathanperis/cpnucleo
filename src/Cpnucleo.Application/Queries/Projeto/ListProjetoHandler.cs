namespace Cpnucleo.Application.Queries.Projeto;

public class ListProjetoHandler : IRequestHandler<ListProjetoQuery, ListProjetoViewModel>
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
        IEnumerable<Domain.Entities.Projeto> projetos = await _unitOfWork.ProjetoRepository.AllAsync(request.GetDependencies);

        if (projetos == null)
        {
            return new ListProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<ProjetoDTO> result = _mapper.Map<IEnumerable<ProjetoDTO>>(projetos);

        return new ListProjetoViewModel { Projetos = result, OperationResult = OperationResult.Success };
    }
}
