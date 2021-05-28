using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Apontamento;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Apontamento
{
    public class RemoveApontamentoCommand : IRequest<RemoveApontamentoResponse>
    {
        public Guid Id { get; set; }
    }
}
