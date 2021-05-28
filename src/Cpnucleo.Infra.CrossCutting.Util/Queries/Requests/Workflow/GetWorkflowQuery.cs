using Cpnucleo.Infra.CrossCutting.Util.Queries.Responses.Workflow;
using MediatR;
using System;

namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Requests.Workflow
{
    public class GetWorkflowQuery : IRequest<GetWorkflowResponse>
    {
        public Guid Id { get; set; }
    }
}
