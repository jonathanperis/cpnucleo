using WebApi.Common.Extensions;

namespace WebApi.Unit.Tests.Common.Extensions;

public class ListingSseExtensionsTests
{
    [TestCase("text/event-stream", true)]
    [TestCase("application/json, text/event-stream;q=0.5", true)]
    [TestCase("TEXT/EVENT-STREAM", true)]
    [TestCase("text/event-stream;q=0", false)]
    [TestCase("application/json", false)]
    [TestCase("not a media type", false)]
    [TestCase("", false)]
    public void AcceptsServerSentEvents_HonorsMediaTypeAndQuality(string accept, bool expected)
    {
        var context = new DefaultHttpContext();
        context.Request.Headers.Accept = accept;

        context.Request.AcceptsServerSentEvents().ShouldBe(expected);
    }

    [Test]
    public async Task ListingStream_StopsQuietlyWhenTheClientDisconnects()
    {
        using var cancellation = new CancellationTokenSource();
        var notifier = new ListingChangeNotifier();
        var snapshots = 0;
        var stream = ListingSseExtensions.CreateListingStream("Project", _ => Task.FromResult(++snapshots), notifier,
            Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance, cancellation.Token);

        await using var enumerator = stream.GetAsyncEnumerator(cancellation.Token);
        (await enumerator.MoveNextAsync()).ShouldBeTrue();
        enumerator.Current.ShouldBe(1);

        notifier.NotifyChanged("Project");
        (await enumerator.MoveNextAsync()).ShouldBeTrue();
        enumerator.Current.ShouldBe(2);

        await cancellation.CancelAsync();
        (await enumerator.MoveNextAsync()).ShouldBeFalse();
    }
}
