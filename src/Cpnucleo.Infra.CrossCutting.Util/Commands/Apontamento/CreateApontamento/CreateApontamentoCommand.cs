﻿using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Apontamento.CreateApontamento
{
    [DataContract]
    public class CreateApontamentoCommand
    {
        [DataMember(Order = 1)]
        public ApontamentoViewModel Apontamento { get; set; }
    }
}
