using Cpnucleo.Domain.Queries.Responses.Sistema;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.Sistema
{
    public class ListSistemaQuery : IRequest<ListSistemaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
