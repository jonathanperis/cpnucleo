namespace WebApi.Unit.Tests.Common.Services;

public class ListingChangeNotifierTests
{
    private const string Projects = nameof(Domain.Entities.Project);
    private const string Workflows = nameof(Domain.Entities.Workflow);

    [Test]
    public async Task WaitForChangeAsync_CompletesAllSubscribers_WhenListingChanges()
    {
        var notifier = new ListingChangeNotifier();
        var observedVersion = notifier.CurrentVersion(Projects);

        var firstSubscriber = notifier.WaitForChangeAsync(Projects, observedVersion, TestContext.CurrentContext.CancellationToken);
        var secondSubscriber = notifier.WaitForChangeAsync(Projects, observedVersion, TestContext.CurrentContext.CancellationToken);

        notifier.NotifyChanged(Projects);

        var nextVersion = await firstSubscriber;
        nextVersion.ShouldBeGreaterThan(observedVersion);
        (await secondSubscriber).ShouldBe(nextVersion);
    }

    [Test]
    public async Task WaitForChangeAsync_ReturnsImmediately_WhenVersionAlreadyChanged()
    {
        var notifier = new ListingChangeNotifier();
        var observedVersion = notifier.CurrentVersion(Projects);

        notifier.NotifyChanged(Projects);

        var nextVersion = await notifier.WaitForChangeAsync(Projects, observedVersion, TestContext.CurrentContext.CancellationToken);

        nextVersion.ShouldBeGreaterThan(observedVersion);
    }

    [Test]
    public async Task NotifyChanged_OnlyWakesStreamsOfTheChangedResources()
    {
        var notifier = new ListingChangeNotifier();
        var projects = notifier.WaitForChangeAsync(Projects, notifier.CurrentVersion(Projects), TestContext.CurrentContext.CancellationToken);
        var workflows = notifier.WaitForChangeAsync(Workflows, notifier.CurrentVersion(Workflows), TestContext.CurrentContext.CancellationToken);

        notifier.NotifyChanged(Projects);

        (await projects).ShouldBe(1);
        workflows.IsCompleted.ShouldBeFalse();
        await Should.ThrowAsync<TimeoutException>(() =>
            notifier.WaitForChangeAsync(Workflows, 0, TestContext.CurrentContext.CancellationToken, TimeSpan.FromMilliseconds(50)));
    }
}
