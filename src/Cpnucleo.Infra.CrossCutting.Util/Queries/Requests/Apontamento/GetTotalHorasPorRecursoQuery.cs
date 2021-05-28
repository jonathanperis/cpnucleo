using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Apontamento;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Apontamento
{
    public class GetTotalHorasPorRecursoQuery : IRequest<GetTotalHorasPorRecursoResponse>
    {
        public Guid IdRecurso { get; set; }
        public Guid IdTarefa { get; set; }
    }
}
