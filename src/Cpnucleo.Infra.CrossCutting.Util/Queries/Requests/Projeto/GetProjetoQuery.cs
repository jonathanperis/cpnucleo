using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Projeto;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Projeto
{
    public class GetProjetoQuery : IRequest<GetProjetoResponse>
    {
        public Guid Id { get; set; }
    }
}
