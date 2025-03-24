namespace GrpcServer.Services;

internal class ImpedimentGrpcService(ISender sender) : ServiceBase<IImpedimentGrpcService>, IImpedimentGrpcService
{
    public async UnaryResult<OperationResult> CreateImpediment(CreateImpedimentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<GetImpedimentByIdQueryViewModel> GetImpedimentById(GetImpedimentByIdQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<ListImpedimentQueryViewModel> ListImpediment(ListImpedimentQuery query)
    {
        return await sender.Send(query);
    }

    public async UnaryResult<OperationResult> RemoveImpediment(RemoveImpedimentCommand command)
    {
        return await sender.Send(command);
    }

    public async UnaryResult<OperationResult> UpdateImpediment(UpdateImpedimentCommand command)
    {
        return await sender.Send(command);
    }
}