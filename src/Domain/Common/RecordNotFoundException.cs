namespace Domain.Common;

/// <summary>
/// The record does not exist or is not visible to the caller. Hidden and missing records are
/// indistinguishable on purpose: REST answers 404 and gRPC NotFound.
/// </summary>
public sealed class RecordNotFoundException() : Exception("The requested record was not found.");
