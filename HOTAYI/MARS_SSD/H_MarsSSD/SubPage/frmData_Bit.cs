using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmData_Bit : Form
    {
        public frmData_Bit()
        {
            InitializeComponent();

            InitialXML();
            InitialGrid();
            InitialBitCombobox();
            InitialBitData();

            tmrBit.Start();
        }

        #region [ Variables ]
        private XML XMLBit = new XML();
        private string strBitCol0 = "DataBitName";

        public ushort[] ushBit = new ushort[Define.MAX_BITCH_NUM];

        private bool bTimerRunning = false;
        #endregion

        #region [ Event Function ]

        #endregion

        #region [ Timmer Function ]

        private void tmrBit_Tick(object sender, EventArgs e)
        {
            bTimerRunning = true;

            if (!chkBitMod.Checked)
            {
                SM.SHM.GetBitData(ref ushBit);
                SetBitValue(ushBit[cbSelBit.SelectedIndex]);
            }

            bTimerRunning = false;
        }
        #endregion

        #region [ ComboBox Event ]

        private void cbSelBit_SelectedIndexChanged(object sender, EventArgs e)
        {
			dgridBit.ClearSelection();
            int iStart = cbSelBit.SelectedIndex * Define.MAX_BIT_NUM;

            for (int i = 0; i < Define.MAX_BIT_NUM; i++)
            {
                this.dgridBit.Rows[i].Cells[1].Value = XMLBit.ReadDataTable(iStart + i, strBitCol0);
            }

            // add input value update
            // here
            SetBitValue(ushBit[cbSelBit.SelectedIndex]);
        }
        #endregion

        #region [ CheckBox Event ]


        private void chkBitMod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBitMod.Checked)
            {
                chkBitMod.ForeColor = Color.Red;
                this.tbColumnBitName.ReadOnly = false;

                chkBitEnable.Enabled = false;
                cbSelBit.Enabled = false;
            }
            else
            {
                chkBitMod.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                this.tbColumnBitName.ReadOnly = true;

                chkBitEnable.Enabled = true;
                cbSelBit.Enabled = true;

                XMLBit.UpdateData();
            }
        }

        private void chkBitEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBitEnable.Checked)
            {
                chkBitEnable.ForeColor = Color.Red;
                chkBitMod.Enabled = false;
				SelectionColorEnable();
            }
            else
            {
                chkBitEnable.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                chkBitMod.Enabled = true;
				SelectionColorDisable();
            }
        }

        #endregion

        #region [ Button Event ]

        #endregion

        #region [ Grid Setting ]

        #region - Function
        private void InitialGrid()
        {
			int nVisibleRowCount = 16;
            this.dgridBit.RowTemplate.Height = dgridBit.Height / (nVisibleRowCount + 1);
            dgridBit.ColumnHeadersHeight = dgridBit.Height - dgridBit.RowTemplate.Height * nVisibleRowCount;

            for (int i = 0; i < Define.MAX_BIT_NUM; i++)
            {
                int calidx = i + (0 * Define.MAX_BIT_NUM);
                this.dgridBit.Rows.Add(i, XMLBit.ReadDataTable(calidx, strBitCol0), "FALSE");
            }
			SelectionColorDisable();


			this.tbColumnBitName.ReadOnly = true;
        }

        private void SetBitValue(UInt16 data)
        {
            for (int i = 0; i < Define.MAX_BIT_NUM; i++)
            {
                if (((data >> i) & 0x0001) == 0x0001)
                {
                    this.dgridBit.Rows[i].Cells[2].Value = "ON";
                    this.dgridBit.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    //this.dgridBit.Rows[i].Cells[2].ReadOnly = true;
                }
                else
                {
                    this.dgridBit.Rows[i].Cells[2].Value = "OFF";
                    this.dgridBit.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    //this.dgridBit.Rows[i].Cells[2].ReadOnly = false;
                }
            }
        }
		
		private void SelectionColorEnable()
		{
			dgridBit.Columns[0].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
			dgridBit.Columns[0].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dgridBit.Columns[1].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
			dgridBit.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		}

		private void SelectionColorDisable()
		{
			dgridBit.Columns[0].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridBit.Columns[0].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dgridBit.Columns[1].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridBit.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dgridBit.Columns[2].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridBit.Columns[2].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dgridBit.ClearSelection();
		}


        #endregion

        #region - Event

        private void dgridBit_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (this.dgridBit.Columns[1].ReadOnly) return;
            if (e.ColumnIndex != 1) return;

            string strNewValue = "";
            if (this.dgridBit[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridBit[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            int calidx = e.RowIndex + (cbSelBit.SelectedIndex * Define.MAX_BITCH_NUM);

            XMLBit.WriteDataTable(calidx, strBitCol0, strNewValue);

        }

        private void dgridBit_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!chkBitEnable.Checked) return;
            if (e.RowIndex == -1) return;
            while (bTimerRunning) ;

            tmrBit.Stop();

            int iCol = e.ColumnIndex;
            int iRow = e.RowIndex;
            int ch = cbSelBit.SelectedIndex;

            if (iCol != 2) return;
			//@@ if (ch == 0 || ch == 1 || ch == 2) return; // Channel 0,1,2 는 상태 못바꾸도록
			if (iCol == 2  &&  iRow == 7  && ch == 1)
			{
				GLog.Add(string.Format("BIT! InspLock = {0}", !SM.SHM.IsBitData(BIT.InspLock)));
			}
            if (((ushBit[ch] >> iRow) & 0x0001) == 0x0001)
            {
                ushBit[ch] &= Convert.ToUInt16((~(0x0001 << iRow)) & 0xFFFF);
            }
            else
            {
                ushBit[ch] |= Convert.ToUInt16(0x0001 << iRow);
            }

            SetBitValue(ushBit[ch]);
            SM.SHM.SetBitData((uint)ch, ushBit[ch]);
            //Global.SHM.WriteSHM_Bit();

            this.dgridBit.Rows[iRow].Selected = false;

            // Bit Read -LWJ
            //Global.COM.SetCommand(CMD.MS_READ_BIT);

            tmrBit.Start();
        }

        private void dgridBit_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!chkBitMod.Checked) return;

            string strMsg = "";
            if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                strMsg = "Do you want to update the data name?\r\n(Initialize the existing name)";
            else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
                strMsg = "데이터이름을 업데이트하시겠습니까?\r\n(기존 이름은 초기화됩니다.)";

            frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;


            for (int i = 0; i < (int)BIT.BitsFF; i++)
            {
                if (EnumHelper.GetDescription((BIT)i) == i.ToString())
                    XMLBit.WriteDataTable(i, strBitCol0, " ");
                else
                    XMLBit.WriteDataTable(i, strBitCol0, EnumHelper.GetDescription((BIT)i));
                
                if (i >= Define.MAX_BIT_NUM *  cbSelBit.SelectedIndex  && i < Define.MAX_BIT_NUM *  (cbSelBit.SelectedIndex+1))
                    this.dgridBit.Rows[i % Define.MAX_BIT_NUM].Cells[1].Value = XMLBit.ReadDataTable( i, strBitCol0);
            }
            XMLBit.UpdateData();
            XMLBit.RefillData();
        }
        #endregion

        #endregion

        #region [ Function ]

        #region - Private Function

        private void InitialXML()
        {
            XMLBit.Initializ("DataBit");
        }

        private void InitialBitData()
        {
            int i;
            for (i = 0; i < Define.MAX_BITCH_NUM; i++)
            {
                ushBit[i] = new ushort();
                ushBit[i] = 0;
                // for test
                //ushBit[i] = 0x5555;
            }
        }

        private void InitialBitCombobox()
        {
            cbSelBit.Items.Clear();

            for (int i = 0; i < Define.MAX_BITCH_NUM; i++)
            {
                if (i < 10)
                    cbSelBit.Items.Add("BIT CH : 0" + Convert.ToInt32(i));
                else
                    cbSelBit.Items.Add("BIT CH : " + Convert.ToInt32(i));
            }

            cbSelBit.SelectedIndex = 0;
        }
        #endregion

        #region - Public Function

        public void SetBitData(int ch, int bit, bool bOn)
        {
            if (!bOn)
            {
                ushBit[ch] &= Convert.ToUInt16((~(0x0001 << bit)) & 0xFFFF);
            }
            else
            {
                ushBit[ch] |= Convert.ToUInt16(0x0001 << bit);
            }
            if (ch == cbSelBit.SelectedIndex)
                SetBitValue(ushBit[ch]);
            SM.SHM.SetBitData((uint)ch, ushBit[ch]);
            //Global.SHM.WriteSHM_Bit();

            // Bit Read -LWJ
            //Global.COM.SetCommand(CMD.MS_READ_BIT);
        }

        public bool IsBitData(int ch, int bitnum)
        {
            if (ch >= Define.MAX_BITCH_NUM)
                ch = Define.MAX_BITCH_NUM - 1;
            else if (ch < 0)
                ch = 0;

            if (bitnum >= Define.MAX_BIT_NUM)
                bitnum = Define.MAX_BIT_NUM - 1;
            else if (bitnum < 0)
                bitnum = 0;

            if (((ushBit[ch] >> bitnum) & 0x0001) == 0x0001)
                return true;
            else
                return false;
        }

        public bool IsBitData(BIT _bit)
        {
            int data = Convert.ToInt32(_bit);

            int ch = data / 16;
            int bitnum = data % 16;

            if (ch >= Define.MAX_BITCH_NUM)
                ch = Define.MAX_BITCH_NUM - 1;
            else if (ch < 0)
                ch = 0;

            if (bitnum >= Define.MAX_BIT_NUM)
                bitnum = Define.MAX_BIT_NUM - 1;
            else if (bitnum < 0)
                bitnum = 0;

            if (((ushBit[ch] >> bitnum) & 0x0001) == 0x0001)
                return true;
            else
                return false;
        }

        public bool IsWorking()
        {
            if (chkBitMod.Checked || chkBitEnable.Checked)
                return true;
            return false;
        }
		
        public void UpdateCheckBox(PW_MODE Password_Now)
        {
            if (!chkBitMod.Checked)
                chkBitEnable.Enabled = true;
            else
                chkBitEnable.Enabled = false;

            if (!chkBitEnable.Checked)
                chkBitMod.Enabled = true;
            else
                chkBitMod.Enabled = false;            
        }

        public void WorkEnd()
        {
            if (chkBitMod.Checked)
                chkBitMod.Checked = false;

            if (chkBitEnable.Checked)
                chkBitEnable.Checked = false;
        }

        public void DisableWork()
        {
            if (chkBitMod.Enabled)
                chkBitMod.Enabled = false;
            if (chkBitEnable.Enabled)
                chkBitEnable.Enabled = false;
        }

        public void EnableWork()
        {
            if (!chkBitMod.Enabled)
                chkBitMod.Enabled = true;
            if (!chkBitEnable.Enabled)
                chkBitEnable.Enabled = true;
        }
        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            chkBitMod.Text = Language.languageDefault.NameModify;
            chkBitEnable.Text = Language.languageDefault.Enable;
        }
        #endregion

        #endregion

       
    }
}
