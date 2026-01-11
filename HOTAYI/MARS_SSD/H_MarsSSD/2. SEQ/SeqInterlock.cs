using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
	public class SeqInterlock
	{
		private	PStopwatch	m_StartDelay	= new PStopwatch();

		public	void	Run()
		{
			try
			{
				ItkEMO();
				ItkKeyLock();
				ItkLoadLift();
				ItkUnloadLift();
				ItkCollision();
				ItkSwitch();
				ItkLoadPnP();
				ItkUnloadPnP();
				ItkLoadInspPnP();
				ItkUnloadInspPnP();
				ItkLoadTransfer();
				ItkUnloadTransfer();
				ItkInspPlate();
				ItkLoadBuffer();
				ItkRejectBuffer();
				ItkDustControlAlarm();
				ItkIonizerBlowAlarm();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkEMO()
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.EMO) == true  ||
					SM.SHM.IsBitData(BIT.DoorOpen) == true)
				{
					if (SM.SHM.IsBitData(BIT.AutoRun) == true)			G.seqMain.AutoRunStop();
					if (SM.SHM.IsBitData(BIT.AllHoming) == true)		G.allHome.Stop();
				}

				if (SM.SHM.IsBitData(BIT.EMO) == true)
				{
					G.seqMain.ManualRunStop();
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkKeyLock()
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.Safety) == false)
				{
					SM.SHM.SetBitData(BIT.KeyLockAuto, false);

					return;
				}

				SM.SHM.SetBitData(BIT.KeyLockAuto, G.cKernel.IsAIn(DI.KeyLock));

				if (SM.SHM.IsBitData(BIT.RecipeAutoChanging) == true)			return;

				if (SM.SHM.IsBitData(BIT.KeyLockAuto) == false)
				{
					G.AlmMng.ADD(ALM.WARNING_KEYLOCK_MANUAL);
					if (SM.SHM.IsBitData(BIT.AutoRun) == true  ||
						SM.SHM.IsBitData(BIT.Pause) == true)			G.seqMain.AutoRunPause();
				}
				else
				{
					if (G.AlmMng.Find(ALM.WARNING_KEYLOCK_MANUAL) == true)
					{
						G.AlmMng.Del(ALM.WARNING_KEYLOCK_MANUAL);
						SM.SHM.SetBitData(BIT.WarningBuzzer, false);
					}
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkDustControlAlarm()
		{
			try
			{
				if (G.cKernel.IsAIn(DI.DustControlAlarm) == true)
				{
					G.AlmMng.ADD(ALM.ERROR_DUST_CONTROL_ALARM);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkCollision()
		{
			try
			{
				if (G.cKernel.IsBIn(DI.TrayTransCollideSen) == true)
				{
					if (NDm.Read(eNDM.ManualMotorMoveDir) == 1)
					{
						if (G.cKernel.Motor(MT.LDTrayTransX).CurOpState != OpState.Idle)
						{
							GLog.Add(string.Format("Interlock Log - ItkCollision(seqLoadTransfer)"));
							G.seqLoadTransfer.ResetStep();
						}
						if (G.cKernel.Motor(MT.ULDTrayTransX).CurOpState != OpState.Idle)
						{
							GLog.Add(string.Format("Interlock Log - ItkCollision(seqUnloadTransfer)"));
							G.seqUnloadTransfer.ResetStep();
						}
					}
					G.AlmMng.ADD(ALM.ERROR_SEQ_INTERLOCK_TRANSFER_COLLISION_SENSOR);
				}
				
				if (G.cKernel.IsBIn(DI.InspPnPCollideSen) == true)
				{
					if (NDm.Read(eNDM.ManualMotorMoveDir) == 1)
					{
						if (G.cKernel.Motor(MT.LDInspPnPX).CurOpState != OpState.Idle)
						{
							GLog.Add(string.Format("Interlock Log - ItkCollision(LDInspPnPX)"));
							G.seqLoadInspPnP.ResetStep();
						}
						if (G.cKernel.Motor(MT.ULDInspPnPX).CurOpState != OpState.Idle)
						{
							GLog.Add(string.Format("Interlock Log - ItkCollision(ULDInspPnPX)"));
							G.seqUnloadInspPnP.ResetStep();
						}
					}
					G.AlmMng.ADD(ALM.ERROR_SEQ_INTERLOCK_LOADINSP_PnP_COLLISION_SENSOR);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkLoadLift()
		{
			try
            {
                if (G.DryRunIsBIn(DI.LDTrayLiftAreaSen1, false) == true &&
                    G.cKernel.Motor(MT.LDTrayLiftZ).CurOpState != OpState.Idle  &&
					G.seqTrayLoader.IsReady == true)
                {
                    G.seqTrayLoader.ResetLiftStep();
                    G.AlmMng.ADD(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR1_CHECK);
                }

                if (G.cKernel.IsAIn(DI.LDTrayLiftDecSen) == false &&
                    G.cKernel.IsAIn(DI.LDTrayLiftExistSen) == false &&
                    G.DryRunIsBIn(DI.LDTrayLiftAreaSen2, false) == true &&
                    G.cKernel.Motor(MT.LDTrayLiftZ).CurOpState != OpState.Idle)
                {
                    G.seqTrayLoader.ResetLiftStep();
                    G.AlmMng.ADD(ALM.ERROR_SEQ_LOADER_LIFT_AREA_SENSOR2_CHECK);
                }
            }
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkUnloadLift()
		{
			try
            {
                if (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen1, false) == true &&
                    G.cKernel.Motor(MT.ULDTrayLiftZ).CurOpState != OpState.Idle)
                {
                    G.seqTrayUnloader.ResetLiftStep();
                    G.AlmMng.ADD(ALM.ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR1_CHECK);
                }

                if (G.cKernel.IsAIn(DI.ULDTrayLiftExistSen) == false &&
                    (G.DryRunIsBIn(DI.ULDTrayLiftAreaSen1, false) == true || G.DryRunIsBIn(DI.ULDTrayLiftAreaSen2, false) == true) &&
                    G.cKernel.Motor(MT.ULDTrayLiftZ).CurOpState != OpState.Idle)
                {
                    G.seqTrayUnloader.ResetLiftStep();
                    G.AlmMng.ADD(ALM.ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR2_CHECK);
                }
            }
            catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkSwitch()
		{
			try
			{
				if		(G.cKernel.IsAIn(DI.PAUSE) == true)
				{
					G.cKernel.AOut(DO.StopBtnLamp,	true);
					SM.SHM.SetButtonNum(BUTTON.PAUSE);
				}
				else if (G.cKernel.IsAIn(DI.START) == true  &&
						 (SM.SHM.IsBitData(BIT.KeyLockAuto) == true  ||  SM.SHM.GetUseSkipData(USESKIP.Safety) == false))
				{
					G.cKernel.AOut(DO.StartBtnLamp,	true);

					if ( SM.SHM.IsBitData(BIT.Error) == true)			return;

					NDm.Write(eNDM.AutoRunSWDown, (int)m_StartDelay.ElapsedMilliseconds);

					if		(m_StartDelay.IsRunning == false)
					{
						m_StartDelay.Restart();
					}
					else if (m_StartDelay.IsTimeOverMs(1000) == true  &&  SM.SHM.IsBitData(BIT.AutoRun) == false )
					{
						if (SM.SHM.IsBitData(BIT.AllHome) == false)			G.AlmMng.ADD(ALM.ERROR_SEQ_ALLHOME);
						else												SM.SHM.SetButtonNum(BUTTON.START);
					}
				}
				else
				{
					if (m_StartDelay.IsRunning == true)
					{
						m_StartDelay.Stop();
						m_StartDelay.Reset();
						NDm.Write(eNDM.AutoRunSWDown, (int)m_StartDelay.ElapsedMilliseconds);
					}
					else if (NDm.Read(eNDM.AutoRunSWDown) != 0)
					{
						m_StartDelay.Reset();
						NDm.Write(eNDM.AutoRunSWDown, 0);
					}

					G.cKernel.AOut(DO.StartBtnLamp,	false);
					G.cKernel.AOut(DO.StopBtnLamp,	false);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkLoadPnP()
		{
			try
			{
				if (G.cKernel.Motor(MT.LDPnPZ).IsHomeDone() == false  ||
					 G.seqLoadPnP.CheckCylUpDown(SenPos.PICKER_UP) == false)
				{ 
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.LDPnPX).CurOpState != OpState.Idle)
                    {
                        if (G.cKernel.Motor(MT.LDPnPZ).IsHomeDone() == false)
                        {
                            GLog.Add(string.Format("Interlock Log - ItkLoadPnP(LDPnPX) - LDPnPZ"));
                        }
                        if (G.seqLoadPnP.CheckCylUpDown(SenPos.PICKER_UP) == false)
                        {
                            GLog.Add(string.Format("Interlock Log - ItkLoadPnP(LDPnPX) - CheckCylUpDown"));
                        }
                        GLog.Add(string.Format("Interlock Log - ItkLoadPnP(LDPnPX)"));
						G.cKernel.Motor(MT.LDPnPX).Stop();
					}
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.LDPnPY).CurOpState != OpState.Idle)
                    {
                        if (G.cKernel.Motor(MT.LDPnPZ).IsHomeDone() == false)
                        {
                            GLog.Add(string.Format("Interlock Log - ItkLoadPnP(LDPnPY) - LDPnPZ"));
                        }
                        if (G.seqLoadPnP.CheckCylUpDown(SenPos.PICKER_UP) == false)
                        {
                            GLog.Add(string.Format("Interlock Log - ItkLoadPnP(LDPnPY) - CheckCylUpDown"));
                        }
                        GLog.Add(string.Format("Interlock Log - ItkLoadPnP(LDPnPY)"));
						G.cKernel.Motor(MT.LDPnPY).Stop();
					}
					SM.SHM.InterlockMsgMotAdd(MT.LDPnPX, "Please Check the Load PnP Z Axis Position and Up/Down Cylinder status");
					SM.SHM.InterlockMsgMotAdd(MT.LDPnPY, "Please Check the Load PnP Z Axis Position and Up/Down Cylinder status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.LDPnPX);
					SM.SHM.InterlockMsgMotReset(MT.LDPnPY);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkUnloadPnP()
		{
			try
			{
				if (G.cKernel.Motor(MT.ULDPnPZ).IsHomeDone() == false  ||
					(G.cKernel.Motor(MT.ULDPnPZ).IsBiggerThan((int)SeqUnloadPnP.PosZ.READY) == true  &&
					 G.seqUnloadPnP.CheckCylUpDown(SenPos.PICKER_UP) == false))
				{ 
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.ULDPnPX).CurOpState != OpState.Idle)
					{
						G.cKernel.Motor(MT.ULDPnPX).Stop();
						GLog.Add(string.Format("Interlock Log - ItkUnloadPnP(ULDPnPX -4)"));
					}
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.ULDPnPY).CurOpState != OpState.Idle)
					{
						G.cKernel.Motor(MT.ULDPnPY).Stop();
						GLog.Add(string.Format("Interlock Log - ItkUnloadPnP(ULDPnPY)"));
					}
					SM.SHM.InterlockMsgMotAdd(MT.ULDPnPX, "Please Check the Unload PnP Z Axis Position and Up/Down Cylinder status");
					SM.SHM.InterlockMsgMotAdd(MT.ULDPnPY, "Please Check the Unload PnP Z Axis Position and Up/Down Cylinder status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.ULDPnPX);
					SM.SHM.InterlockMsgMotReset(MT.ULDPnPY);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkLoadInspPnP()
		{
			try
			{
				if (G.cKernel.Motor(MT.LDInspPnPZ).IsHomeDone() == false  ||
					(G.cKernel.Motor(MT.LDInspPnPZ).IsBiggerThan((int)SeqLoadInspPnP.PosZ.READY) == true  ||
					 G.seqLoadInspPnP.CheckCylUpDown(SenPos.PICKER_UP) == false))
				{ 
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.LDInspPnPX).CurOpState != OpState.Idle)
					{
						GLog.Add(string.Format("Interlock Log - ItkLoadInspPnP(LDInspPnPX)"));
						G.cKernel.Motor(MT.LDInspPnPX).Stop();
					}
					SM.SHM.InterlockMsgMotAdd(MT.LDInspPnPX, "Please Check the Load Insp PnP Z Axis Position and Up/Down Cylinder status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.LDInspPnPX);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkUnloadInspPnP()
		{
			try
			{
				if (G.cKernel.Motor(MT.ULDInspPnPZ).IsHomeDone() == false  ||
					(G.cKernel.Motor(MT.ULDInspPnPZ).IsBiggerThan((int)SeqUnloadInspPnP.PosZ.READY) == true  ||
					 G.seqUnloadInspPnP.CheckCylUpDown(SenPos.PICKER_UP) == false))
				{ 
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.ULDInspPnPX).CurOpState != OpState.Idle)
					{
						GLog.Add(string.Format("Interlock Log - ItkUnloadInspPnP(ULDInspPnPX)"));
						G.cKernel.Motor(MT.ULDInspPnPX).Stop();
					}
					SM.SHM.InterlockMsgMotAdd(MT.ULDInspPnPX, "Please Check the Unload Insp PnP Z Axis Position and Up/Down Cylinder status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.ULDInspPnPX);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkLoadTransfer()
		{
			try
			{
				if (G.cKernel.Motor(MT.LDTrayTransZ).IsHomeDone() == false  ||
					G.cKernel.Motor(MT.LDTrayTransZ).IsBiggerThan((int)SeqLoadTransfer.PosZ.READY) == true)
				{ 
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.LDTrayTransX).CurOpState != OpState.Idle)
					{
						GLog.Add(string.Format("Interlock Log - ItkLoadTransfer(LDTrayTransX)"));
						G.cKernel.Motor(MT.LDTrayTransX).Stop();
					}
					
					SM.SHM.InterlockMsgMotAdd(MT.LDTrayTransX, "Please Check the Load Transfer Z Axis Position status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.LDTrayTransX);
				}

				if		(G.cKernel.Motor(MT.LDTrayTransX).IsHomeDone() == false  ||
						 G.cKernel.Motor(MT.LDTrayTransX).IsSmallerThan((int)SeqLoadTransfer.PosX.LOADCONV) == true)
				{
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.LDTrayTransZ).CurOpState != OpState.Idle  &&
						G.cKernel.Motor(MT.LDTrayTransZ).CurOpState != OpState.Home  &&
						G.cKernel.Motor(MT.LDTrayTransZ).IsRdy() == false &&
						NDm.Read(eNDM.ManualMotorMoveDir) == 1)
					{
						GLog.Add(string.Format("Interlock Log - ItkLoadTransfer(LDTrayTransZ)-1"));
						G.cKernel.Motor(MT.LDTrayTransZ).Stop();
					}

					SM.SHM.InterlockMsgMotAdd(MT.LDTrayTransZ, "Please Check that the Load Transfer X Axis Current Position is less Than Load Conveyor Postion.");
				}
				else if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true)
				{
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.LDTrayTransZ).CurOpState != OpState.Idle  &&
						G.cKernel.Motor(MT.LDTrayTransZ).IsRdy() == false)
					{
						GLog.Add(string.Format("Interlock Log - ItkLoadTransfer(LDTrayTransZ)-2"));
						G.cKernel.Motor(MT.LDTrayTransZ).Stop();
					}

					SM.SHM.InterlockMsgMotAdd(MT.LDTrayTransZ, "Please Check the Error Or Pause status");
				}
				else if (G.cKernel.IsBIn(DI.LDTrayTransSafetySen) == true  )
				{
					if (NDm.Read(eNDM.ManualMotorMoveDir) == 1)
					{
						GLog.Add(string.Format("Interlock Log - ItkLoadTransfer(LDTrayTransZ)-3"));
						G.cKernel.Motor(MT.LDTrayTransZ).Stop();
					}

					SM.SHM.InterlockMsgMotAdd(MT.LDTrayTransZ, "Please Check the Axis Z Safety Sensor");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.LDTrayTransZ);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkUnloadTransfer()
		{
			try
			{
				if (G.cKernel.Motor(MT.ULDTrayTransZ).IsHomeDone() == false  ||
					G.cKernel.Motor(MT.ULDTrayTransZ).IsBiggerThan((int)SeqUnloadTransfer.PosZ.READY) == true)
				{ 
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.ULDTrayTransX).CurOpState != OpState.Idle)
					{
						GLog.Add(string.Format("Interlock Log - ItkUnloadTransfer(ULDTrayTransX)"));
						G.cKernel.Motor(MT.ULDTrayTransX).Stop();
					}
					
					SM.SHM.InterlockMsgMotAdd(MT.ULDTrayTransX, "Please Check the Unload Transfer Z Axis Position status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.ULDTrayTransX);
				}

				if		(G.cKernel.Motor(MT.ULDTrayTransX).IsHomeDone() == false  ||
						 G.cKernel.Motor(MT.ULDTrayTransX).IsSmallerThan((int)SeqUnloadTransfer.PosX.UNLOADCONV) == true)
				{
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.ULDTrayTransZ).CurOpState != OpState.Idle  &&
						G.cKernel.Motor(MT.ULDTrayTransZ).CurOpState != OpState.Home  &&
						G.cKernel.Motor(MT.ULDTrayTransZ).IsRdy() == false  &&
						NDm.Read(eNDM.ManualMotorMoveDir) == 1)
					{
						GLog.Add(string.Format("Interlock Log - ItkLoadTransfer(ULDTrayTransZ)-1"));
						G.cKernel.Motor(MT.ULDTrayTransZ).Stop();
					}

					SM.SHM.InterlockMsgMotAdd(MT.ULDTrayTransZ, "Please Check that the Unload Transfer X Axis Current Position is less Than Unload Conveyor Postion.");
				}
				else if	(SM.SHM.IsBitData(BIT.Error) == true  ||  SM.SHM.IsBitData(BIT.Pause) == true)
				{
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.ULDTrayTransZ).CurOpState != OpState.Idle)
					{
						GLog.Add(string.Format("Interlock Log - ItkUnloadTransfer(ULDTrayTransZ)-2"));
						G.cKernel.Motor(MT.ULDTrayTransZ).Stop();
					}

					SM.SHM.InterlockMsgMotAdd(MT.ULDTrayTransZ, "Please Check the Error Or Pause status");
				}
				else if (G.cKernel.IsBIn(DI.ULDTrayTransSafetySen) == true)
				{
					if (NDm.Read(eNDM.ManualMotorMoveDir) == 1)
					{
						GLog.Add(string.Format("Interlock Log - ItkUnloadTransfer(ULDTrayTransZ)-3"));
						G.cKernel.Motor(MT.ULDTrayTransZ).Stop();
					}

					SM.SHM.InterlockMsgMotAdd(MT.ULDTrayTransZ, "Please Check the Axis Z Safety Sensor");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.ULDTrayTransZ);
				}

			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkInspPlate()
		{
			try
			{
				if (G.cKernel.Motor(MT.InspPlate1Z).IsHomeDone() == false  ||
					G.cKernel.Motor(MT.InspPlate1Z).IsBiggerThan((int)SeqInspPlate.PosZ.READY) == true)
				{ 
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.InspPlate1Y).CurOpState != OpState.Idle)
					{
						GLog.Add(string.Format("Interlock Log - ItkInspPlate(InspPlate1Z)"));
						G.cKernel.Motor(MT.InspPlate1Y).Stop();
					}
					
					SM.SHM.InterlockMsgMotAdd(MT.InspPlate1Y, "Please Check the Insp Plate1 Z Axis Position status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.InspPlate1Y);
				}

				
				if (G.cKernel.Motor(MT.InspPlate2Z).IsHomeDone() == false  ||
					G.cKernel.Motor(MT.InspPlate2Z).IsBiggerThan((int)SeqInspPlate.PosZ.READY) == true)
				{ 
					if (SM.SHM.IsBitData(BIT.MotorTeachingMode) == false  &&
						G.cKernel.Motor(MT.InspPlate2Y).CurOpState != OpState.Idle)
					{
						GLog.Add(string.Format("Interlock Log - ItkInspPlate(InspPlate2Z)"));
						G.cKernel.Motor(MT.InspPlate2Y).Stop();
					}
					
					SM.SHM.InterlockMsgMotAdd(MT.InspPlate2Y, "Please Check the  Insp Plate2 ZAxis Position status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.InspPlate2Y);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkLoadBuffer()
		{
			try
			{
				if (G.cKernel.IsAIn(DI.LDBuf1DuctDnCyl) == false)
				{
					if (G.cKernel.Motor(MT.LDBuf1Y).CurOpState != OpState.Idle)
					{
						GLog.Add(string.Format("Interlock Log - ItkLoadBuffer(LDBuf1Y)"));
						G.cKernel.Motor(MT.LDBuf1Y).Stop();
					}
					SM.SHM.InterlockMsgMotAdd(MT.LDBuf1Y, "Please Check the Load-Buffer-Duct #1 Down status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.LDBuf1Y);
				}

				if (G.cKernel.IsAIn(DI.LDBuf2DuctDnCyl) == false)
				{
					if (G.cKernel.Motor(MT.LDBuf2Y).CurOpState != OpState.Idle)
					{
						GLog.Add(string.Format("Interlock Log - ItkLoadBuffer(LDBuf2Y)"));
						G.cKernel.Motor(MT.LDBuf2Y).Stop();
					}
					SM.SHM.InterlockMsgMotAdd(MT.LDBuf2Y, "Please Check the Load-Buffer-Duct #2 Down status");
				}
				else
				{
					SM.SHM.InterlockMsgMotReset(MT.LDBuf2Y);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkRejectBuffer()
		{
			try
			{

			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ItkIonizerBlowAlarm()
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
