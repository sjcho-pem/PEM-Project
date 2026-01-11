namespace H_MarsSSD
{
	partial class frmRecipeManager
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgvInfo = new System.Windows.Forms.DataGridView();
			this.btnClose = new System.Windows.Forms.Button();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btnAdd = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lbAddError = new System.Windows.Forms.Label();
			this.cbRecipe = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.tbDeviceID = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.chkModify = new System.Windows.Forms.CheckBox();
			this.btnDel = new System.Windows.Forms.Button();
			this.ColNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColDeviceID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ColRecipeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// dgvInfo
			// 
			this.dgvInfo.AllowUserToAddRows = false;
			this.dgvInfo.AllowUserToDeleteRows = false;
			this.dgvInfo.AllowUserToResizeColumns = false;
			this.dgvInfo.AllowUserToResizeRows = false;
			this.dgvInfo.BackgroundColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvInfo.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgvInfo.ColumnHeadersHeight = 30;
			this.dgvInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgvInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColNo,
            this.ColDeviceID,
            this.ColRecipeName});
			this.dgvInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvInfo.Location = new System.Drawing.Point(3, 49);
			this.dgvInfo.MultiSelect = false;
			this.dgvInfo.Name = "dgvInfo";
			this.dgvInfo.ReadOnly = true;
			this.dgvInfo.RowHeadersVisible = false;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.dgvInfo.RowsDefaultCellStyle = dataGridViewCellStyle3;
			this.dgvInfo.RowTemplate.Height = 30;
			this.dgvInfo.RowTemplate.ReadOnly = true;
			this.dgvInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvInfo.Size = new System.Drawing.Size(608, 414);
			this.dgvInfo.TabIndex = 2;
			this.dgvInfo.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInfo_CellDoubleClick);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnClose.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.btnClose.Location = new System.Drawing.Point(485, 2);
			this.btnClose.Margin = new System.Windows.Forms.Padding(1, 2, 1, 0);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(122, 44);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "CLOSE";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 5;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel2.Controls.Add(this.btnClose, 4, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 535);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(608, 46);
			this.tableLayoutPanel2.TabIndex = 3;
			// 
			// btnAdd
			// 
			this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnAdd.Enabled = false;
			this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.btnAdd.Location = new System.Drawing.Point(482, 2);
			this.btnAdd.Margin = new System.Windows.Forms.Padding(1, 2, 1, 0);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(116, 43);
			this.btnAdd.TabIndex = 5;
			this.btnAdd.Text = "ADD";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.lbAddError);
			this.panel1.Controls.Add(this.btnAdd);
			this.panel1.Controls.Add(this.cbRecipe);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.tbDeviceID);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 463);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(608, 72);
			this.panel1.TabIndex = 4;
			// 
			// lbAddError
			// 
			this.lbAddError.Font = new System.Drawing.Font("맑은 고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.lbAddError.ForeColor = System.Drawing.Color.Red;
			this.lbAddError.Location = new System.Drawing.Point(10, 40);
			this.lbAddError.Name = "lbAddError";
			this.lbAddError.Size = new System.Drawing.Size(468, 25);
			this.lbAddError.TabIndex = 6;
			// 
			// cbRecipe
			// 
			this.cbRecipe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbRecipe.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
			this.cbRecipe.FormattingEnabled = true;
			this.cbRecipe.ItemHeight = 17;
			this.cbRecipe.Location = new System.Drawing.Point(305, 11);
			this.cbRecipe.Name = "cbRecipe";
			this.cbRecipe.Size = new System.Drawing.Size(160, 25);
			this.cbRecipe.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(251, 15);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 17);
			this.label2.TabIndex = 2;
			this.label2.Text = "Recipe";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(10, 15);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 17);
			this.label1.TabIndex = 1;
			this.label1.Text = "Device ID";
			// 
			// tbDeviceID
			// 
			this.tbDeviceID.BackColor = System.Drawing.Color.White;
			this.tbDeviceID.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold);
			this.tbDeviceID.Location = new System.Drawing.Point(75, 10);
			this.tbDeviceID.Name = "tbDeviceID";
			this.tbDeviceID.Size = new System.Drawing.Size(160, 25);
			this.tbDeviceID.TabIndex = 0;
			this.tbDeviceID.Click += new System.EventHandler(this.tbDeviceID_Click);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 5;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel1.Controls.Add(this.chkModify, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.btnDel, 4, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(608, 46);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// chkModify
			// 
			this.chkModify.BackColor = System.Drawing.Color.Transparent;
			this.chkModify.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkModify.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkModify.Location = new System.Drawing.Point(3, 3);
			this.chkModify.Name = "chkModify";
			this.chkModify.Size = new System.Drawing.Size(115, 40);
			this.chkModify.TabIndex = 6;
			this.chkModify.Text = "MODIFY";
			this.chkModify.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chkModify.UseVisualStyleBackColor = false;
			this.chkModify.CheckedChanged += new System.EventHandler(this.chkModify_CheckedChanged);
			// 
			// btnDel
			// 
			this.btnDel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnDel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnDel.Enabled = false;
			this.btnDel.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.btnDel.Location = new System.Drawing.Point(485, 0);
			this.btnDel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 2);
			this.btnDel.Name = "btnDel";
			this.btnDel.Size = new System.Drawing.Size(122, 44);
			this.btnDel.TabIndex = 3;
			this.btnDel.Text = "DELETE";
			this.btnDel.UseVisualStyleBackColor = true;
			this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
			// 
			// ColNo
			// 
			this.ColNo.HeaderText = "No.";
			this.ColNo.Name = "ColNo";
			this.ColNo.ReadOnly = true;
			this.ColNo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ColNo.Width = 50;
			// 
			// ColDeviceID
			// 
			this.ColDeviceID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.ColDeviceID.HeaderText = "Device ID";
			this.ColDeviceID.Name = "ColDeviceID";
			this.ColDeviceID.ReadOnly = true;
			// 
			// ColRecipeName
			// 
			this.ColRecipeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
			this.ColRecipeName.DefaultCellStyle = dataGridViewCellStyle2;
			this.ColRecipeName.HeaderText = "Recipe";
			this.ColRecipeName.Name = "ColRecipeName";
			this.ColRecipeName.ReadOnly = true;
			// 
			// frmRecipeManager
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(614, 584);
			this.ControlBox = false;
			this.Controls.Add(this.dgvInfo);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.tableLayoutPanel2);
			this.MinimumSize = new System.Drawing.Size(630, 600);
			this.Name = "frmRecipeManager";
			this.Padding = new System.Windows.Forms.Padding(3);
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Recipe Manager";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmRecipeManager_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvInfo;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbDeviceID;
		private System.Windows.Forms.ComboBox cbRecipe;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lbAddError;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.CheckBox chkModify;
		private System.Windows.Forms.Button btnDel;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColNo;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColDeviceID;
		private System.Windows.Forms.DataGridViewTextBoxColumn ColRecipeName;
	}
}