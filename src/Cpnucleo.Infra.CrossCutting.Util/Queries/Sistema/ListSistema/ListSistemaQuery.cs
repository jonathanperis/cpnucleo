namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Sistema.ListSistema
{
    [DataContract]
    public class ListSistemaQuery
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
