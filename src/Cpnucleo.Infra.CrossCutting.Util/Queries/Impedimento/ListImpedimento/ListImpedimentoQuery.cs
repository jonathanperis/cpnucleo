namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento
{
    [DataContract]
    public class ListImpedimentoQuery
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
