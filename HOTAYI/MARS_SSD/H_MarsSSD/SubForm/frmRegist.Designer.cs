namespace H_MarsSSD
{
	partial class frmRegist
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
			this.txtRegId = new System.Windows.Forms.TextBox();
			this.txtRegPw = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.cbRegLevel = new System.Windows.Forms.ComboBox();
			this.txtCheckPw = new System.Windows.Forms.TextBox();
			this.lbPw = new System.Windows.Forms.Label();
			this.lbCheckPw = new System.Windows.Forms.Label();
			this.lbLevel = new System.Windows.Forms.Label();
			this.lbId = new System.Windows.Forms.Label();
			this.btnKeyboardPw1 = new System.Windows.Forms.Button();
			this.btnKeyboardPw2 = new System.Windows.Forms.Button();
			this.btnKeyboardID = new System.Windows.Forms.Button();
			this.picVisiblePwNotUse2 = new System.Windows.Forms.PictureBox();
			this.picVisiblePwNotUse1 = new System.Windows.Forms.PictureBox();
			this.btnCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picVisiblePwNotUse2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picVisiblePwNotUse1)).BeginInit();
			this.SuspendLayout();
			// 
			// txtRegId
			// 
			this.txtRegId.BackColor = System.Drawing.Color.White;
			this.txtRegId.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.txtRegId.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.txtRegId.Location = new System.Drawing.Point(50, 76);
			this.txtRegId.Name = "txtRegId";
			this.txtRegId.Size = new System.Drawing.Size(230, 27);
			this.txtRegId.TabIndex = 27;
			// 
			// txtRegPw
			// 
			this.txtRegPw.BackColor = System.Drawing.Color.White;
			this.txtRegPw.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtRegPw.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.txtRegPw.Location = new System.Drawing.Point(50, 141);
			this.txtRegPw.Name = "txtRegPw";
			this.txtRegPw.Size = new System.Drawing.Size(230, 27);
			this.txtRegPw.TabIndex = 28;
			// 
			// btnAdd
			// 
			this.btnAdd.BackColor = System.Drawing.Color.AliceBlue;
			this.btnAdd.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
			this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnAdd.ForeColor = System.Drawing.Color.Black;
			this.btnAdd.Location = new System.Drawing.Point(112, 336);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(109, 46);
			this.btnAdd.TabIndex = 31;
			this.btnAdd.Text = "ADD";
			this.btnAdd.UseVisualStyleBackColor = false;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// cbRegLevel
			// 
			this.cbRegLevel.BackColor = System.Drawing.Color.White;
			this.cbRegLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbRegLevel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cbRegLevel.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.cbRegLevel.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.cbRegLevel.FormattingEnabled = true;
			this.cbRegLevel.Items.AddRange(new object[] {
            "OPERATOR",
            "ENGINEER",
            "PROFESSIONAL"});
			this.cbRegLevel.Location = new System.Drawing.Point(50, 274);
			this.cbRegLevel.Name = "cbRegLevel";
			this.cbRegLevel.Size = new System.Drawing.Size(230, 28);
			this.cbRegLevel.TabIndex = 30;
			// 
			// txtCheckPw
			// 
			this.txtCheckPw.BackColor = System.Drawing.Color.White;
			this.txtCheckPw.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtCheckPw.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.txtCheckPw.Location = new System.Drawing.Point(50, 208);
			this.txtCheckPw.Name = "txtCheckPw";
			this.txtCheckPw.Size = new System.Drawing.Size(230, 27);
			this.txtCheckPw.TabIndex = 29;
			// 
			// lbPw
			// 
			this.lbPw.AutoSize = true;
			this.lbPw.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.lbPw.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lbPw.Location = new System.Drawing.Point(46, 119);
			this.lbPw.Name = "lbPw";
			this.lbPw.Size = new System.Drawing.Size(87, 20);
			this.lbPw.TabIndex = 35;
			this.lbPw.Text = "PASSWORD";
			// 
			// lbCheckPw
			// 
			this.lbCheckPw.AutoSize = true;
			this.lbCheckPw.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.lbCheckPw.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lbCheckPw.Location = new System.Drawing.Point(46, 184);
			this.lbCheckPw.Name = "lbCheckPw";
			this.lbCheckPw.Size = new System.Drawing.Size(155, 20);
			this.lbCheckPw.TabIndex = 36;
			this.lbCheckPw.Text = "CONFIRM PASSWORD";
			// 
			// lbLevel
			// 
			this.lbLevel.AutoSize = true;
			this.lbLevel.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.lbLevel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lbLevel.Location = new System.Drawing.Point(46, 251);
			this.lbLevel.Name = "lbLevel";
			this.lbLevel.Size = new System.Drawing.Size(48, 20);
			this.lbLevel.TabIndex = 37;
			this.lbLevel.Text = "LEVEL";
			// 
			// lbId
			// 
			this.lbId.AutoSize = true;
			this.lbId.Font = new System.Drawing.Font("Segoe UI", 11.25F);
			this.lbId.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lbId.Location = new System.Drawing.Point(46, 53);
			this.lbId.Name = "lbId";
			this.lbId.Size = new System.Drawing.Size(24, 20);
			this.lbId.TabIndex = 38;
			this.lbId.Text = "ID";
			// 
			// btnKeyboardPw1
			// 
			this.btnKeyboardPw1.BackColor = System.Drawing.Color.Gainsboro;
			this.btnKeyboardPw1.FlatAppearance.BorderSize = 0;
			this.btnKeyboardPw1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKeyboardPw1.Image = global::H_MarsSSD.Properties.Resources.Keyboard24p;
			this.btnKeyboardPw1.Location = new System.Drawing.Point(283, 138);
			this.btnKeyboardPw1.Name = "btnKeyboardPw1";
			this.btnKeyboardPw1.Size = new System.Drawing.Size(36, 31);
			this.btnKeyboardPw1.TabIndex = 44;
			this.btnKeyboardPw1.UseVisualStyleBackColor = false;
			this.btnKeyboardPw1.Click += new System.EventHandler(this.btnKeyboard_Click);
			// 
			// btnKeyboardPw2
			// 
			this.btnKeyboardPw2.BackColor = System.Drawing.Color.Gainsboro;
			this.btnKeyboardPw2.FlatAppearance.BorderSize = 0;
			this.btnKeyboardPw2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKeyboardPw2.Image = global::H_MarsSSD.Properties.Resources.Keyboard24p;
			this.btnKeyboardPw2.Location = new System.Drawing.Point(283, 205);
			this.btnKeyboardPw2.Name = "btnKeyboardPw2";
			this.btnKeyboardPw2.Size = new System.Drawing.Size(36, 31);
			this.btnKeyboardPw2.TabIndex = 45;
			this.btnKeyboardPw2.UseVisualStyleBackColor = false;
			this.btnKeyboardPw2.Click += new System.EventHandler(this.btnKeyboard_Click);
			// 
			// btnKeyboardID
			// 
			this.btnKeyboardID.BackColor = System.Drawing.Color.Gainsboro;
			this.btnKeyboardID.FlatAppearance.BorderSize = 0;
			this.btnKeyboardID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKeyboardID.Image = global::H_MarsSSD.Properties.Resources.Keyboard24p;
			this.btnKeyboardID.Location = new System.Drawing.Point(283, 73);
			this.btnKeyboardID.Name = "btnKeyboardID";
			this.btnKeyboardID.Size = new System.Drawing.Size(36, 31);
			this.btnKeyboardID.TabIndex = 43;
			this.btnKeyboardID.UseVisualStyleBackColor = false;
			this.btnKeyboardID.Click += new System.EventHandler(this.btnKeyboard_Click);
			// 
			// picVisiblePwNotUse2
			// 
			this.picVisiblePwNotUse2.BackColor = System.Drawing.SystemColors.Window;
			this.picVisiblePwNotUse2.Image = global::H_MarsSSD.Properties.Resources.VisiblePassword2_24p;
			this.picVisiblePwNotUse2.Location = new System.Drawing.Point(250, 210);
			this.picVisiblePwNotUse2.Name = "picVisiblePwNotUse2";
			this.picVisiblePwNotUse2.Size = new System.Drawing.Size(27, 23);
			this.picVisiblePwNotUse2.TabIndex = 42;
			this.picVisiblePwNotUse2.TabStop = false;
			this.picVisiblePwNotUse2.Click += new System.EventHandler(this.picVisiblePw_Click);
			// 
			// picVisiblePwNotUse1
			// 
			this.picVisiblePwNotUse1.BackColor = System.Drawing.SystemColors.Window;
			this.picVisiblePwNotUse1.Image = global::H_MarsSSD.Properties.Resources.VisiblePassword2_24p;
			this.picVisiblePwNotUse1.Location = new System.Drawing.Point(250, 143);
			this.picVisiblePwNotUse1.Name = "picVisiblePwNotUse1";
			this.picVisiblePwNotUse1.Size = new System.Drawing.Size(27, 23);
			this.picVisiblePwNotUse1.TabIndex = 40;
			this.picVisiblePwNotUse1.TabStop = false;
			this.picVisiblePwNotUse1.Click += new System.EventHandler(this.picVisiblePw_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.Transparent;
			this.btnCancel.FlatAppearance.BorderSize = 0;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Image = global::H_MarsSSD.Properties.Resources.main_close;
			this.btnCancel.Location = new System.Drawing.Point(295, 5);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(38, 38);
			this.btnCancel.TabIndex = 10;
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmRegist
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(337, 417);
			this.Controls.Add(this.btnKeyboardPw2);
			this.Controls.Add(this.btnKeyboardPw1);
			this.Controls.Add(this.btnKeyboardID);
			this.Controls.Add(this.picVisiblePwNotUse2);
			this.Controls.Add(this.picVisiblePwNotUse1);
			this.Controls.Add(this.lbId);
			this.Controls.Add(this.lbLevel);
			this.Controls.Add(this.lbCheckPw);
			this.Controls.Add(this.lbPw);
			this.Controls.Add(this.txtCheckPw);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.cbRegLevel);
			this.Controls.Add(this.btnAdd);
			this.Controls.Add(this.txtRegPw);
			this.Controls.Add(this.txtRegId);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "frmRegist";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmRegist";
			this.Load += new System.EventHandler(this.frmRegist_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmRegist_Paint);
			((System.ComponentModel.ISupportInitialize)(this.picVisiblePwNotUse2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picVisiblePwNotUse1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.TextBox txtRegId;
		private System.Windows.Forms.TextBox txtRegPw;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ComboBox cbRegLevel;
		private System.Windows.Forms.TextBox txtCheckPw;
		private System.Windows.Forms.Label lbPw;
		private System.Windows.Forms.Label lbCheckPw;
		private System.Windows.Forms.Label lbLevel;
		private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.PictureBox picVisiblePwNotUse1;
        private System.Windows.Forms.PictureBox picVisiblePwNotUse2;
        private System.Windows.Forms.Button btnKeyboardID;
        private System.Windows.Forms.Button btnKeyboardPw1;
        private System.Windows.Forms.Button btnKeyboardPw2;
    }
}