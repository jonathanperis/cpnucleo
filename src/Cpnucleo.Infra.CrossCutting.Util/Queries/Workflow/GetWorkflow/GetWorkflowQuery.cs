using System;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.GetWorkflow
{
    [DataContract]
    public class GetWorkflowQuery
    {
        [DataMember(Order = 1)]
        public Guid Id { get; set; }
    }
}
