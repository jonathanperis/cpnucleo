namespace Cpnucleo.Application.Queries.RecursoProjeto.GetByProjeto;

public class GetByProjetoHandler : IRequestHandler<GetByProjetoQuery, GetByProjetoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetByProjetoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetByProjetoViewModel> Handle(GetByProjetoQuery request, CancellationToken cancellationToken)
    {
        var recursoProjetos = await _unitOfWork.RecursoProjetoRepository.GetByProjetoAsync(request.IdProjeto);

        if (recursoProjetos == null)
        {
            return new GetByProjetoViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<RecursoProjetoDTO> result = _mapper.Map<IEnumerable<RecursoProjetoDTO>>(recursoProjetos);

        return new GetByProjetoViewModel { RecursoProjetos = result, OperationResult = OperationResult.Success };
    }
}
