namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaByTarefaQuery : IRequest<GetImpedimentoTarefaByTarefaViewModel>
{
    public Guid IdTarefa { get; set; }
}
