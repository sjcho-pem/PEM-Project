namespace H_MarsSSD
{
    partial class Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.plMainCenter = new System.Windows.Forms.Panel();
			this.plMainBtm = new System.Windows.Forms.Panel();
			this.lbPwMode = new System.Windows.Forms.Label();
			this.lbID = new System.Windows.Forms.Label();
			this.lbRunTime = new System.Windows.Forms.Label();
			this.lbxVersion = new System.Windows.Forms.Label();
			this.lbx_CurrentTime = new System.Windows.Forms.Label();
			this.plMainLeft = new System.Windows.Forms.Panel();
			this.btnError = new System.Windows.Forms.Button();
			this.btnMotor = new System.Windows.Forms.Button();
			this.btnMain = new System.Windows.Forms.Button();
			this.btnLog = new System.Windows.Forms.Button();
			this.btnIO = new System.Windows.Forms.Button();
			this.btnData = new System.Windows.Forms.Button();
			this.btnManual = new System.Windows.Forms.Button();
			this.plMainTop = new System.Windows.Forms.Panel();
			this.btnMng = new System.Windows.Forms.Button();
			this.btnLogIn = new System.Windows.Forms.Button();
			this.lbSimMode = new System.Windows.Forms.Label();
			this.btnMinimization = new System.Windows.Forms.Button();
			this.btnLanguage = new System.Windows.Forms.Button();
			this.panelErr = new System.Windows.Forms.Panel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.btnX_Quit = new System.Windows.Forms.Button();
			this.plRight = new System.Windows.Forms.Panel();
			this.plMainBtm.SuspendLayout();
			this.plMainLeft.SuspendLayout();
			this.plMainTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// plMainCenter
			// 
			this.plMainCenter.BackColor = System.Drawing.Color.Transparent;
			this.plMainCenter.Location = new System.Drawing.Point(166, 153);
			this.plMainCenter.Margin = new System.Windows.Forms.Padding(0);
			this.plMainCenter.Name = "plMainCenter";
			this.plMainCenter.Size = new System.Drawing.Size(1463, 883);
			this.plMainCenter.TabIndex = 59;
			// 
			// plMainBtm
			// 
			this.plMainBtm.BackColor = System.Drawing.Color.GhostWhite;
			this.plMainBtm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.plMainBtm.Controls.Add(this.lbPwMode);
			this.plMainBtm.Controls.Add(this.lbID);
			this.plMainBtm.Controls.Add(this.lbRunTime);
			this.plMainBtm.Controls.Add(this.lbxVersion);
			this.plMainBtm.Controls.Add(this.lbx_CurrentTime);
			this.plMainBtm.Location = new System.Drawing.Point(0, 1036);
			this.plMainBtm.Name = "plMainBtm";
			this.plMainBtm.Size = new System.Drawing.Size(1920, 45);
			this.plMainBtm.TabIndex = 60;
			// 
			// lbPwMode
			// 
			this.lbPwMode.BackColor = System.Drawing.Color.Transparent;
			this.lbPwMode.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbPwMode.ForeColor = System.Drawing.Color.DimGray;
			this.lbPwMode.Location = new System.Drawing.Point(163, 8);
			this.lbPwMode.Margin = new System.Windows.Forms.Padding(3);
			this.lbPwMode.Name = "lbPwMode";
			this.lbPwMode.Size = new System.Drawing.Size(153, 25);
			this.lbPwMode.TabIndex = 69;
			this.lbPwMode.Text = "PROFESSIONAL";
			this.lbPwMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbID
			// 
			this.lbID.BackColor = System.Drawing.Color.Transparent;
			this.lbID.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbID.ForeColor = System.Drawing.Color.DimGray;
			this.lbID.Location = new System.Drawing.Point(7, 8);
			this.lbID.Margin = new System.Windows.Forms.Padding(3);
			this.lbID.Name = "lbID";
			this.lbID.Size = new System.Drawing.Size(150, 25);
			this.lbID.TabIndex = 67;
			this.lbID.Text = "ID";
			this.lbID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbRunTime
			// 
			this.lbRunTime.AutoSize = true;
			this.lbRunTime.BackColor = System.Drawing.Color.Transparent;
			this.lbRunTime.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbRunTime.ForeColor = System.Drawing.Color.DimGray;
			this.lbRunTime.Location = new System.Drawing.Point(342, 8);
			this.lbRunTime.Name = "lbRunTime";
			this.lbRunTime.Size = new System.Drawing.Size(103, 25);
			this.lbRunTime.TabIndex = 68;
			this.lbRunTime.Text = "RUN TIME";
			this.lbRunTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbxVersion
			// 
			this.lbxVersion.AutoSize = true;
			this.lbxVersion.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.lbxVersion.ForeColor = System.Drawing.Color.DimGray;
			this.lbxVersion.Location = new System.Drawing.Point(1474, 7);
			this.lbxVersion.Name = "lbxVersion";
			this.lbxVersion.Size = new System.Drawing.Size(109, 28);
			this.lbxVersion.TabIndex = 58;
			this.lbxVersion.Text = "Ver.0.0.0.0";
			this.lbxVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbx_CurrentTime
			// 
			this.lbx_CurrentTime.AutoSize = true;
			this.lbx_CurrentTime.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.lbx_CurrentTime.ForeColor = System.Drawing.Color.DimGray;
			this.lbx_CurrentTime.Location = new System.Drawing.Point(1637, 4);
			this.lbx_CurrentTime.Name = "lbx_CurrentTime";
			this.lbx_CurrentTime.Size = new System.Drawing.Size(252, 32);
			this.lbx_CurrentTime.TabIndex = 57;
			this.lbx_CurrentTime.Text = "1982-05-22 21:00:00";
			this.lbx_CurrentTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// plMainLeft
			// 
			this.plMainLeft.BackColor = System.Drawing.Color.GhostWhite;
			this.plMainLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.plMainLeft.Controls.Add(this.btnError);
			this.plMainLeft.Controls.Add(this.btnMotor);
			this.plMainLeft.Controls.Add(this.btnMain);
			this.plMainLeft.Controls.Add(this.btnLog);
			this.plMainLeft.Controls.Add(this.btnIO);
			this.plMainLeft.Controls.Add(this.btnData);
			this.plMainLeft.Controls.Add(this.btnManual);
			this.plMainLeft.Font = new System.Drawing.Font("휴먼둥근헤드라인", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.plMainLeft.Location = new System.Drawing.Point(0, 153);
			this.plMainLeft.Margin = new System.Windows.Forms.Padding(0);
			this.plMainLeft.Name = "plMainLeft";
			this.plMainLeft.Size = new System.Drawing.Size(166, 884);
			this.plMainLeft.TabIndex = 61;
			// 
			// btnError
			// 
			this.btnError.BackColor = System.Drawing.Color.White;
			this.btnError.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnError.Location = new System.Drawing.Point(10, 503);
			this.btnError.Name = "btnError";
			this.btnError.Size = new System.Drawing.Size(142, 90);
			this.btnError.TabIndex = 18;
			this.btnError.Text = "ERROR";
			this.btnError.UseVisualStyleBackColor = false;
			this.btnError.Click += new System.EventHandler(this.btnError_Click);
			// 
			// btnMotor
			// 
			this.btnMotor.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnMotor.BackColor = System.Drawing.Color.White;
			this.btnMotor.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnMotor.ForeColor = System.Drawing.Color.Black;
			this.btnMotor.Location = new System.Drawing.Point(11, 305);
			this.btnMotor.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.btnMotor.Name = "btnMotor";
			this.btnMotor.Size = new System.Drawing.Size(142, 90);
			this.btnMotor.TabIndex = 15;
			this.btnMotor.Text = "MOTOR";
			this.btnMotor.UseVisualStyleBackColor = false;
			this.btnMotor.Click += new System.EventHandler(this.btnMotor_Click);
			// 
			// btnMain
			// 
			this.btnMain.BackColor = System.Drawing.Color.White;
			this.btnMain.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnMain.ForeColor = System.Drawing.Color.Black;
			this.btnMain.Location = new System.Drawing.Point(11, 5);
			this.btnMain.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.btnMain.Name = "btnMain";
			this.btnMain.Size = new System.Drawing.Size(142, 90);
			this.btnMain.TabIndex = 5;
			this.btnMain.Text = "MAIN";
			this.btnMain.UseVisualStyleBackColor = false;
			this.btnMain.Click += new System.EventHandler(this.btnMain_Click);
			// 
			// btnLog
			// 
			this.btnLog.BackColor = System.Drawing.Color.White;
			this.btnLog.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnLog.ForeColor = System.Drawing.Color.Black;
			this.btnLog.Location = new System.Drawing.Point(11, 601);
			this.btnLog.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.btnLog.Name = "btnLog";
			this.btnLog.Size = new System.Drawing.Size(142, 90);
			this.btnLog.TabIndex = 10;
			this.btnLog.Text = "LOG";
			this.btnLog.UseVisualStyleBackColor = false;
			this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
			// 
			// btnIO
			// 
			this.btnIO.BackColor = System.Drawing.Color.White;
			this.btnIO.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnIO.ForeColor = System.Drawing.Color.Black;
			this.btnIO.Location = new System.Drawing.Point(11, 405);
			this.btnIO.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.btnIO.Name = "btnIO";
			this.btnIO.Size = new System.Drawing.Size(142, 90);
			this.btnIO.TabIndex = 7;
			this.btnIO.Text = "I/O";
			this.btnIO.UseVisualStyleBackColor = false;
			this.btnIO.Click += new System.EventHandler(this.btnIO_Click);
			// 
			// btnData
			// 
			this.btnData.BackColor = System.Drawing.Color.White;
			this.btnData.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnData.ForeColor = System.Drawing.Color.Black;
			this.btnData.Location = new System.Drawing.Point(11, 205);
			this.btnData.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.btnData.Name = "btnData";
			this.btnData.Size = new System.Drawing.Size(142, 90);
			this.btnData.TabIndex = 8;
			this.btnData.Text = "DATA";
			this.btnData.UseVisualStyleBackColor = false;
			this.btnData.Click += new System.EventHandler(this.btnData_Click);
			// 
			// btnManual
			// 
			this.btnManual.BackColor = System.Drawing.Color.White;
			this.btnManual.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnManual.ForeColor = System.Drawing.Color.Black;
			this.btnManual.Location = new System.Drawing.Point(11, 105);
			this.btnManual.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.btnManual.Name = "btnManual";
			this.btnManual.Size = new System.Drawing.Size(142, 90);
			this.btnManual.TabIndex = 9;
			this.btnManual.Text = "MANUAL";
			this.btnManual.UseVisualStyleBackColor = false;
			this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
			// 
			// plMainTop
			// 
			this.plMainTop.BackColor = System.Drawing.Color.GhostWhite;
			this.plMainTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.plMainTop.Controls.Add(this.btnMng);
			this.plMainTop.Controls.Add(this.btnLogIn);
			this.plMainTop.Controls.Add(this.lbSimMode);
			this.plMainTop.Controls.Add(this.btnMinimization);
			this.plMainTop.Controls.Add(this.btnLanguage);
			this.plMainTop.Controls.Add(this.panelErr);
			this.plMainTop.Controls.Add(this.pictureBox1);
			this.plMainTop.Controls.Add(this.btnX_Quit);
			this.plMainTop.Font = new System.Drawing.Font("휴먼둥근헤드라인", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.plMainTop.Location = new System.Drawing.Point(0, 0);
			this.plMainTop.Margin = new System.Windows.Forms.Padding(0);
			this.plMainTop.Name = "plMainTop";
			this.plMainTop.Size = new System.Drawing.Size(1920, 154);
			this.plMainTop.TabIndex = 62;
			// 
			// btnMng
			// 
			this.btnMng.BackColor = System.Drawing.Color.Transparent;
			this.btnMng.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnMng.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.btnMng.Location = new System.Drawing.Point(1349, 6);
			this.btnMng.Name = "btnMng";
			this.btnMng.Size = new System.Drawing.Size(188, 35);
			this.btnMng.TabIndex = 69;
			this.btnMng.Text = "MANAGEMENT";
			this.btnMng.UseVisualStyleBackColor = false;
			this.btnMng.Visible = false;
			this.btnMng.Click += new System.EventHandler(this.btnMng_Click);
			// 
			// btnLogIn
			// 
			this.btnLogIn.BackColor = System.Drawing.Color.GhostWhite;
			this.btnLogIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.btnLogIn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.btnLogIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLogIn.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.btnLogIn.Image = global::H_MarsSSD.Properties.Resources.LogOutState;
			this.btnLogIn.Location = new System.Drawing.Point(1543, 6);
			this.btnLogIn.Name = "btnLogIn";
			this.btnLogIn.Size = new System.Drawing.Size(134, 35);
			this.btnLogIn.TabIndex = 68;
			this.btnLogIn.UseVisualStyleBackColor = false;
			this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
			// 
			// lbSimMode
			// 
			this.lbSimMode.BackColor = System.Drawing.Color.Red;
			this.lbSimMode.ForeColor = System.Drawing.Color.White;
			this.lbSimMode.Location = new System.Drawing.Point(165, 11);
			this.lbSimMode.Name = "lbSimMode";
			this.lbSimMode.Size = new System.Drawing.Size(632, 33);
			this.lbSimMode.TabIndex = 67;
			this.lbSimMode.Text = "SIMULATION MODE - Motor Velocity X10";
			this.lbSimMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.lbSimMode.Visible = false;
			this.lbSimMode.DoubleClick += new System.EventHandler(this.lbSimMode_DoubleClick);
			// 
			// btnMinimization
			// 
			this.btnMinimization.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnMinimization.AutoSize = true;
			this.btnMinimization.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnMinimization.BackColor = System.Drawing.Color.Transparent;
			this.btnMinimization.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnMinimization.Image = global::H_MarsSSD.Properties.Resources.minimization2;
			this.btnMinimization.Location = new System.Drawing.Point(1824, 3);
			this.btnMinimization.Name = "btnMinimization";
			this.btnMinimization.Size = new System.Drawing.Size(38, 38);
			this.btnMinimization.TabIndex = 65;
			this.btnMinimization.UseVisualStyleBackColor = false;
			this.btnMinimization.Click += new System.EventHandler(this.btnMinimization_Click);
			// 
			// btnLanguage
			// 
			this.btnLanguage.BackColor = System.Drawing.Color.Transparent;
			this.btnLanguage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLanguage.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.btnLanguage.Location = new System.Drawing.Point(1683, 6);
			this.btnLanguage.Name = "btnLanguage";
			this.btnLanguage.Size = new System.Drawing.Size(135, 35);
			this.btnLanguage.TabIndex = 62;
			this.btnLanguage.Text = "ENGLISH";
			this.btnLanguage.UseVisualStyleBackColor = false;
			this.btnLanguage.Click += new System.EventHandler(this.btnLanguage_Click);
			// 
			// panelErr
			// 
			this.panelErr.BackColor = System.Drawing.Color.Transparent;
			this.panelErr.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.panelErr.Location = new System.Drawing.Point(164, 50);
			this.panelErr.Name = "panelErr";
			this.panelErr.Size = new System.Drawing.Size(1742, 95);
			this.panelErr.TabIndex = 29;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(39, 38);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(186, 72);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Visible = false;
			// 
			// btnX_Quit
			// 
			this.btnX_Quit.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnX_Quit.AutoSize = true;
			this.btnX_Quit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnX_Quit.BackColor = System.Drawing.Color.Transparent;
			this.btnX_Quit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnX_Quit.Image = ((System.Drawing.Image)(resources.GetObject("btnX_Quit.Image")));
			this.btnX_Quit.Location = new System.Drawing.Point(1868, 3);
			this.btnX_Quit.Name = "btnX_Quit";
			this.btnX_Quit.Size = new System.Drawing.Size(38, 38);
			this.btnX_Quit.TabIndex = 1;
			this.btnX_Quit.UseVisualStyleBackColor = false;
			this.btnX_Quit.Click += new System.EventHandler(this.btnX_Quit_Click);
			// 
			// plRight
			// 
			this.plRight.BackColor = System.Drawing.Color.Transparent;
			this.plRight.Location = new System.Drawing.Point(1629, 154);
			this.plRight.Margin = new System.Windows.Forms.Padding(0);
			this.plRight.Name = "plRight";
			this.plRight.Size = new System.Drawing.Size(290, 883);
			this.plRight.TabIndex = 63;
			// 
			// Main
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ClientSize = new System.Drawing.Size(1920, 1080);
			this.ControlBox = false;
			this.Controls.Add(this.plRight);
			this.Controls.Add(this.plMainTop);
			this.Controls.Add(this.plMainLeft);
			this.Controls.Add(this.plMainBtm);
			this.Controls.Add(this.plMainCenter);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
			this.Load += new System.EventHandler(this.Main_Load);
			this.plMainBtm.ResumeLayout(false);
			this.plMainBtm.PerformLayout();
			this.plMainLeft.ResumeLayout(false);
			this.plMainTop.ResumeLayout(false);
			this.plMainTop.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plMainCenter;
        private System.Windows.Forms.Panel plMainBtm;
        private System.Windows.Forms.Panel plMainLeft;
        private System.Windows.Forms.Button btnMotor;
        private System.Windows.Forms.Button btnMain;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button btnIO;
        private System.Windows.Forms.Button btnData;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Panel plMainTop;
        private System.Windows.Forms.Panel panelErr;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnX_Quit;
        private System.Windows.Forms.Label lbx_CurrentTime;
        private System.Windows.Forms.Panel plRight;
        private System.Windows.Forms.Label lbxVersion;
        private System.Windows.Forms.Button btnLanguage;
        private System.Windows.Forms.Button btnError;
		private System.Windows.Forms.Button btnMinimization;
		private System.Windows.Forms.Label lbSimMode;
		private System.Windows.Forms.Button btnMng;
		private System.Windows.Forms.Button btnLogIn;
		private System.Windows.Forms.Label lbPwMode;
		private System.Windows.Forms.Label lbID;
		private System.Windows.Forms.Label lbRunTime;
	}
}

