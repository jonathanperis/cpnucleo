namespace Cpnucleo.Application.Commands.Sistema.UpdateSistema;

public class UpdateSistemaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
