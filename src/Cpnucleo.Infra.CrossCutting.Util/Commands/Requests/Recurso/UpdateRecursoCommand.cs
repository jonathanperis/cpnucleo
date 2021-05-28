using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Recurso;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Recurso
{
    public class UpdateRecursoCommand : IRequest<UpdateRecursoResponse>
    {
        public RecursoViewModel Recurso { get; set; }
    }
}
