﻿namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Workflow;

public class GetWorkflowViewModel : BaseQuery
{
    public WorkflowDTO Workflow { get; set; }
    public OperationResult OperationResult { get; set; }
}