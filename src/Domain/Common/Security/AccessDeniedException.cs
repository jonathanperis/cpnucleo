namespace Domain.Common.Security;

/// <summary>
/// The authenticated caller is not allowed to perform the operation.
/// REST maps it to HTTP 403 and gRPC to PermissionDenied.
/// </summary>
public sealed class AccessDeniedException(string message) : Exception(message);
