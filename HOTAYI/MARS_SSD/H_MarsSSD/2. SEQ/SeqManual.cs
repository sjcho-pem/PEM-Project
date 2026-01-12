using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
	//using static G;

	public class SeqManual
	{
		public enum stRun
		{
			START = 100,
			CHECK,

			END = 199,
		}

		private SMM<stRun>	smmRun		= new SMM<stRun>(stRun.END, "MANUAL RUN PROCESS");
		private bool		bRunning	= false;

		//-----------------------------------------------------------------------
		// Tenkey Process
		public void Run()
		{
			try
			{
				MANUAL eNo = SM.SHM.GetManualNum();

				switch (smmRun.CurStep)
				{
					case stRun.START:
						{
							if (smmRun.eInput == InState.run)
							{
								smmRun.eInput = InState.none;
								smmRun.Next(stRun.CHECK);
							}
						}
						break;

					case stRun.CHECK:
						{
							SetBitMoveSafety();
							bool bEmo			= SM.SHM.IsBitData(BIT.EMO);
							bool bkeylock_auto	= SM.SHM.IsBitData(BIT.KeyLockAuto);
							if (bEmo == true  ||  bkeylock_auto == true)
							{
								if (bkeylock_auto == true  &&  eNo != MANUAL.NotManual)
								{
									SM.SHM.MsgShow(SeqMsg.KeyLockErr);
								}

								SM.SHM.ClearManualNum();
								break;
							}

							if (bRunning == true)
								break;
							if (eNo != MANUAL.NotManual)
							{
								bRunning = true;
								smmRun.ResetDelayms();
								GLog.Add(string.Format("Manual No : {0}({1})", eNo.ToString(), (int)eNo));
								SM.SHM.SetBitData(BIT.IsManualReq, true);
								SM.SHM.ClearManualNum();
							}
						}
						break;

					case stRun.END:
						{
							smmRun.Next(stRun.START);
						}
						break;
				}

				if (bRunning == false)
					return;

				switch (eNo)
				{
					//#region 단위 동작 정의(#0 ~ #199)

					case MANUAL.TrayLoader:			if (G.seqTrayLoader.IsHomeReady() == true)			G.seqTrayLoader.HomeStart();		break;
					case MANUAL.TrayUnloader:		if (G.seqTrayUnloader.IsHomeReady() == true)		G.seqTrayUnloader.HomeStart();		break;
					case MANUAL.LoadTransfer:		if (G.seqLoadTransfer.IsHomeReady() == true)		G.seqLoadTransfer.HomeStart();		break;
					case MANUAL.UnloadTransfer:		if (G.seqUnloadTransfer.IsHomeReady() == true)		G.seqUnloadTransfer.HomeStart();	break;
					case MANUAL.TrayClean:			if (G.seqTrayClean.IsHomeReady() == true)			G.seqTrayClean.HomeStart();			break;
					case MANUAL.LoadTrayTable1:		if (G.seqLoadTrayTable1.IsHomeReady() == true)		G.seqLoadTrayTable1.HomeStart();	break;
					case MANUAL.LoadTrayTable2:		if (G.seqLoadTrayTable2.IsHomeReady() == true)		G.seqLoadTrayTable2.HomeStart();	break;
					case MANUAL.UnloadTrayTable1:	if (G.seqUnloadTrayTable1.IsHomeReady() == true)	G.seqUnloadTrayTable1.HomeStart();	break;
					case MANUAL.UnloadTrayTable2:	if (G.seqUnloadTrayTable2.IsHomeReady() == true)	G.seqUnloadTrayTable2.HomeStart();	break;
					case MANUAL.LoadPnP:			if (G.seqLoadPnP.IsHomeReady() == true)				G.seqLoadPnP.HomeStart();			break;
					case MANUAL.UnloadPnP:			if (G.seqUnloadPnP.IsHomeReady() == true)			G.seqUnloadPnP.HomeStart();			break;
					case MANUAL.LoadBuffer1:		if (G.seqLoadBuffer1.IsHomeReady() == true)			G.seqLoadBuffer1.HomeStart();		break;
					case MANUAL.LoadBuffer2:		if (G.seqLoadBuffer2.IsHomeReady() == true)			G.seqLoadBuffer2.HomeStart();		break;
					case MANUAL.LoadInspPnP:		if (G.seqLoadInspPnP.IsHomeReady() == true)			G.seqLoadInspPnP.HomeStart();		break;
					case MANUAL.UnloadInspPnP:		if (G.seqUnloadInspPnP.IsHomeReady() == true)		G.seqUnloadInspPnP.HomeStart();		break;
					case MANUAL.InspPlate1:			if (G.seqInspPlate1.IsHomeReady() == true)			G.seqInspPlate1.HomeStart();		break;
					case MANUAL.InspPlate2:			if (G.seqInspPlate2.IsHomeReady() == true)			G.seqInspPlate2.HomeStart();		break;
					case MANUAL.UnloadBuffer:		if (G.seqUnloadBuffer.IsHomeReady() == true)		G.seqUnloadBuffer.HomeStart();		break;
					case MANUAL.RejectBuffer:		if (G.seqRejectBuffer.IsHomeReady() == true)		G.seqRejectBuffer.HomeStart();		break;

					case MANUAL.LoadPnPPick1: SM.SHM.ClearManualNum(); if (G.seqLoadPnP.IsPickTrayRetryReady() == true) G.seqLoadPnP.PickRetryStart(0); break;
					case MANUAL.LoadPnPPick2: SM.SHM.ClearManualNum(); if (G.seqLoadPnP.IsPickTrayRetryReady() == true) G.seqLoadPnP.PickRetryStart(1); break;
					case MANUAL.LoadPnPPick3: SM.SHM.ClearManualNum(); if (G.seqLoadPnP.IsPickTrayRetryReady() == true) G.seqLoadPnP.PickRetryStart(2); break;
					case MANUAL.LoadPnPPick4: SM.SHM.ClearManualNum(); if (G.seqLoadPnP.IsPickTrayRetryReady() == true) G.seqLoadPnP.PickRetryStart(3); break;
					case MANUAL.LoadPnPPick5: SM.SHM.ClearManualNum(); if (G.seqLoadPnP.IsPickTrayRetryReady() == true) G.seqLoadPnP.PickRetryStart(4); break;
					case MANUAL.LoadPnPPick6: SM.SHM.ClearManualNum(); if (G.seqLoadPnP.IsPickTrayRetryReady() == true) G.seqLoadPnP.PickRetryStart(5); break;
					case MANUAL.LoadPnPPick7: SM.SHM.ClearManualNum(); if (G.seqLoadPnP.IsPickTrayRetryReady() == true) G.seqLoadPnP.PickRetryStart(6); break;
					case MANUAL.LoadPnPPick8: SM.SHM.ClearManualNum(); if (G.seqLoadPnP.IsPickTrayRetryReady() == true) G.seqLoadPnP.PickRetryStart(7); break;
					case MANUAL.UnloadPnPPickReject1: SM.SHM.ClearManualNum(); if (G.seqUnloadPnP.IsPickRejectRetryReady() == true) G.seqUnloadPnP.PickRejectRetryStart(0); break;
					case MANUAL.UnloadPnPPickReject2: SM.SHM.ClearManualNum(); if (G.seqUnloadPnP.IsPickRejectRetryReady() == true) G.seqUnloadPnP.PickRejectRetryStart(1); break;
					case MANUAL.UnloadPnPPickReject3: SM.SHM.ClearManualNum(); if (G.seqUnloadPnP.IsPickRejectRetryReady() == true) G.seqUnloadPnP.PickRejectRetryStart(2); break;
					case MANUAL.UnloadPnPPickReject4: SM.SHM.ClearManualNum(); if (G.seqUnloadPnP.IsPickRejectRetryReady() == true) G.seqUnloadPnP.PickRejectRetryStart(3); break;
					case MANUAL.UnloadPnPPickReject5: SM.SHM.ClearManualNum(); if (G.seqUnloadPnP.IsPickRejectRetryReady() == true) G.seqUnloadPnP.PickRejectRetryStart(4); break;
					case MANUAL.UnloadPnPPickReject6: SM.SHM.ClearManualNum(); if (G.seqUnloadPnP.IsPickRejectRetryReady() == true) G.seqUnloadPnP.PickRejectRetryStart(5); break;
					case MANUAL.UnloadPnPPickReject7: SM.SHM.ClearManualNum(); if (G.seqUnloadPnP.IsPickRejectRetryReady() == true) G.seqUnloadPnP.PickRejectRetryStart(6); break;
					case MANUAL.UnloadPnPPickReject8: SM.SHM.ClearManualNum(); if (G.seqUnloadPnP.IsPickRejectRetryReady() == true) G.seqUnloadPnP.PickRejectRetryStart(7); break;
					case MANUAL.LoadCovMinusJog:
						G.cKernel.Motor(MT.LDConv1).MoveJog(false, 10000);
						G.cKernel.Motor(MT.LDConv2).MoveJog(false, 10000);
						break;
					case MANUAL.LoadCovPlusJog:
						G.cKernel.Motor(MT.LDConv1).MoveJog(true, 10000);
						G.cKernel.Motor(MT.LDConv2).MoveJog(true, 10000);
						break;
					case MANUAL.LoadCovStop:
						G.cKernel.Motor(MT.LDConv1).Stop();
						G.cKernel.Motor(MT.LDConv2).Stop();
						break;

					case MANUAL.AllHomeStop: // All Home Stop
						break;

					case MANUAL.AllHomeStart: // All Home
						{
							if (SM.SHM.IsBitData(BIT.AllHoming) == false)
							{
								G.allHome.Start();
							}
						}
						break;
					case MANUAL.ReferencePositionTop1:
						{
							if (G.seqInspPlate1.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate1 Sequence");
								pMsg.ShowDialog();
								return;
							}
							if (G.cKernel.Motor(MT.InspPlate1Y).IsRdy() == false || G.cKernel.Motor(MT.InspPlate1Y).IsHomeDone() == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check InspPlateY1 Motor");
								pMsg.ShowDialog();
								return;
							}
							if (SM.SHM.IsBitData(BIT.Pause) == true || SM.SHM.IsBitData(BIT.AutoRun))
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please stop the machine and try again.");
								pMsg.ShowDialog();
								return;
							}

                            G.seqInspPlate1.ReferenceStart(0);
						}
						break;
					case MANUAL.ReferencePositionBtm1:
						{
							if (G.seqInspPlate2.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate2 Sequence");
								pMsg.ShowDialog();
								return;
							}
							if (G.cKernel.Motor(MT.InspPlate2Y).IsRdy() == false || G.cKernel.Motor(MT.InspPlate2Y).IsHomeDone() == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check InspPlateY2 Motor");
								pMsg.ShowDialog();
								return;
							}
							if (SM.SHM.IsBitData(BIT.Pause) == true || SM.SHM.IsBitData(BIT.AutoRun))
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please stop the machine and try again.");
								pMsg.ShowDialog();
								return;
							}

                            G.seqInspPlate2.ReferenceStart(0);
						}
						break;
					case MANUAL.ReferencePositionTop2:
						{
							if (G.seqInspPlate1.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate1");
								pMsg.ShowDialog();
								return;
							}
							if (SM.SHM.IsBitData(BIT.TopInspConnect) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check TOP PC Socket Connetion");
								pMsg.ShowDialog();
								return;
							}
							if (SM.SHM.IsBitData(BIT.Pause) == true || SM.SHM.IsBitData(BIT.AutoRun))
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please stop the machine and try again.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate1.ReferenceStart(1);
						}
						break;
					case MANUAL.ReferencePositionBtm2:
						{
							if (G.seqInspPlate2.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate2");
								pMsg.ShowDialog();
								return;
							}
							if (SM.SHM.IsBitData(BIT.BtmInspConnect) == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Bottom PC Socket Connetion");
								pMsg.ShowDialog();
								return;
							}
							if (SM.SHM.IsBitData(BIT.Pause) == true || SM.SHM.IsBitData(BIT.AutoRun))
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please stop the machine and try again.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate2.ReferenceStart(1);
						}
						break;


					//	nHead 0: 1번헤드, 1:2번 헤드  nJigType 0: 조명, 1: 3D
					case MANUAL.TopCalibrationLightHead1:
						{
							if (G.seqInspPlate1.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate1.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate1.CalibrationStart(0, 0);
						}
						break;
					case MANUAL.TopCalibrationLightHead2:
						{
							if (G.seqInspPlate1.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate1.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate1.CalibrationStart(1, 0);
						}
						break;
					case MANUAL.TopCalibration3DHead1:
						{
							if (G.seqInspPlate1.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate1.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate1.CalibrationStart(0, 1);
						}
						break;
					case MANUAL.TopCalibration3DHead2:
						{

							if (G.seqInspPlate1.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate1.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate1.CalibrationStart(1, 1);
						}
						break;


					case MANUAL.BottomCalibrationLightHead1:
						{
							if (G.seqInspPlate2.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate2.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate2.CalibrationStart(0, 0);
						}
						break;


					case MANUAL.BottomCalibrationLightHead2:
						{
							if (G.seqInspPlate2.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate2.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate2.CalibrationStart(1, 0);
						}
						break;


					case MANUAL.BottomCalibration3DHead1:
						{
							if (G.seqInspPlate2.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate2.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate2.CalibrationStart(0, 1);
						}
						break;


					case MANUAL.BottomCalibration3DHead2:
						{
							if (G.seqInspPlate2.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate2.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate2.CalibrationStart(1, 1);
						}
						break;


					case MANUAL.Stage1Maintenance:
						{
							if (G.seqInspPlate2.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate1.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate1.CalibrationStart(0, 2);
						}
						break;
					case MANUAL.Stage2Maintenance:
						{
							if (G.seqInspPlate2.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate2.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate2.CalibrationStart(0, 3);
						}
						break;
					case MANUAL.BotHeadMaintenance:
						{
							if (G.seqInspPlate1.IsReady == false)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Inspection Plate1.");
								pMsg.ShowDialog();
								return;
							}

							G.seqInspPlate1.CalibrationStart(0, 4);
						}
						break;


					case MANUAL.LDPnPAllGrpCyl:
						G.seqLoadPnP.Reverse(SeqLoadPnP.Act.Vac1);
						G.seqLoadPnP.Reverse(SeqLoadPnP.Act.Vac2);
						G.seqLoadPnP.Reverse(SeqLoadPnP.Act.Vac3);
						G.seqLoadPnP.Reverse(SeqLoadPnP.Act.Vac4);
						break;
					case MANUAL.LDPnPAllUpDnCyl:
						G.seqLoadPnP.Reverse(SeqLoadPnP.Act.PnPUpDn1);
						G.seqLoadPnP.Reverse(SeqLoadPnP.Act.PnPUpDn2);
						G.seqLoadPnP.Reverse(SeqLoadPnP.Act.PnPUpDn3);
						G.seqLoadPnP.Reverse(SeqLoadPnP.Act.PnPUpDn4);
						break;
					case MANUAL.LDInspPnPAllGrpCyl:
						G.seqLoadInspPnP.Reverse(SeqLoadInspPnP.Act.Vac1);
						G.seqLoadInspPnP.Reverse(SeqLoadInspPnP.Act.Vac2);
						G.seqLoadInspPnP.Reverse(SeqLoadInspPnP.Act.Vac3);
						G.seqLoadInspPnP.Reverse(SeqLoadInspPnP.Act.Vac4);
						break;
					case MANUAL.LDInspPnPAllUpDnCyl:
						G.seqLoadInspPnP.Reverse(SeqLoadInspPnP.Act.PnPUpDn);
						break;
					case MANUAL.ULDPnPAllGrpCyl:
						G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.Vac1);
						G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.Vac2);
						G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.Vac3);
						G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.Vac4);
						break;
					case MANUAL.ULDPnPAllUpDnCyl:
						G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.PnPUpDn1);
						G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.PnPUpDn2);
						G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.PnPUpDn3);
						G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.PnPUpDn4);
						break;
					case MANUAL.ULDInspPnPAllGrpCyl:
						G.seqUnloadInspPnP.Reverse(SeqUnloadInspPnP.Act.Vac1);
						G.seqUnloadInspPnP.Reverse(SeqUnloadInspPnP.Act.Vac2);
						G.seqUnloadInspPnP.Reverse(SeqUnloadInspPnP.Act.Vac3);
						G.seqUnloadInspPnP.Reverse(SeqUnloadInspPnP.Act.Vac4);
						break;
					case MANUAL.ULDInspPnPAllUpDnCyl:
						G.seqUnloadInspPnP.Reverse(SeqUnloadInspPnP.Act.PnPUpDn);
						break;
					case MANUAL.LDTable1Grip:
						G.seqLoadTrayTable1.Reverse(SeqLoadTrayTable1.Act.TrayGripX);
						break;
					case MANUAL.LDTable2Grip:
						G.seqLoadTrayTable2.Reverse(SeqLoadTrayTable2.Act.TrayGripX);
						break;
					case MANUAL.ULDTable1Grip:
						G.seqUnloadTrayTable1.Reverse(SeqUnloadTrayTable.Act.TrayGripX);
						break;
					case MANUAL.ULDTable2Grip:
						G.seqUnloadTrayTable2.Reverse(SeqUnloadTrayTable.Act.TrayGripX);
						break;
					case MANUAL.TrayCleanAirReverse:
						G.seqTrayClean.Reverse(SeqTrayClean.Act.AirMove);
						break;
					case MANUAL.LdTable1Reverse:
						G.seqLoadTrayTable1.Reverse(SeqLoadTrayTable1.Act.TrayGripX);
						break;
					case MANUAL.LdTable2Reverse:
						G.seqLoadTrayTable2.Reverse(SeqLoadTrayTable2.Act.TrayGripX);
						break;
					case MANUAL.ULdTable1Reverse:
						G.seqUnloadTrayTable1.Reverse(SeqUnloadTrayTable.Act.TrayGripX);
						break;
					case MANUAL.ULdTable2Reverse:
						G.seqUnloadTrayTable2.Reverse(SeqUnloadTrayTable.Act.TrayGripX);
						break;

					case MANUAL.ULDPnPSort1: G.seqUnloadPnP.SortStart(FlowTray.ULDTable1); break;
					case MANUAL.ULDPnPSort2: G.seqUnloadPnP.SortStart(FlowTray.ULDTable2); break;



					case MANUAL.ManualCyl:
						SM.SHM.SetActPos((Seq)SM.SHM.GetManualParam(0), (int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2));

						switch ((Seq)SM.SHM.GetManualParam(0))
						{
							case Seq.TrayLoader: G.seqTrayLoader.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.TrayClean: G.seqTrayClean.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.LoadTransfer: G.seqLoadTransfer.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.LoadTrayTable1: G.seqLoadTrayTable1.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.LoadTrayTable2: G.seqLoadTrayTable2.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.LoadPnP: G.seqLoadPnP.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.LoadBuffer1: G.seqLoadBuffer1.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.LoadBuffer2: G.seqLoadBuffer2.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.LoadInspPnP: G.seqLoadInspPnP.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.InspPlate1: G.seqInspPlate1.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.InspPlate2: G.seqInspPlate2.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.UnloadInspPnP: G.seqUnloadInspPnP.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.UnloadBuffer: G.seqUnloadBuffer.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.RejectBuffer: G.seqRejectBuffer.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.UnloadPnP: G.seqUnloadPnP.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.UnloadTrayTable1: G.seqUnloadTrayTable1.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.UnloadTrayTable2: G.seqUnloadTrayTable2.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.UnloadTransfer: G.seqUnloadTransfer.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;
							case Seq.Unloader: G.seqTrayUnloader.ActPos((int)SM.SHM.GetManualParam(1), (SenPos)SM.SHM.GetManualParam(2)); break;

							default:
								break;
						}
						break;

					case MANUAL.AllTrayGrip:
						G.seqLoadTrayTable1.ActPos(SeqLoadTrayTable1.Act.TrayGripX, (SenPos)SM.SHM.GetManualParam(0));
						G.seqLoadTrayTable2.ActPos(SeqLoadTrayTable2.Act.TrayGripX, (SenPos)SM.SHM.GetManualParam(0));
						G.seqUnloadTrayTable1.ActPos(SeqUnloadTrayTable.Act.TrayGripX, (SenPos)SM.SHM.GetManualParam(0));
						G.seqUnloadTrayTable2.ActPos(SeqUnloadTrayTable.Act.TrayGripX, (SenPos)SM.SHM.GetManualParam(0));
						break;

					//	메뉴얼 시퀀스
					case MANUAL.LOADER_HOME:		if (G.seqTrayLoader.IsReady == false)	return; G.seqTrayLoader.HomeStart();			break;
					case MANUAL.LOADER_LOAD:		if (G.seqTrayLoader.IsReady == false)	return; G.seqTrayLoader.LoadStart();			break;
					case MANUAL.LOADER_UNLOAD:		if (G.seqTrayLoader.IsReady == false)	return; G.seqTrayLoader.UnloadStart();			break;
					case MANUAL.LOADER_DOWN:		if (G.seqTrayLoader.IsReady == false)	return; G.seqTrayLoader.LiftDownStart(false);	break;
					case MANUAL.LOADER_UP:			if (G.seqTrayLoader.IsReady == false)	return; G.seqTrayLoader.LiftUpStart(false);		break;
						
					case MANUAL.TRAYCLEAN_HOME:		if (G.seqTrayClean.IsReady == false) return; G.seqTrayClean.HomeStart();				break;
					case MANUAL.TRAYCLEAN_CLEAN:	if (G.seqTrayClean.IsReady == false) return; G.seqTrayClean.TrayCleanStart();			break;
					case MANUAL.TRAYCLEAN_GRIP:		if (G.seqTrayClean.IsReady == false) return; G.seqTrayClean.GripStart();				break;
					case MANUAL.TRAYCLEAN_RELEASE:	if (G.seqTrayClean.IsReady == false) return; G.seqTrayClean.ReleaseStart();				break;

					case MANUAL.LDTRANS_HOME:		if (G.seqLoadTransfer.IsReady == false) return; G.seqLoadTransfer.HomeStart();													break;
					case MANUAL.LDTRANS_PICK:		if (G.seqLoadTransfer.IsReady == false) return; G.seqLoadTransfer.PickStart((SeqLoadTransfer.PosX)SM.SHM.GetManualParam(1));	break;
					case MANUAL.LDTRANS_PICKCONV:	if (G.seqLoadTransfer.IsReady == false) return; G.seqLoadTransfer.PickConvStart(false);											break;
					case MANUAL.LDTRANS_PLACE:		if (G.seqLoadTransfer.IsReady == false) return; G.seqLoadTransfer.PlaceStart((SeqLoadTransfer.PosX)SM.SHM.GetManualParam(1));	break;
					case MANUAL.LDTRANS_PLACECONV:	if (G.seqLoadTransfer.IsReady == false) return; G.seqLoadTransfer.PlaceConvStart(false);										break;
					case MANUAL.LDTRANS_PNP:		if (G.seqLoadTransfer.IsReady == false) return; G.seqLoadTransfer.PnPStart((SeqLoadTransfer.PosX)SM.SHM.GetManualParam(0), (SeqLoadTransfer.PosX)SM.SHM.GetManualParam(1));		break;

					case MANUAL.LOADTABLE1_HOME:	if (G.seqLoadTrayTable1.IsReady == false) return; G.seqLoadTrayTable1.HomeStart();									break;
					case MANUAL.LOADTABLE1_UP:		if (G.seqLoadTrayTable1.IsReady == false) return; G.seqLoadTrayTable1.UpStart(SeqLoadTrayTable1.UDMode.Manual);	break;
					case MANUAL.LOADTABLE1_DOWN:	if (G.seqLoadTrayTable1.IsReady == false) return; G.seqLoadTrayTable1.DownStart(SeqLoadTrayTable1.UDMode.Manual);	break;
					case MANUAL.LOADTABLE1_GRIP:	if (G.seqLoadTrayTable1.IsReady == false) return; G.seqLoadTrayTable1.GripStart();									break;
					case MANUAL.LOADTABLE1_RELEASE:	if (G.seqLoadTrayTable1.IsReady == false) return; G.seqLoadTrayTable1.ReleaseStart(); break;

					case MANUAL.LOADTABLE2_HOME:	if (G.seqLoadTrayTable2.IsReady == false) return; G.seqLoadTrayTable2.HomeStart();									break;
					case MANUAL.LOADTABLE2_UP:		if (G.seqLoadTrayTable2.IsReady == false) return; G.seqLoadTrayTable2.UpStart(SeqLoadTrayTable2.UDMode.Manual);		break;
					case MANUAL.LOADTABLE2_DOWN:	if (G.seqLoadTrayTable2.IsReady == false) return; G.seqLoadTrayTable2.DownStart(SeqLoadTrayTable2.UDMode.Manual);	break;
					case MANUAL.LOADTABLE2_GRIP:	if (G.seqLoadTrayTable2.IsReady == false) return; G.seqLoadTrayTable2.GripStart();									break;
					case MANUAL.LOADTABLE2_RELEASE:	if (G.seqLoadTrayTable2.IsReady == false) return; G.seqLoadTrayTable2.ReleaseStart();								break;

					case MANUAL.LOADPNP_HOME:			if (G.seqLoadPnP.IsReady == false) return; G.seqLoadPnP.HomeStart();		break;
					case MANUAL.LOADPNP_PICKTRAY:		if (G.seqLoadPnP.IsReady == false) return; G.seqLoadPnP.PickTrayStart();	break;
					case MANUAL.LOADPNP_PLACEBUFFER:	if (G.seqLoadPnP.IsReady == false) return; G.seqLoadPnP.PlaceBufStart();	break;
					case MANUAL.LOADPNP_PNP:			if (G.seqLoadPnP.IsReady == false) return; G.seqLoadPnP.PnPStart();			break;
					case MANUAL.LOADPNP_EMPTYCHECK:		if (G.seqLoadPnP.IsReady == false) return; G.seqLoadPnP.EmptyCheckStart(FlowTray.LDTable1 + (int)SM.SHM.GetManualParam(0)); break;

					case MANUAL.LDBUFFER1_HOME:			if (G.seqLoadBuffer1.IsReady == false) return; G.seqLoadBuffer1.HomeStart(); break;
					case MANUAL.LDBUFFER1_LOADWAIT:		if (G.seqLoadBuffer1.IsReady == false) return; G.seqLoadBuffer1.LoadWaitStart(); break;
					case MANUAL.LDBUFFER1_CLEAN:		if (G.seqLoadBuffer1.IsReady == false) return; G.seqLoadBuffer1.CleanStart(); break;
					case MANUAL.LDBUFFER1_UNLOADWAIT:	if (G.seqLoadBuffer1.IsReady == false) return; G.seqLoadBuffer1.UnloadWaitStart(); break;

					case MANUAL.LDBUFFER2_HOME:			if (G.seqLoadBuffer2.IsReady == false) return; G.seqLoadBuffer2.HomeStart(); break;
					case MANUAL.LDBUFFER2_LOADWAIT:		if (G.seqLoadBuffer2.IsReady == false) return; G.seqLoadBuffer2.LoadWaitStart(); break;
					case MANUAL.LDBUFFER2_CLEAN:		if (G.seqLoadBuffer2.IsReady == false) return; G.seqLoadBuffer2.CleanStart(); break;
					case MANUAL.LDBUFFER2_UNLOADWAIT:	if (G.seqLoadBuffer2.IsReady == false) return; G.seqLoadBuffer2.UnloadWaitStart(); break;

					case MANUAL.LDINSPPNP_HOME:			if (G.seqLoadInspPnP.IsReady == false) return; G.seqLoadInspPnP.HomeStart(); break;
					case MANUAL.LDINSPPNP_PICK:			if (G.seqLoadInspPnP.IsReady == false) return; G.seqLoadInspPnP.ManualLoadBufSet((int)SM.SHM.GetManualParam(0)); G.seqLoadInspPnP.PickStart(); break; //pick - 1: loadbuffer1 2: loadbuffer2   - 실제 시퀀스에서 사용할떈 0/1 이므로 -1 추가 
					case MANUAL.LDINSPPNP_PLACE:		if (G.seqLoadInspPnP.IsReady == false) return; G.seqLoadInspPnP.ManualInspPlateSet((int)SM.SHM.GetManualParam(1)); G.seqLoadInspPnP.PlaceStart(); break; //place - 1: inspplate1 2: inspplate2  - 실제 시퀀스에서 사용할떈 0/1 이므로 -1 추가  
					case MANUAL.LDINSPPNP_PNP:			if (G.seqLoadInspPnP.IsReady == false) return; G.seqLoadInspPnP.ManualLoadBufSet((int)SM.SHM.GetManualParam(0)); G.seqLoadInspPnP.ManualInspPlateSet((int)SM.SHM.GetManualParam(1)); G.seqLoadInspPnP.PnPStart(); break;

					case MANUAL.INSPPLATE1_HOME:		if (G.seqInspPlate1.IsReady == false) return; G.seqInspPlate1.HomeStart(); break;
					case MANUAL.INSPPLATE1_LOADWAIT:	if (G.seqInspPlate1.IsReady == false) return; G.seqInspPlate1.LoadWaitStart(); break;
					case MANUAL.INSPPLATE1_UNLOADWAIT:	if (G.seqInspPlate1.IsReady == false) return; G.seqInspPlate1.UnloadWaitStart(); break;
					case MANUAL.INSPPLATE1_INSPWAIT:	if (G.seqInspPlate1.IsReady == false) return; G.seqInspPlate1.InspWaitStart(); break;

					case MANUAL.INSPPLATE2_HOME:		if (G.seqInspPlate2.IsReady == false) return; G.seqInspPlate2.HomeStart(); break;
					case MANUAL.INSPPLATE2_LOADWAIT:	if (G.seqInspPlate2.IsReady == false) return; G.seqInspPlate2.LoadWaitStart(); break;
					case MANUAL.INSPPLATE2_UNLOADWAIT:	if (G.seqInspPlate2.IsReady == false) return; G.seqInspPlate2.UnloadWaitStart(); break;
					case MANUAL.INSPPLATE2_INSPWAIT:	if (G.seqInspPlate2.IsReady == false) return; G.seqInspPlate2.InspWaitStart(); break;

					case MANUAL.ULDINSPPNP_HOME:		if (G.seqUnloadInspPnP.IsReady == false) return; G.seqUnloadInspPnP.HomeStart(); break;
					case MANUAL.ULDINSPPNP_PICK:		if (G.seqUnloadInspPnP.IsReady == false) return; G.seqUnloadInspPnP.ManualInspPlateSet((int)SM.SHM.GetManualParam(1)); G.seqUnloadInspPnP.PickStart(); break; //place - 1: inspplate1 2: inspplate2 - 실제 시퀀스에서 사용할떈 0/1 이므로 -1 추가  
					case MANUAL.ULDINSPPNP_PLACE:		if (G.seqUnloadInspPnP.IsReady == false) return; G.seqUnloadInspPnP.PlaceStart(); break;
					case MANUAL.ULDINSPPNP_PNP:			if (G.seqUnloadInspPnP.IsReady == false) return; G.seqUnloadInspPnP.ManualInspPlateSet((int)SM.SHM.GetManualParam(0)); G.seqUnloadInspPnP.PnPStart(); break;

					case MANUAL.ULDBUFFER_HOME:			if (G.seqUnloadBuffer.IsReady == false) return; G.seqUnloadBuffer.HomeStart(); break;
					case MANUAL.ULDBUFFER_LOADWAIT:		if (G.seqUnloadBuffer.IsReady == false) return; G.seqUnloadBuffer.LoadWaitStart(); break;
					case MANUAL.ULDBUFFER_UNLOADWAIT:	if (G.seqUnloadBuffer.IsReady == false) return; G.seqUnloadBuffer.UnloadWaitStart(); break;

					case MANUAL.REJECTBUFFER_HOME:		if (G.seqRejectBuffer.IsReady == false) return; G.seqRejectBuffer.HomeStart(); break;
					case MANUAL.REJECTBUFFER_READY:		if (G.seqRejectBuffer.IsReady == false) return; G.seqRejectBuffer.ReadyStart(); break;
					case MANUAL.REJECTBUFFER_CHANGE:	if (G.seqRejectBuffer.IsReady == false) return; G.seqRejectBuffer.TrayChangeStart(); break;

					case MANUAL.UNLOADPNP_HOME:			if (G.seqUnloadPnP.IsReady == false) return; G.seqUnloadPnP.HomeStart(); break;
					case MANUAL.UNLOADPNP_PICKBUFFER:	if (G.seqUnloadPnP.IsReady == false) return; G.seqUnloadPnP.PickBufStart(); break;
					case MANUAL.UNLOADPNP_PICKREJECT:	if (G.seqUnloadPnP.IsReady == false) return; G.seqUnloadPnP.PickRejectStart(true); break;
					case MANUAL.UNLOADPNP_PLACE:		if (G.seqUnloadPnP.IsReady == false) return; G.seqUnloadPnP.PlaceStart(); break;
					case MANUAL.UNLOADPNP_PNP:			if (G.seqUnloadPnP.IsReady == false) return; G.seqUnloadPnP.PnPStart(); break;

					case MANUAL.UNLOADTABLE1_HOME:		if (G.seqUnloadTrayTable1.IsReady == false) return; G.seqUnloadTrayTable1.HomeStart(); break;
					case MANUAL.UNLOADTABLE1_UP:		if (G.seqUnloadTrayTable1.IsReady == false) return; G.seqUnloadTrayTable1.UpStart(SeqUnloadTrayTable.UDMode.Manual); break;
					case MANUAL.UNLOADTABLE1_DOWN:		if (G.seqUnloadTrayTable1.IsReady == false) return; G.seqUnloadTrayTable1.DownStart(SeqUnloadTrayTable.UDMode.Manual); break;
					case MANUAL.UNLOADTABLE1_GRIP:		if (G.seqUnloadTrayTable1.IsReady == false) return; G.seqUnloadTrayTable1.GripStart(); break;
					case MANUAL.UNLOADTABLE1_RELEASE:	if (G.seqUnloadTrayTable1.IsReady == false) return; G.seqUnloadTrayTable1.ReleaseStart(); break;

					case MANUAL.UNLOADTABLE2_HOME:		if (G.seqUnloadTrayTable2.IsReady == false) return; G.seqUnloadTrayTable2.HomeStart(); break;
					case MANUAL.UNLOADTABLE2_UP:		if (G.seqUnloadTrayTable1.IsReady == false) return; G.seqUnloadTrayTable2.UpStart(SeqUnloadTrayTable.UDMode.Manual); break;
					case MANUAL.UNLOADTABLE2_DOWN:		if (G.seqUnloadTrayTable1.IsReady == false) return; G.seqUnloadTrayTable2.DownStart(SeqUnloadTrayTable.UDMode.Manual); break;
					case MANUAL.UNLOADTABLE2_GRIP:		if (G.seqUnloadTrayTable1.IsReady == false) return; G.seqUnloadTrayTable2.GripStart(); break;
					case MANUAL.UNLOADTABLE2_RELEASE:	if (G.seqUnloadTrayTable1.IsReady == false) return; G.seqUnloadTrayTable2.ReleaseStart(); break;

					case MANUAL.ULDTRANS_HOME:			if (G.seqUnloadTransfer.IsReady == false) return; G.seqUnloadTransfer.HomeStart(); break;
					case MANUAL.ULDTRANS_PICK:			if (G.seqUnloadTransfer.IsReady == false) return; G.seqUnloadTransfer.PickStart((SeqUnloadTransfer.PosX)SM.SHM.GetManualParam(0)); break;
					case MANUAL.ULDTRANS_PLACE:			if (G.seqUnloadTransfer.IsReady == false) return; G.seqUnloadTransfer.PlaceStart((SeqUnloadTransfer.PosX)SM.SHM.GetManualParam(1)); break;
					case MANUAL.ULDTRANS_PLACECONV:		if (G.seqUnloadTransfer.IsReady == false) return; G.seqUnloadTransfer.PlaceConvStart(false); break;
					case MANUAL.ULDTRANS_PNP:			if (G.seqUnloadTransfer.IsReady == false) return; G.seqUnloadTransfer.PnPStart((SeqUnloadTransfer.PosX)SM.SHM.GetManualParam(0), (SeqUnloadTransfer.PosX)SM.SHM.GetManualParam(1)); break;

					case MANUAL.UNLOADER_HOME:			if (G.seqTrayUnloader.IsReady == false) return; G.seqTrayUnloader.HomeStart(); break;
					case MANUAL.UNLOADER_UNLOAD:		if (G.seqTrayUnloader.IsReady == false) return; G.seqTrayUnloader.UnloadStart(); break;
					case MANUAL.UNLOADER_LIFTUP:		if (G.seqTrayUnloader.IsReady == false) return; G.seqTrayUnloader.LiftUpStart(); break;
					case MANUAL.UNLOADER_LIFTDOWN:		if (G.seqTrayUnloader.IsReady == false) return; G.seqTrayUnloader.LiftDownStart(); break;
					case MANUAL.UNLOADER_ALIGN:			if (G.seqTrayUnloader.IsReady == false) return; G.seqTrayUnloader.AlignStart(); break;

					case MANUAL.LDTRANS_PICKCONV_TEACH: if (G.seqLoadTransfer.IsReady == false) return; G.seqLoadTransfer.PickConvStart(true); break;
					case MANUAL.ULDTRANS_PLACECONV_TEACH:
						if (G.seqUnloadTransfer.IsReady == false) return; G.seqUnloadTransfer.PlaceConvStart(true); break;

					//	TCP
					case MANUAL.SEND_RUN_STATE_REPORT:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendRunStateReport();
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendRunStateReport();
									break;
								case CommInsp.CmdDir.ToReview:
									G.commInspRef.SendRunStateReport();
									break;
							}
						}
						break;
					case MANUAL.SEND_PROD_DATA_REPORT:
						{
							FlowPart FlowInsp = ((int)SM.SHM.GetManualParam(0)) == 0 ? FlowPart.Insp1 : FlowPart.Insp2;
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(1))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendProdDataReport((int)SM.SHM.GetManualParam(0) + 1, SM.FMP[FlowInsp].GetInfo());
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendProdDataReport((int)SM.SHM.GetManualParam(0) + 1, SM.FMP[FlowInsp].GetInfo());
									break;
								case CommInsp.CmdDir.ToReview:
									G.commInspRef.SendProdDataReport((int)SM.SHM.GetManualParam(0) + 1, SM.FMP[FlowInsp].GetInfo());
									break;
							}
							break;
						}
					case MANUAL.SEND_RUN_STATE_REQUEST:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendRunStateRequest();
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendRunStateRequest();
									break;
								case CommInsp.CmdDir.ToReview:
									G.commInspRef.SendRunStateRequest();
									break;
							}
						}
						break;
					case MANUAL.SEND_AUTO_RUN_START:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendAutoRunStart();
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendAutoRunStart();
									break;
							}
						}
						break;
					case MANUAL.SEND_INSP_STATE_REQUEST:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendInspStateRequest();
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendInspStateRequest();
									break;
							}
						}
						break;
					case MANUAL.SEND_RESULT_DATA_REPORT_CONFIRM:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendResultDataReportConfirm(CommInsp.Confirm.OK);
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendResultDataReportConfirm(CommInsp.Confirm.OK);
									break;
							}
						}
						break;
					case MANUAL.SEND_REJECT_DATA_REPORT:
						{

							G.commInspRef.SendRejectDataReport((string)SM.SHM.GetManualParam(0), int.Parse(SM.SHM.GetManualParam(1).ToString()), int.Parse(SM.SHM.GetManualParam(2).ToString()), int.Parse(SM.SHM.GetManualParam(3).ToString()), int.Parse(SM.SHM.GetManualParam(4).ToString()));
						}
						break;
					case MANUAL.SEND_VERIFY_DATA_REPORT_CONFIRM:
						{
							G.commInspRef.SendVerifyDataReportConfirm(CommInsp.Confirm.OK);
						}
						break;
					case MANUAL.SEND_HOME_START:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendHomeStart();
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendHomeStart();
									break;
							}
						}
						break;
					case MANUAL.SEND_JOBCHECK:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendJobCheckRequest(SM.SHM.GetManualParam(1).ToString());
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendJobCheckRequest(SM.SHM.GetManualParam(1).ToString());
									break;
							}
						}
						break;
					case MANUAL.SEND_ALARM_REPORT:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendAlarmReport((int)ALM.UNKNOWN);
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendAlarmReport((int)ALM.UNKNOWN);
									break;
							}
						}
						break;
					case MANUAL.SEND_REFERENCE_START:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendReferenceStart((int)SM.SHM.GetManualParam(1));
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendReferenceStart((int)SM.SHM.GetManualParam(1));
									break;
							}
						}
						break;
					case MANUAL.SEND_VERIFY_DATA_REPORT_END_CONFIRM:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToReview:
									G.commInspRef.SendVerifyDataReportEndConfirm(CommInsp.Confirm.OK);
									break;
							}
						}
						break;
					case MANUAL.SEND_PAUSE_START:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendPauseStart(2);
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendPauseStart(2);
									break;
							}
						}
						break;
					case MANUAL.SEND_ALARM_CLEAR:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendAlarmClear();
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendAlarmClear();
									break;
							}
						}
						break;
					case MANUAL.SEND_YIELD_CLEAR:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.SendYieldClear();
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.SendYieldClear();
									break;
							}
						}
						break;
					case MANUAL.RESET_RCV_DATA_ALL:
						{
							switch ((CommInsp.CmdDir)SM.SHM.GetManualParam(0))
							{
								case CommInsp.CmdDir.ToTop:
									G.commInspTop.ResetRcvDataAll();
									break;
								case CommInsp.CmdDir.ToBtm:
									G.commInspBtm.ResetRcvDataAll();
									break;
								case CommInsp.CmdDir.ToReview:
									G.commInspRef.ResetRcvDataAll();
									break;
							}
						}
						break;

					case MANUAL.FrontBarcodeScanStart:
						G.LoadLPCAMMBarcode.TriggerOn(); break;
					case MANUAL.FrontBarcodeScanStop:
						G.LoadLPCAMMBarcode.TriggerOff();
						SM.SHM.SetMsg(SeqMsg.ManualBarcode, G.LoadLPCAMMBarcode.GetResultData());
						SM.SHM.MsgShow(SeqMsg.ManualBarcode);
						break;
					case MANUAL.FrontBarcodeConnect: G.LoadLPCAMMBarcode.SystemConnect(); break;

					case MANUAL.SideBarcodeScanStart:
						G.LoadOESBarcode.TriggerOn(); break;
					case MANUAL.SideBarcodeScanStop:
						G.LoadOESBarcode.TriggerOff();
						SM.SHM.SetMsg(SeqMsg.ManualBarcode, G.LoadOESBarcode.GetResultData());
						SM.SHM.MsgShow(SeqMsg.ManualBarcode);
						break;
					case MANUAL.SideBarcodeConnect: G.LoadOESBarcode.SystemConnect(); break;

					case MANUAL.Unload1BarcodeScanStart:
						G.UnloadOESBarcode.TriggerOn(); break;
					case MANUAL.Unload1BarcodeScanStop:
						G.UnloadOESBarcode.TriggerOff();
						SM.SHM.SetMsg(SeqMsg.ManualBarcode, G.UnloadOESBarcode.GetResultData());
						SM.SHM.MsgShow(SeqMsg.ManualBarcode);
						break;
					case MANUAL.Unload1BarcodeConnect: G.UnloadOESBarcode.SystemConnect(); break;
					case MANUAL.Unload2BarcodeScanStart:
						G.UnloadLPCAMMBarcode.TriggerOn(); break;
					case MANUAL.Unload2BarcodeScanStop:
						G.UnloadLPCAMMBarcode.TriggerOff();
						SM.SHM.SetMsg(SeqMsg.ManualBarcode, G.UnloadLPCAMMBarcode.GetResultData());
						SM.SHM.MsgShow(SeqMsg.ManualBarcode);
						break;
					case MANUAL.Unload2BarcodeConnect: G.UnloadOESBarcode.SystemConnect(); break;
						
					case MANUAL.Reject1BarcodeScanStart:
						G.RejectOESBarcode.TriggerOn(); break;
					case MANUAL.Reject1BarcodeScanStop:
						G.RejectOESBarcode.TriggerOff();
						SM.SHM.SetMsg(SeqMsg.ManualBarcode, G.RejectOESBarcode.GetResultData());
						SM.SHM.MsgShow(SeqMsg.ManualBarcode);
						break;
					case MANUAL.Reject1BarcodeConnect: G.RejectOESBarcode.SystemConnect(); break;

					case MANUAL.Reject2BarcodeScanStart:
						G.RejectLPCAMMBarcode.TriggerOn(); break;
					case MANUAL.Reject2BarcodeScanStop:
						G.RejectLPCAMMBarcode.TriggerOff();
						SM.SHM.SetMsg(SeqMsg.ManualBarcode, G.RejectLPCAMMBarcode.GetResultData());
						SM.SHM.MsgShow(SeqMsg.ManualBarcode);
						break;
					case MANUAL.Reject2BarcodeConnect: G.RejectLPCAMMBarcode.SystemConnect(); break;
					case MANUAL.SmartVision1Reconnect: G.commFTVLoader.ConnectReTry(); break;
					case MANUAL.SmartVision2Reconnect: G.commFTVLoader.ConnectReTry(); break;
					
					case MANUAL.LoadPnP_AllUp:
						{
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.PnPUpDn1, SenPos.PICKER_UP);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.PnPUpDn2, SenPos.PICKER_UP);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.PnPUpDn3, SenPos.PICKER_UP);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.PnPUpDn4, SenPos.PICKER_UP);
						}
						break;
					case MANUAL.LoadPnP_AllDown:
						{
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.PnPUpDn1, SenPos.PICKER_DOWN);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.PnPUpDn2, SenPos.PICKER_DOWN);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.PnPUpDn3, SenPos.PICKER_DOWN);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.PnPUpDn4, SenPos.PICKER_DOWN);
						}
						break;
					case MANUAL.LoadPnP_AllGrip:
						{
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.Vac1, SenPos.VAC);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.Vac2, SenPos.VAC);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.Vac3, SenPos.VAC);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.Vac4, SenPos.VAC);
						}
						break;
					case MANUAL.LoadPnP_AllRelease:
						{
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.Vac1, SenPos.BLOW);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.Vac2, SenPos.BLOW);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.Vac3, SenPos.BLOW);
							G.seqLoadPnP.ActPos(SeqLoadPnP.Act.Vac4, SenPos.BLOW);
						}
						break;
                    case MANUAL.LoadPnP_AllUpDn:
                        {
                            G.seqLoadPnP.Reverse(SeqLoadPnP.Act.PnPUpDn1);
                            G.seqLoadPnP.Reverse(SeqLoadPnP.Act.PnPUpDn2);
                            G.seqLoadPnP.Reverse(SeqLoadPnP.Act.PnPUpDn3);
                            G.seqLoadPnP.Reverse(SeqLoadPnP.Act.PnPUpDn4);
                        }
                        break;
                    case MANUAL.LoadPnP_Unload_AllUpDn:
                        {
                            G.seqLoadBuffer1.Reverse(SeqLoadBuffer.Act.CleanAirMove);
                            G.seqLoadBuffer2.Reverse(SeqLoadBuffer.Act.CleanAirMove);
                        }
                        break;


                    case MANUAL.UnloadPnP_AllUp:
						{
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.PnPUpDn1, SenPos.PICKER_UP);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.PnPUpDn2, SenPos.PICKER_UP);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.PnPUpDn3, SenPos.PICKER_UP);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.PnPUpDn4, SenPos.PICKER_UP);
						}
						break;
					case MANUAL.UnloadPnP_AllDown:
						{

							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.PnPUpDn1, SenPos.PICKER_DOWN);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.PnPUpDn2, SenPos.PICKER_DOWN);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.PnPUpDn3, SenPos.PICKER_DOWN);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.PnPUpDn4, SenPos.PICKER_DOWN);
						}
						break;
					case MANUAL.UnloadPnP_AllGrip:
						{

							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.Vac1, SenPos.VAC);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.Vac2, SenPos.VAC);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.Vac3, SenPos.VAC);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.Vac4, SenPos.VAC);
						}
						break;
					case MANUAL.UnloadPnP_AllRelease:
						{

							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.Vac1, SenPos.BLOW);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.Vac2, SenPos.BLOW);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.Vac3, SenPos.BLOW);
							G.seqUnloadPnP.ActPos(SeqUnloadPnP.Act.Vac4, SenPos.BLOW);
						}
						break;
					case MANUAL.UnloadPnP_AllUpDn:
						{
							G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.PnPUpDn1);
							G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.PnPUpDn2);
							G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.PnPUpDn3);
							G.seqUnloadPnP.Reverse(SeqUnloadPnP.Act.PnPUpDn4);
						}
						break;

					case MANUAL.LoadInspPnP_AllUp:
						{
							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.PnPUpDn, SenPos.PICKER_UP);
						}
						break;
					case MANUAL.LoadInspPnP_AllDown:
						{
							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.PnPUpDn, SenPos.PICKER_DOWN);
						}
						break;
					case MANUAL.LoadInspPnP_AllGrip:
						{
							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.Vac1, SenPos.VAC);
							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.Vac2, SenPos.VAC);
							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.Vac3, SenPos.VAC);
							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.Vac4, SenPos.VAC);

						}
						break;
					case MANUAL.LoadInspPnP_AllRelease:
						{

							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.Vac1, SenPos.BLOW);
							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.Vac2, SenPos.BLOW);
							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.Vac3, SenPos.BLOW);
							G.seqLoadInspPnP.ActPos(SeqLoadInspPnP.Act.Vac4, SenPos.BLOW);
						}
						break;
					case MANUAL.LoadInspPnP_AllUpDn:
						{
							G.seqLoadInspPnP.Reverse(SeqLoadInspPnP.Act.PnPUpDn);
						}
						break;


					case MANUAL.UnloadInspPnP_AllUp:
						{
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.PnPUpDn, SenPos.PICKER_UP);
						}
						break;
					case MANUAL.UnloadInspPnP_AllDown:
						{
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.PnPUpDn, SenPos.PICKER_DOWN);
						}
						break;
					case MANUAL.UnloadInspPnP_AllGrip:
						{
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.Vac1, SenPos.VAC);
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.Vac2, SenPos.VAC);
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.Vac3, SenPos.VAC);
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.Vac4, SenPos.VAC);
						}
						break;
					case MANUAL.UnloadInspPnP_AllRelease:
						{
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.Vac1, SenPos.BLOW);
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.Vac2, SenPos.BLOW);
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.Vac3, SenPos.BLOW);
							G.seqUnloadInspPnP.ActPos(SeqUnloadInspPnP.Act.Vac4, SenPos.BLOW);
						}
						break;
					case MANUAL.UnloadInspPnP_AllUpDn:
						{
							G.seqUnloadInspPnP.Reverse(SeqUnloadInspPnP.Act.PnPUpDn);
						}
						break;

					case MANUAL.ALL_TABLE_UP:
						{
							if (G.seqLoadTrayTable1.IsReady == true && G.seqLoadTrayTable1.IsUpReady() == true && G.seqLoadTrayTable1.IsUpState() == false)			G.seqLoadTrayTable1.UpStart(SeqLoadTrayTable1.UDMode.Manual);
							if (G.seqLoadTrayTable2.IsReady == true && G.seqLoadTrayTable2.IsUpReady() == true && G.seqLoadTrayTable2.IsUpState() == false)			G.seqLoadTrayTable2.UpStart(SeqLoadTrayTable2.UDMode.Manual);
							if (G.seqUnloadTrayTable1.IsReady == true && G.seqUnloadTrayTable1.IsUpReady() == true && G.seqUnloadTrayTable1.IsUpState() == false)	G.seqUnloadTrayTable1.UpStart(SeqUnloadTrayTable.UDMode.Manual);
							if (G.seqUnloadTrayTable2.IsReady == true && G.seqUnloadTrayTable2.IsUpReady() == true && G.seqUnloadTrayTable2.IsUpState() == false)	G.seqUnloadTrayTable2.UpStart(SeqUnloadTrayTable.UDMode.Manual);
						}
						break;

					//	UBIGEM
					case MANUAL.SEND_UBIGEM_Open_Lid_Request:
						{
							G.commUbiGEM.SendOpenLidRequest("TEST_TRAYID", "TEST_LIDID");
						}
						break;
					case MANUAL.SEND_UBIGEM_Inspection_Request:
						{
							G.commUbiGEM.SendInspectionRequest("TEST_WORKORDER", "TEST_SN");
						}
						break;
					case MANUAL.SEND_UBIGEM_Inspection_Result:
						{
							G.commUbiGEM.SendInspectionResult(FlowPart.ULDBuffer);
						}
						break;
					case MANUAL.SEND_UBIGEM_Tray_Validation:
						{
//							G.commUbiGEM.SendTrayValidation(FlowTray.CapCover);
						}
						break;
					case MANUAL.SEND_UBIGEM_Lot_End:
						{
							G.commUbiGEM.SendLotEnd();
						}
						break;
					case MANUAL.SEND_UBIGEM_ControlStateChanged:
						{
							G.commUbiGEM.SendControlStateChanged(UbiGEM.Net.Structure.ControlState.OnlineLocal);
						}
						break;
					case MANUAL.SEND_UBIGEM_ProcessStateChanged:
						{
							G.commUbiGEM.SendProcessStateChanged(1);
						}
						break;
					case MANUAL.SEND_UBIGEM_Alarm_Set:
						{
							G.commUbiGEM.SendAlarmSet(ALM.ERROR_EMO);
						}
						break;
					case MANUAL.SEND_UBIGEM_Alarm_Clear:
						{
							G.commUbiGEM.SendAlarmClear(ALM.ERROR_EMO);
						}
						break;
					default:
						break;
				}


				bRunning = false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#region [ Public Functions ]
		public void Init()
		{
			try
			{
				smmRun.Next(stRun.START);
				Start();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		public void Start()
		{
			try
			{
				SM.SHM.ClearManualNum();
				smmRun.eInput = InState.run;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}

		}
		public void Stop()
		{
			try
			{
				SM.SHM.ClearManualNum();
				smmRun.Next(stRun.END);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public bool IsRunning()
		{
			try
			{
				if (smmRun.eInput == InState.run)			return true;
				else										return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}

		}
		#endregion
		#region [Function]

		private void SetBitMoveSafety()
		{
			try
			{
				if (G.seqLoadTransfer.TrayCleanUpSafety() == false ||
					G.seqUnloadTransfer.TrayCleanUpSafety() == false)
				{
					SM.SHM.SetBitData(BIT.TrayCleanUpSafety, false);
				}
				else
				{
					SM.SHM.SetBitData(BIT.TrayCleanUpSafety, true);
				}
				
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion
	}
}
