namespace WebApi.Endpoints.UserAssignment.GetUserAssignmentById;

/// <summary>
/// Request model for fetching an userAssignment by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the userAssignment.
    /// </summary>
    [DefaultValue("ad45c2fd-0320-4cfc-b516-64da8f3cda11")]
    public Guid Id { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}

/// <summary>
/// Response model for the userAssignment fetched by its unique identifier.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the userAssignment details.
    /// </summary>
    public UserAssignmentDto? UserAssignment { get; set; }
}
