namespace Cpnucleo.Application.Common.DTOs;

public class ImpedimentoTarefaDTO : BaseDTO
{
    public string Descricao { get; set; }

    public Guid IdTarefa { get; set; }

    public Guid IdImpedimento { get; set; }

    public TarefaDTO? Tarefa { get; set; }

    public ImpedimentoDTO? Impedimento { get; set; }
}