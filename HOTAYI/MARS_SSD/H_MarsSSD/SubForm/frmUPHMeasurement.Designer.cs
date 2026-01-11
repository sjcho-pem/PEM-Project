namespace H_MarsSSD
{
	partial class frmUPHMeasurement
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tmrUPHUpdate = new System.Windows.Forms.Timer(this.components);
			this.tmrAlarmUpdate = new System.Windows.Forms.Timer(this.components);
			this.btnStop = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.lbUPH = new System.Windows.Forms.Label();
			this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvAlarm = new System.Windows.Forms.DataGridView();
			this.lbFirstInsp = new System.Windows.Forms.Label();
			this.lbProdCnt = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lbLastInsp = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgvAlarm)).BeginInit();
			this.SuspendLayout();
			// 
			// tmrUPHUpdate
			// 
			this.tmrUPHUpdate.Interval = 500;
			this.tmrUPHUpdate.Tick += new System.EventHandler(this.tmrUPHUpdate_Tick);
			// 
			// tmrAlarmUpdate
			// 
			this.tmrAlarmUpdate.Interval = 2000;
			// 
			// btnStop
			// 
			this.btnStop.BackColor = System.Drawing.Color.White;
			this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStop.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnStop.ForeColor = System.Drawing.Color.Red;
			this.btnStop.Location = new System.Drawing.Point(276, 3);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(268, 47);
			this.btnStop.TabIndex = 2;
			this.btnStop.Text = "STOP";
			this.btnStop.UseVisualStyleBackColor = false;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnStart
			// 
			this.btnStart.BackColor = System.Drawing.Color.White;
			this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnStart.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.btnStart.ForeColor = System.Drawing.Color.Lime;
			this.btnStart.Location = new System.Drawing.Point(3, 3);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(267, 47);
			this.btnStart.TabIndex = 1;
			this.btnStart.Text = "START";
			this.btnStart.UseVisualStyleBackColor = false;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = System.Drawing.Color.Black;
			this.label4.Location = new System.Drawing.Point(199, 70);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(153, 26);
			this.label4.TabIndex = 85;
			this.label4.Text = "UPH";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbUPH
			// 
			this.lbUPH.BackColor = System.Drawing.Color.Transparent;
			this.lbUPH.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
			this.lbUPH.ForeColor = System.Drawing.Color.Black;
			this.lbUPH.Location = new System.Drawing.Point(160, 79);
			this.lbUPH.Name = "lbUPH";
			this.lbUPH.Size = new System.Drawing.Size(230, 75);
			this.lbUPH.TabIndex = 84;
			this.lbUPH.Text = "0";
			this.lbUPH.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dataGridViewTextBoxColumn4
			// 
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle5;
			this.dataGridViewTextBoxColumn4.HeaderText = "Count";
			this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			this.dataGridViewTextBoxColumn4.ReadOnly = true;
			this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn4.Width = 70;
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle6;
			this.dataGridViewTextBoxColumn1.HeaderText = "Alarm";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dgvAlarm
			// 
			this.dgvAlarm.AllowUserToAddRows = false;
			this.dgvAlarm.AllowUserToDeleteRows = false;
			this.dgvAlarm.AllowUserToResizeRows = false;
			this.dgvAlarm.BackgroundColor = System.Drawing.Color.White;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvAlarm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dgvAlarm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvAlarm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn4});
			this.dgvAlarm.Location = new System.Drawing.Point(3, 266);
			this.dgvAlarm.MultiSelect = false;
			this.dgvAlarm.Name = "dgvAlarm";
			this.dgvAlarm.ReadOnly = true;
			this.dgvAlarm.RowHeadersVisible = false;
			dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI Symbol", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.dgvAlarm.RowsDefaultCellStyle = dataGridViewCellStyle8;
			this.dgvAlarm.RowTemplate.Height = 23;
			this.dgvAlarm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvAlarm.Size = new System.Drawing.Size(541, 106);
			this.dgvAlarm.TabIndex = 81;
			// 
			// lbFirstInsp
			// 
			this.lbFirstInsp.BackColor = System.Drawing.Color.White;
			this.lbFirstInsp.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbFirstInsp.ForeColor = System.Drawing.Color.Black;
			this.lbFirstInsp.Location = new System.Drawing.Point(243, 199);
			this.lbFirstInsp.Name = "lbFirstInsp";
			this.lbFirstInsp.Size = new System.Drawing.Size(296, 25);
			this.lbFirstInsp.TabIndex = 93;
			this.lbFirstInsp.Text = "00:00:00.000";
			this.lbFirstInsp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbProdCnt
			// 
			this.lbProdCnt.BackColor = System.Drawing.Color.White;
			this.lbProdCnt.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbProdCnt.ForeColor = System.Drawing.Color.Black;
			this.lbProdCnt.Location = new System.Drawing.Point(242, 167);
			this.lbProdCnt.Name = "lbProdCnt";
			this.lbProdCnt.Size = new System.Drawing.Size(297, 25);
			this.lbProdCnt.TabIndex = 97;
			this.lbProdCnt.Text = "0";
			this.lbProdCnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.AliceBlue;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.label7.ForeColor = System.Drawing.Color.Black;
			this.label7.Location = new System.Drawing.Point(7, 167);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(229, 25);
			this.label7.TabIndex = 96;
			this.label7.Text = "PROD COUNT";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Navy;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(8, 199);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(229, 25);
			this.label3.TabIndex = 98;
			this.label3.Text = "FIRST INSP";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbLastInsp
			// 
			this.lbLastInsp.BackColor = System.Drawing.Color.White;
			this.lbLastInsp.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbLastInsp.ForeColor = System.Drawing.Color.Black;
			this.lbLastInsp.Location = new System.Drawing.Point(242, 229);
			this.lbLastInsp.Name = "lbLastInsp";
			this.lbLastInsp.Size = new System.Drawing.Size(297, 25);
			this.lbLastInsp.TabIndex = 100;
			this.lbLastInsp.Text = "00:00:00.000";
			this.lbLastInsp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label9
			// 
			this.label9.BackColor = System.Drawing.Color.Navy;
			this.label9.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.label9.ForeColor = System.Drawing.Color.White;
			this.label9.Location = new System.Drawing.Point(8, 229);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(229, 25);
			this.label9.TabIndex = 99;
			this.label9.Text = "LAST INSP";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmUPHMeasurement
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(549, 377);
			this.Controls.Add(this.lbLastInsp);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.lbProdCnt);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.lbFirstInsp);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.lbUPH);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.dgvAlarm);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(565, 416);
			this.Name = "frmUPHMeasurement";
			this.Text = "UPH Measurement";
			this.Load += new System.EventHandler(this.frmUPHMeasurement_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvAlarm)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Timer tmrUPHUpdate;
		private System.Windows.Forms.Timer tmrAlarmUpdate;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lbUPH;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridView dgvAlarm;
		private System.Windows.Forms.Label lbFirstInsp;
		private System.Windows.Forms.Label lbProdCnt;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lbLastInsp;
		private System.Windows.Forms.Label label9;
	}
}