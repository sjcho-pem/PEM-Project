using DataManRefresh.Discovery;
using Xunit;

namespace DataManRefresh.Tests;

public class MockRefreshTests
{
    [Fact]
    public async Task Discover_emits_six_barcode_readers_like_setup_tool_refresh()
    {
        using var discoverer = new MockSystemDiscoverer();
        var found = new List<DiscoveredDevice>();
        var done = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        discoverer.SystemDiscovered += (_, device) =>
        {
            lock (found)
            {
                found.Add(device);
                if (found.Count >= 6)
                {
                    done.TrySetResult();
                }
            }
        };

        // Setup Tool Refresh: Clear + Discover()
        found.Clear();
        discoverer.Discover();

        await done.Task.WaitAsync(TimeSpan.FromSeconds(5));

        Assert.Equal(6, found.Count);
        Assert.All(found, d => Assert.False(string.IsNullOrWhiteSpace(d.Name)));
        Assert.Contains(found, d => d.IpAddress == "192.168.1.101");
        Assert.Contains(found, d => d.PortName == "COM3");
    }

    [Fact]
    public async Task Second_refresh_clears_and_rediscovers_six_devices()
    {
        using var discoverer = new MockSystemDiscoverer();
        var batches = new List<List<DiscoveredDevice>>();
        List<DiscoveredDevice> current = [];
        var first = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var second = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        discoverer.SystemDiscovered += (_, device) =>
        {
            lock (batches)
            {
                current.Add(device);
                if (current.Count == 6)
                {
                    batches.Add([.. current]);
                    if (batches.Count == 1)
                    {
                        first.TrySetResult();
                    }
                    else if (batches.Count == 2)
                    {
                        second.TrySetResult();
                    }
                }
            }
        };

        discoverer.Discover();
        await first.Task.WaitAsync(TimeSpan.FromSeconds(5));

        // Refresh again: UI clears list then Discover() again
        current = [];
        discoverer.Discover();
        await second.Task.WaitAsync(TimeSpan.FromSeconds(5));

        Assert.Equal(2, batches.Count);
        Assert.Equal(6, batches[0].Count);
        Assert.Equal(6, batches[1].Count);
    }
}
