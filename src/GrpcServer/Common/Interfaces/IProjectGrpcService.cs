namespace GrpcServer.Common.Interfaces;

public interface IProjectGrpcService : IService<IProjectGrpcService>
{
    UnaryResult<OperationResult> CreateProject(CreateProjectCommand command);
    
    UnaryResult<GetProjectByIdQueryViewModel> GetProjectById(GetProjectByIdQuery query);

    UnaryResult<ListProjectQueryViewModel> ListProject(ListProjectQuery query);    
    
    UnaryResult<OperationResult> RemoveProject(RemoveProjectCommand command);
    
    UnaryResult<OperationResult> UpdateProject(UpdateProjectCommand command);
}