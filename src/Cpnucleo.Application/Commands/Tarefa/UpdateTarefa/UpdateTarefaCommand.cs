namespace Cpnucleo.Application.Commands.Tarefa.UpdateTarefa;

public class UpdateTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataTermino { get; set; }
    public int QtdHoras { get; set; }
    public string Detalhe { get; set; }
    public bool Ativo { get; set; }
    public Guid IdProjeto { get; set; }
    public Guid IdWorkflow { get; set; }
    public Guid IdRecurso { get; set; }
    public Guid IdTipoTarefa { get; set; }
}
