namespace Cpnucleo.Application.Queries.Apontamento.GetByRecurso;

public class GetByRecursoHandler : IRequestHandler<GetByRecursoQuery, GetByRecursoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetByRecursoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetByRecursoViewModel> Handle(GetByRecursoQuery request, CancellationToken cancellationToken)
    {
        var apontamentos = await _unitOfWork.ApontamentoRepository.GetByRecursoAsync(request.IdRecurso);

        if (apontamentos == null)
        {
            return new GetByRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        IEnumerable<ApontamentoDTO> result = _mapper.Map<IEnumerable<ApontamentoDTO>>(apontamentos);

        return new GetByRecursoViewModel { Apontamentos = result, OperationResult = OperationResult.Success };
    }
}
