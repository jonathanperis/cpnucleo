using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Sistema;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Sistema
{
    [DataContract]
    public class UpdateSistemaCommand : IRequest<UpdateSistemaResponse>
    {
        [DataMember(Order = 1)]
        public SistemaViewModel Sistema { get; set; }
    }
}
