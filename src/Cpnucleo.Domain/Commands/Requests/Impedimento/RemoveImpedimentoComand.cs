using Cpnucleo.Domain.Commands.Responses.Impedimento;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.Impedimento
{
    public class RemoveImpedimentoComand : IRequest<RemoveImpedimentoResponse>
    {
        public Guid Id { get; set; }
    }
}
