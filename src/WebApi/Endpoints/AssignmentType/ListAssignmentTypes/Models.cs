namespace WebApi.Endpoints.AssignmentType.ListAssignmentTypes;

/// <summary>
/// Request model for listing assignmentTypes.
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
            RuleFor(x => x.Pagination).ValidPagination(typeof(Domain.Entities.AssignmentType));
        }
    }     
}

/// <summary>
/// Response model for the list of assignmentTypes.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the paginated result of assignmentTypes.
    /// </summary>
    public PaginatedResult<AssignmentTypeDto?> Result { get; set; } = null!;
}
