namespace WebApi.Common.Models;

public class RemoveRequest
{
    public List<Guid> Ids { get; set; } = [];
}

/// <summary>
/// Shared rules for batch removals. FastEndpoints binds validators by the exact request type, so
/// every removal request declares <c>Validator : RemoveRequestValidator&lt;ItsRequest&gt;</c>.
/// </summary>
public abstract class RemoveRequestValidator<TRequest> : Validator<TRequest> where TRequest : RemoveRequest
{
    protected RemoveRequestValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage("Ids are required.")
            .Must(ids => ids.Distinct().Count() <= BatchIds.MaximumCount)
            .WithMessage($"At most {BatchIds.MaximumCount} ids can be removed at once.");
        RuleForEach(x => x.Ids)
            .NotEmpty().WithMessage("Each Id is required.");
    }
}
