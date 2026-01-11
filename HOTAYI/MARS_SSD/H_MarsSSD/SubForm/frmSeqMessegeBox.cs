using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public partial class frmSeqMessageBox : Form
    {
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

        public delegate void btnEtcClickEventHandler();
        public event btnEtcClickEventHandler btnEtcClick;
        protected virtual void btnEtcClickEvent()
        {
            if (btnEtcClick != null)
                btnEtcClick();
        }


		#region [ Enum , Struct ]

		private	enum	teButtons
		{
			OKCancel,
			OK,
			AutoClose,
			None,
			OKCancelNotClose,
		}

		private	struct	stMsgBoxInfo
		{
			public	DateTime		dtVisibleTime;
			public	bool			bVisible;
			public	string			strTitle;
			public	string			strMsg;
			public	List<string>	ltButtonNames;
			public	string			strResult;
		}

		#endregion
	
		#region [ Variables ]
		
		private PStopwatch	m_pswBtnDown		= new PStopwatch();
		//private EnumArray <teSeqMsg, stMsgBoxInfo> m_stMsgBoxInfo = new EnumArray<teSeqMsg, stMsgBoxInfo>(teSeqMsg.Max);
		private ConcurrentDictionary  <SeqMsg, stMsgBoxInfo> m_stMsgBoxInfo = new ConcurrentDictionary<SeqMsg, stMsgBoxInfo>();
		
		private	bool		m_bNotClose	= false;
		private	object		lockObj		= new object();
		private	SeqMsg	CurMsg		= SeqMsg.Unknown;
		private	bool		bVisibleChange;
		#endregion

		public frmSeqMessageBox()
		{
			InitializeComponent();
			CenterToScreen();
			
			tmSeqMsg.Enabled = true;
			tmSeqMsg.Interval = 100;
			tmSeqMsg.Tick += Tmr_Tick;
			tmSeqMsg.Start();
			m_pswBtnDown.Reset();
			
			lstBoxMsg.DrawMode = DrawMode.OwnerDrawFixed;

			SetLanguageText();
		}


        #region [ Event Function ]
		
		private void frmSeqMessageBox_VisibleChanged(object sender, EventArgs e)
		{
			SelectedMsgChange();
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
			//progBtnDown.Value	= (int)m_pswBtnDown.ElapsedMilliseconds;
		}

        private void Tmr_Tick(object sender, EventArgs e)
        {	
			Dictionary <SeqMsg, stMsgBoxInfo> m_TempMsgBoxInfo ;
			lock (lockObj)
			{
				//그냥 사용 시 for문 사용중에 array값에 변화가 생기면 exception 발생하여 얕은 복사 사용
				m_TempMsgBoxInfo = new Dictionary<SeqMsg, stMsgBoxInfo>(m_stMsgBoxInfo) ;
			}

			bool bVisible = false;
			int nIdx = 0;
			foreach (var item in m_TempMsgBoxInfo)
			{
				if (m_TempMsgBoxInfo[item.Key].bVisible == true)
				{
					bVisible = true;
					break;
				}
				nIdx ++;
			}
			this. Visible = bVisible;
			if (this. Visible == true && bVisibleChange == true)		
			{
				bVisibleChange= false;
				LoadListBox();
			}
			
		}
        #endregion

        #region [ Label Function ]

        private void lb_Quitbtn_Click(object sender, EventArgs e)
        {
			Close(CurMsg);
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
			
			if (btn.Text == "END LOT")		NDm.Write(eNDM.AutoRunMode, (int)AutoRunMode.EndLot);
            else if (btn.Text == "QUIT LOT")
			{
				if (SM.FMT.IsFlow(FlowTray.LDTable1))		SM.FMT[FlowTray.LDTable1].SetAutoRunMode(AutoRunMode.QuitLot);
				if (SM.FMT.IsFlow(FlowTray.LDTable2))		SM.FMT[FlowTray.LDTable2].SetAutoRunMode(AutoRunMode.QuitLot);

				NDm.Write(eNDM.AutoRunMode, (int)AutoRunMode.QuitLot);
			}
			else if (btn.Text == "MOVE TO MANUAL")
			{
				btnEtcClickEvent();
			}

			SetResult(CurMsg, btn.Text);

			if (m_bNotClose == false)			Close(CurMsg);
			if (lstBoxMsg.Items.Count > 0)		lstBoxMsg.SelectedIndex = -1;
		}

		private void btn_MouseUp(object sender, MouseEventArgs e)
		{
			Button btn	= (Button)sender;
			SetResult(CurMsg, btn.Text);
			if (m_bNotClose == false)			Close(CurMsg);
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

		private void	LoadListBox()
		{

			lstBoxMsg.Items.Clear();
			
			Dictionary <SeqMsg, stMsgBoxInfo> m_TempMsgBoxInfo ;
			//그냥 사용 시 for문 사용중에 array값에 변화가 생기면 exception 발생하여 얕은 복사 사용
			lock (lockObj)
			{
				m_TempMsgBoxInfo = new Dictionary<SeqMsg, stMsgBoxInfo>(m_stMsgBoxInfo) ;
			}
			 var sortedList = m_TempMsgBoxInfo.OrderByDescending(kvp => kvp.Value.dtVisibleTime).ToList();

			foreach (var kvp in sortedList)
			{
				if (kvp.Value.bVisible)
				{
					lstBoxMsg.Items.Add(kvp);
					if (lstBoxMsg.SelectedIndex < 0)	lstBoxMsg.SelectedIndex = 0;
				}
			}
		}

		private void	SelectedMsgChange()
		{
			if (CurMsg == SeqMsg.Unknown)			return;
			if (this.Visible == false)				return;

			Dictionary <SeqMsg, stMsgBoxInfo> m_TempMsgBoxInfo ;
			//그냥 사용 시 for문 사용중에 array값에 변화가 생기면 exception 발생하여 얕은 복사 사용
			lock (lockObj)
			{
				m_TempMsgBoxInfo = new Dictionary<SeqMsg, stMsgBoxInfo>(m_stMsgBoxInfo) ;
			}
			
			lbX_Title.Text	= m_TempMsgBoxInfo[CurMsg].strTitle;
			lbx_Msg.Text	= m_TempMsgBoxInfo[CurMsg].strMsg;
			fpnButtons.Controls.Clear();
			for (int i = 0; i < m_TempMsgBoxInfo[CurMsg].ltButtonNames.Count; i++)
			{
				Button btn = new Button();
				string btnName = m_TempMsgBoxInfo[CurMsg].ltButtonNames[i];
				btn.Name = btnName;
				btn.Size = new Size((fpnButtons.Width / m_TempMsgBoxInfo[CurMsg].ltButtonNames.Count) - 10 , fpnButtons.Height);
				btn.Margin = new Padding(5,0,5,0);
				btn.Text = btnName;
				btn.Font = new Font("맑은 고딕", 12, FontStyle.Bold);
				btn.BackColor = Color.LightGray;
				btn.Click += btn_Click;
				
				fpnButtons.Controls.Add(btn);
			}
			this.InitLayout();
		}

		private void lstBoxMsg_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
				return;

			var item = (KeyValuePair<SeqMsg, stMsgBoxInfo>)lstBoxMsg.Items[e.Index];
			e.DrawBackground();

			// dtVisibleTime 폰트 설정
			var dtFont = new Font("Arial", 10, FontStyle.Regular);
			var msgFont = new Font("Arial", 12, FontStyle.Bold);

			// dtVisibleTime 텍스트 그리기
			e.Graphics.DrawString(item.Value.dtVisibleTime.ToString("yyyy-MM-dd HH:mm:ss"), dtFont, Brushes.Black, e.Bounds.Left, e.Bounds.Top);

			// strMsg 텍스트 그리기 , 여러줄인 경우 첫줄만 표기
			string[] lines = item.Value.strMsg.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
			e.Graphics.DrawString(lines[0], msgFont, Brushes.Black, e.Bounds.Left, e.Bounds.Top + 20);

			e.DrawFocusRectangle();
		}

		
		private void SelectedMsgChange(object sender, EventArgs e)
		{
			if (lstBoxMsg.SelectedIndex < 0)
				return;

			var selectedItem = (KeyValuePair<SeqMsg, stMsgBoxInfo>)lstBoxMsg.SelectedItem;
			CurMsg = selectedItem.Key;
			SelectedMsgChange();
		}

		private void	AddInfo(SeqMsg Key)
		{
			
			lock (lockObj)
			{
				switch (Key)
				{
					case SeqMsg.VerifyReqSel:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "Random", "All NG", "All OK" };
							stTemp.strMsg = Language.languageDefault.MB_VerifyRequest;
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.VerifyReq:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { };
							stTemp.strMsg = Language.languageDefault.MB_VerifyRequest;
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.KeyLockErr:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "OK" };
							stTemp.strMsg = Language.languageDefault.MB_KeyLockAuto;
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.RejectNone:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { };
							stTemp.strMsg = "Reject Tray Don't Exist";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.UnloadNone:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { };
							stTemp.strMsg = Language.languageDefault.MB_UnloadTrayNone;
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult =  "Unload Tray Don't Exist";;
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.TraySize:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "QUIT LOT", "CLOSE" };
							stTemp.strMsg = "The tray size on the loader conveyor is different from the recipe.";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.InspPause:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "OK" };
							stTemp.strMsg = Language.languageDefault.MB_PauseFromInspPC;
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.BypassByNG:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "QUIT LOT", "CLOSE"};
							stTemp.strMsg = "Too many NG occurred. Did you Convert to QUIT LOT mode?";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.BypassByContinueNG:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "QUIT LOT", "CLOSE"};
							stTemp.strMsg = "Too many continuous NG occurred.  Did you Convert to QUIT LOT mode?";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.RejectTrayChange:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { };
							stTemp.strMsg = "Please replace the reject tray";
							stTemp.strTitle = Language.languageDefault.MB_Warnning;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.ManualBarcode:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { };
							stTemp.strMsg = Language.languageDefault.MB_VerifyRequest;
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.TrayBarcodeMisMatch:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "QUIT LOT", "MOVE TO MANUAL", "OK" };
							stTemp.strMsg = "";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.TrayBarcodeFail:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "QUIT LOT", "MOVE TO MANUAL", "OK" };
							stTemp.strMsg = "Failed to read the Tray barcode." + Environment.NewLine + "Please Check Tray";
							stTemp.strTitle = Language.languageDefault.MB_Warnning;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;

					case SeqMsg.TrayBarcode_RegexMatchFail:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "QUIT LOT", "MOVE TO MANUAL", "OK" };
							stTemp.strMsg = "";
							stTemp.strTitle = Language.languageDefault.MB_Warnning;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;

					case SeqMsg.LidBarcode_RegexMatchFail:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "QUIT LOT","MOVE TO MANUAL", "CLOSE" };
							stTemp.strMsg = "";
							stTemp.strTitle = Language.languageDefault.MB_Warnning;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;

					case SeqMsg.PCBMisMatch:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "EDIT", "QUIT LOT", "CLOSE" };
							stTemp.strMsg = "Pcb Quantity Information Is Does Not Match The Actual Count, Please Choice an action Edit/ QUIT LOT / Close";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.TrayMisMatch:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> {"QUIT LOT","CLOSE" };
							stTemp.strMsg = "Tray is not Correct. Choice the mode.";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.SGEMConnect:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "RETRY" };
							stTemp.strMsg = "There is an issue with the SECS/GEM communication data, or no response is received. If this alarm continues to occur, please contact an engineer or CS.";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.MESNG:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "OK" };
							stTemp.strMsg = "";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.JOBCHECKNG:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> {"QUIT LOT","CLOSE" };
							stTemp.strMsg = "The job of the inspection program does not exist. Please check.";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.SEQERR:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "OK" };
							stTemp.strMsg = "";
							stTemp.strTitle = Language.languageDefault.MB_Warnning;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.SEQINFO:
						{
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "OK" };
							stTemp.strMsg = "";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
					case SeqMsg.Terminal1:
					case SeqMsg.Terminal2:
					case SeqMsg.Terminal3:
					case SeqMsg.Terminal4:
					case SeqMsg.Terminal5:
						{ 
							stMsgBoxInfo stTemp = new stMsgBoxInfo();
							stTemp.bVisible = false;
							stTemp.ltButtonNames = new List<string> { "OK" };
							stTemp.strMsg = "";
							stTemp.strTitle = Language.languageDefault.MB_Info;
							stTemp.strResult = "";
							m_stMsgBoxInfo.AddOrUpdate(Key, stTemp, (key, oldValue) => stTemp);
						}
						break;
				}
			}
		}

        private void	DlgClose()
        {
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion

        #region - Public Function
		
		public void		Show(SeqMsg Key)
		{
			if (m_stMsgBoxInfo.ContainsKey(Key) == false)		AddInfo(Key);
			stMsgBoxInfo temp = m_stMsgBoxInfo[Key];
			
			if (temp.bVisible == true )		return;
			
			temp.dtVisibleTime = DateTime.Now;
			temp.bVisible = true;
			m_stMsgBoxInfo[Key] = temp;
			bVisibleChange = true;
		}

		public void		Close(SeqMsg Key)
		{
			if (m_stMsgBoxInfo.ContainsKey(Key) == false)		return;

			stMsgBoxInfo temp = m_stMsgBoxInfo[Key];
			
			if (temp.bVisible == false )		return;
			temp.bVisible = false;
			m_stMsgBoxInfo[Key] = temp;
			bVisibleChange = true;
		}
		
		private void	SetResult(SeqMsg Key, string Result)
		{
			if (m_stMsgBoxInfo.ContainsKey(Key) == false)		return;
			stMsgBoxInfo temp = m_stMsgBoxInfo[Key];
			temp.strResult = Result;
			m_stMsgBoxInfo[Key] = temp;
		}

		public string	GetResult(SeqMsg Key)
		{
			if (m_stMsgBoxInfo.ContainsKey(Key) == false)		return "";
			string strResult = m_stMsgBoxInfo[Key].strResult;
			
			stMsgBoxInfo temp = m_stMsgBoxInfo[Key];
			temp.strResult = "";
			m_stMsgBoxInfo[Key] = temp;

			return strResult;
		}
		//message가 가변인 경우  show 호출 전 사용
		public	void	SetMessage(SeqMsg Key, string strMsg)
		{
			if (m_stMsgBoxInfo.ContainsKey(Key) == false)		AddInfo(Key);
			stMsgBoxInfo temp = m_stMsgBoxInfo[Key];
			temp.strMsg = strMsg;
			m_stMsgBoxInfo[Key] = temp;
		}

		//터미널 키에서만 사용
		public int		GetCurTerminalKeyCount()
		{
			return m_stMsgBoxInfo.Keys.Count(Key => Key > SeqMsg.Terminal1);
		}
		
		public bool		IsVisible(SeqMsg Key)
		{
			if (m_stMsgBoxInfo.ContainsKey(Key) == false) return false;

			if (m_stMsgBoxInfo[Key].bVisible == false) return false;

			return true;
		}

		public void		AllClose()
		{
			lock (lockObj)
			{
				m_stMsgBoxInfo.Clear();
				this.Close();
			}
		}
        #endregion

        #region - Language Function 

        public void SetLanguageText()
        {

        }
		#endregion

		#endregion

	}
}
