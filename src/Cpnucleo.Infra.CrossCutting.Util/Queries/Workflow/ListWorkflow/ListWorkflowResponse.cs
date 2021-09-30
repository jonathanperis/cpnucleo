﻿namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow;

[DataContract]
public class ListWorkflowResponse
{
    [DataMember(Order = 1)]
    public OperationResult Status { get; set; }

    [DataMember(Order = 2)]
    public IEnumerable<WorkflowViewModel> Workflows { get; set; }
}