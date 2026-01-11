using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmData_Useskip : Form
    {
        public frmData_Useskip()
        {
            InitializeComponent();
            InitialXML();
            InitialData();
            InitialGrid();
			InitUsePart();
        }

        #region [ Variables ]
        private XML XMLUseskip = new XML();
		private XML XMLUsePart = new XML();
        private string strUseskipCol0 = "UseSkipName";
        private string strUseskipCol1 = "UseSkipValue";
        private string strUsePartCol0 = "UsePartMode";

        public ushort[] ushUseSkip = new ushort[2];
        #endregion

        #region [ Event Function ]

        #endregion

        #region [ Timmer Function ]

        #endregion

        #region [ CheckBox Event ]

        private void chkUseskipEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseskipEnable.Checked)
            {
                chkUseskipEnable.ForeColor = Color.Red;
                chkUseskipMod.Enabled = false;
				chkDefault.Enabled = true;
				SelectionColorEnable();

            }
            else
            {
                chkUseskipEnable.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                chkUseskipMod.Enabled = true;
				chkDefault.Enabled = false;

				UpdateUseskipValue();
                
				SelectionColorDisable();
                SM.SHM.SetUseSkipData(ushUseSkip);
            }
        }

        private void chkUseskipMod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseskipMod.Checked)
            {
                chkUseskipMod.ForeColor = Color.Red;
                chkUseskipEnable.Enabled = false;

                this.tbColumnUseSkip1Name.ReadOnly = false;
                this.tbColumnUseSkip2Name.ReadOnly = false;
            }
            else
            {
                chkUseskipMod.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                chkUseskipEnable.Enabled = true;

                this.tbColumnUseSkip1Name.ReadOnly = true;
                this.tbColumnUseSkip2Name.ReadOnly = true;
                XMLUseskip.UpdateData();
            }
        }

		//기본 디폴트값으로 변경
		private void chkDefault_CheckedChanged(object sender, EventArgs e)
		{
			if (chkDefault.Checked) chkDefault.Checked = false;

			WriteUseskip(USESKIP.PrevLotEndWait,false );
			WriteUseskip(USESKIP.LoadPnPStop,false );
			WriteUseskip(USESKIP.InspOfflineJobCheck,true );
			WriteUseskip(USESKIP.InspSkipResultNG,true );
			WriteUseskip(USESKIP.InspSkipResultRandom,false );
			WriteUseskip(USESKIP.InspResultAlwaysOK,false );
			WriteUseskip(USESKIP.InspDebugPlate1,false );
			WriteUseskip(USESKIP.InspDebugPlate2,false );
			WriteUseskip(USESKIP.TopInspection,true );
			WriteUseskip(USESKIP.BtmInspection,true );
			WriteUseskip(USESKIP.SendRejectProdId,true );
			WriteUseskip(USESKIP.SGEM,true );
			WriteUseskip(USESKIP.BarcodeRead,true );
			WriteUseskip(USESKIP.Safety,true );
			WriteUseskip(USESKIP.RunStateLog,false );
			WriteUseskip(USESKIP.DRYRUN,false );

			for (UsePartName part = UsePartName.Picker1; part < UsePartName.Max; part++)
			{
				SM.SHM.SetUsePart(part, UsePartMode.Enable);

				if (XMLUsePart.WriteDataTable((int)part, strUsePartCol0, (int)UsePartMode.Enable) == false)
				{
					XMLUsePart.AddDataTable((int)part, strUsePartCol0, (int)UsePartMode.Enable);
				}
			}
			
			XMLUsePart.UpdateData();
			UpdateGridUsePart();

		}
        #endregion

        #region [ Button Event ]

        #endregion

        #region [ Grid Setting ]

        #region - function

        private void InitialGrid()
        {
			int nVisibleRowCount = 16;
            dgridUseskip1.RowTemplate.Height = dgridUseskip1.Height / (nVisibleRowCount + 1);
            dgridUseskip1.ColumnHeadersHeight = dgridUseskip1.Height - dgridUseskip1.RowTemplate.Height * nVisibleRowCount;
			
            dgridUseskip2.RowTemplate.Height = dgridUseskip2.Height / (nVisibleRowCount + 1);
            dgridUseskip2.ColumnHeadersHeight = dgridUseskip2.Height - dgridUseskip2.RowTemplate.Height * nVisibleRowCount;

            for (int i = 0; i < Define.MAX_USESKIP_NUM / 2; i++)
                this.dgridUseskip1.Rows.Add(i, XMLUseskip.ReadDataTable(i, strUseskipCol0), XMLUseskip.ReadDataTable(i, strUseskipCol1));

            this.dgridUseskip1.RowTemplate.Height = 35;

            for (int i = 0; i < Define.MAX_USESKIP_NUM / 2; i++)
            {
                int calidx = i + (Define.MAX_USESKIP_NUM / 2);
                this.dgridUseskip2.Rows.Add(calidx, XMLUseskip.ReadDataTable(calidx, strUseskipCol0), XMLUseskip.ReadDataTable(calidx, strUseskipCol1));
            }
			
			SelectionColorDisable();

			this.dgridUseskip2.RowTemplate.Height = 35;

            this.tbColumnUseSkip1Name.ReadOnly = true;
            this.tbColumnUseSkip2Name.ReadOnly = true;

            ReadUseskipValue(); // read from "db data" to "variable"
            DispUseskipValue(); // Display value
        }        

        // 변수에 내용을 화면 그리드에 디스플레이함.
        private void DispUseskipValue()
        {
            for (int i = 0; i < Define.MAX_USESKIP_NUM / 2; i++)
            {
                if (IsUseSkip(ushUseSkip[0], i))                
                {
                    this.dgridUseskip1.Rows[i].Cells[2].Value = "True";
                    this.dgridUseskip1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    //this.dgridUseskip1.Rows[i].Cells[2].ReadOnly = true;
                }
                else
                {
                    this.dgridUseskip1.Rows[i].Cells[2].Value = "False";
                    this.dgridUseskip1.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    //this.dgridUseskip1.Rows[i].Cells[2].ReadOnly = false;
                }
                
                if (IsUseSkip(ushUseSkip[1], i))
                {
                    this.dgridUseskip2.Rows[i].Cells[2].Value = "True";
                    this.dgridUseskip2.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                    //this.dgridUseskip2.Rows[i].Cells[2].ReadOnly = true;
                }
                else
                {
                    this.dgridUseskip2.Rows[i].Cells[2].Value = "False";
                    this.dgridUseskip2.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    //this.dgridUseskip2.Rows[i].Cells[2].ReadOnly = false;
                }
            }           
        }

		private void SelectionColorEnable()
		{
			dgridUseskip1.Columns[0].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
			dgridUseskip1.Columns[0].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dgridUseskip1.Columns[1].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
			dgridUseskip1.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;

			dgridUseskip2.Columns[0].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
			dgridUseskip2.Columns[0].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dgridUseskip2.Columns[1].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
			dgridUseskip2.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;

			dgridUsePart.Columns[0].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
			dgridUsePart.Columns[0].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
			dgridUsePart.Columns[1].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
			dgridUsePart.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
		}

		private void SelectionColorDisable()
		{

			dgridUseskip1.Columns[0].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridUseskip1.Columns[0].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dgridUseskip1.Columns[1].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridUseskip1.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dgridUseskip1.Columns[2].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridUseskip1.Columns[2].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;

			dgridUseskip2.Columns[0].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridUseskip2.Columns[0].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dgridUseskip2.Columns[1].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridUseskip2.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dgridUseskip2.Columns[2].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridUseskip2.Columns[2].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;

			dgridUsePart.Columns[0].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridUsePart.Columns[0].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dgridUsePart.Columns[1].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridUsePart.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
			dgridUsePart.Columns[2].DefaultCellStyle.SelectionBackColor = Color.Transparent;
			dgridUsePart.Columns[2].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;

			dgridUseskip1.ClearSelection();
			dgridUseskip2.ClearSelection();
			dgridUsePart.ClearSelection();
		}
        #endregion

        #region - Event

        private void dgridUseskip1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!chkUseskipEnable.Checked) return;
            if (e.RowIndex == -1) return;

            int iCol = e.ColumnIndex;
            int iRow = e.RowIndex;

            if (iCol != 2) return;

            if (IsUseSkip(ushUseSkip[0], iRow) == true)
                Off(0, iRow);
            else
                On(0, iRow);

            this.dgridUseskip1.Rows[iRow].Selected = false;

            DispUseskipValue();
        }

        private void dgridUseskip1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!chkUseskipMod.Checked) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex != 1) return;

            string strNewValue = "";
            if (this.dgridUseskip1[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridUseskip1[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            XMLUseskip.WriteDataTable(e.RowIndex, strUseskipCol0, strNewValue);
        }

        private void dgridUseskip2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!chkUseskipEnable.Checked) return;
            if (e.RowIndex == -1) return;

            int iCol = e.ColumnIndex;
            int iRow = e.RowIndex;

            if (iCol != 2) return;

			if		(iRow == (int)USESKIP.InspDebugPlate1%16)		WriteUseskip(USESKIP.InspDebugPlate2, false);
            else if (iRow == (int)USESKIP.InspDebugPlate2%16)		WriteUseskip(USESKIP.InspDebugPlate1, false);

            if (IsUseSkip(ushUseSkip[1], iRow) == true)
                Off(1, iRow);
            else
                On(1, iRow);

            this.dgridUseskip2.Rows[iRow].Selected = false;          
			
			if (iRow == (int)USESKIP.InspDebugPlate1%16 || 
				iRow == (int)USESKIP.InspDebugPlate2%16)
			{
				UsePartMode mode = UsePartMode.Enable;
				if (IsUseSkip(ushUseSkip[1], (int)USESKIP.InspDebugPlate1%16) == true  || 
					IsUseSkip(ushUseSkip[1], (int)USESKIP.InspDebugPlate2%16) == true)   
					mode = UsePartMode.Disable;
				
				XMLUsePart.UpdateData();
				UpdateGridUsePart();
			}
            DispUseskipValue();
        }

        private void dgridUseskip2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!chkUseskipMod.Checked) return;
            if (e.RowIndex == -1) return;
            if (e.ColumnIndex != 1) return;

            string strNewValue = "";
            if (this.dgridUseskip2[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridUseskip2[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            XMLUseskip.WriteDataTable(e.RowIndex + (Define.MAX_USESKIP_NUM / 2), strUseskipCol0, strNewValue);
        }

        
        private void dgridUseskip1_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!chkUseskipMod.Checked) return;

            string strMsg = "";
            if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                strMsg = "Do you want to update the data name?\r\n(Initialize the existing name)";
            else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
                strMsg = "데이터이름을 업데이트하시겠습니까?\r\n(기존 이름은 초기화됩니다.)";

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            for (int i = 0; i < (Define.MAX_USESKIP_NUM / 2); i++)
            {
                if (EnumHelper.GetDescription((USESKIP)i) == i.ToString())
                    XMLUseskip.WriteDataTable(i, strUseskipCol0, " ");
                else
                    XMLUseskip.WriteDataTable(i, strUseskipCol0, EnumHelper.GetDescription((USESKIP)i));

                this.dgridUseskip1.Rows[i].Cells[1].Value = XMLUseskip.ReadDataTable(i, strUseskipCol0);
            }
            XMLUseskip.UpdateData();
            XMLUseskip.RefillData();
        }

        private void dgridUseskip2_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!chkUseskipMod.Checked) return;

            string strMsg = "";
            if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                strMsg = "Do you want to update the data name?\r\n(Initialize the existing name)";
            else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
                strMsg = "데이터이름을 업데이트하시겠습니까?\r\n(기존 이름은 초기화됩니다.)";

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;


            for (int i = 0; i < (Define.MAX_USESKIP_NUM / 2); i++)
            {
                int calidx = i + (Define.MAX_USESKIP_NUM / 2);

                if (EnumHelper.GetDescription((USESKIP)calidx) == calidx.ToString())
                    XMLUseskip.WriteDataTable(calidx, strUseskipCol0, " ");
                else
                    XMLUseskip.WriteDataTable(calidx, strUseskipCol0, EnumHelper.GetDescription((USESKIP)calidx));

                dgridUseskip2.Rows[i].Cells[1].Value = XMLUseskip.ReadDataTable(calidx, strUseskipCol0);
            }
            XMLUseskip.UpdateData();
            XMLUseskip.RefillData();
        }

        #endregion

        #endregion

        #region [ Function ]

        #region - Private Function

        private void InitialXML()
        {
            XMLUseskip.Initializ("UseSkip");
        }

        private void InitialData()
        {
            ushUseSkip[0] = new ushort();
            ushUseSkip[1] = new ushort();
        }

        private Func<ushort, int, bool> IsUseSkip = delegate (ushort ush, int n) { 
            if (((ush >> n) & 0x0001) == 0x0001)
                return true; 
            return false; 
            };

        private void Off(int nUshFlag, int n) {
            ushUseSkip[nUshFlag] &= Convert.ToUInt16((~(0x0001 << n)) & 0xFFFF);          
        }

        private void On(int nUshFlag, int n) {    
            ushUseSkip[nUshFlag] |= Convert.ToUInt16(0x0001 << n);
        }

        // Use/Skip 변수에 내용을 데이터베이스에 써주고 업데이트함.
        private void UpdateUseskipValue()
        {            
            int nHalf = Define.MAX_USESKIP_NUM / 2;
            for (int i = 0; i < Define.MAX_USESKIP_NUM; i++)
            {
                int n = i < nHalf ? 0:1;
                int t = i < nHalf ? i:i - nHalf;
                if (IsUseSkip(ushUseSkip[n], t))
                    XMLUseskip.WriteDataTable(i, strUseskipCol1, "true");
                else
                    XMLUseskip.WriteDataTable(i, strUseskipCol1, "false");
            }
            
            XMLUseskip.UpdateData();
            
            // History Log
            Dictionary<string, XML.CompareData> compData = XMLUseskip.GetCompareData();
            foreach (var data in compData)
            {
                if (data.Value.colname == strUseskipCol1)
                {
                    string strGridName = "";
                    if (data.Value.index <16)   strGridName = dgridUseskip1.Rows[data.Value.index].Cells[1].Value.ToString();
                    else                        strGridName = dgridUseskip2.Rows[data.Value.index-16].Cells[1].Value.ToString();
                    string msg = string.Format("USESKIP,{0}[{1:D2}]:[{2}] ▶ [{3}]",
                        strGridName,data.Value.index, data.Value.oldvalue, data.Value.nowvalue);

                    UI.Logs.Write(LOGPART.HIS, msg);
                }
            }
            XMLUseskip.CompareDataClear();
        }

        // XML 데이타에서 내용을 읽어 변수에 옮겨준다.
        private void ReadUseskipValue()
        {
            int nHalf = Define.MAX_USESKIP_NUM / 2;

            for (int i = 0; i < Define.MAX_USESKIP_NUM; i++)
            {
                if (XMLUseskip.ReadDataTable(i, strUseskipCol1) == "true")               
                {
                    On(i < nHalf ? 0:1, i < nHalf ? i:i - nHalf);
                }
                else
                { 
                    Off(i < nHalf ? 0:1, i < nHalf ? i:i -nHalf);
                }
            }
        }
        #endregion

        #region - Public Function
		
        public void UpdateCheckBox(PW_MODE Password_Now)
        {            
            if (!chkUseskipEnable.Checked)
                chkUseskipMod.Enabled = true;
            else
                chkUseskipMod.Enabled = false;

            if (!chkUseskipMod.Checked)
                chkUseskipEnable.Enabled = true;
            else
                chkUseskipEnable.Enabled = false;
        }

		public void UpdateDataGrid(PW_MODE Password_Now)
        {
            switch (Password_Now)
            {
                case PW_MODE.OPER:
                case PW_MODE.ENG:
                    foreach (DataGridViewRow row in dgridUseskip2.Rows)
                            row.DefaultCellStyle.BackColor = Color.LightGray;
                    dgridUseskip2.ForeColor = SystemColors.Window;
                    dgridUseskip2.Enabled = false;
                    break;
                case PW_MODE.PRO:
                    foreach (DataGridViewRow row in dgridUseskip2.Rows)
                            row.DefaultCellStyle.BackColor = Color.White;
                    dgridUseskip2.ForeColor = SystemColors.ControlText;
                    dgridUseskip2.Enabled = true;
                    DispUseskipValue();
                    break;
            }
        }

        public bool IsWorking()
        {
            if (chkUseskipEnable.Checked || chkUseskipMod.Checked)
                return true;
            return false;
        }

        public void WorkEnd()
        {
            if (chkUseskipEnable.Checked)
                chkUseskipEnable.Checked = false;

            if (chkUseskipMod.Checked)
                chkUseskipMod.Checked = false;
        }

        public void DisableWork()
        {
            if (chkUseskipEnable.Enabled)
                chkUseskipEnable.Enabled = false;
            if (chkUseskipMod.Enabled)
                chkUseskipMod.Enabled = false;
        }

        public void EnableWork()
        {
            if (!chkUseskipEnable.Enabled)
                chkUseskipEnable.Enabled = true;
            if (!chkUseskipMod.Enabled)
                chkUseskipMod.Enabled = true;
        }

        // 현재 Useskip 데이터를 읽음.
        public bool GetUseskip(USESKIP eUSnum)
        {
            int num = (int)eUSnum;

            if (num >= Define.MAX_USESKIP_NUM)
                num = Define.MAX_USESKIP_NUM - 1;

            if (num < (Define.MAX_USESKIP_NUM / 2))
            {
                if (IsUseSkip(ushUseSkip[0], num))
                    return true;
                else
                    return false;
            }
            else
            {
                if (IsUseSkip(ushUseSkip[1], (num - (Define.MAX_USESKIP_NUM / 2))))
                    return true;
                else
                    return false;
            }
        }
        public bool GetUseskip(int nIdx)
        {
            return GetUseskip((USESKIP)nIdx);
        }

        // Useskip 데이터 하나를 변경하여 MMI 창에도 변경하고, DB에 저장도 하고, SEQ.에 보내기도 함.
        public bool WriteUseskip(USESKIP eUSnum, bool bFlag)
        {
            int num = (int)eUSnum;

            if (num >= Define.MAX_USESKIP_NUM)
                return false;
            if (num < 0)
                return false;

            if (num < (Define.MAX_USESKIP_NUM / 2))
            {
                if (bFlag)      On(0, num);
                else            Off(0, num);
            }
            else
            {
                if (bFlag)      On(1, num - (Define.MAX_USESKIP_NUM / 2));                    
                else            Off(1, num - (Define.MAX_USESKIP_NUM / 2));                    
            }
            
            SaveUseskip();

            return true;
        }

        // Useskip 데이터 하나만 변경함. (SaveUseskip과 한쌍)
        public bool SetUseskip(USESKIP eUSnum, bool bFlag)
        {
            int num = (int)eUSnum;

            if (num >= Define.MAX_USESKIP_NUM)
                return false;
            if (num < 0)
                return false;

            if (num < (Define.MAX_USESKIP_NUM / 2))
            {
                if (bFlag)      On(0, num);
                else            Off(0, num);
            }
            else
            {
                if (bFlag)      On(1, num - (Define.MAX_USESKIP_NUM / 2));                    
                else            Off(1, num - (Define.MAX_USESKIP_NUM / 2));
            }

            return true;
        }
        // SetUseskip로 여러 데이터 변경 한 후에 변경 작업 끝나면 한번에 DB 및 Recipe 화면 및 Seq. 에 업데이트함.
        public bool SaveUseskip()
        {
            DispUseskipValue();
            UpdateUseskipValue();

            SM.SHM.SetUseSkipData(ushUseSkip);

            return true;
        }
        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            chkUseskipMod.Text		= Language.languageDefault.NameModify;
            chkUseskipEnable.Text	= Language.languageDefault.Enable;

			dgridUseskip1.Columns[0].HeaderText = Language.languageDefault.DGRID_HD_No;
			dgridUseskip1.Columns[1].HeaderText = Language.languageDefault.DGRID_HD_UseSkip;
			dgridUseskip1.Columns[2].HeaderText = Language.languageDefault.DGRID_HD_Use;

			dgridUseskip2.Columns[0].HeaderText = Language.languageDefault.DGRID_HD_No;
			dgridUseskip2.Columns[1].HeaderText = Language.languageDefault.DGRID_HD_UseSkip;
			dgridUseskip2.Columns[2].HeaderText = Language.languageDefault.DGRID_HD_Use;
        }
		#endregion

		#endregion

		#region		[ Part Use ]
		private	void	InitUsePart()
		{
			int	nval;

			XMLUsePart.Initializ("UsePart");
			SM.SHM.InitUsePart();

			dgridUsePart.Rows.Clear();
			
			int nVisibleRowCount = 16;
            dgridUsePart.RowTemplate.Height = dgridUsePart.Height / (nVisibleRowCount + 1);
            dgridUsePart.ColumnHeadersHeight = dgridUsePart.Height - dgridUsePart.RowTemplate.Height * nVisibleRowCount;

            for (UsePartName part_name = 0; part_name < UsePartName.Max; part_name++)
            {
                dgridUsePart.Rows.Add((int)part_name, EnumHelper.GetDescription(part_name), " ");

                if (int.TryParse(XMLUsePart.ReadDataTable((int)part_name, strUsePartCol0), out nval) == true)
                {
                    SM.SHM.SetUsePart(part_name, (UsePartMode)nval);
                }
            }

            if (dgridUsePart.RowCount > 0) this.dgridUsePart.Rows[0].Selected = false;
            UpdateGridUsePart();
		}

		private	void	UpdateGridUsePart()
		{
			for (UsePartName part_name = 0; part_name < UsePartName.Max; part_name++)
			{
				switch (SM.SHM.GetUsePart(part_name))
				{
					case UsePartMode.Enable:
						dgridUsePart[2, (int)part_name].Value	= "Enable";
						dgridUsePart.Rows[(int)part_name].DefaultCellStyle.BackColor	= Color.White;
						break;
					case UsePartMode.Load:
						dgridUsePart[2, (int)part_name].Value	= "Only Load";
						dgridUsePart.Rows[(int)part_name].DefaultCellStyle.BackColor	= Color.Orange;
						break;
					case UsePartMode.Unload:
						dgridUsePart[2, (int)part_name].Value	= "Only Unload";
						dgridUsePart.Rows[(int)part_name].DefaultCellStyle.BackColor	= Color.Orange;
						break;
					case UsePartMode.Disable:
						dgridUsePart[2, (int)part_name].Value	= "Disable";
						dgridUsePart.Rows[(int)part_name].DefaultCellStyle.BackColor	= Color.Red;
						break;
				}
			}
		}

		private	void	dgridUsePart_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (!chkUseskipEnable.Checked)			return;
            if (e.RowIndex == -1)					return;
            if (e.ColumnIndex != 2)					return;

			frmListBoxSelect	pup		= new frmListBoxSelect();
			UsePartMode		mode	= UsePartMode.Disable;

            if (e.RowIndex >= (int)UsePartName.Picker1 && e.RowIndex <= (int)UsePartName.Picker4)
            {
                if (SM.SHM.IsBitData(BIT.Pause) == true || SM.SHM.IsBitData(BIT.AutoRun) == true)
                {
                    frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Confirmation, "These Option can be changed after stopp the machine.");
                    if (pMsg.ShowDialog() == DialogResult.OK) return;
                }
            }
            if (e.RowIndex < (int)UsePartName.LDTable1)
			{
				if (pup.ShowDialog(new List<string> { "Disable", "Enable" }, dgridUsePart[e.ColumnIndex, e.RowIndex].Value.ToString()) == DialogResult.Cancel)			return;
			}
			else
			{
				if (pup.ShowDialog(new List<string> { "Disable", "Enable", "Only Load", "Only Unload" }, dgridUsePart[e.ColumnIndex, e.RowIndex].Value.ToString()) == DialogResult.Cancel)			return;
			}

			switch (pup.SelItem)
			{
				case "Disable":			mode	= UsePartMode.Disable;		break;
				case "Enable":			mode	= UsePartMode.Enable;			break;
				case "Only Load":		mode	= UsePartMode.Load;			break;
				case "Only Unload":		mode	= UsePartMode.Unload;			break;
			}

			SM.SHM.SetUsePart((UsePartName)e.RowIndex, mode);

			if (XMLUsePart.WriteDataTable(e.RowIndex, strUsePartCol0, (int)mode) == false)
			{
				XMLUsePart.AddDataTable(e.RowIndex, strUsePartCol0, (int)mode);
			}
			XMLUsePart.UpdateData();

			UpdateGridUsePart();


			this.dgridUsePart.Rows[e.RowIndex].Selected = false;
		}
		
		private	void	tmrUpdateUsePart_Tick(object sender, EventArgs e)
		{
			
		}
		#endregion

		}
}
