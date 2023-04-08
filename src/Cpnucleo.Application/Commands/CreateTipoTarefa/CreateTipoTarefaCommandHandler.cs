using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Commands.CreateTipoTarefa;

namespace Cpnucleo.Application.Commands.CreateTipoTarefa;

public sealed class CreateTipoTarefaCommandHandler : IRequestHandler<CreateTipoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateTipoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(CreateTipoTarefaCommand request, CancellationToken cancellationToken)
    {
        var tipoTarefa = Domain.Entities.TipoTarefa.Create(request.Nome, request.Image);
        _context.TipoTarefas.Add(tipoTarefa);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
