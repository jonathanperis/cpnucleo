using Cpnucleo.Domain.Commands.Responses.Sistema;
using MediatR;

namespace Cpnucleo.Domain.Commands.Requests.Sistema
{
    public class CreateSistemaComand : IRequest<CreateSistemaResponse>
    {
        public Domain.Entities.Sistema Sistema { get; set; }
    }
}
