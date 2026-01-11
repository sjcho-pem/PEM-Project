namespace H_MarsSSD
{
	partial class frmLevelSetting
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.cbLevelSetting = new System.Windows.Forms.ComboBox();
			this.chkIO = new System.Windows.Forms.CheckBox();
			this.chkMotor = new System.Windows.Forms.CheckBox();
			this.chkData = new System.Windows.Forms.CheckBox();
			this.chkManual = new System.Windows.Forms.CheckBox();
			this.chkLog = new System.Windows.Forms.CheckBox();
			this.chkError = new System.Windows.Forms.CheckBox();
			this.chkMain = new System.Windows.Forms.CheckBox();
			this.btnSave = new System.Windows.Forms.Button();
			this.chkDM = new System.Windows.Forms.CheckBox();
			this.chkBit = new System.Windows.Forms.CheckBox();
			this.chkUseSkip = new System.Windows.Forms.CheckBox();
			this.chkRecipe = new System.Windows.Forms.CheckBox();
			this.chkPw = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.gbMenu = new System.Windows.Forms.GroupBox();
			this.gbData = new System.Windows.Forms.GroupBox();
			this.chkSDM = new System.Windows.Forms.CheckBox();
			this.gbPassword = new System.Windows.Forms.GroupBox();
			this.btnSetDefault = new System.Windows.Forms.Button();
			this.gbMotor = new System.Windows.Forms.GroupBox();
			this.chkTeaching = new System.Windows.Forms.CheckBox();
			this.gbMenu.SuspendLayout();
			this.gbData.SuspendLayout();
			this.gbPassword.SuspendLayout();
			this.gbMotor.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.Transparent;
			this.btnCancel.FlatAppearance.BorderSize = 0;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnCancel.Image = global::H_MarsSSD.Properties.Resources.main_close;
			this.btnCancel.Location = new System.Drawing.Point(727, 4);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(38, 38);
			this.btnCancel.TabIndex = 16;
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// cbLevelSetting
			// 
			this.cbLevelSetting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbLevelSetting.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cbLevelSetting.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbLevelSetting.FormattingEnabled = true;
			this.cbLevelSetting.Items.AddRange(new object[] {
            "OPERATOR",
            "ENGINEER",
            "PROFESSIONAL"});
			this.cbLevelSetting.Location = new System.Drawing.Point(206, 11);
			this.cbLevelSetting.Name = "cbLevelSetting";
			this.cbLevelSetting.Size = new System.Drawing.Size(186, 38);
			this.cbLevelSetting.TabIndex = 19;
			this.cbLevelSetting.SelectedIndexChanged += new System.EventHandler(this.cbLevelSetting_SelectedIndexChanged);
			// 
			// chkIO
			// 
			this.chkIO.AutoSize = true;
			this.chkIO.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkIO.Location = new System.Drawing.Point(133, 55);
			this.chkIO.Name = "chkIO";
			this.chkIO.Size = new System.Drawing.Size(47, 21);
			this.chkIO.TabIndex = 22;
			this.chkIO.Text = "I/O";
			this.chkIO.UseVisualStyleBackColor = true;
			this.chkIO.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkMotor
			// 
			this.chkMotor.AutoSize = true;
			this.chkMotor.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkMotor.Location = new System.Drawing.Point(19, 134);
			this.chkMotor.Name = "chkMotor";
			this.chkMotor.Size = new System.Drawing.Size(74, 21);
			this.chkMotor.TabIndex = 23;
			this.chkMotor.Text = "MOTOR";
			this.chkMotor.UseVisualStyleBackColor = true;
			this.chkMotor.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkData
			// 
			this.chkData.AutoSize = true;
			this.chkData.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkData.Location = new System.Drawing.Point(19, 95);
			this.chkData.Name = "chkData";
			this.chkData.Size = new System.Drawing.Size(61, 21);
			this.chkData.TabIndex = 24;
			this.chkData.Text = "DATA";
			this.chkData.UseVisualStyleBackColor = true;
			this.chkData.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkManual
			// 
			this.chkManual.AutoSize = true;
			this.chkManual.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkManual.Location = new System.Drawing.Point(19, 55);
			this.chkManual.Name = "chkManual";
			this.chkManual.Size = new System.Drawing.Size(83, 21);
			this.chkManual.TabIndex = 25;
			this.chkManual.Text = "MANUAL";
			this.chkManual.UseVisualStyleBackColor = true;
			this.chkManual.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkLog
			// 
			this.chkLog.AutoSize = true;
			this.chkLog.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkLog.Location = new System.Drawing.Point(133, 132);
			this.chkLog.Name = "chkLog";
			this.chkLog.Size = new System.Drawing.Size(53, 21);
			this.chkLog.TabIndex = 26;
			this.chkLog.Text = "LOG";
			this.chkLog.UseVisualStyleBackColor = true;
			this.chkLog.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkError
			// 
			this.chkError.AutoSize = true;
			this.chkError.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkError.Location = new System.Drawing.Point(133, 95);
			this.chkError.Name = "chkError";
			this.chkError.Size = new System.Drawing.Size(68, 21);
			this.chkError.TabIndex = 27;
			this.chkError.Text = "ERROR";
			this.chkError.UseVisualStyleBackColor = true;
			this.chkError.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkMain
			// 
			this.chkMain.AutoSize = true;
			this.chkMain.Checked = true;
			this.chkMain.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkMain.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkMain.Location = new System.Drawing.Point(19, 55);
			this.chkMain.Name = "chkMain";
			this.chkMain.Size = new System.Drawing.Size(58, 21);
			this.chkMain.TabIndex = 29;
			this.chkMain.Text = "Main";
			this.chkMain.UseVisualStyleBackColor = true;
			this.chkMain.Visible = false;
			this.chkMain.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// btnSave
			// 
			this.btnSave.AutoEllipsis = true;
			this.btnSave.BackColor = System.Drawing.Color.AliceBlue;
			this.btnSave.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
			this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSave.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnSave.Location = new System.Drawing.Point(329, 302);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(109, 46);
			this.btnSave.TabIndex = 29;
			this.btnSave.Text = "SAVE";
			this.btnSave.UseVisualStyleBackColor = false;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnSave.KeyDown += new System.Windows.Forms.KeyEventHandler(this.btnSave_KeyDown);
			// 
			// chkDM
			// 
			this.chkDM.AutoSize = true;
			this.chkDM.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkDM.Location = new System.Drawing.Point(19, 93);
			this.chkDM.Name = "chkDM";
			this.chkDM.Size = new System.Drawing.Size(49, 21);
			this.chkDM.TabIndex = 28;
			this.chkDM.Text = "DM";
			this.chkDM.UseVisualStyleBackColor = true;
			this.chkDM.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkBit
			// 
			this.chkBit.AutoSize = true;
			this.chkBit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkBit.Location = new System.Drawing.Point(19, 172);
			this.chkBit.Name = "chkBit";
			this.chkBit.Size = new System.Drawing.Size(47, 21);
			this.chkBit.TabIndex = 27;
			this.chkBit.Text = "BIT";
			this.chkBit.UseVisualStyleBackColor = true;
			this.chkBit.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkUseSkip
			// 
			this.chkUseSkip.AutoSize = true;
			this.chkUseSkip.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkUseSkip.Location = new System.Drawing.Point(126, 55);
			this.chkUseSkip.Name = "chkUseSkip";
			this.chkUseSkip.Size = new System.Drawing.Size(83, 21);
			this.chkUseSkip.TabIndex = 26;
			this.chkUseSkip.Text = "USE/SKIP";
			this.chkUseSkip.UseVisualStyleBackColor = true;
			this.chkUseSkip.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkRecipe
			// 
			this.chkRecipe.AutoSize = true;
			this.chkRecipe.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkRecipe.Location = new System.Drawing.Point(19, 55);
			this.chkRecipe.Name = "chkRecipe";
			this.chkRecipe.Size = new System.Drawing.Size(69, 21);
			this.chkRecipe.TabIndex = 25;
			this.chkRecipe.Text = "RECIPE";
			this.chkRecipe.UseVisualStyleBackColor = true;
			this.chkRecipe.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// chkPw
			// 
			this.chkPw.AutoSize = true;
			this.chkPw.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this.chkPw.Location = new System.Drawing.Point(16, 49);
			this.chkPw.Name = "chkPw";
			this.chkPw.Size = new System.Drawing.Size(161, 21);
			this.chkPw.TabIndex = 0;
			this.chkPw.Text = "CONFRIM PASSWORD";
			this.chkPw.UseVisualStyleBackColor = true;
			this.chkPw.CheckedChanged += new System.EventHandler(this.chkTotal_CheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.Black;
			this.label2.Location = new System.Drawing.Point(12, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(186, 32);
			this.label2.TabIndex = 29;
			this.label2.Text = "LEVEL SETTING";
			// 
			// gbMenu
			// 
			this.gbMenu.BackColor = System.Drawing.Color.White;
			this.gbMenu.Controls.Add(this.chkManual);
			this.gbMenu.Controls.Add(this.chkMain);
			this.gbMenu.Controls.Add(this.chkLog);
			this.gbMenu.Controls.Add(this.chkData);
			this.gbMenu.Controls.Add(this.chkError);
			this.gbMenu.Controls.Add(this.chkIO);
			this.gbMenu.Controls.Add(this.chkMotor);
			this.gbMenu.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbMenu.Location = new System.Drawing.Point(12, 64);
			this.gbMenu.Name = "gbMenu";
			this.gbMenu.Size = new System.Drawing.Size(244, 227);
			this.gbMenu.TabIndex = 30;
			this.gbMenu.TabStop = false;
			this.gbMenu.Text = "MENU";
			// 
			// gbData
			// 
			this.gbData.BackColor = System.Drawing.Color.White;
			this.gbData.Controls.Add(this.chkSDM);
			this.gbData.Controls.Add(this.chkUseSkip);
			this.gbData.Controls.Add(this.chkDM);
			this.gbData.Controls.Add(this.chkRecipe);
			this.gbData.Controls.Add(this.chkBit);
			this.gbData.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbData.Location = new System.Drawing.Point(262, 64);
			this.gbData.Name = "gbData";
			this.gbData.Size = new System.Drawing.Size(244, 227);
			this.gbData.TabIndex = 31;
			this.gbData.TabStop = false;
			this.gbData.Text = "DATA";
			// 
			// chkSDM
			// 
			this.chkSDM.AutoSize = true;
			this.chkSDM.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkSDM.Location = new System.Drawing.Point(19, 133);
			this.chkSDM.Name = "chkSDM";
			this.chkSDM.Size = new System.Drawing.Size(56, 21);
			this.chkSDM.TabIndex = 29;
			this.chkSDM.Text = "SDM";
			this.chkSDM.UseVisualStyleBackColor = true;
			// 
			// gbPassword
			// 
			this.gbPassword.BackColor = System.Drawing.Color.White;
			this.gbPassword.Controls.Add(this.chkPw);
			this.gbPassword.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbPassword.Location = new System.Drawing.Point(512, 183);
			this.gbPassword.Name = "gbPassword";
			this.gbPassword.Size = new System.Drawing.Size(244, 108);
			this.gbPassword.TabIndex = 32;
			this.gbPassword.TabStop = false;
			this.gbPassword.Text = "PW";
			// 
			// btnSetDefault
			// 
			this.btnSetDefault.AutoEllipsis = true;
			this.btnSetDefault.BackColor = System.Drawing.Color.AliceBlue;
			this.btnSetDefault.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
			this.btnSetDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnSetDefault.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnSetDefault.Location = new System.Drawing.Point(419, 11);
			this.btnSetDefault.Name = "btnSetDefault";
			this.btnSetDefault.Size = new System.Drawing.Size(129, 38);
			this.btnSetDefault.TabIndex = 33;
			this.btnSetDefault.Text = "Set Default";
			this.btnSetDefault.UseVisualStyleBackColor = false;
			this.btnSetDefault.Click += new System.EventHandler(this.btnSetDefault_Click);
			// 
			// gbMotor
			// 
			this.gbMotor.BackColor = System.Drawing.Color.White;
			this.gbMotor.Controls.Add(this.chkTeaching);
			this.gbMotor.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gbMotor.Location = new System.Drawing.Point(512, 64);
			this.gbMotor.Name = "gbMotor";
			this.gbMotor.Size = new System.Drawing.Size(244, 108);
			this.gbMotor.TabIndex = 33;
			this.gbMotor.TabStop = false;
			this.gbMotor.Text = "MOTOR INTERLOCK";
			// 
			// chkTeaching
			// 
			this.chkTeaching.AutoSize = true;
			this.chkTeaching.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
			this.chkTeaching.Location = new System.Drawing.Point(16, 51);
			this.chkTeaching.Name = "chkTeaching";
			this.chkTeaching.Size = new System.Drawing.Size(135, 21);
			this.chkTeaching.TabIndex = 0;
			this.chkTeaching.Text = "TEACHING MODE";
			this.chkTeaching.UseVisualStyleBackColor = true;
			// 
			// frmLevelSetting
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(768, 359);
			this.Controls.Add(this.gbMotor);
			this.Controls.Add(this.btnSetDefault);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.gbPassword);
			this.Controls.Add(this.gbData);
			this.Controls.Add(this.gbMenu);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cbLevelSetting);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmLevelSetting";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmLevelSetting";
			this.Load += new System.EventHandler(this.frmLevelSetting_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmLevelSetting_Paint);
			this.gbMenu.ResumeLayout(false);
			this.gbMenu.PerformLayout();
			this.gbData.ResumeLayout(false);
			this.gbData.PerformLayout();
			this.gbPassword.ResumeLayout(false);
			this.gbPassword.PerformLayout();
			this.gbMotor.ResumeLayout(false);
			this.gbMotor.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.ComboBox cbLevelSetting;
		private System.Windows.Forms.CheckBox chkIO;
		private System.Windows.Forms.CheckBox chkMotor;
		private System.Windows.Forms.CheckBox chkData;
		private System.Windows.Forms.CheckBox chkManual;
		private System.Windows.Forms.CheckBox chkLog;
		private System.Windows.Forms.CheckBox chkError;
		private System.Windows.Forms.CheckBox chkPw;
		private System.Windows.Forms.CheckBox chkDM;
		private System.Windows.Forms.CheckBox chkBit;
		private System.Windows.Forms.CheckBox chkUseSkip;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkMain;
        private System.Windows.Forms.GroupBox gbMenu;
        private System.Windows.Forms.GroupBox gbData;
        private System.Windows.Forms.GroupBox gbPassword;
        private System.Windows.Forms.CheckBox chkRecipe;
        private System.Windows.Forms.Button btnSetDefault;
        private System.Windows.Forms.CheckBox chkSDM;
        private System.Windows.Forms.GroupBox gbMotor;
        private System.Windows.Forms.CheckBox chkTeaching;
    }
}