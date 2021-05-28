using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.RecursoProjeto;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.RecursoProjeto
{
    public class RemoveRecursoProjetoComand : IRequest<RemoveRecursoProjetoResponse>
    {
        public Guid Id { get; set; }
    }
}
