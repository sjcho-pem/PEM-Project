using System;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmData_Dm : Form
    {        
        public frmData_Dm()
        {
            InitializeComponent();
            InitialXML();
            InitialGrid();
            InitialDMData();

            tmrDm.Start();
            //UpdateDm();
        }

        #region [ Variables ]

        private XML XMLNDm = new XML();
        private XML XMLDDm = new XML();
        private string strDmCol0 = "DataMemoryName";
        
        public int[] nDm = new int[Define.MAX_DM_NUM];
        public double[] dDm = new double[Define.MAX_DM_NUM];
        #endregion

        #region [ Event Function ]

        #endregion

        #region [ Timmer Function ]

        private void tmrDm_Tick(object sender, EventArgs e)
        {

            if (!chkNDmEnable.Checked && !chkNDmMod.Checked)
            {
                SM.SHM.GetNDMData(ref nDm);
                UpdateNDm();
            }

            if (!chkDDmEnable.Checked && !chkDDmMod.Checked)
            {
                SM.SHM.GetDDMData(ref dDm);
                UpdateDDm();
            }
        }
        #endregion

        #region [ CheckBox Event ]

        // NDM Modify CheckBox
        private void chkNDmMod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNDmMod.Checked)
            {
                chkNDmMod.ForeColor = Color.Red;
                chkNDmEnable.Enabled = false;

                this.tbColumnNDmName.ReadOnly = false;
            }
            else
            {
                chkNDmMod.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                chkNDmEnable.Enabled = true;
                this.tbColumnNDmName.ReadOnly = true;

                XMLNDm.UpdateData();
            }
        }

        // NDM Enable CheckBox
        private void chkNDmEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNDmEnable.Checked)
            {
                chkNDmEnable.ForeColor = Color.Red;
                chkNDmMod.Enabled = false;
                this.tbColumnNDmValue.ReadOnly = false;
            }
            else
            {
                chkNDmEnable.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                chkNDmMod.Enabled = true;
                this.tbColumnNDmValue.ReadOnly = true;
            }
        }

        // DDM Modify CheckBox
        private void chkDDmMod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDDmMod.Checked)
            {
                chkDDmMod.ForeColor = Color.Red;
                chkDDmEnable.Enabled = false;

                this.tbColumnDDmName.ReadOnly = false;
            }
            else
            {
                chkDDmMod.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                chkDDmEnable.Enabled = true;
                this.tbColumnDDmName.ReadOnly = true;

                XMLDDm.UpdateData();
            }
        }

        // DDM Enable CheckBox
        private void chkDDmEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkDDmEnable.Checked)
            {
                chkDDmEnable.ForeColor = Color.Red;
                chkDDmMod.Enabled = false;
                this.tbColumnDDmValue.ReadOnly = false;
            }
            else
            {
                chkDDmEnable.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                chkDDmMod.Enabled = true;
                this.tbColumnDDmValue.ReadOnly = true;
            }
        }
        #endregion

        #region [ Button Event ]

        #endregion

        #region [ Grid Setting ]

        #region -Function
        private void InitialGrid()
        {
			
			int nVisibleRowCount = 16;
            dgridNDm.RowTemplate.Height = dgridNDm.Height / (nVisibleRowCount + 1);
            dgridNDm.ColumnHeadersHeight = dgridNDm.Height - dgridNDm.RowTemplate.Height * nVisibleRowCount;
			
            dgridDDm.RowTemplate.Height = dgridDDm.Height / (nVisibleRowCount + 1);
            dgridDDm.ColumnHeadersHeight = dgridDDm.Height - dgridDDm.RowTemplate.Height * nVisibleRowCount;

            // NDM 초기화
            for (int i = 0; i < Define.MAX_DM_NUM; i++)
                this.dgridNDm.Rows.Add(i, XMLNDm.ReadDataTable(i, strDmCol0), 0);

            this.tbColumnNDmName.ReadOnly = true;
            this.tbColumnNDmValue.ReadOnly = true;

            // DDM 초기화
            for (int i = 0; i < Define.MAX_DM_NUM; i++)
                this.dgridDDm.Rows.Add(i, XMLDDm.ReadDataTable(i, strDmCol0), 0);

            this.tbColumnDDmName.ReadOnly = true;
            this.tbColumnDDmValue.ReadOnly = true;
        }

        // NDM
        private void UpdateNDm()
        {
            for (int i = 0; i < Define.MAX_DM_NUM; i++)
            {
                this.dgridNDm.Rows[i].Cells[2].Value = Convert.ToString(nDm[i]);
            }
        }

        // DDM
        private void UpdateDDm()
        {
            for (int i = 0; i < Define.MAX_DM_NUM; i++)
            {
                // UI에서만 소수점 3자리까지 표기
                Int32 nNum = (int)(dDm[i] * 1000);
                double dResult = Convert.ToDouble(nNum) / 1000;

                this.dgridDDm.Rows[i].Cells[2].Value = Convert.ToString(dResult);
            }
        }

        // NDM
        private void UpdateNDm(int idx)
        {
            this.dgridNDm.Rows[idx].Cells[2].Value = Convert.ToString(nDm[idx]);
        }

        // DDM
        private void UpdateDDm(int idx)
        {
            // UI에서만 소수점 3자리까지 표기
            Int32 nNum = (int)(dDm[idx] * 1000);
            double dResult = Convert.ToDouble(nNum) / 1000;

            this.dgridDDm.Rows[idx].Cells[2].Value = Convert.ToString(dResult);
        }

        #endregion

        #region - Event
        // NDM
        private void dgridNDm_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (!chkNDmEnable.Checked && !chkNDmMod.Checked) return;

            bool isNum = false;
            int value = 0;

            string strNewValue = "";
            if (this.dgridNDm[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridNDm[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            if (!dgridNDm.Columns[1].ReadOnly)
            {
                XMLNDm.WriteDataTable(e.RowIndex, strDmCol0, strNewValue);
            }
            else if (!dgridNDm.Columns[2].ReadOnly)
            {
                isNum = int.TryParse(strNewValue, out value);

                if (!isNum)
                {
                    MessageBox.Show("Invalid number! Try again!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateNDm(e.RowIndex);
                }
                else
                {
                    SetNDm(e.RowIndex, Convert.ToInt32(strNewValue));
                    UpdateNDm(e.RowIndex);
                    //UI.SHM.SetDMData((uint)e.RowIndex, Convert.ToInt32(strNewValue));
                    NDm.Write((eNDM)e.RowIndex, Convert.ToInt32(strNewValue));
                }
            }
        }

        // DDM
        private void dgridDDm_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (!chkDDmEnable.Checked && !chkDDmMod.Checked) return;

            bool isNum = false;
            double value = 0;

            string strNewValue = "";
            if (this.dgridDDm[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridDDm[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            if (!dgridDDm.Columns[1].ReadOnly)
            {
                XMLDDm.WriteDataTable(e.RowIndex, strDmCol0, strNewValue);
            }
            else if (!dgridDDm.Columns[2].ReadOnly)
            {
                isNum = double.TryParse(strNewValue, out value);

                if (!isNum)
                {
                    MessageBox.Show("Invalid number! Try again!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    UpdateDDm(e.RowIndex);
                }
                else
                {
                    SetDDm(e.RowIndex, Convert.ToDouble(strNewValue));
                    UpdateDDm(e.RowIndex);
                    //UI.SHM.SetDMData((uint)e.RowIndex, Convert.ToInt32(strNewValue));
                    DDm.Write((eDDM)e.RowIndex, Convert.ToDouble(strNewValue));
                }
            }
        }


        private void dgridNDm_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!chkNDmMod.Checked ) return;
            string strMsg = "";
            if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                strMsg = "Do you want to update the data name?\r\n(Initialize the existing name)";
            else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
                strMsg = "데이터이름을 업데이트하시겠습니까?\r\n(기존 이름은 초기화됩니다.)";

            frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;


            for (int i = 0; i < (int)eNDM.DM255; i++)
            {
                if (EnumHelper.GetDescription((eNDM)i) == i.ToString())
                    XMLNDm.WriteDataTable(i, strDmCol0, " ");
                else
                    XMLNDm.WriteDataTable(i, strDmCol0, EnumHelper.GetDescription((eNDM)i));

                dgridNDm.Rows[i].Cells[1].Value = XMLNDm.ReadDataTable(i, strDmCol0);
            }

            XMLNDm.UpdateData();
            XMLNDm.RefillData();

        }

        private void dgridDDm_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!chkDDmMod.Checked) return;

            string strMsg = "";
            if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                strMsg = "Do you want to update the data name?\r\n(Initialize the existing name)";
            else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
                strMsg = "데이터이름을 업데이트하시겠습니까?\r\n(기존 이름은 초기화됩니다.)";

            frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            for (int i = 0; i < (int)eDDM.DM255; i++)
            {
                if (EnumHelper.GetDescription((eDDM)i) == i.ToString())
                    XMLDDm.WriteDataTable(i, strDmCol0, " ");
                else
                    XMLDDm.WriteDataTable(i, strDmCol0, EnumHelper.GetDescription((eDDM)i));

                dgridDDm.Rows[i].Cells[1].Value = XMLDDm.ReadDataTable(i, strDmCol0);
            }
            XMLDDm.UpdateData();
            XMLDDm.RefillData();
        }

        #endregion
        #endregion

        #region [ Function ]

        #region - Private Function

        private void InitialXML()
        {
            XMLNDm.Initializ("NDataMemory");
            XMLDDm.Initializ("DDataMemory");
        }

        private void InitialDMData()
        {
            for (int i = 0; i < Define.MAX_DM_NUM; i++)
            {
                // NDM
                nDm[i] = new int();
                nDm[i] = 0;
                // for teset
                //uiDm[i] = Convert.ToUInt32(i);

                // DDM
                dDm[i] = new double();
                dDm[i] = 0.0;
            }
        }

        // NDM
        private void SetNDm(int idx, int value)
        {
            nDm[idx] = value;
        }

        // DDM
        private void SetDDm(int idx, double value)
        {
            dDm[idx] = value;
        }
        #endregion

        #region - Public Function

        // NDM
        public int GetNDmData(eNDM data)
        {
            int num = Convert.ToInt32(data);

            if (num >= Define.MAX_DM_NUM)
                num = Define.MAX_DM_NUM - 1;
            if (num < 0)
                num = 0;

            return (nDm[num]);
        }

        // DDM
        public double GetDDmData(eDDM data)
        {
            int num = Convert.ToInt32(data);

            if (num >= Define.MAX_DM_NUM)
                num = Define.MAX_DM_NUM - 1;
            if (num < 0)
                num = 0;

            return (dDm[num]);
        }

        // NDM
        public int GetNDmData(int num)
        {
            if (num >= Define.MAX_DM_NUM)
                num = Define.MAX_DM_NUM - 1;
            if (num < 0)
                num = 0;

            return (nDm[num]);
        }

        // DDM
        public double GetDDmData(int num)
        {
            if (num >= Define.MAX_DM_NUM)
                num = Define.MAX_DM_NUM - 1;
            if (num < 0)
                num = 0;

            return (dDm[num]);
        }
		
        public void UpdateCheckBox(PW_MODE Password_Now)
        {
            if (!chkNDmEnable.Checked)
                chkNDmMod.Enabled = true;
            else
                chkNDmMod.Enabled = false;

            if (!chkNDmMod.Checked)
                chkNDmEnable.Enabled = true;
            else
                chkNDmEnable.Enabled = false;

            if (!chkDDmEnable.Checked)
                chkDDmMod.Enabled = true;
            else
                chkDDmMod.Enabled = false;

            if (!chkDDmMod.Checked)
                chkDDmEnable.Enabled = true;
            else
                chkDDmEnable.Enabled = false;
        }

        public bool IsWorking()
        {
            if (chkNDmEnable.Checked || chkNDmMod.Checked)
                return true;

            if (chkDDmEnable.Checked || chkDDmMod.Checked)
                return true;
            return false;
        }

        public void WorkEnd()
        {
            if (chkNDmEnable.Checked)
                chkNDmEnable.Checked = false;
            if (chkNDmMod.Checked)
                chkNDmMod.Checked = false;

            if (chkDDmEnable.Checked)
                chkDDmEnable.Checked = false;
            if (chkDDmMod.Checked)
                chkDDmMod.Checked = false;
        }

        public void DisableWork()
        {
            if (chkNDmEnable.Enabled)
                chkNDmEnable.Enabled = false;
            if (chkNDmMod.Enabled)
                chkNDmMod.Enabled = false;

            if (chkDDmEnable.Enabled)
                chkDDmEnable.Enabled = false;
            if (chkDDmMod.Enabled)
                chkDDmMod.Enabled = false;
        }

        public void EnableWork()
        {
            if (!chkNDmEnable.Enabled)
                chkNDmEnable.Enabled = true;
            if (!chkNDmMod.Enabled)
                chkNDmMod.Enabled = true;

            if (!chkDDmEnable.Enabled)
                chkDDmEnable.Enabled = true;
            if (!chkDDmMod.Enabled)
                chkDDmMod.Enabled = true;
        }

        // NDM
        public void SetNDMNameValue(int rowIndex, string strNewValue)
        {
            XMLNDm.WriteDataTable(rowIndex, strDmCol0, strNewValue);
        }

        // DDM
        public void SetDDMNameValue(int rowIndex, string strNewValue)
        {
            XMLDDm.WriteDataTable(rowIndex, strDmCol0, strNewValue);
        }

        //public void SetValue(int rowIndex, int value)
        //{
        //    SetDm(rowIndex, value);
        //    UpdateDm(rowIndex);
        //    UI.SHM.SetDMData((uint)rowIndex, value);
        //}
        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            chkNDmMod.Text = Language.languageDefault.NameModify;
            chkNDmEnable.Text = Language.languageDefault.Enable;
            chkDDmMod.Text = Language.languageDefault.NameModify;
            chkDDmEnable.Text = Language.languageDefault.Enable;
        }

        #endregion

        #endregion

    }
}

