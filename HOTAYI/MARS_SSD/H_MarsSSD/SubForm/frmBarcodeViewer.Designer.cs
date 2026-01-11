namespace H_MarsSSD
{
	partial class frmBarcodeViewer
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
			this.picResultImage = new System.Windows.Forms.PictureBox();
			this.lbDeviceNameText = new System.Windows.Forms.Label();
			this.lbDeviceName = new System.Windows.Forms.Label();
			this.lbBarcodeInfo = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lbQRInfoText = new System.Windows.Forms.Label();
			this.btnTrigger = new System.Windows.Forms.Button();
			this.chkLive = new System.Windows.Forms.CheckBox();
			this.btmClose = new System.Windows.Forms.Button();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.rbUnload1 = new System.Windows.Forms.RadioButton();
			this.rbLoader2 = new System.Windows.Forms.RadioButton();
			this.rbLoader1 = new System.Windows.Forms.RadioButton();
			this.rbReject1 = new System.Windows.Forms.RadioButton();
			this.rbReject2 = new System.Windows.Forms.RadioButton();
			this.rbUnload2 = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.picResultImage)).BeginInit();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// picResultImage
			// 
			this.picResultImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tableLayoutPanel1.SetColumnSpan(this.picResultImage, 8);
			this.picResultImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picResultImage.Location = new System.Drawing.Point(3, 128);
			this.picResultImage.Name = "picResultImage";
			this.picResultImage.Size = new System.Drawing.Size(674, 416);
			this.picResultImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picResultImage.TabIndex = 68;
			this.picResultImage.TabStop = false;
			// 
			// lbDeviceNameText
			// 
			this.lbDeviceNameText.BackColor = System.Drawing.Color.White;
			this.lbDeviceNameText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lbDeviceNameText.Location = new System.Drawing.Point(3, 11);
			this.lbDeviceNameText.Name = "lbDeviceNameText";
			this.lbDeviceNameText.Size = new System.Drawing.Size(138, 21);
			this.lbDeviceNameText.TabIndex = 69;
			this.lbDeviceNameText.Text = "DEVICE NAME   : ";
			// 
			// lbDeviceName
			// 
			this.lbDeviceName.AutoSize = true;
			this.lbDeviceName.BackColor = System.Drawing.Color.White;
			this.lbDeviceName.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lbDeviceName.Location = new System.Drawing.Point(137, 11);
			this.lbDeviceName.Name = "lbDeviceName";
			this.lbDeviceName.Size = new System.Drawing.Size(0, 21);
			this.lbDeviceName.TabIndex = 70;
			// 
			// lbBarcodeInfo
			// 
			this.lbBarcodeInfo.AutoSize = true;
			this.lbBarcodeInfo.BackColor = System.Drawing.Color.White;
			this.lbBarcodeInfo.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lbBarcodeInfo.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbBarcodeInfo.Location = new System.Drawing.Point(138, 43);
			this.lbBarcodeInfo.Name = "lbBarcodeInfo";
			this.lbBarcodeInfo.Size = new System.Drawing.Size(0, 21);
			this.lbBarcodeInfo.TabIndex = 71;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.White;
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tableLayoutPanel1.SetColumnSpan(this.panel1, 8);
			this.panel1.Controls.Add(this.lbDeviceName);
			this.panel1.Controls.Add(this.lbQRInfoText);
			this.panel1.Controls.Add(this.lbDeviceNameText);
			this.panel1.Controls.Add(this.lbBarcodeInfo);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(674, 79);
			this.panel1.TabIndex = 72;
			// 
			// lbQRInfoText
			// 
			this.lbQRInfoText.AutoSize = true;
			this.lbQRInfoText.BackColor = System.Drawing.Color.White;
			this.lbQRInfoText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.lbQRInfoText.ForeColor = System.Drawing.Color.MidnightBlue;
			this.lbQRInfoText.Location = new System.Drawing.Point(3, 42);
			this.lbQRInfoText.Name = "lbQRInfoText";
			this.lbQRInfoText.Size = new System.Drawing.Size(82, 21);
			this.lbQRInfoText.TabIndex = 72;
			this.lbQRInfoText.Text = "QR INFO :";
			// 
			// btnTrigger
			// 
			this.btnTrigger.BackColor = System.Drawing.Color.White;
			this.btnTrigger.Dock = System.Windows.Forms.DockStyle.Right;
			this.btnTrigger.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnTrigger.Location = new System.Drawing.Point(600, 88);
			this.btnTrigger.Name = "btnTrigger";
			this.btnTrigger.Size = new System.Drawing.Size(77, 34);
			this.btnTrigger.TabIndex = 73;
			this.btnTrigger.Text = "TRIGGER";
			this.btnTrigger.UseVisualStyleBackColor = false;
			this.btnTrigger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnTrigger_MouseDown);
			this.btnTrigger.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnTrigger_MouseUp);
			// 
			// chkLive
			// 
			this.chkLive.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.chkLive.AutoSize = true;
			this.chkLive.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkLive.Location = new System.Drawing.Point(545, 96);
			this.chkLive.Name = "chkLive";
			this.chkLive.Size = new System.Drawing.Size(47, 17);
			this.chkLive.TabIndex = 74;
			this.chkLive.Text = "LIVE";
			this.chkLive.UseVisualStyleBackColor = true;
			this.chkLive.CheckStateChanged += new System.EventHandler(this.chkLive_CheckStateChanged);
			// 
			// btmClose
			// 
			this.btmClose.BackColor = System.Drawing.Color.White;
			this.btmClose.Dock = System.Windows.Forms.DockStyle.Right;
			this.btmClose.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btmClose.Location = new System.Drawing.Point(600, 550);
			this.btmClose.Name = "btmClose";
			this.btmClose.Size = new System.Drawing.Size(77, 46);
			this.btmClose.TabIndex = 75;
			this.btmClose.Text = "CLOSE";
			this.btmClose.UseVisualStyleBackColor = false;
			this.btmClose.Click += new System.EventHandler(this.btmClose_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
			this.tableLayoutPanel1.ColumnCount = 8;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.5F));
			this.tableLayoutPanel1.Controls.Add(this.rbUnload1, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.rbLoader2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btmClose, 7, 3);
			this.tableLayoutPanel1.Controls.Add(this.chkLive, 6, 1);
			this.tableLayoutPanel1.Controls.Add(this.picResultImage, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnTrigger, 7, 1);
			this.tableLayoutPanel1.Controls.Add(this.rbLoader1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.rbReject1, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.rbReject2, 5, 1);
			this.tableLayoutPanel1.Controls.Add(this.rbUnload2, 3, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.36615F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.56594F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.067913F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(680, 599);
			this.tableLayoutPanel1.TabIndex = 76;
			// 
			// rbUnload1
			// 
			this.rbUnload1.AutoSize = true;
			this.rbUnload1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rbUnload1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbUnload1.Location = new System.Drawing.Point(173, 88);
			this.rbUnload1.Name = "rbUnload1";
			this.rbUnload1.Size = new System.Drawing.Size(79, 34);
			this.rbUnload1.TabIndex = 78;
			this.rbUnload1.TabStop = true;
			this.rbUnload1.Text = "UNLOAD1";
			this.rbUnload1.UseVisualStyleBackColor = true;
			this.rbUnload1.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
			// 
			// rbLoader2
			// 
			this.rbLoader2.AutoSize = true;
			this.rbLoader2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rbLoader2.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.rbLoader2.Location = new System.Drawing.Point(88, 88);
			this.rbLoader2.Name = "rbLoader2";
			this.rbLoader2.Size = new System.Drawing.Size(79, 34);
			this.rbLoader2.TabIndex = 77;
			this.rbLoader2.TabStop = true;
			this.rbLoader2.Text = "LOADER2";
			this.rbLoader2.UseVisualStyleBackColor = true;
			this.rbLoader2.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
			// 
			// rbLoader1
			// 
			this.rbLoader1.AutoSize = true;
			this.rbLoader1.Checked = true;
			this.rbLoader1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rbLoader1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbLoader1.Location = new System.Drawing.Point(3, 88);
			this.rbLoader1.Name = "rbLoader1";
			this.rbLoader1.Size = new System.Drawing.Size(79, 34);
			this.rbLoader1.TabIndex = 76;
			this.rbLoader1.TabStop = true;
			this.rbLoader1.Text = "LOADER1";
			this.rbLoader1.UseVisualStyleBackColor = true;
			this.rbLoader1.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
			// 
			// rbReject1
			// 
			this.rbReject1.AutoSize = true;
			this.rbReject1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rbReject1.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.rbReject1.Location = new System.Drawing.Point(343, 88);
			this.rbReject1.Name = "rbReject1";
			this.rbReject1.Size = new System.Drawing.Size(79, 34);
			this.rbReject1.TabIndex = 79;
			this.rbReject1.TabStop = true;
			this.rbReject1.Text = "REJECT 1";
			this.rbReject1.UseVisualStyleBackColor = true;
			this.rbReject1.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
			// 
			// rbReject2
			// 
			this.rbReject2.AutoSize = true;
			this.rbReject2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rbReject2.Font = new System.Drawing.Font("Segoe UI", 8F);
			this.rbReject2.Location = new System.Drawing.Point(428, 88);
			this.rbReject2.Name = "rbReject2";
			this.rbReject2.Size = new System.Drawing.Size(79, 34);
			this.rbReject2.TabIndex = 80;
			this.rbReject2.TabStop = true;
			this.rbReject2.Text = "REJECT 2";
			this.rbReject2.UseVisualStyleBackColor = true;
			this.rbReject2.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
			// 
			// rbUnload2
			// 
			this.rbUnload2.AutoSize = true;
			this.rbUnload2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rbUnload2.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.rbUnload2.Location = new System.Drawing.Point(258, 88);
			this.rbUnload2.Name = "rbUnload2";
			this.rbUnload2.Size = new System.Drawing.Size(79, 34);
			this.rbUnload2.TabIndex = 81;
			this.rbUnload2.TabStop = true;
			this.rbUnload2.Text = "UNLOAD2";
			this.rbUnload2.UseVisualStyleBackColor = true;
			this.rbUnload2.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
			// 
			// frmBarcodeViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(680, 599);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.MinimumSize = new System.Drawing.Size(477, 615);
			this.Name = "frmBarcodeViewer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "QR Viewer";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmBarcodeViewer_Load);
			((System.ComponentModel.ISupportInitialize)(this.picResultImage)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.PictureBox picResultImage;
		private System.Windows.Forms.Label lbDeviceNameText;
		private System.Windows.Forms.Label lbDeviceName;
		private System.Windows.Forms.Label lbBarcodeInfo;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lbQRInfoText;
		private System.Windows.Forms.Button btnTrigger;
		private System.Windows.Forms.CheckBox chkLive;
		private System.Windows.Forms.Button btmClose;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.RadioButton rbLoader2;
		private System.Windows.Forms.RadioButton rbLoader1;
		private System.Windows.Forms.RadioButton rbUnload1;
		private System.Windows.Forms.RadioButton rbReject1;
		private System.Windows.Forms.RadioButton rbReject2;
		private System.Windows.Forms.RadioButton rbUnload2;
	}
}