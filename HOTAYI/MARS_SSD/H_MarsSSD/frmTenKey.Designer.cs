namespace H_MarsSSD
{
    partial class frmTenKey
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.dgridManual = new System.Windows.Forms.DataGridView();
			this.tbColumnTenKeyNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnTenKeyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgridBTNTenKeyRun = new System.Windows.Forms.DataGridViewButtonColumn();
			this.chkOperation = new System.Windows.Forms.CheckBox();
			this.chkNameModify = new System.Windows.Forms.CheckBox();
			this.lbSelectManual = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.dgridManual)).BeginInit();
			this.SuspendLayout();
			// 
			// dgridManual
			// 
			this.dgridManual.AllowUserToAddRows = false;
			this.dgridManual.AllowUserToDeleteRows = false;
			this.dgridManual.AllowUserToResizeColumns = false;
			this.dgridManual.AllowUserToResizeRows = false;
			this.dgridManual.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridManual.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgridManual.ColumnHeadersHeight = 40;
			this.dgridManual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridManual.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnTenKeyNum,
            this.tbColumnTenKeyName,
            this.dgridBTNTenKeyRun});
			this.dgridManual.EnableHeadersVisualStyles = false;
			this.dgridManual.Location = new System.Drawing.Point(12, 142);
			this.dgridManual.MultiSelect = false;
			this.dgridManual.Name = "dgridManual";
			this.dgridManual.RowHeadersVisible = false;
			this.dgridManual.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridManual.RowTemplate.Height = 35;
			this.dgridManual.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridManual.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgridManual.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridManual.Size = new System.Drawing.Size(1439, 666);
			this.dgridManual.TabIndex = 26;
			this.dgridManual.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridManual_CellClick);
			this.dgridManual.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridManual_CellValueChanged);
			this.dgridManual.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridManual_ColumnHeaderMouseDoubleClick);
			// 
			// tbColumnTenKeyNum
			// 
			this.tbColumnTenKeyNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnTenKeyNum.DefaultCellStyle = dataGridViewCellStyle2;
			this.tbColumnTenKeyNum.HeaderText = "NO";
			this.tbColumnTenKeyNum.Name = "tbColumnTenKeyNum";
			this.tbColumnTenKeyNum.ReadOnly = true;
			this.tbColumnTenKeyNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnTenKeyNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnTenKeyNum.Width = 90;
			// 
			// tbColumnTenKeyName
			// 
			this.tbColumnTenKeyName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.tbColumnTenKeyName.DefaultCellStyle = dataGridViewCellStyle3;
			this.tbColumnTenKeyName.HeaderText = "MANUAL OPERATION NAME";
			this.tbColumnTenKeyName.Name = "tbColumnTenKeyName";
			this.tbColumnTenKeyName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnTenKeyName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnTenKeyName.Width = 1062;
			// 
			// dgridBTNTenKeyRun
			// 
			this.dgridBTNTenKeyRun.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.dgridBTNTenKeyRun.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgridBTNTenKeyRun.HeaderText = "ACTION";
			this.dgridBTNTenKeyRun.Name = "dgridBTNTenKeyRun";
			this.dgridBTNTenKeyRun.ReadOnly = true;
			this.dgridBTNTenKeyRun.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridBTNTenKeyRun.Text = "RUN";
			this.dgridBTNTenKeyRun.UseColumnTextForButtonValue = true;
			this.dgridBTNTenKeyRun.Width = 265;
			// 
			// chkOperation
			// 
			this.chkOperation.BackColor = System.Drawing.Color.Transparent;
			this.chkOperation.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkOperation.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkOperation.Location = new System.Drawing.Point(247, 90);
			this.chkOperation.Name = "chkOperation";
			this.chkOperation.Size = new System.Drawing.Size(260, 49);
			this.chkOperation.TabIndex = 28;
			this.chkOperation.Text = "OPERATION MODE";
			this.chkOperation.UseVisualStyleBackColor = false;
			this.chkOperation.CheckedChanged += new System.EventHandler(this.chkOperation_CheckedChanged);
			// 
			// chkNameModify
			// 
			this.chkNameModify.BackColor = System.Drawing.Color.Transparent;
			this.chkNameModify.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkNameModify.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkNameModify.Location = new System.Drawing.Point(12, 90);
			this.chkNameModify.Name = "chkNameModify";
			this.chkNameModify.Size = new System.Drawing.Size(215, 49);
			this.chkNameModify.TabIndex = 3;
			this.chkNameModify.Text = "NAME MODIFY";
			this.chkNameModify.UseVisualStyleBackColor = false;
			this.chkNameModify.CheckedChanged += new System.EventHandler(this.chkNameModify_CheckedChanged);
			// 
			// lbSelectManual
			// 
			this.lbSelectManual.BackColor = System.Drawing.Color.Black;
			this.lbSelectManual.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbSelectManual.Font = new System.Drawing.Font("Segoe UI", 38F, System.Drawing.FontStyle.Bold);
			this.lbSelectManual.ForeColor = System.Drawing.Color.Khaki;
			this.lbSelectManual.Location = new System.Drawing.Point(12, 9);
			this.lbSelectManual.Name = "lbSelectManual";
			this.lbSelectManual.Size = new System.Drawing.Size(125, 80);
			this.lbSelectManual.TabIndex = 27;
			this.lbSelectManual.Text = "000";
			this.lbSelectManual.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmTenKey
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 820);
			this.Controls.Add(this.chkOperation);
			this.Controls.Add(this.lbSelectManual);
			this.Controls.Add(this.dgridManual);
			this.Controls.Add(this.chkNameModify);
			this.ForeColor = System.Drawing.SystemColors.Desktop;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmTenKey";
			this.VisibleChanged += new System.EventHandler(this.frmManual_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.dgridManual)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dgridManual;
        private System.Windows.Forms.CheckBox chkOperation;
        private System.Windows.Forms.CheckBox chkNameModify;
        private System.Windows.Forms.Label lbSelectManual;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnTenKeyNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnTenKeyName;
        private System.Windows.Forms.DataGridViewButtonColumn dgridBTNTenKeyRun;
    }
}