namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.GetByTarefa;

public class GetImpedimentoTarefaByTarefaQuery : IRequest<GetImpedimentoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
