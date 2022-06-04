namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso;

public class CreateRecursoCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
}
