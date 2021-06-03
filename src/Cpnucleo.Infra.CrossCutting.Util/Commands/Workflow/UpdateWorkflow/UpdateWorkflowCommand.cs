﻿using Cpnucleo.Infra.CrossCutting.Util.ViewModels;
using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Commands.Workflow.UpdateWorkflow
{
    [DataContract]
    public class UpdateWorkflowCommand : IRequest<UpdateWorkflowResponse>
    {
        [DataMember(Order = 1)]
        public WorkflowViewModel Workflow { get; set; }
    }
}
