using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.RecursoProjeto;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.RecursoProjeto
{
    public class GetByProjetoQuery : IRequest<GetByProjetoResponse>
    {
        public Guid IdProjeto { get; set; }
    }
}
