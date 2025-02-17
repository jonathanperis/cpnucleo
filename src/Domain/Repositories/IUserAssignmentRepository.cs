namespace Domain.Repositories;

public interface IUserAssignmentRepository
{
    Task<bool> CreateUserAssignment(UserAssignment userAssignment);
    Task<UserAssignment?> GetUserAssignmentById(Guid id);
    Task<List<UserAssignment?>?> ListUserAssignments();
    Task<bool> RemoveUserAssignment(Guid id);
    Task<bool> UpdateUserAssignment(Guid id, Guid userId, Guid assignmentId);
}
