namespace Domain.Repositories;

public interface IAssignmentRepository
{
    Task<bool> CreateAssignment(Assignment assignment);
    Task<Assignment?> GetAssignmentById(Guid id);
    Task<List<Assignment?>?> ListAssignments();
    Task<bool> RemoveAssignment(Guid id);
    Task<bool> UpdateAssignment(Guid id, string name, string description, DateTime startDate, DateTime endDate, int amountHours, Guid projectId, Guid workflowId, Guid userId, Guid assignmentTypeId);
}
