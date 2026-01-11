namespace H_MarsSSD
{
    partial class frmData_Bit
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
			this.dgridBit = new System.Windows.Forms.DataGridView();
			this.tbColumnBitNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnBitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnBitStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.chkBitEnable = new System.Windows.Forms.CheckBox();
			this.chkBitMod = new System.Windows.Forms.CheckBox();
			this.cbSelBit = new System.Windows.Forms.ComboBox();
			this.tmrBit = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.dgridBit)).BeginInit();
			this.SuspendLayout();
			// 
			// dgridBit
			// 
			this.dgridBit.AllowUserToAddRows = false;
			this.dgridBit.AllowUserToDeleteRows = false;
			this.dgridBit.AllowUserToResizeColumns = false;
			this.dgridBit.AllowUserToResizeRows = false;
			this.dgridBit.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridBit.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgridBit.ColumnHeadersHeight = 40;
			this.dgridBit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridBit.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnBitNum,
            this.tbColumnBitName,
            this.tbColumnBitStatus});
			this.dgridBit.EnableHeadersVisualStyles = false;
			this.dgridBit.Location = new System.Drawing.Point(12, 121);
			this.dgridBit.MultiSelect = false;
			this.dgridBit.Name = "dgridBit";
			this.dgridBit.RowHeadersVisible = false;
			this.dgridBit.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridBit.RowTemplate.Height = 35;
			this.dgridBit.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridBit.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dgridBit.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridBit.Size = new System.Drawing.Size(700, 650);
			this.dgridBit.TabIndex = 15;
			this.dgridBit.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridBit_CellDoubleClick);
			this.dgridBit.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridBit_CellValueChanged);
			this.dgridBit.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridBit_ColumnHeaderMouseDoubleClick);
			// 
			// tbColumnBitNum
			// 
			this.tbColumnBitNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnBitNum.DefaultCellStyle = dataGridViewCellStyle2;
			this.tbColumnBitNum.HeaderText = "NO";
			this.tbColumnBitNum.Name = "tbColumnBitNum";
			this.tbColumnBitNum.ReadOnly = true;
			this.tbColumnBitNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnBitNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnBitNum.Width = 90;
			// 
			// tbColumnBitName
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.tbColumnBitName.DefaultCellStyle = dataGridViewCellStyle3;
			this.tbColumnBitName.HeaderText = "BIT NAME";
			this.tbColumnBitName.Name = "tbColumnBitName";
			this.tbColumnBitName.ReadOnly = true;
			this.tbColumnBitName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnBitName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnBitName.Width = 430;
			// 
			// tbColumnBitStatus
			// 
			this.tbColumnBitStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnBitStatus.DefaultCellStyle = dataGridViewCellStyle4;
			this.tbColumnBitStatus.HeaderText = "STATUS";
			this.tbColumnBitStatus.Name = "tbColumnBitStatus";
			this.tbColumnBitStatus.ReadOnly = true;
			this.tbColumnBitStatus.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnBitStatus.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnBitStatus.Width = 180;
			// 
			// chkBitEnable
			// 
			this.chkBitEnable.BackColor = System.Drawing.Color.Transparent;
			this.chkBitEnable.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkBitEnable.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkBitEnable.Location = new System.Drawing.Point(239, 66);
			this.chkBitEnable.Name = "chkBitEnable";
			this.chkBitEnable.Size = new System.Drawing.Size(160, 49);
			this.chkBitEnable.TabIndex = 14;
			this.chkBitEnable.Text = "ENABLE";
			this.chkBitEnable.UseVisualStyleBackColor = false;
			this.chkBitEnable.CheckedChanged += new System.EventHandler(this.chkBitEnable_CheckedChanged);
			// 
			// chkBitMod
			// 
			this.chkBitMod.BackColor = System.Drawing.Color.Transparent;
			this.chkBitMod.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkBitMod.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkBitMod.Location = new System.Drawing.Point(14, 66);
			this.chkBitMod.Name = "chkBitMod";
			this.chkBitMod.Size = new System.Drawing.Size(210, 49);
			this.chkBitMod.TabIndex = 13;
			this.chkBitMod.Text = "NAME MODIFY";
			this.chkBitMod.UseVisualStyleBackColor = false;
			this.chkBitMod.CheckedChanged += new System.EventHandler(this.chkBitMod_CheckedChanged);
			// 
			// cbSelBit
			// 
			this.cbSelBit.BackColor = System.Drawing.SystemColors.Window;
			this.cbSelBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSelBit.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.cbSelBit.ForeColor = System.Drawing.SystemColors.MenuText;
			this.cbSelBit.ItemHeight = 37;
			this.cbSelBit.Items.AddRange(new object[] {
            "BIT CH : 00"});
			this.cbSelBit.Location = new System.Drawing.Point(12, 12);
			this.cbSelBit.MaxDropDownItems = 10;
			this.cbSelBit.Name = "cbSelBit";
			this.cbSelBit.Size = new System.Drawing.Size(301, 45);
			this.cbSelBit.Sorted = true;
			this.cbSelBit.TabIndex = 12;
			this.cbSelBit.SelectedIndexChanged += new System.EventHandler(this.cbSelBit_SelectedIndexChanged);
			// 
			// tmrBit
			// 
			this.tmrBit.Tick += new System.EventHandler(this.tmrBit_Tick);
			// 
			// frmData_Bit
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 816);
			this.Controls.Add(this.dgridBit);
			this.Controls.Add(this.chkBitEnable);
			this.Controls.Add(this.chkBitMod);
			this.Controls.Add(this.cbSelBit);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmData_Bit";
			this.Text = "frmData_Bit";
			((System.ComponentModel.ISupportInitialize)(this.dgridBit)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgridBit;
        private System.Windows.Forms.CheckBox chkBitEnable;
        private System.Windows.Forms.CheckBox chkBitMod;
        private System.Windows.Forms.ComboBox cbSelBit;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnBitNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnBitName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnBitStatus;
        private System.Windows.Forms.Timer tmrBit;
    }
}