namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class GetByTarefaQuery : BaseQuery, IRequest<IEnumerable<ImpedimentoTarefaViewModel>>
{
    public Guid IdTarefa { get; set; }
}