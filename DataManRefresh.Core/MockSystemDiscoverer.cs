namespace DataManRefresh.Discovery;

/// <summary>
/// Cognex SDK 없이 Setup Tool Refresh 동작을 재현하는 목 디스커버러.
/// 바코드 리더 6대를 가정한다.
/// </summary>
public sealed class MockSystemDiscoverer : ISystemDiscoverer
{
    private static readonly DiscoveredDevice[] BarcodeReaders =
    [
        new()
        {
            Name = "DM370-LINE-01",
            Type = "DataMan 370",
            Connection = "Ethernet",
            IpAddress = "192.168.1.101",
            Firmware = "6.1.10",
            Status = "Available"
        },
        new()
        {
            Name = "DM370-LINE-02",
            Type = "DataMan 370",
            Connection = "Ethernet",
            IpAddress = "192.168.1.102",
            Firmware = "6.1.10",
            Status = "Available"
        },
        new()
        {
            Name = "DM280-STATION-A",
            Type = "DataMan 280",
            Connection = "Ethernet",
            IpAddress = "192.168.1.110",
            Firmware = "5.7.3",
            Status = "Available"
        },
        new()
        {
            Name = "DM8600-HAND-01",
            Type = "DataMan 8600",
            Connection = "USB Serial",
            PortName = "COM3",
            Baudrate = 115200,
            Firmware = "5.6.3",
            Status = "Available"
        },
        new()
        {
            Name = "DM8050-HAND-02",
            Type = "DataMan 8050",
            Connection = "USB Serial",
            PortName = "COM5",
            Baudrate = 115200,
            Firmware = "5.6.3",
            Status = "Available"
        },
        new()
        {
            Name = "DM474-FIXED-06",
            Type = "DataMan 474",
            Connection = "Ethernet",
            IpAddress = "192.168.1.120",
            Firmware = "6.2.0",
            Status = "Available"
        }
    ];

    private CancellationTokenSource? _cts;
    private readonly object _gate = new();
    private bool _disposed;

    public event EventHandler<DiscoveredDevice>? SystemDiscovered;

    public void Discover()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        lock (_gate)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            // Cognex Discover()처럼 비동기로 SystemDiscovered를 순차 발생시킨다.
            _ = Task.Run(async () =>
            {
                try
                {
                    foreach (var device in BarcodeReaders)
                    {
                        token.ThrowIfCancellationRequested();
                        await Task.Delay(80, token).ConfigureAwait(false);
                        SystemDiscovered?.Invoke(this, device);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Refresh를 연속 클릭하면 이전 검색을 취소한다.
                }
            }, token);
        }
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        lock (_gate)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
