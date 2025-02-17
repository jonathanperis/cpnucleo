namespace Domain.Repositories;

public interface IWorkflowRepository
{
    Task<bool> CreateWorkflow(Workflow workflow);
    Task<Workflow?> GetWorkflowById(Guid id);
    Task<List<Workflow?>?> ListWorkflow();
    Task<bool> RemoveWorkflow(Guid id);
    Task<bool> UpdateWorkflow(Guid id, string name, byte order);
}
