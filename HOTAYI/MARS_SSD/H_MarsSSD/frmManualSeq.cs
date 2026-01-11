using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.ComponentModel;
using System.Windows.Threading;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;

namespace H_MarsSSD
{
	public partial class frmManualSeq : Form
	{
		private struct	tsBlinkInfo
		{
			public	bool		bBlink;
			public	Control		Ctrl;

			public tsBlinkInfo(bool _bBlink, Control _Ctrl)
			{
				bBlink	= _bBlink;
				Ctrl	= _Ctrl;
			}
		}

		#region [ Variables ]
		private	TableLayoutPanel[]	m_panels;
		#endregion

		//TableLayoutPanel이 한번에 갱신되게 하기 위해 추가
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
				return cp;
			}
		}

		public			frmManualSeq()
		{
			InitializeComponent();
			Initialize();
		}

		private void	Initialize()
		{
			for (SeqID i = (SeqID)0; i < SeqID.Max; i++)
			{
				TableLayoutPanel temptlp = new TableLayoutPanel();
			   
				switch ((SeqID)i)
				{
					case SeqID.LOADER_HOME:
					case SeqID.LOADER_LOAD:
					case SeqID.LOADER_UNLOAD:
					case SeqID.LOADER_DOWN:
					case SeqID.LOADER_UP:
						temptlp = tlpLOADER;
						break;
					case SeqID.TRAYCLEAN_HOME:
					case SeqID.TRAYCLEAN_CLEAN:
					case SeqID.TRAYCLEAN_GRIP:
					case SeqID.TRAYCLEAN_RELEASE:
						temptlp = tlpTRAYCLEAN;
						break;
					case SeqID.LDTRANS_HOME:
					case SeqID.LDTRANS_PICK:
					case SeqID.LDTRANS_PICKCONV:
					case SeqID.LDTRANS_PLACE:
					case SeqID.LDTRANS_PLACECONV:
					case SeqID.LDTRANS_PNP:
						temptlp = tlpLDTRANS;
						break;
					case SeqID.LOADTABLE1_HOME:
					case SeqID.LOADTABLE1_UP:
					case SeqID.LOADTABLE1_DOWN:
					case SeqID.LOADTABLE1_GRIP:
					case SeqID.LOADTABLE1_RELEASE:
						temptlp = tlpLOADTABLE1;
						break;
					case SeqID.LOADTABLE2_HOME:
					case SeqID.LOADTABLE2_UP:
					case SeqID.LOADTABLE2_DOWN:
					case SeqID.LOADTABLE2_GRIP:
					case SeqID.LOADTABLE2_RELEASE:
						temptlp = tlpLOADTABLE2;
						break;
					case SeqID.LOADPNP_HOME:
					case SeqID.LOADPNP_PICKTRAY:
					case SeqID.LOADPNP_PLACEBUFFER:
					case SeqID.LOADPNP_PNP:
					case SeqID.LOADPNP_EMPTYCHECK:
						temptlp = tlpLOADPNP;
						break;
					case SeqID.LDBUFFER1_HOME:
					case SeqID.LDBUFFER1_LOADWAIT:
					case SeqID.LDBUFFER1_CLEAN:
					case SeqID.LDBUFFER1_UNLOADWAIT:
						temptlp = tlpLDBUFFER1;
						break;
					case SeqID.LDBUFFER2_HOME:
					case SeqID.LDBUFFER2_LOADWAIT:
					case SeqID.LDBUFFER2_CLEAN:
					case SeqID.LDBUFFER2_UNLOADWAIT:
						temptlp = tlpLDBUFFER2;
						break;
					case SeqID.LDINSPPNP_HOME:
					case SeqID.LDINSPPNP_PICK:
					case SeqID.LDINSPPNP_PLACE:
					case SeqID.LDINSPPNP_PNP:
						temptlp = tlpLDINSPPNP;
						break;
					case SeqID.INSPPLATE1_HOME:
					case SeqID.INSPPLATE1_LOADWAIT:
					case SeqID.INSPPLATE1_UNLOADWAIT:
					case SeqID.INSPPLATE1_INSPWAIT:
						temptlp = tlpINSPPLATE1;
						break;
					case SeqID.INSPPLATE2_HOME:
					case SeqID.INSPPLATE2_LOADWAIT:
					case SeqID.INSPPLATE2_UNLOADWAIT:
					case SeqID.INSPPLATE2_INSPWAIT:
						temptlp = tlpINSPPLATE2;
						break;
					case SeqID.ULDINSPPNP_HOME:
					case SeqID.ULDINSPPNP_PICK:
					case SeqID.ULDINSPPNP_PLACE:
					case SeqID.ULDINSPPNP_PNP:
						temptlp = tlpULDINSPPNP;
						break;
					case SeqID.ULDBUFFER_HOME:
					case SeqID.ULDBUFFER_LOADWAIT:
					case SeqID.ULDBUFFER_UNLOADWAIT:
						temptlp = tlpULDBUFFER;
						break;
					case SeqID.REJECTBUFFER_HOME:
					case SeqID.REJECTBUFFER_READY:
					case SeqID.REJECTBUFFER_CHANGE:
						temptlp = tlpREJECTBUFFER;
						break;
					case SeqID.UNLOADPNP_HOME:
					case SeqID.UNLOADPNP_PICKBUFFER:
					case SeqID.UNLOADPNP_PICKREJECT:
					case SeqID.UNLOADPNP_PLACE:
					case SeqID.UNLOADPNP_PNP:
						temptlp = tlpUNLOADPNP;
						break;
					case SeqID.UNLOADTABLE1_HOME:
					case SeqID.UNLOADTABLE1_UP:
					case SeqID.UNLOADTABLE1_DOWN:
					case SeqID.UNLOADTABLE1_GRIP:
					case SeqID.UNLOADTABLE1_RELEASE:
						temptlp = tlpUNLOADTABLE1;
						break;
					case SeqID.UNLOADTABLE2_HOME:
					case SeqID.UNLOADTABLE2_UP:
					case SeqID.UNLOADTABLE2_DOWN:
					case SeqID.UNLOADTABLE2_GRIP:
					case SeqID.UNLOADTABLE2_RELEASE:
						temptlp = tlpUNLOADTABLE2;
						break;
					case SeqID.ULDTRANS_HOME:
					case SeqID.ULDTRANS_PICK:
					case SeqID.ULDTRANS_PLACE:
					case SeqID.ULDTRANS_PLACECONV:
					case SeqID.ULDTRANS_PNP:
						temptlp = tlpULDTRANS;
						break;
					case SeqID.UNLOADER_HOME:
					case SeqID.UNLOADER_UNLOAD:
					case SeqID.UNLOADER_LIFTUP:
					case SeqID.UNLOADER_LIFTDOWN:
					case SeqID.UNLOADER_ALIGN:
						temptlp = tlpUNLOADER;
						break;
					default:
						continue;
				}

				Label	lb	= new Label();
				Button	btn = new Button();

				ButtonInit(btn, i);
				LabelInit(lb, i);

				int n = temptlp.Controls.Count;
				temptlp.Controls.Add(lb,	n/2, 0);
				temptlp.Controls.Add(btn,	n/2, 1);
			}
			
			m_panels	= new TableLayoutPanel[]
			{
				tlpLOADER,
				tlpTRAYCLEAN,
				tlpLDTRANS,
				tlpLOADTABLE1,
				tlpLOADTABLE2,
				tlpLOADPNP,
				tlpLDBUFFER1,
				tlpLDBUFFER2,
				tlpULDBUFFER,
				tlpLDINSPPNP,
				tlpINSPPLATE1,
				tlpINSPPLATE2,
				tlpULDINSPPNP,
				tlpULDBUFFER,
				tlpREJECTBUFFER,
				tlpUNLOADPNP,
				tlpUNLOADTABLE1,
				tlpUNLOADTABLE2,
				tlpULDTRANS,
				tlpUNLOADER,
			};
		}

		#region [ Variables ]
		private	CBlinkTimer		m_tmrBlink		= new CBlinkTimer();
		#endregion

		private void	ButtonInit(Button btn, SeqID seq)
		{
			btn.Anchor		= ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			btn.BackColor	= System.Drawing.Color.White;
			btn.Font		= new System.Drawing.Font("Segoe UI", 8F);

			string[] str = EnumHelper.GetDescription(seq).Split('/');
			if (str.Length == 2)			btn.Text = str[1];
			else							btn.Text = EnumHelper.GetDescription(seq);

			btn.Tag			= (int)seq;
			btn.FlatStyle	= FlatStyle.Flat;
			btn.Dock		= DockStyle.Fill;
			btn.TextAlign	= ContentAlignment.MiddleCenter;

			btn.Click		+= Btn_ClickEvent;
		}

		private void	LabelInit(Label lb, SeqID seq)
		{
			lb.Anchor		= ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			lb.Font			= new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
			lb.BackColor	= System.Drawing.Color.White;
			lb.Text			= "";
			lb.TextAlign	= System.Drawing.ContentAlignment.MiddleCenter;
			lb.Margin		= new System.Windows.Forms.Padding(3);
			lb.Dock			= DockStyle.Fill;
			lb.BorderStyle	= BorderStyle.None;
			lb.Tag			= (int)seq;
		}

		private void	Btn_ClickEvent(object sender, EventArgs e)
		{
			Button				btn				= sender as Button;
			frmPnPSelect		pnpBox			= new frmPnPSelect();
			frmListBoxSelect	listBox			= new frmListBoxSelect();
			string[]			strlist			= { "BUFFER1", "BUFFER2"}; 
			List<string>		ltlist			= strlist.ToList();
			string				msg				= "";
			MANUAL				manual			= MANUAL.NotManual;


			if (Enum.TryParse(((SeqID)btn.Tag).ToString(), out manual) == false)
			{
				MessageBox.Show(string.Format("매뉴얼 Enum에 시퀀스 Enum과 같은 이름이 없습니다. 두개의 이름을 동일하게 맞춰 주십시요.\nSeq:{0}", ((SeqID)btn.Tag).ToString()));
				return;
			}

			switch ((SeqID)btn.Tag)
			{
				case SeqID.LDTRANS_PLACE:
				case SeqID.LDTRANS_PNP:
				case SeqID.LDINSPPNP_PICK:
				case SeqID.LDINSPPNP_PLACE:
				case SeqID.LDINSPPNP_PNP:
				case SeqID.ULDINSPPNP_PICK:
				case SeqID.ULDINSPPNP_PLACE:
				case SeqID.ULDINSPPNP_PNP:
				case SeqID.ULDTRANS_PICK:
				case SeqID.ULDTRANS_PLACE:
				case SeqID.ULDTRANS_PNP:
					if (pnpBox.ShowDialog((SeqID)btn.Tag) == DialogResult.Cancel)
						return;

					if		((int)SM.SHM.GetManualParam(0) == -1)			msg = string.Format("Do you want to Manual Act?\r\n{0} \r\n Place : {1}", manual.ToString(), pnpBox.SelectedPlace);
					else if ((int)SM.SHM.GetManualParam(1) == -1)			msg = string.Format("Do you want to Manual Act?\r\n{0} \r\n Pick : {1}", manual.ToString(), pnpBox.SelectedPick);
					else													msg = string.Format("Do you want to Manual Act?\r\n{0} \r\n Pick : {1} , Place : {2}", manual.ToString(), pnpBox.SelectedPick,pnpBox.SelectedPlace);
					
					break;
				case SeqID.LOADPNP_EMPTYCHECK:
					{
						if (listBox.ShowDialog(new List<string> { "Table1", "Table2" }, btn.Text) == DialogResult.Cancel)			return;
						if (string.IsNullOrWhiteSpace(listBox.SelItem) == true)			return;

						msg	= string.Format("do you want to Manual Act?\r\n({0})", manual.ToString());

						if (listBox.SelItem == "Table1")			SM.SHM.SetManualParams(new object[] { 0 });
						else										SM.SHM.SetManualParams(new object[] { 1 });
					}
					break;
				default:
					{
						msg = string.Format("Do you want to Manual Act?\r\n({0})", manual.ToString());
					}
					break;
			}

			frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation,msg );
			if (pMsg.ShowDialog() == DialogResult.Cancel)			return;
			
			UI.Logs.Write(LOGPART.TK, string.Format("{0} Click", manual.ToString()));
			SM.SHM.SetManualNum(manual);
		}
		
		#region [ Event Function ]
	 
		#endregion

		#region [ Timmer Function ]
		private void	tmrUIUpdate_Tick(object sender, EventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)			SetControlEnable(this, false);
				else												SetControlEnable(this, true);

				foreach (TableLayoutPanel panel in m_panels)
				{
					for (int i = 0; i < panel.Controls.Count; i++)
					{
						if		(panel.Controls[i].GetType() == typeof(Button))
						{
							SetControlColor(panel.Controls[i], SM.SHM.GetReadyState((SeqID)panel.Controls[i].Tag), Color.Black);
						}
						else if (panel.Controls[i].GetType() == typeof(Label))
						{
							SetControlText(panel.Controls[i], SM.SHM.GetCurStep((SeqID)panel.Controls[i].Tag));
						}
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region [ Button Event ]
	
		#endregion

		#region [ Function ]

		#region - Private Function
		private void	UIUpdate()
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)			SetControlEnable(this, false);
				else												SetControlEnable(this, true);

				foreach (TableLayoutPanel panel in m_panels)
				{
					for (int i = 0; i < panel.Controls.Count; i++)
					{
						if		(panel.Controls[i].GetType() == typeof(Button))
						{
							SetControlColor(panel.Controls[i], SM.SHM.GetReadyState((SeqID)panel.Controls[i].Tag), Color.Black);
						}
						else if (panel.Controls[i].GetType() == typeof(Label))
						{
							SetControlText(panel.Controls[i], SM.SHM.GetCurStep((SeqID)panel.Controls[i].Tag));
						}
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetControlEnable(Control Ctrl, bool bEnable)
		{
			try
			{
				if (Ctrl.Enabled != bEnable)			Ctrl.Enabled = bEnable;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetControlColor(Control Ctrl, Color BackColor, Color ForeColor)
		{
			try
			{
				if (Ctrl.BackColor != BackColor)			Ctrl.BackColor = BackColor;
				if (Ctrl.ForeColor != ForeColor)			Ctrl.ForeColor = ForeColor;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetControlText(Control Ctrl, string strText)
		{
			try
			{
				Ctrl.Text	= strText;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		#endregion

		#region - Public Function

		#endregion

		#region - Language Function 

		#endregion

		#endregion

		private void	frmManualSeq_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
				if (tmrUIUpdate.Enabled == false)				tmrUIUpdate.Start();
				SM.SHM.StartManual();
			}
			else
			{
				if (tmrUIUpdate.Enabled == true)				tmrUIUpdate.Stop();
				SM.SHM.StopManual();
			}
		}
	}
}
