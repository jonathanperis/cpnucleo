using Cpnucleo.Domain.Queries.Responses.Recurso;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.Recurso
{
    public class AuthQuery : IRequest<AuthResponse>
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
