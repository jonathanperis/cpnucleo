namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth
{
    [DataContract]
    public class AuthResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public RecursoViewModel Recurso { get; set; }
    }
}
