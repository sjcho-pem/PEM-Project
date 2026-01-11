namespace H_MarsSSD
{
    partial class frmSubItems
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.lbMcState = new System.Windows.Forms.Label();
			this.lbOkReset = new System.Windows.Forms.Label();
			this.lbYeild = new System.Windows.Forms.Label();
			this.lbNgReset = new System.Windows.Forms.Label();
			this.lbTotal = new System.Windows.Forms.Label();
			this.richMainMonitor = new System.Windows.Forms.RichTextBox();
			this.MachineCtrlButtonPanel = new System.Windows.Forms.Panel();
			this.progPause = new H_MarsSSD.ucProgressBar();
			this.progRun = new H_MarsSSD.ucProgressBar();
			this.btnTact = new System.Windows.Forms.Button();
			this.btnLotInfo = new System.Windows.Forms.Button();
			this.btnFlowMapInfo = new System.Windows.Forms.Button();
			this.btnBuzzer = new System.Windows.Forms.Button();
			this.btnReset = new System.Windows.Forms.Button();
			this.btnAllHome = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.btnRun = new System.Windows.Forms.Button();
			this.tmrSubItems = new System.Windows.Forms.Timer(this.components);
			this.panelEx1 = new System.Windows.Forms.Panel();
			this.btnSimLotInsert = new System.Windows.Forms.Button();
			this.gbConnectionState = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.lbSmartVision2State = new System.Windows.Forms.Label();
			this.lbSmartVision2Led = new System.Windows.Forms.Label();
			this.lbTrayBCRLed = new System.Windows.Forms.Label();
			this.lbSmartVision1Led = new System.Windows.Forms.Label();
			this.lbSmartVision1State = new System.Windows.Forms.Label();
			this.lbTrayBCRState = new System.Windows.Forms.Label();
			this.lbMESState = new System.Windows.Forms.Label();
			this.lbRevLed = new System.Windows.Forms.Label();
			this.lbBtmState = new System.Windows.Forms.Label();
			this.lbCtrlLed = new System.Windows.Forms.Label();
			this.lbTopState = new System.Windows.Forms.Label();
			this.lbControlState = new System.Windows.Forms.Label();
			this.lbTopLed = new System.Windows.Forms.Label();
			this.lbRevState = new System.Windows.Forms.Label();
			this.lbGEMLed = new System.Windows.Forms.Label();
			this.lbBtmLed = new System.Windows.Forms.Label();
			this.lbCurLotQty = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lbCurLotUserOK = new System.Windows.Forms.Label();
			this.lbCurLotYield = new System.Windows.Forms.Label();
			this.lbCurLotTotal = new System.Windows.Forms.Label();
			this.lbCurLotNG = new System.Windows.Forms.Label();
			this.lbCurLotOK = new System.Windows.Forms.Label();
			this.lbUserOKCount = new System.Windows.Forms.Label();
			this.lbUserOKReset = new System.Windows.Forms.Label();
			this.btnYieldClear = new System.Windows.Forms.Button();
			this.lbYeildCount = new System.Windows.Forms.Label();
			this.lbTotalCount = new System.Windows.Forms.Label();
			this.lbNgCount = new System.Windows.Forms.Label();
			this.lbOkCount = new System.Windows.Forms.Label();
			this.MachineCtrlButtonPanel.SuspendLayout();
			this.panelEx1.SuspendLayout();
			this.gbConnectionState.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbMcState
			// 
			this.lbMcState.BackColor = System.Drawing.Color.White;
			this.lbMcState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbMcState.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbMcState.ForeColor = System.Drawing.Color.Black;
			this.lbMcState.Location = new System.Drawing.Point(6, 143);
			this.lbMcState.Name = "lbMcState";
			this.lbMcState.Size = new System.Drawing.Size(277, 36);
			this.lbMcState.TabIndex = 11;
			this.lbMcState.Text = "IDLE";
			this.lbMcState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbOkReset
			// 
			this.lbOkReset.BackColor = System.Drawing.Color.Lime;
			this.lbOkReset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbOkReset.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lbOkReset.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbOkReset.Location = new System.Drawing.Point(5, 147);
			this.lbOkReset.Name = "lbOkReset";
			this.lbOkReset.Size = new System.Drawing.Size(69, 39);
			this.lbOkReset.TabIndex = 0;
			this.lbOkReset.Text = "GOOD";
			this.lbOkReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbYeild
			// 
			this.lbYeild.BackColor = System.Drawing.Color.White;
			this.lbYeild.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbYeild.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lbYeild.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbYeild.Location = new System.Drawing.Point(5, 311);
			this.lbYeild.Name = "lbYeild";
			this.lbYeild.Size = new System.Drawing.Size(69, 39);
			this.lbYeild.TabIndex = 5;
			this.lbYeild.Text = "YEILD";
			this.lbYeild.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbNgReset
			// 
			this.lbNgReset.BackColor = System.Drawing.Color.Red;
			this.lbNgReset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbNgReset.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lbNgReset.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbNgReset.Location = new System.Drawing.Point(5, 188);
			this.lbNgReset.Name = "lbNgReset";
			this.lbNgReset.Size = new System.Drawing.Size(69, 39);
			this.lbNgReset.TabIndex = 1;
			this.lbNgReset.Text = "NG";
			this.lbNgReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbTotal
			// 
			this.lbTotal.BackColor = System.Drawing.Color.White;
			this.lbTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lbTotal.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbTotal.Location = new System.Drawing.Point(5, 270);
			this.lbTotal.Name = "lbTotal";
			this.lbTotal.Size = new System.Drawing.Size(69, 39);
			this.lbTotal.TabIndex = 4;
			this.lbTotal.Text = "TOTAL";
			this.lbTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// richMainMonitor
			// 
			this.richMainMonitor.BackColor = System.Drawing.Color.White;
			this.richMainMonitor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.richMainMonitor.Location = new System.Drawing.Point(0, 678);
			this.richMainMonitor.Name = "richMainMonitor";
			this.richMainMonitor.ReadOnly = true;
			this.richMainMonitor.Size = new System.Drawing.Size(290, 203);
			this.richMainMonitor.TabIndex = 0;
			this.richMainMonitor.Text = "";
			// 
			// MachineCtrlButtonPanel
			// 
			this.MachineCtrlButtonPanel.BackColor = System.Drawing.Color.GhostWhite;
			this.MachineCtrlButtonPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.MachineCtrlButtonPanel.Controls.Add(this.progPause);
			this.MachineCtrlButtonPanel.Controls.Add(this.progRun);
			this.MachineCtrlButtonPanel.Controls.Add(this.btnTact);
			this.MachineCtrlButtonPanel.Controls.Add(this.btnLotInfo);
			this.MachineCtrlButtonPanel.Controls.Add(this.btnFlowMapInfo);
			this.MachineCtrlButtonPanel.Controls.Add(this.btnBuzzer);
			this.MachineCtrlButtonPanel.Controls.Add(this.btnReset);
			this.MachineCtrlButtonPanel.Controls.Add(this.btnAllHome);
			this.MachineCtrlButtonPanel.Controls.Add(this.btnStop);
			this.MachineCtrlButtonPanel.Controls.Add(this.btnPause);
			this.MachineCtrlButtonPanel.Controls.Add(this.btnRun);
			this.MachineCtrlButtonPanel.Controls.Add(this.lbMcState);
			this.MachineCtrlButtonPanel.Location = new System.Drawing.Point(0, 394);
			this.MachineCtrlButtonPanel.Name = "MachineCtrlButtonPanel";
			this.MachineCtrlButtonPanel.Size = new System.Drawing.Size(290, 287);
			this.MachineCtrlButtonPanel.TabIndex = 6;
			// 
			// progPause
			// 
			this.progPause.Location = new System.Drawing.Point(101, 266);
			this.progPause.Maximum = 1000;
			this.progPause.Minimum = 0;
			this.progPause.Name = "progPause";
			this.progPause.Size = new System.Drawing.Size(87, 11);
			this.progPause.TabIndex = 23;
			this.progPause.Value = 0;
			// 
			// progRun
			// 
			this.progRun.Location = new System.Drawing.Point(6, 266);
			this.progRun.Maximum = 1000;
			this.progRun.Minimum = 0;
			this.progRun.Name = "progRun";
			this.progRun.Size = new System.Drawing.Size(87, 11);
			this.progRun.TabIndex = 22;
			this.progRun.Value = 0;
			// 
			// btnTact
			// 
			this.btnTact.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this.btnTact.Location = new System.Drawing.Point(196, 7);
			this.btnTact.Name = "btnTact";
			this.btnTact.Size = new System.Drawing.Size(87, 43);
			this.btnTact.TabIndex = 21;
			this.btnTact.Text = "TACT";
			this.btnTact.UseVisualStyleBackColor = true;
			this.btnTact.Click += new System.EventHandler(this.btnTact_Click);
			// 
			// btnLotInfo
			// 
			this.btnLotInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this.btnLotInfo.Location = new System.Drawing.Point(6, 7);
			this.btnLotInfo.Name = "btnLotInfo";
			this.btnLotInfo.Size = new System.Drawing.Size(87, 43);
			this.btnLotInfo.TabIndex = 19;
			this.btnLotInfo.Text = "LOT\r\nINFO";
			this.btnLotInfo.UseVisualStyleBackColor = true;
			this.btnLotInfo.Click += new System.EventHandler(this.btnLotInfo_Click);
			// 
			// btnFlowMapInfo
			// 
			this.btnFlowMapInfo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this.btnFlowMapInfo.Location = new System.Drawing.Point(101, 7);
			this.btnFlowMapInfo.Name = "btnFlowMapInfo";
			this.btnFlowMapInfo.Size = new System.Drawing.Size(87, 43);
			this.btnFlowMapInfo.TabIndex = 18;
			this.btnFlowMapInfo.Text = "FLOW MAP\r\nINFO";
			this.btnFlowMapInfo.UseVisualStyleBackColor = true;
			this.btnFlowMapInfo.Click += new System.EventHandler(this.btnFlowMapInfo_Click);
			// 
			// btnBuzzer
			// 
			this.btnBuzzer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnBuzzer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnBuzzer.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnBuzzer.Image = global::H_MarsSSD.Properties.Resources.BellOff;
			this.btnBuzzer.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnBuzzer.Location = new System.Drawing.Point(101, 55);
			this.btnBuzzer.Name = "btnBuzzer";
			this.btnBuzzer.Size = new System.Drawing.Size(87, 82);
			this.btnBuzzer.TabIndex = 17;
			this.btnBuzzer.Text = "BUZZER\r\n";
			this.btnBuzzer.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnBuzzer.UseVisualStyleBackColor = true;
			this.btnBuzzer.Click += new System.EventHandler(this.btnBuzzer_Click);
			// 
			// btnReset
			// 
			this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnReset.Image = global::H_MarsSSD.Properties.Resources.Reset_36;
			this.btnReset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnReset.Location = new System.Drawing.Point(196, 55);
			this.btnReset.Name = "btnReset";
			this.btnReset.Size = new System.Drawing.Size(87, 82);
			this.btnReset.TabIndex = 16;
			this.btnReset.Text = "ALARM\r\nRESET";
			this.btnReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnReset.UseVisualStyleBackColor = true;
			this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
			// 
			// btnAllHome
			// 
			this.btnAllHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAllHome.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAllHome.ForeColor = System.Drawing.Color.Black;
			this.btnAllHome.Image = global::H_MarsSSD.Properties.Resources.HomeDisable;
			this.btnAllHome.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnAllHome.Location = new System.Drawing.Point(6, 55);
			this.btnAllHome.Name = "btnAllHome";
			this.btnAllHome.Size = new System.Drawing.Size(87, 82);
			this.btnAllHome.TabIndex = 15;
			this.btnAllHome.Text = "ALL HOME";
			this.btnAllHome.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnAllHome.UseVisualStyleBackColor = true;
			this.btnAllHome.Click += new System.EventHandler(this.btnAllHome_Click);
			// 
			// btnStop
			// 
			this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStop.Image = global::H_MarsSSD.Properties.Resources.StopOn_72p;
			this.btnStop.Location = new System.Drawing.Point(196, 185);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(87, 80);
			this.btnStop.TabIndex = 14;
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnPause
			// 
			this.btnPause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnPause.Image = global::H_MarsSSD.Properties.Resources.PauseOn_72p;
			this.btnPause.Location = new System.Drawing.Point(101, 185);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(87, 80);
			this.btnPause.TabIndex = 13;
			this.btnPause.UseVisualStyleBackColor = true;
			this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
			this.btnPause.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnPause_MouseDown);
			this.btnPause.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnPause_MouseUp);
			// 
			// btnRun
			// 
			this.btnRun.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRun.Image = global::H_MarsSSD.Properties.Resources.StartOn_72p;
			this.btnRun.Location = new System.Drawing.Point(6, 185);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(87, 80);
			this.btnRun.TabIndex = 12;
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
			// 
			// tmrSubItems
			// 
			this.tmrSubItems.Tick += new System.EventHandler(this.tmrSubItems_Tick);
			// 
			// panelEx1
			// 
			this.panelEx1.BackColor = System.Drawing.Color.GhostWhite;
			this.panelEx1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelEx1.Controls.Add(this.btnSimLotInsert);
			this.panelEx1.Controls.Add(this.gbConnectionState);
			this.panelEx1.Controls.Add(this.lbCurLotQty);
			this.panelEx1.Controls.Add(this.label7);
			this.panelEx1.Controls.Add(this.label6);
			this.panelEx1.Controls.Add(this.lbCurLotUserOK);
			this.panelEx1.Controls.Add(this.lbCurLotYield);
			this.panelEx1.Controls.Add(this.lbCurLotTotal);
			this.panelEx1.Controls.Add(this.lbCurLotNG);
			this.panelEx1.Controls.Add(this.lbCurLotOK);
			this.panelEx1.Controls.Add(this.lbUserOKCount);
			this.panelEx1.Controls.Add(this.lbUserOKReset);
			this.panelEx1.Controls.Add(this.btnYieldClear);
			this.panelEx1.Controls.Add(this.lbYeildCount);
			this.panelEx1.Controls.Add(this.lbTotalCount);
			this.panelEx1.Controls.Add(this.lbNgCount);
			this.panelEx1.Controls.Add(this.lbOkCount);
			this.panelEx1.Controls.Add(this.lbOkReset);
			this.panelEx1.Controls.Add(this.lbTotal);
			this.panelEx1.Controls.Add(this.lbNgReset);
			this.panelEx1.Controls.Add(this.lbYeild);
			this.panelEx1.Location = new System.Drawing.Point(0, 0);
			this.panelEx1.Name = "panelEx1";
			this.panelEx1.Size = new System.Drawing.Size(290, 397);
			this.panelEx1.TabIndex = 7;
			// 
			// btnSimLotInsert
			// 
			this.btnSimLotInsert.Location = new System.Drawing.Point(5, 355);
			this.btnSimLotInsert.Name = "btnSimLotInsert";
			this.btnSimLotInsert.Size = new System.Drawing.Size(67, 34);
			this.btnSimLotInsert.TabIndex = 36;
			this.btnSimLotInsert.Text = "Simul\r\nLot Insert";
			this.btnSimLotInsert.UseVisualStyleBackColor = true;
			this.btnSimLotInsert.Visible = false;
			this.btnSimLotInsert.Click += new System.EventHandler(this.btnSimLotInsert_Click);
			// 
			// gbConnectionState
			// 
			this.gbConnectionState.Controls.Add(this.tableLayoutPanel1);
			this.gbConnectionState.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.gbConnectionState.Location = new System.Drawing.Point(3, 6);
			this.gbConnectionState.Name = "gbConnectionState";
			this.gbConnectionState.Size = new System.Drawing.Size(282, 99);
			this.gbConnectionState.TabIndex = 35;
			this.gbConnectionState.TabStop = false;
			this.gbConnectionState.Text = "CONNECTION STATE";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 8;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Controls.Add(this.lbSmartVision2State, 5, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbSmartVision2Led, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbTrayBCRLed, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbSmartVision1Led, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbSmartVision1State, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbTrayBCRState, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbMESState, 7, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbRevLed, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbBtmState, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbCtrlLed, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbTopState, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbControlState, 7, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbTopLed, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbRevState, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.lbGEMLed, 6, 1);
			this.tableLayoutPanel1.Controls.Add(this.lbBtmLed, 2, 0);
			this.tableLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 23);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(277, 70);
			this.tableLayoutPanel1.TabIndex = 8;
			// 
			// lbSmartVision2State
			// 
			this.lbSmartVision2State.BackColor = System.Drawing.Color.White;
			this.lbSmartVision2State.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbSmartVision2State.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbSmartVision2State.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.lbSmartVision2State.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lbSmartVision2State.Location = new System.Drawing.Point(148, 35);
			this.lbSmartVision2State.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.lbSmartVision2State.Name = "lbSmartVision2State";
			this.lbSmartVision2State.Size = new System.Drawing.Size(57, 35);
			this.lbSmartVision2State.TabIndex = 34;
			this.lbSmartVision2State.Text = "SMART\r\nVISION2";
			this.lbSmartVision2State.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbSmartVision2State.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbState_MouseDoubleClick);
			// 
			// lbSmartVision2Led
			// 
			this.lbSmartVision2Led.BackColor = System.Drawing.Color.DarkGray;
			this.lbSmartVision2Led.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbSmartVision2Led.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbSmartVision2Led.Location = new System.Drawing.Point(138, 35);
			this.lbSmartVision2Led.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.lbSmartVision2Led.Name = "lbSmartVision2Led";
			this.lbSmartVision2Led.Size = new System.Drawing.Size(9, 35);
			this.lbSmartVision2Led.TabIndex = 33;
			// 
			// lbTrayBCRLed
			// 
			this.lbTrayBCRLed.BackColor = System.Drawing.Color.DarkGray;
			this.lbTrayBCRLed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbTrayBCRLed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbTrayBCRLed.Location = new System.Drawing.Point(0, 35);
			this.lbTrayBCRLed.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.lbTrayBCRLed.Name = "lbTrayBCRLed";
			this.lbTrayBCRLed.Size = new System.Drawing.Size(9, 35);
			this.lbTrayBCRLed.TabIndex = 32;
			// 
			// lbSmartVision1Led
			// 
			this.lbSmartVision1Led.BackColor = System.Drawing.Color.DarkGray;
			this.lbSmartVision1Led.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbSmartVision1Led.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbSmartVision1Led.Location = new System.Drawing.Point(69, 35);
			this.lbSmartVision1Led.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.lbSmartVision1Led.Name = "lbSmartVision1Led";
			this.lbSmartVision1Led.Size = new System.Drawing.Size(9, 35);
			this.lbSmartVision1Led.TabIndex = 31;
			// 
			// lbSmartVision1State
			// 
			this.lbSmartVision1State.BackColor = System.Drawing.Color.White;
			this.lbSmartVision1State.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbSmartVision1State.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbSmartVision1State.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.lbSmartVision1State.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lbSmartVision1State.Location = new System.Drawing.Point(79, 35);
			this.lbSmartVision1State.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.lbSmartVision1State.Name = "lbSmartVision1State";
			this.lbSmartVision1State.Size = new System.Drawing.Size(57, 35);
			this.lbSmartVision1State.TabIndex = 30;
			this.lbSmartVision1State.Text = "SMART\r\nVISION1";
			this.lbSmartVision1State.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbSmartVision1State.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbState_MouseDoubleClick);
			// 
			// lbTrayBCRState
			// 
			this.lbTrayBCRState.BackColor = System.Drawing.Color.White;
			this.lbTrayBCRState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbTrayBCRState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbTrayBCRState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.lbTrayBCRState.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lbTrayBCRState.Location = new System.Drawing.Point(10, 35);
			this.lbTrayBCRState.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.lbTrayBCRState.Name = "lbTrayBCRState";
			this.lbTrayBCRState.Size = new System.Drawing.Size(57, 35);
			this.lbTrayBCRState.TabIndex = 29;
			this.lbTrayBCRState.Text = "BRC";
			this.lbTrayBCRState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbTrayBCRState.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbState_MouseDoubleClick);
			// 
			// lbMESState
			// 
			this.lbMESState.BackColor = System.Drawing.Color.White;
			this.lbMESState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbMESState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbMESState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.lbMESState.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lbMESState.Location = new System.Drawing.Point(217, 35);
			this.lbMESState.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.lbMESState.Name = "lbMESState";
			this.lbMESState.Size = new System.Drawing.Size(58, 35);
			this.lbMESState.TabIndex = 27;
			this.lbMESState.Text = "SGEM";
			this.lbMESState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbMESState.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbState_MouseDoubleClick);
			// 
			// lbRevLed
			// 
			this.lbRevLed.BackColor = System.Drawing.Color.DarkGray;
			this.lbRevLed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbRevLed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbRevLed.Location = new System.Drawing.Point(138, 0);
			this.lbRevLed.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.lbRevLed.Name = "lbRevLed";
			this.lbRevLed.Size = new System.Drawing.Size(9, 35);
			this.lbRevLed.TabIndex = 27;
			// 
			// lbBtmState
			// 
			this.lbBtmState.BackColor = System.Drawing.Color.White;
			this.lbBtmState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbBtmState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbBtmState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.lbBtmState.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lbBtmState.Location = new System.Drawing.Point(79, 0);
			this.lbBtmState.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.lbBtmState.Name = "lbBtmState";
			this.lbBtmState.Size = new System.Drawing.Size(57, 35);
			this.lbBtmState.TabIndex = 24;
			this.lbBtmState.Text = "BTM\r\nVISION";
			this.lbBtmState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbBtmState.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbState_MouseDoubleClick);
			// 
			// lbCtrlLed
			// 
			this.lbCtrlLed.BackColor = System.Drawing.Color.DarkGray;
			this.lbCtrlLed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCtrlLed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbCtrlLed.Location = new System.Drawing.Point(207, 0);
			this.lbCtrlLed.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.lbCtrlLed.Name = "lbCtrlLed";
			this.lbCtrlLed.Size = new System.Drawing.Size(9, 35);
			this.lbCtrlLed.TabIndex = 18;
			// 
			// lbTopState
			// 
			this.lbTopState.BackColor = System.Drawing.Color.White;
			this.lbTopState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbTopState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbTopState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.lbTopState.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lbTopState.Location = new System.Drawing.Point(10, 0);
			this.lbTopState.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.lbTopState.Name = "lbTopState";
			this.lbTopState.Size = new System.Drawing.Size(57, 35);
			this.lbTopState.TabIndex = 21;
			this.lbTopState.Text = "TOP\r\nVISION";
			this.lbTopState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbTopState.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbState_MouseDoubleClick);
			// 
			// lbControlState
			// 
			this.lbControlState.BackColor = System.Drawing.Color.White;
			this.lbControlState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbControlState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbControlState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.lbControlState.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lbControlState.Location = new System.Drawing.Point(217, 0);
			this.lbControlState.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.lbControlState.Name = "lbControlState";
			this.lbControlState.Size = new System.Drawing.Size(58, 35);
			this.lbControlState.TabIndex = 25;
			this.lbControlState.Text = "WMX";
			this.lbControlState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbTopLed
			// 
			this.lbTopLed.BackColor = System.Drawing.Color.DarkGray;
			this.lbTopLed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbTopLed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbTopLed.Location = new System.Drawing.Point(0, 0);
			this.lbTopLed.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.lbTopLed.Name = "lbTopLed";
			this.lbTopLed.Size = new System.Drawing.Size(9, 35);
			this.lbTopLed.TabIndex = 20;
			// 
			// lbRevState
			// 
			this.lbRevState.BackColor = System.Drawing.Color.White;
			this.lbRevState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbRevState.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbRevState.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.lbRevState.ForeColor = System.Drawing.SystemColors.WindowText;
			this.lbRevState.Location = new System.Drawing.Point(148, 0);
			this.lbRevState.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
			this.lbRevState.Name = "lbRevState";
			this.lbRevState.Size = new System.Drawing.Size(57, 35);
			this.lbRevState.TabIndex = 26;
			this.lbRevState.Text = "RPS";
			this.lbRevState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbRevState.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbState_MouseDoubleClick);
			// 
			// lbGEMLed
			// 
			this.lbGEMLed.BackColor = System.Drawing.Color.DarkGray;
			this.lbGEMLed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbGEMLed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lbGEMLed.Location = new System.Drawing.Point(207, 35);
			this.lbGEMLed.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.lbGEMLed.Name = "lbGEMLed";
			this.lbGEMLed.Size = new System.Drawing.Size(9, 35);
			this.lbGEMLed.TabIndex = 28;
			// 
			// lbBtmLed
			// 
			this.lbBtmLed.BackColor = System.Drawing.Color.DarkGray;
			this.lbBtmLed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbBtmLed.Location = new System.Drawing.Point(69, 0);
			this.lbBtmLed.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.lbBtmLed.Name = "lbBtmLed";
			this.lbBtmLed.Size = new System.Drawing.Size(9, 35);
			this.lbBtmLed.TabIndex = 22;
			// 
			// lbCurLotQty
			// 
			this.lbCurLotQty.BackColor = System.Drawing.Color.White;
			this.lbCurLotQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCurLotQty.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lbCurLotQty.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbCurLotQty.Location = new System.Drawing.Point(130, 108);
			this.lbCurLotQty.Name = "lbCurLotQty";
			this.lbCurLotQty.Size = new System.Drawing.Size(44, 35);
			this.lbCurLotQty.TabIndex = 34;
			this.lbCurLotQty.Text = "0";
			this.lbCurLotQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.White;
			this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label7.ForeColor = System.Drawing.SystemColors.WindowText;
			this.label7.Location = new System.Drawing.Point(183, 108);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(98, 35);
			this.label7.TabIndex = 33;
			this.label7.Text = "DAILY TOTAL";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.White;
			this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label6.ForeColor = System.Drawing.SystemColors.WindowText;
			this.label6.Location = new System.Drawing.Point(82, 108);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(44, 35);
			this.label6.TabIndex = 30;
			this.label6.Text = "LOT";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbCurLotUserOK
			// 
			this.lbCurLotUserOK.BackColor = System.Drawing.Color.White;
			this.lbCurLotUserOK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCurLotUserOK.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbCurLotUserOK.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbCurLotUserOK.Location = new System.Drawing.Point(82, 229);
			this.lbCurLotUserOK.Name = "lbCurLotUserOK";
			this.lbCurLotUserOK.Size = new System.Drawing.Size(92, 39);
			this.lbCurLotUserOK.TabIndex = 32;
			this.lbCurLotUserOK.Text = "0";
			this.lbCurLotUserOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbCurLotYield
			// 
			this.lbCurLotYield.BackColor = System.Drawing.Color.White;
			this.lbCurLotYield.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCurLotYield.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbCurLotYield.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbCurLotYield.Location = new System.Drawing.Point(82, 311);
			this.lbCurLotYield.Name = "lbCurLotYield";
			this.lbCurLotYield.Size = new System.Drawing.Size(92, 39);
			this.lbCurLotYield.TabIndex = 31;
			this.lbCurLotYield.Text = "0.00%";
			this.lbCurLotYield.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbCurLotTotal
			// 
			this.lbCurLotTotal.BackColor = System.Drawing.Color.White;
			this.lbCurLotTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCurLotTotal.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbCurLotTotal.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbCurLotTotal.Location = new System.Drawing.Point(82, 270);
			this.lbCurLotTotal.Name = "lbCurLotTotal";
			this.lbCurLotTotal.Size = new System.Drawing.Size(92, 39);
			this.lbCurLotTotal.TabIndex = 30;
			this.lbCurLotTotal.Text = "0";
			this.lbCurLotTotal.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbCurLotNG
			// 
			this.lbCurLotNG.BackColor = System.Drawing.Color.White;
			this.lbCurLotNG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCurLotNG.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbCurLotNG.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbCurLotNG.Location = new System.Drawing.Point(82, 188);
			this.lbCurLotNG.Name = "lbCurLotNG";
			this.lbCurLotNG.Size = new System.Drawing.Size(92, 39);
			this.lbCurLotNG.TabIndex = 29;
			this.lbCurLotNG.Text = "0";
			this.lbCurLotNG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbCurLotOK
			// 
			this.lbCurLotOK.BackColor = System.Drawing.Color.White;
			this.lbCurLotOK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCurLotOK.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbCurLotOK.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbCurLotOK.Location = new System.Drawing.Point(82, 147);
			this.lbCurLotOK.Name = "lbCurLotOK";
			this.lbCurLotOK.Size = new System.Drawing.Size(92, 39);
			this.lbCurLotOK.TabIndex = 28;
			this.lbCurLotOK.Text = "0";
			this.lbCurLotOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbUserOKCount
			// 
			this.lbUserOKCount.BackColor = System.Drawing.Color.White;
			this.lbUserOKCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbUserOKCount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbUserOKCount.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbUserOKCount.Location = new System.Drawing.Point(183, 229);
			this.lbUserOKCount.Name = "lbUserOKCount";
			this.lbUserOKCount.Size = new System.Drawing.Size(98, 39);
			this.lbUserOKCount.TabIndex = 27;
			this.lbUserOKCount.Text = "0";
			this.lbUserOKCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbUserOKReset
			// 
			this.lbUserOKReset.BackColor = System.Drawing.Color.Lime;
			this.lbUserOKReset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbUserOKReset.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.lbUserOKReset.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbUserOKReset.Location = new System.Drawing.Point(5, 229);
			this.lbUserOKReset.Name = "lbUserOKReset";
			this.lbUserOKReset.Size = new System.Drawing.Size(69, 39);
			this.lbUserOKReset.TabIndex = 26;
			this.lbUserOKReset.Text = "USER OK";
			this.lbUserOKReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnYieldClear
			// 
			this.btnYieldClear.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnYieldClear.Location = new System.Drawing.Point(183, 352);
			this.btnYieldClear.Name = "btnYieldClear";
			this.btnYieldClear.Size = new System.Drawing.Size(98, 39);
			this.btnYieldClear.TabIndex = 25;
			this.btnYieldClear.Text = "CLEAR";
			this.btnYieldClear.UseVisualStyleBackColor = true;
			this.btnYieldClear.Click += new System.EventHandler(this.btnYieldClear_Click);
			// 
			// lbYeildCount
			// 
			this.lbYeildCount.BackColor = System.Drawing.Color.White;
			this.lbYeildCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbYeildCount.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbYeildCount.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbYeildCount.Location = new System.Drawing.Point(183, 311);
			this.lbYeildCount.Name = "lbYeildCount";
			this.lbYeildCount.Size = new System.Drawing.Size(98, 39);
			this.lbYeildCount.TabIndex = 14;
			this.lbYeildCount.Text = "0.00%";
			this.lbYeildCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbTotalCount
			// 
			this.lbTotalCount.BackColor = System.Drawing.Color.White;
			this.lbTotalCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbTotalCount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbTotalCount.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbTotalCount.Location = new System.Drawing.Point(183, 270);
			this.lbTotalCount.Name = "lbTotalCount";
			this.lbTotalCount.Size = new System.Drawing.Size(98, 39);
			this.lbTotalCount.TabIndex = 13;
			this.lbTotalCount.Text = "0";
			this.lbTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbNgCount
			// 
			this.lbNgCount.BackColor = System.Drawing.Color.White;
			this.lbNgCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbNgCount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbNgCount.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbNgCount.Location = new System.Drawing.Point(183, 188);
			this.lbNgCount.Name = "lbNgCount";
			this.lbNgCount.Size = new System.Drawing.Size(98, 39);
			this.lbNgCount.TabIndex = 12;
			this.lbNgCount.Text = "0";
			this.lbNgCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbOkCount
			// 
			this.lbOkCount.BackColor = System.Drawing.Color.White;
			this.lbOkCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbOkCount.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbOkCount.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbOkCount.Location = new System.Drawing.Point(183, 147);
			this.lbOkCount.Name = "lbOkCount";
			this.lbOkCount.Size = new System.Drawing.Size(98, 39);
			this.lbOkCount.TabIndex = 11;
			this.lbOkCount.Text = "0";
			this.lbOkCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmSubItems
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.DimGray;
			this.ClientSize = new System.Drawing.Size(546, 881);
			this.Controls.Add(this.panelEx1);
			this.Controls.Add(this.richMainMonitor);
			this.Controls.Add(this.MachineCtrlButtonPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmSubItems";
			this.Text = "frmSubItems";
			this.MachineCtrlButtonPanel.ResumeLayout(false);
			this.panelEx1.ResumeLayout(false);
			this.gbConnectionState.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.RichTextBox richMainMonitor;
        private System.Windows.Forms.Timer tmrSubItems;
        private System.Windows.Forms.Label lbMcState;
        private System.Windows.Forms.Label lbOkReset;
        private System.Windows.Forms.Label lbYeild;
        private System.Windows.Forms.Label lbNgReset;
        private System.Windows.Forms.Label lbTotal;
        private System.Windows.Forms.Panel MachineCtrlButtonPanel;
        private System.Windows.Forms.Panel panelEx1;
        private System.Windows.Forms.Label lbYeildCount;
        private System.Windows.Forms.Label lbTotalCount;
        private System.Windows.Forms.Label lbNgCount;
        private System.Windows.Forms.Label lbOkCount;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnAllHome;
		private System.Windows.Forms.Button btnBuzzer;
		private System.Windows.Forms.Button btnFlowMapInfo;
		private System.Windows.Forms.Button btnLotInfo;
		private System.Windows.Forms.Button btnYieldClear;
		private System.Windows.Forms.Button btnTact;
		private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lbUserOKCount;
        private System.Windows.Forms.Label lbUserOKReset;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbCurLotUserOK;
        private System.Windows.Forms.Label lbCurLotYield;
        private System.Windows.Forms.Label lbCurLotTotal;
        private System.Windows.Forms.Label lbCurLotNG;
        private System.Windows.Forms.Label lbCurLotOK;
        private System.Windows.Forms.Label lbCurLotQty;
		private System.Windows.Forms.GroupBox gbConnectionState;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label lbMESState;
		private System.Windows.Forms.Label lbRevLed;
		private System.Windows.Forms.Label lbBtmState;
		private System.Windows.Forms.Label lbCtrlLed;
		private System.Windows.Forms.Label lbTopState;
		private System.Windows.Forms.Label lbControlState;
		private System.Windows.Forms.Label lbTopLed;
		private System.Windows.Forms.Label lbRevState;
		private System.Windows.Forms.Label lbGEMLed;
		private System.Windows.Forms.Label lbBtmLed;
		private ucProgressBar progPause;
		private ucProgressBar progRun;
		private System.Windows.Forms.Label lbTrayBCRLed;
		private System.Windows.Forms.Label lbSmartVision1Led;
		private System.Windows.Forms.Label lbSmartVision1State;
		private System.Windows.Forms.Label lbTrayBCRState;
		private System.Windows.Forms.Label lbSmartVision2State;
		private System.Windows.Forms.Label lbSmartVision2Led;
		private System.Windows.Forms.Button btnSimLotInsert;
	}
}