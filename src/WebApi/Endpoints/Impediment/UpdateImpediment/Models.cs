namespace WebApi.Endpoints.Impediment.UpdateImpediment;

/// <summary>
/// Request model for updating an impediment.
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
    [DefaultValue("Updated Impediment")]
    public required string Name { get; set; }

    public class Validator : Validator<Request>
    {
        public Validator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}

/// <summary>
/// Response model for the updated impediment.
/// </summary>
public class Response
{
    /// <summary>
    /// Gets or sets a value indicating whether the update was successful.
    /// </summary>
    public bool Success { get; set; }
}
