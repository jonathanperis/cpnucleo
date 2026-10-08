namespace WebApi.Common.Models;

public class RestoreRequest
{
    public List<Guid> Ids { get; set; } = [];
}

/// <summary>
/// Shared rules for batch restores (the same bounds as removals). FastEndpoints binds validators by
/// the exact request type, so every restore request declares
/// <c>Validator : RestoreRequestValidator&lt;ItsRequest&gt;</c>.
/// </summary>
public abstract class RestoreRequestValidator<TRequest> : Validator<TRequest> where TRequest : RestoreRequest
{
    protected RestoreRequestValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage("Ids are required.")
            .Must(ids => ids.Distinct().Count() <= BatchIds.MaximumCount)
            .WithMessage($"At most {BatchIds.MaximumCount} ids can be restored at once.");
        RuleForEach(x => x.Ids)
            .NotEmpty().WithMessage("Each Id is required.");
    }
}
