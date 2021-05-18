using Cpnucleo.Domain.Commands.Responses.Apontamento;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.Apontamento
{
    public class RemoveApontamentoComand : IRequest<RemoveApontamentoResponse>
    {
        public Guid Id { get; set; }
    }
}
