namespace H_MarsSSD
{
    partial class frmErrRecipeDisp
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
			this.tmrErrRecipeDisp = new System.Windows.Forms.Timer(this.components);
			this.lbNowRecipe = new System.Windows.Forms.Label();
			this.lbErrCode = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.lbDeviceBtm2 = new System.Windows.Forms.Label();
			this.lbDeviceBtm1 = new System.Windows.Forms.Label();
			this.lbCount2 = new System.Windows.Forms.Label();
			this.lbCount1 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lbDeviceTop2 = new System.Windows.Forms.Label();
			this.lbDeviceTop1 = new System.Windows.Forms.Label();
			this.lbStartTime = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.lbStartTime2 = new System.Windows.Forms.Label();
			this.lbStartTime1 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lbLotID2 = new System.Windows.Forms.Label();
			this.lbLotID1 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.btnAllUPPopup = new System.Windows.Forms.Button();
			this.btnAvgClear = new System.Windows.Forms.Button();
			this.lbLast = new System.Windows.Forms.Label();
			this.lbAverage = new System.Windows.Forms.Label();
			this.lbUphAvg = new System.Windows.Forms.Label();
			this.lbTactAvg = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.lbUphLast = new System.Windows.Forms.Label();
			this.lbTactLast = new System.Windows.Forms.Label();
			this.tmrStatusDisp = new System.Windows.Forms.Timer(this.components);
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tmrErrRecipeDisp
			// 
			this.tmrErrRecipeDisp.Enabled = true;
			this.tmrErrRecipeDisp.Interval = 400;
			this.tmrErrRecipeDisp.Tick += new System.EventHandler(this.tmrErrRecipeDisp_Tick);
			// 
			// lbNowRecipe
			// 
			this.lbNowRecipe.BackColor = System.Drawing.Color.Transparent;
			this.lbNowRecipe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbNowRecipe.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.lbNowRecipe.ForeColor = System.Drawing.Color.Black;
			this.lbNowRecipe.Location = new System.Drawing.Point(0, 52);
			this.lbNowRecipe.Margin = new System.Windows.Forms.Padding(5);
			this.lbNowRecipe.Name = "lbNowRecipe";
			this.lbNowRecipe.Size = new System.Drawing.Size(685, 42);
			this.lbNowRecipe.TabIndex = 51;
			this.lbNowRecipe.Text = "MACHINE RECIPE NAME";
			this.lbNowRecipe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbErrCode
			// 
			this.lbErrCode.BackColor = System.Drawing.Color.Transparent;
			this.lbErrCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbErrCode.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
			this.lbErrCode.ForeColor = System.Drawing.Color.Crimson;
			this.lbErrCode.Location = new System.Drawing.Point(0, 1);
			this.lbErrCode.Margin = new System.Windows.Forms.Padding(5);
			this.lbErrCode.Name = "lbErrCode";
			this.lbErrCode.Size = new System.Drawing.Size(685, 42);
			this.lbErrCode.TabIndex = 50;
			this.lbErrCode.Text = "ERROR CODE";
			this.lbErrCode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.lbErrCode.Click += new System.EventHandler(this.lbErrCode_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.lbDeviceBtm2);
			this.groupBox1.Controls.Add(this.lbDeviceBtm1);
			this.groupBox1.Controls.Add(this.lbCount2);
			this.groupBox1.Controls.Add(this.lbCount1);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.lbDeviceTop2);
			this.groupBox1.Controls.Add(this.lbDeviceTop1);
			this.groupBox1.Controls.Add(this.lbStartTime);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.lbStartTime2);
			this.groupBox1.Controls.Add(this.lbStartTime1);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.lbLotID2);
			this.groupBox1.Controls.Add(this.lbLotID1);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(690, -8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(734, 101);
			this.groupBox1.TabIndex = 55;
			this.groupBox1.TabStop = false;
			// 
			// label10
			// 
			this.label10.BackColor = System.Drawing.Color.Transparent;
			this.label10.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.ForeColor = System.Drawing.Color.Black;
			this.label10.Location = new System.Drawing.Point(355, 14);
			this.label10.Margin = new System.Windows.Forms.Padding(5);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(168, 21);
			this.label10.TabIndex = 74;
			this.label10.Text = "BTM Program";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbDeviceBtm2
			// 
			this.lbDeviceBtm2.BackColor = System.Drawing.Color.Transparent;
			this.lbDeviceBtm2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbDeviceBtm2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbDeviceBtm2.ForeColor = System.Drawing.Color.Black;
			this.lbDeviceBtm2.Location = new System.Drawing.Point(355, 68);
			this.lbDeviceBtm2.Margin = new System.Windows.Forms.Padding(5);
			this.lbDeviceBtm2.Name = "lbDeviceBtm2";
			this.lbDeviceBtm2.Size = new System.Drawing.Size(170, 28);
			this.lbDeviceBtm2.TabIndex = 73;
			this.lbDeviceBtm2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbDeviceBtm1
			// 
			this.lbDeviceBtm1.BackColor = System.Drawing.Color.Transparent;
			this.lbDeviceBtm1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbDeviceBtm1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
			this.lbDeviceBtm1.ForeColor = System.Drawing.Color.Black;
			this.lbDeviceBtm1.Location = new System.Drawing.Point(355, 37);
			this.lbDeviceBtm1.Margin = new System.Windows.Forms.Padding(5);
			this.lbDeviceBtm1.Name = "lbDeviceBtm1";
			this.lbDeviceBtm1.Size = new System.Drawing.Size(170, 28);
			this.lbDeviceBtm1.TabIndex = 72;
			this.lbDeviceBtm1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbCount2
			// 
			this.lbCount2.BackColor = System.Drawing.Color.Transparent;
			this.lbCount2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCount2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.lbCount2.ForeColor = System.Drawing.Color.Black;
			this.lbCount2.Location = new System.Drawing.Point(530, 69);
			this.lbCount2.Margin = new System.Windows.Forms.Padding(5);
			this.lbCount2.Name = "lbCount2";
			this.lbCount2.Size = new System.Drawing.Size(69, 28);
			this.lbCount2.TabIndex = 71;
			this.lbCount2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbCount1
			// 
			this.lbCount1.BackColor = System.Drawing.Color.Transparent;
			this.lbCount1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbCount1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
			this.lbCount1.ForeColor = System.Drawing.Color.Black;
			this.lbCount1.Location = new System.Drawing.Point(530, 38);
			this.lbCount1.Margin = new System.Windows.Forms.Padding(5);
			this.lbCount1.Name = "lbCount1";
			this.lbCount1.Size = new System.Drawing.Size(69, 28);
			this.lbCount1.TabIndex = 70;
			this.lbCount1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.BackColor = System.Drawing.Color.Transparent;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.Black;
			this.label3.Location = new System.Drawing.Point(524, 14);
			this.label3.Margin = new System.Windows.Forms.Padding(5);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(82, 21);
			this.label3.TabIndex = 69;
			this.label3.Text = "Qty";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.Black;
			this.label2.Location = new System.Drawing.Point(183, 12);
			this.label2.Margin = new System.Windows.Forms.Padding(5);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(167, 21);
			this.label2.TabIndex = 68;
			this.label2.Text = "TOP Program";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbDeviceTop2
			// 
			this.lbDeviceTop2.BackColor = System.Drawing.Color.Transparent;
			this.lbDeviceTop2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbDeviceTop2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbDeviceTop2.ForeColor = System.Drawing.Color.Black;
			this.lbDeviceTop2.Location = new System.Drawing.Point(180, 68);
			this.lbDeviceTop2.Margin = new System.Windows.Forms.Padding(5);
			this.lbDeviceTop2.Name = "lbDeviceTop2";
			this.lbDeviceTop2.Size = new System.Drawing.Size(170, 28);
			this.lbDeviceTop2.TabIndex = 67;
			this.lbDeviceTop2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbDeviceTop1
			// 
			this.lbDeviceTop1.BackColor = System.Drawing.Color.Transparent;
			this.lbDeviceTop1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbDeviceTop1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
			this.lbDeviceTop1.ForeColor = System.Drawing.Color.Black;
			this.lbDeviceTop1.Location = new System.Drawing.Point(180, 37);
			this.lbDeviceTop1.Margin = new System.Windows.Forms.Padding(5);
			this.lbDeviceTop1.Name = "lbDeviceTop1";
			this.lbDeviceTop1.Size = new System.Drawing.Size(170, 28);
			this.lbDeviceTop1.TabIndex = 66;
			this.lbDeviceTop1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbStartTime
			// 
			this.lbStartTime.BackColor = System.Drawing.Color.Transparent;
			this.lbStartTime.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbStartTime.ForeColor = System.Drawing.Color.Black;
			this.lbStartTime.Location = new System.Drawing.Point(606, 13);
			this.lbStartTime.Margin = new System.Windows.Forms.Padding(5);
			this.lbStartTime.Name = "lbStartTime";
			this.lbStartTime.Size = new System.Drawing.Size(120, 21);
			this.lbStartTime.TabIndex = 65;
			this.lbStartTime.Text = "Start Time";
			this.lbStartTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.Black;
			this.label7.Location = new System.Drawing.Point(21, 12);
			this.label7.Margin = new System.Windows.Forms.Padding(5);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(154, 21);
			this.label7.TabIndex = 64;
			this.label7.Text = "Work Order";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbStartTime2
			// 
			this.lbStartTime2.BackColor = System.Drawing.Color.Transparent;
			this.lbStartTime2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbStartTime2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbStartTime2.ForeColor = System.Drawing.Color.Black;
			this.lbStartTime2.Location = new System.Drawing.Point(605, 69);
			this.lbStartTime2.Margin = new System.Windows.Forms.Padding(5);
			this.lbStartTime2.Name = "lbStartTime2";
			this.lbStartTime2.Size = new System.Drawing.Size(120, 28);
			this.lbStartTime2.TabIndex = 61;
			this.lbStartTime2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbStartTime1
			// 
			this.lbStartTime1.BackColor = System.Drawing.Color.Transparent;
			this.lbStartTime1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbStartTime1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbStartTime1.ForeColor = System.Drawing.Color.Black;
			this.lbStartTime1.Location = new System.Drawing.Point(605, 38);
			this.lbStartTime1.Margin = new System.Windows.Forms.Padding(5);
			this.lbStartTime1.Name = "lbStartTime1";
			this.lbStartTime1.Size = new System.Drawing.Size(120, 28);
			this.lbStartTime1.TabIndex = 60;
			this.lbStartTime1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label5
			// 
			this.label5.BackColor = System.Drawing.Color.Transparent;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.Black;
			this.label5.Location = new System.Drawing.Point(4, 68);
			this.label5.Margin = new System.Windows.Forms.Padding(5);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(16, 28);
			this.label5.TabIndex = 59;
			this.label5.Text = "2";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.BackColor = System.Drawing.Color.Transparent;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.Black;
			this.label4.Location = new System.Drawing.Point(4, 37);
			this.label4.Margin = new System.Windows.Forms.Padding(5);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(16, 28);
			this.label4.TabIndex = 58;
			this.label4.Text = "1";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbLotID2
			// 
			this.lbLotID2.BackColor = System.Drawing.Color.Transparent;
			this.lbLotID2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbLotID2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbLotID2.ForeColor = System.Drawing.Color.Black;
			this.lbLotID2.Location = new System.Drawing.Point(25, 68);
			this.lbLotID2.Margin = new System.Windows.Forms.Padding(5);
			this.lbLotID2.Name = "lbLotID2";
			this.lbLotID2.Size = new System.Drawing.Size(150, 28);
			this.lbLotID2.TabIndex = 57;
			this.lbLotID2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbLotID1
			// 
			this.lbLotID1.BackColor = System.Drawing.Color.Transparent;
			this.lbLotID1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbLotID1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbLotID1.ForeColor = System.Drawing.Color.Black;
			this.lbLotID1.Location = new System.Drawing.Point(25, 37);
			this.lbLotID1.Margin = new System.Windows.Forms.Padding(5);
			this.lbLotID1.Name = "lbLotID1";
			this.lbLotID1.Size = new System.Drawing.Size(150, 28);
			this.lbLotID1.TabIndex = 56;
			this.lbLotID1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.btnAllUPPopup);
			this.groupBox2.Controls.Add(this.btnAvgClear);
			this.groupBox2.Controls.Add(this.lbLast);
			this.groupBox2.Controls.Add(this.lbAverage);
			this.groupBox2.Controls.Add(this.lbUphAvg);
			this.groupBox2.Controls.Add(this.lbTactAvg);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.label6);
			this.groupBox2.Controls.Add(this.lbUphLast);
			this.groupBox2.Controls.Add(this.lbTactLast);
			this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(1436, -8);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(280, 101);
			this.groupBox2.TabIndex = 60;
			this.groupBox2.TabStop = false;
			// 
			// btnAllUPPopup
			// 
			this.btnAllUPPopup.BackColor = System.Drawing.SystemColors.ControlLight;
			this.btnAllUPPopup.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAllUPPopup.Location = new System.Drawing.Point(224, 36);
			this.btnAllUPPopup.Name = "btnAllUPPopup";
			this.btnAllUPPopup.Size = new System.Drawing.Size(47, 29);
			this.btnAllUPPopup.TabIndex = 67;
			this.btnAllUPPopup.Text = "UPH";
			this.btnAllUPPopup.UseVisualStyleBackColor = false;
			this.btnAllUPPopup.Click += new System.EventHandler(this.btnAllUPPopup_Click);
			// 
			// btnAvgClear
			// 
			this.btnAvgClear.BackColor = System.Drawing.SystemColors.ControlLight;
			this.btnAvgClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAvgClear.Location = new System.Drawing.Point(223, 67);
			this.btnAvgClear.Name = "btnAvgClear";
			this.btnAvgClear.Size = new System.Drawing.Size(48, 28);
			this.btnAvgClear.TabIndex = 66;
			this.btnAvgClear.Text = "Clear";
			this.btnAvgClear.UseVisualStyleBackColor = false;
			this.btnAvgClear.Click += new System.EventHandler(this.btnAvgClear_Click);
			// 
			// lbLast
			// 
			this.lbLast.BackColor = System.Drawing.Color.Transparent;
			this.lbLast.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbLast.ForeColor = System.Drawing.Color.Black;
			this.lbLast.Location = new System.Drawing.Point(54, 12);
			this.lbLast.Margin = new System.Windows.Forms.Padding(5);
			this.lbLast.Name = "lbLast";
			this.lbLast.Size = new System.Drawing.Size(73, 21);
			this.lbLast.TabIndex = 63;
			this.lbLast.Text = "Last";
			this.lbLast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbAverage
			// 
			this.lbAverage.BackColor = System.Drawing.Color.Transparent;
			this.lbAverage.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbAverage.ForeColor = System.Drawing.Color.Black;
			this.lbAverage.Location = new System.Drawing.Point(134, 12);
			this.lbAverage.Margin = new System.Windows.Forms.Padding(5);
			this.lbAverage.Name = "lbAverage";
			this.lbAverage.Size = new System.Drawing.Size(90, 21);
			this.lbAverage.TabIndex = 62;
			this.lbAverage.Text = "Average";
			this.lbAverage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lbUphAvg
			// 
			this.lbUphAvg.BackColor = System.Drawing.Color.Transparent;
			this.lbUphAvg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbUphAvg.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbUphAvg.ForeColor = System.Drawing.Color.Black;
			this.lbUphAvg.Location = new System.Drawing.Point(140, 68);
			this.lbUphAvg.Margin = new System.Windows.Forms.Padding(5);
			this.lbUphAvg.Name = "lbUphAvg";
			this.lbUphAvg.Size = new System.Drawing.Size(78, 28);
			this.lbUphAvg.TabIndex = 61;
			this.lbUphAvg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbTactAvg
			// 
			this.lbTactAvg.BackColor = System.Drawing.Color.Transparent;
			this.lbTactAvg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbTactAvg.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbTactAvg.ForeColor = System.Drawing.Color.Black;
			this.lbTactAvg.Location = new System.Drawing.Point(140, 37);
			this.lbTactAvg.Margin = new System.Windows.Forms.Padding(5);
			this.lbTactAvg.Name = "lbTactAvg";
			this.lbTactAvg.Size = new System.Drawing.Size(78, 28);
			this.lbTactAvg.TabIndex = 60;
			this.lbTactAvg.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Black;
			this.label1.Location = new System.Drawing.Point(6, 68);
			this.label1.Margin = new System.Windows.Forms.Padding(5);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 28);
			this.label1.TabIndex = 59;
			this.label1.Text = "UPH";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label6
			// 
			this.label6.BackColor = System.Drawing.Color.Transparent;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.Black;
			this.label6.Location = new System.Drawing.Point(6, 37);
			this.label6.Margin = new System.Windows.Forms.Padding(5);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(44, 28);
			this.label6.TabIndex = 58;
			this.label6.Text = "TACT";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lbUphLast
			// 
			this.lbUphLast.BackColor = System.Drawing.Color.Transparent;
			this.lbUphLast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbUphLast.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbUphLast.ForeColor = System.Drawing.Color.Black;
			this.lbUphLast.Location = new System.Drawing.Point(52, 68);
			this.lbUphLast.Margin = new System.Windows.Forms.Padding(5);
			this.lbUphLast.Name = "lbUphLast";
			this.lbUphLast.Size = new System.Drawing.Size(78, 28);
			this.lbUphLast.TabIndex = 57;
			this.lbUphLast.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// lbTactLast
			// 
			this.lbTactLast.BackColor = System.Drawing.Color.Transparent;
			this.lbTactLast.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lbTactLast.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lbTactLast.ForeColor = System.Drawing.Color.Black;
			this.lbTactLast.Location = new System.Drawing.Point(52, 37);
			this.lbTactLast.Margin = new System.Windows.Forms.Padding(5);
			this.lbTactLast.Name = "lbTactLast";
			this.lbTactLast.Size = new System.Drawing.Size(78, 28);
			this.lbTactLast.TabIndex = 56;
			this.lbTactLast.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tmrStatusDisp
			// 
			this.tmrStatusDisp.Enabled = true;
			this.tmrStatusDisp.Interval = 200;
			this.tmrStatusDisp.Tick += new System.EventHandler(this.tmrStatusDisp_Tick);
			// 
			// frmErrRecipeDisp
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1742, 95);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.lbErrCode);
			this.Controls.Add(this.lbNowRecipe);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmErrRecipeDisp";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "frmErrRecipeDisp";
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrErrRecipeDisp;
        private System.Windows.Forms.Label lbNowRecipe;
        private System.Windows.Forms.Label lbErrCode;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lbLotID2;
		private System.Windows.Forms.Label lbLotID1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lbUphLast;
		private System.Windows.Forms.Label lbTactLast;
		private System.Windows.Forms.Timer tmrStatusDisp;
		private System.Windows.Forms.Label lbTactAvg;
		private System.Windows.Forms.Label lbLast;
		private System.Windows.Forms.Label lbAverage;
		private System.Windows.Forms.Label lbUphAvg;
		private System.Windows.Forms.Button btnAvgClear;
		private System.Windows.Forms.Label lbStartTime;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label lbStartTime2;
		private System.Windows.Forms.Label lbStartTime1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbDeviceTop2;
        private System.Windows.Forms.Label lbDeviceTop1;
        private System.Windows.Forms.Label lbCount2;
        private System.Windows.Forms.Label lbCount1;
        private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnAllUPPopup;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label lbDeviceBtm2;
		private System.Windows.Forms.Label lbDeviceBtm1;
	}
}