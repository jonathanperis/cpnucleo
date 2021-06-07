using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto
{
    [DataContract]
    public class GetByProjetoQuery
    {
        [DataMember(Order = 1)]
        public Guid IdProjeto { get; set; }
    }
}
