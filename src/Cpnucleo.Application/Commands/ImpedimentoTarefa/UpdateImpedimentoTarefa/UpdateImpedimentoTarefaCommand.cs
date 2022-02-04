namespace Cpnucleo.Application.Commands.ImpedimentoTarefa.UpdateImpedimentoTarefa;

public class UpdateImpedimentoTarefaCommand : IRequest<OperationResult>
{
    public Guid Id { get; }
    public string Descricao { get; }
    public Guid IdTarefa { get; }
    public Guid IdImpedimento { get; }
    public bool Ativo { get; }

    public UpdateImpedimentoTarefaCommand(Guid id, string descricao, Guid idTarefa, Guid idImpedimento, bool ativo)
    {
        Id = id;
        Descricao = descricao;
        IdTarefa = idTarefa;
        IdImpedimento = idImpedimento;
        Ativo = ativo;
    }
}
