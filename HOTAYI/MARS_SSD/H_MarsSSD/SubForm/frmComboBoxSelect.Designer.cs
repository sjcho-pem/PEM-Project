namespace H_MarsSSD
{
	partial class frmComboBoxSelect
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
			this.cbFlowMapList = new System.Windows.Forms.ComboBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lbX_Title = new System.Windows.Forms.Label();
			this.tmrSuggest = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// cbFlowMapList
			// 
			this.cbFlowMapList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbFlowMapList.Font = new System.Drawing.Font("Segoe UI Symbol", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbFlowMapList.FormattingEnabled = true;
			this.cbFlowMapList.Location = new System.Drawing.Point(12, 48);
			this.cbFlowMapList.Name = "cbFlowMapList";
			this.cbFlowMapList.Size = new System.Drawing.Size(400, 33);
			this.cbFlowMapList.TabIndex = 0;
			this.cbFlowMapList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cbFlowMapList_KeyPress);
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnOK.Location = new System.Drawing.Point(12, 92);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(200, 40);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btn_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI Symbol", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
			this.btnCancel.Location = new System.Drawing.Point(212, 92);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(200, 40);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btn_Click);
			// 
			// lbX_Title
			// 
			this.lbX_Title.BackColor = System.Drawing.Color.Transparent;
			this.lbX_Title.Font = new System.Drawing.Font("Segoe UI Black", 18F, System.Drawing.FontStyle.Bold);
			this.lbX_Title.ForeColor = System.Drawing.Color.Black;
			this.lbX_Title.Location = new System.Drawing.Point(6, 8);
			this.lbX_Title.Name = "lbX_Title";
			this.lbX_Title.Size = new System.Drawing.Size(98, 35);
			this.lbX_Title.TabIndex = 58;
			this.lbX_Title.Text = "SELECT";
			this.lbX_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// tmrSuggest
			// 
			this.tmrSuggest.Tick += new System.EventHandler(this.tmrSuggest_Tick);
			// 
			// frmComboBoxSelect
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(422, 142);
			this.ControlBox = false;
			this.Controls.Add(this.lbX_Title);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cbFlowMapList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmComboBoxSelect";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmComboBoxSelect_Load);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.ComboBox cbFlowMapList;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lbX_Title;
		private System.Windows.Forms.Timer tmrSuggest;
	}
}