namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaQuery : BaseQuery, IRequest<ImpedimentoTarefaViewModel>
{
    public Guid Id { get; set; }
}