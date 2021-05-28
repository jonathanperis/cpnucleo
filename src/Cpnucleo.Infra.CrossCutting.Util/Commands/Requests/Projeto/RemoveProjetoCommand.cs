using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Projeto
{
    public class RemoveProjetoCommand : IRequest<RemoveProjetoResponse>
    {
        public Guid Id { get; set; }
    }
}
