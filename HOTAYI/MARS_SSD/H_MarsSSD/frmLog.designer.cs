namespace H_MarsSSD
{
    partial class frmLog
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tmrLogging = new System.Windows.Forms.Timer(this.components);
			this.btnClear = new System.Windows.Forms.Button();
			this.tbLoading = new System.Windows.Forms.TextBox();
			this.panel = new System.Windows.Forms.Panel();
			this.rbtnTime = new System.Windows.Forms.RadioButton();
			this.rbtnErr = new System.Windows.Forms.RadioButton();
			this.rbtnSeq = new System.Windows.Forms.RadioButton();
			this.rbtnMMI = new System.Windows.Forms.RadioButton();
			this.dgLogView = new System.Windows.Forms.DataGridView();
			this.LogDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LogLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LogMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.MenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.mitemSys = new System.Windows.Forms.ToolStripMenuItem();
			this.mitemPw = new System.Windows.Forms.ToolStripMenuItem();
			this.mitemErr = new System.Windows.Forms.ToolStripMenuItem();
			this.mitemEx = new System.Windows.Forms.ToolStripMenuItem();
			this.mitemTk = new System.Windows.Forms.ToolStripMenuItem();
			this.mitemData = new System.Windows.Forms.ToolStripMenuItem();
			this.mitemMt = new System.Windows.Forms.ToolStripMenuItem();
			this.mitemWarn = new System.Windows.Forms.ToolStripMenuItem();
			this.mitemHis = new System.Windows.Forms.ToolStripMenuItem();
			this.mitemAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.LogLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LogLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ErrCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ErrorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ErrCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ErrTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnUpdate = new System.Windows.Forms.Button();
			this.gbDataRange = new System.Windows.Forms.GroupBox();
			this.dtpTo = new System.Windows.Forms.DateTimePicker();
			this.label_To = new System.Windows.Forms.Label();
			this.dtpFrom = new System.Windows.Forms.DateTimePicker();
			this.label_From = new System.Windows.Forms.Label();
			this.btnSearch = new System.Windows.Forms.Button();
			this.txtSearchValue = new System.Windows.Forms.TextBox();
			this.tbTimeAnalysis = new System.Windows.Forms.TextBox();
			this.btnFindNext = new System.Windows.Forms.Button();
			this.btnPreFind = new System.Windows.Forms.Button();
			this.rbtnAllError = new System.Windows.Forms.RadioButton();
			this.btnFileOpen = new System.Windows.Forms.Button();
			this.fontDialog1 = new System.Windows.Forms.FontDialog();
			this.rbtnAllYield = new System.Windows.Forms.RadioButton();
			this.rbtnAllHistory = new System.Windows.Forms.RadioButton();
			this.btnRefresh = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgLogView)).BeginInit();
			this.MenuStrip.SuspendLayout();
			this.gbDataRange.SuspendLayout();
			this.SuspendLayout();
			// 
			// tmrLogging
			// 
			this.tmrLogging.Interval = 300;
			// 
			// btnClear
			// 
			this.btnClear.BackColor = System.Drawing.Color.White;
			this.btnClear.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnClear.Location = new System.Drawing.Point(1063, 21);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(113, 70);
			this.btnClear.TabIndex = 22;
			this.btnClear.Text = "CLEAR";
			this.btnClear.UseVisualStyleBackColor = false;
			this.btnClear.Visible = false;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// tbLoading
			// 
			this.tbLoading.BackColor = System.Drawing.SystemColors.ControlLight;
			this.tbLoading.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbLoading.Location = new System.Drawing.Point(607, 423);
			this.tbLoading.Multiline = true;
			this.tbLoading.Name = "tbLoading";
			this.tbLoading.ReadOnly = true;
			this.tbLoading.Size = new System.Drawing.Size(220, 50);
			this.tbLoading.TabIndex = 21;
			this.tbLoading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tbLoading.Visible = false;
			// 
			// panel
			// 
			this.panel.BackColor = System.Drawing.Color.Black;
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel.Location = new System.Drawing.Point(466, 22);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(3, 75);
			this.panel.TabIndex = 20;
			// 
			// rbtnTime
			// 
			this.rbtnTime.Appearance = System.Windows.Forms.Appearance.Button;
			this.rbtnTime.BackColor = System.Drawing.Color.White;
			this.rbtnTime.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbtnTime.Location = new System.Drawing.Point(1063, 22);
			this.rbtnTime.Name = "rbtnTime";
			this.rbtnTime.Size = new System.Drawing.Size(113, 70);
			this.rbtnTime.TabIndex = 16;
			this.rbtnTime.TabStop = true;
			this.rbtnTime.Text = "TIME\nANALYSIS";
			this.rbtnTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rbtnTime.UseVisualStyleBackColor = false;
			this.rbtnTime.Visible = false;
			this.rbtnTime.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
			// 
			// rbtnErr
			// 
			this.rbtnErr.Appearance = System.Windows.Forms.Appearance.Button;
			this.rbtnErr.BackColor = System.Drawing.Color.White;
			this.rbtnErr.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbtnErr.Location = new System.Drawing.Point(1063, 21);
			this.rbtnErr.Name = "rbtnErr";
			this.rbtnErr.Size = new System.Drawing.Size(113, 70);
			this.rbtnErr.TabIndex = 15;
			this.rbtnErr.TabStop = true;
			this.rbtnErr.Text = "ERROR\nSUMMARY";
			this.rbtnErr.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rbtnErr.UseVisualStyleBackColor = false;
			this.rbtnErr.Visible = false;
			this.rbtnErr.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
			// 
			// rbtnSeq
			// 
			this.rbtnSeq.Appearance = System.Windows.Forms.Appearance.Button;
			this.rbtnSeq.BackColor = System.Drawing.Color.White;
			this.rbtnSeq.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbtnSeq.Location = new System.Drawing.Point(335, 23);
			this.rbtnSeq.Name = "rbtnSeq";
			this.rbtnSeq.Size = new System.Drawing.Size(123, 70);
			this.rbtnSeq.TabIndex = 14;
			this.rbtnSeq.TabStop = true;
			this.rbtnSeq.Text = "SEQ LOG";
			this.rbtnSeq.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rbtnSeq.UseVisualStyleBackColor = false;
			this.rbtnSeq.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
			// 
			// rbtnMMI
			// 
			this.rbtnMMI.Appearance = System.Windows.Forms.Appearance.Button;
			this.rbtnMMI.BackColor = System.Drawing.Color.White;
			this.rbtnMMI.Checked = true;
			this.rbtnMMI.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbtnMMI.Location = new System.Drawing.Point(211, 23);
			this.rbtnMMI.Name = "rbtnMMI";
			this.rbtnMMI.Size = new System.Drawing.Size(123, 70);
			this.rbtnMMI.TabIndex = 13;
			this.rbtnMMI.TabStop = true;
			this.rbtnMMI.Text = "MMI LOG";
			this.rbtnMMI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rbtnMMI.UseVisualStyleBackColor = false;
			this.rbtnMMI.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
			// 
			// dgLogView
			// 
			this.dgLogView.AllowUserToAddRows = false;
			this.dgLogView.AllowUserToDeleteRows = false;
			this.dgLogView.AllowUserToResizeColumns = false;
			this.dgLogView.AllowUserToResizeRows = false;
			this.dgLogView.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgLogView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgLogView.ColumnHeadersHeight = 40;
			this.dgLogView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgLogView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LogDate,
            this.LogLevel,
            this.LogMessage});
			this.dgLogView.ContextMenuStrip = this.MenuStrip;
			this.dgLogView.EnableHeadersVisualStyles = false;
			this.dgLogView.Location = new System.Drawing.Point(12, 99);
			this.dgLogView.MultiSelect = false;
			this.dgLogView.Name = "dgLogView";
			this.dgLogView.ReadOnly = true;
			this.dgLogView.RowHeadersVisible = false;
			this.dgLogView.RowTemplate.Height = 23;
			this.dgLogView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dgLogView.Size = new System.Drawing.Size(1439, 770);
			this.dgLogView.TabIndex = 19;
			this.dgLogView.VirtualMode = true;
			this.dgLogView.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgLogView_CellValueNeeded);
			// 
			// LogDate
			// 
			this.LogDate.HeaderText = "DATE";
			this.LogDate.Name = "LogDate";
			this.LogDate.ReadOnly = true;
			this.LogDate.Width = 170;
			// 
			// LogLevel
			// 
			this.LogLevel.HeaderText = "LEVEL";
			this.LogLevel.Name = "LogLevel";
			this.LogLevel.ReadOnly = true;
			this.LogLevel.Width = 120;
			// 
			// LogMessage
			// 
			this.LogMessage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.LogMessage.FillWeight = 200F;
			this.LogMessage.HeaderText = "MESSAGE";
			this.LogMessage.Name = "LogMessage";
			this.LogMessage.ReadOnly = true;
			this.LogMessage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// MenuStrip
			// 
			this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.mitemSys,
            this.mitemPw,
            this.mitemErr,
            this.mitemEx,
            this.mitemTk,
            this.mitemData,
            this.mitemMt,
            this.mitemWarn,
            this.mitemHis,
            this.mitemAll,
            this.toolStripSeparator1});
			this.MenuStrip.Name = "MenuStrip";
			this.MenuStrip.Size = new System.Drawing.Size(127, 236);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(123, 6);
			// 
			// mitemSys
			// 
			this.mitemSys.Name = "mitemSys";
			this.mitemSys.Size = new System.Drawing.Size(126, 22);
			this.mitemSys.Tag = "SYSTEM";
			this.mitemSys.Text = "System";
			this.mitemSys.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// mitemPw
			// 
			this.mitemPw.Name = "mitemPw";
			this.mitemPw.Size = new System.Drawing.Size(126, 22);
			this.mitemPw.Tag = "PASSWORD";
			this.mitemPw.Text = "Password";
			this.mitemPw.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// mitemErr
			// 
			this.mitemErr.Name = "mitemErr";
			this.mitemErr.Size = new System.Drawing.Size(126, 22);
			this.mitemErr.Tag = "ERROR";
			this.mitemErr.Text = "Error";
			this.mitemErr.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// mitemEx
			// 
			this.mitemEx.Name = "mitemEx";
			this.mitemEx.Size = new System.Drawing.Size(126, 22);
			this.mitemEx.Tag = "EXCEPTION";
			this.mitemEx.Text = "Exception";
			this.mitemEx.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// mitemTk
			// 
			this.mitemTk.Name = "mitemTk";
			this.mitemTk.Size = new System.Drawing.Size(126, 22);
			this.mitemTk.Tag = "TENKEY";
			this.mitemTk.Text = "Tenkey";
			this.mitemTk.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// mitemData
			// 
			this.mitemData.Name = "mitemData";
			this.mitemData.Size = new System.Drawing.Size(126, 22);
			this.mitemData.Tag = "DATA";
			this.mitemData.Text = "Data";
			this.mitemData.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// mitemMt
			// 
			this.mitemMt.Name = "mitemMt";
			this.mitemMt.Size = new System.Drawing.Size(126, 22);
			this.mitemMt.Tag = "MOTOR";
			this.mitemMt.Text = "Motor";
			this.mitemMt.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// mitemWarn
			// 
			this.mitemWarn.Name = "mitemWarn";
			this.mitemWarn.Size = new System.Drawing.Size(126, 22);
			this.mitemWarn.Tag = "Warning";
			this.mitemWarn.Text = "Warning";
			this.mitemWarn.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// mitemHis
			// 
			this.mitemHis.Name = "mitemHis";
			this.mitemHis.Size = new System.Drawing.Size(126, 22);
			this.mitemHis.Tag = "HISTORY";
			this.mitemHis.Text = "History";
			this.mitemHis.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// mitemAll
			// 
			this.mitemAll.Checked = true;
			this.mitemAll.CheckState = System.Windows.Forms.CheckState.Checked;
			this.mitemAll.Name = "mitemAll";
			this.mitemAll.Size = new System.Drawing.Size(126, 22);
			this.mitemAll.Tag = "ALL";
			this.mitemAll.Text = "All";
			this.mitemAll.Click += new System.EventHandler(this.mitemSys_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(123, 6);
			// 
			// LogLocation
			// 
			this.LogLocation.HeaderText = "LOCATION";
			this.LogLocation.Name = "LogLocation";
			this.LogLocation.Width = 150;
			// 
			// LogLine
			// 
			this.LogLine.HeaderText = "LINE";
			this.LogLine.Name = "LogLine";
			// 
			// ErrCode
			// 
			this.ErrCode.HeaderText = "CODE";
			this.ErrCode.Name = "ErrCode";
			this.ErrCode.Width = 70;
			// 
			// ErrorName
			// 
			this.ErrorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ErrorName.FillWeight = 200F;
			this.ErrorName.HeaderText = "ERROR NAME";
			this.ErrorName.Name = "ErrorName";
			// 
			// ErrCount
			// 
			this.ErrCount.HeaderText = "COUNT";
			this.ErrCount.Name = "ErrCount";
			this.ErrCount.Width = 70;
			// 
			// ErrTime
			// 
			this.ErrTime.HeaderText = "TIME";
			this.ErrTime.Name = "ErrTime";
			// 
			// btnUpdate
			// 
			this.btnUpdate.BackColor = System.Drawing.Color.White;
			this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnUpdate.Location = new System.Drawing.Point(1063, 21);
			this.btnUpdate.Name = "btnUpdate";
			this.btnUpdate.Size = new System.Drawing.Size(113, 70);
			this.btnUpdate.TabIndex = 17;
			this.btnUpdate.Text = "UPDATE";
			this.btnUpdate.UseVisualStyleBackColor = false;
			this.btnUpdate.Visible = false;
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			// 
			// gbDataRange
			// 
			this.gbDataRange.Controls.Add(this.dtpTo);
			this.gbDataRange.Controls.Add(this.label_To);
			this.gbDataRange.Controls.Add(this.dtpFrom);
			this.gbDataRange.Controls.Add(this.label_From);
			this.gbDataRange.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.gbDataRange.Location = new System.Drawing.Point(12, 13);
			this.gbDataRange.Name = "gbDataRange";
			this.gbDataRange.Size = new System.Drawing.Size(187, 81);
			this.gbDataRange.TabIndex = 12;
			this.gbDataRange.TabStop = false;
			this.gbDataRange.Text = "DATE RANGE";
			// 
			// dtpTo
			// 
			this.dtpTo.CalendarMonthBackground = System.Drawing.Color.Gray;
			this.dtpTo.CalendarTitleBackColor = System.Drawing.Color.Gray;
			this.dtpTo.Checked = false;
			this.dtpTo.CustomFormat = "yyyy/MM/dd";
			this.dtpTo.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
			this.dtpTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpTo.Location = new System.Drawing.Point(52, 49);
			this.dtpTo.Name = "dtpTo";
			this.dtpTo.Size = new System.Drawing.Size(119, 21);
			this.dtpTo.TabIndex = 3;
			this.dtpTo.ValueChanged += new System.EventHandler(this.dtpTo_ValueChanged);
			// 
			// label_To
			// 
			this.label_To.AutoSize = true;
			this.label_To.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.label_To.Location = new System.Drawing.Point(26, 53);
			this.label_To.Name = "label_To";
			this.label_To.Size = new System.Drawing.Size(23, 15);
			this.label_To.TabIndex = 2;
			this.label_To.Text = "TO";
			// 
			// dtpFrom
			// 
			this.dtpFrom.CalendarMonthBackground = System.Drawing.Color.Gray;
			this.dtpFrom.CalendarTitleBackColor = System.Drawing.Color.Gray;
			this.dtpFrom.Checked = false;
			this.dtpFrom.CustomFormat = "yyyy/MM/dd";
			this.dtpFrom.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
			this.dtpFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtpFrom.Location = new System.Drawing.Point(52, 22);
			this.dtpFrom.Name = "dtpFrom";
			this.dtpFrom.Size = new System.Drawing.Size(119, 21);
			this.dtpFrom.TabIndex = 1;
			this.dtpFrom.ValueChanged += new System.EventHandler(this.dtpFrom_ValueChanged);
			// 
			// label_From
			// 
			this.label_From.AutoSize = true;
			this.label_From.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.label_From.Location = new System.Drawing.Point(8, 25);
			this.label_From.Name = "label_From";
			this.label_From.Size = new System.Drawing.Size(41, 15);
			this.label_From.TabIndex = 0;
			this.label_From.Text = "FROM";
			// 
			// btnSearch
			// 
			this.btnSearch.BackColor = System.Drawing.Color.White;
			this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnSearch.Location = new System.Drawing.Point(1194, 52);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(98, 39);
			this.btnSearch.TabIndex = 23;
			this.btnSearch.Text = "SEARCH";
			this.btnSearch.UseVisualStyleBackColor = false;
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// txtSearchValue
			// 
			this.txtSearchValue.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSearchValue.Location = new System.Drawing.Point(1196, 21);
			this.txtSearchValue.Name = "txtSearchValue";
			this.txtSearchValue.Size = new System.Drawing.Size(177, 25);
			this.txtSearchValue.TabIndex = 24;
			// 
			// tbTimeAnalysis
			// 
			this.tbTimeAnalysis.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tbTimeAnalysis.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.tbTimeAnalysis.Location = new System.Drawing.Point(12, 100);
			this.tbTimeAnalysis.Multiline = true;
			this.tbTimeAnalysis.Name = "tbTimeAnalysis";
			this.tbTimeAnalysis.ReadOnly = true;
			this.tbTimeAnalysis.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.tbTimeAnalysis.Size = new System.Drawing.Size(1439, 770);
			this.tbTimeAnalysis.TabIndex = 18;
			this.tbTimeAnalysis.Visible = false;
			// 
			// btnFindNext
			// 
			this.btnFindNext.BackColor = System.Drawing.Color.White;
			this.btnFindNext.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnFindNext.Location = new System.Drawing.Point(1335, 52);
			this.btnFindNext.Name = "btnFindNext";
			this.btnFindNext.Size = new System.Drawing.Size(39, 39);
			this.btnFindNext.TabIndex = 25;
			this.btnFindNext.Text = "▶";
			this.btnFindNext.UseVisualStyleBackColor = false;
			this.btnFindNext.Click += new System.EventHandler(this.butFindNext_Click);
			// 
			// btnPreFind
			// 
			this.btnPreFind.BackColor = System.Drawing.Color.White;
			this.btnPreFind.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnPreFind.Location = new System.Drawing.Point(1294, 52);
			this.btnPreFind.Name = "btnPreFind";
			this.btnPreFind.Size = new System.Drawing.Size(39, 39);
			this.btnPreFind.TabIndex = 26;
			this.btnPreFind.Text = "◀";
			this.btnPreFind.UseVisualStyleBackColor = false;
			this.btnPreFind.Click += new System.EventHandler(this.butFindPre_Click);
			// 
			// rbtnAllError
			// 
			this.rbtnAllError.Appearance = System.Windows.Forms.Appearance.Button;
			this.rbtnAllError.BackColor = System.Drawing.Color.White;
			this.rbtnAllError.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbtnAllError.Location = new System.Drawing.Point(476, 25);
			this.rbtnAllError.Name = "rbtnAllError";
			this.rbtnAllError.Size = new System.Drawing.Size(123, 70);
			this.rbtnAllError.TabIndex = 27;
			this.rbtnAllError.TabStop = true;
			this.rbtnAllError.Text = "ERROR\nSUMMARY";
			this.rbtnAllError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rbtnAllError.UseVisualStyleBackColor = false;
			this.rbtnAllError.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
			// 
			// btnFileOpen
			// 
			this.btnFileOpen.BackColor = System.Drawing.Color.White;
			this.btnFileOpen.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnFileOpen.Location = new System.Drawing.Point(845, 25);
			this.btnFileOpen.Name = "btnFileOpen";
			this.btnFileOpen.Size = new System.Drawing.Size(123, 70);
			this.btnFileOpen.TabIndex = 28;
			this.btnFileOpen.Text = "FOLDER OPEN";
			this.btnFileOpen.UseVisualStyleBackColor = false;
			this.btnFileOpen.Click += new System.EventHandler(this.btnFolderOpen_Click);
			// 
			// rbtnAllYield
			// 
			this.rbtnAllYield.Appearance = System.Windows.Forms.Appearance.Button;
			this.rbtnAllYield.BackColor = System.Drawing.Color.White;
			this.rbtnAllYield.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbtnAllYield.Location = new System.Drawing.Point(599, 25);
			this.rbtnAllYield.Name = "rbtnAllYield";
			this.rbtnAllYield.Size = new System.Drawing.Size(123, 70);
			this.rbtnAllYield.TabIndex = 29;
			this.rbtnAllYield.TabStop = true;
			this.rbtnAllYield.Text = "YIELD SUMMARY";
			this.rbtnAllYield.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rbtnAllYield.UseVisualStyleBackColor = false;
			this.rbtnAllYield.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
			// 
			// rbtnAllHistory
			// 
			this.rbtnAllHistory.Appearance = System.Windows.Forms.Appearance.Button;
			this.rbtnAllHistory.BackColor = System.Drawing.Color.White;
			this.rbtnAllHistory.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbtnAllHistory.Location = new System.Drawing.Point(722, 25);
			this.rbtnAllHistory.Name = "rbtnAllHistory";
			this.rbtnAllHistory.Size = new System.Drawing.Size(123, 70);
			this.rbtnAllHistory.TabIndex = 30;
			this.rbtnAllHistory.TabStop = true;
			this.rbtnAllHistory.Text = "DATA HISTORY";
			this.rbtnAllHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.rbtnAllHistory.UseVisualStyleBackColor = false;
			this.rbtnAllHistory.CheckedChanged += new System.EventHandler(this.rbtn_CheckedChanged);
			// 
			// btnRefresh
			// 
			this.btnRefresh.BackColor = System.Drawing.Color.White;
			this.btnRefresh.BackgroundImage = global::H_MarsSSD.Properties.Resources.Refresh_48p;
			this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnRefresh.Location = new System.Drawing.Point(1380, 21);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(68, 70);
			this.btnRefresh.TabIndex = 31;
			this.btnRefresh.UseVisualStyleBackColor = false;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// frmLog
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1473, 881);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.btnUpdate);
			this.Controls.Add(this.rbtnAllHistory);
			this.Controls.Add(this.rbtnAllYield);
			this.Controls.Add(this.btnFileOpen);
			this.Controls.Add(this.rbtnAllError);
			this.Controls.Add(this.btnPreFind);
			this.Controls.Add(this.btnFindNext);
			this.Controls.Add(this.txtSearchValue);
			this.Controls.Add(this.btnSearch);
			this.Controls.Add(this.btnClear);
			this.Controls.Add(this.tbLoading);
			this.Controls.Add(this.tbTimeAnalysis);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.rbtnTime);
			this.Controls.Add(this.rbtnErr);
			this.Controls.Add(this.rbtnSeq);
			this.Controls.Add(this.rbtnMMI);
			this.Controls.Add(this.dgLogView);
			this.Controls.Add(this.gbDataRange);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmLog";
			this.Text = "frmLog";
			this.VisibleChanged += new System.EventHandler(this.frmLog_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.dgLogView)).EndInit();
			this.MenuStrip.ResumeLayout(false);
			this.gbDataRange.ResumeLayout(false);
			this.gbDataRange.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tmrLogging;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbLoading;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.RadioButton rbtnTime;
        private System.Windows.Forms.RadioButton rbtnErr;
        private System.Windows.Forms.RadioButton rbtnSeq;
        private System.Windows.Forms.RadioButton rbtnMMI;
        private System.Windows.Forms.DataGridView dgLogView;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn LogLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrTime;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox gbDataRange;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label label_To;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label label_From;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchValue;
		private System.Windows.Forms.TextBox tbTimeAnalysis;
		private System.Windows.Forms.DataGridViewTextBoxColumn LogDate;
		private System.Windows.Forms.DataGridViewTextBoxColumn LogLevel;
		private System.Windows.Forms.DataGridViewTextBoxColumn LogMessage;
		private System.Windows.Forms.Button btnFindNext;
		private System.Windows.Forms.Button btnPreFind;
        private System.Windows.Forms.RadioButton rbtnAllError;
        private System.Windows.Forms.Button btnFileOpen;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.RadioButton rbtnAllYield;
        private System.Windows.Forms.ContextMenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mitemSys;
        private System.Windows.Forms.ToolStripMenuItem mitemPw;
        private System.Windows.Forms.ToolStripMenuItem mitemErr;
        private System.Windows.Forms.ToolStripMenuItem mitemEx;
        private System.Windows.Forms.ToolStripMenuItem mitemTk;
        private System.Windows.Forms.ToolStripMenuItem mitemData;
        private System.Windows.Forms.ToolStripMenuItem mitemMt;
        private System.Windows.Forms.ToolStripMenuItem mitemHis;
        private System.Windows.Forms.ToolStripMenuItem mitemAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.RadioButton rbtnAllHistory;
        private System.Windows.Forms.Button btnRefresh;
		private System.Windows.Forms.ToolStripMenuItem mitemWarn;
	}
}