namespace WebApi.Common.Models;

public static class PaginationRules
{
    /// <summary>
    /// Requires the pagination and reports each violated bound under its flat query field name, so a
    /// bad listing request is a 400 before the endpoint (or a live stream) starts.
    /// </summary>
    public static IRuleBuilderOptionsConditions<T, PaginationParams> ValidPagination<T>(this IRuleBuilder<T, PaginationParams> rule) =>
        rule.Custom((pagination, context) =>
        {
            if (pagination is null)
            {
                context.AddFailure("pagination", "Pagination is required.");
                return;
            }

            foreach (var (field, message) in pagination.Problems()) context.AddFailure(field, message);
        });
}
