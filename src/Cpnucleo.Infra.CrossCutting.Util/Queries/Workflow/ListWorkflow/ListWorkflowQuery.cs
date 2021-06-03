using MediatR;
using System.Runtime.Serialization;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Workflow.ListWorkflow
{
    [DataContract]
    public class ListWorkflowQuery : IRequest<ListWorkflowResponse>
    {
        [DataMember(Order = 1)]
        public bool GetDependencies { get; set; }
    }
}
