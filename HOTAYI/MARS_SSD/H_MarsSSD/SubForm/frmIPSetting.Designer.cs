namespace H_MarsSSD
{
    partial class frmIPSetting
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
            this.gbIP = new System.Windows.Forms.GroupBox();
            this.btnReconnect = new System.Windows.Forms.Button();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.tbIP = new System.Windows.Forms.TextBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.gbSetup = new System.Windows.Forms.GroupBox();
            this.tbEquipID = new System.Windows.Forms.TextBox();
            this.brnSetupSet = new System.Windows.Forms.Button();
            this.tbEquipModel = new System.Windows.Forms.TextBox();
            this.lbEquipModel = new System.Windows.Forms.Label();
            this.lbEquipID = new System.Windows.Forms.Label();
            this.gbIP.SuspendLayout();
            this.gbSetup.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbIP
            // 
            this.gbIP.BackColor = System.Drawing.SystemColors.Control;
            this.gbIP.Controls.Add(this.btnReconnect);
            this.gbIP.Controls.Add(this.tbPort);
            this.gbIP.Controls.Add(this.tbIP);
            this.gbIP.Controls.Add(this.btnSet);
            this.gbIP.Controls.Add(this.label1);
            this.gbIP.Font = new System.Drawing.Font("Segoe UI Black", 15F, System.Drawing.FontStyle.Bold);
            this.gbIP.Location = new System.Drawing.Point(12, 12);
            this.gbIP.Name = "gbIP";
            this.gbIP.Size = new System.Drawing.Size(358, 103);
            this.gbIP.TabIndex = 7;
            this.gbIP.TabStop = false;
            this.gbIP.Text = "TOP INSPECTION";
            // 
            // btnReconnect
            // 
            this.btnReconnect.BackgroundImage = global::H_MarsSSD.Properties.Resources.Refresh_48p;
            this.btnReconnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReconnect.Location = new System.Drawing.Point(309, 47);
            this.btnReconnect.Name = "btnReconnect";
            this.btnReconnect.Size = new System.Drawing.Size(34, 35);
            this.btnReconnect.TabIndex = 8;
            this.btnReconnect.UseVisualStyleBackColor = true;
            this.btnReconnect.Click += new System.EventHandler(this.btnReconnect_Click);
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(187, 47);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(62, 35);
            this.tbPort.TabIndex = 5;
            // 
            // tbIP
            // 
            this.tbIP.Location = new System.Drawing.Point(6, 47);
            this.tbIP.Name = "tbIP";
            this.tbIP.Size = new System.Drawing.Size(162, 35);
            this.tbIP.TabIndex = 4;
            // 
            // btnSet
            // 
            this.btnSet.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold);
            this.btnSet.Location = new System.Drawing.Point(255, 47);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(48, 35);
            this.btnSet.TabIndex = 3;
            this.btnSet.Text = "SET";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Black", 20F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(165, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 37);
            this.label1.TabIndex = 6;
            this.label1.Text = ":";
            // 
            // gbSetup
            // 
            this.gbSetup.Controls.Add(this.tbEquipID);
            this.gbSetup.Controls.Add(this.brnSetupSet);
            this.gbSetup.Controls.Add(this.tbEquipModel);
            this.gbSetup.Controls.Add(this.lbEquipModel);
            this.gbSetup.Controls.Add(this.lbEquipID);
            this.gbSetup.Font = new System.Drawing.Font("굴림", 1F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.gbSetup.Location = new System.Drawing.Point(12, 125);
            this.gbSetup.Name = "gbSetup";
            this.gbSetup.Size = new System.Drawing.Size(358, 88);
            this.gbSetup.TabIndex = 18;
            this.gbSetup.TabStop = false;
            this.gbSetup.Text = " ";
            // 
            // tbEquipID
            // 
            this.tbEquipID.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.tbEquipID.Location = new System.Drawing.Point(143, 8);
            this.tbEquipID.Name = "tbEquipID";
            this.tbEquipID.Size = new System.Drawing.Size(146, 31);
            this.tbEquipID.TabIndex = 13;
            // 
            // brnSetupSet
            // 
            this.brnSetupSet.Font = new System.Drawing.Font("Segoe UI Black", 10F, System.Drawing.FontStyle.Bold);
            this.brnSetupSet.Location = new System.Drawing.Point(295, 43);
            this.brnSetupSet.Name = "brnSetupSet";
            this.brnSetupSet.Size = new System.Drawing.Size(48, 35);
            this.brnSetupSet.TabIndex = 16;
            this.brnSetupSet.Text = "SET";
            this.brnSetupSet.UseVisualStyleBackColor = true;
            this.brnSetupSet.Click += new System.EventHandler(this.brnSetupSet_Click);
            // 
            // tbEquipModel
            // 
            this.tbEquipModel.Font = new System.Drawing.Font("Segoe UI Black", 13F, System.Drawing.FontStyle.Bold);
            this.tbEquipModel.Location = new System.Drawing.Point(143, 47);
            this.tbEquipModel.Name = "tbEquipModel";
            this.tbEquipModel.Size = new System.Drawing.Size(146, 31);
            this.tbEquipModel.TabIndex = 15;
            // 
            // lbEquipModel
            // 
            this.lbEquipModel.Font = new System.Drawing.Font("Segoe UI Black", 11F, System.Drawing.FontStyle.Bold);
            this.lbEquipModel.Location = new System.Drawing.Point(6, 52);
            this.lbEquipModel.Name = "lbEquipModel";
            this.lbEquipModel.Size = new System.Drawing.Size(131, 21);
            this.lbEquipModel.TabIndex = 14;
            this.lbEquipModel.Text = "EquipModel";
            this.lbEquipModel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbEquipID
            // 
            this.lbEquipID.Font = new System.Drawing.Font("Segoe UI Black", 11F, System.Drawing.FontStyle.Bold);
            this.lbEquipID.Location = new System.Drawing.Point(6, 14);
            this.lbEquipID.Name = "lbEquipID";
            this.lbEquipID.Size = new System.Drawing.Size(131, 21);
            this.lbEquipID.TabIndex = 12;
            this.lbEquipID.Text = "EquipID";
            this.lbEquipID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmIPSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(377, 225);
            this.ControlBox = false;
            this.Controls.Add(this.gbSetup);
            this.Controls.Add(this.gbIP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIPSetting";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmIPSetting";
            this.Deactivate += new System.EventHandler(this.frmIPSetting_Deactivate);
            this.gbIP.ResumeLayout(false);
            this.gbIP.PerformLayout();
            this.gbSetup.ResumeLayout(false);
            this.gbSetup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox gbIP;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.TextBox tbIP;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReconnect;
        private System.Windows.Forms.GroupBox gbSetup;
        private System.Windows.Forms.TextBox tbEquipID;
        private System.Windows.Forms.Button brnSetupSet;
        private System.Windows.Forms.TextBox tbEquipModel;
        private System.Windows.Forms.Label lbEquipModel;
        private System.Windows.Forms.Label lbEquipID;
    }
}