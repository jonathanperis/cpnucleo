namespace WebApi.Endpoints.AssignmentImpediment.ListAssignmentImpediments;

/// <summary>
/// Request model for listing assignmentImpediments.
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
/// Response model for the list of assignmentImpediments.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of assignmentImpediments.
    /// </summary>
    public required PaginatedResult<AssignmentImpedimentDto?> Result { get; set; }
}
