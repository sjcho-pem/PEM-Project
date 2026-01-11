namespace H_MarsSSD
{
	partial class frmLogin
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
			this.txtPw = new System.Windows.Forms.TextBox();
			this.btnLogin = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnKeyboardPW = new System.Windows.Forms.Button();
			this.btnKeyboardID = new System.Windows.Forms.Button();
			this.lbWarning = new System.Windows.Forms.Label();
			this.picVisiblePwNotUse = new System.Windows.Forms.PictureBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cbID = new System.Windows.Forms.ComboBox();
			((System.ComponentModel.ISupportInitialize)(this.picVisiblePwNotUse)).BeginInit();
			this.SuspendLayout();
			// 
			// txtPw
			// 
			this.txtPw.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPw.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.txtPw.Location = new System.Drawing.Point(55, 134);
			this.txtPw.Name = "txtPw";
			this.txtPw.Size = new System.Drawing.Size(211, 33);
			this.txtPw.TabIndex = 1;
			this.txtPw.UseSystemPasswordChar = true;
			this.txtPw.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPw_KeyUp);
			// 
			// btnLogin
			// 
			this.btnLogin.AutoEllipsis = true;
			this.btnLogin.BackColor = System.Drawing.Color.AliceBlue;
			this.btnLogin.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
			this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.btnLogin.Location = new System.Drawing.Point(56, 191);
			this.btnLogin.Name = "btnLogin";
			this.btnLogin.Size = new System.Drawing.Size(210, 43);
			this.btnLogin.TabIndex = 2;
			this.btnLogin.Text = "LOG IN";
			this.btnLogin.UseVisualStyleBackColor = false;
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label1.Location = new System.Drawing.Point(51, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(23, 19);
			this.label1.TabIndex = 17;
			this.label1.Text = "ID";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label2.Location = new System.Drawing.Point(52, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(87, 19);
			this.label2.TabIndex = 18;
			this.label2.Text = "PASSWORD";
			// 
			// btnKeyboardPW
			// 
			this.btnKeyboardPW.BackColor = System.Drawing.Color.Gainsboro;
			this.btnKeyboardPW.FlatAppearance.BorderSize = 0;
			this.btnKeyboardPW.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKeyboardPW.Image = global::H_MarsSSD.Properties.Resources.Keyboard24p;
			this.btnKeyboardPW.Location = new System.Drawing.Point(268, 134);
			this.btnKeyboardPW.Name = "btnKeyboardPW";
			this.btnKeyboardPW.Size = new System.Drawing.Size(36, 30);
			this.btnKeyboardPW.TabIndex = 32;
			this.btnKeyboardPW.UseVisualStyleBackColor = false;
			this.btnKeyboardPW.Click += new System.EventHandler(this.btnKeyboardPW_Click);
			// 
			// btnKeyboardID
			// 
			this.btnKeyboardID.BackColor = System.Drawing.Color.Gainsboro;
			this.btnKeyboardID.FlatAppearance.BorderSize = 0;
			this.btnKeyboardID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKeyboardID.Image = global::H_MarsSSD.Properties.Resources.Keyboard24p;
			this.btnKeyboardID.Location = new System.Drawing.Point(268, 69);
			this.btnKeyboardID.Name = "btnKeyboardID";
			this.btnKeyboardID.Size = new System.Drawing.Size(36, 31);
			this.btnKeyboardID.TabIndex = 34;
			this.btnKeyboardID.UseVisualStyleBackColor = false;
			this.btnKeyboardID.Visible = false;
			this.btnKeyboardID.Click += new System.EventHandler(this.btnKeyboardID_Click);
			// 
			// lbWarning
			// 
			this.lbWarning.AutoSize = true;
			this.lbWarning.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbWarning.ForeColor = System.Drawing.Color.Red;
			this.lbWarning.Location = new System.Drawing.Point(53, 170);
			this.lbWarning.Name = "lbWarning";
			this.lbWarning.Size = new System.Drawing.Size(126, 17);
			this.lbWarning.TabIndex = 37;
			this.lbWarning.Text = "Wrong ID or PW !!!";
			this.lbWarning.Visible = false;
			// 
			// picVisiblePwNotUse
			// 
			this.picVisiblePwNotUse.BackColor = System.Drawing.SystemColors.Window;
			this.picVisiblePwNotUse.Image = global::H_MarsSSD.Properties.Resources.VisiblePassword2_24p;
			this.picVisiblePwNotUse.Location = new System.Drawing.Point(236, 139);
			this.picVisiblePwNotUse.Name = "picVisiblePwNotUse";
			this.picVisiblePwNotUse.Size = new System.Drawing.Size(27, 23);
			this.picVisiblePwNotUse.TabIndex = 35;
			this.picVisiblePwNotUse.TabStop = false;
			this.picVisiblePwNotUse.Click += new System.EventHandler(this.picVisiblePwNotUse_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.Transparent;
			this.btnCancel.FlatAppearance.BorderSize = 0;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnCancel.Image = global::H_MarsSSD.Properties.Resources.main_close;
			this.btnCancel.Location = new System.Drawing.Point(286, 2);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(38, 38);
			this.btnCancel.TabIndex = 15;
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// cbID
			// 
			this.cbID.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
			this.cbID.FormattingEnabled = true;
			this.cbID.Location = new System.Drawing.Point(55, 69);
			this.cbID.Name = "cbID";
			this.cbID.Size = new System.Drawing.Size(211, 33);
			this.cbID.TabIndex = 38;
			// 
			// frmLogin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(330, 296);
			this.Controls.Add(this.cbID);
			this.Controls.Add(this.lbWarning);
			this.Controls.Add(this.picVisiblePwNotUse);
			this.Controls.Add(this.btnKeyboardID);
			this.Controls.Add(this.btnKeyboardPW);
			this.Controls.Add(this.btnLogin);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtPw);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmLogin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmLogin";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmLogin_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmLogin_Paint);
			((System.ComponentModel.ISupportInitialize)(this.picVisiblePwNotUse)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox txtPw;
		private System.Windows.Forms.Button btnLogin;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnKeyboardPW;
        private System.Windows.Forms.Button btnKeyboardID;
        private System.Windows.Forms.Label lbWarning;
        private System.Windows.Forms.PictureBox picVisiblePwNotUse;
		private System.Windows.Forms.ComboBox cbID;
	}
}