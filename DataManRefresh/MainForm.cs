using DataManRefresh.Discovery;

namespace DataManRefresh;

/// <summary>
/// DataMan Setup Tool Connect 페이지의 Refresh 동작을 재현한다.
/// Refresh 클릭 → 목록 Clear → Discover() → SystemDiscovered로 장치 추가.
/// </summary>
public sealed class MainForm : Form
{
    private readonly ISystemDiscoverer _discoverer;
    private readonly ListView _deviceList;
    private readonly Button _btnRefresh;
    private readonly Label _statusLabel;
    private readonly ToolStripStatusLabel _deviceCountLabel;

    public MainForm()
    {
        Text = "DataMan Device Connect";
        Width = 920;
        Height = 520;
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(720, 400);
        Font = new Font("Segoe UI", 9F);

        // Cognex SDK 사용 시: new CognexSystemDiscoverer()
        _discoverer = new MockSystemDiscoverer();
        _discoverer.SystemDiscovered += OnSystemDiscovered;

        var toolbar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 48,
            Padding = new Padding(12, 8, 12, 8)
        };

        _btnRefresh = new Button
        {
            Text = "Refresh",
            Width = 110,
            Height = 30,
            Left = 12,
            Top = 8,
            Anchor = AnchorStyles.Top | AnchorStyles.Left
        };
        _btnRefresh.Click += BtnRefresh_Click;

        _statusLabel = new Label
        {
            AutoSize = true,
            Left = 140,
            Top = 14,
            Text = "Click Refresh to search for DataMan devices (6 barcode readers)."
        };

        toolbar.Controls.Add(_btnRefresh);
        toolbar.Controls.Add(_statusLabel);

        _deviceList = new ListView
        {
            Dock = DockStyle.Fill,
            View = View.Details,
            FullRowSelect = true,
            GridLines = true,
            MultiSelect = false,
            HideSelection = false
        };
        _deviceList.Columns.Add("Name", 180);
        _deviceList.Columns.Add("Type", 130);
        _deviceList.Columns.Add("Connection", 110);
        _deviceList.Columns.Add("IP / Port", 140);
        _deviceList.Columns.Add("Firmware", 100);
        _deviceList.Columns.Add("Status", 100);

        var statusStrip = new StatusStrip();
        _deviceCountLabel = new ToolStripStatusLabel("Devices: 0");
        statusStrip.Items.Add(_deviceCountLabel);

        Controls.Add(_deviceList);
        Controls.Add(toolbar);
        Controls.Add(statusStrip);

        // Setup Tool처럼 시작 시 한 번 검색
        Shown += (_, _) => RefreshDevices();
        FormClosed += (_, _) => _discoverer.Dispose();
    }

    private void BtnRefresh_Click(object? sender, EventArgs e) => RefreshDevices();

    /// <summary>
    /// DataMan Setup Tool Refresh와 동일:
    /// 1) 목록 Clear
    /// 2) Discover() 재호출 (Ethernet + Serial)
    /// </summary>
    private void RefreshDevices()
    {
        _deviceList.Items.Clear();
        UpdateDeviceCount();
        _statusLabel.Text = "Searching network and serial ports…";
        _btnRefresh.Enabled = false;

        try
        {
            _discoverer.Discover();
        }
        finally
        {
            // Discover는 비동기로 이벤트를 올리므로, 버튼은 잠시 후 다시 활성화
            var reenable = new System.Windows.Forms.Timer { Interval = 700 };
            reenable.Tick += (_, _) =>
            {
                reenable.Stop();
                reenable.Dispose();
                if (!IsDisposed)
                {
                    _btnRefresh.Enabled = true;
                    _statusLabel.Text = $"Found {_deviceList.Items.Count} device(s). Click Refresh to rescan.";
                }
            };
            reenable.Start();
        }
    }

    private void OnSystemDiscovered(object? sender, DiscoveredDevice device)
    {
        if (IsDisposed)
        {
            return;
        }

        // Cognex SystemDiscovered는 백그라운드에서 올 수 있음 → UI 스레드로 마샬링
        if (InvokeRequired)
        {
            BeginInvoke(() => AddDevice(device));
            return;
        }

        AddDevice(device);
    }

    private void AddDevice(DiscoveredDevice device)
    {
        var endpoint = device.IpAddress
            ?? (device.PortName is null ? device.Connection : $"{device.PortName} @ {device.Baudrate}");

        var item = new ListViewItem(device.Name);
        item.SubItems.Add(device.Type);
        item.SubItems.Add(device.Connection);
        item.SubItems.Add(endpoint);
        item.SubItems.Add(device.Firmware);
        item.SubItems.Add(device.Status);
        item.Tag = device;

        _deviceList.Items.Add(item);
        UpdateDeviceCount();
    }

    private void UpdateDeviceCount()
    {
        _deviceCountLabel.Text = $"Devices: {_deviceList.Items.Count}";
    }
}
