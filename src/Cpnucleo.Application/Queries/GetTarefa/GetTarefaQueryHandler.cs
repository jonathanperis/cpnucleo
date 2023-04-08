using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Queries.GetTarefa;

namespace Cpnucleo.Application.Queries.GetTarefa;

public sealed class GetTarefaQueryHandler : IRequestHandler<GetTarefaQuery, GetTarefaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTarefaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetTarefaViewModel> Handle(GetTarefaQuery request, CancellationToken cancellationToken)
    {
        var tarefa = await _context.Tarefas
            .Where(x => x.Id == request.Id && x.Ativo)
            .ProjectTo<TarefaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (tarefa is null)
        {
            return new GetTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new GetTarefaViewModel { Tarefa = tarefa, OperationResult = OperationResult.Success };
    }
}