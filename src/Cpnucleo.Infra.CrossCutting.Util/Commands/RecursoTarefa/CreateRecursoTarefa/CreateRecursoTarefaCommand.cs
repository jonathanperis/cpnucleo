using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.CreateRecursoTarefa
{
    [DataContract]
    public class CreateRecursoTarefaCommand : IRequest<CreateRecursoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }
    }
}
