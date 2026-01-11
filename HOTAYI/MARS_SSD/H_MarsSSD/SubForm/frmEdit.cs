using System;
using System.Drawing;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmEdit : Form
    {
        #region [ Initialize ]
        public frmEdit(string strId, string strPw, string strLevel)
        {
            InitializeComponent();
            lbId.Text = strId;
            txtEditPw.Text = strPw;
            cbEditLevel.SelectedItem = strLevel;            
        }
        #endregion

        #region [ Event ]
        private void frmEdit_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtEditPw;
            txtEditPw.UseSystemPasswordChar = true;
            picVisiblePwNotUse.Image = Properties.Resources.VisiblePassword2_24p;
        }

        private void picVisiblePwNotUse_Click(object sender, EventArgs e)
        {
            txtEditPw.UseSystemPasswordChar = !txtEditPw.UseSystemPasswordChar;
            if (txtEditPw.UseSystemPasswordChar == false) 
                picVisiblePwNotUse.Image = Properties.Resources.VisiblePassword1_24p;
            else
                picVisiblePwNotUse.Image = Properties.Resources.VisiblePassword2_24p;
        }

        private void frmEdit_Paint(object sender, PaintEventArgs e)
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
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                //빈 문자열 검사
                if (txtEditPw.Text == "")
                {
                    lbPw.ForeColor = Color.Red;
                    txtEditPw.Focus();
                    frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Enter your Member info!");
                    pMsg.ShowDialog();
                    return;
                }
                else
                {
                    lbPw.ForeColor = Color.Black;
                }
                //공백 검사
                if (txtEditPw.Text.Contains(" "))
                {
                    lbPw.ForeColor = Color.Red;
                    txtEditPw.Focus();
                    frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "WhiteSpace is not allowed!");
                    pMsg.ShowDialog();
                    return;
                }
                else
                {
                    lbPw.ForeColor = Color.Black;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                UI.Logs.Write(LOGPART.PW, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        private void btnKeyboardPw_Click(object sender, EventArgs e)
        {
            if (UI.ModalKeyBoard.ShowDialog() == DialogResult.OK)
                txtEditPw.Text = UI.ModalKeyBoard.Value;
            else
                return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion [ Button Event ] (End)

        #region [ Method ]
        public string[] GetEditInfo()
        {
            string[] strArray = new string[2];
            strArray[0] = txtEditPw.Text;
            strArray[1] = cbEditLevel.SelectedItem.ToString();
            return strArray;
        }
        #endregion [ Method ] (End)
    }
}
