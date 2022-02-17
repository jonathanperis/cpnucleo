namespace Cpnucleo.Infra.CrossCutting.Util.Common.DTOs;

public class ApontamentoDTO : BaseDTO
{
    public string Descricao { get; set; }

    public DateTime? DataApontamento { get; set; }

    public int QtdHoras { get; set; }

    public Guid IdTarefa { get; set; }

    public Guid IdRecurso { get; set; }

    public TarefaDTO? Tarefa { get; set; }
}