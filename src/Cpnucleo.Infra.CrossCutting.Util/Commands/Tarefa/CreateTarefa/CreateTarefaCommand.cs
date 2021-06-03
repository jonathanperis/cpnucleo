using Cpnucleo.Infra.CrossCutting.Util.Commands.Responses.Tarefa;
using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Requests.Tarefa
{
    [DataContract]
    public class CreateTarefaCommand : IRequest<CreateTarefaResponse>
    {
        [DataMember(Order = 1)]
        public TarefaViewModel Tarefa { get; set; }
    }
}
