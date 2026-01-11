using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace H_MarsSSD
{
    public partial class frmManualRun : Form
    {
		private	struct	tsBlinkInfo
		{
			public	bool		bBlink;
			public	Control		Ctrl;

			public tsBlinkInfo(bool _bBlink, Control _Ctrl)
			{
				bBlink	= _bBlink;
				Ctrl	= _Ctrl;
			}
		}

		private	CBlinkTimer	m_tmrBlink	= new CBlinkTimer();



        public			frmManualRun()
        {
            InitializeComponent();
        }



		private	void	frmManualRun_Load(object sender, EventArgs e)
		{
			try
			{
				tmrUIUpdate.Start();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnInit_Click(object sender, EventArgs e)
		{
			try
			{
				Button	btn	= (Button)sender;

				switch (btn.Name)
				{
					case "btnInit_All":					SM.SHM.SetManualNum(MANUAL.AllHomeStart);			break;
					case "btnInit_LoadLift":			SM.SHM.SetManualNum(MANUAL.TrayLoader);				break;
					case "btnInit_UnloadLift":			SM.SHM.SetManualNum(MANUAL.TrayUnloader);			break;
					case "btnInit_LoadTrans":			SM.SHM.SetManualNum(MANUAL.LoadTransfer);			break;
					case "btnInit_UnloadTrans":			SM.SHM.SetManualNum(MANUAL.UnloadTransfer);			break;
					case "btnInit_TrayClean":			SM.SHM.SetManualNum(MANUAL.TrayClean);				break;
					case "btnInit_LoadTrayTable1":		SM.SHM.SetManualNum(MANUAL.LoadTrayTable1);			break;
					case "btnInit_LoadTrayTable2":		SM.SHM.SetManualNum(MANUAL.LoadTrayTable2);			break;
					case "btnInit_UnloadTrayTable1":	SM.SHM.SetManualNum(MANUAL.UnloadTrayTable1);		break;
					case "btnInit_UnloadTrayTable2":	SM.SHM.SetManualNum(MANUAL.UnloadTrayTable2);		break;
					case "btnInit_LoadPnP":				SM.SHM.SetManualNum(MANUAL.LoadPnP);				break;
					case "btnInit_UnloadPnP":			SM.SHM.SetManualNum(MANUAL.UnloadPnP);				break;
					case "btnInit_LoadBuffer1":			SM.SHM.SetManualNum(MANUAL.LoadBuffer1);			break;
					case "btnInit_LoadBuffer2":			SM.SHM.SetManualNum(MANUAL.LoadBuffer2);			break;
					case "btnInit_LoadInspPnP":			SM.SHM.SetManualNum(MANUAL.LoadInspPnP);			break;
					case "btnInit_UnloadInspPnP":		SM.SHM.SetManualNum(MANUAL.UnloadInspPnP);			break;
					case "btnInit_InspPlate1":			SM.SHM.SetManualNum(MANUAL.InspPlate1);				break;
					case "btnInit_InspPlate2":			SM.SHM.SetManualNum(MANUAL.InspPlate2);				break;
					case "btnInit_UnloadBuffer":		SM.SHM.SetManualNum(MANUAL.UnloadBuffer);			break;
					case "btnInit_RejectBuffer":		SM.SHM.SetManualNum(MANUAL.RejectBuffer);			break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}



		private	void	ControlBlink()
		{
			try
			{
				bool			bblink		= m_tmrBlink.Blink();
				tsBlinkInfo[]	binfoAll	= new tsBlinkInfo[] {	new tsBlinkInfo(G.seqTrayLoader.IsHomeReady(),			btnInit_LoadLift),
																	new tsBlinkInfo(G.seqTrayLoader.IsHomeReady(),			btnInit_LoadLift),
																	new tsBlinkInfo(G.seqTrayUnloader.IsHomeReady(),			btnInit_UnloadLift),
																	new tsBlinkInfo(G.seqLoadTransfer.IsHomeReady(),		btnInit_LoadTrans),
																	new tsBlinkInfo(G.seqUnloadTransfer.IsHomeReady(),		btnInit_UnloadTrans),
																	new tsBlinkInfo(G.seqTrayClean.IsHomeReady(),			btnInit_TrayClean),
																	new tsBlinkInfo(G.seqLoadTrayTable1.IsHomeReady(),		btnInit_LoadTrayTable1),
																	new tsBlinkInfo(G.seqLoadTrayTable2.IsHomeReady(),		btnInit_LoadTrayTable2),
																	new tsBlinkInfo(G.seqUnloadTrayTable1.IsHomeReady(),	btnInit_UnloadTrayTable1),
																	new tsBlinkInfo(G.seqUnloadTrayTable2.IsHomeReady(),	btnInit_UnloadTrayTable2),
																	new tsBlinkInfo(G.seqLoadPnP.IsHomeReady(),				btnInit_LoadPnP),
																	new tsBlinkInfo(G.seqUnloadPnP.IsHomeReady(),			btnInit_UnloadPnP),
																	new tsBlinkInfo(G.seqLoadBuffer1.IsHomeReady(),			btnInit_LoadBuffer1),
																	new tsBlinkInfo(G.seqLoadBuffer2.IsHomeReady(),			btnInit_LoadBuffer2),
																	new tsBlinkInfo(G.seqLoadInspPnP.IsHomeReady(),			btnInit_LoadInspPnP),
																	new tsBlinkInfo(G.seqUnloadInspPnP.IsHomeReady(),		btnInit_UnloadInspPnP),
																	new tsBlinkInfo(G.seqInspPlate1.IsHomeReady(),			btnInit_InspPlate1),
																	new tsBlinkInfo(G.seqInspPlate2.IsHomeReady(),			btnInit_InspPlate2),
																	new tsBlinkInfo(G.seqUnloadBuffer.IsHomeReady(),		btnInit_UnloadBuffer),
																	new tsBlinkInfo(G.seqRejectBuffer.IsHomeReady(),		btnInit_RejectBuffer) };
				
				foreach (tsBlinkInfo info in binfoAll)
				{
					if (bblink == true  &&  info.bBlink == false)			SetControlColor(info.Ctrl,	Color.Green, Color.White);
					else													SetControlColor(info.Ctrl,	Color.White, SystemColors.ControlText);
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



		private	void	tmrUIUpdate_Tick(object sender, EventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)			SetControlEnable(this, false);
				else												SetControlEnable(this, true);

				ControlBlink();


                // 
                //                 //  { "LDLIFT HOME", "LDLIFT LOAD", "LDLIFT DOWN", "LDLIFT UP" };
                //                 string str = "";
                //                 bool bIsReady = false;
                //                 if (SM.SHM.m_dicStep.TryGetValue("LDLIFT LIFTUP",   out str))   lbLdLiftUpStep.Text = str;        
                //                 if (SM.SHM.m_dicStep.TryGetValue("LDLIFT LIFTDOWN", out str))   lbLdLiftDownStep.Text = str;
                //                 if (SM.SHM.m_dicStep.TryGetValue("LDLIFT LOAD",     out str))   lbLdLiftLoadStep.Text = str;
                // 
                //                 if (SM.SHM.m_dicIsReady.TryGetValue("LDLIFT LIFTUP",    out bIsReady)) btnLdLiftLiftUp.BackColor = bIsReady ==true ? Color.White : Color.Lime;
                //                 if (SM.SHM.m_dicIsReady.TryGetValue("LDLIFT LIFTDOWN",  out bIsReady)) btnLdLiftLiftDown.BackColor = bIsReady == true ? Color.White : Color.Lime;
                //                 if (SM.SHM.m_dicIsReady.TryGetValue("LDLIFT LOAD",      out bIsReady)) btnLdLiftLoad.BackColor = bIsReady == true ? Color.White : Color.Lime;

                //                 lbLdTransPlaceStep.Text = NDm.Read(eNDM.seqLdTransPlace).ToString();
                //                 lbLdTransPnPStep.Text = NDm.Read(eNDM.seqLdTransPickAndPlace).ToString();
                //                 lbLdTransTrayLoadStep.Text = NDm.Read(eNDM.seqLdTransTrayLoad).ToString();
                // 
                //                 btnLdTransPlace.BackColor = SM.SHM.IsBitData(BIT.LdTransPlaceReady) == true ? Color.White : Color.Lime;
                //                 btnLdTransPnP.BackColor = SM.SHM.IsBitData(BIT.LdTransPickAndPlaceReady) == true ? Color.White : Color.Lime;
                //                 btnLdTransTrayLoad.BackColor = SM.SHM.IsBitData(BIT.LdTransTrayLoadReady) == true ? Color.White : Color.Lime;



                //   { "LDLIFT HOME", "LDLIFT LOAD", "LDLIFT DOWN", "LDLIFT UP" };
                //  { "CAPBUFFER HOME", "CAPBUFFER LOAD", "CAPBUFFER UNLOAD" };
                //  { "CAPCOVER HOME", "CAPCOVER CAPCOVER" };
                //  { "CAPUNCOVER HOME", "CAPUNCOVER CAPUNCOVER" };
                //  { "INSPPLATE HOME", "INSPPLATE LOADWAIT", "INSPPLATE UNLOADWAIT", "INSPPLATE INSPWAIT" };
                // { "LDBUFFER1 HOME", "LDBUFFER1 LOADWAIT", "LDBUFFER1 CLEAN", "LDBUFFER1 INSPWAIT" };
                // { "LDBUFFER2 HOME", "LDBUFFER2 LOADWAIT", "LDBUFFER2 CLEAN", "LDBUFFER2 INSPWAIT" };
                // { "LDINSPPNP HOME", "LDINSPPNP PICK", "LDINSPPNP PLACE", "LDINSPPNP PNP" };
                //  { "LOADPNP HOME", "LOADPNP PICK", "LOADPNP PLACE", "LOADPNP PNP" };
                //  { "LDTRANS HOME", "LDTRANS PLACE", "LDTRANS TRAYLOAD", "LDTRANS PNP" };
                // { "LOADTRAY1 HOME" , "LOADTRAY1 LOAD" , "LOADTRAY1 UNLOAD","LOADTRAY1 WAIT"};
                // { "LOADTRAY2 HOME" , "LOADTRAY2 LOAD" , "LOADTRAY2 UNLOAD","LOADTRAY2 WAIT"};
                // { "REJECTBUFFER HOME", "REJECTBUFFER MOVE" };
                //  { "TRAYCLEAN HOME", "TRAYCLEAN CLEAN" };
                //  { "ULDBUFFER HOME", "ULDBUFFER LOADWAIT", "ULDBUFFER UNLOADWAIT" };
                //  { "ULDINSPPNP HOME", "ULDINSPPNP PICK", "ULDINSPPNP", "ULDINSPPNP PNP" };
                //  { "ULDLIFT HOME", "ULDLIFT UNLOAD", "ULDLIFT LIFTUP", "ULDLIFT LIFTDOWN", "ULDLIFT ALIGN" };
                //  { "UNLOADPnP HOME", "UNLOADPnP PICK", "UNLOADPnP PICKREJECT", "UNLOADPnP PLACE", "UNLOADPnP PNP" };
                //  { "UNDTRANS HOME", "UNDTRANS PLACE", "UNDTRANS TRAYUNLOAD", "UNDTRANS PNP" };
                // { "UNLOADTRAY1 HOME", "UNLOADTRAY1 LOAD","UNLOADTRAY1 UNLOAD", "UNLOADTRAY1 WAIT"};
                // { "UNLOADTRAY2 HOME", "UNLOADTRAY2 LOAD","UNLOADTRAY2 UNLOAD", "UNLOADTRAY2 WAIT"};
            }
            catch
			{

			}
		}
    }
}
