namespace Cpnucleo.Infra.CrossCutting.Util.Queries.ImpedimentoTarefa;

public class GetImpedimentoTarefaQuery : IRequest<GetImpedimentoTarefaViewModel>
{
    public Guid Id { get; set; }
}
