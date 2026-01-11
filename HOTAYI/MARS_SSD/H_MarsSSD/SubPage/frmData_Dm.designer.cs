namespace H_MarsSSD
{
    partial class frmData_Dm
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
			this.chkNDmMod = new System.Windows.Forms.CheckBox();
			this.chkNDmEnable = new System.Windows.Forms.CheckBox();
			this.tmrDm = new System.Windows.Forms.Timer(this.components);
			this.dgridNDm = new System.Windows.Forms.DataGridView();
			this.tbColumnDmNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnNDmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnNDmValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgridDDm = new System.Windows.Forms.DataGridView();
			this.tbColumnDDmNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnDDmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnDDmValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.chkDDmEnable = new System.Windows.Forms.CheckBox();
			this.chkDDmMod = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.dgridNDm)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridDDm)).BeginInit();
			this.SuspendLayout();
			// 
			// chkNDmMod
			// 
			this.chkNDmMod.BackColor = System.Drawing.Color.Transparent;
			this.chkNDmMod.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkNDmMod.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkNDmMod.Location = new System.Drawing.Point(14, 66);
			this.chkNDmMod.Name = "chkNDmMod";
			this.chkNDmMod.Size = new System.Drawing.Size(210, 49);
			this.chkNDmMod.TabIndex = 9;
			this.chkNDmMod.Text = "NAME MODIFY";
			this.chkNDmMod.UseVisualStyleBackColor = false;
			this.chkNDmMod.CheckedChanged += new System.EventHandler(this.chkNDmMod_CheckedChanged);
			// 
			// chkNDmEnable
			// 
			this.chkNDmEnable.BackColor = System.Drawing.Color.Transparent;
			this.chkNDmEnable.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkNDmEnable.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkNDmEnable.Location = new System.Drawing.Point(249, 66);
			this.chkNDmEnable.Name = "chkNDmEnable";
			this.chkNDmEnable.Size = new System.Drawing.Size(160, 49);
			this.chkNDmEnable.TabIndex = 10;
			this.chkNDmEnable.Text = "ENABLE";
			this.chkNDmEnable.UseVisualStyleBackColor = false;
			this.chkNDmEnable.CheckedChanged += new System.EventHandler(this.chkNDmEnable_CheckedChanged);
			// 
			// tmrDm
			// 
			this.tmrDm.Tick += new System.EventHandler(this.tmrDm_Tick);
			// 
			// dgridNDm
			// 
			this.dgridNDm.AllowUserToAddRows = false;
			this.dgridNDm.AllowUserToDeleteRows = false;
			this.dgridNDm.AllowUserToResizeColumns = false;
			this.dgridNDm.AllowUserToResizeRows = false;
			this.dgridNDm.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridNDm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgridNDm.ColumnHeadersHeight = 40;
			this.dgridNDm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridNDm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnDmNum,
            this.tbColumnNDmName,
            this.tbColumnNDmValue});
			this.dgridNDm.EnableHeadersVisualStyles = false;
			this.dgridNDm.Location = new System.Drawing.Point(14, 121);
			this.dgridNDm.MultiSelect = false;
			this.dgridNDm.Name = "dgridNDm";
			this.dgridNDm.RowHeadersVisible = false;
			this.dgridNDm.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridNDm.RowTemplate.Height = 35;
			this.dgridNDm.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridNDm.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgridNDm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridNDm.Size = new System.Drawing.Size(700, 650);
			this.dgridNDm.TabIndex = 12;
			this.dgridNDm.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridNDm_CellValueChanged);
			this.dgridNDm.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridNDm_ColumnHeaderMouseDoubleClick);
			// 
			// tbColumnDmNum
			// 
			this.tbColumnDmNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnDmNum.DefaultCellStyle = dataGridViewCellStyle2;
			this.tbColumnDmNum.HeaderText = "NO";
			this.tbColumnDmNum.Name = "tbColumnDmNum";
			this.tbColumnDmNum.ReadOnly = true;
			this.tbColumnDmNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnDmNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnDmNum.Width = 90;
			// 
			// tbColumnNDmName
			// 
			this.tbColumnNDmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.tbColumnNDmName.DefaultCellStyle = dataGridViewCellStyle3;
			this.tbColumnNDmName.HeaderText = "INTEGER DATA MEMORY NAME";
			this.tbColumnNDmName.Name = "tbColumnNDmName";
			this.tbColumnNDmName.ReadOnly = true;
			this.tbColumnNDmName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnNDmName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnNDmName.Width = 390;
			// 
			// tbColumnNDmValue
			// 
			this.tbColumnNDmValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.tbColumnNDmValue.DefaultCellStyle = dataGridViewCellStyle4;
			this.tbColumnNDmValue.HeaderText = "VALUE";
			this.tbColumnNDmValue.Name = "tbColumnNDmValue";
			this.tbColumnNDmValue.ReadOnly = true;
			this.tbColumnNDmValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnNDmValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnNDmValue.Width = 200;
			// 
			// dgridDDm
			// 
			this.dgridDDm.AllowUserToAddRows = false;
			this.dgridDDm.AllowUserToDeleteRows = false;
			this.dgridDDm.AllowUserToResizeColumns = false;
			this.dgridDDm.AllowUserToResizeRows = false;
			this.dgridDDm.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridDDm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.dgridDDm.ColumnHeadersHeight = 40;
			this.dgridDDm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridDDm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnDDmNum,
            this.tbColumnDDmName,
            this.tbColumnDDmValue});
			this.dgridDDm.EnableHeadersVisualStyles = false;
			this.dgridDDm.Location = new System.Drawing.Point(736, 121);
			this.dgridDDm.MultiSelect = false;
			this.dgridDDm.Name = "dgridDDm";
			this.dgridDDm.RowHeadersVisible = false;
			this.dgridDDm.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridDDm.RowTemplate.Height = 35;
			this.dgridDDm.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridDDm.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgridDDm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridDDm.Size = new System.Drawing.Size(700, 650);
			this.dgridDDm.TabIndex = 15;
			this.dgridDDm.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridDDm_CellValueChanged);
			this.dgridDDm.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridDDm_ColumnHeaderMouseDoubleClick);
			// 
			// tbColumnDDmNum
			// 
			this.tbColumnDDmNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnDDmNum.DefaultCellStyle = dataGridViewCellStyle6;
			this.tbColumnDDmNum.HeaderText = "NO";
			this.tbColumnDDmNum.Name = "tbColumnDDmNum";
			this.tbColumnDDmNum.ReadOnly = true;
			this.tbColumnDDmNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnDDmNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnDDmNum.Width = 90;
			// 
			// tbColumnDDmName
			// 
			this.tbColumnDDmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.tbColumnDDmName.DefaultCellStyle = dataGridViewCellStyle7;
			this.tbColumnDDmName.HeaderText = "DOUBLE DATA MEMORY NAME";
			this.tbColumnDDmName.Name = "tbColumnDDmName";
			this.tbColumnDDmName.ReadOnly = true;
			this.tbColumnDDmName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnDDmName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnDDmName.Width = 390;
			// 
			// tbColumnDDmValue
			// 
			this.tbColumnDDmValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.tbColumnDDmValue.DefaultCellStyle = dataGridViewCellStyle8;
			this.tbColumnDDmValue.HeaderText = "VALUE";
			this.tbColumnDDmValue.Name = "tbColumnDDmValue";
			this.tbColumnDDmValue.ReadOnly = true;
			this.tbColumnDDmValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnDDmValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnDDmValue.Width = 200;
			// 
			// chkDDmEnable
			// 
			this.chkDDmEnable.BackColor = System.Drawing.Color.Transparent;
			this.chkDDmEnable.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkDDmEnable.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkDDmEnable.Location = new System.Drawing.Point(971, 66);
			this.chkDDmEnable.Name = "chkDDmEnable";
			this.chkDDmEnable.Size = new System.Drawing.Size(160, 49);
			this.chkDDmEnable.TabIndex = 14;
			this.chkDDmEnable.Text = "ENABLE";
			this.chkDDmEnable.UseVisualStyleBackColor = false;
			this.chkDDmEnable.CheckedChanged += new System.EventHandler(this.chkDDmEnable_CheckedChanged);
			// 
			// chkDDmMod
			// 
			this.chkDDmMod.BackColor = System.Drawing.Color.Transparent;
			this.chkDDmMod.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkDDmMod.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkDDmMod.Location = new System.Drawing.Point(736, 66);
			this.chkDDmMod.Name = "chkDDmMod";
			this.chkDDmMod.Size = new System.Drawing.Size(210, 49);
			this.chkDDmMod.TabIndex = 13;
			this.chkDDmMod.Text = "NAME MODIFY";
			this.chkDDmMod.UseVisualStyleBackColor = false;
			this.chkDDmMod.CheckedChanged += new System.EventHandler(this.chkDDmMod_CheckedChanged);
			// 
			// frmData_Dm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 816);
			this.Controls.Add(this.dgridDDm);
			this.Controls.Add(this.chkDDmEnable);
			this.Controls.Add(this.chkDDmMod);
			this.Controls.Add(this.dgridNDm);
			this.Controls.Add(this.chkNDmEnable);
			this.Controls.Add(this.chkNDmMod);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmData_Dm";
			this.Text = "frmData_Dm";
			((System.ComponentModel.ISupportInitialize)(this.dgridNDm)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridDDm)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkNDmMod;
        private System.Windows.Forms.CheckBox chkNDmEnable;
        private System.Windows.Forms.DataGridView dgridNDm;
        private System.Windows.Forms.Timer tmrDm;
        private System.Windows.Forms.DataGridView dgridDDm;
        private System.Windows.Forms.CheckBox chkDDmEnable;
        private System.Windows.Forms.CheckBox chkDDmMod;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnDDmNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnDDmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnDDmValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnDmNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnNDmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnNDmValue;
    }
}