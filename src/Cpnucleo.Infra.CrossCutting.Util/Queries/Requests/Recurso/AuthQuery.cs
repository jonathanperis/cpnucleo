using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Recurso;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Recurso
{
    public class AuthQuery : IRequest<AuthResponse>
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
