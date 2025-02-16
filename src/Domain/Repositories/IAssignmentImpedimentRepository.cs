namespace Domain.Repositories;

public interface IAssignmentImpedimentRepository
{
    Task<bool> CreateAssignmentImpediment(AssignmentImpediment assignmentImpediment);
    Task<AssignmentImpedimentDto?> GetAssignmentImpedimentById(Ulid id);
    Task<List<AssignmentImpedimentDto>?> ListAssignmentImpediments();
    Task<bool> RemoveAssignmentImpediment(Ulid id);
    Task<bool> UpdateAssignmentImpediment(Ulid id, string description, Ulid assignmentId, Ulid impedimentId);
}
