using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema
{
    public class RemoveSistemaCommand : IRequest<RemoveSistemaResponse>
    {
        public Guid Id { get; set; }
    }
}
