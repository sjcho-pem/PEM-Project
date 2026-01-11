namespace H_MarsSSD
{
	partial class frmTrayPnPRecipeEditor
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
			this.dgvTrayCol = new System.Windows.Forms.DataGridView();
			this.lbTrayCol = new System.Windows.Forms.Label();
			this.lbTrayRow = new System.Windows.Forms.Label();
			this.tbTrayMaxCol = new System.Windows.Forms.TextBox();
			this.tbTrayMaxRow = new System.Windows.Forms.TextBox();
			this.dgvTrayRow = new System.Windows.Forms.DataGridView();
			this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnImport = new System.Windows.Forms.Button();
			this.tabSetGap = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.dgvLdPnPCol = new System.Windows.Forms.DataGridView();
			this.lbLDPnPCol = new System.Windows.Forms.Label();
			this.lbLDPnPRow = new System.Windows.Forms.Label();
			this.dgvLdPnPRow = new System.Windows.Forms.DataGridView();
			this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbLdPnPMaxCol = new System.Windows.Forms.TextBox();
			this.tbLdPnPMaxRow = new System.Windows.Forms.TextBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.dgvUldPnPCol = new System.Windows.Forms.DataGridView();
			this.lbULDPnPCol = new System.Windows.Forms.Label();
			this.lbULDPnPRow = new System.Windows.Forms.Label();
			this.dgvUldPnPRow = new System.Windows.Forms.DataGridView();
			this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbUldPnPMaxCol = new System.Windows.Forms.TextBox();
			this.tbUldPnPMaxRow = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.dgvTrayCol)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvTrayRow)).BeginInit();
			this.tabSetGap.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvLdPnPCol)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvLdPnPRow)).BeginInit();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvUldPnPCol)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvUldPnPRow)).BeginInit();
			this.SuspendLayout();
			// 
			// dgvTrayCol
			// 
			this.dgvTrayCol.AllowUserToAddRows = false;
			this.dgvTrayCol.AllowUserToDeleteRows = false;
			this.dgvTrayCol.AllowUserToResizeColumns = false;
			this.dgvTrayCol.AllowUserToResizeRows = false;
			this.dgvTrayCol.BackgroundColor = System.Drawing.Color.White;
			this.dgvTrayCol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvTrayCol.ColumnHeadersVisible = false;
			this.dgvTrayCol.Location = new System.Drawing.Point(69, 75);
			this.dgvTrayCol.Name = "dgvTrayCol";
			this.dgvTrayCol.RowHeadersVisible = false;
			this.dgvTrayCol.RowTemplate.Height = 23;
			this.dgvTrayCol.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvTrayCol.Size = new System.Drawing.Size(940, 51);
			this.dgvTrayCol.TabIndex = 0;
			this.dgvTrayCol.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
			// 
			// lbTrayCol
			// 
			this.lbTrayCol.AutoSize = true;
			this.lbTrayCol.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbTrayCol.Location = new System.Drawing.Point(23, 12);
			this.lbTrayCol.Name = "lbTrayCol";
			this.lbTrayCol.Size = new System.Drawing.Size(63, 20);
			this.lbTrayCol.TabIndex = 1;
			this.lbTrayCol.Text = "Column";
			// 
			// lbTrayRow
			// 
			this.lbTrayRow.AutoSize = true;
			this.lbTrayRow.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbTrayRow.Location = new System.Drawing.Point(23, 42);
			this.lbTrayRow.Name = "lbTrayRow";
			this.lbTrayRow.Size = new System.Drawing.Size(40, 20);
			this.lbTrayRow.TabIndex = 2;
			this.lbTrayRow.Text = "Row";
			// 
			// tbTrayMaxCol
			// 
			this.tbTrayMaxCol.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbTrayMaxCol.Location = new System.Drawing.Point(87, 8);
			this.tbTrayMaxCol.Name = "tbTrayMaxCol";
			this.tbTrayMaxCol.Size = new System.Drawing.Size(100, 27);
			this.tbTrayMaxCol.TabIndex = 4;
			this.tbTrayMaxCol.Text = "10";
			this.tbTrayMaxCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tbTrayMaxCol.TextChanged += new System.EventHandler(this.tbMaxVal_TextChanged);
			// 
			// tbTrayMaxRow
			// 
			this.tbTrayMaxRow.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbTrayMaxRow.Location = new System.Drawing.Point(87, 39);
			this.tbTrayMaxRow.Name = "tbTrayMaxRow";
			this.tbTrayMaxRow.Size = new System.Drawing.Size(100, 27);
			this.tbTrayMaxRow.TabIndex = 5;
			this.tbTrayMaxRow.Text = "4";
			this.tbTrayMaxRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tbTrayMaxRow.TextChanged += new System.EventHandler(this.tbMaxVal_TextChanged);
			// 
			// dgvTrayRow
			// 
			this.dgvTrayRow.AllowUserToAddRows = false;
			this.dgvTrayRow.AllowUserToDeleteRows = false;
			this.dgvTrayRow.AllowUserToResizeColumns = false;
			this.dgvTrayRow.AllowUserToResizeRows = false;
			this.dgvTrayRow.BackgroundColor = System.Drawing.Color.White;
			this.dgvTrayRow.ColumnHeadersHeight = 50;
			this.dgvTrayRow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4});
			this.dgvTrayRow.Location = new System.Drawing.Point(20, 75);
			this.dgvTrayRow.Name = "dgvTrayRow";
			this.dgvTrayRow.RowHeadersVisible = false;
			this.dgvTrayRow.RowTemplate.Height = 23;
			this.dgvTrayRow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvTrayRow.Size = new System.Drawing.Size(50, 530);
			this.dgvTrayRow.TabIndex = 6;
			this.dgvTrayRow.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
			// 
			// Column4
			// 
			this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column4.HeaderText = "GAP";
			this.Column4.Name = "Column4";
			this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// btnSave
			// 
			this.btnSave.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnSave.Location = new System.Drawing.Point(962, 662);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(107, 37);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnImport
			// 
			this.btnImport.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnImport.Location = new System.Drawing.Point(849, 662);
			this.btnImport.Name = "btnImport";
			this.btnImport.Size = new System.Drawing.Size(107, 37);
			this.btnImport.TabIndex = 8;
			this.btnImport.Text = "Import";
			this.btnImport.UseVisualStyleBackColor = true;
			this.btnImport.Click += new System.EventHandler(this.btnOpen_Click);
			// 
			// tabSetGap
			// 
			this.tabSetGap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabSetGap.Controls.Add(this.tabPage1);
			this.tabSetGap.Controls.Add(this.tabPage2);
			this.tabSetGap.Controls.Add(this.tabPage3);
			this.tabSetGap.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabSetGap.Location = new System.Drawing.Point(12, 9);
			this.tabSetGap.Name = "tabSetGap";
			this.tabSetGap.SelectedIndex = 0;
			this.tabSetGap.Size = new System.Drawing.Size(1061, 647);
			this.tabSetGap.TabIndex = 9;
			this.tabSetGap.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabSetGap_Selected);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.dgvTrayCol);
			this.tabPage1.Controls.Add(this.lbTrayCol);
			this.tabPage1.Controls.Add(this.lbTrayRow);
			this.tabPage1.Controls.Add(this.dgvTrayRow);
			this.tabPage1.Controls.Add(this.tbTrayMaxCol);
			this.tabPage1.Controls.Add(this.tbTrayMaxRow);
			this.tabPage1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabPage1.Location = new System.Drawing.Point(4, 26);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(1053, 617);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Tray";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.dgvLdPnPCol);
			this.tabPage2.Controls.Add(this.lbLDPnPCol);
			this.tabPage2.Controls.Add(this.lbLDPnPRow);
			this.tabPage2.Controls.Add(this.dgvLdPnPRow);
			this.tabPage2.Controls.Add(this.tbLdPnPMaxCol);
			this.tabPage2.Controls.Add(this.tbLdPnPMaxRow);
			this.tabPage2.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabPage2.Location = new System.Drawing.Point(4, 26);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(1053, 617);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "LoadPnP";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// dgvLdPnPCol
			// 
			this.dgvLdPnPCol.AllowUserToAddRows = false;
			this.dgvLdPnPCol.AllowUserToDeleteRows = false;
			this.dgvLdPnPCol.AllowUserToResizeColumns = false;
			this.dgvLdPnPCol.AllowUserToResizeRows = false;
			this.dgvLdPnPCol.BackgroundColor = System.Drawing.Color.White;
			this.dgvLdPnPCol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvLdPnPCol.ColumnHeadersVisible = false;
			this.dgvLdPnPCol.Location = new System.Drawing.Point(134, 84);
			this.dgvLdPnPCol.Name = "dgvLdPnPCol";
			this.dgvLdPnPCol.RowHeadersVisible = false;
			this.dgvLdPnPCol.RowTemplate.Height = 23;
			this.dgvLdPnPCol.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgvLdPnPCol.Size = new System.Drawing.Size(900, 50);
			this.dgvLdPnPCol.TabIndex = 7;
			this.dgvLdPnPCol.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
			// 
			// lbLDPnPCol
			// 
			this.lbLDPnPCol.AutoSize = true;
			this.lbLDPnPCol.Location = new System.Drawing.Point(70, 17);
			this.lbLDPnPCol.Name = "lbLDPnPCol";
			this.lbLDPnPCol.Size = new System.Drawing.Size(63, 20);
			this.lbLDPnPCol.TabIndex = 8;
			this.lbLDPnPCol.Text = "Column";
			// 
			// lbLDPnPRow
			// 
			this.lbLDPnPRow.AutoSize = true;
			this.lbLDPnPRow.Location = new System.Drawing.Point(70, 47);
			this.lbLDPnPRow.Name = "lbLDPnPRow";
			this.lbLDPnPRow.Size = new System.Drawing.Size(40, 20);
			this.lbLDPnPRow.TabIndex = 9;
			this.lbLDPnPRow.Text = "Row";
			// 
			// dgvLdPnPRow
			// 
			this.dgvLdPnPRow.AllowUserToAddRows = false;
			this.dgvLdPnPRow.AllowUserToDeleteRows = false;
			this.dgvLdPnPRow.AllowUserToResizeColumns = false;
			this.dgvLdPnPRow.AllowUserToResizeRows = false;
			this.dgvLdPnPRow.BackgroundColor = System.Drawing.Color.White;
			this.dgvLdPnPRow.ColumnHeadersHeight = 50;
			this.dgvLdPnPRow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column5});
			this.dgvLdPnPRow.Location = new System.Drawing.Point(85, 133);
			this.dgvLdPnPRow.Name = "dgvLdPnPRow";
			this.dgvLdPnPRow.RowHeadersVisible = false;
			this.dgvLdPnPRow.RowTemplate.Height = 23;
			this.dgvLdPnPRow.Size = new System.Drawing.Size(50, 355);
			this.dgvLdPnPRow.TabIndex = 12;
			this.dgvLdPnPRow.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
			// 
			// Column5
			// 
			this.Column5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column5.HeaderText = "GAP";
			this.Column5.Name = "Column5";
			this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tbLdPnPMaxCol
			// 
			this.tbLdPnPMaxCol.Location = new System.Drawing.Point(134, 13);
			this.tbLdPnPMaxCol.Name = "tbLdPnPMaxCol";
			this.tbLdPnPMaxCol.Size = new System.Drawing.Size(100, 27);
			this.tbLdPnPMaxCol.TabIndex = 10;
			this.tbLdPnPMaxCol.Text = "4";
			this.tbLdPnPMaxCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tbLdPnPMaxCol.TextChanged += new System.EventHandler(this.tbMaxVal_TextChanged);
			// 
			// tbLdPnPMaxRow
			// 
			this.tbLdPnPMaxRow.Location = new System.Drawing.Point(134, 44);
			this.tbLdPnPMaxRow.Name = "tbLdPnPMaxRow";
			this.tbLdPnPMaxRow.Size = new System.Drawing.Size(100, 27);
			this.tbLdPnPMaxRow.TabIndex = 11;
			this.tbLdPnPMaxRow.Text = "2";
			this.tbLdPnPMaxRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tbLdPnPMaxRow.TextChanged += new System.EventHandler(this.tbMaxVal_TextChanged);
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.dgvUldPnPCol);
			this.tabPage3.Controls.Add(this.lbULDPnPCol);
			this.tabPage3.Controls.Add(this.lbULDPnPRow);
			this.tabPage3.Controls.Add(this.dgvUldPnPRow);
			this.tabPage3.Controls.Add(this.tbUldPnPMaxCol);
			this.tabPage3.Controls.Add(this.tbUldPnPMaxRow);
			this.tabPage3.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabPage3.Location = new System.Drawing.Point(4, 26);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(1053, 617);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "UnloadPnP";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// dgvUldPnPCol
			// 
			this.dgvUldPnPCol.AllowUserToAddRows = false;
			this.dgvUldPnPCol.AllowUserToDeleteRows = false;
			this.dgvUldPnPCol.AllowUserToResizeColumns = false;
			this.dgvUldPnPCol.AllowUserToResizeRows = false;
			this.dgvUldPnPCol.BackgroundColor = System.Drawing.Color.White;
			this.dgvUldPnPCol.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvUldPnPCol.ColumnHeadersVisible = false;
			this.dgvUldPnPCol.Location = new System.Drawing.Point(134, 84);
			this.dgvUldPnPCol.Name = "dgvUldPnPCol";
			this.dgvUldPnPCol.RowHeadersVisible = false;
			this.dgvUldPnPCol.RowTemplate.Height = 23;
			this.dgvUldPnPCol.Size = new System.Drawing.Size(900, 50);
			this.dgvUldPnPCol.TabIndex = 13;
			this.dgvUldPnPCol.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
			// 
			// lbULDPnPCol
			// 
			this.lbULDPnPCol.AutoSize = true;
			this.lbULDPnPCol.Location = new System.Drawing.Point(70, 17);
			this.lbULDPnPCol.Name = "lbULDPnPCol";
			this.lbULDPnPCol.Size = new System.Drawing.Size(63, 20);
			this.lbULDPnPCol.TabIndex = 14;
			this.lbULDPnPCol.Text = "Column";
			// 
			// lbULDPnPRow
			// 
			this.lbULDPnPRow.AutoSize = true;
			this.lbULDPnPRow.Location = new System.Drawing.Point(70, 47);
			this.lbULDPnPRow.Name = "lbULDPnPRow";
			this.lbULDPnPRow.Size = new System.Drawing.Size(40, 20);
			this.lbULDPnPRow.TabIndex = 15;
			this.lbULDPnPRow.Text = "Row";
			// 
			// dgvUldPnPRow
			// 
			this.dgvUldPnPRow.AllowUserToAddRows = false;
			this.dgvUldPnPRow.AllowUserToDeleteRows = false;
			this.dgvUldPnPRow.AllowUserToResizeColumns = false;
			this.dgvUldPnPRow.AllowUserToResizeRows = false;
			this.dgvUldPnPRow.BackgroundColor = System.Drawing.Color.White;
			this.dgvUldPnPRow.ColumnHeadersHeight = 50;
			this.dgvUldPnPRow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6});
			this.dgvUldPnPRow.Location = new System.Drawing.Point(85, 133);
			this.dgvUldPnPRow.Name = "dgvUldPnPRow";
			this.dgvUldPnPRow.RowHeadersVisible = false;
			this.dgvUldPnPRow.RowTemplate.Height = 23;
			this.dgvUldPnPRow.Size = new System.Drawing.Size(50, 355);
			this.dgvUldPnPRow.TabIndex = 18;
			this.dgvUldPnPRow.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_CellClick);
			// 
			// Column6
			// 
			this.Column6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Column6.HeaderText = "GAP";
			this.Column6.Name = "Column6";
			this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tbUldPnPMaxCol
			// 
			this.tbUldPnPMaxCol.Location = new System.Drawing.Point(134, 13);
			this.tbUldPnPMaxCol.Name = "tbUldPnPMaxCol";
			this.tbUldPnPMaxCol.Size = new System.Drawing.Size(100, 27);
			this.tbUldPnPMaxCol.TabIndex = 16;
			this.tbUldPnPMaxCol.Text = "4";
			this.tbUldPnPMaxCol.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tbUldPnPMaxCol.TextChanged += new System.EventHandler(this.tbMaxVal_TextChanged);
			// 
			// tbUldPnPMaxRow
			// 
			this.tbUldPnPMaxRow.Location = new System.Drawing.Point(134, 44);
			this.tbUldPnPMaxRow.Name = "tbUldPnPMaxRow";
			this.tbUldPnPMaxRow.Size = new System.Drawing.Size(100, 27);
			this.tbUldPnPMaxRow.TabIndex = 17;
			this.tbUldPnPMaxRow.Text = "2";
			this.tbUldPnPMaxRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.tbUldPnPMaxRow.TextChanged += new System.EventHandler(this.tbMaxVal_TextChanged);
			// 
			// frmTrayPnPRecipeEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1085, 711);
			this.Controls.Add(this.tabSetGap);
			this.Controls.Add(this.btnImport);
			this.Controls.Add(this.btnSave);
			this.Name = "frmTrayPnPRecipeEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "TraySizeSet";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTrayPnPRecipeEditor_FormClosing);
			this.Load += new System.EventHandler(this.frmTrayPnPRecipeEditor_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgvTrayCol)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvTrayRow)).EndInit();
			this.tabSetGap.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvLdPnPCol)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvLdPnPRow)).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvUldPnPCol)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgvUldPnPRow)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.DataGridView dgvTrayCol;
		private System.Windows.Forms.Label lbTrayCol;
		private System.Windows.Forms.Label lbTrayRow;
		private System.Windows.Forms.TextBox tbTrayMaxCol;
		private System.Windows.Forms.TextBox tbTrayMaxRow;
		private System.Windows.Forms.DataGridView dgvTrayRow;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnImport;
		private System.Windows.Forms.TabControl tabSetGap;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.DataGridView dgvLdPnPCol;
		private System.Windows.Forms.Label lbLDPnPCol;
		private System.Windows.Forms.Label lbLDPnPRow;
		private System.Windows.Forms.DataGridView dgvLdPnPRow;
		private System.Windows.Forms.TextBox tbLdPnPMaxCol;
		private System.Windows.Forms.TextBox tbLdPnPMaxRow;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.DataGridView dgvUldPnPCol;
		private System.Windows.Forms.Label lbULDPnPCol;
		private System.Windows.Forms.Label lbULDPnPRow;
		private System.Windows.Forms.DataGridView dgvUldPnPRow;
		private System.Windows.Forms.TextBox tbUldPnPMaxCol;
		private System.Windows.Forms.TextBox tbUldPnPMaxRow;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
	}
}