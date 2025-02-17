namespace Domain.Repositories;

public interface IAssignmentTypeRepository
{
    Task<bool> CreateAssignmentType(AssignmentType assignmentType);
    Task<AssignmentType?> GetAssignmentTypeById(Guid id);
    Task<List<AssignmentType>?> ListAssignmentTypes();
    Task<bool> RemoveAssignmentType(Guid id);
    Task<bool> UpdateAssignmentType(Guid id, string name);
}
