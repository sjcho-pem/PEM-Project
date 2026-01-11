namespace H_MarsSSD
{
	partial class frmProductEditPopup
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
			this.btnCopy = new System.Windows.Forms.Button();
			this.btnPaste = new System.Windows.Forms.Button();
			this.btnClearAll = new System.Windows.Forms.Button();
			this.btnSaveAll = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.btnClear = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.dgvProdInfo = new System.Windows.Forms.DataGridView();
			this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tmrSSDEditorUpdate = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.tlpButton.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvProdInfo)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
			this.tableLayoutPanel1.Controls.Add(this.tlpButton, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.dgvProdInfo, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(584, 561);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tlpButton
			// 
			this.tlpButton.ColumnCount = 4;
			this.tableLayoutPanel1.SetColumnSpan(this.tlpButton, 2);
			this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tlpButton.Controls.Add(this.btnCopy, 2, 0);
			this.tlpButton.Controls.Add(this.btnPaste, 2, 1);
			this.tlpButton.Controls.Add(this.btnClearAll, 1, 1);
			this.tlpButton.Controls.Add(this.btnSaveAll, 1, 0);
			this.tlpButton.Controls.Add(this.btnClose, 3, 0);
			this.tlpButton.Controls.Add(this.btnClear, 0, 1);
			this.tlpButton.Controls.Add(this.btnSave, 0, 0);
			this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpButton.Location = new System.Drawing.Point(4, 423);
			this.tlpButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.tlpButton.Name = "tlpButton";
			this.tlpButton.RowCount = 2;
			this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tlpButton.Size = new System.Drawing.Size(576, 135);
			this.tlpButton.TabIndex = 1;
			// 
			// btnCopy
			// 
			this.btnCopy.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnCopy.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
			this.btnCopy.Location = new System.Drawing.Point(292, 3);
			this.btnCopy.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btnCopy.Name = "btnCopy";
			this.btnCopy.Size = new System.Drawing.Size(136, 61);
			this.btnCopy.TabIndex = 6;
			this.btnCopy.Text = "COPY";
			this.btnCopy.UseVisualStyleBackColor = true;
			this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
			// 
			// btnPaste
			// 
			this.btnPaste.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnPaste.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
			this.btnPaste.Location = new System.Drawing.Point(292, 70);
			this.btnPaste.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btnPaste.Name = "btnPaste";
			this.btnPaste.Size = new System.Drawing.Size(136, 62);
			this.btnPaste.TabIndex = 5;
			this.btnPaste.Text = "PASTE";
			this.btnPaste.UseVisualStyleBackColor = true;
			this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
			// 
			// btnClearAll
			// 
			this.btnClearAll.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnClearAll.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
			this.btnClearAll.Location = new System.Drawing.Point(148, 70);
			this.btnClearAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btnClearAll.Name = "btnClearAll";
			this.btnClearAll.Size = new System.Drawing.Size(136, 62);
			this.btnClearAll.TabIndex = 4;
			this.btnClearAll.Text = "CLEAR ALL";
			this.btnClearAll.UseVisualStyleBackColor = true;
			this.btnClearAll.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnSaveAll
			// 
			this.btnSaveAll.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSaveAll.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
			this.btnSaveAll.Location = new System.Drawing.Point(148, 3);
			this.btnSaveAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btnSaveAll.Name = "btnSaveAll";
			this.btnSaveAll.Size = new System.Drawing.Size(136, 61);
			this.btnSaveAll.TabIndex = 3;
			this.btnSaveAll.Text = "SAVE ALL";
			this.btnSaveAll.UseVisualStyleBackColor = true;
			this.btnSaveAll.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnClose.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
			this.btnClose.Location = new System.Drawing.Point(436, 3);
			this.btnClose.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btnClose.Name = "btnClose";
			this.tlpButton.SetRowSpan(this.btnClose, 2);
			this.btnClose.Size = new System.Drawing.Size(136, 129);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "CLOSE";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// btnClear
			// 
			this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnClear.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
			this.btnClear.Location = new System.Drawing.Point(4, 70);
			this.btnClear.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btnClear.Name = "btnClear";
			this.btnClear.Size = new System.Drawing.Size(136, 62);
			this.btnClear.TabIndex = 1;
			this.btnClear.Text = "CLEAR";
			this.btnClear.UseVisualStyleBackColor = true;
			this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// btnSave
			// 
			this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
			this.btnSave.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
			this.btnSave.Location = new System.Drawing.Point(4, 3);
			this.btnSave.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(136, 61);
			this.btnSave.TabIndex = 0;
			this.btnSave.Text = "SAVE";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// dgvProdInfo
			// 
			this.dgvProdInfo.AllowUserToAddRows = false;
			this.dgvProdInfo.AllowUserToDeleteRows = false;
			this.dgvProdInfo.AllowUserToResizeColumns = false;
			this.dgvProdInfo.AllowUserToResizeRows = false;
			this.dgvProdInfo.BackgroundColor = System.Drawing.Color.White;
			this.dgvProdInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvProdInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4});
			this.tableLayoutPanel1.SetColumnSpan(this.dgvProdInfo, 2);
			this.dgvProdInfo.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvProdInfo.Location = new System.Drawing.Point(3, 3);
			this.dgvProdInfo.Name = "dgvProdInfo";
			this.dgvProdInfo.ReadOnly = true;
			this.dgvProdInfo.RowHeadersVisible = false;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.dgvProdInfo.RowsDefaultCellStyle = dataGridViewCellStyle4;
			this.dgvProdInfo.RowTemplate.Height = 23;
			this.dgvProdInfo.Size = new System.Drawing.Size(578, 414);
			this.dgvProdInfo.TabIndex = 2;
			this.dgvProdInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProdInfo_CellClick);
			// 
			// Column1
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			this.Column1.DefaultCellStyle = dataGridViewCellStyle1;
			this.Column1.HeaderText = "No";
			this.Column1.Name = "Column1";
			this.Column1.ReadOnly = true;
			this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Column1.Width = 50;
			// 
			// Column2
			// 
			this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
			this.Column2.DefaultCellStyle = dataGridViewCellStyle2;
			this.Column2.HeaderText = "Item";
			this.Column2.Name = "Column2";
			this.Column2.ReadOnly = true;
			this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Column3
			// 
			this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
			this.Column3.DefaultCellStyle = dataGridViewCellStyle3;
			this.Column3.HeaderText = "Value";
			this.Column3.Name = "Column3";
			this.Column3.ReadOnly = true;
			this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// Column4
			// 
			this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column4.HeaderText = "Set";
			this.Column4.Name = "Column4";
			this.Column4.ReadOnly = true;
			this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tmrSSDEditorUpdate
			// 
			this.tmrSSDEditorUpdate.Enabled = true;
			this.tmrSSDEditorUpdate.Interval = 300;
			this.tmrSSDEditorUpdate.Tick += new System.EventHandler(this.tmrSSDEditorUpdate_Tick);
			// 
			// frmProductEditPopup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(584, 561);
			this.ControlBox = false;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmProductEditPopup";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Product Infomation Editor";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmProductEditPopup_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tlpButton.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvProdInfo)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tlpButton;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnClear;
		private System.Windows.Forms.DataGridView dgvProdInfo;
		private System.Windows.Forms.Button btnPaste;
		private System.Windows.Forms.Button btnClearAll;
		private System.Windows.Forms.Button btnSaveAll;
		private System.Windows.Forms.Button btnCopy;
		private System.Windows.Forms.Timer tmrSSDEditorUpdate;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
	}
}