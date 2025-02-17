namespace Domain.Repositories;

public interface IAssignmentImpedimentRepository
{
    Task<bool> CreateAssignmentImpediment(AssignmentImpediment assignmentImpediment);
    Task<AssignmentImpediment?> GetAssignmentImpedimentById(Guid id);
    Task<List<AssignmentImpediment>?> ListAssignmentImpediments();
    Task<bool> RemoveAssignmentImpediment(Guid id);
    Task<bool> UpdateAssignmentImpediment(Guid id, string description, Guid assignmentId, Guid impedimentId);
}
