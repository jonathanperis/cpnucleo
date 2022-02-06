namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefaByTarefa;

public class GetImpedimentoTarefaByTarefaQuery : IRequest<GetImpedimentoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
