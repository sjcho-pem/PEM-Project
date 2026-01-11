namespace H_MarsSSD
{
	partial class frmImageViewer
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
			this.picViewer = new System.Windows.Forms.PictureBox();
			this.btnClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picViewer)).BeginInit();
			this.SuspendLayout();
			// 
			// picViewer
			// 
			this.picViewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.picViewer.Location = new System.Drawing.Point(0, 0);
			this.picViewer.Name = "picViewer";
			this.picViewer.Size = new System.Drawing.Size(849, 627);
			this.picViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picViewer.TabIndex = 0;
			this.picViewer.TabStop = false;
			// 
			// btnClose
			// 
			this.btnClose.BackColor = System.Drawing.Color.Transparent;
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnClose.Location = new System.Drawing.Point(7, 6);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 45);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = false;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// frmImageViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(849, 627);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.picViewer);
			this.Name = "frmImageViewer";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Image Viewer";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmImageViewer_Load);
			((System.ComponentModel.ISupportInitialize)(this.picViewer)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox picViewer;
		private System.Windows.Forms.Button btnClose;
	}
}