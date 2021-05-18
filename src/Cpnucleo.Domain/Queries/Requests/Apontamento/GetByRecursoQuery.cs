using Cpnucleo.Domain.Queries.Responses.Apontamento;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Apontamento
{
    public class GetByRecursoQuery : IRequest<GetByRecursoResponse>
    {
        public Guid IdRecurso { get; set; }
    }
}
