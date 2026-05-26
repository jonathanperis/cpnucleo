namespace WebApi.Unit.Tests.Common.Services;

public class ListingChangeNotifierTests
{
    [Test]
    public async Task WaitForChangeAsync_CompletesAllSubscribers_WhenListingChanges()
    {
        var notifier = new ListingChangeNotifier();
        var observedVersion = notifier.CurrentVersion;

        var firstSubscriber = notifier.WaitForChangeAsync(observedVersion, TestContext.CurrentContext.CancellationToken);
        var secondSubscriber = notifier.WaitForChangeAsync(observedVersion, TestContext.CurrentContext.CancellationToken);

        notifier.NotifyChanged();

        var nextVersion = await firstSubscriber;
        nextVersion.ShouldBeGreaterThan(observedVersion);
        (await secondSubscriber).ShouldBe(nextVersion);
    }

    [Test]
    public async Task WaitForChangeAsync_ReturnsImmediately_WhenVersionAlreadyChanged()
    {
        var notifier = new ListingChangeNotifier();
        var observedVersion = notifier.CurrentVersion;

        notifier.NotifyChanged();

        var nextVersion = await notifier.WaitForChangeAsync(observedVersion, TestContext.CurrentContext.CancellationToken);

        nextVersion.ShouldBeGreaterThan(observedVersion);
    }
}
