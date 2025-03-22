namespace Application.Common.Mappings;

[Mapper]
internal static partial class EntityToDtoMapper
{
    public static partial AppointmentDto MapToDto(this Appointment entity);
    public static partial AssignmentDto MapToDto(this Assignment entity);
    public static partial AssignmentImpedimentDto MapToDto(this AssignmentImpediment entity);
    public static partial AssignmentTypeDto MapToDto(this AssignmentType entity);
    public static partial ImpedimentDto MapToDto(this Impediment entity);
    public static partial OrganizationDto MapToDto(this Organization entity);
    public static partial ProjectDto MapToDto(this Project entity);
    public static partial UserAssignmentDto MapToDto(this UserAssignment entity);
    
    [MapperIgnoreSource(nameof(User.Password))]
    public static partial UserDto MapToDto(this User entity);
    
    public static partial UserProjectDto MapToDto(this UserProject entity);
    public static partial WorkflowDto MapToDto(this Workflow entity);
}