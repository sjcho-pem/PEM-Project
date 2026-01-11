namespace H_MarsSSD
{
	partial class frmEdit
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
			this.cbEditLevel = new System.Windows.Forms.ComboBox();
			this.btnEdit = new System.Windows.Forms.Button();
			this.txtEditPw = new System.Windows.Forms.TextBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.lbPw = new System.Windows.Forms.Label();
			this.lbLevel = new System.Windows.Forms.Label();
			this.lbId = new System.Windows.Forms.Label();
			this.picVisiblePwNotUse = new System.Windows.Forms.PictureBox();
			this.btnKeyboardPw = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.picVisiblePwNotUse)).BeginInit();
			this.SuspendLayout();
			// 
			// cbEditLevel
			// 
			this.cbEditLevel.BackColor = System.Drawing.Color.White;
			this.cbEditLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbEditLevel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cbEditLevel.Font = new System.Drawing.Font("Segoe UI", 11F);
			this.cbEditLevel.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.cbEditLevel.FormattingEnabled = true;
			this.cbEditLevel.Items.AddRange(new object[] {
            "OPERATOR",
            "ENGINEER",
            "PROFESSIONAL"});
			this.cbEditLevel.Location = new System.Drawing.Point(44, 182);
			this.cbEditLevel.Name = "cbEditLevel";
			this.cbEditLevel.Size = new System.Drawing.Size(236, 28);
			this.cbEditLevel.TabIndex = 36;
			// 
			// btnEdit
			// 
			this.btnEdit.BackColor = System.Drawing.Color.AliceBlue;
			this.btnEdit.FlatAppearance.BorderColor = System.Drawing.SystemColors.WindowFrame;
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.btnEdit.ForeColor = System.Drawing.Color.Black;
			this.btnEdit.Location = new System.Drawing.Point(108, 241);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(109, 46);
			this.btnEdit.TabIndex = 37;
			this.btnEdit.Text = "EDIT";
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// txtEditPw
			// 
			this.txtEditPw.BackColor = System.Drawing.Color.White;
			this.txtEditPw.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtEditPw.ForeColor = System.Drawing.SystemColors.WindowFrame;
			this.txtEditPw.Location = new System.Drawing.Point(44, 114);
			this.txtEditPw.Name = "txtEditPw";
			this.txtEditPw.Size = new System.Drawing.Size(236, 27);
			this.txtEditPw.TabIndex = 35;
			// 
			// btnCancel
			// 
			this.btnCancel.BackColor = System.Drawing.Color.Transparent;
			this.btnCancel.FlatAppearance.BorderSize = 0;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancel.Image = global::H_MarsSSD.Properties.Resources.main_close;
			this.btnCancel.Location = new System.Drawing.Point(296, 4);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(38, 38);
			this.btnCancel.TabIndex = 33;
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(41, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(24, 20);
			this.label4.TabIndex = 42;
			this.label4.Text = "ID";
			// 
			// lbPw
			// 
			this.lbPw.AutoSize = true;
			this.lbPw.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbPw.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lbPw.Location = new System.Drawing.Point(40, 91);
			this.lbPw.Name = "lbPw";
			this.lbPw.Size = new System.Drawing.Size(87, 20);
			this.lbPw.TabIndex = 43;
			this.lbPw.Text = "PASSWORD";
			// 
			// lbLevel
			// 
			this.lbLevel.AutoSize = true;
			this.lbLevel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbLevel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lbLevel.Location = new System.Drawing.Point(40, 159);
			this.lbLevel.Name = "lbLevel";
			this.lbLevel.Size = new System.Drawing.Size(48, 20);
			this.lbLevel.TabIndex = 44;
			this.lbLevel.Text = "LEVEL";
			// 
			// lbId
			// 
			this.lbId.AutoSize = true;
			this.lbId.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbId.Location = new System.Drawing.Point(71, 52);
			this.lbId.Name = "lbId";
			this.lbId.Size = new System.Drawing.Size(25, 20);
			this.lbId.TabIndex = 48;
			this.lbId.Text = "ID";
			// 
			// picVisiblePwNotUse
			// 
			this.picVisiblePwNotUse.BackColor = System.Drawing.SystemColors.Window;
			this.picVisiblePwNotUse.Image = global::H_MarsSSD.Properties.Resources.VisiblePassword2_24p;
			this.picVisiblePwNotUse.Location = new System.Drawing.Point(250, 116);
			this.picVisiblePwNotUse.Name = "picVisiblePwNotUse";
			this.picVisiblePwNotUse.Size = new System.Drawing.Size(27, 23);
			this.picVisiblePwNotUse.TabIndex = 50;
			this.picVisiblePwNotUse.TabStop = false;
			this.picVisiblePwNotUse.Click += new System.EventHandler(this.picVisiblePwNotUse_Click);
			// 
			// btnKeyboardPw
			// 
			this.btnKeyboardPw.BackColor = System.Drawing.Color.Gainsboro;
			this.btnKeyboardPw.FlatAppearance.BorderSize = 0;
			this.btnKeyboardPw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnKeyboardPw.Image = global::H_MarsSSD.Properties.Resources.Keyboard24p;
			this.btnKeyboardPw.Location = new System.Drawing.Point(280, 112);
			this.btnKeyboardPw.Name = "btnKeyboardPw";
			this.btnKeyboardPw.Size = new System.Drawing.Size(36, 31);
			this.btnKeyboardPw.TabIndex = 51;
			this.btnKeyboardPw.UseVisualStyleBackColor = false;
			this.btnKeyboardPw.Click += new System.EventHandler(this.btnKeyboardPw_Click);
			// 
			// frmEdit
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(337, 317);
			this.Controls.Add(this.btnKeyboardPw);
			this.Controls.Add(this.picVisiblePwNotUse);
			this.Controls.Add(this.lbId);
			this.Controls.Add(this.lbLevel);
			this.Controls.Add(this.lbPw);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.cbEditLevel);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.txtEditPw);
			this.Controls.Add(this.btnCancel);
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmEdit";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmEdit_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmEdit_Paint);
			((System.ComponentModel.ISupportInitialize)(this.picVisiblePwNotUse)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox cbEditLevel;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.TextBox txtEditPw;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lbPw;
		private System.Windows.Forms.Label lbLevel;
		private System.Windows.Forms.Label lbId;
        private System.Windows.Forms.PictureBox picVisiblePwNotUse;
        private System.Windows.Forms.Button btnKeyboardPw;
    }
}