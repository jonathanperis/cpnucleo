namespace Cpnucleo.Application.Queries.Recurso;

public sealed class ListRecursoHandler : IRequestHandler<ListRecursoQuery, ListRecursoViewModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListRecursoHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ListRecursoViewModel> Handle(ListRecursoQuery request, CancellationToken cancellationToken)
    {
        List<RecursoDTO> recursos = await _unitOfWork.RecursoRepository.List(request.GetDependencies)
            .ProjectTo<RecursoDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (recursos is null)
        {
            return new ListRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListRecursoViewModel { Recursos = recursos, OperationResult = OperationResult.Success };
    }
}
