using Grpc.Core;
using Grpc.Core.Interceptors;
using Status = Grpc.Core.Status;
using StatusCode = Grpc.Core.StatusCode;

namespace GrpcServer.Common.Security;

public sealed class ValidationInterceptor : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try { return await continuation(request, context); }
        catch (ArgumentException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "The command contains an invalid value."));
        }
        catch (Npgsql.PostgresException ex) when (ex.SqlState == "23505")
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "A record with this identity already exists."));
        }
        catch (Npgsql.PostgresException ex) when (ex.SqlState is "23503" or "23514")
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "The relationship or value is invalid."));
        }
    }
}
