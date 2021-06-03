using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Sistema.CreateSistema
{
    [DataContract]
    public class CreateSistemaCommand : IRequest<CreateSistemaResponse>
    {
        [DataMember(Order = 1)]
        public SistemaViewModel Sistema { get; set; }
    }
}
