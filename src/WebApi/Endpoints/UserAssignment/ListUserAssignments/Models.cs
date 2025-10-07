namespace WebApi.Endpoints.UserAssignment.ListUserAssignments;

/// <summary>
/// Request model for listing userAssignments.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    public required PaginationParams Pagination { get; set; }
    
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination)
                .NotNull().WithMessage("Pagination is required.");;
        }
    }    
}

/// <summary>
/// Response model for the list of userAssignments.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of userAssignments.
    /// </summary>
    public required PaginatedResult<UserAssignmentDto?> Result { get; set; }
}
