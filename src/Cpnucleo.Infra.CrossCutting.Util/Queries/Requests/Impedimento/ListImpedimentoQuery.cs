using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Impedimento;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Impedimento
{
    public class ListImpedimentoQuery : IRequest<ListImpedimentoResponse>
    {
        public bool GetDependencies { get; set; }
    }
}
