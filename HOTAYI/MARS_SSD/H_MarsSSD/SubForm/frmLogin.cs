using System;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmLogin : Form
    {
        #region [ Variable ]

        private bool bLogIn = false;
        public bool IsLogIn
        {
            get { return bLogIn; }
        }

        private string  strLoginTime = "";
        public string LoginTime
        {
            get { return strLoginTime; }
        }
        #endregion [ Variable ] (End)

        #region [ Initialize ]
        public frmLogin()
        {
            InitializeComponent();
            PassLogIn();
        }
        #endregion [ Initialize ] (End)

        #region [ Event ]
        private void frmLogin_Load(object sender, EventArgs e)
        {
			txtPw.Text = "";
			lbWarning.Visible = false;
			txtPw.UseSystemPasswordChar = true;

			cbID.Items.Clear();
			for (int i = 0; i < Define.MAX_MEMBER_NUM; i++)
			{
				string strID = UI.ModalManage.ReadXMLData(i , "ID");

				if (string.IsNullOrEmpty(strID))		continue;

				cbID.Items.Add(strID);
			}
			cbID.SelectedIndex = 0;

            picVisiblePwNotUse.Image = Properties.Resources.VisiblePassword2_24p;

            this.ActiveControl = cbID;
        }

        private void picVisiblePwNotUse_Click(object sender, EventArgs e)
        {
            txtPw.UseSystemPasswordChar = !txtPw.UseSystemPasswordChar;
            if (txtPw.UseSystemPasswordChar == false) 
                picVisiblePwNotUse.Image = Properties.Resources.VisiblePassword1_24p;
            else
                picVisiblePwNotUse.Image = Properties.Resources.VisiblePassword2_24p;
        }

        private void frmLogin_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush pBr = new SolidBrush(Color.Black);
            Pen pPen = new Pen(pBr, 1);

            Size _size = this.Size;
            g.DrawRectangle(pPen, 1, 1, _size.Width - 3, _size.Height - 3);

            pBr.Dispose();
            pPen.Dispose();
        }

        private void txtPw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        #endregion [ Event ] (End)

        #region [ Button Event ]
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {	
                if (VerifyMember(cbID.Text , txtPw.Text))
                {
					G.commInspTop.SendUserChangeRequest(cbID.Text, (int)nowPWMode);
					G.commInspBtm.SendUserChangeRequest(cbID.Text, (int)nowPWMode);

                    UI.Logs.Write(LOGPART.PW, string.Format("Log In !! "));
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    lbWarning.Visible = true;
                }
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.PW, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        private void btnKeyboardPW_Click(object sender, EventArgs e)
        {
            if (UI.ModalKeyBoard.ShowDialog() == DialogResult.OK)
                txtPw.Text = UI.ModalKeyBoard.Value;
            else
                return;
        }

        private void btnKeyboardID_Click(object sender, EventArgs e)
        {
           // if (UI.ModalKeyBoard.ShowDialog() == DialogResult.OK)
           //     txtId.Text = UI.ModalKeyBoard.Value;
           // else
           //     return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion [ Button Event ] (End)

        #region [ Method ]
        public void LogOut()
        {
            try
            {
                //RUNNINGTIME 계산
                string strLogoutTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                TimeSpan ts = DateTime.Parse(strLogoutTime) - DateTime.Parse(strLoginTime);
                string strRunningTime = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);

                if (bLogIn == true && (nowPWMode != PW_MODE.ADMIN))
                {
                    //Member만
                    UI.ModalManage.WriteLogTime("LOGOUT", strLogoutTime);
                    UI.ModalManage.WriteLogTime("RUNNINGTIME", strRunningTime);
                }
                UI.Logs.Write(LOGPART.PW, string.Format("Log Out !! (RUN TIME : {0})", strRunningTime));
                bLogIn = false;
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.PW, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        private bool VerifyMember(string strId, string strPw)
        {
            //아이디, 비밀번호 확인
            try
            {
                if (strId == "" || strPw == "")  return false;

                strLoginTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                if (strId == "admin" && strPw == "pemtron1004")
                {
                    //어드민 확인
                    bLogIn = true;
                    nowPWMode = PW_MODE.ADMIN;
                }
                else
                {
                    //Member 확인
                    if (UI.ModalManage.CheckMember(strId, strPw) == false)  return false;
                    
                    bLogIn = true;
                    if (UI.ModalManage.ReadXMLData("LEVEL") == "OPERATOR")
                        nowPWMode = PW_MODE.OPER;
                    if (UI.ModalManage.ReadXMLData("LEVEL") == "ENGINEER")
                        nowPWMode = PW_MODE.ENG;
                    if (UI.ModalManage.ReadXMLData("LEVEL") == "PROFESSIONAL")
                        nowPWMode = PW_MODE.PRO;

                    UI.ModalManage.WriteLogTime("LOGIN", strLoginTime);
                }
				UI.strMemberID = strId;

                return true;
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.PW, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
                return false;
            }
        }

        private PW_MODE nowPWMode
        {
            set
            {
                if (Program.m_PasswordMode != value)
                {
                    UI.PageMotor.UpdateCheckBox(value);
                    UI.PageError.UpdateCheckBox(value);
                    UI.SubpageBit.UpdateCheckBox(value);
                    UI.SubpageDm.UpdateCheckBox(value);
                    UI.SubpageUseskip.UpdateCheckBox(value);
                    UI.PageTenKey.UpdateCheckBox(value);
                    UI.SubpageRecipe.UpdateCheckBox(value);
                    UI.PageMotor.UpdateButton(value);
					UI.PageData.UpdatePageVisible(value);
					UI.PageManual.UpdatePageVisible(value);

                }
                Program.m_PasswordMode = value;
            }
            get
            {
                return Program.m_PasswordMode;
            }
        }

        private void PassLogIn()
        {
            //개발용 자동 로그인 기능
            string strId = SDm.Read(eSDM.ID);
            string strPw = SDm.Read(eSDM.PW);

            if (string.IsNullOrWhiteSpace(strId) || string.IsNullOrWhiteSpace(strPw))
                return;

            VerifyMember(strId, strPw);
        }

        #endregion [ Method ] (End)
    }
}