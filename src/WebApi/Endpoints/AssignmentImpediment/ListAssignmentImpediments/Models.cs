namespace WebApi.Endpoints.AssignmentImpediment.ListAssignmentImpediments;

/// <summary>
/// Request model for listing assignmentImpediments.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the pagination parameters for the request.
    /// </summary>
    [FromQuery]
    public PaginationParams Pagination { get; set; } = new();
        
    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Pagination).ValidPagination(typeof(Domain.Entities.AssignmentImpediment));
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
    public PaginatedResult<AssignmentImpedimentDto?> Result { get; set; } = null!;
}
