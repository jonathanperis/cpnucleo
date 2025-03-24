namespace GrpcServer.Services;

internal class ProjectGrpcService(ISender sender) : ServiceBase<IProjectGrpcService>, IProjectGrpcService
{
    public async UnaryResult<OperationResult> CreateProject(CreateProjectCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetProjectByIdQueryViewModel> GetProjectById(GetProjectByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListProjectQueryViewModel> ListProject(ListProjectQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveProject(RemoveProjectCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateProject(UpdateProjectCommand command)
    {
        return await sender.Send(command);
    }
}