namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.CreateRecurso;

public class CreateRecursoResponse : BaseCommand
{
    public OperationResult Status { get; set; }

    public RecursoViewModel Recurso { get; set; }
}