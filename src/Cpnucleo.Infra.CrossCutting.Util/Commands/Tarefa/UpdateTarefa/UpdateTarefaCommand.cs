using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Tarefa.UpdateTarefa
{
    [DataContract]
    public class UpdateTarefaCommand : IRequest<UpdateTarefaResponse>
    {
        [DataMember(Order = 1)]
        public TarefaViewModel Tarefa { get; set; }
    }
}
