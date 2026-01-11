using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace H_MarsSSD
{
	public partial class frmBarcode : Form
	{
		Timer tUpdate;
		bool  m_bStartSendData = false;

		private struct TrayInfo
		{
			public int Row;
			public int Col;
			public TrayInfo(int row, int col)
			{
				Row = row;
				Col = col;
			}
		}
		public frmBarcode(bool bUseInsert = true)
		{

			InitializeComponent();
			tUpdate = new Timer();
			tUpdate.Interval = 500;
			tUpdate.Tick += Update;

			if (bUseInsert == false)
			{
				this.Width = 314;
				this.Height = 617;
				btnCancel.Location = new Point(146, 507);
				chkStartUsed.Visible = false;
				btnLotStart.Visible = false;
			}
		}
		public void SetLanguageText()
		{
			try
			{
				lbLDLiftCount.Text		= Language.languageDefault.ProdCount;
				lbInsertLotCount.Text	= Language.languageDefault.ProdCount;
				gbInsertLot.Text		= Language.languageDefault.InsertLOTInfo;
				gbLDLift.Text			= Language.languageDefault.LDLift;
				btnOK.Text				= Language.languageDefault.MB_OK;
				btnCancel.Text			= Language.languageDefault.MB_Cancel;
				btnLotStart.Text		= Language.languageDefault.LOTStart;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true)
			{
 				tbLotID.Enabled			= false;
 				tbDeviceIDTop.Enabled	= false;
				tbProdCnt.Enabled		= false;
				cbType.Enabled			= false;
				btnOK.Enabled			= false; // 통신받아서 업데이트 될떄 활성화
			}
			else
			{
				tbLotID.Text	= "";
				tbDeviceIDTop.Text	= "";
				tbProdCnt.Text	= "";
 				tbLotID.Enabled			= true;
 				tbDeviceIDTop.Enabled	= true;
				tbProdCnt.Enabled		= true;
				cbType.Enabled			= true;
				btnOK.Enabled			= true;
			}
			

			cbType.Items.Clear();
			//기본으로 현재 열려있는 레시피의 정보 입력
			for (int i = 0; i <= (int)ModuleType.LPCAMM; i++)
			{
				cbType.Items.Add(EnumHelper.GetDescription((ModuleType)i));
			}
			ModuleType SelectedModule = EnumHelper.GetValueFromDescription<ModuleType>(SM.SHM.GetRecipeData(DEVICE.ModuleType));
			cbType.SelectedItem = EnumHelper.GetDescription(SelectedModule);

			this.ActiveControl	= tbLotID;
			tbLotID.Focus();
			Init();
			tUpdate.Start();
		}

		protected override void OnClosed(EventArgs e)
		{
			chkStartUsed.Checked	= false;
			tUpdate.Stop();
			base.OnClosed(e);
		}
		
		private void btnOK_Click(object sender, EventArgs e)
		{
			try
			{
				frmMessageBox	msg;

				if (G.seqTrayLoader.IsReady == false)
				{
					msg	= new frmMessageBox(frmMessageBox.teButtons.OK, "Error", Language.languageDefault.MB_WaitLotLoad);
					msg.ShowDialog();
					return;
				}

				if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.LDLift31) == true)
				{
					msg	= new frmMessageBox(frmMessageBox.teButtons.OK, "Error", Language.languageDefault.MB_ExistLoadLift);
					msg.ShowDialog();
					return;
				}

				// 금지된 특수 문자 배열 가져오기
				char[] invalidChars = Path.GetInvalidFileNameChars();

				// 파일 이름에 금지된 문자가 포함되어 있는지 확인
				foreach (char c in invalidChars)
				{
					if (tbLotID.Text.Contains(c))
					{
						msg = new frmMessageBox(frmMessageBox.teButtons.OK, "Error", $"The character <{c}> cannot be used.");
						msg.ShowDialog();
						return;
					}
				}

				if (SM.FMT.IsFlow(FlowTray.LDWaitBuf) == false)  SM.FMT.Next(FlowTray.Empty, FlowTray.LDWaitBuf);

				var	tcinfo					= SM.FMT[FlowTray.LDWaitBuf].GetInfo();
				tcinfo.WorkOrder			= tbLotID.Text.Trim();
				tcinfo.ProgramID_Top		= tbDeviceIDTop.Text.Trim();
				tcinfo.ProgramID_Btm		= tbDeviceIDBtm.Text.Trim();
				tcinfo.ModuleType			= cbType.SelectedItem.ToString();

				string selectedModule = cbType.SelectedItem.ToString();

				tcinfo.RowCnt = SM.SHM.GetRecipeDataToInt(selectedModule, DEVICE.TrayRowCnt) / 1000;
				tcinfo.ColCnt = SM.SHM.GetRecipeDataToInt(selectedModule, DEVICE.TrayColCnt) / 1000;
				tcinfo.MaxCntProdInTray = tcinfo.RowCnt * tcinfo.ColCnt;
				tcinfo.MaxCntProdInLot = int.Parse(tbProdCnt.Text);
				SM.FMT[FlowTray.LDWaitBuf].SetInfo(tcinfo);

				this.DialogResult	= DialogResult.OK;

				this.Close();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			try
			{
				this.DialogResult	= DialogResult.Cancel;
				this.Close();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void btnClear_Click(object sender, EventArgs e)
		{
			try
			{
				if (SM.FMT.IsFlow(FlowTray.LDWaitBuf) == false)	return;
				
				Init();
				SM.FMT.Clear(FlowTray.LDWaitBuf);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void TextBox_Click(object sender, EventArgs e)
		{
			return;
		//	((TextBox)sender).SelectAll();
		}

		private void btnLotStart_Click(object sender, EventArgs e)
		{
			try
			{
				frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_LoadTray);
				if (pMsg.ShowDialog() == DialogResult.Cancel) return;

				SM.SHM.SetBitData(BIT.LotLoadStart, true);

				this.Close();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void tb_TextChanged(object sender, EventArgs e)
		{
			try
			{
				return;

				Control ctrl = (Control)sender;

				if (SM.FMT.IsFlow(FlowTray.LDWaitBuf) == false)			SM.FMT.Next(FlowTray.Empty, FlowTray.LDWaitBuf);

				var	tcinfo	= SM.FMT[FlowTray.LDWaitBuf].GetInfo();

				switch (ctrl.Name)
				{
					case "tbLotID":		tcinfo.WorkOrder			= tbLotID.Text.Trim();					break;
					case "tbProdCnt":	int.TryParse(tbProdCnt.Text, out tcinfo.MaxCntProdInLot);		break;
					default:			return;
				}

				SM.FMT[FlowTray.LDWaitBuf].SetInfo(tcinfo);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void chkStartUsed_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				if (chkStartUsed.Checked == true)			btnLotStart.Enabled	= true;
				else										btnLotStart.Enabled	= false;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void Init()
		{
			try
			{	
				lbWLotCnt.Text		= "";
				lbModelNameTop.Text	= "";
				lbModelNameBtm.Text = "";
				lbWLotID.Text		= "";

				tbLotID.Text		= " ";
				tbDeviceIDTop.Text	= " ";
				tbDeviceIDBtm.Text	= " ";
				tbProdCnt.Text		= " ";
				lbType.Text			= "";
				
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void Update(object sender, EventArgs e)
		{
			try
			{
				//sgem으로 받은 데이터가 온 경우
				if (string.IsNullOrEmpty(SDm.Read(eSDM.WO)) == false && 
					SM.SHM.GetUseSkipData(USESKIP.SGEM) == true) 
				{
					if (SM.FMT.IsFlow(FlowTray.LDWaitBuf) == false)	SM.FMT.Next(FlowTray.Empty, FlowTray.LDWaitBuf);
					
					InfoTray.tsInfo info_t =  	SM.FMT[FlowTray.LDWaitBuf].GetInfo();
					info_t.WorkOrder		= SDm.Read(eSDM.WO);
					info_t.ProgramID_Top	= SDm.Read(eSDM.PROGRAMID_TOP);
					info_t.ProgramID_Btm	= SDm.Read(eSDM.PROGRAMID_BOTTOM);
					info_t.TrayBarcode		= SDm.Read(eSDM.TRAYID);
					info_t.ColCnt			= int.Parse(SDm.Read(eSDM.COLUMN));
					info_t.RowCnt			= int.Parse(SDm.Read(eSDM.ROW));
					info_t.MaxCntProdInLot	= int.Parse(SDm.Read(eSDM.TOTALQTY));
					
					if (SDm.Read(eSDM.FORMFACTOR) == "OES30")		info_t.ModuleType = ModuleType.OES30.ToString();
					if (SDm.Read(eSDM.FORMFACTOR) == "OES42")		info_t.ModuleType = ModuleType.OES42.ToString();
					if (SDm.Read(eSDM.FORMFACTOR) == "OES80")		info_t.ModuleType = ModuleType.OES80.ToString();
					if (SDm.Read(eSDM.FORMFACTOR) == "OES110")		info_t.ModuleType = ModuleType.OES110.ToString();
					if (SDm.Read(eSDM.FORMFACTOR) == "OES111.49")	info_t.ModuleType = ModuleType.OES111.ToString();
					if (SDm.Read(eSDM.FORMFACTOR) == "LPCAMM")		info_t.ModuleType = ModuleType.LPCAMM.ToString();
					else											G.AlmMng.ADD(ALM.ERROR_FIND_RECIPE);

					SM.FMT[FlowTray.LDWaitBuf].SetInfo(info_t);
					
					SDm.Write(eSDM.WO,"");
					SDm.Write(eSDM.PROGRAMID_TOP,"");
					SDm.Write(eSDM.PROGRAMID_BOTTOM,"");
					SDm.Write(eSDM.TRAYID,"");
					SDm.Write(eSDM.LIDID,"");
					SDm.Write(eSDM.HASLID,"");
					SDm.Write(eSDM.COLUMN,"");
					SDm.Write(eSDM.ROW,"");
					SDm.Write(eSDM.TOTALQTY,"");
					SDm.Write(eSDM.FORMFACTOR, "");
					SDm.Write(eSDM.DDRGENERATION, "");
					
					Init();
					m_bStartSendData = false;
					btnOK.Enabled = true;
				}

				if (SM.FMT.IsFlow(FlowTray.LDWaitBuf) == true)
				{
					lbWLotID.Text			= SM.FMT[FlowTray.LDWaitBuf].GetInfo().WorkOrder;
					lbWLotCnt.Text			= SM.FMT[FlowTray.LDWaitBuf].GetInfo().MaxCntProdInLot.ToString();
					lbModelNameTop.Text		= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ProgramID_Top;
					lbModelNameBtm.Text		= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ProgramID_Btm;

					tbLotID.Text			= SM.FMT[FlowTray.LDWaitBuf].GetInfo().WorkOrder;
					tbDeviceIDTop.Text		= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ProgramID_Top;
					tbDeviceIDBtm.Text		= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ProgramID_Btm;
					tbProdCnt.Text			= SM.FMT[FlowTray.LDWaitBuf].GetInfo().MaxCntProdInLot.ToString();
					
					string CurModuleType	= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ModuleType;
					cbType.SelectedItem		= (ModuleType)Enum.Parse(typeof(ModuleType), CurModuleType);
					lbType.Text				= CurModuleType;

				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

	}
}

