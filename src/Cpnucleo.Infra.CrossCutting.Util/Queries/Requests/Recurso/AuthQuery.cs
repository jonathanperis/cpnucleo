using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso
{
    [DataContract]
    public class AuthQuery : IRequest<AuthResponse>
    {
        [DataMember(Order = 1)]
        public string Login { get; set; }

        [DataMember(Order = 2)]
        public string Senha { get; set; }
    }
}
