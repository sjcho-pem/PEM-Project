using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmMotor : Form
    {
        public delegate void btnCylClickEventHandler();
        public event btnCylClickEventHandler btnCylClick;
        protected virtual void btnCylClickEvent()
        {
            if (btnCylClick != null)
                btnCylClick();
        }

        public frmMotor()
        {
            InitializeComponent();
            InitialXML();
            InitialMotorSelectCombobox();
            InitialMotorGrid();
            InitialControls();

            ReadAllVelocityRate();
        }

        #region [ Variables ]
        private XML XMLMotorConfig = new XML();
        private string strMotorConfigColName0 = "MotorNo";
        private string strMotorConfigColName1 = "MotorName";
        private string strMotorConfigColName2 = "MotorUse";

        private XML[,] XMLMotorData = new XML[Define.MAX_RECIPE_NUM, Define.MAX_MOTORDATA_FILE_NUM];        
        private string strMotorDataColName2 = "MotorIndexName";
        private string strMotorDataColName3 = "MotorPosition";
        private string strMotorDataColName4 = "MotorVelocity";

        private XML XMLMotorJogData = new XML();
        private string strMotorJogDataColName0 = "JogDistance";
        private string strMotorJogDataColName1 = "JogVelocity";

        private TABLE_MOTOR[] arrMtTable = new TABLE_MOTOR[Define.MAX_MOTOR_NUM];
        public TABLE_MOTOR[] arrMotorTable
        {
            get { return arrMtTable; }
        }

        //public STATUS_MOTOR[] stMtStatus = new STATUS_MOTOR[Define.MAX_MOTOR_NUM];  // not use ??

        private STATUS_MOTOR stMtStatus = new STATUS_MOTOR();

        private bool bToggle = false;

        private int iNowMotorNum = 0;
        public int iMotorNumNow
        {
            get { return iNowMotorNum; }
        }
        #endregion

        #region [ Event Function ]

        private void frmMotor_Load(object sender, EventArgs e)
        {
			// 최초 첫 모터 표시
			if (cbSelMotor.SelectedIndex < 0)
			{
				cbSelMotor.SelectedIndex = 0;
				lbIndexNo.Text = "0";  // clear
			}
            RedrawMotorGrid(cbSelMotor.SelectedIndex);
        }

        private void frmMotor_VisibleChanged(object sender, EventArgs e)
        {
            if (UI.PageMotor.Visible)
            {
                tmrMotor.Enabled = true;
            }
            else
            {
                btnManualCyl.Visible = false;
                tmrMotor.Enabled = false;
            }
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            Keys key = keyData & ~(Keys.Shift | Keys.Control);
            switch (key)
            {
                case Keys.F:
                    if ((keyData & Keys.Control) != 0)
                    {
                        if (chkMotorJog.Checked || chkMotorModify.Checked) break;
                        frmSearch pCombo = new frmSearch(cbSelMotor, "MOTOR");

                        if (pCombo.ShowDialog() == DialogResult.OK)
                        {
                            cbSelMotor.SelectedIndex = pCombo.GetVal();
                            UI.Logs.Write(LOGPART.MT, string.Format("[{0}]Motor, Motor Selected !!", pCombo.GetVal()));
                        }
                        pCombo.Close();
                    }
                    break;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region [ Timmer Function ]

        private void tmrMotor_Tick(object sender, EventArgs e)
        {
            if (cbSelMotor.SelectedIndex < 0) return;

            // Motor 상태 정보 전달 -LWJ
            //Global.COM.SetCommand(CMD.MS_WRITE_MOTOR_STATUS, (uint)cbSelMotor.SelectedIndex);

            SM.SHM.GetMotorSatusData((uint)cbSelMotor.SelectedIndex, ref stMtStatus);

            this.lbActPos.Text = (Convert.ToDouble(stMtStatus.nActPos) / 1000).ToString("0.000");

            //단위변환 소숫점 3자리 0.000
            this.lbCurPos.Text = (Convert.ToDouble(stMtStatus.nCurPos) / 1000).ToString("0.000");


            bool bAlarm = (stMtStatus.svStatus.ALM == true) || (stMtStatus.svStatus.WARNG == true);
            if (bAlarm)
                tmrMotor_Flash.Enabled = true;
            else
            {
                tmrMotor_Flash.Enabled = false;
                SetAlarmCode(false, 0);
            }

            SetHome((stMtStatus.svStatus.HOME == true)); // NA15를 HOME 비트로 사용
            SetAlarm((stMtStatus.svStatus.ALM == true));
            SetRdy((stMtStatus.svStatus.Rdy == true));
            SetServo((stMtStatus.svStatus.SVON == true));
            //SetInposi((stStatus.ushStatus.PSET == true) && (stStatus.ushStatus.DEN == true));
            SetInposi((stMtStatus.svStatus.INPOS == true));
            SetCw((stMtStatus.ushIO.P_OT == true));
            SetCcw((stMtStatus.ushIO.N_OT == true));
            SetOrg((stMtStatus.ushIO.Org == true));


			if (SM.SHM.DicInterlockMot.ContainsKey((MT)cbSelMotor.SelectedIndex) == true)
			{
				lbInterlockMsg.Text	= SM.SHM.DicInterlockMot[(MT)cbSelMotor.SelectedIndex];

				if (chkMotorJog.Checked == true)
				{
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false)
					{
						if (btnIndexMove.Enabled == true)			btnIndexMove.Enabled	= false;
						if (btnRepeatMove.Enabled == true)			btnRepeatMove.Enabled	= false;
						if (gbStepMove.Enabled == true)				gbStepMove.Enabled		= false;
						if (gbJogMove.Enabled == true)				gbJogMove.Enabled		= false;						
					}
					else
					{
						if (btnIndexMove.Enabled == false)			btnIndexMove.Enabled	= true;
						if (btnRepeatMove.Enabled == false)			btnRepeatMove.Enabled	= true;
						if (gbStepMove.Enabled == false)			gbStepMove.Enabled		= true;
						if (gbJogMove.Enabled == false)				gbJogMove.Enabled		= true;
					}
				}
			}
			else
			{
				lbInterlockMsg.Text	= "";

				if (chkMotorJog.Checked == true)
				{
					if (btnRepeatMove.Enabled == false)			btnRepeatMove.Enabled	= true;
					if (gbStepMove.Enabled == false)			gbStepMove.Enabled		= true;
					if (gbJogMove.Enabled == false)				gbJogMove.Enabled		= true;

					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == true)
					{
						if (btnIndexMove.Enabled == true) btnIndexMove.Enabled = false;
					}
					{
						if (btnIndexMove.Enabled == false) btnIndexMove.Enabled = true;
					}
				}

			
			}
        }

        private void tmrMotor_Flash_Tick(object sender, EventArgs e)
        {
            if (bToggle)
            {
                SetAlarmCode(bToggle, stMtStatus.ushAlarm);
                bToggle = !bToggle;
            }
            else
            {
                SetAlarmCode(bToggle, stMtStatus.ushAlarm);
                bToggle = !bToggle;
            }
        }
        #endregion

        #region [ Grid Setting ]

        #region - Function 
        private void InitialMotorGrid()
        {
            this.dgridMotor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.tbColumnMotorNum,
            this.tbColumnMotorName,
            this.tbColumnMotorPosition,
            this.tbColumnMotorVelocity});

            for (int i = 0; i < Define.MAX_MOTORDATA_NUM; i++)
                this.dgridMotor.Rows.Add(i + 1, "", "0", "0");

            this.dgridMotor.RowTemplate.Height = 30;

            for (int j = 0; j < Define.MAX_MOTOR_NUM; j++)
            {
                arrMtTable[j].nPos = new int[SHMDEF.MAX_MOTOR_TABLE_NUM];
                arrMtTable[j].nSpeed = new uint[SHMDEF.MAX_MOTOR_TABLE_NUM];
            }

            dgridMotor.Columns[0].DefaultCellStyle.SelectionBackColor = Color.Transparent;
            dgridMotor.Columns[0].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
            dgridMotor.ClearSelection();
        }

        // Recipe 교체시 SubpageRecipe에서 한번 호출하여 준다.
        public void RecipeChange()
        {
            //GridPanel panel = sgridMotor.PrimaryGrid;
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            GetMotorTableFromDB(iRecipe);

            cbSelMotor.SelectedIndex = 0;

            this.dgridMotor.Rows.Clear();

            for (int i = 0; i < Define.MAX_MOTORDATA_NUM; i++)
            {
                int temp = cbSelMotor.SelectedIndex / 10;
                int rem = cbSelMotor.SelectedIndex % 10;

                int dbidx = i + (rem * 100);

                string strTemp1 = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName2);
                string strTemp2 = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName3);
                string strTemp3 = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName4);

                // 단위 변환 um -> mm, degree
                strTemp2 = (Convert.ToDouble(strTemp2) / 1000).ToString("0.000");
                strTemp3 = (Convert.ToDouble(strTemp3) / 1000).ToString("0.000");

                this.dgridMotor.Rows.Add(i, strTemp1, strTemp2, strTemp3);
            }

            for (int j = 0; j < Define.MAX_MOTOR_NUM; j++)
            {
                SM.SHM.SetMotorTableData((uint)j, this.arrMtTable[j]);
            }
        }

        private void RedrawMotorGrid(int motor)
        {
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            this.dgridMotor.Rows.Clear();
            for (int i = 0; i < Define.MAX_MOTORDATA_NUM; i++)
            {
                int temp = motor / 10;
                int rem = motor % 10;

                int dbidx = i + (rem * 100);

                string strTemp1 = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName2);
                string strTemp2 = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName3);
                string strTemp3 = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName4);

                // 단위 변환 um -> mm, degree
                strTemp2 = (Convert.ToDouble(strTemp2) / 1000).ToString("0.000");
                strTemp3 = (Convert.ToDouble(strTemp3) / 1000).ToString("0.000");

                this.dgridMotor.Rows.Add(i, strTemp1, strTemp2, strTemp3);
            }
			dgridMotor.Columns[1].AutoSizeMode	= DataGridViewAutoSizeColumnMode.Fill;

            string strJogDist = XMLMotorJogData.ReadDataTable(motor, strMotorJogDataColName0);
            string strJogVel = XMLMotorJogData.ReadDataTable(motor, strMotorJogDataColName1);

            lbJogDist.Text = (Convert.ToDouble(strJogDist) / 1000).ToString("0.000");
            lbJogVel.Text = (Convert.ToDouble(strJogVel) / 1000).ToString("0.000"); 
            
            lbIndexNo.Text = "0";
        }
        #endregion

        #region - Event

        private void dgridMotor_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            if (this.tbColumnMotorName.ReadOnly) return;
            double doValue = 0;

            bool isNum = false;
            int temp = cbSelMotor.SelectedIndex / 10;
            int rem = cbSelMotor.SelectedIndex % 10;
            int dbidx = e.RowIndex + (rem * 100);
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            string strNewValue = "";
            if (this.dgridMotor[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridMotor[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            if (e.ColumnIndex == 1)
            {
                XMLMotorData[iRecipe, temp].WriteDataTable(dbidx, strMotorDataColName2, strNewValue);
            }
            else if (e.ColumnIndex == 2)
            {
                isNum = double.TryParse(strNewValue, out doValue);

                if (!isNum)
                {
                    MessageBox.Show("Invalid number! Try again!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RedrawMotorGrid(cbSelMotor.SelectedIndex);
                }
                else
                {
                    String sVal = strNewValue;
                    Int32 num = Convert.ToInt32((Convert.ToDouble(sVal) * 1000));
                    XMLMotorData[iRecipe, temp].WriteDataTable(dbidx, strMotorDataColName3, num.ToString());
                }
            }
            else if (e.ColumnIndex == 3)
            {
                isNum = double.TryParse(strNewValue, out doValue);

                if (!isNum)
                {
                    MessageBox.Show("Invalid number! Try again!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RedrawMotorGrid(cbSelMotor.SelectedIndex);
                }
                else
                {
                    String sVal = strNewValue;
                    Int32 num = Convert.ToInt32((Convert.ToDouble(sVal) * 1000));
                    XMLMotorData[iRecipe, temp].WriteDataTable(dbidx, strMotorDataColName4, num.ToString());
                }
            }
        }

		private void dgridMotor_SelectionChanged(object sender, EventArgs e)
		{
			DataGridView a = sender as DataGridView;
            if (chkMotorJog.Checked == false)			return;
            if (a.SelectedCells.Count != 1)				return;

            int nRowIdx = a.SelectedCells[0].RowIndex;
            int nColIdx = a.SelectedCells[0].ColumnIndex;

            if (nColIdx == 1 || nColIdx == 2 || nColIdx == 3) 
                 lbIndexNo.Text =  nRowIdx.ToString();
		}
        #endregion

        #endregion

        #region [ ComboBox Event ]

        private void cbSelMotor_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbIndexNo.Text = "0";  // clear
            lbMtNo.Text = (cbSelMotor.SelectedIndex).ToString("00");
            this.RedrawMotorGrid(cbSelMotor.SelectedIndex);
        }

        private void cbAllVelocity_SelectedIndexChanged(object sender, EventArgs e)
        {
            XMLMotorJogData.WriteDataTable((int)VelocityRate.XML_INDEX, strMotorJogDataColName1, cbAllVelocity.SelectedIndex.ToString());
        }
        #endregion

        #region [ CheckBox Event ]
        private void chkMotorModify_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMotorModify.Checked)
            {
                this.tbColumnMotorName.ReadOnly = false;
                this.tbColumnMotorPosition.ReadOnly = false;
                this.tbColumnMotorVelocity.ReadOnly = false;
                chkMotorModify.ForeColor = Color.Red;
                chkMotorModify.Enabled = false;
                chkMotorJog.Enabled = false;
                //cbSelMotor.Enabled = false;
                gpMotorSelect.Enabled = false;
                btnRefresh.Enabled = true;
                btnStop.Enabled = false;

				btnTeachingMode.Enabled		= false;
				btnTeachingMode.BackColor	= Color.White;

				dgridMotor.Columns[1].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dgridMotor.Columns[2].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dgridMotor.Columns[3].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dgridMotor.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                dgridMotor.Columns[2].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                dgridMotor.Columns[3].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                dgridMotor.ClearSelection();
            }
            else
            {
                dgridMotor.Columns[1].DefaultCellStyle.SelectionBackColor = Color.Transparent;
                dgridMotor.Columns[2].DefaultCellStyle.SelectionBackColor = Color.Transparent;
                dgridMotor.Columns[3].DefaultCellStyle.SelectionBackColor = Color.Transparent;
                dgridMotor.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
                dgridMotor.Columns[2].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
                dgridMotor.Columns[3].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
                dgridMotor.ClearSelection();
            }
        }

        private void chkMotorJog_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMotorJog.Checked)
            {
                gbMotorMove.Enabled = true;
                btnIndexMove.Enabled = true;
                btnRepeatMove.Enabled = true;
                btnPosIndexWrite.Enabled = true;
                chkMotorJog.Enabled = false;
                chkMotorJog.ForeColor = Color.Red;
                chkMotorModify.Enabled = false;
                //cbSelMotor.Enabled = false;
                gpMotorSelect.Enabled = false;
                gbAllVelocity.Enabled = true;
                btnRefresh.Enabled = true;
                btnStop.Enabled = true;

				btnTeachingMode.Enabled		= true;

				dgridMotor.Columns[1].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dgridMotor.Columns[2].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dgridMotor.Columns[3].DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                dgridMotor.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                dgridMotor.Columns[2].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                dgridMotor.Columns[3].DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                dgridMotor.ClearSelection();
            }
            else
            {
                dgridMotor.Columns[1].DefaultCellStyle.SelectionBackColor = Color.Transparent;
                dgridMotor.Columns[2].DefaultCellStyle.SelectionBackColor = Color.Transparent;
                dgridMotor.Columns[3].DefaultCellStyle.SelectionBackColor = Color.Transparent;
                dgridMotor.Columns[1].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
                dgridMotor.Columns[2].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
                dgridMotor.Columns[3].DefaultCellStyle.SelectionForeColor = SystemColors.ControlText;
                dgridMotor.ClearSelection();
            }
        }

        #endregion

        #region [ Button Event ]
        private void btnMotorSave_Click(object sender, EventArgs e)
        {
            if (!chkMotorModify.Checked && !chkMotorJog.Checked) return;

            int temp = cbSelMotor.SelectedIndex / 10;
            int rem = cbSelMotor.SelectedIndex % 10;
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            
			SM.SHM.SetBitData(BIT.MotorTeachingMode, false);

            XMLMotorJogData.UpdateData();
            XMLMotorData[iRecipe, temp].UpdateData();

            XMLMotorJogData.RefillData();
            XMLMotorData[iRecipe, temp].RefillData();  // 다시 데이터 베이스 내용을 읽어온다. (즉, Confirm의 효과 또는 데이터 베이스와 완전 일치. 처리시간이 많이 걸림.

            RedrawMotorGrid(cbSelMotor.SelectedIndex);

            GetMotorTableFromDB(UI.SubpageRecipe.iNowRecipeNum, cbSelMotor.SelectedIndex);
            SM.SHM.SetMotorTableData((uint)cbSelMotor.SelectedIndex, this.arrMtTable[cbSelMotor.SelectedIndex]);

            // All Velocity Rate Read To Send
            ReadAllVelocityRate();

            // LOG
            string msg = string.Format("[{0:D2}]Motor, Save Motor Data !!", cbSelMotor.SelectedIndex);
            UI.Logs.Write(LOGPART.DATA, msg);
                        
            // History Log
            Dictionary<string, XML.CompareData> compData = XMLMotorData[iRecipe, temp].GetCompareData();
            foreach (var data in compData)
            {
                if (data.Value.colname == strMotorDataColName3)
                {
					msg = $" [{cbSelMotor.SelectedIndex:D2}] Motor({(MT)cbSelMotor.SelectedIndex}) - [{data.Value.index% 100:D2}] Index({dgridMotor.Rows[data.Value.index% 100].Cells[1].Value}), Position : [{data.Value.oldvalue}] ▶ [{data.Value.nowvalue}]";
					UI.Logs.Write(LOGPART.HIS, msg);
                }
                if (data.Value.colname == strMotorDataColName4)
                {
					msg = $" [{cbSelMotor.SelectedIndex:D2}] Motor({(MT)cbSelMotor.SelectedIndex}) - [{data.Value.index% 100:D2}] Index({dgridMotor.Rows[data.Value.index% 100].Cells[1].Value}), Velocity : [{data.Value.oldvalue}] ▶ [{data.Value.nowvalue}]";
					UI.Logs.Write(LOGPART.HIS, msg);
                }
            }
            XMLMotorData[iRecipe, temp].CompareDataClear();
            

            // 저장해야만 수정 모드를 마칠 수 있다.
            if (chkMotorModify.Checked || chkMotorJog.Checked)
            {
                this.tbColumnMotorName.ReadOnly = true;
                this.tbColumnMotorPosition.ReadOnly = true;
                this.tbColumnMotorVelocity.ReadOnly = true;

                chkMotorModify.Checked = false;
                chkMotorModify.ForeColor = Color.Black;
                chkMotorModify.Enabled = true;

                //cbSelMotor.Enabled = true;
                gpMotorSelect.Enabled = true;

                chkMotorJog.Checked = false;
                chkMotorJog.ForeColor = Color.Black;
                chkMotorJog.Enabled = true;
                gbMotorMove.Enabled = false;
                btnIndexMove.Enabled = false;
                btnRepeatMove.Enabled = false;
                btnPosIndexWrite.Enabled = false;
                btnRefresh.Enabled = false;
                gbAllVelocity.Enabled = false;
                btnStop.Enabled = false;

				btnTeachingMode.Enabled		= false;
				btnTeachingMode.BackColor	= Color.White;
            }
        }

        private void btnDispUp_Click(object sender, EventArgs e)
        {
            double iNow = Convert.ToDouble(lbJogDist.Text.ToString());
            double iLevel = 0.001;

            for (int i = 0; i < cbDistLevel.SelectedIndex; i++)
                iLevel *= 10;

            double iResult = iNow + iLevel;
            if (iResult > 10)
                iResult = 10;  // 10mm 이상은 막음.

            lbJogDist.Text = Convert.ToString(iResult);
            UInt32 data = Convert.ToUInt32(Convert.ToDouble(lbJogDist.Text) * 1000);
            XMLMotorJogData.WriteDataTable(cbSelMotor.SelectedIndex, strMotorJogDataColName0, data.ToString());
        }

        private void btnDispDn_Click(object sender, EventArgs e)
        {
            double iNow = Convert.ToDouble(lbJogDist.Text.ToString());
            double iLevel = 0.001;

            for (int i = 0; i < cbDistLevel.SelectedIndex; i++)
                iLevel *= 10;

            double iResult = iNow - iLevel;
            if (iResult <= 0.001)
                iResult = 0.001;  // 10mm 이상은 막음.

            lbJogDist.Text = Convert.ToString(iResult);
            UInt32 data = Convert.ToUInt32(Convert.ToDouble(lbJogDist.Text) * 1000);
            XMLMotorJogData.WriteDataTable(cbSelMotor.SelectedIndex, strMotorJogDataColName0, data.ToString());
        }

        private void lbJogDist_Click(object sender, EventArgs e)
        {
            frmKeyPad fPad = new frmKeyPad(lbJogDist, lbJogDist.Text, KeyPadType.unsigned);

            if (fPad.ShowDialog() == DialogResult.Cancel)
                return;

            double iResult = Convert.ToDouble(lbJogDist.Text);
            if (iResult > 10)
                iResult = 10;  // 10mm 이상은 막음.
            lbJogDist.Text = Convert.ToString(iResult);
            UInt32 data = Convert.ToUInt32(Convert.ToDouble(lbJogDist.Text) * 1000);
            XMLMotorJogData.WriteDataTable(cbSelMotor.SelectedIndex, strMotorJogDataColName0, data.ToString());
        }

        private void lbJogVel_Click(object sender, EventArgs e)
        {
            frmKeyPad fPad = new frmKeyPad(lbJogVel, lbJogVel.Text, KeyPadType.unsigned);

            if (fPad.ShowDialog() == DialogResult.Cancel)
                return;

            double iResult = Convert.ToDouble(lbJogVel.Text);
            if (iResult > 100)
                iResult = 100;  // 100 mm/sec 이상은 막음.
            lbJogVel.Text = Convert.ToString(iResult);
            UInt32 data = Convert.ToUInt32(Convert.ToDouble(lbJogVel.Text) * 1000);
            XMLMotorJogData.WriteDataTable(cbSelMotor.SelectedIndex, strMotorJogDataColName1, data.ToString());
        }

        private void btnVelUp_Click(object sender, EventArgs e)
        {
            double iNow = Convert.ToDouble(lbJogVel.Text.ToString());
            double iLevel = 0.001;

            for (int i = 0; i < cbVelLevel.SelectedIndex; i++)
                iLevel *= 10;

            double iResult = iNow + iLevel;
            if (iResult > 100)
                iResult = 100;  // 100 mm/sec 이상은 막음.

            lbJogVel.Text = Convert.ToString(iResult);
            UInt32 data = Convert.ToUInt32(Convert.ToDouble(lbJogVel.Text) * 1000);
            XMLMotorJogData.WriteDataTable(cbSelMotor.SelectedIndex, strMotorJogDataColName1, data.ToString());
        }

        private void btnVelDn_Click(object sender, EventArgs e)
        {
            double iNow = Convert.ToDouble(lbJogVel.Text.ToString());
            double iLevel = 0.001;

            for (int i = 0; i < cbVelLevel.SelectedIndex; i++)
                iLevel *= 10;

            double iResult = iNow - iLevel;
            if (iResult <= 0.001)
                iResult = 0.001;  // 100 mm/sec 이상은 막음.

            lbJogVel.Text = Convert.ToString(iResult);
            UInt32 data = Convert.ToUInt32(Convert.ToDouble(lbJogVel.Text) * 1000);
            XMLMotorJogData.WriteDataTable(cbSelMotor.SelectedIndex, strMotorJogDataColName1, data.ToString());
        }

        private void btnStepMoveUp_Click(object sender, EventArgs e)
        {
            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;
            mtInfo.nDist = Convert.ToInt32(Convert.ToDouble(lbJogDist.Text) * 1000);
            mtInfo.nVel = Convert.ToInt32(Convert.ToDouble(lbJogVel.Text) * 1000);

            SM.SHM.SetButtonNum(BUTTON.MT_STEP, mtInfo);
			NDm.Write(eNDM.ManualMotorMoveDir, 1);

            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, (+)STEP MOVE !!", (int)mtInfo.eMt));
        }

        private void btnStepMoveDn_Click(object sender, EventArgs e)
        {
            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;
            mtInfo.nDist = -1 * Convert.ToInt32(Convert.ToDouble(lbJogDist.Text) * 1000.0);
            mtInfo.nVel = Convert.ToInt32(Convert.ToDouble(lbJogVel.Text) * 1000);

            SM.SHM.SetButtonNum(BUTTON.MT_STEP, mtInfo);
			NDm.Write(eNDM.ManualMotorMoveDir, -1);

            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, (-)STEP MOVE !!", (int)mtInfo.eMt));
        }

        private void btnJogMoveUp_MouseDown(object sender, MouseEventArgs e)
        {
            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;
            mtInfo.bFlag = true;
            mtInfo.nVel = Convert.ToInt32(Convert.ToDouble(lbJogVel.Text) * 1000);

            SM.SHM.SetButtonNum(BUTTON.MT_JOG, mtInfo);
			NDm.Write(eNDM.ManualMotorMoveDir, 1);

            this.btnJogMoveUp.BackgroundImage = Properties.Resources.JogPlusOn_64p;
            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, (+)JOG MOVE !!", (int)mtInfo.eMt));
        }

        private void btnJogMoveUp_MouseUp(object sender, MouseEventArgs e)
        {
            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;

            SM.SHM.SetButtonNum(BUTTON.MT_JOGSTOP, mtInfo);

            this.btnJogMoveUp.BackgroundImage = Properties.Resources.JogPlusOff_64p;
            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, (+)JOG STOP !!", (int)mtInfo.eMt));
        }

        private void btnJogMoveDn_MouseDown(object sender, MouseEventArgs e)
        {
            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;
            mtInfo.bFlag = false;
            mtInfo.nVel = Convert.ToInt32(Convert.ToDouble(lbJogVel.Text) * 1000);

            SM.SHM.SetButtonNum(BUTTON.MT_JOG, mtInfo);
			NDm.Write(eNDM.ManualMotorMoveDir, -1);

            this.btnJogMoveDn.BackgroundImage = Properties.Resources.JogMinusOn_64;
            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, (-)JOG MOVE !!", (int)mtInfo.eMt));
        }

        private void btnJogMoveDn_MouseUp(object sender, MouseEventArgs e)
        {
            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;

            SM.SHM.SetButtonNum(BUTTON.MT_JOGSTOP, mtInfo);

            this.btnJogMoveDn.BackgroundImage = Properties.Resources.JogMinusOff_64;
            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, (-)JOG STOP !!", (int)mtInfo.eMt));
        }

        private void btnIndexMove_Click(object sender, EventArgs e)
        {
            int nIDX = Convert.ToInt32(lbIndexNo.Text.ToString());
            if (nIDX != dgridMotor.CurrentCell.RowIndex) return;
            if (dgridMotor.SelectedCells.Count < 1)      return;

            // Confirmation.
            string strMsg = string.Format("[{0:D3}] " + Language.languageDefault.MB_IndexMove, nIDX);

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, strMsg);

            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;
            mtInfo.nIdx = nIDX;

			STATUS_MOTOR	status	= new STATUS_MOTOR();
			double			dtg		= 0;
			SM.SHM.GetMotorSatusData((uint)cbSelMotor.SelectedIndex, ref status);
			double.TryParse(dgridMotor[2, nIDX].Value.ToString(), out dtg);
			if		(status.nActPos < dtg*1000)			NDm.Write(eNDM.ManualMotorMoveDir, 1);
			else if (status.nActPos > dtg*1000)			NDm.Write(eNDM.ManualMotorMoveDir, -1);
			else										NDm.Write(eNDM.ManualMotorMoveDir, 0);

            SM.SHM.SetButtonNum(BUTTON.MT_INDEX, mtInfo);

            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, [{1:D2}]Index, INDEX MOVE !!", (int)mtInfo.eMt, mtInfo.nIdx));
        }

        private void btnPosIndexWrite_Click(object sender, EventArgs e)
        {
            int nIDX = Convert.ToInt32(lbIndexNo.Text.ToString());
            if (nIDX != dgridMotor.CurrentCell.RowIndex) return;

            string sMsg = string.Format("[{0:D2}] {1}", (nIDX).ToString(), Language.languageDefault.MB_IndexWrite);
            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, sMsg);

            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            int temp = cbSelMotor.SelectedIndex / 10;
            int rem = cbSelMotor.SelectedIndex % 10;
            int dbidx = nIDX + (rem * 100);
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            float fnum = Convert.ToSingle(lbCurPos.Text) * 1000;

            XMLMotorData[iRecipe, temp].WriteDataTable(dbidx, strMotorDataColName3, (fnum).ToString());
            RedrawMotorGrid(cbSelMotor.SelectedIndex);

            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, [{1:D2}]Index, INDEX WRITE !!",
                cbSelMotor.SelectedIndex, nIDX));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            string strMsg = string.Format("Motor Data Refresh?");
            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel)
                return;

            int temp = cbSelMotor.SelectedIndex / 10;
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            XMLMotorData[iRecipe, temp].RefillData();
            this.RedrawMotorGrid(cbSelMotor.SelectedIndex);
        }

        private void ledServo_Click(object sender, EventArgs e)
        {
            if (!chkMotorJog.Checked) return;

            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;
            if (stMtStatus.svStatus.SVON == true)
                mtInfo.bFlag = false;
            else
                mtInfo.bFlag = true;
            
            string sMsg = string.Format("[{0}] Servo On/Off?", (mtInfo.eMt).ToString());
            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, sMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            SM.SHM.SetButtonNum(BUTTON.MT_SERVO, mtInfo);
            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, Servo On/Off !!", (int)mtInfo.eMt));
        }

        private void ledHome_Click(object sender, EventArgs e)
        {
            if (!chkMotorJog.Checked) return;

            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;

            string sMsg = string.Format("[{0}] Motor Home Start?", (mtInfo.eMt).ToString());
            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, sMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            SM.SHM.SetButtonNum(BUTTON.MT_HOME, mtInfo);
            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, Motor Home !!", (int)mtInfo.eMt));
        }
        
        private void btnVelIndexWrite_Click(object sender, EventArgs e)
        {
            int nIDX = Convert.ToInt32(lbIndexNo.Text.ToString());            
            if (nIDX != dgridMotor.CurrentCell.RowIndex) return;

            string sMsg = string.Format("[{0:D2}] {1}", (nIDX).ToString(), Language.languageDefault.MB_IndexWrite);
            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, sMsg);

            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            int temp = cbSelMotor.SelectedIndex / 10;
            int rem = cbSelMotor.SelectedIndex % 10;
            int dbidx = nIDX + (rem * 100);
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            float fnum = Convert.ToSingle(lbJogVel.Text) * 1000;

            XMLMotorData[iRecipe, temp].WriteDataTable(dbidx, strMotorDataColName4, (fnum).ToString());
            RedrawMotorGrid(cbSelMotor.SelectedIndex);

            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, [{1:D2}]Index, VELOCITY INDEX WRITE !!",
                cbSelMotor.SelectedIndex, nIDX));
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;
            
            SM.SHM.SetButtonNum(BUTTON.MT_STOP, mtInfo);

            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, STOP !!", (int)mtInfo.eMt));
        }
        private void btnRepeatMove_Click(object sender, EventArgs e)
        {
            int nIDX = Convert.ToInt32(lbIndexNo.Text.ToString());
            if (nIDX != dgridMotor.CurrentCell.RowIndex) return;

            string strMsg = string.Format("[{0:D3}] REPEAT MOVE", nIDX);

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, strMsg);

            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            MtInfo mtInfo = new MtInfo();
            mtInfo.eMt = (MT)cbSelMotor.SelectedIndex;
            mtInfo.nIdx = nIDX;
            SM.SHM.SetButtonNum(BUTTON.MT_REPEAT, mtInfo);

            UI.Logs.Write(LOGPART.MT, string.Format("[{0:D2}]Motor, [{1:D2}]Index, REPEAT MOVE !!", (int)mtInfo.eMt, mtInfo.nIdx));
        }
		
        private void btnManualCyl_Click(object sender, EventArgs e)
        {
            btnCylClickEvent();
        }
        #endregion

        #region [ Motor Status Funtion ]
        private void SetHome(bool bon)
        {
            if (bon)
                ledHome.BackColor = Color.Yellow;
            else
                ledHome.BackColor = Color.White;
        }

        private void SetAlarm(bool bon)
        {
            if (bon)
                ledAlarm.BackColor = Color.Red;
            else
                ledAlarm.BackColor = Color.White;
        }

        private void SetAlarmCode(bool toggle, ushort ushCode)
        {
            if (toggle)
                ledAlarm.Text = ushCode.ToString("X");
            else
                ledAlarm.Text = "ALARM";
        }

        private void SetRdy(bool bon)
        {
            if (bon)
                ledReady.BackColor = Color.Yellow;
            else
                ledReady.BackColor = Color.White;
        }

        private void SetServo(bool bon)
        {
            if (bon)
                ledServo.BackColor = Color.Yellow;
            else
                ledServo.BackColor = Color.White;
        }

        private void SetInposi(bool bon)
        {
            if (bon)
                ledInposi.BackColor = Color.Yellow;
            else
                ledInposi.BackColor = Color.White;
        }

        private void SetCw(bool bon)
        {
            if (bon)
                ledCw.BackColor = Color.Yellow;
            else
                ledCw.BackColor = Color.White;
        }

        private void SetCcw(bool bon)
        {
            if (bon)
                ledCcw.BackColor = Color.Yellow;
            else
                ledCcw.BackColor = Color.White;
        }

        private void SetOrg(bool bon)
        {
            if (bon)
                ledOrg.BackColor = Color.Yellow;
            else
                ledOrg.BackColor = Color.White;
        }        
        #endregion

        #region [ Function ]

        #region - Private Functions
        private void InitialXML()
        {
            int i, j;
            //string strDB = "";

            for (i = 0; i < Define.MAX_RECIPE_NUM; i++)
            {
                for (j = 0; j < Define.MAX_MOTORDATA_FILE_NUM; j++)
                {
                    //strDB = string.Format("MotorData{0:D3}_{1:D1}0", i + 1, j + 1);
                    XMLMotorData[i, j] = new XML();                    
                }
            }

            this.XMLMotorConfig.Initializ("MotorConfig");

            this.XMLMotorJogData.Initializ("MotorJogData");
        }

        private void InitialMotorSelectCombobox()
        {
            cbSelMotor.Items.Clear();

            for (int i = 0; i < XMLMotorConfig.GetDataTableCount(); i++)
            {
                if (XMLMotorConfig.ReadDataTable(i, strMotorConfigColName2) == "true")
                {                                         
                    int nNo = Convert.ToInt32(XMLMotorConfig.ReadDataTable(i, strMotorConfigColName0)) - 1;
                    cbSelMotor.Items.Add("[" + nNo.ToString("00") + "] " + XMLMotorConfig.ReadDataTable(i, strMotorConfigColName1));
                    iNowMotorNum = i + 1;
                }
                else
                    return;
            }
        }

        private void InitialControls()
        {
            gbMotorMove.Enabled = false;
            btnIndexMove.Enabled = false;
            btnRepeatMove.Enabled = false;
            btnPosIndexWrite.Enabled = false;
            gbAllVelocity.Enabled = false;
            btnStop.Enabled = false;

            lbJogDist.Text = "0.1";
            lbJogVel.Text = "1";
            lbIndexNo.Text = "0";

            cbDistLevel.Items.Add("0.001");
            cbDistLevel.Items.Add("0.010");
            cbDistLevel.Items.Add("0.100");
            cbDistLevel.Items.Add("1.000");
            cbDistLevel.SelectedIndex = 0;

            cbVelLevel.Items.Add("0.001");
            cbVelLevel.Items.Add("0.010");
            cbVelLevel.Items.Add("0.100");
            cbVelLevel.Items.Add("1.000");
            cbVelLevel.SelectedIndex = 0;

            cbAllVelocity.Items.Add("100");
            cbAllVelocity.Items.Add("80");
            cbAllVelocity.Items.Add("50");
            cbAllVelocity.Items.Add("30");
            cbAllVelocity.Items.Add("10");            
        }

        private void ReadAllVelocityRate()
        {
            string strVelPercent = XMLMotorJogData.ReadDataTable((int)VelocityRate.XML_INDEX, strMotorJogDataColName1);
            int nIdx = int.Parse(strVelPercent);
            if (nIdx < cbAllVelocity.Items.Count)
            {
                cbAllVelocity.SelectedIndex = nIdx;

                MtInfo mtInfo = new MtInfo();
                mtInfo.nVel = nIdx;
                SM.SHM.SetButtonNum(BUTTON.MT_VELRATE, mtInfo);

                UI.Logs.Write(LOGPART.MT, string.Format("ALL MOTOR, VELOCITY RATE CHANGE !!", (int)mtInfo.eMt));
            }
        }
		
        private bool ReconfirmPassword()
        {
            //비밀번호 재확인
            if (Program.m_PasswordMode == PW_MODE.ADMIN)
                return false;
            if (UI.ModalLevelSetting.ReadLevelXMLData((int)Program.m_PasswordMode, "RECONFIRM_PW"))
                return true;

            return false;
        }

        #endregion

        #region - Public Functions

        // Recipe 복사시 호출한다.
        public void MotorRecipeCopy(int iRef, int iTarget)
        {
            // motor database initdatabase.
            string strDB;
            for (int k = 0; k < Define.MAX_MOTORDATA_FILE_NUM; k++)
            {
                strDB = string.Format("MotorData{0:D3}_{1:D1}0", iTarget + 1, k + 1);
                if (XMLMotorData[iTarget, k].Initializ(strDB, iTarget + 1) == false) return;
            }

            for (int i = 0; i < Define.MAX_MOTORDATA_FILE_NUM; i++)
            {
                for (int j = 0; j < Define.MAX_MOTORDATA_NUM * 10; j++)
                {
                    XMLMotorData[iTarget, i].WriteDataTable(j, strMotorDataColName2, XMLMotorData[iRef, i].ReadDataTable(j, strMotorDataColName2));
                    XMLMotorData[iTarget, i].WriteDataTable(j, strMotorDataColName3, XMLMotorData[iRef, i].ReadDataTable(j, strMotorDataColName3));
                    XMLMotorData[iTarget, i].WriteDataTable(j, strMotorDataColName4, XMLMotorData[iRef, i].ReadDataTable(j, strMotorDataColName4));
                }

                XMLMotorData[iTarget, i].UpdateData();
            }
        }

        // 해당 레시피에 모든(60개) 모터 테이블 정보를 얻는다. 
        public void GetMotorTableFromDB(int iRecipe)
        {
            string strPos = "";
            string strVel = "";

            int temp = 0;
            int rem = 0;
            int dbidx = 0;

            // motor database initdatabase.
            string strDB;
            for (int j = 0; j < Define.MAX_MOTORDATA_FILE_NUM; j++)
            {
                strDB = string.Format("MotorData{0:D3}_{1:D1}0", iRecipe + 1, j + 1);                
                if (XMLMotorData[iRecipe, j].Initializ(strDB, iRecipe + 1) == false) return;
            }

            // MotorConfig에 설정되어 있는 모터 갯수만큼만.
            for (int i = 0; i < Define.MAX_MOTOR_NUM; i++)
            {
                for (int j = 0; j < Define.MAX_MOTORDATA_NUM; j++)
                {
                    temp = i / 10;
                    rem = i % 10;
                    dbidx = j + (rem * 100);
                    strPos = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName3);
                    strVel = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName4);

                    if (strPos == "")
                        arrMtTable[i].nPos[j] = 0;
                    else
                        arrMtTable[i].nPos[j] = Convert.ToInt32(strPos);

                    if (strVel == "")
                        arrMtTable[i].nSpeed[j] = 0;
                    else
                        arrMtTable[i].nSpeed[j] = Convert.ToUInt32(strVel);
                }
            }
        }

        public void GetMotorTableFromDB(int iRecipe, int MtNum)
        {

            string strPos = "";
            string strVel = "";

            int temp = 0;
            int rem = 0;
            int dbidx = 0;

            for (int j = 0; j < Define.MAX_MOTORDATA_NUM; j++)
            {
                temp = MtNum / 10;
                rem = MtNum % 10;
                dbidx = j + (rem * 100);
                strPos = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName3);
                strVel = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName4);

                if (strPos == "")
                    arrMtTable[MtNum].nPos[j] = 0;
                else
                    arrMtTable[MtNum].nPos[j] = Convert.ToInt32(strPos);

                if (strVel == "")
                    arrMtTable[MtNum].nSpeed[j] = 0;
                else
                    arrMtTable[MtNum].nSpeed[j] = Convert.ToUInt32(strVel);
            }
        }

        public bool IsWorking()
        {
            if (chkMotorModify.Checked || chkMotorJog.Checked)
                return true;
            return false;
        }

        public void WorkEnd()
        {
            if (chkMotorModify.Checked || chkMotorJog.Checked)
                btnMotorSave.PerformClick();
        }

        public void DisableWork()
        {
            if (btnMotorSave.Enabled)
                btnMotorSave.Enabled = false;
            if (chkMotorJog.Enabled)
                chkMotorJog.Enabled = false;
            if (chkMotorModify.Enabled)
                chkMotorModify.Enabled = false;
        }

        public void EnableWork()
        {
            if (!btnMotorSave.Enabled)
                btnMotorSave.Enabled = true;
            if (!chkMotorJog.Enabled)
                chkMotorJog.Enabled = true;
            if (!chkMotorModify.Enabled)
                chkMotorModify.Enabled = true;

            ///UpdateCheckBox(Global.ModalPwMode.Password_Now);
        }
		
        public void UpdateCheckBox(PW_MODE Password_Now)
        {            
            if(!chkMotorJog.Checked)
                chkMotorJog.Enabled = true;
            else
                chkMotorJog.Enabled = false;

            if (!chkMotorModify.Checked)
                chkMotorModify.Enabled = true;
            else
                chkMotorModify.Enabled = false;

            btnMotorSave.Enabled = true;
        }

        public void UpdateButton(PW_MODE Password_Now)
        {
            int nIdx = 0;
            switch (Password_Now)
            {
                case PW_MODE.OPER:  nIdx = 0;   break;
                case PW_MODE.ENG:   nIdx = 1;   break;
                case PW_MODE.PRO:   nIdx = 2;   break;
                case PW_MODE.ADMIN:
                    btnTeachingMode.Enabled = true;
                    return;
            }

            btnTeachingMode.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "TEACHING");
        }

        public int GetMotorIndexPosition(int motornum, int index)
        {
            if (motornum >= Define.MAX_MOTOR_NUM)
                motornum = Define.MAX_MOTOR_NUM - 1;
            if (motornum < 0)
                motornum = 0;

            if (index >= Define.MAX_MOTORDATA_NUM)
                index = Define.MAX_MOTORDATA_NUM - 1;
            if (index < 0)
                index = 0;

            int temp = motornum / 10;
            int rem = motornum % 10;
            int dbidx = index + (rem * 100);
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            string strPosition = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName3);

            return Convert.ToInt32(strPosition);
        }

        // 모터 테이블 정보를 구조체로 얻는다.
        public bool GetMotorTable(int iMtNum, ref TABLE_MOTOR stTable)
        {
            if (iMtNum >= Define.MAX_MOTOR_NUM)
                return false;

            stTable = arrMtTable[iMtNum];

            return true;
        }

        public int GetMotorIndexSpeed(int motornum, int index)
        {
            if (motornum >= Define.MAX_MOTOR_NUM)
                motornum = Define.MAX_MOTOR_NUM - 1;
            if (motornum < 0)
                motornum = 0;

            if (index >= Define.MAX_MOTORDATA_NUM)
                index = Define.MAX_MOTORDATA_NUM - 1;
            if (index < 0)
                index = 0;

            int temp = motornum / 10;
            int rem = motornum % 10;
            int dbidx = index + (rem * 100);
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            string strSpeed = XMLMotorData[iRecipe, temp].ReadDataTable(dbidx, strMotorDataColName4);

            return Convert.ToInt32(strSpeed);
        }

        public bool SetMotorIndexPosition(int motornum, int index, int postion)
        {
            if (motornum >= Define.MAX_MOTOR_NUM)
                return false;
            if (motornum < 0)
                return false;

            if (index >= Define.MAX_MOTORDATA_NUM)
                return false;
            if (index < 0)
                return false;

            int temp = motornum / 10;
            int rem = motornum % 10;
            int dbidx = index + (rem * 100);
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            XMLMotorData[iRecipe, temp].WriteDataTable(dbidx, strMotorDataColName3, postion.ToString());

            return true;
        }

        public bool SetMotorIndexSpeed(int motornum, int index, int speed)
        {
            if (motornum >= Define.MAX_MOTOR_NUM)
                return false;
            if (motornum < 0)
                return false;

            if (index >= Define.MAX_MOTORDATA_NUM)
                return false;
            if (index < 0)
                return false;

            int temp = motornum / 10;
            int rem = motornum % 10;
            int dbidx = index + (rem * 100);
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            XMLMotorData[iRecipe, temp].WriteDataTable(dbidx, strMotorDataColName4, speed.ToString());

            return true;
        }

        public bool SaveMotorIndex(int motornum)
        {
            if (motornum >= Define.MAX_MOTOR_NUM)
                return false;
            if (motornum < 0)
                return false;

            int temp = motornum / 10;
            int rem = motornum % 10;
            int iRecipe = UI.SubpageRecipe.iNowRecipeNum;

            XMLMotorData[iRecipe, temp].UpdateData();
            XMLMotorData[iRecipe, temp].RefillData();

            this.RedrawMotorGrid(motornum);

            this.GetMotorTableFromDB(iRecipe, motornum);
            SM.SHM.SetMotorTableData((uint)motornum, this.arrMtTable[motornum]);

            // 모터 값을 읽어온다 -LWJ
            //Global.COM.SetCommand(CMD.MS_READ_MOTOR_TABLE, (uint)motornum);

            return true;
        }
		
        public void SetSelectedMotorIndex(int nIdx)
        {
            btnManualCyl.Visible = true;
            cbSelMotor.SelectedIndex = nIdx;
        }

        #endregion

        #region - Language Function 
        public void SetLanguageText()
        {
            lbMotorSelect.Text = Language.languageDefault.MotorSelect;
            chkMotorJog.Text = Language.languageDefault.JogMode;
            chkMotorModify.Text = Language.languageDefault.MotorModify;
            gbMotorStatus.Text = Language.languageDefault.MotorStatus;
            gbMotorMove.Text = Language.languageDefault.MotorMove;
            lbDistance.Text = Language.languageDefault.Distance;
            lbVelocity.Text = Language.languageDefault.Velocity;
            btnIndexMove.Text = Language.languageDefault.IndexMove;
            btnPosIndexWrite.Text = Language.languageDefault.PositionIndexWrite;
            gbJogMove.Text = Language.languageDefault.JogMove;
            lbJogMinus.Text = Language.languageDefault.MinusJog;
            lbJogPlus.Text = Language.languageDefault.PlusJog;
            gbStepMove.Text = Language.languageDefault.StepMove;
            lbStepMinus.Text = Language.languageDefault.MinsuStep;
            lbStepPlus.Text = Language.languageDefault.PlusStep;
			gbAllVelocity.Text = Language.languageDefault.VelocityRate;
			btnVelIndexWrite.Text = Language.languageDefault.VelocityIndexWrite;
			btnRepeatMove.Text = Language.languageDefault.RepeatMode;
			iNDEXWRITEToolStripMenuItem.Text	= Language.languageDefault.PositionIndexWrite;
			lbItkMsg.Text	= Language.languageDefault.ItkMsg;
			btnTeachingMode.Text	= Language.languageDefault.TeachingMode;

			dgridMotor.Columns[0].HeaderText = Language.languageDefault.DGRID_HD_No;
			dgridMotor.Columns[1].HeaderText = Language.languageDefault.DGRID_HD_Index;
			dgridMotor.Columns[2].HeaderText = Language.languageDefault.DGRID_HD_Position;
			dgridMotor.Columns[3].HeaderText = Language.languageDefault.DGRID_HD_Velocity;
        }
        #endregion

        #endregion

		private	void	dgridMotor_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right)			return;
			if (e.RowIndex < 0  ||  e.ColumnIndex != 2)
			{
				dgridMotor.ContextMenuStrip	= null;
				return;
			}

			if (chkMotorJog.Checked == true)			dgridMotor.ContextMenuStrip	= cmsGridMotor;
			else										dgridMotor.ContextMenuStrip	= null;

			dgridMotor.ClearSelection();
			dgridMotor.CurrentCell			= dgridMotor[e.ColumnIndex, e.RowIndex];
			dgridMotor.CurrentCell.Selected	= true;

            lbIndexNo.Text = Convert.ToString(e.RowIndex);
		}

		private	void	btnTeachingMode_Click(object sender, EventArgs e)
		{
			  if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false)
            {
                if (ReconfirmPassword() == true)
                {
                    if (UI.ModalKeyBoard.ShowDialog() != DialogResult.OK)
                        return;
                    if (UI.ModalKeyBoard.Value != UI.ModalManage.ReadXMLData("PW"))
                        return;
                }

                string strMsg = string.Format("This is an UNINTERLOCKED state!");
                frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Warnning, strMsg);
                if (pMsg.ShowDialog() == DialogResult.Cancel)
                    return;

                UI.Logs.Write(LOGPART.PW, "Click Motor Teaching Mode !!");
                 
                SM.SHM.SetBitData(BIT.MotorTeachingMode, true);
                btnTeachingMode.BackColor = Color.Red;
            }
            else
            {
                SM.SHM.SetBitData(BIT.MotorTeachingMode, false);
                btnTeachingMode.BackColor = Color.White;
            }
		}
	}
}
