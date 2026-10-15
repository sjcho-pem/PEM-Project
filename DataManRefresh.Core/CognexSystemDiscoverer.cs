#if USE_COGNEX_SDK
using Cognex.DataMan.Discovery;

namespace DataManRefresh.Discovery;

/// <summary>
/// 실제 Cognex DataMan SDK Refresh 구현.
/// Setup Tool Refresh = EthSystemDiscoverer.Discover() + SerSystemDiscoverer.Discover()
/// </summary>
public sealed class CognexSystemDiscoverer : ISystemDiscoverer
{
    private readonly EthSystemDiscoverer _eth = new();
    private readonly SerSystemDiscoverer _ser = new();

    public event EventHandler<DiscoveredDevice>? SystemDiscovered;

    public CognexSystemDiscoverer()
    {
        _eth.SystemDiscovered += OnEthDiscovered;
        _ser.SystemDiscovered += OnSerDiscovered;
    }

    public void Discover()
    {
        // DataMan Setup Tool Refresh와 동일한 동작
        _eth.Discover();
        _ser.Discover();
    }

    private void OnEthDiscovered(EthSystemDiscoverer.SystemInfo info)
    {
        SystemDiscovered?.Invoke(this, new DiscoveredDevice
        {
            Name = info.Name,
            Type = info.Type.ToString(),
            Connection = "Ethernet",
            IpAddress = info.IPAddress?.ToString(),
            Firmware = info.Version ?? string.Empty,
            Status = "Available"
        });
    }

    private void OnSerDiscovered(SerSystemDiscoverer.SystemInfo info)
    {
        SystemDiscovered?.Invoke(this, new DiscoveredDevice
        {
            Name = info.Name,
            Type = info.Type.ToString(),
            Connection = "USB/RS-232",
            PortName = info.PortName,
            Baudrate = info.Baudrate,
            Status = "Available"
        });
    }

    public void Dispose()
    {
        _eth.SystemDiscovered -= OnEthDiscovered;
        _ser.SystemDiscovered -= OnSerDiscovered;
        _eth.Dispose();
        _ser.Dispose();
    }
}
#else
namespace DataManRefresh.Discovery;

/// <summary>
/// Cognex DLL 미참조 시 빌드용 스텁.
/// 실제 장비 연동: csproj에 USE_COGNEX_SDK 정의 + Cognex Discovery DLL 참조 후
/// CognexSystemDiscoverer 를 사용한다.
/// </summary>
public static class CognexSystemDiscovererNotes
{
    // Refresh API 요약:
    //   using Cognex.DataMan.Discovery;
    //   var eth = new EthSystemDiscoverer();
    //   eth.SystemDiscovered += (sender, info) => { /* list add */ };
    //   eth.Discover();  // <-- Setup Tool Refresh
    //
    //   var ser = new SerSystemDiscoverer();
    //   ser.SystemDiscovered += (sender, info) => { /* list add */ };
    //   ser.Discover();  // <-- USB/Serial Refresh
}
#endif
