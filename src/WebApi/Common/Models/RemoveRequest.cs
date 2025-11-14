namespace WebApi.Common.Models;

/// <summary>
/// Shared request model for removing entities.
/// </summary>
public class RemoveRequest
{
    /// <summary>
    /// Gets or sets the unique identifiers for the entities to be removed.
    /// </summary>
    public required List<Guid> Ids { get; set; }

    public class Validator : Validator<RemoveRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Ids)
                .NotEmpty().WithMessage("Ids are required.");
            RuleForEach(x => x.Ids)
                .NotEmpty().WithMessage("Each Id is required.");
        }
    }
}
