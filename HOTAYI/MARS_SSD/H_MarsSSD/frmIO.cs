using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmIO : Form
    {
        public frmIO()
        {
            InitializeComponent();
            InitialXML();
            InitialIOGrid();
            InitialInputCombobox();
            InitialOutputCombobox();
            InitialSearch();
        }

        #region [ Variables ]
        private XML XMLIOConfig = new XML();
        private string strIOConfigColName0 = "InputNum";
        private string strIOConfigColName1 = "OutputNum";

        private XML XMLIO = new XML();
        private string strIOColName0 = "InputName";
        private string strIOColName1 = "OutputName";


        public ushort[] ushInput = new ushort[Define.MAX_IOCH_NUM];
        public ushort[] ushOutput = new ushort[Define.MAX_IOCH_NUM];

        private uint uiNowInputCnt = new uint();
        private uint uiNowOutputCnt = new uint();

        private bool bTimerRunning = false;

        private int nInPutSelectIndex = 0;
        private int nOutPutSelectIndex = 0;
        #endregion

        #region [ Event Function ]

        private void frmIO_VisibleChanged(object sender, EventArgs e)
        {
            if (UI.PageIO.Visible)
                //tmrIO.Enabled = Global.bConnectSeq;
                tmrIO.Enabled = true;
            else
                tmrIO.Enabled = false;
        }

        private void txtSearchValue_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {                
                if (pnSearch.Location.X < 200)
                    FindNextInput();
                else if (pnSearch.Location.X > 200)
                    FindNextOutput();
            }

            if (e.KeyData == Keys.Escape)
            {
                pnSearch.Visible = false;
            }
        }
        #endregion

        #region [ Timmer Function ]

        private void tmrIO_Tick(object sender, EventArgs e)
        {
            if (chkIOModify.Checked) return;

            bTimerRunning = true;

            //Global.COM.SetCommand(CMD.MS_WRITE_IO);

            SM.SHM.GetInputData(ref ushInput);
            SetInputValue(ushInput[nInPutSelectIndex]);

            SM.SHM.GetOutputData(ref ushOutput);
            SetOutputValue(ushOutput[nOutPutSelectIndex]);

            bTimerRunning = false;

        }
        #endregion

        #region [ CheckBox Event ]

        private void chkIOModify_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIOModify.Checked)
            {
                this.tbColumnInputName.ReadOnly = false;
                this.tbColumnOutputName.ReadOnly = false;
                chkIOModify.ForeColor = Color.Red;
                cbSelInput.Enabled = false;
                cbSelOutput.Enabled = false;
                chkOutputEnable.Enabled = false;
            }
            else
            {
                XMLIO.UpdateData();

                this.tbColumnInputName.ReadOnly = true;
                this.tbColumnOutputName.ReadOnly = true;
                chkIOModify.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                cbSelInput.Enabled = true;
                cbSelOutput.Enabled = true;
                chkOutputEnable.Enabled = true;
            }
        }

        private void chkOutputEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOutputEnable.Checked)
            {
                chkOutputEnable.ForeColor = Color.Red;
                chkIOModify.Enabled = false;

            }
            else
            {
                chkOutputEnable.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                chkIOModify.Enabled = true;
            }
        }
        #endregion

        #region [ Button Event ]

        #endregion

        #region [ ComboBox Function ]

        private void cbSelInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iStart = cbSelInput.SelectedIndex * Define.MAX_IOBIT_NUM;

            nInPutSelectIndex = cbSelInput.SelectedIndex;

            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {
                this.dgridInput.Rows[i].Cells[1].Value = XMLIO.ReadDataTable(iStart + i, strIOColName0);
            }

            // add input value update
            // here
            SetInputValue(ushInput[cbSelInput.SelectedIndex]);
        }

        private void cbSelOutput_SelectedValueChanged(object sender, EventArgs e)
        {
            int iStart = cbSelOutput.SelectedIndex * Define.MAX_IOBIT_NUM;

            nOutPutSelectIndex = cbSelOutput.SelectedIndex;

            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {
                this.dgridOutput.Rows[i].Cells[1].Value = XMLIO.ReadDataTable(iStart + i, strIOColName1);
            }

            // add output value update
            // here
            SetOutputValue(ushOutput[cbSelOutput.SelectedIndex]);
        }
        #endregion

        #region [ Grid Function ]

        #region - Grid Setting
        private void InitialIOGrid()
        {
            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
                this.dgridInput.Rows.Add(i, XMLIO.ReadDataTable(i, strIOColName0), "OFF");

            // 그리드 Row에 기본 간격을 설정
            this.dgridInput.RowTemplate.Height = 38;

            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
                this.dgridOutput.Rows.Add(i, XMLIO.ReadDataTable(i, strIOColName1), "OFF");

            // 그리드 Row에 기본 간격을 설정
            this.dgridOutput.RowTemplate.Height = 38;

            this.tbColumnInputName.ReadOnly = true;
            this.tbColumnOutputName.ReadOnly = true;
        }

        private void SetInputValue(UInt16 data)
        {
            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {
                if (((data >> i) & 0x0001) == 0x0001)
                {
                    this.dgridInput.Rows[i].Cells[2].Value = "ON";
                    this.dgridInput.Rows[i].Cells[2].Style.BackColor = Color.Yellow;
                    this.dgridInput.Rows[i].Cells[2].Style.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    this.dgridInput.Rows[i].Cells[2].Value = "OFF";
                    this.dgridInput.Rows[i].Cells[2].Style.BackColor = Color.White;
                    this.dgridInput.Rows[i].Cells[2].Style.SelectionBackColor = Color.White;
                }
            }
        }

        private void SetOutputValue(int num, bool bon)
        {
            if (bon)
            {
                this.dgridOutput.Rows[num].Cells[2].Value = "ON";
                //cell.ReadOnly = true;
            }
            else
            {
                this.dgridOutput.Rows[num].Cells[2].Value = "OFF";
                //cell.ReadOnly = false;
            }
        }

        private void SetOutputValue(UInt16 data)
        {
            for (int i = 0; i < Define.MAX_IOBIT_NUM; i++)
            {
                if (((data >> i) & 0x0001) == 0x0001)
                {
                    this.dgridOutput.Rows[i].Cells[2].Value = "ON";
                    this.dgridOutput.Rows[i].Cells[2].Style.BackColor = Color.Yellow;
                    this.dgridOutput.Rows[i].Cells[2].Style.SelectionBackColor = Color.Yellow;
                }
                else
                {
                    this.dgridOutput.Rows[i].Cells[2].Value = "OFF";
                    this.dgridOutput.Rows[i].Cells[2].Style.BackColor = Color.White;
                    this.dgridOutput.Rows[i].Cells[2].Style.SelectionBackColor = Color.White;
                }
            }
        }
        #endregion

        #region - Grid Event

        private void dgridInput_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cbSelInput.SelectedIndex < 0) return;
            if (e.ColumnIndex != 1) return;
            if (e.RowIndex == -1) return;
            if (this.tbColumnInputName.ReadOnly == true) return;

            string strNewValue = "";
            if (this.dgridInput[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridInput[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            int idx = e.RowIndex + (cbSelInput.SelectedIndex * 16);
            this.XMLIO.WriteDataTable(idx, strIOColName0, strNewValue);
        }

        private void dgridOutput_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (cbSelOutput.SelectedIndex < 0) return;
            if (e.ColumnIndex != 1) return;
            if (e.RowIndex == -1) return;
            if (this.tbColumnOutputName.ReadOnly == true) return;

            string strNewValue = "";
            if (this.dgridOutput[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridOutput[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            int idx = e.RowIndex + (cbSelOutput.SelectedIndex * 16);
            this.XMLIO.WriteDataTable(idx, strIOColName1, strNewValue);
        }

        private void dgridOutput_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!chkOutputEnable.Checked) return;
            if (e.RowIndex == -1) return;
            while (bTimerRunning) ;

            tmrIO.Stop();

            int iCol = e.ColumnIndex;
            int iRow = e.RowIndex;
            int ch = cbSelOutput.SelectedIndex;

            if (iCol != 2) return;

            if (((ushOutput[ch] >> iRow) & 0x0001) == 0x0001)
            {
                ushOutput[ch] &= Convert.ToUInt16((~(0x0001 << iRow)) & 0xFFFF);
            }
            else
            {
                ushOutput[ch] |= Convert.ToUInt16(0x0001 << iRow);
            }

            // Cell 더블 클릭시 OutPut 반대로 변경 ex) 실린더 UP => Down으로 변경(선택사항) -LWJ
            //Global.COM.SetCommand(CMD.MS_SET_OUTPUT, (uint)(ch * 16 + iRow));
            SM.SHM.SetOutputData((DO)((ch * 16) + iRow));

            tmrIO.Start();
        }
        private void dgridOutput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.XButton2))
            {
                pnSearch.Location = new Point(706, 77);
                pnSearch.Visible = true;
                txtSearchValue.Focus();
            }

            if (e.KeyChar == (char)Keys.Escape)
                pnSearch.Visible = false;
        }
        private void dgridInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)(Keys.XButton2))
            {
                pnSearch.Location = new Point(144, 77);
                pnSearch.Visible = true;
                txtSearchValue.Focus();
            }

            if (e.KeyChar == (char)Keys.Escape)
                pnSearch.Visible = false;
        }

        
        private void  dgridOutput_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!chkIOModify.Checked) return;
            string strMsg = "";
            if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                strMsg = "Do you want to update the data name?\r\n(Initialize the existing name)";
            else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
                strMsg = "데이터이름을 업데이트하시겠습니까?\r\n(기존 이름은 초기화됩니다.)";

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;
            

            for (int i = 0; i < (int)DO.Total; i++)
            {
                if (EnumHelper.GetDescription((DO)i).Contains("None"))
                    XMLIO.WriteDataTable(i, strIOColName1, " ");
                else
                    XMLIO.WriteDataTable(i, strIOColName1, EnumHelper.GetDescription((DO)i));
                
                if (i >= Define.MAX_IOBIT_NUM *  cbSelOutput.SelectedIndex  && i < Define.MAX_IOBIT_NUM *  (cbSelOutput.SelectedIndex+1)  )
                    dgridOutput.Rows[i%Define.MAX_BIT_NUM].Cells[1].Value = XMLIO.ReadDataTable(i, strIOColName1);
            }

            XMLIO.UpdateData();
            XMLIO.RefillData();
        }

        private void dgridInput_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!chkIOModify.Checked) return;
            string strMsg = "";
            if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                strMsg = "Do you want to update the data name?\r\n(Initialize the existing name)";
            else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
                strMsg = "데이터이름을 업데이트하시겠습니까?\r\n(기존 이름은 초기화됩니다.)";

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;
            
            for (int i = 0; i < (int)DI.Total; i++)
            {
                if (EnumHelper.GetDescription((DI)i).Contains("None"))
                    XMLIO.WriteDataTable(i, strIOColName0, " ");
                else
                    XMLIO.WriteDataTable(i, strIOColName0, EnumHelper.GetDescription((DI)i));

                if ( i >= Define.MAX_IOBIT_NUM *  cbSelInput.SelectedIndex  && i < Define.MAX_IOBIT_NUM *  (cbSelInput.SelectedIndex+1)  )
                    dgridInput.Rows[i%Define.MAX_BIT_NUM].Cells[1].Value = XMLIO.ReadDataTable(i, strIOColName0);
            }
            XMLIO.UpdateData();
            XMLIO.RefillData();
        }

        #endregion
        #endregion

        #region [ Function ]

        #region - Private Funtion

        private void InitialIOData()
        {
            for (int i = 0; i < Define.MAX_IOCH_NUM; i++)
            {
                ushInput[i] = new ushort();
                ushOutput[i] = new ushort();
                ushInput[i] = 0x0000;
                ushOutput[i] = 0x0000;
            }
        }

        private void InitialInputCombobox()
        {
            cbSelInput.Items.Clear();

            int chnum = Convert.ToInt32(XMLIOConfig.ReadDataTable(0, strIOConfigColName0));

            this.uiNowInputCnt = (uint)chnum;

            if (chnum > Define.MAX_IOCH_NUM)
                chnum = Define.MAX_IOCH_NUM;
            if (chnum < 1)
                chnum = 1;

            for (int i = 0; i < chnum; i++)
            {
                if (i < 10)
                    cbSelInput.Items.Add("INPUT CH : 0" + Convert.ToInt32(i));
                else
                    cbSelInput.Items.Add("INPUT CH : " + Convert.ToInt32(i));
            }

            cbSelInput.SelectedIndex = 0;
        }

        private void InitialOutputCombobox()
        {
            cbSelOutput.Items.Clear();

            int chnum = Convert.ToInt32(XMLIOConfig.ReadDataTable(0, strIOConfigColName1));

            this.uiNowOutputCnt = (uint)chnum;

            if (chnum > Define.MAX_IOCH_NUM)
                chnum = Define.MAX_IOCH_NUM;
            if (chnum < 1)
                chnum = 1;

            for (int i = 0; i < chnum; i++)
            {
                if (i < 10)
                    cbSelOutput.Items.Add("OUTPUT CH : 0" + Convert.ToInt32(i));
                else
                    cbSelOutput.Items.Add("OUTPUT CH : " + Convert.ToInt32(i));
            }

            cbSelOutput.SelectedIndex = 0;
        }

        private void InitialXML()
        {
            this.XMLIOConfig.Initializ("IOConfig");
            this.XMLIO.Initializ("IO");
        }

        private List<string> m_Outlist = new List<string>();
        private List<string> m_Inlist = new List<string>();        
        private void InitialSearch()
        {
            for (int i = 0; i < cbSelInput.Items.Count * Define.MAX_IOBIT_NUM; i++)
            {
                m_Inlist.Add(XMLIO.ReadDataTable(i, strIOColName0));
            }

            for (int i = 0; i < cbSelOutput.Items.Count * Define.MAX_IOBIT_NUM; i++)
            {
                m_Outlist.Add(XMLIO.ReadDataTable(i, strIOColName1));
            }
        }
        private void FindNextInput()
        {
            try
            {
                if (m_Inlist.Count < 1 || dgridInput == null) return;
                if (string.IsNullOrEmpty(txtSearchValue.Text)) return;

                Regex reg = new Regex(txtSearchValue.Text, RegexOptions.IgnoreCase);
                Match m;
                int nRow = 0;

                if (dgridInput.CurrentCell == null)
                {
                    nRow = 0;
                }
                else
                {
                    nRow = dgridInput.CurrentCell.RowIndex + (nInPutSelectIndex * Define.MAX_IOBIT_NUM) + 1;
                }

                for (int i = nRow; i < m_Inlist.Count; i++)
                {
                    m = reg.Match(m_Inlist[i]);
                    if (m.Success)
                    {
                        int ch = i / Define.MAX_IOBIT_NUM;
                        int num = i % Define.MAX_IOBIT_NUM;
                        cbSelInput.SelectedIndex = ch;

                        dgridInput.CurrentCell = dgridInput[1, num];
                        return;
                    }

                    cbSelInput.SelectedIndex = 0;
                    dgridInput.CurrentCell = dgridInput[1, 0];
                }
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }
        private void FindNextOutput()
        {
            try
            {
                if (m_Outlist.Count < 1 || dgridOutput == null) return;
                if (string.IsNullOrEmpty(txtSearchValue.Text)) return;

                Regex reg = new Regex(txtSearchValue.Text, RegexOptions.IgnoreCase);
                Match m;
                int nRow = 0;

                if (dgridOutput.CurrentCell == null)
                {
                    nRow = 0;
                }
                else
                {
                    nRow = dgridOutput.CurrentCell.RowIndex + (nOutPutSelectIndex * Define.MAX_IOBIT_NUM) + 1;
                }
                                
                for (int i = nRow; i < m_Outlist.Count; i++)
                {
                    m = reg.Match(m_Outlist[i]);
                    if (m.Success)
                    {
                        int ch = i / Define.MAX_IOBIT_NUM;
                        int num = i % Define.MAX_IOBIT_NUM;
                        cbSelOutput.SelectedIndex = ch;

                        dgridOutput.CurrentCell = dgridOutput[1, num];                        
                        return;
                    }

                    cbSelOutput.SelectedIndex = 0;
                    dgridOutput.CurrentCell = dgridOutput[1, 0];                    
                }
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }
        #endregion

        #region - Public Funtion

        public bool IsWorking()
        {
            if (chkOutputEnable.Checked || chkIOModify.Checked)
                return true;
            return false;
        }

        public void WorkEnd()
        {
            if (chkOutputEnable.Checked)
                chkOutputEnable.Checked = false;

            if (chkIOModify.Checked)
                chkIOModify.Checked = false;
        }

        public void DisableWork()
        {
            if (chkIOModify.Enabled)
                chkIOModify.Enabled = false;
            if (chkOutputEnable.Enabled)
                chkOutputEnable.Enabled = false;
        }

        public void EnableWork()
        {
            if (!chkIOModify.Enabled)
                chkIOModify.Enabled = true;
            if (!chkOutputEnable.Enabled)
                chkOutputEnable.Enabled = true;
        }

        // ch : 0~7 , bit : 0~7
        public bool IsInput(int ch, int bit)
        {
            if (ch > 15) ch = 15;
            if (ch < 0) ch = 0;
            if (bit > 15) bit = 15;
            if (bit < 0) bit = 0;

            if (((ushInput[ch] >> bit) & 0x0001) == 0x0001)
                return true;
            return false;
        }

        // ch : 0~15 , bit : 0~15
        public bool IsOutput(int ch, int bit)
        {
            if (ch > 15) ch = 15;
            if (ch < 0) ch = 0;
            if (bit > 15) bit = 15;
            if (bit < 0) bit = 0;

            if (((ushOutput[ch] >> bit) & 0x0001) == 0x0001)
                return true;
            return false;
        }
        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            chkIOModify.Text = Language.languageDefault.NameModify;
            chkOutputEnable.Text = Language.languageDefault.OutPutEnable;

			dgridInput.Columns[0].HeaderText = Language.languageDefault.DGRID_HD_No;
			dgridInput.Columns[1].HeaderText = Language.languageDefault.DGRID_HD_Input;
			dgridInput.Columns[2].HeaderText = Language.languageDefault.DGRID_HD_On;

			dgridOutput.Columns[0].HeaderText = Language.languageDefault.DGRID_HD_No;
			dgridOutput.Columns[1].HeaderText = Language.languageDefault.DGRID_HD_Output;
			dgridOutput.Columns[2].HeaderText = Language.languageDefault.DGRID_HD_On;

        }




        #endregion

        #endregion

        
    }
}
