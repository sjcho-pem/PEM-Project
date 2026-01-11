namespace H_MarsSSD
{
    partial class frmSearch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtSearchValue = new System.Windows.Forms.TextBox();
            this.btnPreFind = new System.Windows.Forms.Button();
            this.btnFindNext = new System.Windows.Forms.Button();
            this.dgList = new System.Windows.Forms.DataGridView();
            this.dgNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSet = new System.Windows.Forms.Button();
            this.lbNo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgList)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearchValue
            // 
            this.txtSearchValue.Font = new System.Drawing.Font("Segoe UI Semibold", 30F, System.Drawing.FontStyle.Bold);
            this.txtSearchValue.Location = new System.Drawing.Point(120, 11);
            this.txtSearchValue.Name = "txtSearchValue";
            this.txtSearchValue.Size = new System.Drawing.Size(232, 61);
            this.txtSearchValue.TabIndex = 25;
            this.txtSearchValue.TextChanged += new System.EventHandler(this.txtSearchValue_TextChanged);
            this.txtSearchValue.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearchValue_KeyUp);
            // 
            // btnPreFind
            // 
            this.btnPreFind.BackColor = System.Drawing.Color.White;
            this.btnPreFind.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
            this.btnPreFind.Location = new System.Drawing.Point(358, 11);
            this.btnPreFind.Name = "btnPreFind";
            this.btnPreFind.Size = new System.Drawing.Size(50, 61);
            this.btnPreFind.TabIndex = 28;
            this.btnPreFind.TabStop = false;
            this.btnPreFind.Text = "◀";
            this.btnPreFind.UseVisualStyleBackColor = false;
            this.btnPreFind.Click += new System.EventHandler(this.btnPreFind_Click);
            // 
            // btnFindNext
            // 
            this.btnFindNext.BackColor = System.Drawing.Color.White;
            this.btnFindNext.Font = new System.Drawing.Font("Segoe UI", 25F, System.Drawing.FontStyle.Bold);
            this.btnFindNext.Location = new System.Drawing.Point(414, 11);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new System.Drawing.Size(50, 61);
            this.btnFindNext.TabIndex = 27;
            this.btnFindNext.TabStop = false;
            this.btnFindNext.Text = "▶";
            this.btnFindNext.UseVisualStyleBackColor = false;
            this.btnFindNext.Click += new System.EventHandler(this.btnFindNext_Click);
            // 
            // dgList
            // 
            this.dgList.AllowUserToAddRows = false;
            this.dgList.AllowUserToDeleteRows = false;
            this.dgList.AllowUserToResizeColumns = false;
            this.dgList.AllowUserToResizeRows = false;
            this.dgList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgList.ColumnHeadersHeight = 40;
            this.dgList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgNo,
            this.dgName});
            this.dgList.EnableHeadersVisualStyles = false;
            this.dgList.Location = new System.Drawing.Point(12, 83);
            this.dgList.MultiSelect = false;
            this.dgList.Name = "dgList";
            this.dgList.ReadOnly = true;
            this.dgList.RowHeadersVisible = false;
            this.dgList.RowTemplate.Height = 23;
            this.dgList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgList.Size = new System.Drawing.Size(565, 431);
            this.dgList.TabIndex = 29;
            this.dgList.TabStop = false;
            this.dgList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgList_CellClick);
            // 
            // dgNo
            // 
            this.dgNo.HeaderText = "NO";
            this.dgNo.Name = "dgNo";
            this.dgNo.ReadOnly = true;
            this.dgNo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dgName
            // 
            this.dgName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgName.FillWeight = 200F;
            this.dgName.HeaderText = "NAME";
            this.dgName.Name = "dgName";
            this.dgName.ReadOnly = true;
            this.dgName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // btnSet
            // 
            this.btnSet.BackColor = System.Drawing.SystemColors.Window;
            this.btnSet.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnSet.Font = new System.Drawing.Font("Segoe UI", 19F, System.Drawing.FontStyle.Bold);
            this.btnSet.ForeColor = System.Drawing.SystemColors.WindowText;
            this.btnSet.Location = new System.Drawing.Point(481, 11);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(96, 61);
            this.btnSet.TabIndex = 26;
            this.btnSet.Text = "SET";
            this.btnSet.UseVisualStyleBackColor = false;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // lbNo
            // 
            this.lbNo.BackColor = System.Drawing.Color.Black;
            this.lbNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbNo.Font = new System.Drawing.Font("Segoe UI", 30F, System.Drawing.FontStyle.Bold);
            this.lbNo.ForeColor = System.Drawing.Color.PaleGreen;
            this.lbNo.Location = new System.Drawing.Point(12, 11);
            this.lbNo.Name = "lbNo";
            this.lbNo.Size = new System.Drawing.Size(102, 61);
            this.lbNo.TabIndex = 43;
            this.lbNo.Text = "1";
            this.lbNo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frmSearch
            // 
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(590, 526);
            this.Controls.Add(this.lbNo);
            this.Controls.Add(this.btnSet);
            this.Controls.Add(this.dgList);
            this.Controls.Add(this.btnPreFind);
            this.Controls.Add(this.btnFindNext);
            this.Controls.Add(this.txtSearchValue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.dgList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private System.Windows.Forms.TextBox txtSearchValue;
        private System.Windows.Forms.Button btnPreFind;
        private System.Windows.Forms.Button btnFindNext;
        private System.Windows.Forms.DataGridView dgList;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Label lbNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgName;
    }
}