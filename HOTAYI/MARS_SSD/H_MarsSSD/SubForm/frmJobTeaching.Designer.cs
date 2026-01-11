namespace H_MarsSSD
{
    partial class frmJobTeaching
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
			this.lbText = new System.Windows.Forms.Label();
			this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
			this.tmrTableUp = new System.Windows.Forms.Timer(this.components);
			this.pb = new System.Windows.Forms.PictureBox();
			this.btnNext = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pb)).BeginInit();
			this.SuspendLayout();
			// 
			// lbText
			// 
			this.lbText.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbText.Location = new System.Drawing.Point(22, 342);
			this.lbText.Name = "lbText";
			this.lbText.Size = new System.Drawing.Size(523, 48);
			this.lbText.TabIndex = 7;
			this.lbText.Text = " STEP 1.  Remove materials";
			this.lbText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// elementHost1
			// 
			this.elementHost1.Location = new System.Drawing.Point(84, 3);
			this.elementHost1.Name = "elementHost1";
			this.elementHost1.Size = new System.Drawing.Size(550, 304);
			this.elementHost1.TabIndex = 58;
			this.elementHost1.Text = "elementHost1";
			this.elementHost1.Child = null;
			// 
			// tmrTableUp
			// 
			this.tmrTableUp.Tick += new System.EventHandler(this.tmrTableUp_Tick);
			// 
			// pb
			// 
			this.pb.Image = global::H_MarsSSD.Properties.Resources.Layout;
			this.pb.Location = new System.Drawing.Point(7, 3);
			this.pb.Name = "pb";
			this.pb.Size = new System.Drawing.Size(715, 304);
			this.pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pb.TabIndex = 57;
			this.pb.TabStop = false;
			// 
			// btnNext
			// 
			this.btnNext.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnNext.Location = new System.Drawing.Point(635, 342);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(79, 59);
			this.btnNext.TabIndex = 8;
			this.btnNext.Text = "NEXT";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// frmJobTeaching
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(726, 443);
			this.Controls.Add(this.elementHost1);
			this.Controls.Add(this.pb);
			this.Controls.Add(this.btnNext);
			this.Controls.Add(this.lbText);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmJobTeaching";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "JOB TEACHING";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmJobTeaching_Load);
			this.VisibleChanged += new System.EventHandler(this.frmJobTeaching_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.pb)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lbText;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.PictureBox pb;
		private System.Windows.Forms.Integration.ElementHost elementHost1;
		private System.Windows.Forms.Timer tmrTableUp;
	}
}