namespace H_MarsSSD
{
    partial class frmIO
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.cbSelInput = new System.Windows.Forms.ComboBox();
			this.cbSelOutput = new System.Windows.Forms.ComboBox();
			this.chkIOModify = new System.Windows.Forms.CheckBox();
			this.chkOutputEnable = new System.Windows.Forms.CheckBox();
			this.tmrIO = new System.Windows.Forms.Timer(this.components);
			this.dgridInput = new System.Windows.Forms.DataGridView();
			this.tbColumnInputNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnInputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnInputState = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgridOutput = new System.Windows.Forms.DataGridView();
			this.tbColumnOutputNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnOutputName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnOutputState = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.txtSearchValue = new System.Windows.Forms.TextBox();
			this.pnSearch = new System.Windows.Forms.Panel();
			((System.ComponentModel.ISupportInitialize)(this.dgridInput)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridOutput)).BeginInit();
			this.pnSearch.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbSelInput
			// 
			this.cbSelInput.DisplayMember = "Text";
			this.cbSelInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSelInput.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.cbSelInput.FormattingEnabled = true;
			this.cbSelInput.IntegralHeight = false;
			this.cbSelInput.ItemHeight = 37;
			this.cbSelInput.Location = new System.Drawing.Point(12, 19);
			this.cbSelInput.MaxDropDownItems = 10;
			this.cbSelInput.Name = "cbSelInput";
			this.cbSelInput.Size = new System.Drawing.Size(290, 45);
			this.cbSelInput.Sorted = true;
			this.cbSelInput.TabIndex = 3;
			this.cbSelInput.SelectedIndexChanged += new System.EventHandler(this.cbSelInput_SelectedIndexChanged);
			// 
			// cbSelOutput
			// 
			this.cbSelOutput.DisplayMember = "Text";
			this.cbSelOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSelOutput.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.cbSelOutput.FormattingEnabled = true;
			this.cbSelOutput.IntegralHeight = false;
			this.cbSelOutput.ItemHeight = 37;
			this.cbSelOutput.Location = new System.Drawing.Point(733, 19);
			this.cbSelOutput.MaxDropDownItems = 10;
			this.cbSelOutput.Name = "cbSelOutput";
			this.cbSelOutput.Size = new System.Drawing.Size(290, 45);
			this.cbSelOutput.Sorted = true;
			this.cbSelOutput.TabIndex = 5;
			this.cbSelOutput.SelectedValueChanged += new System.EventHandler(this.cbSelOutput_SelectedValueChanged);
			// 
			// chkIOModify
			// 
			this.chkIOModify.BackColor = System.Drawing.Color.Transparent;
			this.chkIOModify.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkIOModify.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkIOModify.Location = new System.Drawing.Point(12, 96);
			this.chkIOModify.Name = "chkIOModify";
			this.chkIOModify.Size = new System.Drawing.Size(218, 39);
			this.chkIOModify.TabIndex = 6;
			this.chkIOModify.Text = "MODIFY MODE";
			this.chkIOModify.UseVisualStyleBackColor = false;
			this.chkIOModify.CheckedChanged += new System.EventHandler(this.chkIOModify_CheckedChanged);
			// 
			// chkOutputEnable
			// 
			this.chkOutputEnable.BackColor = System.Drawing.Color.Transparent;
			this.chkOutputEnable.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkOutputEnable.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkOutputEnable.Location = new System.Drawing.Point(733, 96);
			this.chkOutputEnable.Name = "chkOutputEnable";
			this.chkOutputEnable.Size = new System.Drawing.Size(290, 40);
			this.chkOutputEnable.TabIndex = 7;
			this.chkOutputEnable.Text = "OUTPUT ENABLE";
			this.chkOutputEnable.UseVisualStyleBackColor = false;
			this.chkOutputEnable.CheckedChanged += new System.EventHandler(this.chkOutputEnable_CheckedChanged);
			// 
			// tmrIO
			// 
			this.tmrIO.Tick += new System.EventHandler(this.tmrIO_Tick);
			// 
			// dgridInput
			// 
			this.dgridInput.AllowUserToAddRows = false;
			this.dgridInput.AllowUserToDeleteRows = false;
			this.dgridInput.AllowUserToResizeColumns = false;
			this.dgridInput.AllowUserToResizeRows = false;
			this.dgridInput.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridInput.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgridInput.ColumnHeadersHeight = 40;
			this.dgridInput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridInput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnInputNum,
            this.tbColumnInputName,
            this.tbColumnInputState});
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridInput.DefaultCellStyle = dataGridViewCellStyle4;
			this.dgridInput.EnableHeadersVisualStyles = false;
			this.dgridInput.Location = new System.Drawing.Point(12, 142);
			this.dgridInput.MultiSelect = false;
			this.dgridInput.Name = "dgridInput";
			this.dgridInput.RowHeadersVisible = false;
			this.dgridInput.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dgridInput.RowsDefaultCellStyle = dataGridViewCellStyle5;
			this.dgridInput.RowTemplate.Height = 38;
			this.dgridInput.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridInput.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dgridInput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dgridInput.Size = new System.Drawing.Size(710, 650);
			this.dgridInput.TabIndex = 8;
			this.dgridInput.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridInput_CellValueChanged);
			this.dgridInput.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridInput_ColumnHeaderMouseDoubleClick);
			this.dgridInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgridInput_KeyPress);
			// 
			// tbColumnInputNum
			// 
			this.tbColumnInputNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnInputNum.DefaultCellStyle = dataGridViewCellStyle2;
			this.tbColumnInputNum.HeaderText = "NO";
			this.tbColumnInputNum.Name = "tbColumnInputNum";
			this.tbColumnInputNum.ReadOnly = true;
			this.tbColumnInputNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnInputNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnInputNum.Width = 60;
			// 
			// tbColumnInputName
			// 
			this.tbColumnInputName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.tbColumnInputName.HeaderText = "INPUT NAME";
			this.tbColumnInputName.Name = "tbColumnInputName";
			this.tbColumnInputName.ReadOnly = true;
			this.tbColumnInputName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnInputName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tbColumnInputState
			// 
			this.tbColumnInputState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnInputState.DefaultCellStyle = dataGridViewCellStyle3;
			this.tbColumnInputState.HeaderText = "ON";
			this.tbColumnInputState.Name = "tbColumnInputState";
			this.tbColumnInputState.ReadOnly = true;
			this.tbColumnInputState.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnInputState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dgridOutput
			// 
			this.dgridOutput.AllowUserToAddRows = false;
			this.dgridOutput.AllowUserToDeleteRows = false;
			this.dgridOutput.AllowUserToResizeColumns = false;
			this.dgridOutput.AllowUserToResizeRows = false;
			this.dgridOutput.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridOutput.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgridOutput.ColumnHeadersHeight = 40;
			this.dgridOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridOutput.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnOutputNum,
            this.tbColumnOutputName,
            this.tbColumnOutputState});
			dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle9.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridOutput.DefaultCellStyle = dataGridViewCellStyle9;
			this.dgridOutput.EnableHeadersVisualStyles = false;
			this.dgridOutput.Location = new System.Drawing.Point(733, 142);
			this.dgridOutput.MultiSelect = false;
			this.dgridOutput.Name = "dgridOutput";
			this.dgridOutput.RowHeadersVisible = false;
			this.dgridOutput.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold);
			this.dgridOutput.RowsDefaultCellStyle = dataGridViewCellStyle10;
			this.dgridOutput.RowTemplate.Height = 38;
			this.dgridOutput.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridOutput.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dgridOutput.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dgridOutput.Size = new System.Drawing.Size(710, 650);
			this.dgridOutput.TabIndex = 9;
			this.dgridOutput.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridOutput_CellDoubleClick);
			this.dgridOutput.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridOutput_CellValueChanged);
			this.dgridOutput.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridOutput_ColumnHeaderMouseDoubleClick);
			this.dgridOutput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dgridOutput_KeyPress);
			// 
			// tbColumnOutputNum
			// 
			this.tbColumnOutputNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnOutputNum.DefaultCellStyle = dataGridViewCellStyle7;
			this.tbColumnOutputNum.HeaderText = "NO";
			this.tbColumnOutputNum.Name = "tbColumnOutputNum";
			this.tbColumnOutputNum.ReadOnly = true;
			this.tbColumnOutputNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnOutputNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnOutputNum.Width = 60;
			// 
			// tbColumnOutputName
			// 
			this.tbColumnOutputName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.tbColumnOutputName.HeaderText = "OUTPUT NAME";
			this.tbColumnOutputName.Name = "tbColumnOutputName";
			this.tbColumnOutputName.ReadOnly = true;
			this.tbColumnOutputName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnOutputName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tbColumnOutputState
			// 
			this.tbColumnOutputState.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnOutputState.DefaultCellStyle = dataGridViewCellStyle8;
			this.tbColumnOutputState.HeaderText = "ON";
			this.tbColumnOutputState.Name = "tbColumnOutputState";
			this.tbColumnOutputState.ReadOnly = true;
			this.tbColumnOutputState.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnOutputState.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// txtSearchValue
			// 
			this.txtSearchValue.Font = new System.Drawing.Font("Segoe UI Semibold", 30F, System.Drawing.FontStyle.Bold);
			this.txtSearchValue.Location = new System.Drawing.Point(10, 10);
			this.txtSearchValue.Margin = new System.Windows.Forms.Padding(10);
			this.txtSearchValue.Name = "txtSearchValue";
			this.txtSearchValue.Size = new System.Drawing.Size(387, 61);
			this.txtSearchValue.TabIndex = 26;
			this.txtSearchValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearchValue_KeyUp);
			// 
			// pnSearch
			// 
			this.pnSearch.BackColor = System.Drawing.Color.CornflowerBlue;
			this.pnSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pnSearch.Controls.Add(this.txtSearchValue);
			this.pnSearch.Location = new System.Drawing.Point(401, 288);
			this.pnSearch.Margin = new System.Windows.Forms.Padding(10);
			this.pnSearch.Name = "pnSearch";
			this.pnSearch.Size = new System.Drawing.Size(410, 87);
			this.pnSearch.TabIndex = 27;
			this.pnSearch.Visible = false;
			// 
			// frmIO
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 881);
			this.Controls.Add(this.pnSearch);
			this.Controls.Add(this.dgridOutput);
			this.Controls.Add(this.dgridInput);
			this.Controls.Add(this.chkOutputEnable);
			this.Controls.Add(this.chkIOModify);
			this.Controls.Add(this.cbSelOutput);
			this.Controls.Add(this.cbSelInput);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmIO";
			this.Text = "frmIO";
			this.VisibleChanged += new System.EventHandler(this.frmIO_VisibleChanged);
			((System.ComponentModel.ISupportInitialize)(this.dgridInput)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridOutput)).EndInit();
			this.pnSearch.ResumeLayout(false);
			this.pnSearch.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cbSelInput;
        private System.Windows.Forms.ComboBox cbSelOutput;
        private System.Windows.Forms.CheckBox chkIOModify;
        private System.Windows.Forms.CheckBox chkOutputEnable;
        private System.Windows.Forms.Timer tmrIO;
        private System.Windows.Forms.DataGridView dgridInput;
        private System.Windows.Forms.DataGridView dgridOutput;
        private System.Windows.Forms.TextBox txtSearchValue;
        private System.Windows.Forms.Panel pnSearch;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnInputNum;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnInputName;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnInputState;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnOutputNum;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnOutputName;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnOutputState;
	}
}