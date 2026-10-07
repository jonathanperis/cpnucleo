namespace WebApi.Common.Models;

public static class PaginationRules
{
    /// <summary>
    /// Requires the pagination and reports each violated bound under its flat query field name, so a
    /// bad listing request is a 400 before the endpoint (or a live stream) starts. With
    /// <paramref name="entityType"/>, filters the listed rows can't apply are violations too.
    /// </summary>
    public static IRuleBuilderOptionsConditions<T, PaginationParams> ValidPagination<T>(this IRuleBuilder<T, PaginationParams> rule, Type? entityType = null) =>
        rule.Custom((pagination, context) =>
        {
            if (pagination is null)
            {
                context.AddFailure("pagination", "Pagination is required.");
                return;
            }

            var problems = entityType is null ? pagination.Problems() : pagination.Problems(entityType);
            foreach (var (field, message) in problems) context.AddFailure(field, message);
        });
}
