using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmData_Recipe : Form
    {        
        public frmData_Recipe()
        {
            InitializeComponent();
            InitialXML();
            InitialGrid();

            // 초기화 시에 상태
            btnRecipeDataSave.Enabled	= false;
			btnTraySizeSenSel.Enabled	= false;
			btnTraySet.Enabled			= false;
            lbSelectRecipe.Text = Convert.ToString(iDataNow + 1);
            dgridRecipe.Rows[iDataNow].Selected = true;

            // 모터 데이터 저장하기
            UI.PageMotor.GetMotorTableFromDB(this.iDataNow);
			
			// Check : Gap Size 정보 로드
			// check : 현재 안쓰는 기능이므로 항상 2번의 데이터를 가져오도록 수정
			//if (SM.SHM.GapSizeOpen(iDataNow+1) == false)
			if (SM.SHM.GapSizeOpen(iDataNow + 1) == false)
			{
				//frmTrayPnPRecipeEditor	frm	= new frmTrayPnPRecipeEditor(iDataNow+1);
				frmTrayPnPRecipeEditor	frm	= new frmTrayPnPRecipeEditor(iDataNow + 1);
				frm.ShowDialog();
			}

			XMLRcpMng.Initializ("RecipeManager");
        }

        #region [ Variables ]
        private XML XMLRecipe	= new XML();
		private	XML	XMLRcpMng	= new XML();

        private string strRecipeCol0 = "RecipeName";

        private XML[] XMLRecipeData = new XML[Define.MAX_RECIPE_NUM];
        private string strRecipeDataCol0 = "RecipeDataName";
        private string strRecipeDataCol1 = "RecipeDataValue";
        private string strRecipeDataCol2 = "RecipeDataUnit";

        private int iDataNow;  // current set Recipe data. (0~(MAX_RECIPEDATA_NUM-1))
		
        public string[] strRecipeData = new string[Define.MAX_RECIPEDATA_NUM];


        public int iNowRecipeNum	{ get => iDataNow; }

		public	int SelRecipeNum	{ get => Convert.ToInt32(lbSelectRecipe.Text) - 1; }
        #endregion

        #region [ Event Function ]

        #endregion

        #region [ Timmer Function ]

        #endregion

        #region [ CheckBox Function ]

        private void chkRecipeDataMod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecipeDataMod.Checked)
            {
                chkRecipeDataMod.ForeColor = Color.Red;
                chkRecipeDataMod.Enabled = false;
                tbColumnRecipeDataName.ReadOnly = false;
                tbColumnRecipeDataUnit.ReadOnly = false;
                tbColumnRecipeDataValue.ReadOnly = false;

                chkRecipeModify.Enabled = false;
                btnRecipeChange.Enabled = false;
                btnPickerSelect.Enabled = false;
                btnRecipeCopy.Enabled = false;
                btnRecipeDataSave.Enabled = true;
				btnTraySizeSenSel.Enabled	= true;
				btnTraySet.Enabled			= true;

                dgridRecipe.Enabled = false;
            }
        }

        private void chkRecipeModify_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRecipeModify.Checked)
            {
                tbColumnRecipeName.ReadOnly = false;
                chkRecipeModify.ForeColor = Color.Red;

                chkRecipeDataMod.Enabled = false;
                btnPickerSelect.Enabled = false;
                dgridRecipe.Enabled = true;
                btnRecipeChange.Enabled = true;
                btnRecipeCopy.Enabled = true;
            }
            else
            {
                XMLRecipe.UpdateData();

                UpdateNowRecipeDisp();

                tbColumnRecipeName.ReadOnly = true;
                chkRecipeModify.ForeColor = Properties.Settings.Default._DarkOliveGreen;

                chkRecipeDataMod.Enabled = true;
                btnPickerSelect.Enabled = true;
                dgridRecipe.Enabled = false;
                btnRecipeChange.Enabled = false;
                btnRecipeCopy.Enabled = false;

                dgridRecipe.Rows[iDataNow].Selected = true;
                lbSelectRecipe.Text = Convert.ToString(iDataNow + 1);
            }
        }
        #endregion

        #region [ Button Event ]

        private void btnRecipeChange_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbSelectRecipe.Text) - 1 == iDataNow) return;
            if (Convert.ToInt32(lbSelectRecipe.Text) == 0) return;

            // Confirmation.
            string strMsg = string.Format(Language.languageDefault.MB_RecipeChange + "\n[{0:D2}] -> [{1:D2}]", iDataNow + 1, Convert.ToInt32(lbSelectRecipe.Text));

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, strMsg);

            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

			if (SM.SHM.GapSizeOpen(iDataNow+1) == false)
			{
				frmTrayPnPRecipeEditor	frm	= new frmTrayPnPRecipeEditor(iDataNow+1);
				frm.ShowDialog();
			}
			

            RecipeChange(Convert.ToInt32(lbSelectRecipe.Text) - 1);
        }

        private void btnRecipeDataSave_Click(object sender, EventArgs e)
        {
            if (!chkRecipeDataMod.Checked) return;

            XMLRecipeData[iDataNow].UpdateData();
            XMLRecipeData[iDataNow].RefillData();

            UpdateRecipeDataGrid(iDataNow);

            chkRecipeDataMod.ForeColor	= Properties.Settings.Default._DarkOliveGreen;
            chkRecipeDataMod.Checked	= false;
            chkRecipeDataMod.Enabled	= true;
            btnRecipeDataSave.Enabled	= false;
			btnTraySizeSenSel.Enabled	= false;
			btnTraySet.Enabled			= false;

            tbColumnRecipeDataName.ReadOnly = true;
            tbColumnRecipeDataUnit.ReadOnly = true;
            tbColumnRecipeDataValue.ReadOnly = true;

            chkRecipeModify.Enabled = true;
            btnPickerSelect.Enabled = true;
            dgridRecipe.Enabled = true;
            
            SM.SHM.SetRecipeData(this.strRecipeData);
            //Global.SHM.WriteSHM_Recipe();

            // Recipe Read -LWJ
            //Global.COM.SetCommand(CMD.MS_READ_RECIPE);

            // LOG
            string msg_ = string.Format("[{0:D2}] Recipe Data Save !!", iDataNow + 1);
            UI.Logs.Write(LOGPART.DATA, msg_);
            
            // History Log
            Dictionary<string, XML.CompareData> compData = XMLRecipeData[iDataNow].GetCompareData();
            foreach (var data in compData)
            {
                if (data.Value.colname == strRecipeDataCol1)
                {
                    double dOldValue , dNowvalue;
                    if (double.TryParse(data.Value.oldvalue, out dOldValue) == false) return;
                    if (double.TryParse(data.Value.nowvalue, out dNowvalue) == false) return;
                   dOldValue = dOldValue/1000;
                   dNowvalue = dNowvalue/1000;

                   string msg = string.Format("Recipe Data({0}[{1:D2}]),{2}[{3:D2}]: [{4:0.000}] ▶ [{5:0.000}]",
                        UI.SubpageRecipe.GetRecipeName(iDataNow),iDataNow+1,dgridRecipeData.Rows[data.Value.index].Cells[1].Value.ToString(),data.Value.index,dOldValue,dNowvalue);
                    
                    UI.Logs.Write(LOGPART.HIS, msg);
                }
            }
            XMLRecipeData[iDataNow].CompareDataClear();
        }

        private void btnRecipeCopy_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(lbSelectRecipe.Text) - 1 == iDataNow) return;

            // Confirmation.
            string strMsg_ = string.Format(Language.languageDefault.MB_RecipeCopy + "\n[{0:D2}] -> [{1:D2}]", iDataNow + 1, Convert.ToInt32(lbSelectRecipe.Text));

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, strMsg_);

            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            string strTemp;
            int iTarget = Convert.ToInt32(lbSelectRecipe.Text) - 1;
            strTemp = string.Format("RecipeData{0:D3}", iTarget + 1);

            if (XMLRecipeData[iTarget].Initializ(strTemp, iTarget + 1) == false)
                return;

            UI.PageMotor.MotorRecipeCopy(iDataNow, iTarget);
            RecipeDataCopy(iDataNow, iTarget);

            // LOG
            string msg_ = string.Format(" [{0:D2}] -> [{1:D2}] Recipe Copy !!", iDataNow + 1, Convert.ToInt32(lbSelectRecipe.Text));
            UI.Logs.Write(LOGPART.DATA, msg_);

        }

        private void btnPickerSelect_Click(object sender, EventArgs e)
        {
            frmPickerSelect pPickerSelect = new frmPickerSelect();
            pPickerSelect.SetLanguageText();
            pPickerSelect.Show();
        }

		private	void	btnPartNoEdit_Click(object sender, EventArgs e)
		{
			Form	fc	= Application.OpenForms["frmRecipeManager"];
			if (fc != null)			return;

			frmRecipeManager	pup	= new frmRecipeManager();

			pup.Show();
		}

		private void btnTraySizeSenSel_Click(object sender, EventArgs e)
		{
			Form	fc	= Application.OpenForms["frmTraySizeSensorSel"];
			if (fc != null)			return;

			frmTraySizeSensorSel	popup	= new frmTraySizeSensorSel();
            popup.SetLanguageText();
            popup.Show();
		}

		
		private void btnTraySet_Click(object sender, EventArgs e)
		{
			//frmTrayPnPRecipeEditor	frm	= new frmTrayPnPRecipeEditor(iDataNow + 1);
			frmTrayPnPRecipeEditor	frm	= new frmTrayPnPRecipeEditor(iDataNow + 1);

			frm.ShowDialog();
		}
        #endregion

        #region [ Grid Setting ]

        #region - Function 
        private void InitialGrid()
        {
			
			int nVisibleRowCount = 16;
            dgridRecipe.RowTemplate.Height = dgridRecipe.Height / (nVisibleRowCount + 1);
            dgridRecipe.ColumnHeadersHeight = dgridRecipe.Height - dgridRecipe.RowTemplate.Height * nVisibleRowCount;

            dgridRecipeData.RowTemplate.Height = dgridRecipeData.Height / (nVisibleRowCount + 1);
            dgridRecipeData.ColumnHeadersHeight = dgridRecipeData.Height - dgridRecipeData.RowTemplate.Height * nVisibleRowCount;

            for (int i = 0; i < Define.MAX_RECIPE_NUM; i++)
                this.dgridRecipe.Rows.Add(i + 1, XMLRecipe.ReadDataTable(i, strRecipeCol0));

            tbColumnRecipeNum.ReadOnly = true;
            tbColumnRecipeName.ReadOnly = true;
            dgridRecipe.RowTemplate.Height = 35;
			
            for (int i = 0; i < Define.MAX_RECIPEDATA_NUM; i++)
            {
				string strTemp = "";
                string strTemp0 = XMLRecipeData[iDataNow].ReadDataTable(i, strRecipeDataCol0);
                string strTemp1 = XMLRecipeData[iDataNow].ReadDataTable(i, strRecipeDataCol1);
                string strTemp2 = XMLRecipeData[iDataNow].ReadDataTable(i, strRecipeDataCol2);
				if (double.TryParse(strTemp1, out double dResult) == true)
				{
					strTemp = (dResult / 1000).ToString("0.000");
				}
				else
				{
					strTemp = strTemp1;
				}
                this.dgridRecipeData.Rows.Add(i, strTemp0, strTemp, strTemp2);

                this.strRecipeData[i] = strTemp1;
            }

            tbColumnRecipeDataNum.ReadOnly = true;
            tbColumnRecipeDataName.ReadOnly = true;
            tbColumnRecipeDataValue.ReadOnly = true;
            tbColumnRecipeDataUnit.ReadOnly = true;

            UpdateNowRecipeDisp();
        }

        private void UpdateRecipeGrid()
        {
            this.dgridRecipe.Rows.Clear();

            for (int i = 0; i < Define.MAX_RECIPE_NUM; i++)
                this.dgridRecipe.Rows.Add(i + 1, XMLRecipe.ReadDataTable(i, strRecipeCol0));
        }

        public void UpdateRecipeDataGrid(int recipeno)
        {
			this.dgridRecipeData.Rows.Clear();

			for (int i = 0; i < Define.MAX_RECIPEDATA_NUM; i++)
			{
				string strTemp = "";
				string strTemp0 = XMLRecipeData[recipeno].ReadDataTable(i, strRecipeDataCol0);
				string strTemp1 = XMLRecipeData[recipeno].ReadDataTable(i, strRecipeDataCol1);
				string strTemp2 = XMLRecipeData[recipeno].ReadDataTable(i, strRecipeDataCol2);
				if (double.TryParse(strTemp1, out double dResult) == true)
				{
					strTemp = (dResult / 1000).ToString("0.000");
				}
				else
				{
					strTemp = strTemp1;
				}
				this.dgridRecipeData.Rows.Add(i, strTemp0, strTemp, strTemp2);
				this.strRecipeData[i] = strTemp1;
			}

		}
		#endregion

		#region - Event

		private void dgridRecipeData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex != -1)
			{
				if (dgridRecipeData.ReadOnly) return;

				double doValue = 0;
				bool isNum = false;
				string strNewValue = "";
				if (dgridRecipeData[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
					strNewValue = "";
				else
					strNewValue = dgridRecipeData[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();
				//int value = 0;

				if (e.ColumnIndex == 1)
				{
					XMLRecipeData[iDataNow].WriteDataTable(e.RowIndex, strRecipeDataCol0, strNewValue);
				}
				else if (e.ColumnIndex == 2)
				{
					if (Double.TryParse(strNewValue, out doValue))
					{
						String sVal = strNewValue.ToString();
						Int32 num = Convert.ToInt32((Convert.ToDouble(sVal) * 1000));
						XMLRecipeData[iDataNow].WriteDataTable(e.RowIndex, strRecipeDataCol1, num.ToString());
					}
					else
					{
						XMLRecipeData[iDataNow].WriteDataTable(e.RowIndex, strRecipeDataCol1, strNewValue);
					}
				}
				else
				{
					XMLRecipeData[iDataNow].WriteDataTable(e.RowIndex, strRecipeDataCol2, strNewValue);
				}
			}
		}
		
        private void dgridRecipeData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
			if (chkRecipeDataMod.Checked == false) return;
			if (e.RowIndex != 0 || e.ColumnIndex != 2) return;
			
			Rectangle cellRect = dgridRecipeData.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);
			ComboBox combo = new ComboBox
			{
				DropDownStyle = ComboBoxStyle.DropDownList,
				Font = new Font("Segoe UI", 12f),
			};


			for (int i = 0; i <= (int)ModuleType.LPCAMM; i++)
			{
				combo.Items.Add(EnumHelper.GetDescription((ModuleType)i));
			}
			ModuleType SelectedModule = EnumHelper.GetValueFromDescription<ModuleType>(SM.SHM.GetRecipeData(DEVICE.ModuleType));

			// 기존 값 반영
			combo.SelectedItem = dgridRecipeData[e.ColumnIndex, e.RowIndex].Value;

			// 선택 시 셀에 값 반영하고 콤보 제거
			combo.SelectedIndexChanged += (s, ev) =>
			{
				dgridRecipeData[e.ColumnIndex, e.RowIndex].Value = combo.SelectedItem;
				dgridRecipeData.Controls.Remove(combo);
				combo.Dispose();
			};

			// 다른 곳 클릭 시 콤보 제거
			combo.LostFocus += (s, ev) =>
			{
				dgridRecipeData.Controls.Remove(combo);
				combo.Dispose();
			};

			// 위치 및 크기 조정 후 DataGridView 위에 표시
			combo.Bounds = cellRect;
			dgridRecipeData.Controls.Add(combo);
			combo.Focus();
			combo.DroppedDown = true; // 클릭 즉시 드롭다운 열기

		}



		private void dgridRecipe_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;
            lbSelectRecipe.Text = Convert.ToString(e.RowIndex + 1);
        }

        private void dgridRecipe_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (dgridRecipe.ReadOnly) return;

                string strNewValue = "";
                if (dgridRecipe[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                    strNewValue = "";
                else
                    strNewValue = dgridRecipe[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

                this.XMLRecipe.WriteDataTable(e.RowIndex, strRecipeCol0, strNewValue);
            }
        }
        
        private void dgridRecipeData_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!chkRecipeDataMod.Checked) return;

            string strMsg = "";
            if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                strMsg = "Do you want to update the data name?\r\n(Initialize the existing name)";
            else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
                strMsg = "데이터이름을 업데이트하시겠습니까?\r\n(기존 이름은 초기화됩니다.)";

            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, strMsg);
            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            for (int i = 0; i < Define.MAX_RECIPEDATA_NUM; i++)
            {
                if (EnumHelper.GetDescription((DEVICE)i) == i.ToString())
                    XMLRecipeData[iDataNow].WriteDataTable(i, strRecipeDataCol0, " ");
                else
                    XMLRecipeData[iDataNow].WriteDataTable(i, strRecipeDataCol0, EnumHelper.GetDescription((DEVICE)i));
                
                dgridRecipeData.Rows[i].Cells[1].Value = XMLRecipeData[iDataNow].ReadDataTable(i, strRecipeDataCol0);
            }
            XMLRecipeData[iDataNow].UpdateData();
            XMLRecipeData[iDataNow].RefillData();
        }
        #endregion

        #endregion

        #region [ Function ]

        #region - Private Function

        private void InitialXML()
        {
            if (XMLRecipe.Initializ("Recipe") == false) 
                return;

            iDataNow = Convert.ToInt32(XMLRecipe.ReadDataTable(200, strRecipeCol0));

            for (int i = 0; i < Define.MAX_RECIPE_NUM; i++)
            {
                XMLRecipeData[i] = new XML();
				string strTemp = string.Format("RecipeData{0:D3}", i + 1);
				 
				if (XMLRecipeData[i].Initializ(strTemp, i + 1) == false) 
				     return;
            }

//             else
//                 XMLRecipeData[iDataNow].Initializ(strTemp, -2, iDataNow, iDataNow + 1); // New 버전 생성(D:DATA//Recipe_000 에 있는 DATA 복사)
        }

        private void UpdateNowRecipeDisp()
        {
            XMLRecipe.WriteDataTable(200, strRecipeCol0, Convert.ToString(iDataNow));
            XMLRecipe.UpdateData();
        }

        private void RecipeDataCopy(int iRef, int iTarget)
        {
            for (int i = 0; i < Define.MAX_RECIPEDATA_NUM; i++)
            {
                XMLRecipeData[iTarget].WriteDataTable(i, strRecipeDataCol0, XMLRecipeData[iRef].ReadDataTable(i, strRecipeDataCol0));
                XMLRecipeData[iTarget].WriteDataTable(i, strRecipeDataCol1, XMLRecipeData[iRef].ReadDataTable(i, strRecipeDataCol1));
                XMLRecipeData[iTarget].WriteDataTable(i, strRecipeDataCol2, XMLRecipeData[iRef].ReadDataTable(i, strRecipeDataCol2));
            }

            XMLRecipeData[iTarget].UpdateData();

            XMLRecipe.WriteDataTable(iTarget, strRecipeCol0, XMLRecipe.ReadDataTable(iRef, strRecipeCol0));

            XMLRecipe.UpdateData();
            UpdateRecipeGrid();
        }
        #endregion

        #region - Public Function

        public string GetRecipeData(int num)
        {
            return strRecipeData[num];
        }

        public string GetRecipeData(DEVICE num)
        {
            return strRecipeData[(int)num];
        }


        public string GetRecipeName(int nowno)
        {
            return XMLRecipe.ReadDataTable(nowno, strRecipeCol0);
        }

        public string GetNowRecipeDataName(int dataNo)
        {
            return XMLRecipeData[iDataNow].ReadDataTable(dataNo, strRecipeDataCol0);
        }

        public void SetNowRecipeDataName(int dataNo, string value)
        {
            XMLRecipeData[iDataNow].WriteDataTable(dataNo, strRecipeDataCol0, value);

            XMLRecipeData[iDataNow].UpdateData();
            XMLRecipeData[iDataNow].RefillData();

            SM.SHM.SetRecipeData(this.strRecipeData);
        }

        public bool GetNowRecipeData(ref uint[] pBuf)
        {
            if (pBuf.Length > strRecipeData.Length) return false;

            Array.Copy(strRecipeData, pBuf, pBuf.Length);

            return true;
        }

        public bool IsWorking()
        {
            if (chkRecipeDataMod.Checked || chkRecipeModify.Checked)
                return true;
            return false;
        }

        public void WorkEnd()
        {
            if (chkRecipeModify.Checked)
                chkRecipeModify.Checked = false;

            if (chkRecipeDataMod.Checked)
                btnRecipeDataSave.PerformClick();
        }

        public void DisableWork()
        {
            if (chkRecipeDataMod.Enabled)
                chkRecipeDataMod.Enabled = false;
            if (chkRecipeModify.Enabled)
                chkRecipeModify.Enabled = false;
            if (btnRecipeChange.Enabled)
                btnRecipeChange.Enabled = false;
            if (btnRecipeDataSave.Enabled)
                btnRecipeDataSave.Enabled = false;
            if (btnPickerSelect.Enabled)
                btnPickerSelect.Enabled = false;
            if (btnRecipeCopy.Enabled)
                btnRecipeCopy.Enabled = false;
			if (btnTraySizeSenSel.Enabled)
				btnTraySizeSenSel.Enabled	= false;
			if (btnTraySet.Enabled)
				btnTraySet.Enabled			= false;
        }

        public void EnableWork()
        {
            if (!chkRecipeDataMod.Enabled)
                chkRecipeDataMod.Enabled = true;
            if (!chkRecipeModify.Enabled)
                chkRecipeModify.Enabled = true;
            if (!btnRecipeChange.Enabled)
                btnRecipeChange.Enabled = true;
            if (!btnRecipeDataSave.Enabled)
                btnRecipeDataSave.Enabled = true;
            if (!btnPickerSelect.Enabled)
                btnPickerSelect.Enabled = true;
            if (!btnRecipeCopy.Enabled)
                btnRecipeCopy.Enabled = true;
			if (!btnTraySizeSenSel.Enabled)
				btnTraySizeSenSel.Enabled	= true;
			if (!btnTraySet.Enabled)
				btnTraySet.Enabled	= true;
        }

        // add extern write recipe data. (여러번 사용하면 안됨)
        // 레시피 데이터 하나를 변경하여 Recipe MMI 창에도 변경하고, DB에 저장도 하고, SEQ.에 보내기도 함.
        public bool WriteRecipeData(int num, int data)
        {
            if (num >= Define.MAX_RECIPEDATA_NUM)
                return false;

            XMLRecipeData[iDataNow].WriteDataTable(num, strRecipeDataCol1, data.ToString());

            XMLRecipeData[iDataNow].UpdateData();
            XMLRecipeData[iDataNow].RefillData();

            UpdateRecipeDataGrid(iDataNow);

            SM.SHM.SetRecipeData(this.strRecipeData);

            return true;
        }

        // SHM에는 카피 하지 않음 단순히 현재 Data Grid만 업데이트를 해줌
        public bool UpdateRecipeData()
        {
            UpdateRecipeDataGrid(iDataNow);

            return true;
        }

        // 레시피 데이터 하나만 변경함. (SaveRecipData와 한쌍)
        public bool SetRecipeData(int num, int data)
        {
            if (num >= Define.MAX_RECIPEDATA_NUM)
                return false;

            XMLRecipeData[iDataNow].WriteDataTable(num, strRecipeDataCol1, data.ToString());
            XMLRecipeData[iDataNow].UpdateData();
            XMLRecipeData[iDataNow].RefillData();

            SM.SHM.SetRecipeData((uint)num, data);

            return true;
        }

		public void UpdateDataGrid(PW_MODE Password_Now)
        {
              switch (Password_Now)
            {
                case PW_MODE.OPER:
                case PW_MODE.ENG:
                    foreach (DataGridViewRow row in dgridRecipeData.Rows)
                            row.DefaultCellStyle.BackColor = Color.LightGray;
                    dgridRecipeData.ForeColor = SystemColors.Window;
                    dgridRecipeData.Enabled = false;
                    break;
                case PW_MODE.PRO:
                    foreach (DataGridViewRow row in dgridRecipeData.Rows)
                            row.DefaultCellStyle.BackColor = Color.White;
                    dgridRecipeData.ForeColor = SystemColors.ControlText;
                    dgridRecipeData.Enabled = true;
                    break;
            }
        }


        // SetRecipeData로 여러 데이터 변경 한 후에 변경 작업 끝나면 한번에 DB 및 Recipe 화면 및 Seq. 에 업데이트함.
        public	bool	SaveRecipeData()
        {
            XMLRecipeData[iDataNow].UpdateData();
            XMLRecipeData[iDataNow].RefillData();

            UpdateRecipeDataGrid(iDataNow);

            SM.SHM.SetRecipeData(this.strRecipeData);

            return true;
        }
		
		public	bool	RecipeChange(int nRecipeIdx)
        {
			if (nRecipeIdx == iDataNow)			return true;

			UI.LoadingFormOpen();
            
            SM.SHM.SetBitData(BIT.AllHome, false);
            int beforeRecipe = iDataNow;

            iDataNow = nRecipeIdx;

            string strTemp;
            strTemp = string.Format("RecipeData{0:D3}", iDataNow + 1);
            if (XMLRecipeData[iDataNow].Initializ(strTemp, iDataNow + 1) == false)			return false;
            //XMLRecipeData[iDataNow].Initializ(strTemp, -2, iDataNow, iDataNow + 1); // New 버전 쓰기위한것
  
            UpdateRecipeDataGrid(iDataNow);
            UpdateNowRecipeDisp();

			dgridRecipe.Rows[iDataNow].Selected = true;
            lbSelectRecipe.Text = Convert.ToString(iDataNow + 1);

            // add send data sequence.
            UI.PageMotor.RecipeChange();
            SM.SHM.SetRecipeData(this.strRecipeData);
			
			if (SM.SHM.GapSizeOpen(iDataNow+1) == false)
			{
				frmTrayPnPRecipeEditor	frm	= new frmTrayPnPRecipeEditor(iDataNow+1);
				frm.ShowDialog();
			}

            // LOG
            string msg = string.Format(" [{0:D2}] -> [{1:D2}] Recipe Change !!", beforeRecipe + 1, iDataNow + 1);
            UI.Logs.Write(LOGPART.DATA, msg);

            UI.LoadingFormClose();
			

			return true;
        }

		public	bool	CheckAllEmpty()
		{

			//if (SM.SHM.IsBitData(BIT.RecipeChangeFail) == true)
			//{
			//	frmMessageBox	pup	= new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "No recipe selected. Select a recipe to use.");
   //             pup.ShowDialog();

			//	return false;
			//}

			int	nold_recipe_idx		= NDm.Read(eNDM.OldRecipeIdx);
            int	nrecipe_idx			= UI.SubpageRecipe.iNowRecipeNum;

			//Recipe 를 변경 한 경우 모든 자재, 메모리를 제거해야 동작할 수 있도록 체크
			//Check : 비교 항목 검토
			if (SM.SHM.GetRecipeData(nold_recipe_idx, DEVICE.ModuleType) != SM.SHM.GetRecipeData(DEVICE.ModuleType)  ||
				SM.SHM.GetRecipeDataToInt(nold_recipe_idx, DEVICE.TrayColCnt) != SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt)  ||
				SM.SHM.GetRecipeDataToInt(nold_recipe_idx, DEVICE.TrayRowCnt) != SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt))
			{
				for (int i = (int)FlowTray.LDConv01; i < (int)FlowTray.ULDConv31; i++)
                {
                    if (SM.FMT.IsFlow((FlowTray)i))
                    {
                        frmMessageBox	pup	= new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, string.Format("Tray has been Changed.\nPlease Remove all Trays manually\nOr Change Recipe No {0}.", NDm.Read(eNDM.OldRecipeIdx)+1));
                        pup.ShowDialog();

                        return false;
                    }
                }

                for (int i = (int)FlowPart.Empty; i < (int)FlowPart.Max; i++)
                {
                    if (SM.FMP.IsFlow((FlowPart)i))
                    {
						if		(SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Unload) == false  &&  ((FlowPart)i) == FlowPart.Insp1)			continue;
						else if (SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Unload) == false  &&  ((FlowPart)i) == FlowPart.Insp2)			continue;

                        frmMessageBox	pup	= new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, string.Format("Product has been Changed.\nPlease Remove all Products manually\nOr Change Recipe No {0}.", NDm.Read(eNDM.OldRecipeIdx)+1));
                        pup.ShowDialog();

                        return false;
                    }
                }
			}

			return true;
		}

		public	XML		GetXMLRecipData(int nIndex)
		{
			if (XMLRecipeData[nIndex].GetDataTableCount() == 0)
			{
				string strTemp;
				strTemp = string.Format("RecipeData{0:D3}", nIndex + 1);
				if (XMLRecipeData[nIndex].Initializ(strTemp, nIndex + 1) == false)			return null;
			}

			return XMLRecipeData[nIndex];
		}

        public void UpdateCheckBox(PW_MODE Password_Now)
        {
            int nIdx = 0;
            switch (Password_Now)
            {
                case PW_MODE.OPER:  nIdx = 0;   break;
                case PW_MODE.ENG:   nIdx = 1;   break;
                case PW_MODE.PRO:   nIdx = 2;   break;
                case PW_MODE.ADMIN:
                    chkRecipeModify.Enabled = true;
                    chkRecipeDataMod.Enabled = true;
                    return;
            }

            chkRecipeModify.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "RECIPE");
            chkRecipeDataMod.Enabled = UI.ModalLevelSetting.ReadLevelXMLData(nIdx, "RECIPE");
        }

        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            btnPickerSelect.Text = Language.languageDefault.Vacuum;
            btnRecipeCopy.Text = Language.languageDefault.Copy;
            btnRecipeChange.Text = Language.languageDefault.Change;
            btnRecipeDataSave.Text = Language.languageDefault.Save;
            chkRecipeModify.Text = Language.languageDefault.RecipeModify;
            chkRecipeDataMod.Text = Language.languageDefault.DataModify;
			btnPartNoEdit.Text	= Language.languageDefault.AutoChange;

			dgridRecipe.Columns[0].HeaderText = Language.languageDefault.DGRID_HD_No;
			dgridRecipe.Columns[1].HeaderText = Language.languageDefault.DGRID_HD_Recipe;

			dgridRecipeData.Columns[0].HeaderText = Language.languageDefault.DGRID_HD_No;
			dgridRecipeData.Columns[1].HeaderText = Language.languageDefault.DGRID_HD_RecipeData;
			dgridRecipeData.Columns[2].HeaderText = Language.languageDefault.DGRID_HD_Value;
			dgridRecipeData.Columns[3].HeaderText = Language.languageDefault.DGRID_HD_Unit;
        }
		#endregion

		#endregion

		private void dgridRecipeData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex != 2)			return;

			if (e.RowIndex == (int)DEVICE.TraySizeChkSen1Use  ||
				e.RowIndex == (int)DEVICE.TraySizeChkSen3Use  ||
				e.RowIndex == (int)DEVICE.TraySizeChkSen4Use)
			{
				btnTraySizeSenSel.PerformClick();
			}
		}

	}
}
