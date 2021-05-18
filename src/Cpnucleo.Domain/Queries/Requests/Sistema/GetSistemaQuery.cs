using Cpnucleo.Domain.Queries.Responses.Sistema;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Sistema
{
    public class GetSistemaQuery : IRequest<GetSistemaResponse>
    {
        public Guid Id { get; set; }
    }
}
