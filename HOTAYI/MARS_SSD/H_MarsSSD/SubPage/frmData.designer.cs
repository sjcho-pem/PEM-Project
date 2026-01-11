namespace H_MarsSSD
{
    partial class frmData
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
			this.panelData = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnUseskip = new System.Windows.Forms.Button();
			this.btnBit = new System.Windows.Forms.Button();
			this.btnDm = new System.Windows.Forms.Button();
			this.btnRecipe = new System.Windows.Forms.Button();
			this.btnSDm = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// panelData
			// 
			this.panelData.BackColor = System.Drawing.Color.GhostWhite;
			this.panelData.Location = new System.Drawing.Point(0, 58);
			this.panelData.Name = "panelData";
			this.panelData.Size = new System.Drawing.Size(1463, 824);
			this.panelData.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.SystemColors.WindowText;
			this.panel1.Location = new System.Drawing.Point(0, 58);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(1463, 2);
			this.panel1.TabIndex = 23;
			// 
			// btnUseskip
			// 
			this.btnUseskip.BackColor = System.Drawing.Color.White;
			this.btnUseskip.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnUseskip.Location = new System.Drawing.Point(382, 7);
			this.btnUseskip.Name = "btnUseskip";
			this.btnUseskip.Size = new System.Drawing.Size(170, 45);
			this.btnUseskip.TabIndex = 22;
			this.btnUseskip.Text = "USE/SKIP";
			this.btnUseskip.UseVisualStyleBackColor = false;
			this.btnUseskip.Click += new System.EventHandler(this.btnUseskip_Click);
			// 
			// btnBit
			// 
			this.btnBit.BackColor = System.Drawing.Color.White;
			this.btnBit.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnBit.Location = new System.Drawing.Point(758, 7);
			this.btnBit.Name = "btnBit";
			this.btnBit.Size = new System.Drawing.Size(170, 45);
			this.btnBit.TabIndex = 21;
			this.btnBit.Text = "BIT";
			this.btnBit.UseVisualStyleBackColor = false;
			this.btnBit.Click += new System.EventHandler(this.btnBit_Click);
			// 
			// btnDm
			// 
			this.btnDm.BackColor = System.Drawing.Color.White;
			this.btnDm.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnDm.Location = new System.Drawing.Point(194, 7);
			this.btnDm.Name = "btnDm";
			this.btnDm.Size = new System.Drawing.Size(170, 45);
			this.btnDm.TabIndex = 19;
			this.btnDm.Text = "DM";
			this.btnDm.UseVisualStyleBackColor = false;
			this.btnDm.Click += new System.EventHandler(this.btnDm_Click);
			// 
			// btnRecipe
			// 
			this.btnRecipe.BackColor = System.Drawing.Color.White;
			this.btnRecipe.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnRecipe.Location = new System.Drawing.Point(6, 7);
			this.btnRecipe.Name = "btnRecipe";
			this.btnRecipe.Size = new System.Drawing.Size(170, 45);
			this.btnRecipe.TabIndex = 18;
			this.btnRecipe.Text = "RECIPE";
			this.btnRecipe.UseVisualStyleBackColor = false;
			this.btnRecipe.Click += new System.EventHandler(this.btnRecipe_Click);
			// 
			// btnSDm
			// 
			this.btnSDm.BackColor = System.Drawing.Color.White;
			this.btnSDm.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnSDm.Location = new System.Drawing.Point(570, 7);
			this.btnSDm.Name = "btnSDm";
			this.btnSDm.Size = new System.Drawing.Size(170, 45);
			this.btnSDm.TabIndex = 25;
			this.btnSDm.Text = "SDM";
			this.btnSDm.UseVisualStyleBackColor = false;
			this.btnSDm.Click += new System.EventHandler(this.btnSDm_Click);
			// 
			// frmData
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 881);
			this.Controls.Add(this.btnSDm);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnRecipe);
			this.Controls.Add(this.btnBit);
			this.Controls.Add(this.btnUseskip);
			this.Controls.Add(this.btnDm);
			this.Controls.Add(this.panelData);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmData";
			this.Text = "frmData";
			this.Load += new System.EventHandler(this.frmData_Load);
			this.VisibleChanged += new System.EventHandler(this.frmData_VisibleChanged);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelData;
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnUseskip;
		private System.Windows.Forms.Button btnBit;
		private System.Windows.Forms.Button btnDm;
		private System.Windows.Forms.Button btnRecipe;
		private System.Windows.Forms.Button btnSDm;
	}
}