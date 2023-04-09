namespace Cpnucleo.Application.Commands.UpdateProjeto;

public sealed class UpdateProjetoCommandHandler : IRequestHandler<UpdateProjetoCommand, OperationResult>
{
    private readonly IApplicationDbContext _context;

    public UpdateProjetoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OperationResult> Handle(UpdateProjetoCommand request, CancellationToken cancellationToken)
    {
        var projeto = await _context.Projetos
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (projeto is null)
        {
            return OperationResult.NotFound;
        }

        projeto = Domain.Entities.Projeto.Update(projeto, request.Nome, request.IdSistema);
        _context.Projetos.Update(projeto);

        bool success = await _context.SaveChangesAsync(cancellationToken);

        OperationResult result = success ? OperationResult.Success : OperationResult.Failed;

        return result;
    }
}
