namespace H_MarsSSD
{
    partial class frmLanguageSelect
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
            this.btnEng = new System.Windows.Forms.Button();
            this.btnKor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEng
            // 
            this.btnEng.BackColor = System.Drawing.Color.LemonChiffon;
            this.btnEng.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnEng.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
            this.btnEng.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnEng.Location = new System.Drawing.Point(12, 12);
            this.btnEng.Name = "btnEng";
            this.btnEng.Size = new System.Drawing.Size(329, 61);
            this.btnEng.TabIndex = 27;
            this.btnEng.TabStop = false;
            this.btnEng.Text = "ENGLISH";
            this.btnEng.UseVisualStyleBackColor = false;
            this.btnEng.Click += new System.EventHandler(this.btnEng_Click);
            // 
            // btnKor
            // 
            this.btnKor.BackColor = System.Drawing.SystemColors.Window;
            this.btnKor.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnKor.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
            this.btnKor.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnKor.Location = new System.Drawing.Point(12, 79);
            this.btnKor.Name = "btnKor";
            this.btnKor.Size = new System.Drawing.Size(329, 61);
            this.btnKor.TabIndex = 28;
            this.btnKor.TabStop = false;
            this.btnKor.Text = "KOREAN";
            this.btnKor.UseVisualStyleBackColor = false;
            this.btnKor.Click += new System.EventHandler(this.btnKor_Click);
            // 
            // frmLanguageSelect
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Azure;
            this.ClientSize = new System.Drawing.Size(354, 154);
            this.Controls.Add(this.btnKor);
            this.Controls.Add(this.btnEng);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLanguageSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLanguageSelect";
            this.Load += new System.EventHandler(this.frmLanguageSelect_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmLanguageSelect_Paint);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnEng;
        private System.Windows.Forms.Button btnKor;
    }
}