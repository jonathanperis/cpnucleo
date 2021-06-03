﻿using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetTotalHorasPorRecurso
{
    [DataContract]
    public class GetTotalHorasPorRecursoQuery : IRequest<GetTotalHorasPorRecursoResponse>
    {
        [DataMember(Order = 1)]
        public Guid IdRecurso { get; set; }

        [DataMember(Order = 2)]
        public Guid IdTarefa { get; set; }
    }
}
