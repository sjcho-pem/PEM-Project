namespace H_MarsSSD
{
    partial class frmBarcode
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
			this.lbInsertLotID = new System.Windows.Forms.Label();
			this.tbLotID = new System.Windows.Forms.TextBox();
			this.lbInsertLotCount = new System.Windows.Forms.Label();
			this.tbProdCnt = new System.Windows.Forms.TextBox();
			this.gbLDLift = new System.Windows.Forms.GroupBox();
			this.lbModelNameBtm = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.lbType = new System.Windows.Forms.Label();
			this.lbModelNameTop = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lbWLotID = new System.Windows.Forms.Label();
			this.lbWLotCnt = new System.Windows.Forms.Label();
			this.lbLDLiftCount = new System.Windows.Forms.Label();
			this.lbLDLiftID = new System.Windows.Forms.Label();
			this.gbInsertLot = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.tbDeviceIDBtm = new System.Windows.Forms.TextBox();
			this.cbType = new System.Windows.Forms.ComboBox();
			this.label20 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tbDeviceIDTop = new System.Windows.Forms.TextBox();
			this.chkStartUsed = new System.Windows.Forms.CheckBox();
			this.btnLotStart = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.gbLDLift.SuspendLayout();
			this.gbInsertLot.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbInsertLotID
			// 
			this.lbInsertLotID.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.lbInsertLotID.Location = new System.Drawing.Point(11, 38);
			this.lbInsertLotID.Name = "lbInsertLotID";
			this.lbInsertLotID.Size = new System.Drawing.Size(99, 25);
			this.lbInsertLotID.TabIndex = 0;
			this.lbInsertLotID.Text = "W/O";
			this.lbInsertLotID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbLotID
			// 
			this.tbLotID.Location = new System.Drawing.Point(11, 67);
			this.tbLotID.Name = "tbLotID";
			this.tbLotID.Size = new System.Drawing.Size(411, 35);
			this.tbLotID.TabIndex = 1;
			this.tbLotID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.tbLotID.Click += new System.EventHandler(this.TextBox_Click);
			// 
			// lbInsertLotCount
			// 
			this.lbInsertLotCount.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.lbInsertLotCount.Location = new System.Drawing.Point(11, 329);
			this.lbInsertLotCount.Name = "lbInsertLotCount";
			this.lbInsertLotCount.Size = new System.Drawing.Size(139, 25);
			this.lbInsertLotCount.TabIndex = 2;
			this.lbInsertLotCount.Text = "QUANTITY";
			this.lbInsertLotCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbProdCnt
			// 
			this.tbProdCnt.Enabled = false;
			this.tbProdCnt.Location = new System.Drawing.Point(11, 362);
			this.tbProdCnt.Name = "tbProdCnt";
			this.tbProdCnt.Size = new System.Drawing.Size(179, 35);
			this.tbProdCnt.TabIndex = 4;
			this.tbProdCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.tbProdCnt.Click += new System.EventHandler(this.TextBox_Click);
			// 
			// gbLDLift
			// 
			this.gbLDLift.Controls.Add(this.lbModelNameBtm);
			this.gbLDLift.Controls.Add(this.label12);
			this.gbLDLift.Controls.Add(this.button1);
			this.gbLDLift.Controls.Add(this.label6);
			this.gbLDLift.Controls.Add(this.lbType);
			this.gbLDLift.Controls.Add(this.lbModelNameTop);
			this.gbLDLift.Controls.Add(this.label2);
			this.gbLDLift.Controls.Add(this.lbWLotID);
			this.gbLDLift.Controls.Add(this.lbWLotCnt);
			this.gbLDLift.Controls.Add(this.lbLDLiftCount);
			this.gbLDLift.Controls.Add(this.lbLDLiftID);
			this.gbLDLift.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
			this.gbLDLift.Location = new System.Drawing.Point(12, 12);
			this.gbLDLift.Name = "gbLDLift";
			this.gbLDLift.Size = new System.Drawing.Size(280, 415);
			this.gbLDLift.TabIndex = 5;
			this.gbLDLift.TabStop = false;
			this.gbLDLift.Text = "WAITING LOT";
			// 
			// lbModelNameBtm
			// 
			this.lbModelNameBtm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbModelNameBtm.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lbModelNameBtm.Location = new System.Drawing.Point(11, 206);
			this.lbModelNameBtm.Name = "lbModelNameBtm";
			this.lbModelNameBtm.Size = new System.Drawing.Size(256, 38);
			this.lbModelNameBtm.TabIndex = 30;
			this.lbModelNameBtm.Text = "-";
			this.lbModelNameBtm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.label12.Location = new System.Drawing.Point(11, 183);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(199, 21);
			this.label12.TabIndex = 29;
			this.label12.Text = "PROGRAM NAME BOTTOM";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(170, 354);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(97, 43);
			this.button1.TabIndex = 20;
			this.button1.Text = "CLEAR";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.btnClear_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.label6.Location = new System.Drawing.Point(11, 256);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(44, 21);
			this.label6.TabIndex = 19;
			this.label6.Text = "TYPE";
			// 
			// lbType
			// 
			this.lbType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbType.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lbType.Location = new System.Drawing.Point(11, 281);
			this.lbType.Name = "lbType";
			this.lbType.Size = new System.Drawing.Size(256, 38);
			this.lbType.TabIndex = 18;
			this.lbType.Text = "-";
			this.lbType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbModelNameTop
			// 
			this.lbModelNameTop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbModelNameTop.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lbModelNameTop.Location = new System.Drawing.Point(11, 132);
			this.lbModelNameTop.Name = "lbModelNameTop";
			this.lbModelNameTop.Size = new System.Drawing.Size(256, 38);
			this.lbModelNameTop.TabIndex = 13;
			this.lbModelNameTop.Text = "-";
			this.lbModelNameTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.label2.Location = new System.Drawing.Point(11, 109);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(166, 21);
			this.label2.TabIndex = 12;
			this.label2.Text = "PROGRAM NAME TOP";
			// 
			// lbWLotID
			// 
			this.lbWLotID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbWLotID.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lbWLotID.Location = new System.Drawing.Point(11, 60);
			this.lbWLotID.Name = "lbWLotID";
			this.lbWLotID.Size = new System.Drawing.Size(256, 38);
			this.lbWLotID.TabIndex = 11;
			this.lbWLotID.Text = "-";
			this.lbWLotID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbWLotCnt
			// 
			this.lbWLotCnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbWLotCnt.Font = new System.Drawing.Font("Segoe UI", 10F);
			this.lbWLotCnt.Location = new System.Drawing.Point(11, 359);
			this.lbWLotCnt.Name = "lbWLotCnt";
			this.lbWLotCnt.Size = new System.Drawing.Size(117, 38);
			this.lbWLotCnt.TabIndex = 10;
			this.lbWLotCnt.Text = "-";
			this.lbWLotCnt.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbLDLiftCount
			// 
			this.lbLDLiftCount.AutoSize = true;
			this.lbLDLiftCount.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.lbLDLiftCount.Location = new System.Drawing.Point(11, 332);
			this.lbLDLiftCount.Name = "lbLDLiftCount";
			this.lbLDLiftCount.Size = new System.Drawing.Size(84, 21);
			this.lbLDLiftCount.TabIndex = 9;
			this.lbLDLiftCount.Text = "QUANTITY";
			// 
			// lbLDLiftID
			// 
			this.lbLDLiftID.AutoSize = true;
			this.lbLDLiftID.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.lbLDLiftID.Location = new System.Drawing.Point(11, 39);
			this.lbLDLiftID.Name = "lbLDLiftID";
			this.lbLDLiftID.Size = new System.Drawing.Size(43, 21);
			this.lbLDLiftID.TabIndex = 8;
			this.lbLDLiftID.Text = "W/O";
			// 
			// gbInsertLot
			// 
			this.gbInsertLot.Controls.Add(this.label4);
			this.gbInsertLot.Controls.Add(this.tbDeviceIDBtm);
			this.gbInsertLot.Controls.Add(this.cbType);
			this.gbInsertLot.Controls.Add(this.label20);
			this.gbInsertLot.Controls.Add(this.label3);
			this.gbInsertLot.Controls.Add(this.tbDeviceIDTop);
			this.gbInsertLot.Controls.Add(this.tbProdCnt);
			this.gbInsertLot.Controls.Add(this.lbInsertLotID);
			this.gbInsertLot.Controls.Add(this.tbLotID);
			this.gbInsertLot.Controls.Add(this.lbInsertLotCount);
			this.gbInsertLot.Font = new System.Drawing.Font("Segoe UI Black", 15F, System.Drawing.FontStyle.Bold);
			this.gbInsertLot.Location = new System.Drawing.Point(298, 12);
			this.gbInsertLot.Name = "gbInsertLot";
			this.gbInsertLot.Size = new System.Drawing.Size(434, 415);
			this.gbInsertLot.TabIndex = 6;
			this.gbInsertLot.TabStop = false;
			this.gbInsertLot.Text = "INSERT LOT INFO";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.label4.Location = new System.Drawing.Point(11, 179);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(259, 25);
			this.label4.TabIndex = 30;
			this.label4.Text = "PROGRAM NAME BOTTOM";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbDeviceIDBtm
			// 
			this.tbDeviceIDBtm.Location = new System.Drawing.Point(11, 209);
			this.tbDeviceIDBtm.Name = "tbDeviceIDBtm";
			this.tbDeviceIDBtm.Size = new System.Drawing.Size(411, 35);
			this.tbDeviceIDBtm.TabIndex = 3;
			this.tbDeviceIDBtm.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// cbType
			// 
			this.cbType.FormattingEnabled = true;
			this.cbType.Location = new System.Drawing.Point(11, 285);
			this.cbType.Name = "cbType";
			this.cbType.Size = new System.Drawing.Size(411, 36);
			this.cbType.TabIndex = 12;
			// 
			// label20
			// 
			this.label20.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.label20.Location = new System.Drawing.Point(11, 254);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(139, 25);
			this.label20.TabIndex = 10;
			this.label20.Text = "Type";
			this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
			this.label3.Location = new System.Drawing.Point(11, 107);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(216, 25);
			this.label3.TabIndex = 4;
			this.label3.Text = "PROGRAM NAME TOP";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tbDeviceIDTop
			// 
			this.tbDeviceIDTop.Location = new System.Drawing.Point(11, 137);
			this.tbDeviceIDTop.Name = "tbDeviceIDTop";
			this.tbDeviceIDTop.Size = new System.Drawing.Size(411, 35);
			this.tbDeviceIDTop.TabIndex = 2;
			this.tbDeviceIDTop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.tbDeviceIDTop.Click += new System.EventHandler(this.TextBox_Click);
			// 
			// chkStartUsed
			// 
			this.chkStartUsed.AutoSize = true;
			this.chkStartUsed.Location = new System.Drawing.Point(17, 434);
			this.chkStartUsed.Name = "chkStartUsed";
			this.chkStartUsed.Size = new System.Drawing.Size(53, 16);
			this.chkStartUsed.TabIndex = 10;
			this.chkStartUsed.TabStop = false;
			this.chkStartUsed.Text = "Used";
			this.chkStartUsed.UseVisualStyleBackColor = true;
			this.chkStartUsed.CheckedChanged += new System.EventHandler(this.chkStartUsed_CheckedChanged);
			// 
			// btnLotStart
			// 
			this.btnLotStart.Enabled = false;
			this.btnLotStart.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
			this.btnLotStart.Location = new System.Drawing.Point(12, 451);
			this.btnLotStart.Name = "btnLotStart";
			this.btnLotStart.Size = new System.Drawing.Size(146, 59);
			this.btnLotStart.TabIndex = 11;
			this.btnLotStart.TabStop = false;
			this.btnLotStart.Text = "랏 스타트";
			this.btnLotStart.UseVisualStyleBackColor = true;
			this.btnLotStart.Click += new System.EventHandler(this.btnLotStart_Click);
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
			this.btnOK.Location = new System.Drawing.Point(432, 451);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(146, 59);
			this.btnOK.TabIndex = 6;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
			this.btnCancel.Location = new System.Drawing.Point(584, 451);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(146, 59);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "CANCEL";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// frmBarcode
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(746, 523);
			this.ControlBox = false;
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.chkStartUsed);
			this.Controls.Add(this.btnLotStart);
			this.Controls.Add(this.gbInsertLot);
			this.Controls.Add(this.gbLDLift);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "frmBarcode";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "바코드 정보";
			this.TopMost = true;
			this.gbLDLift.ResumeLayout(false);
			this.gbLDLift.PerformLayout();
			this.gbInsertLot.ResumeLayout(false);
			this.gbInsertLot.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbInsertLotID;
        private System.Windows.Forms.TextBox tbLotID;
        private System.Windows.Forms.Label lbInsertLotCount;
        private System.Windows.Forms.TextBox tbProdCnt;
        private System.Windows.Forms.GroupBox gbLDLift;
        private System.Windows.Forms.GroupBox gbInsertLot;
        private System.Windows.Forms.Label lbWLotID;
        private System.Windows.Forms.Label lbWLotCnt;
        private System.Windows.Forms.Label lbLDLiftCount;
        private System.Windows.Forms.Label lbLDLiftID;
        private System.Windows.Forms.Label lbModelNameTop;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDeviceIDTop;
		private System.Windows.Forms.CheckBox chkStartUsed;
		private System.Windows.Forms.Button btnLotStart;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lbType;
		private System.Windows.Forms.ComboBox cbType;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label lbModelNameBtm;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox tbDeviceIDBtm;
	}
}