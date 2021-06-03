using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Recurso.UpdateRecurso
{
    [DataContract]
    public class UpdateRecursoCommand : IRequest<UpdateRecursoResponse>
    {
        [DataMember(Order = 1)]
        public RecursoViewModel Recurso { get; set; }
    }
}
