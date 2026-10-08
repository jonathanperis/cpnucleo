using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Http;

/// <summary>
/// The single error envelope returned by the HTTP APIs. It matches FastEndpoints' validation
/// response (<c>statusCode</c>, <c>message</c>, <c>errors</c>), so every failure has one shape.
/// </summary>
public sealed record ApiErrorResponse(
    int StatusCode,
    string Message,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyDictionary<string, string[]>? Errors = null);

public static class ApiErrors
{
    public const string GeneralErrorsKey = "generalErrors";

    public static string DefaultMessage(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "The request is invalid.",
        StatusCodes.Status401Unauthorized => "Authentication is required.",
        StatusCodes.Status403Forbidden => "You do not have permission to perform this action.",
        StatusCodes.Status404NotFound => "The requested record was not found.",
        StatusCodes.Status405MethodNotAllowed => "The HTTP method is not allowed for this resource.",
        StatusCodes.Status409Conflict => "The request conflicts with the current state of the record.",
        StatusCodes.Status415UnsupportedMediaType => "The request content type is not supported.",
        StatusCodes.Status429TooManyRequests => "Rate limit exceeded. Please try again later.",
        _ when statusCode >= 500 => "An unexpected error occurred.",
        _ => "The request could not be completed."
    };

    public static Task WriteAsync(HttpContext context, int statusCode, string? message = null,
        IReadOnlyDictionary<string, string[]>? errors = null, CancellationToken cancellationToken = default)
    {
        context.Response.StatusCode = statusCode;
        return context.Response.WriteAsJsonAsync(
            new ApiErrorResponse(statusCode, message ?? DefaultMessage(statusCode), errors),
            cancellationToken);
    }

    /// <summary>
    /// The envelope for request validation failures; FastEndpoints' error response builder in both
    /// REST hosts. Field keys are camelCased per segment (as <see cref="FieldError"/>); unnamed
    /// failures are general errors.
    /// </summary>
    public static ApiErrorResponse ValidationResponse(int statusCode, IEnumerable<(string Field, string Message)> failures) =>
        new(statusCode, DefaultMessage(statusCode), failures
            .GroupBy(failure => string.IsNullOrWhiteSpace(failure.Field) ? GeneralErrorsKey : CamelCase(failure.Field))
            .ToDictionary(group => group.Key, group => group.Select(failure => failure.Message).ToArray()));

    private static string CamelCase(string field) =>
        string.Join('.', field.Split('.').Select(JsonNamingPolicy.CamelCase.ConvertName));

    /// <summary>Field errors keyed by camelCase request property name, or <see cref="GeneralErrorsKey"/>.</summary>
    public static IReadOnlyDictionary<string, string[]> FieldError(string? field, string message) =>
        new Dictionary<string, string[]>
        {
            [string.IsNullOrWhiteSpace(field) ? GeneralErrorsKey : JsonNamingPolicy.CamelCase.ConvertName(field)] = [message]
        };
}
