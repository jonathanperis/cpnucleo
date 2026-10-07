namespace WebApi.Common.Models;

/// <summary>
/// Shared response model for restoring removed entities.
/// </summary>
public class RestoreResponse
{
    /// <summary>
    /// Gets or sets a value indicating whether the restore was successful.
    /// </summary>
    public bool Success { get; set; }
}
