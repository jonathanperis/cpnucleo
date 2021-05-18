using Cpnucleo.Domain.Commands.Responses.Recurso;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.Recurso
{
    public class RemoveRecursoComand : IRequest<RemoveRecursoResponse>
    {
        public Guid Id { get; set; }
    }
}
