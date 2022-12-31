namespace Cpnucleo.Application.Queries.Recurso;

public sealed class GetRecursoHandler : IRequestHandler<GetRecursoQuery, GetRecursoViewModel>
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
        RecursoDTO recurso = await _unitOfWork.RecursoRepository.Get(request.Id)
            .ProjectTo<RecursoDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (recurso is null)
        {
            return new GetRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        recurso.Senha = null;

        return new GetRecursoViewModel { Recurso = recurso, OperationResult = OperationResult.Success };
    }
}
