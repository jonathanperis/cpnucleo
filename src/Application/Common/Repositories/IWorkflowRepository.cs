namespace Application.Common.Repositories;

public interface IWorkflowRepository
{
    Task<bool> CreateWorkflow(Workflow workflow);
    Task<WorkflowDto?> GetWorkflowById(Ulid id);
    Task<List<WorkflowDto>?> ListWorkflow();
    Task<bool> RemoveWorkflow(Ulid id);
    Task<bool> UpdateWorkflow(Ulid id, string name, byte order);
}
