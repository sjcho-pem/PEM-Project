namespace H_MarsSSD
{
    partial class frmLoading
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
            this.lbText = new System.Windows.Forms.Label();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.plLoading = new System.Windows.Forms.Panel();
            this.tmrLoading = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.plLoading.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbText
            // 
            this.lbText.Font = new System.Drawing.Font("Segoe UI", 40F, System.Drawing.FontStyle.Bold);
            this.lbText.Location = new System.Drawing.Point(389, 162);
            this.lbText.Name = "lbText";
            this.lbText.Size = new System.Drawing.Size(418, 82);
            this.lbText.TabIndex = 0;
            this.lbText.Text = "Loading . . .";
            this.lbText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbLoading
            // 
            this.pbLoading.Image = global::H_MarsSSD.Properties.Resources.Loading_400p;
            this.pbLoading.Location = new System.Drawing.Point(58, 3);
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.Size = new System.Drawing.Size(410, 410);
            this.pbLoading.TabIndex = 1;
            this.pbLoading.TabStop = false;
            // 
            // plLoading
            // 
            this.plLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plLoading.BackColor = System.Drawing.Color.Transparent;
            this.plLoading.Controls.Add(this.lbText);
            this.plLoading.Controls.Add(this.pbLoading);
            this.plLoading.Location = new System.Drawing.Point(46, 126);
            this.plLoading.Name = "plLoading";
            this.plLoading.Size = new System.Drawing.Size(840, 419);
            this.plLoading.TabIndex = 2;
            // 
            // tmrLoading
            // 
            this.tmrLoading.Tick += new System.EventHandler(this.tmrLoading_Tick);
            // 
            // frmLoading
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(954, 634);
            this.Controls.Add(this.plLoading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLoading";
            this.Opacity = 0.5D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmLoading";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.VisibleChanged += new System.EventHandler(this.frmLoading_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.plLoading.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbText;
        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.Panel plLoading;
        private System.Windows.Forms.Timer tmrLoading;
    }
}