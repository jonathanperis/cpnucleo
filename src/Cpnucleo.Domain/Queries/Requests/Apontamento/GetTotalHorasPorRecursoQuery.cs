using Cpnucleo.Domain.Queries.Responses.Apontamento;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Apontamento
{
    public class GetTotalHorasPorRecursoQuery : IRequest<GetTotalHorasPorRecursoResponse>
    {
        public Guid IdRecurso { get; set; }
        public Guid IdTarefa { get; set; }
    }
}
