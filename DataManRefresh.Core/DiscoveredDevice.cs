namespace DataManRefresh.Discovery;

/// <summary>
/// DataMan Setup Tool 장치 목록에 표시되는 항목과 동일한 정보.
/// Cognex EthSystemDiscoverer.SystemInfo / SerSystemDiscoverer.SystemInfo 에 대응.
/// </summary>
public sealed class DiscoveredDevice
{
    public required string Name { get; init; }
    public required string Type { get; init; }
    public required string Connection { get; init; }
    public string? IpAddress { get; init; }
    public string? PortName { get; init; }
    public int? Baudrate { get; init; }
    public string Firmware { get; init; } = string.Empty;
    public string Status { get; init; } = "Available";

    public override string ToString()
    {
        var endpoint = IpAddress ?? PortName ?? Connection;
        return $"{Name} | {Type} | {endpoint}";
    }
}
