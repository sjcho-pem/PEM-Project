using System;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmTenKey : Form
    {
        public frmTenKey()
        {
            InitializeComponent();

            this.InitialXML();
            this.initializeGrid();

            lbSelectManual.Text = "0";
        }

        #region [ Variables ]
        private XML XMLManual = new XML();
        private string m_strDbCol1Name = "ManualNo";
        private string m_strDbCol2Name = "ManualContents";
        #endregion

        #region [ Event Function ]

        private void frmManual_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                SM.SHM.StartManual();
            }
            else
            {
                SM.SHM.StopManual();
            }
        }
        #endregion

        #region [ Timmer Function ]

        #endregion

        #region [ CheckBox Function ]

        private void chkNameModify_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNameModify.Checked)
            {
                tbColumnTenKeyName.ReadOnly = false;
                chkNameModify.ForeColor = Color.Red;
                chkOperation.Enabled = false;
            }
            else
            {
                tbColumnTenKeyName.ReadOnly = true;
                chkNameModify.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                chkOperation.Enabled = true;
                this.XMLManual.UpdateData(); // Database update                
            }
        }

        private void chkOperation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOperation.Checked)
            {
                chkOperation.ForeColor	= Color.Red;
                chkNameModify.Enabled	= false;
            }
            else
            {
                chkOperation.ForeColor	= Properties.Settings.Default._DarkOliveGreen;
                chkNameModify.Enabled	= true;
            }
        }
        #endregion

        #region [ Button Event ]

        #endregion

        #region [ Grid Setting ]

        #region - Function

        private void initializeGrid()
        {
            // 그리드에 초기 Row 데이터를 추가함.
            int temp = XMLManual.GetDataTableCount();

            for (int i = 0; i < temp; i++)
                dgridManual.Rows.Add(i, this.XMLManual.ReadDataTable(i, this.m_strDbCol2Name));

            tbColumnTenKeyNum.ReadOnly = true;
            tbColumnTenKeyName.ReadOnly = true;
            dgridBTNTenKeyRun.ReadOnly = true;

            dgridManual.RowTemplate.Height = 50;
        }
        #endregion

        #region - Event

        private void dgridManual_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!chkNameModify.Checked) return;
            string strMsg = "";
            if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                strMsg = "Do you want to update the data name?\r\n(Initialize the existing name)";
            else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
                strMsg = "데이터이름을 업데이트하시겠습니까?\r\n(기존 이름은 초기화됩니다.)";

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            string strValue = "";

            for (int i = 0; i < (int)MANUAL.NotManual; i++)
            {
                if (EnumHelper.GetDescription((MANUAL)i) == i.ToString() ||
                    EnumHelper.GetDescription((MANUAL)i) == "")
                {
                    XMLManual.WriteDataTable(i, m_strDbCol2Name, " ");
                    strValue = "";
                }
                else
                {
                    XMLManual.WriteDataTable(i, m_strDbCol2Name, EnumHelper.GetDescription((MANUAL)i));
                    strValue = EnumHelper.GetDescription((MANUAL)i);
                }
                dgridManual.Rows[i].Cells[1].Value = strValue;
            }

            XMLManual.UpdateData();
            XMLManual.RefillData();
            
        }

        private void dgridManual_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            lbSelectManual.Text = (e.RowIndex).ToString();

            if (e.ColumnIndex != 2) return;
            if (!chkOperation.Checked)
			{
				frmMessageBox pErrMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.TenKey, "It is not Operation Mode");
				pErrMsg.ShowDialog();
				return;
			}

            int nManNum = Convert.ToInt32(lbSelectManual.Text);
            string msg = string.Format("{0},{1}", nManNum, this.XMLManual.ReadDataTable((int)nManNum, this.m_strDbCol2Name));

            //frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.TenKey, msg);
            //if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            // TenKey 매뉴얼 전달
            SM.SHM.SetManualNum((MANUAL)nManNum);

            UI.Logs.Write(LOGPART.TK, msg);
        }

        private void dgridManual_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (tbColumnTenKeyName.ReadOnly) return;

            string strNewValue = "";
            if (dgridManual[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = dgridManual[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            this.XMLManual.WriteDataTable(e.RowIndex, m_strDbCol1Name, e.RowIndex.ToString());
            this.XMLManual.WriteDataTable(e.RowIndex, m_strDbCol2Name, strNewValue);
        }
        #endregion

        #endregion

        #region [ Function ]

        #region - Private Function

        private void InitialXML()
        {
            // DB 초기화
            this.XMLManual.Initializ("Manual");
        }
        #endregion

        #region - Public Function

        public bool IsWorking()
        {
            if (chkNameModify.Checked)
                return true;
            if (chkOperation.Checked)
                return true;
            return false;
        }

        public void WorkEnd()
        {
            if (chkNameModify.Checked)
                chkNameModify.Checked = false;
            if (chkOperation.Checked)
                chkOperation.Checked = false;
        }

        public void DisableWork()
        {
            if (chkNameModify.Enabled)
                chkNameModify.Enabled = false;
            if (chkOperation.Enabled)
                chkOperation.Enabled = false;
        }

        public void EnableWork()
        {
            if (!chkNameModify.Enabled)
                chkNameModify.Enabled = true;
            if (!chkOperation.Enabled)
                chkOperation.Enabled = true;
        }
		
        public void UpdateCheckBox(PW_MODE Password_now)
        {            
            chkNameModify.Enabled = true;
            chkOperation.Enabled = true;
        }
        #endregion

        #region - Language Function 
        public void SetLanguageText()
        {
            chkNameModify.Text = Language.languageDefault.NameModify;
            chkOperation.Text = Language.languageDefault.OperationMode;
        }
        #endregion

        #endregion
    }
}
