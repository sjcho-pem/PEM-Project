namespace H_MarsSSD
{
    partial class frmData_SDm
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.chkSDmMod = new System.Windows.Forms.CheckBox();
			this.chkSDmEnable = new System.Windows.Forms.CheckBox();
			this.tmrDm = new System.Windows.Forms.Timer(this.components);
			this.dgridSDm = new System.Windows.Forms.DataGridView();
			this.tbColumnDmNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnSDmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnSDmValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dgridSDm)).BeginInit();
			this.SuspendLayout();
			// 
			// chkSDmMod
			// 
			this.chkSDmMod.BackColor = System.Drawing.Color.Transparent;
			this.chkSDmMod.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkSDmMod.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkSDmMod.Location = new System.Drawing.Point(14, 66);
			this.chkSDmMod.Name = "chkSDmMod";
			this.chkSDmMod.Size = new System.Drawing.Size(210, 49);
			this.chkSDmMod.TabIndex = 9;
			this.chkSDmMod.Text = "NAME MODIFY";
			this.chkSDmMod.UseVisualStyleBackColor = false;
			this.chkSDmMod.CheckedChanged += new System.EventHandler(this.chkSDmMod_CheckedChanged);
			// 
			// chkSDmEnable
			// 
			this.chkSDmEnable.BackColor = System.Drawing.Color.Transparent;
			this.chkSDmEnable.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkSDmEnable.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkSDmEnable.Location = new System.Drawing.Point(249, 66);
			this.chkSDmEnable.Name = "chkSDmEnable";
			this.chkSDmEnable.Size = new System.Drawing.Size(160, 49);
			this.chkSDmEnable.TabIndex = 10;
			this.chkSDmEnable.Text = "ENABLE";
			this.chkSDmEnable.UseVisualStyleBackColor = false;
			this.chkSDmEnable.CheckedChanged += new System.EventHandler(this.chkSDmEnable_CheckedChanged);
			// 
			// tmrDm
			// 
			this.tmrDm.Tick += new System.EventHandler(this.tmrDm_Tick);
			// 
			// dgridSDm
			// 
			this.dgridSDm.AllowUserToAddRows = false;
			this.dgridSDm.AllowUserToDeleteRows = false;
			this.dgridSDm.AllowUserToResizeColumns = false;
			this.dgridSDm.AllowUserToResizeRows = false;
			this.dgridSDm.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridSDm.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
			this.dgridSDm.ColumnHeadersHeight = 40;
			this.dgridSDm.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridSDm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnDmNum,
            this.tbColumnSDmName,
            this.tbColumnSDmValue});
			this.dgridSDm.EnableHeadersVisualStyles = false;
			this.dgridSDm.Location = new System.Drawing.Point(14, 121);
			this.dgridSDm.MultiSelect = false;
			this.dgridSDm.Name = "dgridSDm";
			this.dgridSDm.RowHeadersVisible = false;
			this.dgridSDm.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridSDm.RowTemplate.Height = 35;
			this.dgridSDm.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridSDm.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgridSDm.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridSDm.Size = new System.Drawing.Size(710, 650);
			this.dgridSDm.TabIndex = 12;
			this.dgridSDm.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridSDm_CellValueChanged);
			this.dgridSDm.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridSDm_ColumnHeaderMouseDoubleClick);
			// 
			// tbColumnDmNum
			// 
			this.tbColumnDmNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnDmNum.DefaultCellStyle = dataGridViewCellStyle6;
			this.tbColumnDmNum.HeaderText = "NO";
			this.tbColumnDmNum.Name = "tbColumnDmNum";
			this.tbColumnDmNum.ReadOnly = true;
			this.tbColumnDmNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnDmNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnDmNum.Width = 60;
			// 
			// tbColumnNDmName
			// 
			this.tbColumnSDmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.tbColumnSDmName.DefaultCellStyle = dataGridViewCellStyle7;
			this.tbColumnSDmName.HeaderText = "INTEGER DATA MEMORY NAME";
			this.tbColumnSDmName.Name = "tbColumnNDmName";
			this.tbColumnSDmName.ReadOnly = true;
			this.tbColumnSDmName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnSDmName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tbColumnNDmValue
			// 
			this.tbColumnSDmValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.tbColumnSDmValue.DefaultCellStyle = dataGridViewCellStyle8;
			this.tbColumnSDmValue.HeaderText = "VALUE";
			this.tbColumnSDmValue.Name = "tbColumnNDmValue";
			this.tbColumnSDmValue.ReadOnly = true;
			this.tbColumnSDmValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnSDmValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnSDmValue.Width = 156;
			// 
			// frmData_SDm
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 824);
			this.Controls.Add(this.dgridSDm);
			this.Controls.Add(this.chkSDmEnable);
			this.Controls.Add(this.chkSDmMod);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmData_SDm";
			this.Text = "frmData_SDm";
			((System.ComponentModel.ISupportInitialize)(this.dgridSDm)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkSDmMod;
        private System.Windows.Forms.CheckBox chkSDmEnable;
        private System.Windows.Forms.DataGridView dgridSDm;
        private System.Windows.Forms.Timer tmrDm;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnDmNum;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnSDmName;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnSDmValue;
	}
}