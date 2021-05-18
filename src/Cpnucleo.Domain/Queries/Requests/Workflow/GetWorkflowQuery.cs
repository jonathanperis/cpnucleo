using Cpnucleo.Domain.Queries.Responses.Workflow;
using MediatR;
using System;

namespace Cpnucleo.Domain.Queries.Requests.Workflow
{
    public class GetWorkflowQuery : IRequest<GetWorkflowResponse>
    {
        public Guid Id { get; set; }
    }
}
