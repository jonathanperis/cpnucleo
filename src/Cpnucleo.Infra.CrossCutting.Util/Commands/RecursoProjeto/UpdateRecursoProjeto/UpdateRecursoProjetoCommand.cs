using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.UpdateRecursoProjeto
{
    [DataContract]
    public class UpdateRecursoProjetoCommand : IRequest<UpdateRecursoProjetoResponse>
    {
        [DataMember(Order = 1)]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }
    }
}
