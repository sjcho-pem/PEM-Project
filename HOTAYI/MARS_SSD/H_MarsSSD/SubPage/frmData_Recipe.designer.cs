namespace H_MarsSSD
{
    partial class frmData_Recipe
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
			this.chkRecipeModify = new System.Windows.Forms.CheckBox();
			this.chkRecipeDataMod = new System.Windows.Forms.CheckBox();
			this.lbSelectRecipe = new System.Windows.Forms.Label();
			this.dgridRecipeData = new System.Windows.Forms.DataGridView();
			this.tbColumnRecipeDataNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnRecipeDataName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnRecipeDataValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnRecipeDataUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgridRecipe = new System.Windows.Forms.DataGridView();
			this.tbColumnRecipeNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tbColumnRecipeName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.panel = new System.Windows.Forms.Panel();
			this.btnTraySizeSenSel = new System.Windows.Forms.Button();
			this.btnPartNoEdit = new System.Windows.Forms.Button();
			this.btnPickerSelect = new System.Windows.Forms.Button();
			this.btnRecipeCopy = new System.Windows.Forms.Button();
			this.btnRecipeDataSave = new System.Windows.Forms.Button();
			this.btnRecipeChange = new System.Windows.Forms.Button();
			this.btnTraySet = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.dgridRecipeData)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridRecipe)).BeginInit();
			this.SuspendLayout();
			// 
			// chkRecipeModify
			// 
			this.chkRecipeModify.BackColor = System.Drawing.Color.Transparent;
			this.chkRecipeModify.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkRecipeModify.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkRecipeModify.Location = new System.Drawing.Point(14, 66);
			this.chkRecipeModify.Name = "chkRecipeModify";
			this.chkRecipeModify.Size = new System.Drawing.Size(220, 49);
			this.chkRecipeModify.TabIndex = 5;
			this.chkRecipeModify.Text = "RECIPE MODIFY";
			this.chkRecipeModify.UseVisualStyleBackColor = false;
			this.chkRecipeModify.CheckedChanged += new System.EventHandler(this.chkRecipeModify_CheckedChanged);
			// 
			// chkRecipeDataMod
			// 
			this.chkRecipeDataMod.BackColor = System.Drawing.Color.Transparent;
			this.chkRecipeDataMod.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
			this.chkRecipeDataMod.ForeColor = System.Drawing.Color.DarkOliveGreen;
			this.chkRecipeDataMod.Location = new System.Drawing.Point(733, 66);
			this.chkRecipeDataMod.Name = "chkRecipeDataMod";
			this.chkRecipeDataMod.Size = new System.Drawing.Size(215, 49);
			this.chkRecipeDataMod.TabIndex = 6;
			this.chkRecipeDataMod.Text = "DATA MODIFY";
			this.chkRecipeDataMod.UseVisualStyleBackColor = false;
			this.chkRecipeDataMod.CheckedChanged += new System.EventHandler(this.chkRecipeDataMod_CheckedChanged);
			// 
			// lbSelectRecipe
			// 
			this.lbSelectRecipe.BackColor = System.Drawing.Color.Black;
			this.lbSelectRecipe.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lbSelectRecipe.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold);
			this.lbSelectRecipe.ForeColor = System.Drawing.Color.Khaki;
			this.lbSelectRecipe.Location = new System.Drawing.Point(12, 14);
			this.lbSelectRecipe.Name = "lbSelectRecipe";
			this.lbSelectRecipe.Size = new System.Drawing.Size(80, 55);
			this.lbSelectRecipe.TabIndex = 20;
			this.lbSelectRecipe.Text = "000";
			this.lbSelectRecipe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// dgridRecipeData
			// 
			this.dgridRecipeData.AllowUserToAddRows = false;
			this.dgridRecipeData.AllowUserToDeleteRows = false;
			this.dgridRecipeData.AllowUserToResizeColumns = false;
			this.dgridRecipeData.AllowUserToResizeRows = false;
			this.dgridRecipeData.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridRecipeData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dgridRecipeData.ColumnHeadersHeight = 40;
			this.dgridRecipeData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridRecipeData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnRecipeDataNum,
            this.tbColumnRecipeDataName,
            this.tbColumnRecipeDataValue,
            this.tbColumnRecipeDataUnit});
			this.dgridRecipeData.EnableHeadersVisualStyles = false;
			this.dgridRecipeData.Location = new System.Drawing.Point(733, 121);
			this.dgridRecipeData.MultiSelect = false;
			this.dgridRecipeData.Name = "dgridRecipeData";
			this.dgridRecipeData.RowHeadersVisible = false;
			this.dgridRecipeData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridRecipeData.RowTemplate.Height = 35;
			this.dgridRecipeData.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridRecipeData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgridRecipeData.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridRecipeData.Size = new System.Drawing.Size(710, 650);
			this.dgridRecipeData.TabIndex = 24;
			this.dgridRecipeData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridRecipeData_CellClick);
			this.dgridRecipeData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridRecipeData_CellDoubleClick);
			this.dgridRecipeData.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridRecipeData_CellValueChanged);
			this.dgridRecipeData.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgridRecipeData_ColumnHeaderMouseDoubleClick);
			// 
			// tbColumnRecipeDataNum
			// 
			this.tbColumnRecipeDataNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnRecipeDataNum.DefaultCellStyle = dataGridViewCellStyle2;
			this.tbColumnRecipeDataNum.HeaderText = "NO";
			this.tbColumnRecipeDataNum.Name = "tbColumnRecipeDataNum";
			this.tbColumnRecipeDataNum.ReadOnly = true;
			this.tbColumnRecipeDataNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnRecipeDataNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnRecipeDataNum.Width = 60;
			// 
			// tbColumnRecipeDataName
			// 
			this.tbColumnRecipeDataName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.tbColumnRecipeDataName.DefaultCellStyle = dataGridViewCellStyle3;
			this.tbColumnRecipeDataName.HeaderText = "RECIPE DATA NAME ";
			this.tbColumnRecipeDataName.Name = "tbColumnRecipeDataName";
			this.tbColumnRecipeDataName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnRecipeDataName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// tbColumnRecipeDataValue
			// 
			this.tbColumnRecipeDataValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
			this.tbColumnRecipeDataValue.DefaultCellStyle = dataGridViewCellStyle4;
			this.tbColumnRecipeDataValue.HeaderText = "VALUE";
			this.tbColumnRecipeDataValue.Name = "tbColumnRecipeDataValue";
			this.tbColumnRecipeDataValue.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnRecipeDataValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnRecipeDataValue.Width = 117;
			// 
			// tbColumnRecipeDataUnit
			// 
			this.tbColumnRecipeDataUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnRecipeDataUnit.DefaultCellStyle = dataGridViewCellStyle5;
			this.tbColumnRecipeDataUnit.HeaderText = "UNIT";
			this.tbColumnRecipeDataUnit.Name = "tbColumnRecipeDataUnit";
			this.tbColumnRecipeDataUnit.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnRecipeDataUnit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// dgridRecipe
			// 
			this.dgridRecipe.AllowUserToAddRows = false;
			this.dgridRecipe.AllowUserToDeleteRows = false;
			this.dgridRecipe.AllowUserToResizeColumns = false;
			this.dgridRecipe.AllowUserToResizeRows = false;
			this.dgridRecipe.BackgroundColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dgridRecipe.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
			this.dgridRecipe.ColumnHeadersHeight = 40;
			this.dgridRecipe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
			this.dgridRecipe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnRecipeNum,
            this.tbColumnRecipeName});
			this.dgridRecipe.Enabled = false;
			this.dgridRecipe.EnableHeadersVisualStyles = false;
			this.dgridRecipe.Location = new System.Drawing.Point(12, 121);
			this.dgridRecipe.MultiSelect = false;
			this.dgridRecipe.Name = "dgridRecipe";
			this.dgridRecipe.RowHeadersVisible = false;
			this.dgridRecipe.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.dgridRecipe.RowTemplate.Height = 35;
			this.dgridRecipe.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.dgridRecipe.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.dgridRecipe.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgridRecipe.Size = new System.Drawing.Size(710, 650);
			this.dgridRecipe.TabIndex = 25;
			this.dgridRecipe.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridRecipe_CellClick);
			this.dgridRecipe.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgridRecipe_CellValueChanged);
			// 
			// tbColumnRecipeNum
			// 
			this.tbColumnRecipeNum.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			this.tbColumnRecipeNum.DefaultCellStyle = dataGridViewCellStyle7;
			this.tbColumnRecipeNum.HeaderText = "NO";
			this.tbColumnRecipeNum.Name = "tbColumnRecipeNum";
			this.tbColumnRecipeNum.ReadOnly = true;
			this.tbColumnRecipeNum.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnRecipeNum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			this.tbColumnRecipeNum.Width = 60;
			// 
			// tbColumnRecipeName
			// 
			this.tbColumnRecipeName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			this.tbColumnRecipeName.DefaultCellStyle = dataGridViewCellStyle8;
			this.tbColumnRecipeName.HeaderText = "RECIPE NAME";
			this.tbColumnRecipeName.Name = "tbColumnRecipeName";
			this.tbColumnRecipeName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.tbColumnRecipeName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
			// 
			// panel
			// 
			this.panel.BackColor = System.Drawing.Color.Black;
			this.panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel.Location = new System.Drawing.Point(1052, 11);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(3, 60);
			this.panel.TabIndex = 27;
			// 
			// btnTraySizeSenSel
			// 
			this.btnTraySizeSenSel.BackColor = System.Drawing.Color.White;
			this.btnTraySizeSenSel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnTraySizeSenSel.Enabled = false;
			this.btnTraySizeSenSel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnTraySizeSenSel.Image = global::H_MarsSSD.Properties.Resources.sensor;
			this.btnTraySizeSenSel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnTraySizeSenSel.Location = new System.Drawing.Point(927, 14);
			this.btnTraySizeSenSel.Name = "btnTraySizeSenSel";
			this.btnTraySizeSenSel.Size = new System.Drawing.Size(110, 55);
			this.btnTraySizeSenSel.TabIndex = 29;
			this.btnTraySizeSenSel.Text = "SENSOR";
			this.btnTraySizeSenSel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnTraySizeSenSel.UseVisualStyleBackColor = false;
			this.btnTraySizeSenSel.Click += new System.EventHandler(this.btnTraySizeSenSel_Click);
			// 
			// btnPartNoEdit
			// 
			this.btnPartNoEdit.BackColor = System.Drawing.Color.White;
			this.btnPartNoEdit.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnPartNoEdit.Image = global::H_MarsSSD.Properties.Resources.Edit_24;
			this.btnPartNoEdit.Location = new System.Drawing.Point(519, 14);
			this.btnPartNoEdit.Name = "btnPartNoEdit";
			this.btnPartNoEdit.Size = new System.Drawing.Size(120, 55);
			this.btnPartNoEdit.TabIndex = 28;
			this.btnPartNoEdit.Text = "AUTO CHANGE";
			this.btnPartNoEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.btnPartNoEdit.UseVisualStyleBackColor = false;
			this.btnPartNoEdit.Visible = false;
			this.btnPartNoEdit.Click += new System.EventHandler(this.btnPartNoEdit_Click);
			// 
			// btnPickerSelect
			// 
			this.btnPickerSelect.BackColor = System.Drawing.Color.White;
			this.btnPickerSelect.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnPickerSelect.Image = global::H_MarsSSD.Properties.Resources.PickerIcon;
			this.btnPickerSelect.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnPickerSelect.Location = new System.Drawing.Point(653, 14);
			this.btnPickerSelect.Name = "btnPickerSelect";
			this.btnPickerSelect.Size = new System.Drawing.Size(120, 55);
			this.btnPickerSelect.TabIndex = 26;
			this.btnPickerSelect.Text = "VACUUM";
			this.btnPickerSelect.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnPickerSelect.UseVisualStyleBackColor = false;
			this.btnPickerSelect.Visible = false;
			this.btnPickerSelect.Click += new System.EventHandler(this.btnPickerSelect_Click);
			// 
			// btnRecipeCopy
			// 
			this.btnRecipeCopy.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.btnRecipeCopy.BackColor = System.Drawing.Color.White;
			this.btnRecipeCopy.Enabled = false;
			this.btnRecipeCopy.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnRecipeCopy.Image = global::H_MarsSSD.Properties.Resources.RecipeCopy_24p;
			this.btnRecipeCopy.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnRecipeCopy.Location = new System.Drawing.Point(1070, 14);
			this.btnRecipeCopy.Name = "btnRecipeCopy";
			this.btnRecipeCopy.Size = new System.Drawing.Size(120, 55);
			this.btnRecipeCopy.TabIndex = 23;
			this.btnRecipeCopy.Text = "COPY";
			this.btnRecipeCopy.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnRecipeCopy.UseVisualStyleBackColor = false;
			this.btnRecipeCopy.Click += new System.EventHandler(this.btnRecipeCopy_Click);
			// 
			// btnRecipeDataSave
			// 
			this.btnRecipeDataSave.BackColor = System.Drawing.Color.White;
			this.btnRecipeDataSave.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnRecipeDataSave.Image = global::H_MarsSSD.Properties.Resources.SaveBlue_24p;
			this.btnRecipeDataSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnRecipeDataSave.Location = new System.Drawing.Point(1322, 14);
			this.btnRecipeDataSave.Name = "btnRecipeDataSave";
			this.btnRecipeDataSave.Size = new System.Drawing.Size(120, 55);
			this.btnRecipeDataSave.TabIndex = 19;
			this.btnRecipeDataSave.Text = "SAVE";
			this.btnRecipeDataSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnRecipeDataSave.UseVisualStyleBackColor = false;
			this.btnRecipeDataSave.Click += new System.EventHandler(this.btnRecipeDataSave_Click);
			// 
			// btnRecipeChange
			// 
			this.btnRecipeChange.BackColor = System.Drawing.Color.White;
			this.btnRecipeChange.Enabled = false;
			this.btnRecipeChange.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnRecipeChange.Image = global::H_MarsSSD.Properties.Resources.RecipeChange_24p;
			this.btnRecipeChange.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnRecipeChange.Location = new System.Drawing.Point(1196, 14);
			this.btnRecipeChange.Name = "btnRecipeChange";
			this.btnRecipeChange.Size = new System.Drawing.Size(120, 55);
			this.btnRecipeChange.TabIndex = 18;
			this.btnRecipeChange.Text = "CHANGE";
			this.btnRecipeChange.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnRecipeChange.UseVisualStyleBackColor = false;
			this.btnRecipeChange.Click += new System.EventHandler(this.btnRecipeChange_Click);
			// 
			// btnTraySet
			// 
			this.btnTraySet.BackColor = System.Drawing.Color.White;
			this.btnTraySet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.btnTraySet.Enabled = false;
			this.btnTraySet.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.btnTraySet.Image = global::H_MarsSSD.Properties.Resources.gapsize;
			this.btnTraySet.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.btnTraySet.Location = new System.Drawing.Point(811, 14);
			this.btnTraySet.Name = "btnTraySet";
			this.btnTraySet.Size = new System.Drawing.Size(110, 55);
			this.btnTraySet.TabIndex = 30;
			this.btnTraySet.Text = "TRAY";
			this.btnTraySet.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.btnTraySet.UseVisualStyleBackColor = false;
			this.btnTraySet.Click += new System.EventHandler(this.btnTraySet_Click);
			// 
			// frmData_Recipe
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.BackColor = System.Drawing.Color.GhostWhite;
			this.ClientSize = new System.Drawing.Size(1463, 824);
			this.Controls.Add(this.btnTraySet);
			this.Controls.Add(this.btnTraySizeSenSel);
			this.Controls.Add(this.btnPartNoEdit);
			this.Controls.Add(this.panel);
			this.Controls.Add(this.btnPickerSelect);
			this.Controls.Add(this.dgridRecipe);
			this.Controls.Add(this.dgridRecipeData);
			this.Controls.Add(this.btnRecipeCopy);
			this.Controls.Add(this.lbSelectRecipe);
			this.Controls.Add(this.btnRecipeDataSave);
			this.Controls.Add(this.btnRecipeChange);
			this.Controls.Add(this.chkRecipeDataMod);
			this.Controls.Add(this.chkRecipeModify);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmData_Recipe";
			this.Text = "frmData_Recipe";
			((System.ComponentModel.ISupportInitialize)(this.dgridRecipeData)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dgridRecipe)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkRecipeModify;
        private System.Windows.Forms.CheckBox chkRecipeDataMod;
        private System.Windows.Forms.Button btnRecipeChange;
        private System.Windows.Forms.Button btnRecipeDataSave;
        private System.Windows.Forms.Label lbSelectRecipe;
        private System.Windows.Forms.Button btnRecipeCopy;
        private System.Windows.Forms.DataGridView dgridRecipeData;
        private System.Windows.Forms.DataGridView dgridRecipe;
        private System.Windows.Forms.Button btnPickerSelect;
        private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnRecipeNum;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnRecipeName;
		private System.Windows.Forms.Button btnPartNoEdit;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnRecipeDataNum;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnRecipeDataName;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnRecipeDataValue;
		private System.Windows.Forms.DataGridViewTextBoxColumn tbColumnRecipeDataUnit;
		private System.Windows.Forms.Button btnTraySizeSenSel;
		private System.Windows.Forms.Button btnTraySet;
	}
}