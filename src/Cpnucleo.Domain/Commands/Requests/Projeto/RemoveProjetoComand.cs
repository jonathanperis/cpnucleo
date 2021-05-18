using Cpnucleo.Domain.Commands.Responses.Projeto;
using MediatR;
using System;

namespace Cpnucleo.Domain.Commands.Requests.Projeto
{
    public class RemoveProjetoComand : IRequest<RemoveProjetoResponse>
    {
        public Guid Id { get; set; }
    }
}
