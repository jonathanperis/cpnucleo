namespace Domain.Common.Repositories;

public interface IAssignmentRepository
{
    Task<bool> CreateAssignment(Assignment assignment);
    Task<AssignmentDto?> GetAssignmentById(Ulid id);
    Task<List<AssignmentDto>?> ListAssignments();
    Task<bool> RemoveAssignment(Ulid id);
    Task<bool> UpdateAssignment(Ulid id, string name, string description, DateTime startDate, DateTime endDate, byte amountHours, Ulid projectId, Ulid workflowId, Ulid userId, Ulid assignmentTypeId);
}
