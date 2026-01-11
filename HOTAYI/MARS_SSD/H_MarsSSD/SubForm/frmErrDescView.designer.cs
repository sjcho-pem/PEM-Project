namespace H_MarsSSD
{
    partial class frmErrDescView
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			this.richtxtErrDesc = new System.Windows.Forms.RichTextBox();
			this.lbErrCode = new System.Windows.Forms.Label();
			this.gridErrList = new System.Windows.Forms.DataGridView();
			this.No = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ErrorName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.picErrorImage = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.gridErrList)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.picErrorImage)).BeginInit();
			this.SuspendLayout();
			// 
			// richtxtErrDesc
			// 
			this.richtxtErrDesc.BackColor = System.Drawing.Color.WhiteSmoke;
			this.richtxtErrDesc.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.richtxtErrDesc.Location = new System.Drawing.Point(9, 56);
			this.richtxtErrDesc.Name = "richtxtErrDesc";
			this.richtxtErrDesc.ReadOnly = true;
			this.richtxtErrDesc.Size = new System.Drawing.Size(696, 150);
			this.richtxtErrDesc.TabIndex = 0;
			this.richtxtErrDesc.Text = "";
			// 
			// lbErrCode
			// 
			this.lbErrCode.BackColor = System.Drawing.Color.Transparent;
			this.lbErrCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbErrCode.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
			this.lbErrCode.ForeColor = System.Drawing.Color.DarkRed;
			this.lbErrCode.Location = new System.Drawing.Point(9, 10);
			this.lbErrCode.Name = "lbErrCode";
			this.lbErrCode.Size = new System.Drawing.Size(696, 40);
			this.lbErrCode.TabIndex = 3;
			this.lbErrCode.Text = "ERROR CODE";
			this.lbErrCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// gridErrList
			// 
			this.gridErrList.AllowUserToAddRows = false;
			this.gridErrList.AllowUserToResizeColumns = false;
			this.gridErrList.AllowUserToResizeRows = false;
			dataGridViewCellStyle3.BackColor = System.Drawing.Color.Gainsboro;
			this.gridErrList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
			this.gridErrList.BackgroundColor = System.Drawing.Color.WhiteSmoke;
			this.gridErrList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.gridErrList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Sunken;
			this.gridErrList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle4.BackColor = System.Drawing.Color.DimGray;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.gridErrList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
			this.gridErrList.ColumnHeadersHeight = 35;
			this.gridErrList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.gridErrList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.No,
            this.Code,
            this.ErrorName});
			this.gridErrList.EnableHeadersVisualStyles = false;
			this.gridErrList.Location = new System.Drawing.Point(9, 212);
			this.gridErrList.MultiSelect = false;
			this.gridErrList.Name = "gridErrList";
			this.gridErrList.ReadOnly = true;
			this.gridErrList.RowHeadersVisible = false;
			this.gridErrList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.gridErrList.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.gridErrList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("굴림", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.gridErrList.RowTemplate.DividerHeight = 1;
			this.gridErrList.RowTemplate.Height = 30;
			this.gridErrList.RowTemplate.ReadOnly = true;
			this.gridErrList.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.gridErrList.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.gridErrList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.gridErrList.Size = new System.Drawing.Size(696, 336);
			this.gridErrList.TabIndex = 31;
			this.gridErrList.SelectionChanged += new System.EventHandler(this.gridErrList_SelectionChanged);
			// 
			// No
			// 
			this.No.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.No.DividerWidth = 1;
			this.No.HeaderText = "NO";
			this.No.Name = "No";
			this.No.ReadOnly = true;
			this.No.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.No.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.No.Width = 70;
			// 
			// Code
			// 
			this.Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.Code.DividerWidth = 1;
			this.Code.HeaderText = "CODE";
			this.Code.Name = "Code";
			this.Code.ReadOnly = true;
			this.Code.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.Code.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.Code.Width = 110;
			// 
			// ErrorName
			// 
			this.ErrorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.ErrorName.DividerWidth = 1;
			this.ErrorName.HeaderText = "ERROR NAME";
			this.ErrorName.Name = "ErrorName";
			this.ErrorName.ReadOnly = true;
			this.ErrorName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ErrorName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.ErrorName.Width = 516;
			// 
			// picErrorImage
			// 
			this.picErrorImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.picErrorImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.picErrorImage.Location = new System.Drawing.Point(9, 554);
			this.picErrorImage.Name = "picErrorImage";
			this.picErrorImage.Size = new System.Drawing.Size(696, 274);
			this.picErrorImage.TabIndex = 615;
			this.picErrorImage.TabStop = false;
			this.picErrorImage.DoubleClick += new System.EventHandler(this.picErrorImage_DoubleClick);
			// 
			// frmErrDescView
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(715, 832);
			this.Controls.Add(this.picErrorImage);
			this.Controls.Add(this.gridErrList);
			this.Controls.Add(this.lbErrCode);
			this.Controls.Add(this.richtxtErrDesc);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmErrDescView";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Error Description View";
			this.TopMost = true;
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmErrDescView_FormClosed);
			this.Load += new System.EventHandler(this.frmErrDescView_Load);
			((System.ComponentModel.ISupportInitialize)(this.gridErrList)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.picErrorImage)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richtxtErrDesc;
        private System.Windows.Forms.Label lbErrCode;
        private System.Windows.Forms.DataGridView gridErrList;
        private System.Windows.Forms.DataGridViewTextBoxColumn No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn ErrorName;
        private System.Windows.Forms.PictureBox picErrorImage;
    }
}