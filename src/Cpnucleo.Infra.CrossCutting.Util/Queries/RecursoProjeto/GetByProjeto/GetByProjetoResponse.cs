﻿using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto.GetByProjeto
{
    [DataContract]
    public class GetByProjetoResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public IEnumerable<RecursoProjetoViewModel> RecursoProjetos { get; set; }
    }
}
