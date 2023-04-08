using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Queries.ListImpedimentoTarefaByTarefa;

namespace Cpnucleo.Application.Queries.ListImpedimentoTarefaByTarefa;

public sealed class ListImpedimentoTarefaByTarefaQueryHandler : IRequestHandler<ListImpedimentoTarefaByTarefaQuery, ListImpedimentoTarefaByTarefaViewModel>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ListImpedimentoTarefaByTarefaQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ListImpedimentoTarefaByTarefaViewModel> Handle(ListImpedimentoTarefaByTarefaQuery request, CancellationToken cancellationToken)
    {
        var impedimentoTarefas = await _context.ImpedimentoTarefas
            .Where(x => x.IdTarefa == request.IdTarefa && x.Ativo)
            .OrderBy(x => x.DataInclusao)
            .ProjectTo<ImpedimentoTarefaDTO>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (impedimentoTarefas is null)
        {
            return new ListImpedimentoTarefaByTarefaViewModel { OperationResult = OperationResult.NotFound };
        }

        return new ListImpedimentoTarefaByTarefaViewModel { ImpedimentoTarefas = impedimentoTarefas, OperationResult = OperationResult.Success };
    }
}