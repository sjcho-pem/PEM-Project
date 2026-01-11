namespace H_MarsSSD
{
    partial class frmPnPSelect
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
			this.lbPick = new System.Windows.Forms.Label();
			this.lbPlace = new System.Windows.Forms.Label();
			this.cbPick = new System.Windows.Forms.ComboBox();
			this.cbPlace = new System.Windows.Forms.ComboBox();
			this.btnCancel = new System.Windows.Forms.Button();
			this.panel1 = new System.Windows.Forms.Panel();
			this.lbX_Title = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// lbPick
			// 
			this.lbPick.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
			this.lbPick.ForeColor = System.Drawing.Color.Black;
			this.lbPick.Location = new System.Drawing.Point(8, 16);
			this.lbPick.Name = "lbPick";
			this.lbPick.Size = new System.Drawing.Size(152, 33);
			this.lbPick.TabIndex = 0;
			this.lbPick.Text = "PICK POSITION";
			this.lbPick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbPlace
			// 
			this.lbPlace.Font = new System.Drawing.Font("Segoe UI Black", 12F, System.Drawing.FontStyle.Bold);
			this.lbPlace.ForeColor = System.Drawing.Color.Black;
			this.lbPlace.Location = new System.Drawing.Point(5, 68);
			this.lbPlace.Name = "lbPlace";
			this.lbPlace.Size = new System.Drawing.Size(156, 33);
			this.lbPlace.TabIndex = 1;
			this.lbPlace.Text = "PLACE POSITION";
			this.lbPlace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// cbPick
			// 
			this.cbPick.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbPick.FormattingEnabled = true;
			this.cbPick.Location = new System.Drawing.Point(165, 16);
			this.cbPick.Name = "cbPick";
			this.cbPick.Size = new System.Drawing.Size(230, 33);
			this.cbPick.TabIndex = 2;
			// 
			// cbPlace
			// 
			this.cbPlace.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cbPlace.FormattingEnabled = true;
			this.cbPlace.Location = new System.Drawing.Point(165, 68);
			this.cbPlace.Name = "cbPlace";
			this.cbPlace.Size = new System.Drawing.Size(230, 33);
			this.cbPlace.TabIndex = 5;
			// 
			// btnCancel
			// 
			this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnCancel.Location = new System.Drawing.Point(298, 167);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(114, 40);
			this.btnCancel.TabIndex = 6;
			this.btnCancel.Text = "CANCEL";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.panel1.Controls.Add(this.lbPlace);
			this.panel1.Controls.Add(this.lbPick);
			this.panel1.Controls.Add(this.cbPlace);
			this.panel1.Controls.Add(this.cbPick);
			this.panel1.Location = new System.Drawing.Point(7, 44);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(405, 117);
			this.panel1.TabIndex = 7;
			// 
			// lbX_Title
			// 
			this.lbX_Title.BackColor = System.Drawing.Color.Transparent;
			this.lbX_Title.Font = new System.Drawing.Font("Segoe UI Black", 18F, System.Drawing.FontStyle.Bold);
			this.lbX_Title.ForeColor = System.Drawing.Color.Black;
			this.lbX_Title.Location = new System.Drawing.Point(2, 2);
			this.lbX_Title.Name = "lbX_Title";
			this.lbX_Title.Size = new System.Drawing.Size(165, 35);
			this.lbX_Title.TabIndex = 54;
			this.lbX_Title.Text = "PnP SELECT";
			this.lbX_Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnOK
			// 
			this.btnOK.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
			this.btnOK.Location = new System.Drawing.Point(178, 167);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(111, 40);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// frmPnPSelect
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.ClientSize = new System.Drawing.Size(419, 211);
			this.ControlBox = false;
			this.Controls.Add(this.lbX_Title);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmPnPSelect";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.TopMost = true;
			this.Load += new System.EventHandler(this.frmPnPSelect_Load);
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbPick;
        private System.Windows.Forms.Label lbPlace;
        private System.Windows.Forms.ComboBox cbPick;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cbPlace;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbX_Title;
    }
}