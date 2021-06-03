﻿using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa
{
    [DataContract]
    public class CreateTipoTarefaCommand : IRequest<CreateTipoTarefaResponse>
    {
        [DataMember(Order = 1)]
        public TipoTarefaViewModel TipoTarefa { get; set; }
    }
}
