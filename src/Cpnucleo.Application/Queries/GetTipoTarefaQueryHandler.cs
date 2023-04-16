namespace Cpnucleo.Application.Queries;

public sealed class GetTipoTarefaQueryHandler : IRequestHandler<GetTipoTarefaQuery, GetTipoTarefaViewModel>
{
    private readonly IApplicationDbContext _context;

    public GetTipoTarefaQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<GetTipoTarefaViewModel> Handle(GetTipoTarefaQuery request, CancellationToken cancellationToken)
    {
        var tipoTarefa = await _context.TipoTarefas
            .AsNoTracking()
            .Where(x => x.Id == request.Id && x.Ativo)
            .Select(x => x.MapToDto())
            .FirstOrDefaultAsync(cancellationToken);

        if (tipoTarefa is null)
        {
            return new GetTipoTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetTipoTarefaViewModel { TipoTarefa = tipoTarefa, OperationResult = OperationResult.Success };
    }
}