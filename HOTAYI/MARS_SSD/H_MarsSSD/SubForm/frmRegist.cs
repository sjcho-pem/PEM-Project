using System;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmRegist : Form
    {

        #region [ Initialize ]
        public frmRegist()
        {
            InitializeComponent();
        }
        #endregion

        #region [ Event ]
        private void frmRegist_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtRegId;

            //텍스트박스, 라벨 초기화
            txtRegId.Text = "";
            txtRegPw.Text = "";
            txtCheckPw.Text = "";
            cbRegLevel.SelectedIndex = -1;

            txtRegPw.UseSystemPasswordChar = true;
            txtCheckPw.UseSystemPasswordChar = true;
            picVisiblePwNotUse1.Image = Properties.Resources.VisiblePassword2_24p;
            picVisiblePwNotUse2.Image = Properties.Resources.VisiblePassword2_24p;
        }

        private void picVisiblePw_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;

            switch (pic.Name)
            {

                case "picVisiblePwNotUse1":
                    txtRegPw.UseSystemPasswordChar = !txtRegPw.UseSystemPasswordChar;                    
                    
                    if (txtRegPw.UseSystemPasswordChar == false) 
                        picVisiblePwNotUse1.Image = Properties.Resources.VisiblePassword1_24p;
                    else
                        picVisiblePwNotUse1.Image = Properties.Resources.VisiblePassword2_24p;
                    break;

                case "picVisiblePwNotUse2":
                    txtCheckPw.UseSystemPasswordChar = !txtCheckPw.UseSystemPasswordChar;

                    if (txtCheckPw.UseSystemPasswordChar == false) 
                        picVisiblePwNotUse2.Image = Properties.Resources.VisiblePassword1_24p;
                    else
                        picVisiblePwNotUse2.Image = Properties.Resources.VisiblePassword2_24p;
                    break;
            }
        }

        private void btnKeyboard_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (UI.ModalKeyBoard.ShowDialog() == DialogResult.OK)
            {
                switch (btn.Name)
                {
                    case "btnKeyboardID":
                        txtRegId.Text = UI.ModalKeyBoard.Value;
                        break;
                    case "btnKeyboardPw1":
                        txtRegPw.Text = UI.ModalKeyBoard.Value;
                        break;
                    case "btnKeyboardPw2":
                        txtCheckPw.Text = UI.ModalKeyBoard.Value;
                        break;
                }
            }
            else
                return;
        }

        private void frmRegist_Paint(object sender, PaintEventArgs e)
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
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckMemberInput() == false)     return;

                //아이디 중복 검사
                if (UI.ModalManage.CheckDuplicatedID(txtRegId.Text))
                    this.DialogResult = DialogResult.OK;
                else
                    return;
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.PW, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion [ Button Event ] (End)


        #region [ Method ]

        public string[] GetRegistInfo()
        {
            string[] strArray = new string[3];
            strArray[0] = txtRegId.Text;
            strArray[1] = txtRegPw.Text;
            strArray[2] = cbRegLevel.SelectedItem.ToString();
            return strArray;
        }

        private bool CheckMemberInput()
        {
            //빈 문자열 검사
            if ((txtRegId.Text == "") || (txtRegPw.Text == "") || (txtCheckPw.Text == "") || cbRegLevel.SelectedItem == null)
            {
                if (cbRegLevel.SelectedItem == null)
                {
                    lbLevel.ForeColor = Color.Red;
                    cbRegLevel.Focus();
                }
                else
                {
                    lbLevel.ForeColor = Color.Black;
                }
                if ((txtCheckPw.Text == ""))
                {
                    lbCheckPw.ForeColor = Color.Red;
                    txtCheckPw.Focus();
                }
                else
                {
                    lbCheckPw.ForeColor = Color.Black;
                }
                if ((txtRegPw.Text == ""))
                {
                    lbPw.ForeColor = Color.Red;
                    txtRegPw.Focus();
                }
                else
                {
                    lbPw.ForeColor = Color.Black;
                }
                if ((txtRegId.Text == ""))
                {
                    lbId.ForeColor = Color.Red;
                    txtRegId.Focus();
                }
                else
                {
                    lbId.ForeColor = Color.Black;
                }
                frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Enter your Member info!");
                pMsg.ShowDialog();
                return false;
            }
            //비밀번호 불일치 검사
            if (txtRegPw.Text != txtCheckPw.Text)
            {
                txtRegPw.ForeColor = Color.Red;
                txtCheckPw.ForeColor = Color.Red;
                frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Password does not match!");
                pMsg.ShowDialog();
                return false;
            }
            //공백 문자 검사
            if (txtRegId.Text.Contains(" ") || txtRegPw.Text.Contains(" ") || txtCheckPw.Text.Contains(" "))
            {
                frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "WhiteSpace is not allowed!");
                pMsg.ShowDialog();
                return false;
            }
            return true;
        }

        #endregion [ Method ] (End)
    }
}
