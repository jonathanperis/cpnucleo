namespace WebApi.Endpoints.Assignment.ListAssignments;

/// <summary>
/// Request model for listing assignments.
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
/// Response model for the list of assignments.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of assignments.
    /// </summary>
    public required PaginatedResult<AssignmentDto?> Result { get; set; }
}
