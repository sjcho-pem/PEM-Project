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
	public partial class frmListBoxSelect : Form
	{
		private	List<string>	m_ltList	= new List<string>();
		private	string	m_str_sel		= "";
		private	string	m_str_sel_old	= "";
		
		public	string	SelItem { get => m_str_sel;	}

		public	int		SelIndex { get => m_ltList.IndexOf(m_str_sel);	}
		
		private	List<string>	m_ltflow_list	= new List<string>();


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

		public					frmListBoxSelect()
		{
			InitializeComponent();
		}



		public	DialogResult	ShowDialog(List<string> ltList, string strOldSel = "")
		{
			try
			{
				if (ltList.Count == 0)			return DialogResult.None;

				m_ltList		= ltList;
				if (strOldSel == "")			m_str_sel_old	= ltList[0];
				else							m_str_sel_old	= strOldSel;

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		public	DialogResult	ShowDialog(List<string> ltList, int nOldIndex = 0)
		{
			try
			{
				if (ltList.Count == 0)			return DialogResult.None;

				m_ltList		= ltList;
				if (nOldIndex >= ltList.Count)			m_str_sel_old	= ltList[0];
				else									m_str_sel_old	= ltList[nOldIndex];

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		private	void			frmListBoxSelect_Load(object sender, EventArgs e)
		{
			try
			{
				SetLanguageText();
				Init();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void			btn_Click(object sender, EventArgs e)
		{
			try
			{
				Button			btn	= (Button)sender;

				switch (btn.Name)
				{
					case "btnOK"	:
						this.DialogResult	= DialogResult.OK;
						
						if (lbSelect.SelectedItem != null)
						{
							m_str_sel	= lbSelect.SelectedItem.ToString();
						}
						
						Close();
						break;

					case "btnCancel"	:
						this.DialogResult	= DialogResult.Cancel;

						m_str_sel	= m_str_sel_old;

						Close();
						break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void			lbSelect_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			try
			{
				this.DialogResult	= DialogResult.OK;

				m_str_sel	= lbSelect.SelectedItem.ToString();
						
				Close();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void			Init()
		{
			try
			{
				if (m_ltList.Count > 5)
				{
					int nheight	= (m_ltList.Count - 5) * 25;

					this.Height		+= nheight;
					lbSelect.Height	+= nheight;
				}

				lbSelect.Items.Clear();
				
				foreach (string str in m_ltList)
				{
					lbSelect.Items.Add(str);
				}

				lbSelect.SelectedItem	= m_str_sel_old;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void			SetLanguageText()
		{
			try
			{
				btnOK.Text		= Language.languageDefault.MB_OK;
				btnCancel.Text	= Language.languageDefault.MB_Cancel;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
