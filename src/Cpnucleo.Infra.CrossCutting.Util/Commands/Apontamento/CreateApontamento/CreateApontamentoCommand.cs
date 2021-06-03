using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento
{
    [DataContract]
    public class CreateApontamentoCommand : IRequest<CreateApontamentoResponse>
    {
        [DataMember(Order = 1)]
        public ApontamentoViewModel Apontamento { get; set; }
    }
}
