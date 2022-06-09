namespace Cpnucleo.Application.Queries.Recurso;

public class GetRecursoHandler : IRequestHandler<GetRecursoQuery, GetRecursoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetRecursoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetRecursoViewModel> Handle(GetRecursoQuery request, CancellationToken cancellationToken)
    {
        var recurso = await _unitOfWork.RecursoRepository.GetAsync(request.Id);

        if (recurso == null)
        {
            return new GetRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        RecursoDTO result = _mapper.Map<RecursoDTO>(recurso);

        result.Senha = null;

        return new GetRecursoViewModel { Recurso = result, OperationResult = OperationResult.Success };
    }
}
