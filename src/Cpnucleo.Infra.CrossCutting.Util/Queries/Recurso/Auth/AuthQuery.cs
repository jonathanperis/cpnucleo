namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso.Auth;

[DataContract]
public class AuthQuery
{
    [DataMember(Order = 1)]
    public string Login { get; set; }

    [DataMember(Order = 2)]
    public string Senha { get; set; }
}