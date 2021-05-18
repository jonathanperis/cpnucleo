using Cpnucleo.Domain.Commands.Responses.Sistema;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.Sistema
{
    public class RemoveSistemaComand : IRequest<RemoveSistemaResponse>
    {
        public Guid Id { get; set; }
    }
}
