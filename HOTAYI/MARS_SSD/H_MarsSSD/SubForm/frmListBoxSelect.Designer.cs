namespace H_MarsSSD
{
	partial class frmListBoxSelect
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lbSelect = new System.Windows.Forms.ListBox();
			this.lbX_Title = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnOK.Location = new System.Drawing.Point(18, 5);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(176, 40);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btn_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnCancel.Location = new System.Drawing.Point(209, 5);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(176, 40);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btn_Click);
			// 
			// lbSelect
			// 
			this.lbSelect.Font = new System.Drawing.Font("Segoe UI Symbol", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbSelect.FormattingEnabled = true;
			this.lbSelect.ItemHeight = 25;
			this.lbSelect.Location = new System.Drawing.Point(18, 49);
			this.lbSelect.Name = "lbSelect";
			this.lbSelect.Size = new System.Drawing.Size(367, 129);
			this.lbSelect.TabIndex = 3;
			this.lbSelect.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbSelect_MouseDoubleClick);
			// 
			// lbX_Title
			// 
			this.lbX_Title.BackColor = System.Drawing.Color.Transparent;
			this.lbX_Title.Font = new System.Drawing.Font("Segoe UI Black", 18F, System.Drawing.FontStyle.Bold);
			this.lbX_Title.ForeColor = System.Drawing.Color.Black;
			this.lbX_Title.Location = new System.Drawing.Point(12, 11);
			this.lbX_Title.Name = "lbX_Title";
			this.lbX_Title.Size = new System.Drawing.Size(98, 35);
			this.lbX_Title.TabIndex = 54;
			this.lbX_Title.Text = "SELECT";
			this.lbX_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnOK);
			this.panel1.Controls.Add(this.btnCancel);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 184);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(407, 56);
			this.panel1.TabIndex = 55;
			// 
			// frmListBoxSelect
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(407, 240);
			this.ControlBox = false;
			this.Controls.Add(this.lbX_Title);
			this.Controls.Add(this.lbSelect);
			this.Controls.Add(this.panel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmListBoxSelect";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmListBoxSelect_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ListBox lbSelect;
        private System.Windows.Forms.Label lbX_Title;
		private System.Windows.Forms.Panel panel1;
	}
}