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
	public partial class frmComboBoxSelect : Form
	{
		public	enum	teType
		{
			True,
			List,
			ComboBox,
		}

		private	teType	m_type			= teType.True;
		private	int		m_nsel_no		= -1;
		private	int		m_nsel_no_old	= 0;
		private	string	m_str_sel_text	= string.Empty;

		private	ListBox	ltbSuggest		= new ListBox();

		
		public	int		SelNo { get => m_nsel_no;	}

		public	string	SelText { get => m_str_sel_text;	}
		
		private	bool		m_bNumbering	= false;
		private	Object[]	m_objComboList	= null;



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

		public	frmComboBoxSelect()
		{
			InitializeComponent();
		}



		public	DialogResult	ShowDialog(teType Type, ComboBoxStyle dropDownStyle, int nOldSelNo = 0)
		{
			try
			{
				m_type			= Type;
				m_nsel_no_old	= nOldSelNo;

				cbFlowMapList.DropDownStyle	= dropDownStyle;

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		public	DialogResult	ShowDialog(teType Type, int nOldSelNo = 0)
		{
			try
			{
				m_type			= Type;
				m_nsel_no_old	= nOldSelNo;

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		public	DialogResult	ShowDialog(object[] objComboList, ComboBoxStyle dropDownStyle, int nOldSelNo = 0, bool bNumbering = false)
		{
			try
			{
				m_type			= teType.List;
				m_nsel_no_old	= nOldSelNo;
				m_objComboList	= objComboList;
				m_bNumbering	= bNumbering;

				cbFlowMapList.DropDownStyle	= dropDownStyle;

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		public	DialogResult	ShowDialog(object[] objComboList, int nOldSelNo = 0, bool bNumbering = false)
		{
			try
			{
				m_type			= teType.List;
				m_nsel_no_old	= nOldSelNo;
				m_objComboList	= objComboList;
				m_bNumbering	= bNumbering;

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		public	DialogResult	ShowDialog(ComboBox cb, ComboBoxStyle dropDownStyle, int nOldSelNo = 0)
		{
			try
			{
				m_type			= teType.ComboBox;
				m_nsel_no_old	= nOldSelNo;

				cbFlowMapList.DropDownStyle	= dropDownStyle;
				cbFlowMapList.Items.AddRange(cb.Items.Cast<Object>().ToArray());

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}

		public	DialogResult	ShowDialog(ComboBox cb, int nOldSelNo = 0)
		{
			try
			{
				m_type			= teType.ComboBox;
				m_nsel_no_old	= nOldSelNo;

				cbFlowMapList.Items.AddRange(cb.Items.Cast<Object>().ToArray());

				this.ShowDialog();

				return this.DialogResult;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DialogResult.Cancel;
			}
		}



		private	void	frmComboBoxSelect_Load(object sender, EventArgs e)
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

		private void	btn_Click(object sender, EventArgs e)
		{
			try
			{
				Button			btn	= (Button)sender;

				switch (btn.Name)
				{
					case "btnOK"	:
						this.DialogResult	= DialogResult.OK;
						
						m_nsel_no		= cbFlowMapList.SelectedIndex;
						m_str_sel_text	= cbFlowMapList.Text;
						
						Close();
						break;

					case "btnCancel"	:
						this.DialogResult	= DialogResult.Cancel;
						Close();
						break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	LtbSuggest_SelectedValueChanged(object sender, EventArgs e)
		{
			try
			{
				cbFlowMapList.Text	= ltbSuggest.SelectedItem.ToString();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void	Init()
		{
			try
			{
				if (m_type != teType.ComboBox)
				{
					cbFlowMapList.Items.Clear();
				}

				switch (m_type)
				{
					case teType.True:
						cbFlowMapList.Items.Add("False");
						cbFlowMapList.Items.Add("True");
						break;

					case teType.List:
						if (m_bNumbering == true)
						{
							for (int i = 0; i < m_objComboList.Length; i++)
							{
								cbFlowMapList.Items.Add(string.Format("{0}. {1}", i, m_objComboList[i]));
							}
						}
						else
						{
							cbFlowMapList.Items.AddRange(m_objComboList);
						}

						break;
				}

				cbFlowMapList.SelectedIndex	= m_nsel_no_old;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void	SetLanguageText()
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

		private void	cbFlowMapList_KeyPress(object sender, KeyPressEventArgs e)
		{
			try
			{
				if (ltbSuggest.Parent != this)
				{
					ltbSuggest.Location			= new Point(cbFlowMapList.Right+5, 4);
					ltbSuggest.Size				= new Size(cbFlowMapList.Size.Width, this.Height);
					ltbSuggest.SelectionMode	= SelectionMode.One;
					ltbSuggest.Font				= cbFlowMapList.Font;
					ltbSuggest.BringToFront();
					ltbSuggest.SelectedValueChanged += LtbSuggest_SelectedValueChanged;

					this.Controls.Add(ltbSuggest);
					this.Width	+= cbFlowMapList.Size.Width;
				}

				tmrSuggest.Stop();
				tmrSuggest.Start();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		

		private void	tmrSuggest_Tick(object sender, EventArgs e)
		{
			tmrSuggest.Stop();

			foreach (string str in cbFlowMapList.Items)
			{
				if (str.ToUpper().Contains(cbFlowMapList.Text.ToUpper()) == true)
				{
					if (ltbSuggest.Items.Contains(str) == false)			ltbSuggest.Items.Add(str);
				}
				else
				{
					if (ltbSuggest.Items.Contains(str) == true)				ltbSuggest.Items.Remove(str);
				}
			}
		}
	}
}