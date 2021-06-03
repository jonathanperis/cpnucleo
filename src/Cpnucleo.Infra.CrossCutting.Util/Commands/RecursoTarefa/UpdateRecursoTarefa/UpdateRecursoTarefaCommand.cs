using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoTarefa.UpdateRecursoTarefa
{
    [DataContract]
    public class UpdateRecursoTarefaCommand : IRequest<UpdateRecursoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public RecursoTarefaViewModel RecursoTarefa { get; set; }
    }
}
