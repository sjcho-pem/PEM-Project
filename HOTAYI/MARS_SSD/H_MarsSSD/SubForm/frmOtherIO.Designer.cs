namespace H_MarsSSD
{
    partial class frmOtherIO
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
			this.chkOutputEnable = new System.Windows.Forms.CheckBox();
			this.chkIOModify = new System.Windows.Forms.CheckBox();
			this.cbSelOutput = new System.Windows.Forms.ComboBox();
			this.cbSelInput = new System.Windows.Forms.ComboBox();
			this.dgridInput = new System.Windows.Forms.DataGridView();
			this.tbColumnInPutNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnInPutName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnInPutOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnOutPutNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnOutPutName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnOutPutOn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgridOutput = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.dgridInput)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridOutput)).BeginInit();
			this.SuspendLayout();
			// 
			// chkOutputEnable
			// 
			this.chkOutputEnable.BackColor = System.Drawing.Color.Transparent;
			this.chkOutputEnable.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkOutputEnable.ForeColor = System.Drawing.SystemColors.WindowText;
			this.chkOutputEnable.Location = new System.Drawing.Point(431, 47);
			this.chkOutputEnable.Name = "chkOutputEnable";
			this.chkOutputEnable.Size = new System.Drawing.Size(290, 40);
			this.chkOutputEnable.TabIndex = 13;
			this.chkOutputEnable.Text = "OUTPUT ENABLE";
			this.chkOutputEnable.UseVisualStyleBackColor = false;
			this.chkOutputEnable.CheckedChanged += new System.EventHandler(this.chkOutputEnable_CheckedChanged);
			// 
			// chkIOModify
			// 
			this.chkIOModify.BackColor = System.Drawing.Color.Transparent;
			this.chkIOModify.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkIOModify.ForeColor = System.Drawing.SystemColors.WindowText;
			this.chkIOModify.Location = new System.Drawing.Point(9, 48);
			this.chkIOModify.Name = "chkIOModify";
			this.chkIOModify.Size = new System.Drawing.Size(234, 39);
			this.chkIOModify.TabIndex = 12;
			this.chkIOModify.Text = "MODIFY MODE";
			this.chkIOModify.UseVisualStyleBackColor = false;
			this.chkIOModify.CheckedChanged += new System.EventHandler(this.chkIOModify_CheckedChanged);
			// 
			// cbSelOutput
			// 
			this.cbSelOutput.DisplayMember = "Text";
			this.cbSelOutput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSelOutput.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.cbSelOutput.FormattingEnabled = true;
			this.cbSelOutput.ItemHeight = 32;
			this.cbSelOutput.Location = new System.Drawing.Point(431, 4);
			this.cbSelOutput.MaxDropDownItems = 10;
			this.cbSelOutput.Name = "cbSelOutput";
			this.cbSelOutput.Size = new System.Drawing.Size(290, 40);
			this.cbSelOutput.Sorted = true;
			this.cbSelOutput.TabIndex = 11;
			this.cbSelOutput.SelectedValueChanged += new System.EventHandler(this.cbSelOutput_SelectedValueChanged);
			// 
			// cbSelInput
			// 
			this.cbSelInput.DisplayMember = "Text";
			this.cbSelInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbSelInput.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.cbSelInput.FormattingEnabled = true;
			this.cbSelInput.ItemHeight = 32;
			this.cbSelInput.Location = new System.Drawing.Point(9, 4);
			this.cbSelInput.MaxDropDownItems = 10;
			this.cbSelInput.Name = "cbSelInput";
			this.cbSelInput.Size = new System.Drawing.Size(290, 40);
			this.cbSelInput.Sorted = true;
			this.cbSelInput.TabIndex = 9;
			this.cbSelInput.SelectedIndexChanged += new System.EventHandler(this.cbSelInput_SelectedIndexChanged);
			// 
			// dgridInput
			// 
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridInput.DefaultCellStyle = dataGridViewCellStyle1;
			this.dgridInput.Location = new System.Drawing.Point(9, 96);
			this.dgridInput.Name = "dgridInput";
			this.dgridInput.Size = new System.Drawing.Size(416, 368);
			this.dgridInput.TabIndex = 14;
			this.dgridInput.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridInput_CellDoubleClick);
			this.dgridInput.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridInput_CellValueChanged);
			// 
			// tbColumnInPutNum
			// 
			this.tbColumnInPutNum.Name = "tbColumnInPutNum";
			// 
			// tbColumnInPutName
			// 
			this.tbColumnInPutName.Name = "tbColumnInPutName";
			// 
			// tbColumnInPutOn
			// 
			this.tbColumnInPutOn.Name = "tbColumnInPutOn";
			// 
			// tbColumnOutPutNum
			// 
			this.tbColumnOutPutNum.Name = "tbColumnOutPutNum";
			// 
			// tbColumnOutPutName
			// 
			this.tbColumnOutPutName.Name = "tbColumnOutPutName";
			// 
			// tbColumnOutPutOn
			// 
			this.tbColumnOutPutOn.Name = "tbColumnOutPutOn";
			// 
			// dgridOutput
			// 
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridOutput.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgridOutput.Location = new System.Drawing.Point(433, 96);
			this.dgridOutput.Name = "dgridOutput";
			this.dgridOutput.RowTemplate.Height = 23;
			this.dgridOutput.Size = new System.Drawing.Size(415, 368);
			this.dgridOutput.TabIndex = 15;
			this.dgridOutput.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridOutput_CellDoubleClick);
			this.dgridOutput.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridOutput_CellValueChanged);
			// 
			// frmOtherIO
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(857, 476);
			this.Controls.Add(this.dgridOutput);
			this.Controls.Add(this.dgridInput);
			this.Controls.Add(this.chkOutputEnable);
			this.Controls.Add(this.chkIOModify);
			this.Controls.Add(this.cbSelOutput);
			this.Controls.Add(this.cbSelInput);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOtherIO";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Input / Output";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmOtherIO_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.dgridInput)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridOutput)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkOutputEnable;
        private System.Windows.Forms.CheckBox chkIOModify;
        private System.Windows.Forms.ComboBox cbSelOutput;
        private System.Windows.Forms.ComboBox cbSelInput;
        private System.Windows.Forms.DataGridView dgridInput;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnInPutNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnInPutName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnInPutOn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnOutPutNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnOutPutName;
        private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnOutPutOn;
        private System.Windows.Forms.DataGridView dgridOutput;
    }
}