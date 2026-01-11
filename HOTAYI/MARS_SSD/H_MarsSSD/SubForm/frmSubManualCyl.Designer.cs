namespace H_MarsSSD
{
	partial class frmSubManualCyl
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
			this.lbCylPartName = new System.Windows.Forms.Label();
			this.tlpCyl = new System.Windows.Forms.TableLayoutPanel();
			this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
			this.chkUseTransparency = new System.Windows.Forms.CheckBox();
			this.tbarOpacity = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.tbarOpacity)).BeginInit();
			this.SuspendLayout();
			// 
			// lbCylPartName
			// 
			this.lbCylPartName.AutoSize = true;
			this.lbCylPartName.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbCylPartName.Location = new System.Drawing.Point(10, 8);
			this.lbCylPartName.Name = "lbCylPartName";
			this.lbCylPartName.Size = new System.Drawing.Size(71, 30);
			this.lbCylPartName.TabIndex = 61;
			this.lbCylPartName.Text = "Name";
			// 
			// tlpCyl
			// 
			this.tlpCyl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tlpCyl.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tlpCyl.ColumnCount = 9;
			this.tlpCyl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11037F));
			this.tlpCyl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11038F));
			this.tlpCyl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11038F));
			this.tlpCyl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11038F));
			this.tlpCyl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11038F));
			this.tlpCyl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11038F));
			this.tlpCyl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11259F));
			this.tlpCyl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11402F));
			this.tlpCyl.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.11111F));
			this.tlpCyl.Location = new System.Drawing.Point(8, 50);
			this.tlpCyl.Margin = new System.Windows.Forms.Padding(0);
			this.tlpCyl.Name = "tlpCyl";
			this.tlpCyl.RowCount = 7;
			this.tlpCyl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpCyl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.8097F));
			this.tlpCyl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpCyl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.8097F));
			this.tlpCyl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
			this.tlpCyl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 23.80969F));
			this.tlpCyl.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 28.57092F));
			this.tlpCyl.Size = new System.Drawing.Size(835, 360);
			this.tlpCyl.TabIndex = 60;
			// 
			// tmrUpdate
			// 
			this.tmrUpdate.Enabled = true;
			this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
			// 
			// chkUseTransparency
			// 
			this.chkUseTransparency.AutoSize = true;
			this.chkUseTransparency.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkUseTransparency.Location = new System.Drawing.Point(574, 14);
			this.chkUseTransparency.Name = "chkUseTransparency";
			this.chkUseTransparency.Size = new System.Drawing.Size(92, 24);
			this.chkUseTransparency.TabIndex = 62;
			this.chkUseTransparency.Text = "배경 투명";
			this.chkUseTransparency.UseVisualStyleBackColor = true;
			this.chkUseTransparency.CheckedChanged += new System.EventHandler(this.chkUseTransparency_CheckedChanged);
			// 
			// tbarOpacity
			// 
			this.tbarOpacity.Location = new System.Drawing.Point(672, 14);
			this.tbarOpacity.Maximum = 100;
			this.tbarOpacity.Minimum = 20;
			this.tbarOpacity.Name = "tbarOpacity";
			this.tbarOpacity.Size = new System.Drawing.Size(173, 45);
			this.tbarOpacity.TabIndex = 63;
			this.tbarOpacity.TickStyle = System.Windows.Forms.TickStyle.None;
			this.tbarOpacity.Value = 100;
			this.tbarOpacity.Scroll += new System.EventHandler(this.tbarOpacity_Scroll);
			// 
			// frmSubManualCyl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(852, 420);
			this.Controls.Add(this.lbCylPartName);
			this.Controls.Add(this.tlpCyl);
			this.Controls.Add(this.chkUseTransparency);
			this.Controls.Add(this.tbarOpacity);
			this.MaximizeBox = false;
			this.Name = "frmSubManualCyl";
			this.Text = "SubManualCyl";
			this.TopMost = true;
			((System.ComponentModel.ISupportInitialize)(this.tbarOpacity)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label lbCylPartName;
		private System.Windows.Forms.TableLayoutPanel tlpCyl;
		private System.Windows.Forms.Timer tmrUpdate;
		private System.Windows.Forms.CheckBox chkUseTransparency;
		private System.Windows.Forms.TrackBar tbarOpacity;
	}
}