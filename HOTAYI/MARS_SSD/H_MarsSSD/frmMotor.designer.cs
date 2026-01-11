namespace H_MarsSSD
{
    partial class frmMotor
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMotor));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			this.cbSelMotor = new System.Windows.Forms.ComboBox();
			this.gpMotorSelect = new System.Windows.Forms.Panel();
			this.lbMtNo = new System.Windows.Forms.Label();
			this.lbMotorSelect = new System.Windows.Forms.Label();
			this.chkMotorModify = new System.Windows.Forms.CheckBox();
			this.ledOrg = new System.Windows.Forms.Label();
			this.ledCcw = new System.Windows.Forms.Label();
			this.ledCw = new System.Windows.Forms.Label();
			this.ledInposi = new System.Windows.Forms.Label();
			this.ledReady = new System.Windows.Forms.Label();
			this.ledServo = new System.Windows.Forms.Label();
			this.ledAlarm = new System.Windows.Forms.Label();
			this.ledHome = new System.Windows.Forms.Label();
			this.btnIndexMove = new System.Windows.Forms.Button();
			this.btnPosIndexWrite = new System.Windows.Forms.Button();
			this.label12 = new System.Windows.Forms.Label();
			this.lbIndexNo = new System.Windows.Forms.Label();
			this.tmrMotor = new System.Windows.Forms.Timer(this.components);
			this.tmrMotor_Flash = new System.Windows.Forms.Timer(this.components);
			this.dgridMotor = new System.Windows.Forms.DataGridView();
			this.cmsGridMotor = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.iNDEXWRITEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lbActPos = new System.Windows.Forms.Label();
			this.lbCurPos = new System.Windows.Forms.Label();
			this.lbDistance = new System.Windows.Forms.Label();
			this.lbJogDist = new System.Windows.Forms.Label();
			this.cbDistLevel = new System.Windows.Forms.ComboBox();
			this.lbVelocity = new System.Windows.Forms.Label();
			this.lbJogVel = new System.Windows.Forms.Label();
			this.cbVelLevel = new System.Windows.Forms.ComboBox();
			this.chkMotorJog = new System.Windows.Forms.CheckBox();
			this.gbMotorMove = new System.Windows.Forms.GroupBox();
			this.btnVelIndexWrite = new System.Windows.Forms.Button();
			this.gbStepMove = new System.Windows.Forms.GroupBox();
			this.lbStepPlus = new System.Windows.Forms.Label();
			this.lbStepMinus = new System.Windows.Forms.Label();
			this.btnStepMoveDn = new System.Windows.Forms.Button();
			this.btnStepMoveUp = new System.Windows.Forms.Button();
			this.gbJogMove = new System.Windows.Forms.GroupBox();
			this.lbJogPlus = new System.Windows.Forms.Label();
			this.lbJogMinus = new System.Windows.Forms.Label();
			this.btnJogMoveDn = new System.Windows.Forms.Button();
			this.btnJogMoveUp = new System.Windows.Forms.Button();
			this.btnVelDn = new System.Windows.Forms.Button();
			this.btnDispUp = new System.Windows.Forms.Button();
			this.btnVelUp = new System.Windows.Forms.Button();
			this.btnDispDn = new System.Windows.Forms.Button();
			this.gbMotorStatus = new System.Windows.Forms.GroupBox();
			this.tbColumnMotorNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnMotorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnMotorPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnMotorVelocity = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnRepeatMove = new System.Windows.Forms.Button();
			this.gbAllVelocity = new System.Windows.Forms.GroupBox();
			this.label3 = new System.Windows.Forms.Label();
			this.cbAllVelocity = new System.Windows.Forms.ComboBox();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnMotorSave = new System.Windows.Forms.Button();
			this.lbItkMsg = new System.Windows.Forms.Label();
			this.lbInterlockMsg = new System.Windows.Forms.Label();
			this.btnTeachingMode = new System.Windows.Forms.Button();
			this.btnManualCyl = new System.Windows.Forms.Button();
			this.gpMotorSelect.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgridMotor)).BeginInit();
			this.cmsGridMotor.SuspendLayout();
			this.gbMotorMove.SuspendLayout();
			this.gbStepMove.SuspendLayout();
			this.gbJogMove.SuspendLayout();
			this.gbMotorStatus.SuspendLayout();
			this.gbAllVelocity.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbSelMotor
			// 
			this.cbSelMotor.BackColor = System.Drawing.SystemColors.Window;
			this.cbSelMotor.DisplayMember = "Text";
			this.cbSelMotor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSelMotor.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.cbSelMotor.FormattingEnabled = true;
			this.cbSelMotor.IntegralHeight = false;
			this.cbSelMotor.ItemHeight = 37;
			this.cbSelMotor.Location = new System.Drawing.Point(81, 35);
			this.cbSelMotor.MaxDropDownItems = 10;
			this.cbSelMotor.Name = "cbSelMotor";
			this.cbSelMotor.Size = new System.Drawing.Size(425, 45);
			this.cbSelMotor.Sorted = true;
			this.cbSelMotor.TabIndex = 2;
			this.cbSelMotor.SelectedIndexChanged += new System.EventHandler(this.cbSelMotor_SelectedIndexChanged);
			// 
			// gpMotorSelect
			// 
			this.gpMotorSelect.BackColor = System.Drawing.Color.Transparent;
			this.gpMotorSelect.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gpMotorSelect.Controls.Add(this.lbMtNo);
			this.gpMotorSelect.Controls.Add(this.lbMotorSelect);
			this.gpMotorSelect.Controls.Add(this.cbSelMotor);
			this.gpMotorSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.gpMotorSelect.Location = new System.Drawing.Point(12, 12);
			this.gpMotorSelect.Name = "gpMotorSelect";
			this.gpMotorSelect.Size = new System.Drawing.Size(513, 90);
			this.gpMotorSelect.TabIndex = 3;
			this.gpMotorSelect.Text = "MOTOR SELECT";
			// 
			// lbMtNo
			// 
			this.lbMtNo.BackColor = System.Drawing.Color.Black;
			this.lbMtNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbMtNo.Font = new System.Drawing.Font("Segoe UI", 23F, System.Drawing.FontStyle.Bold);
			this.lbMtNo.ForeColor = System.Drawing.Color.PaleGreen;
			this.lbMtNo.Location = new System.Drawing.Point(9, 35);
			this.lbMtNo.Name = "lbMtNo";
			this.lbMtNo.Size = new System.Drawing.Size(66, 45);
			this.lbMtNo.TabIndex = 29;
			this.lbMtNo.Text = "1";
			this.lbMtNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbMotorSelect
			// 
			this.lbMotorSelect.AutoSize = true;
			this.lbMotorSelect.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbMotorSelect.Location = new System.Drawing.Point(4, 1);
			this.lbMotorSelect.Name = "lbMotorSelect";
			this.lbMotorSelect.Size = new System.Drawing.Size(172, 25);
			this.lbMotorSelect.TabIndex = 3;
			this.lbMotorSelect.Text = "[ MOTOR SELECT ]";
			// 
			// chkMotorModify
			// 
			this.chkMotorModify.BackColor = System.Drawing.Color.Transparent;
			this.chkMotorModify.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.chkMotorModify.ForeColor = System.Drawing.Color.Black;
			this.chkMotorModify.Location = new System.Drawing.Point(1004, 63);
			this.chkMotorModify.Name = "chkMotorModify";
			this.chkMotorModify.Size = new System.Drawing.Size(196, 49);
			this.chkMotorModify.TabIndex = 4;
			this.chkMotorModify.Text = "MODIFY MODE";
			this.chkMotorModify.UseVisualStyleBackColor = false;
			this.chkMotorModify.CheckedChanged += new System.EventHandler(this.chkMotorModify_CheckedChanged);
			// 
			// ledOrg
			// 
			this.ledOrg.BackColor = System.Drawing.Color.White;
			this.ledOrg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ledOrg.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.ledOrg.ForeColor = System.Drawing.Color.Black;
			this.ledOrg.Location = new System.Drawing.Point(344, 79);
			this.ledOrg.Margin = new System.Windows.Forms.Padding(1);
			this.ledOrg.Name = "ledOrg";
			this.ledOrg.Size = new System.Drawing.Size(107, 40);
			this.ledOrg.TabIndex = 8;
			this.ledOrg.Text = "ORG";
			this.ledOrg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ledCcw
			// 
			this.ledCcw.BackColor = System.Drawing.Color.White;
			this.ledCcw.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ledCcw.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.ledCcw.ForeColor = System.Drawing.Color.Black;
			this.ledCcw.Location = new System.Drawing.Point(235, 79);
			this.ledCcw.Margin = new System.Windows.Forms.Padding(1);
			this.ledCcw.Name = "ledCcw";
			this.ledCcw.Size = new System.Drawing.Size(107, 40);
			this.ledCcw.TabIndex = 7;
			this.ledCcw.Text = "CCW";
			this.ledCcw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ledCw
			// 
			this.ledCw.BackColor = System.Drawing.Color.White;
			this.ledCw.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ledCw.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.ledCw.ForeColor = System.Drawing.Color.Black;
			this.ledCw.Location = new System.Drawing.Point(126, 79);
			this.ledCw.Margin = new System.Windows.Forms.Padding(1);
			this.ledCw.Name = "ledCw";
			this.ledCw.Size = new System.Drawing.Size(107, 40);
			this.ledCw.TabIndex = 6;
			this.ledCw.Text = "CW";
			this.ledCw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ledInposi
			// 
			this.ledInposi.BackColor = System.Drawing.Color.White;
			this.ledInposi.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ledInposi.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.ledInposi.ForeColor = System.Drawing.Color.Black;
			this.ledInposi.Location = new System.Drawing.Point(17, 79);
			this.ledInposi.Margin = new System.Windows.Forms.Padding(1);
			this.ledInposi.Name = "ledInposi";
			this.ledInposi.Size = new System.Drawing.Size(107, 40);
			this.ledInposi.TabIndex = 5;
			this.ledInposi.Text = "INPOSI";
			this.ledInposi.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ledReady
			// 
			this.ledReady.BackColor = System.Drawing.Color.White;
			this.ledReady.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ledReady.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.ledReady.ForeColor = System.Drawing.Color.Black;
			this.ledReady.Location = new System.Drawing.Point(235, 30);
			this.ledReady.Margin = new System.Windows.Forms.Padding(1);
			this.ledReady.Name = "ledReady";
			this.ledReady.Size = new System.Drawing.Size(107, 40);
			this.ledReady.TabIndex = 4;
			this.ledReady.Text = "READY";
			this.ledReady.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ledServo
			// 
			this.ledServo.BackColor = System.Drawing.Color.White;
			this.ledServo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ledServo.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.ledServo.ForeColor = System.Drawing.Color.Black;
			this.ledServo.Location = new System.Drawing.Point(126, 30);
			this.ledServo.Margin = new System.Windows.Forms.Padding(1);
			this.ledServo.Name = "ledServo";
			this.ledServo.Size = new System.Drawing.Size(107, 40);
			this.ledServo.TabIndex = 3;
			this.ledServo.Text = "SERVO";
			this.ledServo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ledServo.Click += new System.EventHandler(this.ledServo_Click);
			// 
			// ledAlarm
			// 
			this.ledAlarm.BackColor = System.Drawing.Color.White;
			this.ledAlarm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ledAlarm.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.ledAlarm.ForeColor = System.Drawing.Color.Black;
			this.ledAlarm.Location = new System.Drawing.Point(344, 30);
			this.ledAlarm.Margin = new System.Windows.Forms.Padding(1);
			this.ledAlarm.Name = "ledAlarm";
			this.ledAlarm.Size = new System.Drawing.Size(107, 40);
			this.ledAlarm.TabIndex = 2;
			this.ledAlarm.Text = "ALARM";
			this.ledAlarm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// ledHome
			// 
			this.ledHome.BackColor = System.Drawing.Color.White;
			this.ledHome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.ledHome.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.ledHome.ForeColor = System.Drawing.Color.Black;
			this.ledHome.Location = new System.Drawing.Point(17, 30);
			this.ledHome.Margin = new System.Windows.Forms.Padding(1);
			this.ledHome.Name = "ledHome";
			this.ledHome.Size = new System.Drawing.Size(107, 40);
			this.ledHome.TabIndex = 1;
			this.ledHome.Text = "HOME";
			this.ledHome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ledHome.Click += new System.EventHandler(this.ledHome_Click);
			// 
			// btnIndexMove
			// 
			this.btnIndexMove.BackColor = System.Drawing.Color.White;
			this.btnIndexMove.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnIndexMove.ForeColor = System.Drawing.Color.Black;
			this.btnIndexMove.Location = new System.Drawing.Point(162, 751);
			this.btnIndexMove.Name = "btnIndexMove";
			this.btnIndexMove.Size = new System.Drawing.Size(100, 94);
			this.btnIndexMove.TabIndex = 24;
			this.btnIndexMove.Text = "INDEX MOVE";
			this.btnIndexMove.UseVisualStyleBackColor = false;
			this.btnIndexMove.Click += new System.EventHandler(this.btnIndexMove_Click);
			// 
			// btnPosIndexWrite
			// 
			this.btnPosIndexWrite.BackColor = System.Drawing.Color.White;
			this.btnPosIndexWrite.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnPosIndexWrite.ForeColor = System.Drawing.Color.Black;
			this.btnPosIndexWrite.Location = new System.Drawing.Point(344, 134);
			this.btnPosIndexWrite.Name = "btnPosIndexWrite";
			this.btnPosIndexWrite.Size = new System.Drawing.Size(107, 84);
			this.btnPosIndexWrite.TabIndex = 23;
			this.btnPosIndexWrite.Text = "INDEX WRITE";
			this.btnPosIndexWrite.UseVisualStyleBackColor = false;
			this.btnPosIndexWrite.Click += new System.EventHandler(this.btnPosIndexWrite_Click);
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.BackColor = System.Drawing.Color.Transparent;
			this.label12.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.label12.ForeColor = System.Drawing.Color.Black;
			this.label12.Location = new System.Drawing.Point(12, 741);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(148, 37);
			this.label12.TabIndex = 22;
			this.label12.Text = "INDEX NO";
			this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbIndexNo
			// 
			this.lbIndexNo.BackColor = System.Drawing.Color.Black;
			this.lbIndexNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbIndexNo.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold);
			this.lbIndexNo.ForeColor = System.Drawing.Color.PaleGreen;
			this.lbIndexNo.Location = new System.Drawing.Point(24, 786);
			this.lbIndexNo.Name = "lbIndexNo";
			this.lbIndexNo.Size = new System.Drawing.Size(121, 59);
			this.lbIndexNo.TabIndex = 21;
			this.lbIndexNo.Text = "1";
			this.lbIndexNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tmrMotor
			// 
			this.tmrMotor.Tick += new System.EventHandler(this.tmrMotor_Tick);
			// 
			// tmrMotor_Flash
			// 
			this.tmrMotor_Flash.Interval = 500;
			this.tmrMotor_Flash.Tick += new System.EventHandler(this.tmrMotor_Flash_Tick);
			// 
			// dgridMotor
			// 
			this.dgridMotor.AllowUserToAddRows = false;
			this.dgridMotor.AllowUserToResizeColumns = false;
			this.dgridMotor.AllowUserToResizeRows = false;
			this.dgridMotor.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridMotor.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgridMotor.ColumnHeadersHeight = 30;
			this.dgridMotor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridMotor.ContextMenuStrip = this.cmsGridMotor;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Transparent;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridMotor.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgridMotor.EnableHeadersVisualStyles = false;
			this.dgridMotor.Location = new System.Drawing.Point(12, 118);
			this.dgridMotor.MultiSelect = false;
			this.dgridMotor.Name = "dgridMotor";
			this.dgridMotor.RowHeadersVisible = false;
			this.dgridMotor.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridMotor.RowTemplate.Height = 23;
			this.dgridMotor.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridMotor.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgridMotor.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dgridMotor.Size = new System.Drawing.Size(1430, 353);
			this.dgridMotor.TabIndex = 20;
			this.dgridMotor.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridMotor_CellMouseDown);
			this.dgridMotor.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridMotor_CellValueChanged);
			this.dgridMotor.SelectionChanged += new System.EventHandler(this.dgridMotor_SelectionChanged);
			// 
			// cmsGridMotor
			// 
			this.cmsGridMotor.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.cmsGridMotor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iNDEXWRITEToolStripMenuItem});
			this.cmsGridMotor.Name = "cmsGrid";
			this.cmsGridMotor.Size = new System.Drawing.Size(185, 30);
			// 
			// iNDEXWRITEToolStripMenuItem
			// 
			this.iNDEXWRITEToolStripMenuItem.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.iNDEXWRITEToolStripMenuItem.Name = "iNDEXWRITEToolStripMenuItem";
			this.iNDEXWRITEToolStripMenuItem.Size = new System.Drawing.Size(184, 26);
			this.iNDEXWRITEToolStripMenuItem.Text = "INDEX WRITE";
			this.iNDEXWRITEToolStripMenuItem.Click += new System.EventHandler(this.btnPosIndexWrite_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(14, 137);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(94, 28);
			this.label1.TabIndex = 5;
			this.label1.Text = "ACT POS";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.Black;
			this.label2.Location = new System.Drawing.Point(14, 186);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(95, 28);
			this.label2.TabIndex = 6;
			this.label2.Text = "CUR POS";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbActPos
			// 
			this.lbActPos.BackColor = System.Drawing.Color.Transparent;
			this.lbActPos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbActPos.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbActPos.ForeColor = System.Drawing.Color.Black;
			this.lbActPos.Location = new System.Drawing.Point(118, 134);
			this.lbActPos.Name = "lbActPos";
			this.lbActPos.Size = new System.Drawing.Size(221, 35);
			this.lbActPos.TabIndex = 7;
			this.lbActPos.Text = "0";
			this.lbActPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbCurPos
			// 
			this.lbCurPos.BackColor = System.Drawing.Color.Transparent;
			this.lbCurPos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbCurPos.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbCurPos.ForeColor = System.Drawing.Color.Black;
			this.lbCurPos.Location = new System.Drawing.Point(118, 183);
			this.lbCurPos.Name = "lbCurPos";
			this.lbCurPos.Size = new System.Drawing.Size(221, 35);
			this.lbCurPos.TabIndex = 8;
			this.lbCurPos.Text = "0";
			this.lbCurPos.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbDistance
			// 
			this.lbDistance.AutoSize = true;
			this.lbDistance.BackColor = System.Drawing.Color.Transparent;
			this.lbDistance.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.lbDistance.ForeColor = System.Drawing.Color.Black;
			this.lbDistance.Location = new System.Drawing.Point(35, 45);
			this.lbDistance.Name = "lbDistance";
			this.lbDistance.Size = new System.Drawing.Size(150, 25);
			this.lbDistance.TabIndex = 6;
			this.lbDistance.Text = "DISTANCE [mm]";
			this.lbDistance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbJogDist
			// 
			this.lbJogDist.BackColor = System.Drawing.Color.Black;
			this.lbJogDist.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbJogDist.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbJogDist.ForeColor = System.Drawing.Color.PaleGreen;
			this.lbJogDist.Location = new System.Drawing.Point(38, 78);
			this.lbJogDist.Name = "lbJogDist";
			this.lbJogDist.Size = new System.Drawing.Size(172, 35);
			this.lbJogDist.TabIndex = 8;
			this.lbJogDist.Text = "0";
			this.lbJogDist.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lbJogDist.Click += new System.EventHandler(this.lbJogDist_Click);
			// 
			// cbDistLevel
			// 
			this.cbDistLevel.DisplayMember = "Text";
			this.cbDistLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbDistLevel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.cbDistLevel.FormattingEnabled = true;
			this.cbDistLevel.ItemHeight = 25;
			this.cbDistLevel.Location = new System.Drawing.Point(38, 117);
			this.cbDistLevel.Name = "cbDistLevel";
			this.cbDistLevel.Size = new System.Drawing.Size(172, 33);
			this.cbDistLevel.TabIndex = 11;
			// 
			// lbVelocity
			// 
			this.lbVelocity.AutoSize = true;
			this.lbVelocity.BackColor = System.Drawing.Color.Transparent;
			this.lbVelocity.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.lbVelocity.ForeColor = System.Drawing.Color.Black;
			this.lbVelocity.Location = new System.Drawing.Point(339, 45);
			this.lbVelocity.Name = "lbVelocity";
			this.lbVelocity.Size = new System.Drawing.Size(182, 25);
			this.lbVelocity.TabIndex = 12;
			this.lbVelocity.Text = "VELOCITY [mm/sec]";
			this.lbVelocity.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbJogVel
			// 
			this.lbJogVel.BackColor = System.Drawing.Color.Black;
			this.lbJogVel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbJogVel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbJogVel.ForeColor = System.Drawing.Color.PaleGreen;
			this.lbJogVel.Location = new System.Drawing.Point(342, 78);
			this.lbJogVel.Name = "lbJogVel";
			this.lbJogVel.Size = new System.Drawing.Size(172, 35);
			this.lbJogVel.TabIndex = 13;
			this.lbJogVel.Text = "0";
			this.lbJogVel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.lbJogVel.Click += new System.EventHandler(this.lbJogVel_Click);
			// 
			// cbVelLevel
			// 
			this.cbVelLevel.DisplayMember = "Text";
			this.cbVelLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbVelLevel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.cbVelLevel.FormattingEnabled = true;
			this.cbVelLevel.ItemHeight = 25;
			this.cbVelLevel.Location = new System.Drawing.Point(342, 117);
			this.cbVelLevel.Name = "cbVelLevel";
			this.cbVelLevel.Size = new System.Drawing.Size(172, 33);
			this.cbVelLevel.TabIndex = 16;
			// 
			// chkMotorJog
			// 
			this.chkMotorJog.BackColor = System.Drawing.Color.Transparent;
			this.chkMotorJog.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.chkMotorJog.ForeColor = System.Drawing.Color.Black;
			this.chkMotorJog.Location = new System.Drawing.Point(1004, 12);
			this.chkMotorJog.Name = "chkMotorJog";
			this.chkMotorJog.Size = new System.Drawing.Size(196, 49);
			this.chkMotorJog.TabIndex = 7;
			this.chkMotorJog.Text = "JOG MODE";
			this.chkMotorJog.UseVisualStyleBackColor = false;
			this.chkMotorJog.CheckedChanged += new System.EventHandler(this.chkMotorJog_CheckedChanged);
			// 
			// gbMotorMove
			// 
			this.gbMotorMove.Controls.Add(this.btnVelIndexWrite);
			this.gbMotorMove.Controls.Add(this.gbStepMove);
			this.gbMotorMove.Controls.Add(this.gbJogMove);
			this.gbMotorMove.Controls.Add(this.lbDistance);
			this.gbMotorMove.Controls.Add(this.cbVelLevel);
			this.gbMotorMove.Controls.Add(this.lbJogDist);
			this.gbMotorMove.Controls.Add(this.btnVelDn);
			this.gbMotorMove.Controls.Add(this.btnDispUp);
			this.gbMotorMove.Controls.Add(this.btnVelUp);
			this.gbMotorMove.Controls.Add(this.btnDispDn);
			this.gbMotorMove.Controls.Add(this.cbDistLevel);
			this.gbMotorMove.Controls.Add(this.lbVelocity);
			this.gbMotorMove.Controls.Add(this.lbJogVel);
			this.gbMotorMove.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.gbMotorMove.Location = new System.Drawing.Point(480, 485);
			this.gbMotorMove.Name = "gbMotorMove";
			this.gbMotorMove.Size = new System.Drawing.Size(636, 381);
			this.gbMotorMove.TabIndex = 25;
			this.gbMotorMove.TabStop = false;
			this.gbMotorMove.Text = "MOTOR MOVE";
			// 
			// btnVelIndexWrite
			// 
			this.btnVelIndexWrite.BackColor = System.Drawing.Color.White;
			this.btnVelIndexWrite.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.btnVelIndexWrite.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnVelIndexWrite.ForeColor = System.Drawing.Color.Black;
			this.btnVelIndexWrite.Location = new System.Drawing.Point(342, 156);
			this.btnVelIndexWrite.Name = "btnVelIndexWrite";
			this.btnVelIndexWrite.Size = new System.Drawing.Size(248, 34);
			this.btnVelIndexWrite.TabIndex = 24;
			this.btnVelIndexWrite.Text = "VELOCITY INDEX WRITE";
			this.btnVelIndexWrite.UseVisualStyleBackColor = false;
			this.btnVelIndexWrite.Click += new System.EventHandler(this.btnVelIndexWrite_Click);
			// 
			// gbStepMove
			// 
			this.gbStepMove.Controls.Add(this.lbStepPlus);
			this.gbStepMove.Controls.Add(this.lbStepMinus);
			this.gbStepMove.Controls.Add(this.btnStepMoveDn);
			this.gbStepMove.Controls.Add(this.btnStepMoveUp);
			this.gbStepMove.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.gbStepMove.Location = new System.Drawing.Point(28, 205);
			this.gbStepMove.Name = "gbStepMove";
			this.gbStepMove.Size = new System.Drawing.Size(280, 155);
			this.gbStepMove.TabIndex = 22;
			this.gbStepMove.TabStop = false;
			this.gbStepMove.Text = "STEP MOVE";
			// 
			// lbStepPlus
			// 
			this.lbStepPlus.Location = new System.Drawing.Point(150, 125);
			this.lbStepPlus.Name = "lbStepPlus";
			this.lbStepPlus.Size = new System.Drawing.Size(120, 20);
			this.lbStepPlus.TabIndex = 24;
			this.lbStepPlus.Text = "PLUS STEP";
			this.lbStepPlus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbStepMinus
			// 
			this.lbStepMinus.Location = new System.Drawing.Point(10, 125);
			this.lbStepMinus.Name = "lbStepMinus";
			this.lbStepMinus.Size = new System.Drawing.Size(120, 20);
			this.lbStepMinus.TabIndex = 23;
			this.lbStepMinus.Text = "MINUS STEP";
			this.lbStepMinus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnStepMoveDn
			// 
			this.btnStepMoveDn.BackColor = System.Drawing.Color.White;
			this.btnStepMoveDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnStepMoveDn.ForeColor = System.Drawing.Color.Black;
			this.btnStepMoveDn.Image = global::H_MarsSSD.Properties.Resources.StepMinusOff_64p;
			this.btnStepMoveDn.Location = new System.Drawing.Point(10, 32);
			this.btnStepMoveDn.Name = "btnStepMoveDn";
			this.btnStepMoveDn.Size = new System.Drawing.Size(120, 90);
			this.btnStepMoveDn.TabIndex = 18;
			this.btnStepMoveDn.UseVisualStyleBackColor = false;
			this.btnStepMoveDn.Click += new System.EventHandler(this.btnStepMoveDn_Click);
			// 
			// btnStepMoveUp
			// 
			this.btnStepMoveUp.BackColor = System.Drawing.Color.White;
			this.btnStepMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnStepMoveUp.ForeColor = System.Drawing.Color.Black;
			this.btnStepMoveUp.Image = global::H_MarsSSD.Properties.Resources.StepPlusOff_64p;
			this.btnStepMoveUp.Location = new System.Drawing.Point(150, 32);
			this.btnStepMoveUp.Name = "btnStepMoveUp";
			this.btnStepMoveUp.Size = new System.Drawing.Size(120, 90);
			this.btnStepMoveUp.TabIndex = 17;
			this.btnStepMoveUp.UseVisualStyleBackColor = false;
			this.btnStepMoveUp.Click += new System.EventHandler(this.btnStepMoveUp_Click);
			// 
			// gbJogMove
			// 
			this.gbJogMove.Controls.Add(this.lbJogPlus);
			this.gbJogMove.Controls.Add(this.lbJogMinus);
			this.gbJogMove.Controls.Add(this.btnJogMoveDn);
			this.gbJogMove.Controls.Add(this.btnJogMoveUp);
			this.gbJogMove.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.gbJogMove.Location = new System.Drawing.Point(330, 205);
			this.gbJogMove.Name = "gbJogMove";
			this.gbJogMove.Size = new System.Drawing.Size(280, 155);
			this.gbJogMove.TabIndex = 21;
			this.gbJogMove.TabStop = false;
			this.gbJogMove.Text = "JOG MOVE";
			// 
			// lbJogPlus
			// 
			this.lbJogPlus.Location = new System.Drawing.Point(150, 125);
			this.lbJogPlus.Name = "lbJogPlus";
			this.lbJogPlus.Size = new System.Drawing.Size(120, 20);
			this.lbJogPlus.TabIndex = 22;
			this.lbJogPlus.Text = "PLUS JOG";
			this.lbJogPlus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbJogMinus
			// 
			this.lbJogMinus.Location = new System.Drawing.Point(10, 125);
			this.lbJogMinus.Name = "lbJogMinus";
			this.lbJogMinus.Size = new System.Drawing.Size(120, 20);
			this.lbJogMinus.TabIndex = 21;
			this.lbJogMinus.Text = "MINUS JOG";
			this.lbJogMinus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnJogMoveDn
			// 
			this.btnJogMoveDn.BackColor = System.Drawing.Color.White;
			this.btnJogMoveDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnJogMoveDn.ForeColor = System.Drawing.Color.Black;
			this.btnJogMoveDn.Image = global::H_MarsSSD.Properties.Resources.JogMinusOff_64;
			this.btnJogMoveDn.Location = new System.Drawing.Point(10, 32);
			this.btnJogMoveDn.Name = "btnJogMoveDn";
			this.btnJogMoveDn.Size = new System.Drawing.Size(120, 90);
			this.btnJogMoveDn.TabIndex = 20;
			this.btnJogMoveDn.UseVisualStyleBackColor = false;
			this.btnJogMoveDn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogMoveDn_MouseDown);
			this.btnJogMoveDn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogMoveDn_MouseUp);
			// 
			// btnJogMoveUp
			// 
			this.btnJogMoveUp.BackColor = System.Drawing.Color.White;
			this.btnJogMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnJogMoveUp.ForeColor = System.Drawing.Color.Black;
			this.btnJogMoveUp.Image = global::H_MarsSSD.Properties.Resources.JogPlusOff_64p;
			this.btnJogMoveUp.Location = new System.Drawing.Point(150, 32);
			this.btnJogMoveUp.Name = "btnJogMoveUp";
			this.btnJogMoveUp.Size = new System.Drawing.Size(120, 90);
			this.btnJogMoveUp.TabIndex = 19;
			this.btnJogMoveUp.UseVisualStyleBackColor = false;
			this.btnJogMoveUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnJogMoveUp_MouseDown);
			this.btnJogMoveUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnJogMoveUp_MouseUp);
			// 
			// btnVelDn
			// 
			this.btnVelDn.BackColor = System.Drawing.Color.White;
			this.btnVelDn.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.btnVelDn.ForeColor = System.Drawing.Color.Black;
			this.btnVelDn.Image = ((System.Drawing.Image)(resources.GetObject("btnVelDn.Image")));
			this.btnVelDn.Location = new System.Drawing.Point(520, 101);
			this.btnVelDn.Name = "btnVelDn";
			this.btnVelDn.Size = new System.Drawing.Size(70, 50);
			this.btnVelDn.TabIndex = 15;
			this.btnVelDn.UseVisualStyleBackColor = false;
			this.btnVelDn.Click += new System.EventHandler(this.btnVelDn_Click);
			// 
			// btnDispUp
			// 
			this.btnDispUp.BackColor = System.Drawing.Color.White;
			this.btnDispUp.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.btnDispUp.ForeColor = System.Drawing.Color.Black;
			this.btnDispUp.Image = ((System.Drawing.Image)(resources.GetObject("btnDispUp.Image")));
			this.btnDispUp.Location = new System.Drawing.Point(216, 45);
			this.btnDispUp.Name = "btnDispUp";
			this.btnDispUp.Size = new System.Drawing.Size(70, 50);
			this.btnDispUp.TabIndex = 9;
			this.btnDispUp.UseVisualStyleBackColor = false;
			this.btnDispUp.Click += new System.EventHandler(this.btnDispUp_Click);
			// 
			// btnVelUp
			// 
			this.btnVelUp.BackColor = System.Drawing.Color.White;
			this.btnVelUp.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.btnVelUp.ForeColor = System.Drawing.Color.Black;
			this.btnVelUp.Image = ((System.Drawing.Image)(resources.GetObject("btnVelUp.Image")));
			this.btnVelUp.Location = new System.Drawing.Point(520, 45);
			this.btnVelUp.Name = "btnVelUp";
			this.btnVelUp.Size = new System.Drawing.Size(70, 50);
			this.btnVelUp.TabIndex = 14;
			this.btnVelUp.UseVisualStyleBackColor = false;
			this.btnVelUp.Click += new System.EventHandler(this.btnVelUp_Click);
			// 
			// btnDispDn
			// 
			this.btnDispDn.BackColor = System.Drawing.Color.White;
			this.btnDispDn.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.btnDispDn.ForeColor = System.Drawing.Color.Black;
			this.btnDispDn.Image = ((System.Drawing.Image)(resources.GetObject("btnDispDn.Image")));
			this.btnDispDn.Location = new System.Drawing.Point(216, 101);
			this.btnDispDn.Name = "btnDispDn";
			this.btnDispDn.Size = new System.Drawing.Size(70, 50);
			this.btnDispDn.TabIndex = 10;
			this.btnDispDn.UseVisualStyleBackColor = false;
			this.btnDispDn.Click += new System.EventHandler(this.btnDispDn_Click);
			// 
			// gbMotorStatus
			// 
			this.gbMotorStatus.Controls.Add(this.lbCurPos);
			this.gbMotorStatus.Controls.Add(this.ledOrg);
			this.gbMotorStatus.Controls.Add(this.lbActPos);
			this.gbMotorStatus.Controls.Add(this.ledHome);
			this.gbMotorStatus.Controls.Add(this.label2);
			this.gbMotorStatus.Controls.Add(this.ledCcw);
			this.gbMotorStatus.Controls.Add(this.label1);
			this.gbMotorStatus.Controls.Add(this.ledAlarm);
			this.gbMotorStatus.Controls.Add(this.ledCw);
			this.gbMotorStatus.Controls.Add(this.ledServo);
			this.gbMotorStatus.Controls.Add(this.ledInposi);
			this.gbMotorStatus.Controls.Add(this.ledReady);
			this.gbMotorStatus.Controls.Add(this.btnPosIndexWrite);
			this.gbMotorStatus.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.gbMotorStatus.Location = new System.Drawing.Point(11, 485);
			this.gbMotorStatus.Name = "gbMotorStatus";
			this.gbMotorStatus.Size = new System.Drawing.Size(462, 230);
			this.gbMotorStatus.TabIndex = 26;
			this.gbMotorStatus.TabStop = false;
			this.gbMotorStatus.Text = "MOTOR STATUS";
			// 
			// tbColumnMotorNum
			// 
			this.tbColumnMotorNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnMotorNum.DefaultCellStyle = dataGridViewCellStyle3;
			this.tbColumnMotorNum.HeaderText = "NO";
			this.tbColumnMotorNum.Name = "tbColumnMotorNum";
			this.tbColumnMotorNum.ReadOnly = true;
			this.tbColumnMotorNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnMotorNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnMotorNum.Width = 73;
			// 
			// tbColumnMotorName
			// 
			this.tbColumnMotorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.tbColumnMotorName.DefaultCellStyle = dataGridViewCellStyle4;
			this.tbColumnMotorName.HeaderText = "INDEX NAME";
			this.tbColumnMotorName.Name = "tbColumnMotorName";
			this.tbColumnMotorName.ReadOnly = true;
			this.tbColumnMotorName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnMotorName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnMotorName.Width = 510;
			// 
			// tbColumnMotorPosition
			// 
			this.tbColumnMotorPosition.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnMotorPosition.DefaultCellStyle = dataGridViewCellStyle5;
			this.tbColumnMotorPosition.HeaderText = "POSITION";
			this.tbColumnMotorPosition.Name = "tbColumnMotorPosition";
			this.tbColumnMotorPosition.ReadOnly = true;
			this.tbColumnMotorPosition.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnMotorPosition.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnMotorPosition.Width = 250;
			// 
			// tbColumnMotorVelocity
			// 
			this.tbColumnMotorVelocity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnMotorVelocity.DefaultCellStyle = dataGridViewCellStyle6;
			this.tbColumnMotorVelocity.HeaderText = "VELOCITY";
			this.tbColumnMotorVelocity.Name = "tbColumnMotorVelocity";
			this.tbColumnMotorVelocity.ReadOnly = true;
			this.tbColumnMotorVelocity.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnMotorVelocity.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnMotorVelocity.Width = 256;
			// 
			// btnRepeatMove
			// 
			this.btnRepeatMove.BackColor = System.Drawing.Color.White;
			this.btnRepeatMove.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnRepeatMove.ForeColor = System.Drawing.Color.Black;
			this.btnRepeatMove.Location = new System.Drawing.Point(267, 751);
			this.btnRepeatMove.Name = "btnRepeatMove";
			this.btnRepeatMove.Size = new System.Drawing.Size(100, 94);
			this.btnRepeatMove.TabIndex = 29;
			this.btnRepeatMove.Text = "REPEAT MOVE";
			this.btnRepeatMove.UseVisualStyleBackColor = false;
			this.btnRepeatMove.Click += new System.EventHandler(this.btnRepeatMove_Click);
			// 
			// gbAllVelocity
			// 
			this.gbAllVelocity.Controls.Add(this.label3);
			this.gbAllVelocity.Controls.Add(this.cbAllVelocity);
			this.gbAllVelocity.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbAllVelocity.Location = new System.Drawing.Point(532, 12);
			this.gbAllVelocity.Name = "gbAllVelocity";
			this.gbAllVelocity.Size = new System.Drawing.Size(132, 90);
			this.gbAllVelocity.TabIndex = 30;
			this.gbAllVelocity.TabStop = false;
			this.gbAllVelocity.Text = "VELOCITY RATE";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.label3.ForeColor = System.Drawing.Color.Black;
			this.label3.Location = new System.Drawing.Point(98, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(28, 25);
			this.label3.TabIndex = 7;
			this.label3.Text = "%";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cbAllVelocity
			// 
			this.cbAllVelocity.BackColor = System.Drawing.SystemColors.Window;
			this.cbAllVelocity.DisplayMember = "Text";
			this.cbAllVelocity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbAllVelocity.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.cbAllVelocity.FormattingEnabled = true;
			this.cbAllVelocity.ItemHeight = 37;
			this.cbAllVelocity.Location = new System.Drawing.Point(6, 35);
			this.cbAllVelocity.MaxDropDownItems = 10;
			this.cbAllVelocity.Name = "cbAllVelocity";
			this.cbAllVelocity.Size = new System.Drawing.Size(86, 45);
			this.cbAllVelocity.TabIndex = 3;
			this.cbAllVelocity.SelectedIndexChanged += new System.EventHandler(this.cbAllVelocity_SelectedIndexChanged);
			// 
			// btnStop
			// 
			this.btnStop.BackColor = System.Drawing.Color.White;
			this.btnStop.BackgroundImage = global::H_MarsSSD.Properties.Resources.btnStopOn;
			this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnStop.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnStop.ForeColor = System.Drawing.Color.Black;
			this.btnStop.Image = global::H_MarsSSD.Properties.Resources.btnStopOn;
			this.btnStop.Location = new System.Drawing.Point(372, 751);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(100, 94);
			this.btnStop.TabIndex = 28;
			this.btnStop.UseVisualStyleBackColor = false;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnRefresh
			// 
			this.btnRefresh.BackColor = System.Drawing.Color.White;
			this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnRefresh.Enabled = false;
			this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnRefresh.ForeColor = System.Drawing.Color.Black;
			this.btnRefresh.Image = global::H_MarsSSD.Properties.Resources.Refresh_72p;
			this.btnRefresh.Location = new System.Drawing.Point(1198, 12);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(120, 100);
			this.btnRefresh.TabIndex = 27;
			this.btnRefresh.UseVisualStyleBackColor = false;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnMotorSave
			// 
			this.btnMotorSave.BackColor = System.Drawing.Color.White;
			this.btnMotorSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnMotorSave.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnMotorSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnMotorSave.ForeColor = System.Drawing.Color.Black;
			this.btnMotorSave.Image = global::H_MarsSSD.Properties.Resources.SaveBlue_72p;
			this.btnMotorSave.Location = new System.Drawing.Point(1323, 12);
			this.btnMotorSave.Name = "btnMotorSave";
			this.btnMotorSave.Size = new System.Drawing.Size(120, 100);
			this.btnMotorSave.TabIndex = 8;
			this.btnMotorSave.UseVisualStyleBackColor = false;
			this.btnMotorSave.Click += new System.EventHandler(this.btnMotorSave_Click);
			// 
			// lbItkMsg
			// 
			this.lbItkMsg.AutoSize = true;
			this.lbItkMsg.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbItkMsg.Location = new System.Drawing.Point(1122, 500);
			this.lbItkMsg.Name = "lbItkMsg";
			this.lbItkMsg.Size = new System.Drawing.Size(173, 25);
			this.lbItkMsg.TabIndex = 31;
			this.lbItkMsg.Text = "Interlock Message";
			// 
			// lbInterlockMsg
			// 
			this.lbInterlockMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbInterlockMsg.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbInterlockMsg.ForeColor = System.Drawing.Color.Red;
			this.lbInterlockMsg.Location = new System.Drawing.Point(1127, 530);
			this.lbInterlockMsg.Name = "lbInterlockMsg";
			this.lbInterlockMsg.Size = new System.Drawing.Size(316, 299);
			this.lbInterlockMsg.TabIndex = 32;
			// 
			// btnTeachingMode
			// 
			this.btnTeachingMode.BackColor = System.Drawing.Color.White;
			this.btnTeachingMode.Enabled = false;
			this.btnTeachingMode.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.btnTeachingMode.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnTeachingMode.ForeColor = System.Drawing.Color.Black;
			this.btnTeachingMode.Location = new System.Drawing.Point(1243, 832);
			this.btnTeachingMode.Name = "btnTeachingMode";
			this.btnTeachingMode.Size = new System.Drawing.Size(200, 34);
			this.btnTeachingMode.TabIndex = 33;
			this.btnTeachingMode.Text = "TEACHING MODE";
			this.btnTeachingMode.UseVisualStyleBackColor = false;
			this.btnTeachingMode.Click += new System.EventHandler(this.btnTeachingMode_Click);
			// 
			// btnManualCyl
			// 
			this.btnManualCyl.BackColor = System.Drawing.Color.White;
			this.btnManualCyl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnManualCyl.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnManualCyl.ForeColor = System.Drawing.Color.Black;
			this.btnManualCyl.Location = new System.Drawing.Point(670, 12);
			this.btnManualCyl.Name = "btnManualCyl";
			this.btnManualCyl.Size = new System.Drawing.Size(97, 90);
			this.btnManualCyl.TabIndex = 34;
			this.btnManualCyl.Text = "Manual\r\nCylinder";
			this.btnManualCyl.UseVisualStyleBackColor = false;
			this.btnManualCyl.Visible = false;
			this.btnManualCyl.Click += new System.EventHandler(this.btnManualCyl_Click);
			// 
			// frmMotor
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 881);
			this.Controls.Add(this.btnManualCyl);
			this.Controls.Add(this.btnTeachingMode);
			this.Controls.Add(this.lbInterlockMsg);
			this.Controls.Add(this.lbItkMsg);
			this.Controls.Add(this.gbAllVelocity);
			this.Controls.Add(this.btnRepeatMove);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.gbMotorStatus);
			this.Controls.Add(this.btnIndexMove);
			this.Controls.Add(this.chkMotorJog);
			this.Controls.Add(this.gbMotorMove);
			this.Controls.Add(this.btnMotorSave);
			this.Controls.Add(this.chkMotorModify);
			this.Controls.Add(this.dgridMotor);
			this.Controls.Add(this.gpMotorSelect);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.lbIndexNo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmMotor";
			this.Text = "frmMotor";
			this.Load += new System.EventHandler(this.frmMotor_Load);
			this.VisibleChanged += new System.EventHandler(this.frmMotor_VisibleChanged);
			this.gpMotorSelect.ResumeLayout(false);
			this.gpMotorSelect.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgridMotor)).EndInit();
			this.cmsGridMotor.ResumeLayout(false);
			this.gbMotorMove.ResumeLayout(false);
			this.gbMotorMove.PerformLayout();
			this.gbStepMove.ResumeLayout(false);
			this.gbJogMove.ResumeLayout(false);
			this.gbMotorStatus.ResumeLayout(false);
			this.gbMotorStatus.PerformLayout();
			this.gbAllVelocity.ResumeLayout(false);
			this.gbAllVelocity.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbSelMotor;
        private System.Windows.Forms.Panel gpMotorSelect;
        private System.Windows.Forms.CheckBox chkMotorModify;
        private System.Windows.Forms.Button btnMotorSave;
        private System.Windows.Forms.Label ledHome;
        private System.Windows.Forms.Label ledOrg;
        private System.Windows.Forms.Label ledCcw;
        private System.Windows.Forms.Label ledCw;
        private System.Windows.Forms.Label ledInposi;
        private System.Windows.Forms.Label ledReady;
        private System.Windows.Forms.Label ledServo;
        private System.Windows.Forms.Label ledAlarm;
        private System.Windows.Forms.Button btnIndexMove;
        private System.Windows.Forms.Button btnPosIndexWrite;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbIndexNo;
        private System.Windows.Forms.Timer tmrMotor;
        private System.Windows.Forms.Timer tmrMotor_Flash;
        private System.Windows.Forms.Label lbMotorSelect;
        private System.Windows.Forms.DataGridView dgridMotor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbActPos;
        private System.Windows.Forms.Label lbCurPos;
        private System.Windows.Forms.Label lbDistance;
        private System.Windows.Forms.Label lbJogDist;
        private System.Windows.Forms.Button btnDispUp;
        private System.Windows.Forms.Button btnDispDn;
        private System.Windows.Forms.ComboBox cbDistLevel;
        private System.Windows.Forms.Label lbVelocity;
        private System.Windows.Forms.Button btnStepMoveDn;
        private System.Windows.Forms.Label lbJogVel;
        private System.Windows.Forms.Button btnStepMoveUp;
        private System.Windows.Forms.Button btnJogMoveUp;
        private System.Windows.Forms.Button btnVelUp;
        private System.Windows.Forms.Button btnVelDn;
        private System.Windows.Forms.ComboBox cbVelLevel;
        private System.Windows.Forms.CheckBox chkMotorJog;
        private System.Windows.Forms.Button btnJogMoveDn;
        private System.Windows.Forms.GroupBox gbMotorMove;
        private System.Windows.Forms.GroupBox gbMotorStatus;
        private System.Windows.Forms.GroupBox gbJogMove;
        private System.Windows.Forms.GroupBox gbStepMove;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnMotorNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnMotorName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnMotorPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnMotorVelocity;
        private System.Windows.Forms.Label lbJogPlus;
        private System.Windows.Forms.Label lbJogMinus;
        private System.Windows.Forms.Label lbStepPlus;
        private System.Windows.Forms.Label lbStepMinus;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lbMtNo;
        private System.Windows.Forms.Button btnVelIndexWrite;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRepeatMove;
        private System.Windows.Forms.GroupBox gbAllVelocity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbAllVelocity;
		private System.Windows.Forms.ContextMenuStrip cmsGridMotor;
		private System.Windows.Forms.ToolStripMenuItem iNDEXWRITEToolStripMenuItem;
		private System.Windows.Forms.Label lbItkMsg;
		private System.Windows.Forms.Label lbInterlockMsg;
		private System.Windows.Forms.Button btnTeachingMode;
		private System.Windows.Forms.Button btnManualCyl;
	}
}