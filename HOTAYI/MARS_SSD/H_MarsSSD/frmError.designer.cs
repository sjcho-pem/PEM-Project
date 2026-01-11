namespace H_MarsSSD
{
    partial class frmError
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
			this.chkErrorMod = new System.Windows.Forms.CheckBox();
			this.tmrError = new System.Windows.Forms.Timer(this.components);
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.btnErrorUpdate = new System.Windows.Forms.Button();
			this.btnErrorClear = new System.Windows.Forms.Button();
			this.txtImageFilePath = new System.Windows.Forms.TextBox();
			this.cbBuzzer = new System.Windows.Forms.ComboBox();
			this.lbBuzzerSelect = new System.Windows.Forms.Label();
			this.dgridError = new System.Windows.Forms.DataGridView();
			this.tbColumnErrorNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnErrorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnErrorDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.btnOpenFileImage = new System.Windows.Forms.Button();
			this.picErrorImage = new System.Windows.Forms.PictureBox();
			this.gbModify = new System.Windows.Forms.GroupBox();
			this.btnDelete = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgridError)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picErrorImage)).BeginInit();
			this.gbModify.SuspendLayout();
			this.SuspendLayout();
			// 
			// chkErrorMod
			// 
			this.chkErrorMod.BackColor = System.Drawing.Color.Transparent;
			this.chkErrorMod.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkErrorMod.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkErrorMod.Location = new System.Drawing.Point(1234, 618);
			this.chkErrorMod.Name = "chkErrorMod";
			this.chkErrorMod.Size = new System.Drawing.Size(209, 49);
			this.chkErrorMod.TabIndex = 11;
			this.chkErrorMod.Text = "MODIFY MODE";
			this.chkErrorMod.UseVisualStyleBackColor = false;
			this.chkErrorMod.CheckedChanged += new System.EventHandler(this.chkErrorMod_CheckedChanged);
			// 
			// tmrError
			// 
			this.tmrError.Tick += new System.EventHandler(this.tmrError_Tick);
			// 
			// openFileDialog
			// 
			this.openFileDialog.AutoUpgradeEnabled = false;
			this.openFileDialog.FileName = "openFileDialog";
			// 
			// btnErrorUpdate
			// 
			this.btnErrorUpdate.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnErrorUpdate.BackColor = System.Drawing.Color.Transparent;
			this.btnErrorUpdate.Enabled = false;
			this.btnErrorUpdate.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.btnErrorUpdate.Location = new System.Drawing.Point(18, 103);
			this.btnErrorUpdate.Name = "btnErrorUpdate";
			this.btnErrorUpdate.Size = new System.Drawing.Size(107, 84);
			this.btnErrorUpdate.TabIndex = 611;
			this.btnErrorUpdate.Text = "ERROR UPDATE";
			this.btnErrorUpdate.UseVisualStyleBackColor = false;
			this.btnErrorUpdate.Click += new System.EventHandler(this.btnErrorUpdate_Click);
			// 
			// btnErrorClear
			// 
			this.btnErrorClear.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnErrorClear.BackColor = System.Drawing.Color.Transparent;
			this.btnErrorClear.Enabled = false;
			this.btnErrorClear.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.btnErrorClear.Location = new System.Drawing.Point(131, 103);
			this.btnErrorClear.Name = "btnErrorClear";
			this.btnErrorClear.Size = new System.Drawing.Size(107, 84);
			this.btnErrorClear.TabIndex = 612;
			this.btnErrorClear.Text = "ERROR CLEAR";
			this.btnErrorClear.UseVisualStyleBackColor = false;
			this.btnErrorClear.Click += new System.EventHandler(this.btnErrorClear_Click);
			// 
			// txtImageFilePath
			// 
			this.txtImageFilePath.Location = new System.Drawing.Point(12, 848);
			this.txtImageFilePath.Name = "txtImageFilePath";
			this.txtImageFilePath.ReadOnly = true;
			this.txtImageFilePath.Size = new System.Drawing.Size(473, 21);
			this.txtImageFilePath.TabIndex = 615;
			// 
			// cbBuzzer
			// 
			this.cbBuzzer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbBuzzer.Enabled = false;
			this.cbBuzzer.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.cbBuzzer.FormattingEnabled = true;
			this.cbBuzzer.Items.AddRange(new object[] {
            "SOUND 1",
            "SOUND 2",
            "SOUND 3",
            "SOUND 4",
            "SOUND 5",
            "SOUND 6",
            "SOUND 7",
            "SOUND 8",
            "SOUND 9",
            "SOUND 10"});
			this.cbBuzzer.Location = new System.Drawing.Point(18, 57);
			this.cbBuzzer.Name = "cbBuzzer";
			this.cbBuzzer.Size = new System.Drawing.Size(220, 40);
			this.cbBuzzer.TabIndex = 614;
			this.cbBuzzer.SelectedIndexChanged += new System.EventHandler(this.cbBuzzer_SelectedIndexChanged);
			// 
			// lbBuzzerSelect
			// 
			this.lbBuzzerSelect.Enabled = false;
			this.lbBuzzerSelect.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
			this.lbBuzzerSelect.Location = new System.Drawing.Point(18, 22);
			this.lbBuzzerSelect.Name = "lbBuzzerSelect";
			this.lbBuzzerSelect.Size = new System.Drawing.Size(220, 32);
			this.lbBuzzerSelect.TabIndex = 615;
			this.lbBuzzerSelect.Text = "BUZZER SELECT";
			this.lbBuzzerSelect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dgridError
			// 
			this.dgridError.AllowUserToAddRows = false;
			this.dgridError.AllowUserToDeleteRows = false;
			this.dgridError.AllowUserToResizeColumns = false;
			this.dgridError.AllowUserToResizeRows = false;
			this.dgridError.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridError.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgridError.ColumnHeadersHeight = 40;
			this.dgridError.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridError.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnErrorNum,
            this.tbColumnErrorName,
            this.tbColumnErrorDescription});
			this.dgridError.EnableHeadersVisualStyles = false;
			this.dgridError.Location = new System.Drawing.Point(12, 12);
			this.dgridError.MultiSelect = false;
			this.dgridError.Name = "dgridError";
			this.dgridError.RowHeadersVisible = false;
			this.dgridError.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridError.RowTemplate.Height = 30;
			this.dgridError.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgridError.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridError.Size = new System.Drawing.Size(1433, 600);
			this.dgridError.TabIndex = 617;
			this.dgridError.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridError_CellClick);
			this.dgridError.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridError_CellValueChanged);
			this.dgridError.SelectionChanged += new System.EventHandler(this.dgridError_SelectionChanged);
			// 
			// tbColumnErrorNum
			// 
			this.tbColumnErrorNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnErrorNum.DefaultCellStyle = dataGridViewCellStyle2;
			this.tbColumnErrorNum.HeaderText = "NO";
			this.tbColumnErrorNum.Name = "tbColumnErrorNum";
			this.tbColumnErrorNum.ReadOnly = true;
			this.tbColumnErrorNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnErrorNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnErrorNum.Width = 80;
			// 
			// tbColumnErrorName
			// 
			this.tbColumnErrorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.tbColumnErrorName.HeaderText = "ALARM NAME";
			this.tbColumnErrorName.Name = "tbColumnErrorName";
			this.tbColumnErrorName.ReadOnly = true;
			this.tbColumnErrorName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnErrorName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnErrorName.Width = 400;
			// 
			// tbColumnErrorDescription
			// 
			this.tbColumnErrorDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.tbColumnErrorDescription.HeaderText = "ALARM DESCRIPTION";
			this.tbColumnErrorDescription.Name = "tbColumnErrorDescription";
			this.tbColumnErrorDescription.ReadOnly = true;
			this.tbColumnErrorDescription.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnErrorDescription.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// btnOpenFileImage
			// 
			this.btnOpenFileImage.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnOpenFileImage.BackColor = System.Drawing.Color.Transparent;
			this.btnOpenFileImage.BackgroundImage = global::H_MarsSSD.Properties.Resources.FolderSearch_24p;
			this.btnOpenFileImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnOpenFileImage.Enabled = false;
			this.btnOpenFileImage.Font = new System.Drawing.Font("휴먼둥근헤드라인", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnOpenFileImage.Location = new System.Drawing.Point(491, 848);
			this.btnOpenFileImage.Name = "btnOpenFileImage";
			this.btnOpenFileImage.Size = new System.Drawing.Size(42, 24);
			this.btnOpenFileImage.TabIndex = 616;
			this.btnOpenFileImage.UseVisualStyleBackColor = false;
			this.btnOpenFileImage.Click += new System.EventHandler(this.btnOpenFileImage_Click);
			// 
			// picErrorImage
			// 
			this.picErrorImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.picErrorImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picErrorImage.Location = new System.Drawing.Point(12, 618);
			this.picErrorImage.Name = "picErrorImage";
			this.picErrorImage.Size = new System.Drawing.Size(569, 224);
			this.picErrorImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.picErrorImage.TabIndex = 614;
			this.picErrorImage.TabStop = false;
			this.picErrorImage.DoubleClick += new System.EventHandler(this.picErrorImage_DoubleClick);
			// 
			// gbModify
			// 
			this.gbModify.Controls.Add(this.btnErrorUpdate);
			this.gbModify.Controls.Add(this.btnErrorClear);
			this.gbModify.Controls.Add(this.cbBuzzer);
			this.gbModify.Controls.Add(this.lbBuzzerSelect);
			this.gbModify.Font = new System.Drawing.Font("휴먼둥근헤드라인", 9F);
			this.gbModify.Location = new System.Drawing.Point(1191, 673);
			this.gbModify.Name = "gbModify";
			this.gbModify.Size = new System.Drawing.Size(252, 199);
			this.gbModify.TabIndex = 618;
			this.gbModify.TabStop = false;
			// 
			// btnDelete
			// 
			this.btnDelete.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnDelete.BackColor = System.Drawing.Color.Transparent;
			this.btnDelete.BackgroundImage = global::H_MarsSSD.Properties.Resources.Delete_24;
			this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnDelete.Enabled = false;
			this.btnDelete.Font = new System.Drawing.Font("휴먼둥근헤드라인", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnDelete.Location = new System.Drawing.Point(539, 848);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(42, 24);
			this.btnDelete.TabIndex = 619;
			this.btnDelete.UseVisualStyleBackColor = false;
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// frmError
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 881);
			this.Controls.Add(this.btnDelete);
			this.Controls.Add(this.gbModify);
			this.Controls.Add(this.dgridError);
			this.Controls.Add(this.btnOpenFileImage);
			this.Controls.Add(this.txtImageFilePath);
			this.Controls.Add(this.picErrorImage);
			this.Controls.Add(this.chkErrorMod);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmError";
			this.Text = "frmData_Error";
			this.Load += new System.EventHandler(this.frmData_Error_Load);
			((System.ComponentModel.ISupportInitialize)(this.dgridError)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picErrorImage)).EndInit();
			this.gbModify.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkErrorMod;
        public System.Windows.Forms.Timer tmrError;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button btnErrorUpdate;
        private System.Windows.Forms.Button btnErrorClear;
        private System.Windows.Forms.PictureBox picErrorImage;
        private System.Windows.Forms.TextBox txtImageFilePath;
        private System.Windows.Forms.Button btnOpenFileImage;
        private System.Windows.Forms.ComboBox cbBuzzer;
        private System.Windows.Forms.Label lbBuzzerSelect;
        private System.Windows.Forms.DataGridView dgridError;
        private System.Windows.Forms.GroupBox gbModify;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnErrorNum;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnErrorName;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnErrorDescription;
		private System.Windows.Forms.Button btnDelete;
	}
}