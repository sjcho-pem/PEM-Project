namespace H_MarsSSD
{
    partial class frmSelectMessageBox
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSelectMessageBox));
			this.lbX_Title = new System.Windows.Forms.Label();
			this.lbx_Quitbtn = new System.Windows.Forms.Label();
			this.lbx_Msg = new System.Windows.Forms.Label();
			this.tmr_ProgressState = new System.Windows.Forms.Timer(this.components);
			this.fpnButtons = new System.Windows.Forms.FlowLayoutPanel();
			this.progBtnDown = new H_MarsSSD.ucProgressBar();
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
			this.lbx_Quitbtn.Font = new System.Drawing.Font("휴먼둥근헤드라인", 21F, System.Drawing.FontStyle.Bold);
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
			// tmr_ProgressState
			// 
			this.tmr_ProgressState.Interval = 10;
			this.tmr_ProgressState.Tick += new System.EventHandler(this.tmr_ProgressState_Tick);
			// 
			// fpnButtons
			// 
			this.fpnButtons.BackColor = System.Drawing.Color.LightGray;
			this.fpnButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
			this.fpnButtons.Location = new System.Drawing.Point(12, 175);
			this.fpnButtons.Name = "fpnButtons";
			this.fpnButtons.Size = new System.Drawing.Size(502, 50);
			this.fpnButtons.TabIndex = 59;
			// 
			// progBtnDown
			// 
			this.progBtnDown.BackColor = System.Drawing.SystemColors.ControlDark;
			this.progBtnDown.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.progBtnDown.Location = new System.Drawing.Point(2, 233);
			this.progBtnDown.Maximum = 100;
			this.progBtnDown.Minimum = 0;
			this.progBtnDown.Name = "progBtnDown";
			this.progBtnDown.Size = new System.Drawing.Size(524, 6);
			this.progBtnDown.TabIndex = 60;
			this.progBtnDown.Value = 0;
			// 
			// frmSelectMessageBox
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(527, 241);
			this.Controls.Add(this.progBtnDown);
			this.Controls.Add(this.lbx_Msg);
			this.Controls.Add(this.lbx_Quitbtn);
			this.Controls.Add(this.lbX_Title);
			this.Controls.Add(this.fpnButtons);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmSelectMessageBox";
			this.Padding = new System.Windows.Forms.Padding(2, 0, 1, 2);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "frmSelectMessageBox";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmSelectMessageBox_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmSelectMessageBox_Paint);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbX_Title;
        private System.Windows.Forms.Label lbx_Quitbtn;
        private System.Windows.Forms.Label lbx_Msg;
        private System.Windows.Forms.Timer tmr_ProgressState;
		private System.Windows.Forms.FlowLayoutPanel fpnButtons;
		private ucProgressBar progBtnDown;
	}
}