﻿using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.ImpedimentoTarefa.CreateImpedimentoTarefa
{
    [DataContract]
    public class CreateImpedimentoTarefaCommand
    {
        [DataMember(Order = 1)]
        public ImpedimentoTarefaViewModel ImpedimentoTarefa { get; set; }
    }
}
