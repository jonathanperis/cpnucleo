﻿using MediatR;
using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetTarefa
{
    [DataContract]
    public class GetTarefaQuery : IRequest<GetTarefaResponse>
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
