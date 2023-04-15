namespace Cpnucleo.Application.Queries.GetRecurso;

public sealed class GetRecursoQueryHandler : IRequestHandler<GetRecursoQuery, GetRecursoViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetRecursoQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetRecursoViewModel> Handle(GetRecursoQuery request, CancellationToken cancellationToken)
    {
        var recurso = await _context.Recursos
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (recurso is null)
        {
            return new GetRecursoViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetRecursoViewModel { Recurso = recurso, OperationResult = OperationResult.Success };
    }
}