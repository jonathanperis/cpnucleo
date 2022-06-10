namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Sistema;

public class UpdateSistemaCommand : BaseCommand, IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }
}
