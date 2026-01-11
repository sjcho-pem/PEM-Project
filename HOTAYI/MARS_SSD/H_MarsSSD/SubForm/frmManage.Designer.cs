namespace H_MarsSSD
{
	partial class frmManage
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dGridMember = new System.Windows.Forms.DataGridView();
			this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.PW = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LEVEL = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LOGIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.LOGOUT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.RUNNINGTIME = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panel = new System.Windows.Forms.Panel();
			this.btnRegist = new System.Windows.Forms.Button();
			this.txtSearch = new System.Windows.Forms.TextBox();
			this.cbSort = new System.Windows.Forms.ComboBox();
			this.lbSortLevel = new System.Windows.Forms.Label();
			this.btnEdit = new System.Windows.Forms.Button();
			this.lbSearch = new System.Windows.Forms.Label();
			this.btnLevelSetting = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnCancelnSave = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dGridMember)).BeginInit();
			this.SuspendLayout();
			// 
			// dGridMember
			// 
			this.dGridMember.AllowUserToAddRows = false;
			this.dGridMember.AllowUserToDeleteRows = false;
			this.dGridMember.AllowUserToResizeColumns = false;
			this.dGridMember.AllowUserToResizeRows = false;
			this.dGridMember.BackgroundColor = System.Drawing.Color.White;
			this.dGridMember.BorderStyle = System.Windows.Forms.BorderStyle.None;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dGridMember.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dGridMember.ColumnHeadersHeight = 30;
			this.dGridMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dGridMember.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.PW,
            this.LEVEL,
            this.LOGIN,
            this.LOGOUT,
            this.RUNNINGTIME});
			this.dGridMember.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dGridMember.EnableHeadersVisualStyles = false;
			this.dGridMember.Location = new System.Drawing.Point(14, 60);
			this.dGridMember.MultiSelect = false;
			this.dGridMember.Name = "dGridMember";
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dGridMember.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
			this.dGridMember.RowHeadersVisible = false;
			this.dGridMember.RowHeadersWidth = 45;
			this.dGridMember.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
			this.dGridMember.RowsDefaultCellStyle = dataGridViewCellStyle9;
			this.dGridMember.RowTemplate.Height = 25;
			this.dGridMember.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dGridMember.Size = new System.Drawing.Size(1013, 528);
			this.dGridMember.TabIndex = 0;
			this.dGridMember.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGridMember_CellClick);
			this.dGridMember.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dGridMember_KeyPress);
			// 
			// ID
			// 
			this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.ID.DefaultCellStyle = dataGridViewCellStyle2;
			this.ID.Frozen = true;
			this.ID.HeaderText = "ID";
			this.ID.Name = "ID";
			this.ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ID.Width = 150;
			// 
			// PW
			// 
			this.PW.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.PW.DefaultCellStyle = dataGridViewCellStyle3;
			this.PW.Frozen = true;
			this.PW.HeaderText = "PW";
			this.PW.Name = "PW";
			this.PW.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.PW.Width = 150;
			// 
			// LEVEL
			// 
			this.LEVEL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.LEVEL.DefaultCellStyle = dataGridViewCellStyle4;
			this.LEVEL.Frozen = true;
			this.LEVEL.HeaderText = "LEVEL";
			this.LEVEL.Name = "LEVEL";
			this.LEVEL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.LEVEL.Width = 150;
			// 
			// LOGIN
			// 
			this.LOGIN.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.LOGIN.DefaultCellStyle = dataGridViewCellStyle5;
			this.LOGIN.HeaderText = "LOG IN";
			this.LOGIN.Name = "LOGIN";
			this.LOGIN.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.LOGIN.Width = 180;
			// 
			// LOGOUT
			// 
			this.LOGOUT.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.LOGOUT.DefaultCellStyle = dataGridViewCellStyle6;
			this.LOGOUT.HeaderText = "LOG OUT";
			this.LOGOUT.Name = "LOGOUT";
			this.LOGOUT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.LOGOUT.Width = 180;
			// 
			// RUNNINGTIME
			// 
			this.RUNNINGTIME.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.RUNNINGTIME.DefaultCellStyle = dataGridViewCellStyle7;
			this.RUNNINGTIME.HeaderText = "RUNNING TIME";
			this.RUNNINGTIME.Name = "RUNNINGTIME";
			this.RUNNINGTIME.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.RUNNINGTIME.Width = 180;
			// 
			// panel
			// 
			this.panel.BackColor = System.Drawing.SystemColors.WindowText;
			this.panel.Location = new System.Drawing.Point(1, 45);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(1041, 2);
			this.panel.TabIndex = 10;
			// 
			// btnRegist
			// 
			this.btnRegist.BackColor = System.Drawing.Color.AliceBlue;
			this.btnRegist.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
			this.btnRegist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnRegist.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnRegist.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnRegist.Image = global::H_MarsSSD.Properties.Resources.Add_26p;
			this.btnRegist.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnRegist.Location = new System.Drawing.Point(13, 8);
			this.btnRegist.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnRegist.Name = "btnRegist";
			this.btnRegist.Size = new System.Drawing.Size(93, 30);
			this.btnRegist.TabIndex = 5;
			this.btnRegist.Text = "REGIST";
			this.btnRegist.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnRegist.UseVisualStyleBackColor = false;
			this.btnRegist.Click += new System.EventHandler(this.btnRegist_Click);
			// 
			// txtSearch
			// 
			this.txtSearch.BackColor = System.Drawing.SystemColors.Window;
			this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtSearch.Location = new System.Drawing.Point(825, 10);
			this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.txtSearch.Name = "txtSearch";
			this.txtSearch.Size = new System.Drawing.Size(150, 27);
			this.txtSearch.TabIndex = 13;
			this.txtSearch.WordWrap = false;
			this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged_1);
			this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
			// 
			// cbSort
			// 
			this.cbSort.BackColor = System.Drawing.SystemColors.Window;
			this.cbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cbSort.Font = new System.Drawing.Font("Segoe UI", 10.8F);
			this.cbSort.FormattingEnabled = true;
			this.cbSort.Items.AddRange(new object[] {
            "ALL",
            "OPERATOR",
            "ENGINEER",
            "PROFESSIONAL"});
			this.cbSort.Location = new System.Drawing.Point(583, 10);
			this.cbSort.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.cbSort.Name = "cbSort";
			this.cbSort.Size = new System.Drawing.Size(138, 27);
			this.cbSort.TabIndex = 14;
			this.cbSort.SelectedIndexChanged += new System.EventHandler(this.cbSort_SelectedIndexChanged);
			// 
			// lbSortLevel
			// 
			this.lbSortLevel.AutoSize = true;
			this.lbSortLevel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbSortLevel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lbSortLevel.Location = new System.Drawing.Point(482, 12);
			this.lbSortLevel.Name = "lbSortLevel";
			this.lbSortLevel.Size = new System.Drawing.Size(99, 21);
			this.lbSortLevel.TabIndex = 15;
			this.lbSortLevel.Text = "SORT LEVEL";
			// 
			// btnEdit
			// 
			this.btnEdit.BackColor = System.Drawing.Color.AliceBlue;
			this.btnEdit.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
			this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnEdit.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnEdit.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnEdit.Image = global::H_MarsSSD.Properties.Resources.Edit_26p;
			this.btnEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnEdit.Location = new System.Drawing.Point(119, 8);
			this.btnEdit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnEdit.Name = "btnEdit";
			this.btnEdit.Size = new System.Drawing.Size(81, 30);
			this.btnEdit.TabIndex = 16;
			this.btnEdit.Text = "EDIT";
			this.btnEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnEdit.UseVisualStyleBackColor = false;
			this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
			// 
			// lbSearch
			// 
			this.lbSearch.AutoSize = true;
			this.lbSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbSearch.ForeColor = System.Drawing.SystemColors.ControlText;
			this.lbSearch.Location = new System.Drawing.Point(730, 13);
			this.lbSearch.Name = "lbSearch";
			this.lbSearch.Size = new System.Drawing.Size(92, 21);
			this.lbSearch.TabIndex = 17;
			this.lbSearch.Text = "SEARCH ID";
			// 
			// btnLevelSetting
			// 
			this.btnLevelSetting.BackColor = System.Drawing.Color.AliceBlue;
			this.btnLevelSetting.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
			this.btnLevelSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnLevelSetting.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnLevelSetting.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnLevelSetting.Image = global::H_MarsSSD.Properties.Resources.Member_26p;
			this.btnLevelSetting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnLevelSetting.Location = new System.Drawing.Point(315, 8);
			this.btnLevelSetting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnLevelSetting.Name = "btnLevelSetting";
			this.btnLevelSetting.Size = new System.Drawing.Size(149, 30);
			this.btnLevelSetting.TabIndex = 18;
			this.btnLevelSetting.Text = "LEVEL SETTING";
			this.btnLevelSetting.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnLevelSetting.UseVisualStyleBackColor = false;
			this.btnLevelSetting.Click += new System.EventHandler(this.btnLevelSetting_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.BackColor = System.Drawing.Color.AliceBlue;
			this.btnDelete.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
			this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText;
			this.btnDelete.Image = global::H_MarsSSD.Properties.Resources.Delete_24p;
			this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnDelete.Location = new System.Drawing.Point(212, 8);
			this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(90, 30);
			this.btnDelete.TabIndex = 19;
			this.btnDelete.Text = "DELETE";
			this.btnDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.btnDelete.UseVisualStyleBackColor = false;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnCancelnSave
			// 
			this.btnCancelnSave.BackColor = System.Drawing.Color.Transparent;
			this.btnCancelnSave.FlatAppearance.BorderSize = 0;
			this.btnCancelnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.btnCancelnSave.Image = global::H_MarsSSD.Properties.Resources.main_close;
			this.btnCancelnSave.Location = new System.Drawing.Point(993, 5);
			this.btnCancelnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.btnCancelnSave.Name = "btnCancelnSave";
			this.btnCancelnSave.Size = new System.Drawing.Size(38, 38);
			this.btnCancelnSave.TabIndex = 9;
			this.btnCancelnSave.UseVisualStyleBackColor = false;
			this.btnCancelnSave.Click += new System.EventHandler(this.btnCancelnSave_Click);
			// 
			// frmManage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Gainsboro;
			this.ClientSize = new System.Drawing.Size(1043, 602);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.btnLevelSetting);
			this.Controls.Add(this.lbSearch);
			this.Controls.Add(this.btnEdit);
			this.Controls.Add(this.lbSortLevel);
			this.Controls.Add(this.cbSort);
			this.Controls.Add(this.txtSearch);
			this.Controls.Add(this.btnRegist);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.btnCancelnSave);
			this.Controls.Add(this.dGridMember);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmManage";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "frmManage";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmManage_Paint);
			((System.ComponentModel.ISupportInitialize)(this.dGridMember)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dGridMember;
		private System.Windows.Forms.Button btnCancelnSave;
		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Button btnRegist;
		private System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.ComboBox cbSort;
		private System.Windows.Forms.Label lbSortLevel;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Label lbSearch;
		private System.Windows.Forms.Button btnLevelSetting;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PW;
        private System.Windows.Forms.DataGridViewTextBoxColumn LEVEL;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOGIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOGOUT;
        private System.Windows.Forms.DataGridViewTextBoxColumn RUNNINGTIME;
    }
}