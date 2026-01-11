using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmManage : Form
    {

        #region [ Variables ]
        private XML xmlMng = new XML();
        private int nSelectRow;     //dGridMember에서 선택한 Index
        private int nLogInRow;      //로그인 Index
        private int nSearchRow;     //Search Index
        private string strDBLogInCol0 = "ID";
        private string strDBLogInCol1 = "PW";
        private string strDBLogInCol2 = "LEVEL";
        private string strDBLogInCol3 = "LOGIN";
        private string strDBLogInCol4 = "LOGOUT";
        private string strDBLogInCol5 = "RUNNINGTIME";
        #endregion

        #region Initialize
        public frmManage()
        {
            InitializeComponent();
            InitialXML();
            UpdateManangeGrid();
        }

        private void InitialXML()
        {
            xmlMng.Initializ("Member");
        }
        #endregion

        #region [ Event ]

        private void dGridMember_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Ctrl+F
            if (e.KeyChar == (char)(Keys.XButton2))
                this.ActiveControl = txtSearch;
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                FindNextID();
        }

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
                nSearchRow = 0;
        }

        private void cbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strSelectLevel = cbSort.SelectedItem.ToString();

            if (strSelectLevel == "ALL")                  SortDataGridView(strSelectLevel);
            else if (strSelectLevel == "OPERATOR")        SortDataGridView(strSelectLevel);
            else if (strSelectLevel == "ENGINEER")        SortDataGridView(strSelectLevel);
            else if (strSelectLevel == "PROFESSIONAL")    SortDataGridView(strSelectLevel);
        }

        private void dGridMember_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <= 0)    return;
            nSelectRow = e.RowIndex;
        }

        private void frmManage_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush pBr = new SolidBrush(Color.Black);
            Pen pPen = new Pen(pBr, 1);

            Size _size = this.Size;
            g.DrawRectangle(pPen, 1, 1, _size.Width - 3, _size.Height - 3);

            pBr.Dispose();
            pPen.Dispose();
        }

        #endregion [ Event ] (End)

        #region [ Button Event ]

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dGridMember.Rows.Count <= 0)    return;

            string strDeleteID = dGridMember.Rows[nSelectRow].Cells[0].Value.ToString();

            //삭제할 Member 찾기
            for (int i = 0; i < Define.MAX_MEMBER_NUM; i++)
            {
                if (strDeleteID == xmlMng.ReadDataTable(i, strDBLogInCol0))
                {
                    frmMessageBox msg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, "Delete " + strDeleteID + " ?");
                    if (msg.ShowDialog() == DialogResult.OK)
                    {
                        xmlMng.WriteDataTable(i, strDBLogInCol0, "");
                        xmlMng.WriteDataTable(i, strDBLogInCol1, "");
                        xmlMng.WriteDataTable(i, strDBLogInCol2, "");
                        xmlMng.WriteDataTable(i, strDBLogInCol3, "");
                        xmlMng.WriteDataTable(i, strDBLogInCol4, "");
                        xmlMng.WriteDataTable(i, strDBLogInCol5, "");
                        UpdateManangeGrid();

                        UI.Logs.Write(LOGPART.PW, string.Format("Delete {0} !!", strDeleteID));
                        break;
                    }
                    else
                        return;
                }
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            try
            {
                SortDataGridView("ALL");
                if (dGridMember.Rows.Count < 0)    return;
                if (dGridMember.RowCount == Define.MAX_MEMBER_NUM)
                {
                    MessageBox.Show("Member를 추가할 수 없습니다!", Language.languageDefault.MB_Warnning, MessageBoxButtons.OK);
                    return;
                }

                frmRegist fRegist = new frmRegist();
                if (fRegist.ShowDialog() == DialogResult.OK)
                {
                    //GetRegistInfo() : frmRegist에서 입력한 Info를 받아온다
                    //[0]: 아이디 [1]: 비밀번호, [2]: 레벨
                    string[] arrRegist = fRegist.GetRegistInfo();
                    if (arrRegist == null || arrRegist.Length == 0)
                        return;

                    //Member 등록
                    RegistMember(arrRegist);
                    UI.Logs.Write(LOGPART.PW, string.Format("Regist {0} !!", arrRegist[0]));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (dGridMember.Rows.Count <= 0)    return;

                string strId     = dGridMember.Rows[nSelectRow].Cells[0].Value.ToString();  //아이디
                string strPw     = dGridMember.Rows[nSelectRow].Cells[1].Value.ToString();  //비밀번호
                string strLevel  = dGridMember.Rows[nSelectRow].Cells[2].Value.ToString();  //레벨

                frmEdit fEdit = new frmEdit(strId, strPw, strLevel);
                if (fEdit.ShowDialog() == DialogResult.OK)
                {
                    //GetEditInfo() : frmEdit에서 변경한 Info를 받아온다
                    //[0]: 비밀번호, [1]: 레벨
                    string[] arrEdit = fEdit.GetEditInfo();
                    if(arrEdit == null || arrEdit.Length == 0)
                        return;

                    //Member 편집
                    EditMember(arrEdit);

                    //변경 전과 후를 비교하여 LOG 기록
                    if (strPw != arrEdit[0])
                    {
                        UI.Logs.Write(LOGPART.PW, string.Format("Edit {0} Password !!", strId));
                    }
                    if (strLevel != arrEdit[1])
                    {
                        UI.Logs.Write(LOGPART.PW, string.Format("Edit {0} Level {1} ▶ {2}", strId, strLevel, arrEdit[1]));
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLevelSetting_Click(object sender, EventArgs e)
        {
            UI.ModalLevelSetting.ShowDialog();
        }

        private void btnCancelnSave_Click(object sender, EventArgs e)
        {
            SaveXMLFile();
            this.Close();
        }

        #endregion [ Button Event ] (End)

        #region [ Method ]

        #region Public Function

        public void SaveXMLFile()
        {
            try
            {
                //XML 파일 업데이트 & 저장
                xmlMng.UpdateEncryptData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool CheckDuplicatedID(string strValue)
        {
            //Regist Member : 아이디 중복 방지
            for (int i = 0; i < Define.MAX_MEMBER_NUM; i++)
            {
                if (strValue == xmlMng.ReadDataTable(i, strDBLogInCol0))
                {
                    frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Duplicated ID!");
                    pMsg.ShowDialog();
                    return false;
                }
            }
            return true;
        }

        public bool CheckMember(string strId, string strPw)
        {
            try
            {
                //아이디 & 비밀번호 확인
                for (int i = 0; i < Define.MAX_MEMBER_NUM; i++)
                {
                    if (strId == xmlMng.ReadDataTable(i, strDBLogInCol0) &&
                        strPw == xmlMng.ReadDataTable(i, strDBLogInCol1))
                    {
                        // 로그인한 Member의 행 저장
                        nLogInRow = i;
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        public string ReadXMLData(string strColName)
        {
            try
            {
                //현재 로그인 한 Member의 값(PW, LEVEL) 리턴
                return xmlMng.ReadDataTable(nLogInRow, strColName);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
		
        public string ReadXMLData(int nIdx, string strColName)
        {
            try
            {
                //현재 로그인 한 Member의 값(PW, LEVEL) 리턴
                return xmlMng.ReadDataTable(nIdx, strColName);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void WriteLogTime(string strColName, string strValue)
        {
            try
            {
                //로그 기록
                xmlMng.WriteDataTable(nLogInRow, strColName, strValue);
                UpdateManangeGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Private Function

        private List<string> m_ltMember = new List<string>();
        private void UpdateManangeGrid()
        {
            try
            {
                // 데이터그리드뷰 업데이트
                this.dGridMember.Rows.Clear();
                m_ltMember.Clear();
                for (int i = 0; i < Define.MAX_MEMBER_NUM; i++)
                {
                    string strId = xmlMng.ReadDataTable(i, strDBLogInCol0);
                    if (strId == "")    continue;

                    m_ltMember.Add(xmlMng.ReadDataTable(i, strDBLogInCol0));
                    this.dGridMember.Rows.Add(
                    xmlMng.ReadDataTable(i, strDBLogInCol0),
                    xmlMng.ReadDataTable(i, strDBLogInCol1),
                    xmlMng.ReadDataTable(i, strDBLogInCol2),
                    xmlMng.ReadDataTable(i, strDBLogInCol3),
                    xmlMng.ReadDataTable(i, strDBLogInCol4),
                    xmlMng.ReadDataTable(i, strDBLogInCol5)
                    );
                }
                if(dGridMember.CurrentCell != null)
                    nSelectRow = dGridMember.CurrentCell.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RegistMember(string[] strArr)
        {
            try
            {
                for (int i = 0; i < Define.MAX_MEMBER_NUM; i++)
                {
                    //XML의 비어있는 DataTable에 등록
                    string strId = xmlMng.ReadDataTable(i, strDBLogInCol0);
                    if (strId == "")
                    {
                        xmlMng.WriteDataTable(i, strDBLogInCol0, strArr[0]);
                        xmlMng.WriteDataTable(i, strDBLogInCol1, strArr[1]);
                        xmlMng.WriteDataTable(i, strDBLogInCol2, strArr[2]);
                        break;
                    }
                }
                UpdateManangeGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EditMember(string[] arr)
        {
            try
            {
                //Member 편집
                xmlMng.WriteDataTable(nSelectRow, strDBLogInCol1, arr[0]);
                xmlMng.WriteDataTable(nSelectRow, strDBLogInCol2, arr[1]);
                UpdateManangeGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SortDataGridView(string strLevel)
        {
            //데이터그리드뷰 필터링
            if (strLevel != "ALL")
            {
                foreach (DataGridViewRow row in dGridMember.Rows)
                {
                    string strCellValue = row.Cells[2].Value.ToString();

                    if (strLevel != strCellValue)
                        row.Visible = false;
                    else
                        row.Visible = true;
                }
            }
            else
            {
                UpdateManangeGrid();
                cbSort.SelectedIndex = 0;
            }
        }

        private void FindNextID()
        {
            SortDataGridView("ALL");
            if (txtSearch.Text == "" || txtSearch.Text.Contains(" "))
                return;
            Regex reg = new Regex(txtSearch.Text, RegexOptions.IgnoreCase);
            Match m;
            string[] dataes = { "" };

            for (int i = nSearchRow; i < m_ltMember.Count; i++)
            {
                m = reg.Match(m_ltMember[i]);
                if (m.Success)
                {
                    dGridMember.CurrentCell = dGridMember[0, i];
                    nSelectRow = i;
                    nSearchRow = i + 1;
                    return;
                }
            }
        }
        #endregion

        #endregion [ Method ] (End)
    }
}
