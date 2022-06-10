namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Recurso;

public class CreateRecursoCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
}
