namespace Cpnucleo.Application.Commands.Sistema.CreateSistema;

public class CreateSistemaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
