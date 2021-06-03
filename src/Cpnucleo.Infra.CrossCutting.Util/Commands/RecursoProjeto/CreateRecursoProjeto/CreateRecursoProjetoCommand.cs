﻿using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.RecursoProjeto.CreateRecursoProjeto
{
    [DataContract]
    public class CreateRecursoProjetoCommand : IRequest<CreateRecursoProjetoResponse>
    {
        [DataMember(Order = 1)]
        public RecursoProjetoViewModel RecursoProjeto { get; set; }
    }
}