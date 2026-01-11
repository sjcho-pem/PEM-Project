using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmError : Form
    {        
        public frmError()
        {
            InitializeComponent();
            InitialXML();
            InitialGrid();
        }

        #region [ Variables ]
        private XML XMLError = new XML();
        private string strErrorCol0 = "ErrorDataName";
        private string strErrorCol1Kor = "ErrorDataDescription";
        private string strErrorCol1Cha = "ErrorDataDescriptionCha";
        private string strErrorCol1Eng = "ErrorDataDescriptionEng";
        private string strErrorColBuzzer = "ErrorBuzzerType";
        private string strErrorColImagePath = "ErrorImagePath";
        public uint[] uiErrCode = new uint[SHMDEF.ERRCODE_BUF_NUM];
		public List<uint> ltErrCode = new List<uint>();

        private bool bTmrRunning = false;
        #endregion

        #region [ Event Function ]
        private void frmData_Error_Load(object sender, EventArgs e)
        {
            // Grid 선택 이벤트 값 적용
            cbBuzzer.SelectedIndex = Convert.ToInt32(XMLError.ReadDataTable(0, strErrorColBuzzer));

            #region -Get Error Image path DataBase
            SetDisplayFromGetErrorImgPathDB(0);
            #endregion
        }
        #endregion

        #region [ Event Timer ]
        private void tmrError_Tick(object sender, EventArgs e)
        {
            bTmrRunning = true;

            SM.SHM.GetErrCodeData(ref uiErrCode);

            if (uiErrCode[0] > Define.MAX_ERROR_NUM)
                uiErrCode[0] = (uint)Define.MAX_ERROR_NUM;

            UI.ErrorRecipeDisp.DisplayErrorCode(uiErrCode[0], GetErrorName((int)uiErrCode[0]));
			

            bTmrRunning = false;
        }
        #endregion

        #region [ CheckBox Event ]

        private void chkErrorMod_CheckedChanged(object sender, EventArgs e)
        {
            if (chkErrorMod.Checked)
            {
                chkErrorMod.ForeColor = Color.Red;
                this.tbColumnErrorName.ReadOnly = false;
                this.tbColumnErrorDescription.ReadOnly = false;
                cbBuzzer.Enabled = true;

                btnErrorUpdate.Enabled = true;
                btnErrorClear.Enabled = true;
                lbBuzzerSelect.Enabled = true;
                btnOpenFileImage.Enabled = true;
				btnDelete.Enabled = true;
            }
            else
            {
                chkErrorMod.ForeColor = Properties.Settings.Default._DarkOliveGreen;
                this.tbColumnErrorName.ReadOnly = true;
                this.tbColumnErrorDescription.ReadOnly = true;
                cbBuzzer.Enabled = false;

                btnErrorUpdate.Enabled = false;
                btnErrorClear.Enabled = false;
                lbBuzzerSelect.Enabled = false;
                btnOpenFileImage.Enabled = false;
				btnDelete.Enabled = false;

                XMLError.UpdateData();
            }
        }
        #endregion

        #region [ Event Button ]

        private void btnErrorClear_Click(object sender, EventArgs e)
        {
            tmrError.Stop();

            ClearDatatable();

            tmrError.Start();
        }

        private void btnErrorUpdate_Click(object sender, EventArgs e)
        {
            if (bTmrRunning) return;

            tmrError.Enabled = false;

			try
			{
				for (int i = 0; i < (int)(ALM.TOTAL -1); i++)
				{
					int nIdx = i + 1 ;

					XMLError.WriteDataTable(i, strErrorCol0, ((ALM)nIdx).ToString());

					if (Program.m_Languageidx == (int)LAN_MODE.CHA)
						XMLError.WriteDataTable(i, strErrorCol1Cha, GetEnumDescription(nIdx, (int)LAN_MODE.CHA));
					else if (Program.m_Languageidx == (int)LAN_MODE.ENG)
						XMLError.WriteDataTable(i, strErrorCol1Eng, GetEnumDescription(nIdx, (int)LAN_MODE.ENG));
					else
						XMLError.WriteDataTable(i, strErrorCol1Kor, GetEnumDescription(nIdx, (int)LAN_MODE.KOR));

					
					string AlarmPath = @"D:\DATA(HOTAYI_MARS_SSD)\ErrorImage\" + string.Format($"{nIdx}_{((ALM)(nIdx)).ToString()}.png") ;
					if (File.Exists(AlarmPath) == true)	  XMLError.WriteDataTable(i, strErrorColImagePath, AlarmPath);
                  
				}

				XMLError.UpdateData();
                UpdateErrorGrid();
            }
            catch (SystemException ex)
            {
                UI.Logs.Write(LOGPART.EX, ex.Message);
            }

            tmrError.Enabled = true;
        }
        #endregion

        #region [ Grid Setting ]

        #region -Function
        private void InitialGrid()
        {
            for (int i = 0; i < Define.MAX_ERROR_NUM; i++)
            {
                string strErr = XMLError.ReadDataTable(i, strErrorCol0);
                string strDesc = "";

                if (Program.m_Languageidx == (int)LAN_MODE.CHA)
                    strDesc = XMLError.ReadDataTable(i, strErrorCol1Cha);
                else if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                    strDesc = XMLError.ReadDataTable(i, strErrorCol1Eng);
                else
                    strDesc = XMLError.ReadDataTable(i, strErrorCol1Kor);

                this.dgridError.Rows.Add(i + 1, strErr, strDesc);
            }

            this.dgridError.RowTemplate.Height = 35;
        }

        #endregion

        #region -Event        
        private void dgridError_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!chkErrorMod.Checked) return;
            if (e.RowIndex == -1) return;

            string strNewValue = "";
            if (this.dgridError[e.ColumnIndex, e.RowIndex].FormattedValue.ToString() == null)
                strNewValue = "";
            else
                strNewValue = this.dgridError[e.ColumnIndex, e.RowIndex].FormattedValue.ToString();

            if (e.ColumnIndex == 1)
                XMLError.WriteDataTable(e.RowIndex, strErrorCol0, strNewValue);
            else
            {
                if (Program.m_Languageidx == (int)LAN_MODE.CHA)
                    XMLError.WriteDataTable(e.RowIndex, strErrorCol1Cha, strNewValue);
                else if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                    XMLError.WriteDataTable(e.RowIndex, strErrorCol1Eng, strNewValue);
                else
                    XMLError.WriteDataTable(e.RowIndex, strErrorCol1Kor, strNewValue);
            }
        }
        #endregion

        #endregion

        #region [ Buzzer ]

        #region -Variables
        private int nNowColIdx = 0;
        private int nNowRowIdx = 0;
        #endregion

        #region -Function
        public void InitBuzzerData()
        {
            for (int row = 0; row < SHMDEF.BUZZER_ROW_SIZE; row++)
            {
                int val = Convert.ToInt32(XMLError.ReadDataTable(row, strErrorColBuzzer));
                SM.SHM.SetBuzzerData(row, val);
            }
        }
        #endregion

        #region -Event Function
        private void dgridError_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1) return;

            #region --Gets the associated Column and Row index for the cell.
            nNowColIdx = e.ColumnIndex;
            nNowRowIdx = e.RowIndex;
            #endregion

            #region --Gets Database number in ComboBox Setting.
            cbBuzzer.SelectedIndex = Convert.ToInt32(XMLError.ReadDataTable(nNowRowIdx, strErrorColBuzzer));
            #endregion

            #region --Set Display to Image and path.
            SetDisplayFromGetErrorImgPathDB(nNowRowIdx);
            #endregion
        }

		private void dgridError_SelectionChanged(object sender, EventArgs e)
		{
			if (dgridError.SelectedRows == null  ||  dgridError.SelectedRows.Count == 0)			return;

			nNowRowIdx	= dgridError.SelectedRows[0].Index;

			#region --Gets Database number in ComboBox Setting.
            cbBuzzer.SelectedIndex = Convert.ToInt32(XMLError.ReadDataTable(nNowRowIdx, strErrorColBuzzer));
            #endregion

            #region --Set Display to Image and path.
            SetDisplayFromGetErrorImgPathDB(nNowRowIdx);
            #endregion
		}

        private void cbBuzzer_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region --Buzzer Write DB
            SM.SHM.SetBuzzerData(nNowRowIdx, cbBuzzer.SelectedIndex);
            XMLError.WriteDataTable(nNowRowIdx, strErrorColBuzzer, cbBuzzer.SelectedIndex.ToString());
            #endregion            
        }

		private void picErrorImage_DoubleClick(object sender, EventArgs e)
		{
			if (File.Exists(GetNowErrImgFilePath()) == false)
			{
				frmMessageBox msg	= new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "No File");
				msg.Show();
				return;
			}

			Form fc = Application.OpenForms["frmImageViewer"];
			if (fc != null)
			{
				((frmImageViewer)fc).SetImage(GetNowErrImgFilePath());
				return;
			}

			frmImageViewer	viewer	= new frmImageViewer(GetNowErrImgFilePath());
			viewer.Show();
		}

        #endregion

        #endregion

        #region [ Error Image]
        

        #region -Function
        private void SetImageFilePathDisp(string path)
        {
            txtImageFilePath.Text = path;
        }

        private string GetNowErrImgFilePath()
        {
            return txtImageFilePath.Text;
        }

        private void SetErrorImage(string path)
        {
            try
            {
                if (!string.IsNullOrEmpty(path))
                {
                    if (File.Exists(path))
                    {
                        Image img = Image.FromFile(path);
                        picErrorImage.Image = img;

						Form fc = Application.OpenForms["frmImageViewer"];
						if (fc != null)
						{
							((frmImageViewer)fc).SetImage(path);
							return;
						}
                    }
                    else
                    {
                        picErrorImage.Image = null;
                    }
                }
                else
                {
                    picErrorImage.Image = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void SetDisplayFromGetErrorImgPathDB(int index)
        {
            #region -Get Error Image path DataBase
            string imgPath = XMLError.ReadDataTable(index, strErrorColImagePath);
            SetImageFilePathDisp(imgPath);
            SetErrorImage(imgPath);
            #endregion
        }

        public string GetErrorImagePath(int index)
        {
            if (index == 0)
                return string.Empty;

            return XMLError.ReadDataTable(index - 1, strErrorColImagePath);
        }
        #endregion

        #region -Button Event
        private void btnOpenFileImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            try
            {
                openFileDialog.DefaultExt = "*.*";
                openFileDialog.InitialDirectory = ".";
                openFileDialog.Filter = "JPG files (*.jpg)|*.jpg|ALL files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string path = openFileDialog.FileName;
                    SetImageFilePathDisp(path);
                    SetErrorImage(path);

                    #region --Error Image Write DB
                    XMLError.WriteDataTable(nNowRowIdx, strErrorColImagePath, path);
                    #endregion
                }
            }
            catch (SystemException ex)
            {
                UI.Logs.Write(LOGPART.EX, ex.Message);
            }
            openFileDialog.Dispose();
        }

		private void btnDelete_Click(object sender, EventArgs e)
		{
            try
            {
				frmMessageBox	msg	= new frmMessageBox(frmMessageBox.teButtons.OKCancel, "", "DELETE?");
                if (msg.ShowDialog() == DialogResult.OK)
                {
                    string path = "";
                    SetImageFilePathDisp(path);
                    SetErrorImage(path);

                    #region --Error Image Write DB
                    XMLError.WriteDataTable(nNowRowIdx, strErrorColImagePath, path);
                    #endregion
                }
            }
            catch (SystemException ex)
            {
                UI.Logs.Write(LOGPART.EX, ex.Message);
            }
		}
        #endregion

        #endregion

        #region [ Function ]

        #region - Private Function
        private void InitialXML()
        {
            XMLError.Initializ("ErrorData");
			InitBuzzerData();
        }

        private string[] ParsingDescription(string str)
        {
            //언어 추가시 반드시 초기화 추가
            string[] seperator = new string[]{ "<en>", "<ko>", "<cn>" };
            string[] strDescription = new string[seperator.Length];
            int[] startIdx = new int[seperator.Length];
            int nLangIdx = 0;                        
            

            //구분자에 따른 문자열 인덱스 탐색
            for (int i = 0; i < seperator.Length; i++)
            {
                startIdx[i] = str.IndexOf(seperator[i]);
            }

            for (int i = 0; i < seperator.Length; i++)
            {
                if (startIdx[i] > -1) //비어있지 않을때
                {
                    nLangIdx = -1;
                    for (int j = 0; j < seperator.Length; j++)
                    {
                        //현재 문자열보다 더 뒤의 문자열 탐색
                        if (startIdx[j] > 0 && startIdx[j] > startIdx[i])
                        {
                            if (nLangIdx == -1)
                            {
                                nLangIdx = j;
                            }
                            else
                            {//그중 현재 문자열과 가장 가까운 문자열 탐색
                                if (startIdx[j] < startIdx[nLangIdx])
                                {
                                    nLangIdx = j;
                                }
                            }
                            break;
                        }
                    }

                    if (nLangIdx > -1) //다음 구분자가 존재하는 경우
                        strDescription[i] = str.Substring(startIdx[i] + 4, startIdx[nLangIdx] - startIdx[i] - 4);
                    else //다음이 없는 경우
                        strDescription[i] = str.Substring(startIdx[i] + 4);
                }
            }
            return strDescription;

        }

        private string GetEnumDescription(int ErrNum, int LangMode)
        {
            try
            {
                if (ErrNum == 0)
                    return "NORMAL STATUS";
                else
                {

                    string temp = EnumHelper.GetDescription((ALM)ErrNum);
                    string[] strDescription = ParsingDescription(temp);

                    if (string.IsNullOrEmpty( strDescription[LangMode]) == false)
                    {
                        temp = strDescription[LangMode];
                    }
                    else
                    {//언어에 맞는 구분자는 없지만 그외 구분자가 하나라도 있다면 해당하는 첫번째문자열 리턴
                        for (int i = 0; i < strDescription.Length; i++)
                        {
                            if (string.IsNullOrEmpty(strDescription[i]) == false)
                            {
                                temp = strDescription[i];
                                break;
                            }
                        }
                    }
                    
                    return temp;
                }
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
                return "Error";
            }
        }
        #endregion

        #region - Public Function
        public int GetErrorCode()
        {
            return (int)uiErrCode[0];
        }

        public int GetErrorCode(int priority)
        {
            if (priority > 9)
                priority = 9;
            if (priority < 0)
                priority = 0;

            return (int)uiErrCode[priority];
        }

        public string GetErrorName(int ErrNum)
        {
            if (ErrNum == 0)
                return "NORMAL STATUS";
            else
                return (XMLError.ReadDataTable(ErrNum - 1, strErrorCol0));
        }
        public string GetErrorDescription(int ErrNum)
        {
            if (ErrNum == 0)
                return "NORMAL STATUS";
            else
            {
                if (Program.m_Languageidx == (int)LAN_MODE.CHA)
                    return (XMLError.ReadDataTable(ErrNum - 1, strErrorCol1Cha));
                if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                    return (XMLError.ReadDataTable(ErrNum - 1, strErrorCol1Eng));
                else
                    return (XMLError.ReadDataTable(ErrNum - 1, strErrorCol1Kor));
            }
        }

        public bool IsWorking()
        {
            if (chkErrorMod.Checked)
                return true;
            return false;
        }

        public void WorkEnd()
        {
            if (chkErrorMod.Checked)
                chkErrorMod.Checked = false;
        }

        public void DisableWork()
        {
            if (chkErrorMod.Enabled)
                chkErrorMod.Enabled = false;
        }

        public void EnableWork()
        {
            if (!chkErrorMod.Enabled)
                chkErrorMod.Enabled = true;
        }

		public void UpdateCheckBox(PW_MODE Password_now)
        {            
            chkErrorMod.Enabled = true;
        }

        public void ClearDatatable()
        {
            frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Error, Language.languageDefault.MB_ErrClear);

            if (pMsg.ShowDialog() == DialogResult.Cancel) return;

            for (int i = 0; i < Define.MAX_ERROR_NUM; i++)
            {
                //Alarm Name..
                XMLError.WriteDataTable(i, strErrorCol0, "");
                //Alarm Desciption..
                if (Program.m_Languageidx == (int)LAN_MODE.CHA)
                    XMLError.WriteDataTable(i, strErrorCol1Cha, "");
                if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                    XMLError.WriteDataTable(i, strErrorCol1Eng, "");
                else
                    XMLError.WriteDataTable(i, strErrorCol1Kor, "");
            }
            XMLError.UpdateData();
            UpdateErrorGrid();
        }

        public void UpdateErrorGrid()
        {
            this.dgridError.Rows.Clear();

            for (int i = 0; i < Define.MAX_ERROR_NUM; i++)
            {
                string strErr = XMLError.ReadDataTable(i, strErrorCol0);
                string strDesc = "";

                if (Program.m_Languageidx == (int)LAN_MODE.CHA)
                    strDesc = XMLError.ReadDataTable(i, strErrorCol1Cha);
                else if (Program.m_Languageidx == (int)LAN_MODE.ENG)
                    strDesc = XMLError.ReadDataTable(i, strErrorCol1Eng);
                else
                    strDesc = XMLError.ReadDataTable(i, strErrorCol1Kor);

                this.dgridError.Rows.Add(i + 1, strErr, strDesc);
            }

			dgridError.Columns[0].HeaderText = Language.languageDefault.DGRID_HD_No;
			dgridError.Columns[1].HeaderText = Language.languageDefault.DGRID_HD_Alarm;
			dgridError.Columns[2].HeaderText = Language.languageDefault.DGRID_HD_AlarmDescription;
        }

        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            chkErrorMod.Text = Language.languageDefault.ErrorModify;
            lbBuzzerSelect.Text = "[ " + Language.languageDefault.BuzzerSelect + " ]";
            btnErrorUpdate.Text = Language.languageDefault.ErrorUpdate;
            btnErrorClear.Text = Language.languageDefault.ErrorClear;
            UpdateErrorGrid();
        }
		#endregion

		#endregion

		
	}
}
