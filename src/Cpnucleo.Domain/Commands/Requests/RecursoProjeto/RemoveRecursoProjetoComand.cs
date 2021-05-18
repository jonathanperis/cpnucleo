using Cpnucleo.Domain.Commands.Responses.RecursoProjeto;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.RecursoProjeto
{
    public class RemoveRecursoProjetoComand : IRequest<RemoveRecursoProjetoResponse>
    {
        public Guid Id { get; set; }
    }
}
