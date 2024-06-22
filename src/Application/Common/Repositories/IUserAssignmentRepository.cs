namespace Application.Common.Repositories;

public interface IUserAssignmentRepository
{
    Task<bool> CreateUserAssignment(UserAssignment userAssignment);
    Task<UserAssignmentDto?> GetUserAssignmentById(Ulid id);
    Task<List<UserAssignmentDto>?> ListUserAssignments();
    Task<bool> RemoveUserAssignment(Ulid id);
    Task<bool> UpdateUserAssignment(Ulid id, Ulid userId, Ulid assignmentId);
}
