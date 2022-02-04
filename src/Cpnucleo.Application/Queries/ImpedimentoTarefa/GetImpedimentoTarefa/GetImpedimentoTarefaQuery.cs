namespace Cpnucleo.Application.Queries.ImpedimentoTarefa.GetImpedimentoTarefa;

public class GetImpedimentoTarefaQuery : IRequest<GetImpedimentoTarefaViewModel>
{
    public Guid Id { get; }

    public GetImpedimentoTarefaQuery(Guid id)
    {
        Id = id;
    }
}
