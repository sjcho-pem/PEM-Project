namespace H_MarsSSD
{
    partial class frmMessageBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMessageBox));
			this.lbX_Title = new System.Windows.Forms.Label();
			this.lbx_Quitbtn = new System.Windows.Forms.Label();
			this.lbx_Msg = new System.Windows.Forms.Label();
			this.btn_Cancel = new System.Windows.Forms.Button();
			this.btn_OK = new System.Windows.Forms.Button();
			this.pgBar_Timer = new System.Windows.Forms.ProgressBar();
			this.tm_ProgressState = new System.Windows.Forms.Timer(this.components);
			this.tm_ActiveState = new System.Windows.Forms.Timer(this.components);
			this.btn_OK2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lbX_Title
			// 
			this.lbX_Title.BackColor = System.Drawing.Color.Transparent;
			this.lbX_Title.Font = new System.Drawing.Font("Segoe UI Black", 18F, System.Drawing.FontStyle.Bold);
			this.lbX_Title.ForeColor = System.Drawing.Color.Black;
			this.lbX_Title.Location = new System.Drawing.Point(12, 10);
			this.lbX_Title.Name = "lbX_Title";
			this.lbX_Title.Size = new System.Drawing.Size(229, 35);
			this.lbX_Title.TabIndex = 53;
			this.lbX_Title.Text = "MESSAGE BOX";
			this.lbX_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbx_Quitbtn
			// 
			this.lbx_Quitbtn.BackColor = System.Drawing.SystemColors.ControlDarkDark;
			this.lbx_Quitbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 21F, System.Drawing.FontStyle.Bold);
			this.lbx_Quitbtn.Image = ((System.Drawing.Image)(resources.GetObject("lbx_Quitbtn.Image")));
			this.lbx_Quitbtn.Location = new System.Drawing.Point(478, 9);
			this.lbx_Quitbtn.Name = "lbx_Quitbtn";
			this.lbx_Quitbtn.Size = new System.Drawing.Size(36, 35);
			this.lbx_Quitbtn.TabIndex = 54;
			this.lbx_Quitbtn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbx_Quitbtn.Click += new System.EventHandler(this.lb_Quitbtn_Click);
			this.lbx_Quitbtn.MouseLeave += new System.EventHandler(this.lbx_Quitbtn_MouseLeave);
			this.lbx_Quitbtn.MouseHover += new System.EventHandler(this.lbx_Quitbtn_MouseHover);
			// 
			// lbx_Msg
			// 
			this.lbx_Msg.BackColor = System.Drawing.Color.WhiteSmoke;
			this.lbx_Msg.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.lbx_Msg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(110)))), ((int)(((byte)(200)))));
			this.lbx_Msg.Location = new System.Drawing.Point(12, 50);
			this.lbx_Msg.Name = "lbx_Msg";
			this.lbx_Msg.Size = new System.Drawing.Size(503, 112);
			this.lbx_Msg.TabIndex = 55;
			this.lbx_Msg.Text = "MESSAGE";
			this.lbx_Msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btn_Cancel
			// 
			this.btn_Cancel.BackColor = System.Drawing.SystemColors.Control;
			this.btn_Cancel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.btn_Cancel.Location = new System.Drawing.Point(286, 175);
			this.btn_Cancel.Name = "btn_Cancel";
			this.btn_Cancel.Size = new System.Drawing.Size(130, 50);
			this.btn_Cancel.TabIndex = 57;
			this.btn_Cancel.Text = "CANCEL";
			this.btn_Cancel.UseVisualStyleBackColor = false;
			this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
			// 
			// btn_OK
			// 
			this.btn_OK.BackColor = System.Drawing.SystemColors.Control;
			this.btn_OK.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btn_OK.Location = new System.Drawing.Point(111, 175);
			this.btn_OK.Name = "btn_OK";
			this.btn_OK.Size = new System.Drawing.Size(130, 50);
			this.btn_OK.TabIndex = 56;
			this.btn_OK.Text = "OK";
			this.btn_OK.UseVisualStyleBackColor = false;
			this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
			// 
			// pgBar_Timer
			// 
			this.pgBar_Timer.BackColor = System.Drawing.Color.LightGray;
			this.pgBar_Timer.Location = new System.Drawing.Point(12, 175);
			this.pgBar_Timer.Maximum = 500;
			this.pgBar_Timer.Name = "pgBar_Timer";
			this.pgBar_Timer.Size = new System.Drawing.Size(502, 50);
			this.pgBar_Timer.Step = 50;
			this.pgBar_Timer.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.pgBar_Timer.TabIndex = 58;
			// 
			// tm_ProgressState
			// 
			this.tm_ProgressState.Tick += new System.EventHandler(this.tm_ProgressState_Tick);
			// 
			// tm_ActiveState
			// 
			this.tm_ActiveState.Tick += new System.EventHandler(this.tm_ActiveState_Tick);
			// 
			// btn_OK2
			// 
			this.btn_OK2.BackColor = System.Drawing.SystemColors.Control;
			this.btn_OK2.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btn_OK2.Location = new System.Drawing.Point(196, 175);
			this.btn_OK2.Name = "btn_OK2";
			this.btn_OK2.Size = new System.Drawing.Size(130, 50);
			this.btn_OK2.TabIndex = 59;
			this.btn_OK2.Text = "OK";
			this.btn_OK2.UseVisualStyleBackColor = false;
			this.btn_OK2.Click += new System.EventHandler(this.btn_OK2_Click);
			// 
			// frmMessageBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(527, 232);
			this.Controls.Add(this.btn_OK2);
			this.Controls.Add(this.btn_Cancel);
			this.Controls.Add(this.btn_OK);
			this.Controls.Add(this.lbx_Msg);
			this.Controls.Add(this.lbx_Quitbtn);
			this.Controls.Add(this.lbX_Title);
			this.Controls.Add(this.pgBar_Timer);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmMessageBox";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "frmMessageBox";
			this.TopMost = true;
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMessageBox_Paint);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbX_Title;
        private System.Windows.Forms.Label lbx_Quitbtn;
        private System.Windows.Forms.Label lbx_Msg;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.ProgressBar pgBar_Timer;
        private System.Windows.Forms.Timer tm_ProgressState;
        private System.Windows.Forms.Timer tm_ActiveState;
		private System.Windows.Forms.Button btn_OK2;
	}
}