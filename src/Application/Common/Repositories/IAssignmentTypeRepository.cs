namespace Application.Common.Repositories;

public interface IAssignmentTypeRepository
{
    Task<bool> CreateAssignmentType(AssignmentType assignmentType);
    Task<AssignmentTypeDto?> GetAssignmentTypeById(Ulid id);
    Task<List<AssignmentTypeDto>?> ListAssignmentTypes();
    Task<bool> RemoveAssignmentType(Ulid id);
    Task<bool> UpdateAssignmentType(Ulid id, string name);
}
