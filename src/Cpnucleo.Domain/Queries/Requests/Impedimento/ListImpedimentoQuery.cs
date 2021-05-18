using Cpnucleo.Domain.Queries.Responses.Impedimento;
using MediatR;

namespace Cpnucleo.Domain.Queries.Requests.Impedimento
{
    public class ListImpedimentoQuery : IRequest<ListImpedimentoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
