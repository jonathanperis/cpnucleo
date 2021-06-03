using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Projeto.CreateProjeto
{
    [DataContract]
    public class CreateProjetoCommand : IRequest<CreateProjetoResponse>
    {
        [DataMember(Order = 1)]
        public ProjetoViewModel Projeto { get; set; }
    }
}
