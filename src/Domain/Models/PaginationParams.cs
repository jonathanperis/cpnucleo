using System.Globalization;

namespace Domain.Models;

/// <summary>
/// Paging, search, sorting and filters for list queries. Setters only store values: binders and
/// serializers (FastEndpoints query binding, MessagePack over gRPC) must be able to create the
/// object from any input. <see cref="Problems()"/> and <see cref="Require(PaginationParams?)"/> enforce the bounds, so
/// both transports reject the same invalid requests with the same field-level messages instead of
/// failing inside a deserializer.
/// </summary>
public class PaginationParams
{
    public const int MaximumPageSize = 100;
    public const int MaximumPageNumber = int.MaxValue / MaximumPageSize;
    public const int MaximumSearchLength = 128;
    private int? _pageNumber;
    private int? _pageSize;
    private string? _sortColumn;
    private string? _sortOrder;
    private string? _search;

    // ISO-8601 dates and date-times; "K" accepts "Z", an offset or nothing (treated as UTC).
    private static readonly string[] InstantFormats =
        ["yyyy-MM-dd", "yyyy-MM-dd'T'HH:mmK", "yyyy-MM-dd'T'HH:mm:ssK", "yyyy-MM-dd'T'HH:mm:ss.FFFFFFFK"];

    /// <summary>Comma-separated UUIDs (at most <see cref="MaximumPageSize"/>) restricting the result.</summary>
    public string? Ids { get; set; }

    // Relation filters restrict the result to rows whose column of the same name equals one UUID; a
    // resource without that column rejects the filter. Like Ids they are strings, so any input binds
    // and Problems reports it.
    public string? OrganizationId { get; set; }
    public string? ProjectId { get; set; }
    public string? AssignmentId { get; set; }
    public string? UserId { get; set; }
    public string? WorkflowId { get; set; }

    /// <summary>Inclusive lower bound (ISO-8601, normalized to UTC) of the resource's date or period.</summary>
    public string? DateFrom { get; set; }

    /// <summary>Exclusive upper bound (ISO-8601, normalized to UTC) of the resource's date or period.</summary>
    public string? DateTo { get; set; }

    public string? Search
    {
        get => _search;
        set => _search = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public int? PageNumber
    {
        get => _pageNumber ?? 1;
        set => _pageNumber = value;
    }

    public int? PageSize
    {
        get => _pageSize ?? 10;
        set => _pageSize = value;
    }

    public string? SortColumn
    {
        get => _sortColumn ?? "Id";
        set => _sortColumn = value;
    }

    public string? SortOrder
    {
        get => (_sortOrder?.ToUpperInvariant() == "DESC") ? "DESC" : "ASC";
        set => _sortOrder = value;
    }

    public int Offset => (PageNumber.GetValueOrDefault(1) - 1) * PageSize.GetValueOrDefault(10);

    /// <summary>
    /// Every bound the values violate, keyed by the flat query field name (<c>pageNumber</c>,
    /// <c>pageSize</c>, <c>search</c>, <c>ids</c>, <c>projectId</c>, <c>dateFrom</c>, ...). Empty when
    /// the request is valid.
    /// </summary>
    public IReadOnlyList<(string Field, string Message)> Problems()
    {
        var problems = new List<(string, string)>();
        if (PageNumber is < 1 or > MaximumPageNumber)
            problems.Add(("pageNumber", $"PageNumber must be between 1 and {MaximumPageNumber}."));
        if (PageSize is < 1 or > MaximumPageSize)
            problems.Add(("pageSize", $"PageSize must be between 1 and {MaximumPageSize}."));
        if (_search?.Length > MaximumSearchLength)
            problems.Add(("search", $"Search must be at most {MaximumSearchLength} characters."));
        var ids = SplitIds();
        if (ids.Length > MaximumPageSize || ids.Any(id => !Guid.TryParse(id, out _)))
            problems.Add(("ids", $"Ids must contain at most {MaximumPageSize} comma-separated UUIDs."));
        foreach (var (column, value) in RelationValues())
            if (!Guid.TryParse(value, out _)) problems.Add((FieldOf(column), $"{column} must be a UUID."));
        var fromIsValid = TryParseInstant(DateFrom, out var from);
        var toIsValid = TryParseInstant(DateTo, out var to);
        if (!fromIsValid) problems.Add(("dateFrom", "DateFrom must be an ISO-8601 date and time."));
        if (!toIsValid) problems.Add(("dateTo", "DateTo must be an ISO-8601 date and time."));
        if (from > to) problems.Add(("dateTo", "DateTo must be on or after DateFrom."));
        return problems;
    }

    /// <summary>
    /// <see cref="Problems()"/> plus every filter the rows of <paramref name="entityType"/> can't
    /// apply. Unsupported filters are rejected, never ignored: ignoring one would return more rows
    /// than the client asked for.
    /// </summary>
    public IReadOnlyList<(string Field, string Message)> Problems(Type entityType)
    {
        var problems = Problems().ToList();
        var resource = entityType.Name + "s";
        foreach (var (column, _) in RelationValues().Where(filter => !SupportsRelation(entityType, filter.Column)))
            problems.Add((FieldOf(column), $"{resource} cannot be filtered by {FieldOf(column)}."));
        if (DateColumnsOf(entityType) is null)
        {
            if (!string.IsNullOrWhiteSpace(DateFrom)) problems.Add(("dateFrom", $"{resource} cannot be filtered by dateFrom."));
            if (!string.IsNullOrWhiteSpace(DateTo)) problems.Add(("dateTo", $"{resource} cannot be filtered by dateTo."));
        }

        return problems;
    }

    /// <summary>Returns <paramref name="pagination"/> when present and valid, otherwise throws a <see cref="DomainException"/>.</summary>
    public static PaginationParams Require(PaginationParams? pagination)
    {
        if (pagination is null) throw new DomainException("Pagination is required.", "pagination");
        if (pagination.Problems() is [var first, ..]) throw new DomainException(first.Message, first.Field);
        return pagination;
    }

    /// <summary>
    /// Like <see cref="Require(PaginationParams?)"/>, and also rejects filters the rows of
    /// <paramref name="entityType"/> don't support.
    /// </summary>
    public static PaginationParams Require(PaginationParams? pagination, Type entityType)
    {
        if (pagination is null) throw new DomainException("Pagination is required.", "pagination");
        if (pagination.Problems(entityType) is [var first, ..]) throw new DomainException(first.Message, first.Field);
        return pagination;
    }

    /// <summary>True when the rows have a UUID column named <paramref name="column"/>.</summary>
    public static bool SupportsRelation(Type entityType, string column) =>
        entityType.GetProperty(column, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)?.PropertyType == typeof(Guid);

    /// <summary>
    /// The columns a date range applies to: a single date (<c>KeepDate</c>) or a period
    /// (<c>StartDate</c>..<c>EndDate</c>, which matches when it overlaps the range). Null when the
    /// rows have neither.
    /// </summary>
    public static (string Start, string End)? DateColumnsOf(Type entityType)
    {
        bool IsDate(string name) => entityType.GetProperty(name)?.PropertyType == typeof(DateTime);
        if (IsDate("KeepDate")) return ("KeepDate", "KeepDate");
        if (IsDate("StartDate") && IsDate("EndDate")) return ("StartDate", "EndDate");
        return null;
    }

    /// <summary>The relation filters present, as canonical column names and values; call after <see cref="Require(PaginationParams?)"/>.</summary>
    public IReadOnlyList<(string Column, Guid Value)> GetRelationFilters() => RelationValues()
        .Select(filter => (filter.Column, Guid.TryParse(filter.Value, out var id) ? id : Guid.Empty))
        .ToArray();

    /// <summary>The UTC bounds of the date range (from inclusive, to exclusive); call after <see cref="Require(PaginationParams?)"/>.</summary>
    public (DateTime? From, DateTime? To) GetDateRange() =>
        (TryParseInstant(DateFrom, out var from) ? from : null, TryParseInstant(DateTo, out var to) ? to : null);

    /// <summary>The flat query field name of a filter column (<c>ProjectId</c> is <c>projectId</c>).</summary>
    public static string FieldOf(string column) => char.ToLowerInvariant(column[0]) + column[1..];

    /// <summary>The distinct UUIDs of <see cref="Ids"/>; call after <see cref="Require(PaginationParams?)"/>.</summary>
    public Guid[] GetIds() => SplitIds()
        .Select(value => Guid.TryParse(value, out var id) ? id : Guid.Empty)
        .Where(id => id != Guid.Empty)
        .Distinct().ToArray();

    /// <summary>
    /// The search text as a case-insensitive "contains" LIKE pattern, with LIKE wildcards escaped
    /// so <c>%</c>, <c>_</c> and <c>\</c> match literally. Use with <c>ESCAPE '\'</c>.
    /// </summary>
    public string? GetSearchPattern() => _search is null
        ? null
        : "%" + _search.Replace("\\", "\\\\").Replace("%", "\\%").Replace("_", "\\_") + "%";

    private string[] SplitIds() =>
        Ids?.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? [];

    private IEnumerable<(string Column, string Value)> RelationValues()
    {
        (string Column, string? Value)[] values =
        [
            (nameof(OrganizationId), OrganizationId), (nameof(ProjectId), ProjectId), (nameof(AssignmentId), AssignmentId),
            (nameof(UserId), UserId), (nameof(WorkflowId), WorkflowId)
        ];
        return values.Where(filter => !string.IsNullOrWhiteSpace(filter.Value)).Select(filter => (filter.Column, filter.Value!.Trim()));
    }

    /// <summary>Parses an optional ISO-8601 instant as UTC; blank values are valid and mean "no bound".</summary>
    private static bool TryParseInstant(string? value, out DateTime? instant)
    {
        instant = null;
        if (string.IsNullOrWhiteSpace(value)) return true;
        if (!DateTimeOffset.TryParseExact(value.Trim(), InstantFormats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsed))
            return false;
        instant = parsed.UtcDateTime;
        return true;
    }
}
