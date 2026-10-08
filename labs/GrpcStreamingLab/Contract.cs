using FastEndpoints;

namespace GrpcStreamingLab;

// The lab's own contract. MessagePack's contractless resolver (the FastEndpoints default)
// serializes public classes with public settable properties; no attributes are needed.

/// <summary>Opens a live projects listing: an initial snapshot, then one per observed change.</summary>
public sealed class WatchProjectsCommand : IServerStreamCommand<ProjectsSnapshotDto>;

public sealed class ProjectsSnapshotDto
{
    /// <summary>Per-stream counter of snapshots sent.</summary>
    public long Sequence { get; set; }

    /// <summary>What caused the refresh: initial, notify, fallback, resubscribe or degraded.</summary>
    public string Reason { get; set; } = "";

    /// <summary>How many NOTIFY messages this one refresh query absorbed.</summary>
    public int NotificationsCoalesced { get; set; }

    public int Total { get; set; }
    public List<ProjectDto> Items { get; set; } = [];
    public DateTime ServerTimeUtc { get; set; }
}

public sealed class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public DateTime UpdatedAt { get; set; }
}
