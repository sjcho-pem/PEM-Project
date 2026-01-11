using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public delegate void MessageResultHandler(DialogResult DialogResult);

    public partial class frmMessageBox : Form
    {
		public	enum	teButtons
		{
			OKCancel,
			OK,
			AutoClose,
			None,
			OKCancelNotClose,
		}

		public	event	MessageResultHandler	EventMessageResult;

		protected override void WndProc(ref Message m)
		{
			switch(m.Msg)
			{
				case 0x00A3: return; //더블 클릭 시 최대화 막기
				case 0x84:			 //컨트롤내에서 동작하는게 테두리에서 동작하는 것과 동일하게 동작하도록 수정 -> 폼의 드래그 이동이 되도록
					base.WndProc(ref m);
					if ((int)m.Result == 0x1)
						m.Result = (IntPtr)0x2;
					return;
			}

			base.WndProc(ref m);
		}

		public frmMessageBox(bool bSelect, string strTitle, string strMsg)
        {
            InitializeComponent();

            this.Location = new Point(960 - (this.Size.Width / 2), 540 - (this.Size.Height / 2));
	
			Init(bSelect, strTitle, strMsg);
        }

        public frmMessageBox(teButtons Buttons, string strTitle, string strMsg)
        {
            InitializeComponent();

            this.Location = new Point(960 - (this.Size.Width / 2), 540 - (this.Size.Height / 2));
			
			Init(Buttons, strTitle, strMsg);
        }

        #region [ Variables ]
		private	bool	m_bNotClose	= false;
        private	bool	m_tmflag	= false;
        private	int		m_nCloseCnt	= 0;
		private	DialogResult	m_DlgRst	= DialogResult.None;
		public	DialogResult	Result	{ get => m_DlgRst; }

        #endregion

        #region [ Event Function ]
        private void frmMessageBox_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            SolidBrush pBr = new SolidBrush(Color.White);
            Pen pPen = new Pen(pBr, 3);

            Size _size = this.Size;
            g.DrawRectangle(pPen, 0, 0, _size.Width, _size.Height);

            pBr.Dispose();
            pPen.Dispose();
        }
        #endregion

        #region [ Timmer Function ]

        private void tm_ProgressState_Tick(object sender, EventArgs e)
        {
            if (m_tmflag == true)
            {
                if (m_nCloseCnt != 3)
                {
                    m_nCloseCnt++;
                    return;
                }

                tm_ProgressState.Stop();
                DlgClose();
            }
            else if (pgBar_Timer.Value == pgBar_Timer.Maximum)
            {
                pgBar_Timer.Value = pgBar_Timer.Maximum;
                m_tmflag = true;
            }
            else
            {
                pgBar_Timer.PerformStep();
            }
        }
        private void tm_ActiveState_Tick(object sender, EventArgs e)
		{
			if (!this.Focused)
			{
				this.TopMost = false;
				this.TopMost = true;
				this.Activate();
			}
		}
        #endregion

        #region [ Label Function ]

        private void lb_Quitbtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lbx_Quitbtn_MouseHover(object sender, EventArgs e)
        {
            lbx_Quitbtn.BackColor = SystemColors.ControlDarkDark;
        }

        private void lbx_Quitbtn_MouseLeave(object sender, EventArgs e)
        {
            lbx_Quitbtn.BackColor = SystemColors.ControlDarkDark;
        }
        #endregion

        #region [ Button Function ]
        private void btn_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult	= DialogResult.OK;
			m_DlgRst			= DialogResult.OK;

			if (EventMessageResult != null)			EventMessageResult(DialogResult.OK);
			if (m_bNotClose == false)				Close();
        }

        private void btn_OK2_Click(object sender, EventArgs e)
        {
            this.DialogResult	= DialogResult.OK;
			m_DlgRst			= DialogResult.OK;

			if (EventMessageResult != null)			EventMessageResult(DialogResult.OK);
			Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult	= DialogResult.Cancel;
			m_DlgRst			= DialogResult.Cancel;
			if (EventMessageResult != null)			EventMessageResult(DialogResult.Cancel);
            if (m_bNotClose == false)				Close();
        }
        #endregion

        #region [ Function ]

        #region - Private Function

        private void DlgClose()
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region - Public Function
		public	void	Init(bool bSelect, string strTitle, string strMsg)
        {
			if (lbX_Title.Text == strTitle  &&  lbx_Msg.Text == strMsg)			return;

			m_DlgRst	= DialogResult.None;

            lbX_Title.Text = strTitle;
            lbx_Msg.Text = strMsg;

            pgBar_Timer.Minimum = 0;
            pgBar_Timer.Maximum = 500;
            pgBar_Timer.Step = 20;

			tm_ActiveState.Interval = 500;
            tm_ActiveState.Start();

            if (bSelect)
            {// OK, Cancel 버튼으로 진행
                this.BackColor = Color.SkyBlue;
                lbX_Title.ForeColor = Color.Black;
                lbx_Quitbtn.Visible = true;
                btn_OK.Visible = true;
                btn_OK2.Visible = false;
                btn_Cancel.Visible = true;
                pgBar_Timer.Visible = false;
                btn_Cancel.Select();
            }
            else
            {//자동 Close 
                this.BackColor = SystemColors.GradientActiveCaption;
                lbX_Title.ForeColor = Color.Firebrick;
                lbx_Quitbtn.Visible = false;
                btn_OK.Visible = false;
                btn_OK2.Visible = false;
                btn_Cancel.Visible = false;
                pgBar_Timer.Visible = true;
                btn_Cancel.Select();
                tm_ProgressState.Start();
            }

            SetLanguageText();
        }

		public	void	Init(teButtons Buttons, string strTitle, string strMsg)
		{
			if (lbX_Title.Text == strTitle  &&  lbx_Msg.Text == strMsg)			return;

			m_DlgRst	= DialogResult.None;

            lbX_Title.Text = strTitle;
            lbx_Msg.Text = strMsg;

            pgBar_Timer.Minimum = 0;
            pgBar_Timer.Maximum = 500;
            pgBar_Timer.Step = 20;

            if		(Buttons == teButtons.OKCancel  ||
					 Buttons == teButtons.OKCancelNotClose)
            {// OK, Cancel 버튼으로 진행
                this.BackColor = SystemColors.ControlDark;
                lbX_Title.ForeColor = Color.Black;
                lbx_Quitbtn.Visible = true;
                btn_OK.Visible = true;
                btn_OK2.Visible = false;
                btn_Cancel.Visible = true;
                pgBar_Timer.Visible = false;
                btn_Cancel.Select();
            }
			else if (Buttons == teButtons.OK)
			{
				this.BackColor = SystemColors.ControlDark;
                lbX_Title.ForeColor = Color.Black;
                lbx_Quitbtn.Visible = true;
                btn_OK.Visible = false;
                btn_OK2.Visible = true;
                btn_Cancel.Visible = false;
                pgBar_Timer.Visible = false;
                btn_OK2.Select();
			}
            else if (Buttons == teButtons.AutoClose)
            {//자동 Close 
                this.BackColor = SystemColors.GradientActiveCaption;
                lbX_Title.ForeColor = Color.Firebrick;
                lbx_Quitbtn.Visible = false;
                btn_OK.Visible = false;
                btn_OK2.Visible = false;
                btn_Cancel.Visible = false;
                pgBar_Timer.Visible = true;
                btn_Cancel.Select();
                tm_ProgressState.Start();
            }
			else if (Buttons == teButtons.None)
			{
				this.BackColor = SystemColors.ControlDark;
                lbX_Title.ForeColor = Color.Black;
                lbx_Quitbtn.Visible = false;
                btn_OK.Visible = false;
                btn_OK2.Visible = false;
                btn_Cancel.Visible = false;
                pgBar_Timer.Visible = false;
                btn_OK2.Select();
			}

			if (Buttons == teButtons.OKCancelNotClose)			m_bNotClose = true;
			else												m_bNotClose = false;

            SetLanguageText();
		}
        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {
            btn_OK.Text = Language.languageDefault.MB_OK;
            btn_OK2.Text = Language.languageDefault.MB_OK;
            btn_Cancel.Text = Language.languageDefault.MB_Cancel;
        }
        #endregion
        #endregion
    }
}
