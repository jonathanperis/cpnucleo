namespace Cpnucleo.Application.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa;

public class CreateImpedimentoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Descricao { get; }
    public Guid IdTarefa { get; }
    public Guid IdImpedimento { get; }

    public CreateImpedimentoTarefaCommand(Guid id, string descricao, Guid idTarefa, Guid idImpedimento)
    {
        Id = id;
        Descricao = descricao;
        IdTarefa = idTarefa;
        IdImpedimento = idImpedimento;
    }
}
