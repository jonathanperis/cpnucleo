using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Commands.CreateRecursoProjeto;

namespace Cpnucleo.Application.Commands.CreateRecursoProjeto;

public sealed class CreateRecursoProjetoCommandHandler : IRequestHandler<CreateRecursoProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateRecursoProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(CreateRecursoProjetoCommand request, CancellationToken cancellationToken)
    {
        var recursoProjeto = Domain.Entities.RecursoProjeto.Create(request.IdRecurso, request.IdProjeto);
        _context.RecursoProjetos.Add(recursoProjeto);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
