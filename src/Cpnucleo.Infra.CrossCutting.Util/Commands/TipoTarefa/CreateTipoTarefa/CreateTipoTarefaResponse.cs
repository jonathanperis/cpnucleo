﻿using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.TipoTarefa.CreateTipoTarefa
{
    [DataContract]
    public class CreateTipoTarefaResponse
    {
        [DataMember(Order = 1)]
        public OperationResult Status { get; set; }

        [DataMember(Order = 2)]
        public TipoTarefaViewModel TipoTarefa { get; set; }
    }
}
