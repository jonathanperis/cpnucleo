namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;

[DataContract]
public class GetRecursoTarefaQuery
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}