using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Projeto;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Projeto
{
    [DataContract]
    public class UpdateProjetoCommand : IRequest<UpdateProjetoResponse>
    {
        [DataMember(Order = 1)]
        public ProjetoViewModel Projeto { get; set; }
    }
}
