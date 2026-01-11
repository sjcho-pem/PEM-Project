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
	public partial class frmSubManualCyl:Form
	{
		public frmSubManualCyl(Seq seq)
		{
			m_Seq = seq;
			InitializeComponent();
			Initialize();
		}

		#region [ Enum ]

		#endregion

		#region [ Variables ]
		
        private Seq	m_Seq;

		#endregion

		#region [ Event Functions ]

		private void tmrUpdate_Tick(object sender, EventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)		return;

				for (int i = 0; i < (int)Seq.Max; i++)
				{
					Array enumValues = GetEnumArray((Seq)i);
					foreach (object enumValue in enumValues)
					{
						string buttonNameToUpdate = ((Seq)i).ToString() + "_" + enumValue.ToString();
						Control[] foundControls = tlpCyl.Controls.Find(buttonNameToUpdate, true); // 버튼 찾기
						if (foundControls.Length > 0 && foundControls[0] is Button)
						{
							Button foundButton = (Button)foundControls[0];

							if		(enumValue.ToString().Contains("TrayGrip"))		foundButton.Text = GetSenPos("GRIP", "RELEASE", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else if (enumValue.ToString().Contains("Grip"))			foundButton.Text = GetSenPos("RELEASE", "GRIP", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else if (enumValue.ToString().Contains("PnPUpDn"))		foundButton.Text = GetSenPos("DOWN", "UP", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else if (enumValue.ToString().Contains("TableUpDn"))	foundButton.Text = GetSenPos("UP", "DOWN", SM.SHM.GetActPos((Seq)i, (int)enumValue));
							else													foundButton.Text = GetSenPos("FWD", "BWD", SM.SHM.GetActPos((Seq)i, (int)enumValue));
						}
					}
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void chkUseTransparency_CheckedChanged(object sender, EventArgs e)
		{
			if (chkUseTransparency.Checked == true)			this.TransparencyKey = SystemColors.Control;
			if (chkUseTransparency.Checked == false)		this.TransparencyKey = Color.Empty;
		}

		private void tbarOpacity_Scroll(object sender, EventArgs e)
		{
			this.Opacity = ((double)tbarOpacity.Value) / 100;
		}

		#endregion

		#region [ Button Event ]

		private void btn_Click(object sender, EventArgs e)
		{
			try
			{
				Button				btn			= (Button)sender;
				frmListBoxSelect	ltbox		= new frmListBoxSelect();
				SenPos				senPos		= SenPos.UNKNOWN;
				string btnName = btn.Name;

				List<string> ltlist; // 전진,후진 순으로 리스트
				if		(btnName.Contains("TrayGrip"))	ltlist = new List<string> { "Grip", "Release" };
				else if (btnName.Contains("Grip"))		ltlist = new List<string> { "Release", "Grip" };
				else if (btnName.Contains("PnPUpDn"))	ltlist = new List<string> { "Down", "Up" };
				else if (btnName.Contains("TableUpDn"))	ltlist = new List<string> { "UP", "Down" };
				else									ltlist = new List<string> { "Fwd", "Bwd" };

				if (ltbox.ShowDialog(ltlist, btn.Text) == DialogResult.OK)
				{
					if (string.IsNullOrWhiteSpace(ltbox.SelItem) == true)			return;

					if (ltbox.SelItem == ltlist[0])			senPos	= SenPos.FWD;
					else									senPos	= SenPos.BWD;
					
					UI.Logs.Write(LOGPART.TK, string.Format("{0} - {1} Click", btnName, ltbox.SelItem));

					//if (CheckInterference(btn, senPos) == false)			return;

					string[] buttonNameParts = btnName.Split('_');
					Seq seq = (Seq)Enum.Parse(typeof(Seq), buttonNameParts[0]);
					Array enumValues = GetEnumArray(seq);
					int index = Array.IndexOf(enumValues, Enum.Parse(enumValues.GetType().GetElementType(), buttonNameParts[1]));

					SM.SHM.SetManualParams( new object[] {seq, index, senPos});
					SM.SHM.SetManualNum(MANUAL.ManualCyl);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnAll_Click(object sender, EventArgs e)
		{
			try
			{
				Button	btn		= (Button)sender;
				string	btnName	= btn.Name;
				MANUAL	manual	= (MANUAL)Enum.Parse(typeof(MANUAL), btnName);

				 frmMessageBox pMsg = new frmMessageBox(true, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_ManualAct);
				if (pMsg.ShowDialog() == DialogResult.OK)
				{
					SM.SHM.SetManualNum(manual);

					UI.Logs.Write(LOGPART.TK, string.Format("{0} Click", btnName));
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region [ Functions ]

		#region - Public Functions

		#endregion

		#region - Private Functions

		private Array GetEnumArray(Seq Seq)
		{
			switch (Seq)
				{
					case Seq.TrayLoader:			return Enum.GetValues(typeof(SeqTrayLoader.Act));				
					case Seq.TrayClean:			return Enum.GetValues(typeof(SeqTrayClean.Act));			
					case Seq.LoadTransfer:		return Enum.GetValues(typeof(SeqLoadTransfer.Act));		
					case Seq.LoadTrayTable1:		return Enum.GetValues(typeof(SeqLoadTrayTable1.Act));		
					case Seq.LoadTrayTable2:		return Enum.GetValues(typeof(SeqLoadTrayTable1.Act));		
					case Seq.LoadPnP:				return Enum.GetValues(typeof(SeqLoadPnP.Act));			
					case Seq.LoadBuffer1:			return Enum.GetValues(typeof(SeqLoadBuffer.Act));			
					case Seq.LoadBuffer2:			return Enum.GetValues(typeof(SeqLoadBuffer.Act));			
					case Seq.LoadInspPnP:			return Enum.GetValues(typeof(SeqLoadInspPnP.Act));		
					case Seq.InspPlate1:			return Enum.GetValues(typeof(SeqInspPlate.Act));			
					case Seq.InspPlate2:			return Enum.GetValues(typeof(SeqInspPlate.Act));			
					case Seq.UnloadInspPnP:		return Enum.GetValues(typeof(SeqUnloadInspPnP.Act));		
					case Seq.UnloadBuffer:		return Enum.GetValues(typeof(SeqUnloadBuffer.Act));		
					case Seq.RejectBuffer:		return Enum.GetValues(typeof(SeqRejectBuffer.Act));		
					case Seq.UnloadPnP:			return Enum.GetValues(typeof(SeqUnloadPnP.Act));			
					case Seq.UnloadTrayTable1:	return Enum.GetValues(typeof(SeqUnloadTrayTable.Act));	
					case Seq.UnloadTrayTable2:	return Enum.GetValues(typeof(SeqUnloadTrayTable.Act));	
					case Seq.UnloadTransfer:		return Enum.GetValues(typeof(SeqUnloadTransfer.Act));		
					case Seq.Unloader:			return Enum.GetValues(typeof(SeqTrayUnloader.teAct));		
					default:	break;
				}
			return null;
		}

		private void Initialize()
		{
			try 
			{
				this.Text			= string.Format("Sub Manual Cyl_{0}", m_Seq.ToString());
				lbCylPartName.Text	= m_Seq.ToString().ToUpper();
				Array enumValues	= GetEnumArray(m_Seq);

				//개별 버튼 동작
				foreach (object enumValue in enumValues)
				{
					if (enumValue.ToString() == "Max") break;

					int nRow = (int)enumValue / 8;
					int nCol = (int)enumValue - (nRow * 8);

					if ((int)enumValue == 24)		nRow = 2;

					// 각 enum 값에 대한 버튼/라벨 생성
					Label label		= new Label();
					label.Text		= enumValue.ToString();
					label.Dock		= DockStyle.Fill;
					label.Font		= new Font("Segoe UI", 10, FontStyle.Bold);
					label.Margin	= new Padding(1);
					label.TextAlign = ContentAlignment.MiddleCenter;

					Button button = new Button();
					button.Name			= m_Seq.ToString() + "_" + enumValue.ToString();
					button.Dock			= DockStyle.Fill;
					button.Font			= new Font("Segoe UI", 10, FontStyle.Bold);
					button.BackColor	= Color.White;
					button.Margin		= new Padding(1);
					button.TextAlign	= ContentAlignment.MiddleCenter;
					button.Click		+= btn_Click;

					// 특정 위치에 버튼 추가
					tlpCyl.Controls.Add(label, nCol, nRow * 2);
					tlpCyl.Controls.Add(button, nCol, nRow * 2 + 1);
				}

				//ALL 버튼 동작
				string str_seq		= EnumHelper.GetDescription(m_Seq);
				int manual_start	= 0;
				int manual_end		= 0;
				int ncol			= 0;

				if		(str_seq.Contains("LoadPnP") == true)
				{
					manual_start	= (int)MANUAL.LoadPnP_AllUp;
					manual_end		= (int)MANUAL.LoadPnP_AllUpDn;
				}
				else if (str_seq.Contains("LoadInspPnP") == true)
				{
					manual_start	= (int)MANUAL.LoadInspPnP_AllUp;
					manual_end		= (int)MANUAL.LoadInspPnP_AllUpDn;
				}
				else if (str_seq.Contains("UnloadPnP") == true)
				{
					manual_start	= (int)MANUAL.UnloadPnP_AllUp;
					manual_end		= (int)MANUAL.UnloadPnP_AllUpDn;
				}
				else if (str_seq.Contains("UnloadInspPnP") == true)
				{
					manual_start	= (int)MANUAL.UnloadInspPnP_AllUp;
					manual_end		= (int)MANUAL.UnloadInspPnP_AllUpDn;
				}

				for (int i = manual_start; i < manual_end + 1; i++)
				{
					string[] str = EnumHelper.GetDescription((MANUAL)i).Split('_');

					Button button		= new Button();
					string strvalue1	= str[1];
					string subvalue2	= strvalue1.Substring(0, 3) + "\n" + strvalue1.Substring(3);

					button.Name			= EnumHelper.GetDescription((MANUAL)i);
					button.Text			= subvalue2;
					button.Dock			= DockStyle.Fill;
					button.Font			= new Font("Segoe UI", 10, FontStyle.Bold);
					button.BackColor	= Color.White;
					button.Margin		= new Padding(1);
					button.TextAlign	= ContentAlignment.MiddleCenter;
					button.Click		+= btnAll_Click;

					// 특정 위치에 버튼 추가
					tlpCyl.Controls.Add(button, ncol, 6);
					ncol++;
				}

			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private string GetSenPos(string strFwd, string strBwd, SenPos senPos)
		{
			try
			{
				if			((int)senPos == 2)		return strFwd.ToUpper();
				else if		((int)senPos == 3)		return strBwd.ToUpper();
				else		return "-";
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return "";
			}
		}

		#endregion

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
				return cp;
			}
		}

		#endregion
	}
}
