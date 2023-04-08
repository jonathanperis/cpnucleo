using Cpnucleo.Application.Common.Context;
using Cpnucleo.Shared.Commands.CreateProjeto;

namespace Cpnucleo.Application.Commands.CreateProjeto;

public sealed class CreateProjetoCommandHandler : IRequestHandler<CreateProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public CreateProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(CreateProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = Projeto.Create(request.Nome, request.IdSistema);
        _context.Projetos.Add(projeto);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
