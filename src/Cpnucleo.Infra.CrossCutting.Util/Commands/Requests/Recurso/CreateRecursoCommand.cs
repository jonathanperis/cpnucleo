using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Recurso
{
    public class CreateRecursoCommand : IRequest<CreateRecursoResponse>
    {
        public RecursoViewModel Recurso { get; set; }
    }
}
