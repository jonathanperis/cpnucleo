using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso
{
    [DataContract]
    public class GetByRecursoQuery
    {
        [DataMember(Order = 1)]
        public Guid IdRecurso { get; set; }
    }
}
