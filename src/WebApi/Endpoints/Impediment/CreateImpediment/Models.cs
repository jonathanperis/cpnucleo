namespace WebApi.Endpoints.Impediment.CreateImpediment;

/// <summary>
/// Request model for creating a new impediment.
/// </summary>
public class Request
{
    /// <summary>
    /// Gets or sets the unique identifier for the impediment.
    /// </summary>
    [DefaultValue("6da4950a-fac3-4578-8ee0-b05aa15eae35")]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the impediment.
    /// </summary>
    [DefaultValue("New Impediment")]
    public required string Name { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}

/// <summary>
/// Response model for the created impediment.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets the created impediment.
    /// </summary>
    public ImpedimentDto? Impediment { get; set; }
}
