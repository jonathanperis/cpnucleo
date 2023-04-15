﻿namespace Cpnucleo.Application.Commands.CreateImpedimento;

public sealed class CreateImpedimentoCommandHandler : IRequestHandler<CreateImpedimentoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateImpedimentoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async ValueTask<OperationResult> Handle(CreateImpedimentoCommand request, CancellationToken cancellationToken)
    {
        var impedimento = Impedimento.Create(request.Nome);
        _context.Impedimentos.Add(impedimento);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
