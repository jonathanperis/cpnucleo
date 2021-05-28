using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Sistema;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Sistema
{
    public class ListSistemaQuery : IRequest<ListSistemaResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
