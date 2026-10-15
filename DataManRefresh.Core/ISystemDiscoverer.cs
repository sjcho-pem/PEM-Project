namespace DataManRefresh.Discovery;

/// <summary>
/// Cognex DataMan Discovery API 패턴.
/// Refresh = Clear + Discover() 재호출.
/// </summary>
public interface ISystemDiscoverer : IDisposable
{
    event EventHandler<DiscoveredDevice>? SystemDiscovered;

    /// <summary>
    /// Setup Tool Refresh 버튼과 동일: 네트워크/시리얼을 다시 스캔한다.
    /// </summary>
    void Discover();
}
