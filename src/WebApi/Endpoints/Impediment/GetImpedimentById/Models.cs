namespace WebApi.Endpoints.Impediment.GetImpedimentById;

/// <summary>
/// Request model for fetching an impediment by its unique identifier.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the impediment.
    /// </summary>
    [DefaultValue("8de21ef6-19a3-41ee-b4cd-ea3fae2e91c9")]
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
/// Response model for the impediment fetched by its unique identifier.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the impediment details.
    /// </summary>
    public ImpedimentDto? Impediment { get; set; }
}
