namespace H_MarsSSD
{
    partial class frmManual
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
			this.plManual = new System.Windows.Forms.Panel();
			this.panel = new System.Windows.Forms.Panel();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.btnTenkey = new System.Windows.Forms.Button();
			this.btnManualSeq = new System.Windows.Forms.Button();
			this.btnManualCyl = new System.Windows.Forms.Button();
			this.btnManualTCP = new System.Windows.Forms.Button();
			this.btnManualEtc = new System.Windows.Forms.Button();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// plManual
			// 
			this.plManual.Location = new System.Drawing.Point(0, 58);
			this.plManual.Name = "plManual";
			this.plManual.Size = new System.Drawing.Size(1463, 824);
			this.plManual.TabIndex = 29;
			// 
			// panel
			// 
			this.panel.BackColor = System.Drawing.SystemColors.WindowText;
			this.panel.Location = new System.Drawing.Point(0, 58);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(1463, 2);
			this.panel.TabIndex = 30;
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.Controls.Add(this.btnManualCyl);
			this.flowLayoutPanel1.Controls.Add(this.btnManualEtc);
			this.flowLayoutPanel1.Controls.Add(this.btnTenkey);
			this.flowLayoutPanel1.Controls.Add(this.btnManualTCP);
			this.flowLayoutPanel1.Controls.Add(this.btnManualSeq);
			this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 3);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(1365, 52);
			this.flowLayoutPanel1.TabIndex = 32;
			// 
			// btnTenkey
			// 
			this.btnTenkey.BackColor = System.Drawing.Color.White;
			this.btnTenkey.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnTenkey.Location = new System.Drawing.Point(423, 3);
			this.btnTenkey.Name = "btnTenkey";
			this.btnTenkey.Size = new System.Drawing.Size(170, 45);
			this.btnTenkey.TabIndex = 31;
			this.btnTenkey.Text = "TENKEY";
			this.btnTenkey.UseVisualStyleBackColor = false;
			this.btnTenkey.Click += new System.EventHandler(this.btnTenkey_Click);
			// 
			// btnManualSeq
			// 
			this.btnManualSeq.BackColor = System.Drawing.Color.White;
			this.btnManualSeq.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnManualSeq.Location = new System.Drawing.Point(799, 3);
			this.btnManualSeq.Name = "btnManualSeq";
			this.btnManualSeq.Size = new System.Drawing.Size(194, 45);
			this.btnManualSeq.TabIndex = 35;
			this.btnManualSeq.Text = "MANUAL SEQ";
			this.btnManualSeq.UseVisualStyleBackColor = false;
			this.btnManualSeq.Click += new System.EventHandler(this.btnManualSeq_Click);
			// 
			// btnManualCyl
			// 
			this.btnManualCyl.BackColor = System.Drawing.Color.White;
			this.btnManualCyl.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnManualCyl.Location = new System.Drawing.Point(3, 3);
			this.btnManualCyl.Name = "btnManualCyl";
			this.btnManualCyl.Size = new System.Drawing.Size(214, 45);
			this.btnManualCyl.TabIndex = 32;
			this.btnManualCyl.Text = "MANUAL CYL/VAC";
			this.btnManualCyl.UseVisualStyleBackColor = false;
			this.btnManualCyl.Click += new System.EventHandler(this.btnManualCyl_Click);
			// 
			// btnManualTCP
			// 
			this.btnManualTCP.BackColor = System.Drawing.Color.White;
			this.btnManualTCP.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnManualTCP.Location = new System.Drawing.Point(599, 3);
			this.btnManualTCP.Name = "btnManualTCP";
			this.btnManualTCP.Size = new System.Drawing.Size(194, 45);
			this.btnManualTCP.TabIndex = 33;
			this.btnManualTCP.Text = "MANUAL TCP";
			this.btnManualTCP.UseVisualStyleBackColor = false;
			this.btnManualTCP.Click += new System.EventHandler(this.btnManualTCP_Click);
			// 
			// btnManualEtc
			// 
			this.btnManualEtc.BackColor = System.Drawing.Color.White;
			this.btnManualEtc.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnManualEtc.Location = new System.Drawing.Point(223, 3);
			this.btnManualEtc.Name = "btnManualEtc";
			this.btnManualEtc.Size = new System.Drawing.Size(194, 45);
			this.btnManualEtc.TabIndex = 34;
			this.btnManualEtc.Text = "MANUAL etc.";
			this.btnManualEtc.UseVisualStyleBackColor = false;
			this.btnManualEtc.Click += new System.EventHandler(this.btnManualEtc_Click);
			// 
			// frmManual
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 881);
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.plManual);
			this.ForeColor = System.Drawing.SystemColors.Desktop;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmManual";
			this.Load += new System.EventHandler(this.frmManual_Load);
			this.VisibleChanged += new System.EventHandler(this.frmManual_VisibleChanged);
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel plManual;
        private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button btnTenkey;
		private System.Windows.Forms.Button btnManualSeq;
		private System.Windows.Forms.Button btnManualCyl;
		private System.Windows.Forms.Button btnManualTCP;
		private System.Windows.Forms.Button btnManualEtc;
	}
}