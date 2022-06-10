using Cpnucleo.Domain.Services;

namespace Cpnucleo.Application.Test.Helpers;

public class WorkflowHelper
{
    public static IWorkflowService GetInstance()
    {
        return new WorkflowService();
    }
}
