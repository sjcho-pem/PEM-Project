using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public partial class frmScreenLock : Form
	{
		private	int		m_npassword	= -1;
		private	Form	m_frm		= new Form();

		public	frmScreenLock(Form Parent)
		{
			InitializeComponent();

			this.Location			= Parent.Location;
			this.Size				= Parent.Size;

			m_frm.Location			= Parent.Location;
			m_frm.Size				= Parent.Size;
			m_frm.ShowInTaskbar		= false;
			m_frm.BackColor			= Color.Red;
			m_frm.Opacity			= 0.2;
			m_frm.FormBorderStyle	= FormBorderStyle.None;
			m_frm.TopMost			= true;

			Label	lb				= new Label();
			lb.Text					= "SCREEN LOCKED";
			lb.Font					= new Font("Segoe UI Symbol", 180);
			lb.Dock					= DockStyle.Fill;
			lb.TextAlign			= ContentAlignment.MiddleCenter;

			m_frm.Controls.Add(lb);

			m_frm.Show();
			this.BringToFront();
		}



		private void frmScreenLock_Load(object sender, EventArgs e)
		{
			try
			{
				Label		lb	= new Label();
				frmKeyPad	pw	= new frmKeyPad(lb, "", KeyPadType.unsigned);

				if (pw.ShowDialog() == DialogResult.OK)
				{
					m_npassword	= int.Parse(lb.Text);
				}
				else
				{
					m_frm.Close();
					this.Close();
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				if (m_npassword > -1)
				{
					Label		lb	= new Label();
					frmKeyPad	pw	= new frmKeyPad(lb, "", KeyPadType.unsigned);

					if (pw.ShowDialog() == DialogResult.OK)
					{
						if (m_npassword	== int.Parse(lb.Text))
						{
							m_frm.Close();
							this.Close();
						}
					}
				}
				else
				{
					m_frm.Close();
					this.Close();
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
