using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema
{
    public class CreateSistemaComand : IRequest<CreateSistemaResponse>
    {
        public SistemaViewModel Sistema { get; set; }
    }
}
