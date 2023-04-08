using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Commands.CreateImpedimentoTarefa;

namespace Cpnucleo.Application.Commands.CreateImpedimentoTarefa;

public sealed class CreateImpedimentoTarefaCommandHandler : IRequestHandler<CreateImpedimentoTarefaCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateImpedimentoTarefaCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(CreateImpedimentoTarefaCommand request, CancellationToken cancellationToken)
    {
        var impedimentoTarefa = Domain.Entities.ImpedimentoTarefa.Create(request.Descricao, request.IdTarefa, request.IdImpedimento);
        _context.ImpedimentoTarefas.Add(impedimentoTarefa);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
