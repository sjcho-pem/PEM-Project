namespace H_MarsSSD
{
    partial class frmData_Useskip
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			this.chkUseskipMod = new System.Windows.Forms.CheckBox();
			this.chkUseskipEnable = new System.Windows.Forms.CheckBox();
			this.dgridUseskip1 = new System.Windows.Forms.DataGridView();
			this.tbColumnUseSkip1Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnUseSkip1Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnUseSkip1Use = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgridUseskip2 = new System.Windows.Forms.DataGridView();
			this.tbColumnUseSkip2Num = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnUseSkip2Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnUseSkip2Use = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgridUsePart = new System.Windows.Forms.DataGridView();
			this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
			this.tmrUpdateUsePart = new System.Windows.Forms.Timer(this.components);
			this.chkDefault = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.dgridUseskip1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridUseskip2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridUsePart)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
			this.SuspendLayout();
			// 
			// chkUseskipMod
			// 
			this.chkUseskipMod.BackColor = System.Drawing.Color.Transparent;
			this.chkUseskipMod.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkUseskipMod.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkUseskipMod.Location = new System.Drawing.Point(14, 66);
			this.chkUseskipMod.Name = "chkUseskipMod";
			this.chkUseskipMod.Size = new System.Drawing.Size(210, 49);
			this.chkUseskipMod.TabIndex = 10;
			this.chkUseskipMod.Text = "NAME MODIFY";
			this.chkUseskipMod.UseVisualStyleBackColor = false;
			this.chkUseskipMod.CheckedChanged += new System.EventHandler(this.chkUseskipMod_CheckedChanged);
			// 
			// chkUseskipEnable
			// 
			this.chkUseskipEnable.BackColor = System.Drawing.Color.Transparent;
			this.chkUseskipEnable.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkUseskipEnable.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkUseskipEnable.Location = new System.Drawing.Point(242, 66);
			this.chkUseskipEnable.Name = "chkUseskipEnable";
			this.chkUseskipEnable.Size = new System.Drawing.Size(132, 49);
			this.chkUseskipEnable.TabIndex = 11;
			this.chkUseskipEnable.Text = "ENABLE";
			this.chkUseskipEnable.UseVisualStyleBackColor = false;
			this.chkUseskipEnable.CheckedChanged += new System.EventHandler(this.chkUseskipEnable_CheckedChanged);
			// 
			// dgridUseskip1
			// 
			this.dgridUseskip1.AllowUserToAddRows = false;
			this.dgridUseskip1.AllowUserToDeleteRows = false;
			this.dgridUseskip1.AllowUserToResizeColumns = false;
			this.dgridUseskip1.AllowUserToResizeRows = false;
			this.dgridUseskip1.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridUseskip1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgridUseskip1.ColumnHeadersHeight = 40;
			this.dgridUseskip1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridUseskip1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnUseSkip1Num,
            this.tbColumnUseSkip1Name,
            this.tbColumnUseSkip1Use});
			this.dgridUseskip1.EnableHeadersVisualStyles = false;
			this.dgridUseskip1.Location = new System.Drawing.Point(14, 121);
			this.dgridUseskip1.MultiSelect = false;
			this.dgridUseskip1.Name = "dgridUseskip1";
			this.dgridUseskip1.RowHeadersVisible = false;
			this.dgridUseskip1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridUseskip1.RowTemplate.Height = 35;
			this.dgridUseskip1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridUseskip1.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dgridUseskip1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridUseskip1.Size = new System.Drawing.Size(530, 650);
			this.dgridUseskip1.TabIndex = 12;
			this.dgridUseskip1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridUseskip1_CellDoubleClick);
			this.dgridUseskip1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridUseskip1_CellValueChanged);
			this.dgridUseskip1.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridUseskip1_ColumnHeaderMouseDoubleClick);
			// 
			// tbColumnUseSkip1Num
			// 
			this.tbColumnUseSkip1Num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnUseSkip1Num.DefaultCellStyle = dataGridViewCellStyle2;
			this.tbColumnUseSkip1Num.HeaderText = "NO";
			this.tbColumnUseSkip1Num.Name = "tbColumnUseSkip1Num";
			this.tbColumnUseSkip1Num.ReadOnly = true;
			this.tbColumnUseSkip1Num.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnUseSkip1Num.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnUseSkip1Num.Width = 60;
			// 
			// tbColumnUseSkip1Name
			// 
			this.tbColumnUseSkip1Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.tbColumnUseSkip1Name.HeaderText = "USE/SKIP NAME";
			this.tbColumnUseSkip1Name.Name = "tbColumnUseSkip1Name";
			this.tbColumnUseSkip1Name.ReadOnly = true;
			this.tbColumnUseSkip1Name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnUseSkip1Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tbColumnUseSkip1Use
			// 
			this.tbColumnUseSkip1Use.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnUseSkip1Use.DefaultCellStyle = dataGridViewCellStyle3;
			this.tbColumnUseSkip1Use.HeaderText = "USE";
			this.tbColumnUseSkip1Use.Name = "tbColumnUseSkip1Use";
			this.tbColumnUseSkip1Use.ReadOnly = true;
			this.tbColumnUseSkip1Use.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnUseSkip1Use.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnUseSkip1Use.Width = 135;
			// 
			// dgridUseskip2
			// 
			this.dgridUseskip2.AllowUserToAddRows = false;
			this.dgridUseskip2.AllowUserToDeleteRows = false;
			this.dgridUseskip2.AllowUserToResizeColumns = false;
			this.dgridUseskip2.AllowUserToResizeRows = false;
			this.dgridUseskip2.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridUseskip2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.dgridUseskip2.ColumnHeadersHeight = 40;
			this.dgridUseskip2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridUseskip2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnUseSkip2Num,
            this.tbColumnUseSkip2Name,
            this.tbColumnUseSkip2Use});
			this.dgridUseskip2.EnableHeadersVisualStyles = false;
			this.dgridUseskip2.Location = new System.Drawing.Point(546, 121);
			this.dgridUseskip2.MultiSelect = false;
			this.dgridUseskip2.Name = "dgridUseskip2";
			this.dgridUseskip2.RowHeadersVisible = false;
			this.dgridUseskip2.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridUseskip2.RowTemplate.Height = 35;
			this.dgridUseskip2.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridUseskip2.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dgridUseskip2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridUseskip2.Size = new System.Drawing.Size(530, 650);
			this.dgridUseskip2.TabIndex = 13;
			this.dgridUseskip2.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridUseskip2_CellDoubleClick);
			this.dgridUseskip2.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridUseskip2_CellValueChanged);
			this.dgridUseskip2.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridUseskip2_ColumnHeaderMouseDoubleClick);
			// 
			// tbColumnUseSkip2Num
			// 
			this.tbColumnUseSkip2Num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnUseSkip2Num.DefaultCellStyle = dataGridViewCellStyle5;
			this.tbColumnUseSkip2Num.HeaderText = "NO";
			this.tbColumnUseSkip2Num.Name = "tbColumnUseSkip2Num";
			this.tbColumnUseSkip2Num.ReadOnly = true;
			this.tbColumnUseSkip2Num.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnUseSkip2Num.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnUseSkip2Num.Width = 60;
			// 
			// tbColumnUseSkip2Name
			// 
			this.tbColumnUseSkip2Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.tbColumnUseSkip2Name.HeaderText = "USE/SKIP NAME";
			this.tbColumnUseSkip2Name.Name = "tbColumnUseSkip2Name";
			this.tbColumnUseSkip2Name.ReadOnly = true;
			this.tbColumnUseSkip2Name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnUseSkip2Name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tbColumnUseSkip2Use
			// 
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnUseSkip2Use.DefaultCellStyle = dataGridViewCellStyle6;
			this.tbColumnUseSkip2Use.HeaderText = "USE";
			this.tbColumnUseSkip2Use.Name = "tbColumnUseSkip2Use";
			this.tbColumnUseSkip2Use.ReadOnly = true;
			this.tbColumnUseSkip2Use.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnUseSkip2Use.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnUseSkip2Use.Width = 135;
			// 
			// dgridUsePart
			// 
			this.dgridUsePart.AllowUserToAddRows = false;
			this.dgridUsePart.AllowUserToDeleteRows = false;
			this.dgridUsePart.AllowUserToResizeColumns = false;
			this.dgridUsePart.AllowUserToResizeRows = false;
			this.dgridUsePart.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridUsePart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
			this.dgridUsePart.ColumnHeadersHeight = 40;
			this.dgridUsePart.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridUsePart.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
			this.dgridUsePart.EnableHeadersVisualStyles = false;
			this.dgridUsePart.Location = new System.Drawing.Point(1078, 121);
			this.dgridUsePart.MultiSelect = false;
			this.dgridUsePart.Name = "dgridUsePart";
			this.dgridUsePart.RowHeadersVisible = false;
			this.dgridUsePart.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridUsePart.RowTemplate.Height = 35;
			this.dgridUsePart.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridUsePart.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgridUsePart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridUsePart.Size = new System.Drawing.Size(370, 650);
			this.dgridUsePart.TabIndex = 14;
			this.dgridUsePart.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridUsePart_CellDoubleClick);
			// 
			// dataGridViewTextBoxColumn1
			// 
			this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle8;
			this.dataGridViewTextBoxColumn1.HeaderText = "NO";
			this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			this.dataGridViewTextBoxColumn1.ReadOnly = true;
			this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn1.Width = 60;
			// 
			// dataGridViewTextBoxColumn2
			// 
			this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.dataGridViewTextBoxColumn2.HeaderText = "PART USE NAME";
			this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			this.dataGridViewTextBoxColumn2.ReadOnly = true;
			this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn2.Width = 175;
			// 
			// dataGridViewTextBoxColumn3
			// 
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle9;
			this.dataGridViewTextBoxColumn3.HeaderText = "MODE";
			this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			this.dataGridViewTextBoxColumn3.ReadOnly = true;
			this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.dataGridViewTextBoxColumn3.Width = 115;
			// 
			// errorProvider1
			// 
			this.errorProvider1.ContainerControl = this;
			// 
			// tmrUpdateUsePart
			// 
			this.tmrUpdateUsePart.Enabled = true;
			this.tmrUpdateUsePart.Tick += new System.EventHandler(this.tmrUpdateUsePart_Tick);
			// 
			// chkDefault
			// 
			this.chkDefault.BackColor = System.Drawing.Color.Transparent;
			this.chkDefault.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkDefault.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkDefault.Location = new System.Drawing.Point(380, 66);
			this.chkDefault.Name = "chkDefault";
			this.chkDefault.Size = new System.Drawing.Size(160, 49);
			this.chkDefault.TabIndex = 15;
			this.chkDefault.Text = "DEFAULT";
			this.chkDefault.UseVisualStyleBackColor = false;
			this.chkDefault.CheckedChanged += new System.EventHandler(this.chkDefault_CheckedChanged);
			// 
			// frmData_Useskip
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 824);
			this.Controls.Add(this.chkDefault);
			this.Controls.Add(this.dgridUsePart);
			this.Controls.Add(this.dgridUseskip2);
			this.Controls.Add(this.dgridUseskip1);
			this.Controls.Add(this.chkUseskipEnable);
			this.Controls.Add(this.chkUseskipMod);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmData_Useskip";
			this.Text = "frmData_Useskip";
			((System.ComponentModel.ISupportInitialize)(this.dgridUseskip1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridUseskip2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridUsePart)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkUseskipMod;
        private System.Windows.Forms.CheckBox chkUseskipEnable;
        private System.Windows.Forms.DataGridView dgridUseskip1;
        private System.Windows.Forms.DataGridView dgridUseskip2;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnUseSkip1Num;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnUseSkip1Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnUseSkip1Use;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnUseSkip2Num;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnUseSkip2Name;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnUseSkip2Use;
		private System.Windows.Forms.DataGridView dgridUsePart;
		private System.Windows.Forms.ErrorProvider errorProvider1;
		private System.Windows.Forms.Timer tmrUpdateUsePart;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
		private System.Windows.Forms.CheckBox chkDefault;
	}
}