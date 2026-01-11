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
	public delegate void SelsectMessageResultHandler(string strResultButton);

    public partial class frmSelectMessageBox : Form
    {
		public	enum	teButtons
		{
			OKCancel,
			OK,
			AutoClose,
			None,
			OKCancelNotClose,
		}

		public	event	SelsectMessageResultHandler	EventMessageResult;

		private	PStopwatch	m_pswBtnDown		= new PStopwatch();


		protected override void WndProc(ref Message m)
		{
			switch(m.Msg)
			{
				case 0x00A3: return; //더블 클릭 시 최대화 막기
				case 0x84:
					base.WndProc(ref m);
					if ((int)m.Result == 0x1)
						m.Result = (IntPtr)0x2;
					return;
			}

			base.WndProc(ref m);
		}

        public frmSelectMessageBox(List<string> ltButtonNames, string strTitle, string strMsg, int nBtnDownDelayMs, bool bNotClose = false)
        {
            InitializeComponent();

			m_DlgRst	= "";

            this.Location = new Point(960 - (this.Size.Width / 2), 540 - (this.Size.Height / 2));

            lbX_Title.Text = strTitle;
            lbx_Msg.Text = strMsg;

			foreach (string str in ltButtonNames)
			{
				Button btn	= new Button();

				btn.Name		= str;
				btn.Size		= new Size(fpnButtons.Width/ltButtonNames.Count, fpnButtons.Height);
				btn.Margin		= new Padding(0);
				btn.Text		= str;
				btn.Font		= new Font("맑은 고딕", 12, FontStyle.Bold);

				if (nBtnDownDelayMs > 0)
				{
					btn.MouseDown	+= btn_MouseDown;
					btn.MouseUp		+= btn_MouseUp;
				}
				else
				{
					progBtnDown.Visible	= false;

					btn.Click += btn_Click;
				}
				fpnButtons.Controls.Add(btn);
			}

			m_bNotClose	= bNotClose;

			progBtnDown.Minimum	= 0;
			progBtnDown.Value	= 0;
			progBtnDown.Maximum	= nBtnDownDelayMs;
			m_pswBtnDown.Reset();

            SetLanguageText();
        }

		#region [ Variables ]
		private	bool	m_bNotClose	= false;
		private	string	m_DlgRst	= "";
		public	string	Result	{ get => m_DlgRst; }

        #endregion

        #region [ Event Function ]
		private void frmSelectMessageBox_Load(object sender, EventArgs e)
		{
			if (progBtnDown.Maximum > 0)
			{
				tmr_ProgressState.Start();
			}
		}

        private void frmSelectMessageBox_Paint(object sender, PaintEventArgs e)
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
		private void tmr_ProgressState_Tick(object sender, EventArgs e)
		{
			progBtnDown.Value	= (int)m_pswBtnDown.ElapsedMilliseconds;
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
		private void btn_Click(object sender, EventArgs e)
		{
			Button btn	= (Button)sender;

			m_DlgRst	= btn.Text;
			if (EventMessageResult != null)			EventMessageResult(m_DlgRst);

			if (m_bNotClose == false)			Close();
		}

		private void btn_MouseUp(object sender, MouseEventArgs e)
		{
			if (m_pswBtnDown.ElapsedMilliseconds < progBtnDown.Maximum)
			{
				m_pswBtnDown.Stop();
				m_pswBtnDown.Reset();
				return;
			}
			else
			{
				Button btn	= (Button)sender;

				m_DlgRst	= btn.Text;
				if (EventMessageResult != null)			EventMessageResult(m_DlgRst);

				if (m_bNotClose == false)			Close();
			}

		}

		private void btn_MouseDown(object sender, MouseEventArgs e)
		{
			if (m_pswBtnDown.IsRunning == false)
			{
				m_pswBtnDown.Restart();
			}
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

        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {

        }
		#endregion

		#endregion
	}
}
