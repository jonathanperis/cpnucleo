namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;

[DataContract]
public class GetImpedimentoQuery
{
    [DataMember(Order = 1)]
    public Guid Id { get; set; }
}