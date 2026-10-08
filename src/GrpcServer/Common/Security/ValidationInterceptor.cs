using Grpc.Core;
using Grpc.Core.Interceptors;
using Status = Grpc.Core.Status;
using StatusCode = Grpc.Core.StatusCode;

namespace GrpcServer.Common.Security;

/// <summary>
/// Times every call (the counterpart of WebApi's ElapsedTimeMiddleware) and translates the shared
/// domain, access and database errors into gRPC statuses with the same client-safe messages the
/// REST API returns.
/// </summary>
public sealed class ValidationInterceptor(ILogger<ValidationInterceptor> logger) : Interceptor
{
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
    {
        var started = System.Diagnostics.Stopwatch.GetTimestamp();
        try { return await Translate(request, context, continuation); }
        finally
        {
            logger.LogInformation("gRPC {Method} executed in {ElapsedTime} ms.", context.Method,
                System.Diagnostics.Stopwatch.GetElapsedTime(started).TotalMilliseconds);
        }
    }

    private async Task<TResponse> Translate<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        where TRequest : class
        where TResponse : class
    {
        try { return await continuation(request, context); }
        catch (DomainException ex)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, ex.Message));
        }
        catch (AccessDeniedException ex)
        {
            throw new RpcException(new Status(StatusCode.PermissionDenied, ex.Message));
        }
        catch (RecordNotFoundException ex)
        {
            throw new RpcException(new Status(StatusCode.NotFound, ex.Message));
        }
        catch (ArgumentException)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "The command contains an invalid value."));
        }
        catch (Exception ex) when (DatabaseErrors.Classify(ex) is { } error)
        {
            logger.LogInformation("Command rejected by the database: {Kind}", error.Kind);
            throw new RpcException(new Status(error.Kind switch
            {
                DatabaseErrorKind.Duplicate => StatusCode.AlreadyExists,
                DatabaseErrorKind.ActiveDependents => StatusCode.FailedPrecondition,
                DatabaseErrorKind.ConcurrentChange => StatusCode.Aborted,
                _ => StatusCode.InvalidArgument
            }, error.Message));
        }
    }
}
