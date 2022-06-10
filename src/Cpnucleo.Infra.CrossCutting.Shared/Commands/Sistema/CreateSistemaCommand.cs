namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Sistema;

public class CreateSistemaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
