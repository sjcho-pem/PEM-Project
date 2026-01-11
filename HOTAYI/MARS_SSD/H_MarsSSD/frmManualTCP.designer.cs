namespace H_MarsSSD
{
    partial class frmManualTCP
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
			this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
			this.gbTCP = new System.Windows.Forms.GroupBox();
			this.btnLogClear = new System.Windows.Forms.Button();
			this.lstBoxComm = new System.Windows.Forms.ListBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.tlpReview = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tlpBtm = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.panel7 = new System.Windows.Forms.Panel();
			this.tlpTop = new System.Windows.Forms.TableLayoutPanel();
			this.label40 = new System.Windows.Forms.Label();
			this.gbUBIGEM = new System.Windows.Forms.GroupBox();
			this.btnUBIGEMLogClear = new System.Windows.Forms.Button();
			this.ltbUBIGEMLog = new System.Windows.Forms.ListBox();
			this.panel3 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.btnInspection_Result = new System.Windows.Forms.Button();
			this.btnTRAY_VALIDATION = new System.Windows.Forms.Button();
			this.btnLOT_END = new System.Windows.Forms.Button();
			this.btnControlStateChanged = new System.Windows.Forms.Button();
			this.lbCommand = new System.Windows.Forms.Label();
			this.btnAlarmSet = new System.Windows.Forms.Button();
			this.btnProcessStateChanged = new System.Windows.Forms.Button();
			this.btnAlarmClear = new System.Windows.Forms.Button();
			this.gbTCP.SuspendLayout();
			this.panel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel7.SuspendLayout();
			this.gbUBIGEM.SuspendLayout();
			this.panel3.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tmrUpdate
			// 
			this.tmrUpdate.Enabled = true;
			this.tmrUpdate.Interval = 200;
			this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
			// 
			// gbTCP
			// 
			this.gbTCP.Controls.Add(this.btnLogClear);
			this.gbTCP.Controls.Add(this.lstBoxComm);
			this.gbTCP.Controls.Add(this.panel2);
			this.gbTCP.Controls.Add(this.panel1);
			this.gbTCP.Controls.Add(this.panel7);
			this.gbTCP.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.gbTCP.Location = new System.Drawing.Point(3, 3);
			this.gbTCP.Name = "gbTCP";
			this.gbTCP.Size = new System.Drawing.Size(1459, 543);
			this.gbTCP.TabIndex = 70;
			this.gbTCP.TabStop = false;
			this.gbTCP.Text = "TCP Command And Log";
			// 
			// btnLogClear
			// 
			this.btnLogClear.BackColor = System.Drawing.Color.White;
			this.btnLogClear.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnLogClear.Location = new System.Drawing.Point(782, 462);
			this.btnLogClear.Name = "btnLogClear";
			this.btnLogClear.Size = new System.Drawing.Size(663, 50);
			this.btnLogClear.TabIndex = 74;
			this.btnLogClear.Tag = "";
			this.btnLogClear.Text = "Clear";
			this.btnLogClear.UseVisualStyleBackColor = false;
			this.btnLogClear.Click += new System.EventHandler(this.btnLogClear_Click);
			// 
			// lstBoxComm
			// 
			this.lstBoxComm.FormattingEnabled = true;
			this.lstBoxComm.HorizontalScrollbar = true;
			this.lstBoxComm.ItemHeight = 12;
			this.lstBoxComm.Location = new System.Drawing.Point(782, 20);
			this.lstBoxComm.Name = "lstBoxComm";
			this.lstBoxComm.Size = new System.Drawing.Size(663, 436);
			this.lstBoxComm.TabIndex = 73;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.tlpReview);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Location = new System.Drawing.Point(6, 352);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(752, 160);
			this.panel2.TabIndex = 72;
			// 
			// tlpReview
			// 
			this.tlpReview.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tlpReview.ColumnCount = 7;
			this.tlpReview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpReview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpReview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpReview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpReview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpReview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpReview.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpReview.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpReview.Location = new System.Drawing.Point(80, 0);
			this.tlpReview.Margin = new System.Windows.Forms.Padding(0);
			this.tlpReview.Name = "tlpReview";
			this.tlpReview.RowCount = 2;
			this.tlpReview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpReview.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpReview.Size = new System.Drawing.Size(672, 160);
			this.tlpReview.TabIndex = 52;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label2.Dock = System.Windows.Forms.DockStyle.Left;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.Black;
			this.label2.Location = new System.Drawing.Point(0, 0);
			this.label2.Margin = new System.Windows.Forms.Padding(3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 160);
			this.label2.TabIndex = 50;
			this.label2.Text = "Review";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tlpBtm);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Location = new System.Drawing.Point(6, 186);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(752, 160);
			this.panel1.TabIndex = 71;
			// 
			// tlpBtm
			// 
			this.tlpBtm.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tlpBtm.ColumnCount = 7;
			this.tlpBtm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpBtm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpBtm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpBtm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpBtm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpBtm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpBtm.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpBtm.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpBtm.Location = new System.Drawing.Point(80, 0);
			this.tlpBtm.Margin = new System.Windows.Forms.Padding(0);
			this.tlpBtm.Name = "tlpBtm";
			this.tlpBtm.RowCount = 2;
			this.tlpBtm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpBtm.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpBtm.Size = new System.Drawing.Size(672, 160);
			this.tlpBtm.TabIndex = 52;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label1.Dock = System.Windows.Forms.DockStyle.Left;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Margin = new System.Windows.Forms.Padding(3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 160);
			this.label1.TabIndex = 50;
			this.label1.Text = "Inspection #2\r\n(Bottom)";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// panel7
			// 
			this.panel7.Controls.Add(this.tlpTop);
			this.panel7.Controls.Add(this.label40);
			this.panel7.Location = new System.Drawing.Point(6, 20);
			this.panel7.Name = "panel7";
			this.panel7.Size = new System.Drawing.Size(752, 160);
			this.panel7.TabIndex = 70;
			// 
			// tlpTop
			// 
			this.tlpTop.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tlpTop.ColumnCount = 7;
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpTop.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpTop.Location = new System.Drawing.Point(80, 0);
			this.tlpTop.Margin = new System.Windows.Forms.Padding(0);
			this.tlpTop.Name = "tlpTop";
			this.tlpTop.RowCount = 2;
			this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpTop.Size = new System.Drawing.Size(672, 160);
			this.tlpTop.TabIndex = 52;
			// 
			// label40
			// 
			this.label40.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.label40.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.label40.Dock = System.Windows.Forms.DockStyle.Left;
			this.label40.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label40.ForeColor = System.Drawing.Color.Black;
			this.label40.Location = new System.Drawing.Point(0, 0);
			this.label40.Margin = new System.Windows.Forms.Padding(3);
			this.label40.Name = "label40";
			this.label40.Size = new System.Drawing.Size(80, 160);
			this.label40.TabIndex = 50;
			this.label40.Text = "Inspection #1\r\n(Top)";
			this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// gbUBIGEM
			// 
			this.gbUBIGEM.Controls.Add(this.btnUBIGEMLogClear);
			this.gbUBIGEM.Controls.Add(this.ltbUBIGEMLog);
			this.gbUBIGEM.Controls.Add(this.panel3);
			this.gbUBIGEM.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.gbUBIGEM.Location = new System.Drawing.Point(3, 564);
			this.gbUBIGEM.Name = "gbUBIGEM";
			this.gbUBIGEM.Size = new System.Drawing.Size(1459, 248);
			this.gbUBIGEM.TabIndex = 71;
			this.gbUBIGEM.TabStop = false;
			this.gbUBIGEM.Text = "UBIGEM Command And Logs";
			// 
			// btnUBIGEMLogClear
			// 
			this.btnUBIGEMLogClear.BackColor = System.Drawing.Color.White;
			this.btnUBIGEMLogClear.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUBIGEMLogClear.Location = new System.Drawing.Point(782, 186);
			this.btnUBIGEMLogClear.Name = "btnUBIGEMLogClear";
			this.btnUBIGEMLogClear.Size = new System.Drawing.Size(663, 50);
			this.btnUBIGEMLogClear.TabIndex = 75;
			this.btnUBIGEMLogClear.Tag = "";
			this.btnUBIGEMLogClear.Text = "Clear";
			this.btnUBIGEMLogClear.UseVisualStyleBackColor = false;
			this.btnUBIGEMLogClear.Click += new System.EventHandler(this.btnUBIGEMLogClear_Click);
			// 
			// ltbUBIGEMLog
			// 
			this.ltbUBIGEMLog.FormattingEnabled = true;
			this.ltbUBIGEMLog.HorizontalScrollbar = true;
			this.ltbUBIGEMLog.ItemHeight = 12;
			this.ltbUBIGEMLog.Location = new System.Drawing.Point(782, 20);
			this.ltbUBIGEMLog.Name = "ltbUBIGEMLog";
			this.ltbUBIGEMLog.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.ltbUBIGEMLog.Size = new System.Drawing.Size(663, 160);
			this.ltbUBIGEMLog.TabIndex = 74;
			this.ltbUBIGEMLog.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ltbUBIGEMLog_KeyDown);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.tableLayoutPanel3);
			this.panel3.Controls.Add(this.lbCommand);
			this.panel3.Location = new System.Drawing.Point(6, 20);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(752, 160);
			this.panel3.TabIndex = 73;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tableLayoutPanel3.ColumnCount = 5;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel3.Controls.Add(this.btnAlarmClear, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.btnProcessStateChanged, 4, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnAlarmSet, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.btnInspection_Result, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnTRAY_VALIDATION, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnLOT_END, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.btnControlStateChanged, 3, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(80, 0);
			this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(672, 160);
			this.tableLayoutPanel3.TabIndex = 52;
			// 
			// btnInspection_Result
			// 
			this.btnInspection_Result.BackColor = System.Drawing.Color.White;
			this.btnInspection_Result.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnInspection_Result.Location = new System.Drawing.Point(4, 4);
			this.btnInspection_Result.Name = "btnInspection_Result";
			this.btnInspection_Result.Size = new System.Drawing.Size(127, 72);
			this.btnInspection_Result.TabIndex = 59;
			this.btnInspection_Result.Tag = "";
			this.btnInspection_Result.Text = "Send: Inspection Result";
			this.btnInspection_Result.UseVisualStyleBackColor = false;
			this.btnInspection_Result.Click += new System.EventHandler(this.btn_UBIGEM_Click);
			// 
			// btnTRAY_VALIDATION
			// 
			this.btnTRAY_VALIDATION.BackColor = System.Drawing.Color.White;
			this.btnTRAY_VALIDATION.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnTRAY_VALIDATION.Location = new System.Drawing.Point(138, 4);
			this.btnTRAY_VALIDATION.Name = "btnTRAY_VALIDATION";
			this.btnTRAY_VALIDATION.Size = new System.Drawing.Size(127, 72);
			this.btnTRAY_VALIDATION.TabIndex = 60;
			this.btnTRAY_VALIDATION.Tag = "";
			this.btnTRAY_VALIDATION.Text = "Send: TRAY_VALIDATION";
			this.btnTRAY_VALIDATION.UseVisualStyleBackColor = false;
			this.btnTRAY_VALIDATION.Click += new System.EventHandler(this.btn_UBIGEM_Click);
			// 
			// btnLOT_END
			// 
			this.btnLOT_END.BackColor = System.Drawing.Color.White;
			this.btnLOT_END.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnLOT_END.Location = new System.Drawing.Point(272, 4);
			this.btnLOT_END.Name = "btnLOT_END";
			this.btnLOT_END.Size = new System.Drawing.Size(127, 72);
			this.btnLOT_END.TabIndex = 61;
			this.btnLOT_END.Tag = "";
			this.btnLOT_END.Text = "Send: LOT_END";
			this.btnLOT_END.UseVisualStyleBackColor = false;
			this.btnLOT_END.Click += new System.EventHandler(this.btn_UBIGEM_Click);
			// 
			// btnControlStateChanged
			// 
			this.btnControlStateChanged.BackColor = System.Drawing.Color.White;
			this.btnControlStateChanged.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnControlStateChanged.Location = new System.Drawing.Point(406, 4);
			this.btnControlStateChanged.Name = "btnControlStateChanged";
			this.btnControlStateChanged.Size = new System.Drawing.Size(127, 72);
			this.btnControlStateChanged.TabIndex = 62;
			this.btnControlStateChanged.Tag = "";
			this.btnControlStateChanged.Text = "Send: ControlStateChanged";
			this.btnControlStateChanged.UseVisualStyleBackColor = false;
			this.btnControlStateChanged.Click += new System.EventHandler(this.btn_UBIGEM_Click);
			// 
			// lbCommand
			// 
			this.lbCommand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.lbCommand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCommand.Dock = System.Windows.Forms.DockStyle.Left;
			this.lbCommand.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbCommand.ForeColor = System.Drawing.Color.Black;
			this.lbCommand.Location = new System.Drawing.Point(0, 0);
			this.lbCommand.Margin = new System.Windows.Forms.Padding(3);
			this.lbCommand.Name = "lbCommand";
			this.lbCommand.Size = new System.Drawing.Size(80, 160);
			this.lbCommand.TabIndex = 50;
			this.lbCommand.Text = "UBIGEM";
			this.lbCommand.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnAlarmSet
			// 
			this.btnAlarmSet.BackColor = System.Drawing.Color.White;
			this.btnAlarmSet.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAlarmSet.Location = new System.Drawing.Point(4, 83);
			this.btnAlarmSet.Name = "btnAlarmSet";
			this.btnAlarmSet.Size = new System.Drawing.Size(127, 72);
			this.btnAlarmSet.TabIndex = 72;
			this.btnAlarmSet.Tag = "";
			this.btnAlarmSet.Text = "Send: Alarm Set";
			this.btnAlarmSet.UseVisualStyleBackColor = false;
			this.btnAlarmSet.Click += new System.EventHandler(this.btn_UBIGEM_Click);
			// 
			// btnProcessStateChanged
			// 
			this.btnProcessStateChanged.BackColor = System.Drawing.Color.White;
			this.btnProcessStateChanged.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnProcessStateChanged.Location = new System.Drawing.Point(540, 4);
			this.btnProcessStateChanged.Name = "btnProcessStateChanged";
			this.btnProcessStateChanged.Size = new System.Drawing.Size(127, 72);
			this.btnProcessStateChanged.TabIndex = 73;
			this.btnProcessStateChanged.Tag = "";
			this.btnProcessStateChanged.Text = "Send: ProcessStateChanged";
			this.btnProcessStateChanged.UseVisualStyleBackColor = false;
			this.btnProcessStateChanged.Click += new System.EventHandler(this.btn_UBIGEM_Click);
			// 
			// btnAlarmClear
			// 
			this.btnAlarmClear.BackColor = System.Drawing.Color.White;
			this.btnAlarmClear.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAlarmClear.Location = new System.Drawing.Point(138, 83);
			this.btnAlarmClear.Name = "btnAlarmClear";
			this.btnAlarmClear.Size = new System.Drawing.Size(127, 72);
			this.btnAlarmClear.TabIndex = 74;
			this.btnAlarmClear.Tag = "";
			this.btnAlarmClear.Text = "Send: Alarm Clear";
			this.btnAlarmClear.UseVisualStyleBackColor = false;
			this.btnAlarmClear.Click += new System.EventHandler(this.btn_UBIGEM_Click);
			// 
			// frmManualTCP
			// 
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 824);
			this.Controls.Add(this.gbUBIGEM);
			this.Controls.Add(this.gbTCP);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmManualTCP";
			this.Load += new System.EventHandler(this.frmManualTCP_Load);
			this.VisibleChanged += new System.EventHandler(this.frmManualTCP_VisibleChanged);
			this.gbTCP.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel7.ResumeLayout(false);
			this.gbUBIGEM.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.ResumeLayout(false);

        }



		#endregion
		private System.Windows.Forms.Timer tmrUpdate;
		private System.Windows.Forms.GroupBox gbTCP;
		private System.Windows.Forms.Button btnLogClear;
		private System.Windows.Forms.ListBox lstBoxComm;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TableLayoutPanel tlpReview;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.TableLayoutPanel tlpBtm;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.TableLayoutPanel tlpTop;
		private System.Windows.Forms.Label label40;
		private System.Windows.Forms.GroupBox gbUBIGEM;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Button btnInspection_Result;
		private System.Windows.Forms.Label lbCommand;
		private System.Windows.Forms.Button btnTRAY_VALIDATION;
		private System.Windows.Forms.Button btnLOT_END;
		private System.Windows.Forms.Button btnControlStateChanged;
		private System.Windows.Forms.Button btnUBIGEMLogClear;
		private System.Windows.Forms.ListBox ltbUBIGEMLog;
		private System.Windows.Forms.Button btnProcessStateChanged;
		private System.Windows.Forms.Button btnAlarmSet;
		private System.Windows.Forms.Button btnAlarmClear;
	}
}