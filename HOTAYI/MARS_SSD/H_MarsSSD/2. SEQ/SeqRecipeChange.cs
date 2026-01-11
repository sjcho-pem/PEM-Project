using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
	public class SeqRecipeChange
	{
		public enum Cmd
		{
			START	= 10,
			END		= 99,
			END_OK	= 97,
			END_NG	= 98,

			RCPCHG_READY_10 = 100,
			RCPCHG_READY_20,
			RCPCHG_READY_30,
			RCPCHG_READY_40,
			RCPCHG_READY_50,
			RCPCHG_REMOVE_10,
			RCPCHG_REMOVE_20,
			RCPCHG_REMOVE_30,
			RCPCHG_REMOVE_40,
			RCPCHG_REMOVE_50,
			RCPCHG_REMOVE_60,
			RCPCHG_CHANGE,
			RCPCHG_ALLHOME_10,
			RCPCHG_ALLHOME_20,
			RCPCHG_ALLHOME_30,
			RCPCHG_ALLHOME_40,
			RCPCHG_ALLHOME_50,
			RCPCHG_ALLHOME_60,
			RCPCHG_ALLHOME_70,


			REJECT_UNLOAD_10,
			REJECT_UNLOAD_20,
			REJECT_UNLOAD_30,
			REJECT_UNLOAD_40,
			REJECT_UNLOAD_50,
			REJECT_UNLOAD_60,
			REJECT_UNLOAD_70,
			REJECT_UNLOAD_80,
			REJECT_UNLOAD_90,
			REJECT_UNLOAD_100,
			REJECT_UNLOAD_110,
			REJECT_UNLOAD_120,
			REJECT_UNLOAD_130,
			REJECT_UNLOAD_140,
			REJECT_UNLOAD_150,

			BYPASS_10,
			BYPASS_20,
			BYPASS_30,
			BYPASS_40,
			BYPASS_50,
			BYPASS_60,
			BYPASS_70,
			BYPASS_80,
			BYPASS_90,
		}

		private	SMM<Cmd>	m_smmRcpChg			= new SMM<Cmd>(Cmd.START, "RECIPE CHANGE");
		private	SMM<Cmd>	m_smmRjUnload		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(teSeqID.REJECT_UNLOADALL));
		private	SMM<Cmd>	m_smmBypass			= new SMM<Cmd>(Cmd.START, "BYPASS");

		private	XML			m_XMLRcpMng			= new XML();
		private	bool		m_bManual			= false;
		private	int			m_nOldRecipeIdx		= -1;
		private	int			m_nNewRecipeIdx		= -1;

		public	void	Init()
		{
			try
			{
				m_smmRcpChg.Next(Cmd.START);
				m_smmRjUnload.Next(Cmd.START);
				m_smmBypass.Next(Cmd.START);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	RecipeChangeStart(int nOldRecipeIdx, int nNewRecipeIdx,	bool bManual)
		{								  	
			try
			{
				m_nOldRecipeIdx	= nOldRecipeIdx;
				m_nNewRecipeIdx	= nNewRecipeIdx;
				m_bManual		= bManual;
				SetSeqRun(ref m_smmRcpChg);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	RejectUnloadStart(bool bManual = false)
		{
			try
			{
				m_bManual	= bManual;
				SetSeqRun(ref m_smmRjUnload);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		public	void	Stop()
		{
			try
			{
				if (m_smmRcpChg.IsReady == false)
				{
					m_smmRcpChg.Next(Cmd.END_NG);
				}

				if (m_smmRjUnload.IsReady == false)
				{
					m_smmRjUnload.Next(Cmd.END_NG);
				}

				if (m_smmBypass.IsReady == false)
				{
					m_smmBypass.Next(Cmd.END_NG);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	IsRcpChgReady { get => m_smmRcpChg.IsReady; }

		public	bool	IsRjUnloadReady { get => m_smmRjUnload.IsReady; }

		public	bool	IsBypassReady { get => m_smmBypass.IsReady; }



		private	int		GetRecipeNoFromPartNo(string strPartNo)
		{
			try
			{	
				m_XMLRcpMng.Initializ("RecipeManager");

				int		ncnt_row	= m_XMLRcpMng.GetDataTableCount();
				int		nchg_rcp	= 0;
				string	str_part_no	= "";

				for (int i = 0; i < ncnt_row; i++)
				{
					str_part_no	= m_XMLRcpMng.ReadDataTable(i, "PartNo");
				
					if (strPartNo.ToUpper() != str_part_no.ToUpper())			continue;

					string[]	str_no	= m_XMLRcpMng.ReadDataTable(i, "Recipe").Split('.');

					if (int.TryParse(str_no[0], out nchg_rcp) == false)
					{
						return -1;
					}
					break;
				}

				if (nchg_rcp < 1)
				{
					return -2;
				}

				return nchg_rcp;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -3;
			}
		}

		private	bool	CheckRejectTrayEmpty()
		{
			try
			{
				if (SM.FMT.IsFlow(teFlowTray.Reject1) == true)			return false;
				if (SM.FMT.IsFlow(teFlowTray.Reject2) == true)			return false;
				if (SM.FMT.IsFlow(teFlowTray.RejectTrans) == true)		return false;
				if (G.cKernel.IsAIn(DI.RejectLDTableExistSen) == true)	return false;

				return true;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	CheckAllEmpty()
		{
			try
			{
				for (int i = (int)teFlowTray.LDConv01; i < (int)teFlowTray.Tmp1; i++)
				{
					if (SM.FMT.IsFlow((teFlowTray)i) == true)
					{	
						return false;
					}
				}

				if (G.cKernel.IsAIn(DI.RejectLDTableExistSen) == true)
				{
					return false;
				}

				for (int i = (int)teFlowPart.Empty; i < (int)teFlowPart.Max; i++)
				{
					if (SM.FMP.IsFlow((teFlowPart)i))
					{
						if		(SM.SHM.IsUsePart(teUsePartName.Insp1, teUsePartMode.Unload) == false  &&  ((teFlowPart)i) == teFlowPart.Insp1)			continue;
						else if (SM.SHM.IsUsePart(teUsePartName.Insp2, teUsePartMode.Unload) == false  &&  ((teFlowPart)i) == teFlowPart.Insp2)			continue;

						return false;
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	CheckAllEmptyExceptReject()
		{
			try
			{
				for (int i = (int)teFlowTray.LDConv01; i < (int)teFlowTray.Reject1; i++)
				{
					if (SM.FMT.IsFlow((teFlowTray)i) == true)
					{	
						return false;
					}
				}

				for (int i = (int)teFlowPart.Empty; i < (int)teFlowPart.Max; i++)
				{
					if (SM.FMP.IsFlow((teFlowPart)i))
					{
						if		(SM.SHM.IsUsePart(teUsePartName.Insp1, teUsePartMode.Unload) == false  &&  ((teFlowPart)i) == teFlowPart.Insp1)			continue;
						else if (SM.SHM.IsUsePart(teUsePartName.Insp2, teUsePartMode.Unload) == false  &&  ((teFlowPart)i) == teFlowPart.Insp2)			continue;

						return false;
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private	bool	CheckTrayChange(int nOldRcpIdx, int nNewRcpIdx)
		{
			try
			{
				if (SM.SHM.GetRecipeData(nOldRcpIdx, DEVICE.TrayType) != SM.SHM.GetRecipeData(nNewRcpIdx, DEVICE.TrayType)  ||
					SM.SHM.GetRecipeData(nOldRcpIdx, DEVICE.FormFactor) != SM.SHM.GetRecipeData(nNewRcpIdx, DEVICE.FormFactor)  ||
					SM.SHM.GetRecipeData(nOldRcpIdx, DEVICE.TrayColCnt) != SM.SHM.GetRecipeData(nNewRcpIdx, DEVICE.TrayColCnt)  ||
					SM.SHM.GetRecipeData(nOldRcpIdx, DEVICE.TrayRowCnt) != SM.SHM.GetRecipeData(nNewRcpIdx, DEVICE.TrayRowCnt))
				{
					return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return true;
			}
		}



		private	void	SetRejectUnloadAll()
		{
			try
			{
				if (SM.FMT.IsFlow(teFlowTray.Reject1) == true)
				{
					var info	= SM.FMT[teFlowTray.Reject1].GetInfo();
					info.ProcStatus	= (int)teProcStatus.TrayFillDone;
					SM.FMT[teFlowTray.Reject1].SetInfo(info);
				}
				if (SM.FMT.IsFlow(teFlowTray.Reject2) == true)
				{
					var info	= SM.FMT[teFlowTray.Reject2].GetInfo();
					info.ProcStatus	= (int)teProcStatus.TrayFillDone;
					SM.FMT[teFlowTray.Reject2].SetInfo(info);
				}
				if (SM.FMT.IsFlow(teFlowTray.RejectTrans) == true)
				{
					var info	= SM.FMT[teFlowTray.RejectTrans].GetInfo();
					info.ProcStatus	= (int)teProcStatus.TrayFillDone;
					SM.FMT[teFlowTray.RejectTrans].SetInfo(info);
				}

				NDm.Write(eNDM.RejectRunMode, (int)teRejectRunMode.AllUnload);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		private	bool	SetSeqRun(ref SMM<Cmd> smm)
		{
			try
			{
				if (m_smmBypass.IsReady == false  ||
					m_smmRjUnload.IsReady == false  ||
					m_smmRcpChg.IsReady == false)			return false;

				smm.eInput = InState.run;
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}



		public	void	Run()
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.Error) == true)
				{
					if (m_smmRcpChg.CurStep != Cmd.START)			m_smmRcpChg.Next(Cmd.END_NG);
					if (m_smmRjUnload.CurStep != Cmd.START)			m_smmRjUnload.Next(Cmd.END_NG);
					if (m_smmBypass.CurStep != Cmd.START)			m_smmBypass.Next(Cmd.END_NG);
				}
				
				RejecjUnload(m_smmRjUnload);
				Bypass(m_smmBypass);
				RcpChg(m_smmRcpChg);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		public	void	RejecjUnload(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								smm.Next(Cmd.REJECT_UNLOAD_10);
							}

							SM.SHM.MsgClose("RJULD");
						}
						break;

					case Cmd.REJECT_UNLOAD_10:
						{
							if (CheckRejectTrayEmpty() == false)
							{
								SetRejectUnloadAll();

								if (G.seqRejectTransfer.IsReady == false)			G.seqRejectTransfer.ResetStep();

								if		(SM.SHM.IsBitData(BIT.AutoRun) == true)
								{
									SM.SHM.MsgShow("RJULD", frmMessageBox.teButtons.OK, Language.languageDefault.MB_Info, Language.languageDefault.MB_RjTrayRemoving);

									NDm.Write(eNDM.RejectRunMode, (int)teRejectRunMode.AllUnload);
									
									smm.Next(Cmd.REJECT_UNLOAD_20);
									break;
								}
								else if (SM.SHM.IsBitData(BIT.AllHome) == true)
								{
									if (m_bManual == true)			SM.SHM.MsgShow("RJULD", frmMessageBox.teButtons.OKCancelNotClose, Language.languageDefault.MB_Info, Language.languageDefault.MB_RjTrayRemeWant);

									System.Windows.Forms.DialogResult rt	= SM.SHM.PopupMsgResult("RJULD");

									if (rt == System.Windows.Forms.DialogResult.None)			break; 

									if (rt == System.Windows.Forms.DialogResult.Cancel)
									{
										smm.Next(Cmd.END_NG);
									}

									SM.SHM.SetButtonNum(BUTTON.START);

									SM.SHM.MsgShow("RJULD", frmMessageBox.teButtons.OK, Language.languageDefault.MB_Info, Language.languageDefault.MB_RjTrayRemoving);

									NDm.Write(eNDM.RejectRunMode, (int)teRejectRunMode.AllUnload);

									smm.Next(Cmd.REJECT_UNLOAD_20);
									break;
								}
								else
								{
									SM.SHM.MsgShow("RJULD", frmMessageBox.teButtons.OKCancelNotClose, Language.languageDefault.MB_Info, Language.languageDefault.MB_RestartAfterAllHome);
									System.Windows.Forms.DialogResult rt	= SM.SHM.PopupMsgResult("RJULD");
									if (rt == System.Windows.Forms.DialogResult.None)			break;

									smm.Next(Cmd.END_NG);
								}
							}
							else
							{
								smm.Next(Cmd.END_OK);
							}
						}
				        break;

				    case Cmd.REJECT_UNLOAD_20:
				        {
							if (SM.SHM.IsBitData(BIT.AutoRun) == false)			break;

							SM.SHM.MsgShow("RJULD", frmMessageBox.teButtons.OK, Language.languageDefault.MB_Info, Language.languageDefault.MB_RjTrayRemoving);
 
							if (SM.FMT.IsFlow(teFlowTray.Reject1) == true)				break;
							if (SM.FMT.IsFlow(teFlowTray.Reject2) == true)				break;
							if (SM.FMT.IsFlow(teFlowTray.RejectTrans) == true)			break;

							if (G.cKernel.IsAIn(DI.RejectLDTableExistSen) == true  &&
								G.seqRejectLoader.IsActPos(SeqRejectLoadUnload.teAct.Move, SenPos.FWD) == false)
							{
								G.seqRejectLoader.MoveOutStart();
								break;
							}
							if (SM.FMT.IsFlowRange(teFlowTray.RejectULD01, teFlowTray.RejectULD10) == true  &&
								G.seqRejectUnloader.IsActPos(SeqRejectLoadUnload.teAct.Move, SenPos.FWD) == false)
							{
								G.seqRejectUnloader.MoveOutStart();
								break;
							}

							SM.SHM.NDmWriteAddFlag(eNDM.RejectDoorOpen, (int)teRejctDoorOpen.RejectRemove);

							smm.Next(Cmd.REJECT_UNLOAD_30);
						}
						break;

					case Cmd.REJECT_UNLOAD_30 :
						{
							SM.SHM.MsgShow("RJULD", frmMessageBox.teButtons.OK, Language.languageDefault.MB_Info, Language.languageDefault.MB_RjTrayRemove);

							if (G.cKernel.IsAIn(DI.RejectLDTableExistSen) == true)			break;
							if (G.cKernel.IsAIn(DI.RejectULDTableExistSen) == true)			break;
							if (SM.SHM.GetUseSkipData(USESKIP.Safety) == true  &&  G.cKernel.IsAIn(DI.TopRightDoorSen) == false)			break;

							SM.SHM.NDmWriteRemoveFlag(eNDM.RejectDoorOpen, (int)teRejctDoorOpen.RejectRemove);

							if (SM.SHM.GetUseSkipData(USESKIP.Safety) == true)				smm.Next(Cmd.END_OK);
							else 															smm.Next(Cmd.REJECT_UNLOAD_40);
						}
						break;

					case Cmd.REJECT_UNLOAD_40 :
						{
							if (smm.Delayms(3000) == false)			break;

							smm.Next(Cmd.END_OK);
						}
						break;

					case Cmd.END_OK:
						{
							if (m_bManual == true  &&  SM.SHM.IsBitData(BIT.AutoRun) == true)			SM.SHM.SetButtonNum(BUTTON.PAUSE);
							smm.Next(Cmd.START);
						}
						break;

					case Cmd.END_NG:
						{
							if (IsRcpChgReady == false)			m_smmRcpChg.Next(Cmd.END_NG);
							if (m_bManual == true  &&  SM.SHM.IsBitData(BIT.AutoRun) == true)			SM.SHM.SetButtonNum(BUTTON.PAUSE);
							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	Bypass(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								smm.Next(Cmd.BYPASS_10);
							}

							SM.SHM.MsgClose("BYPASS");
						}
						break;

					case Cmd.BYPASS_10:
						{
							if (CheckAllEmptyExceptReject() == false)
							{
								if		(SM.SHM.IsBitData(BIT.AutoRun) == true)
								{
									SM.SHM.MsgShow("BYPASS", frmMessageBox.teButtons.OK, Language.languageDefault.MB_Info, Language.languageDefault.MB_BypassRunning);

									NDm.Write(eNDM.AutoRunMode, (int)teAutoRunMode.Bypass);
								}
								else if (SM.SHM.IsBitData(BIT.AllHome) == true)
								{
									SM.SHM.MsgShow("BYPASS", frmMessageBox.teButtons.OKCancelNotClose, Language.languageDefault.MB_Info, Language.languageDefault.MB_BypassWant);

									System.Windows.Forms.DialogResult rt	= SM.SHM.PopupMsgResult("BYPASS");

									if (rt == System.Windows.Forms.DialogResult.None)			break; 

									if (rt == System.Windows.Forms.DialogResult.Cancel)
									{
										smm.Next(Cmd.END_NG);
									}

									SM.SHM.SetButtonNum(BUTTON.START);

									SM.SHM.MsgShow("BYPASS", frmMessageBox.teButtons.OK, Language.languageDefault.MB_Info, Language.languageDefault.MB_BypassRunning);

									NDm.Write(eNDM.AutoRunMode, (int)teAutoRunMode.Bypass);
								}
								else
								{
									SM.SHM.SetManualParams((int)SenPos.BWD);
									SM.SHM.SetManualNum(MANUAL.AllTrayGrip);

									SM.SHM.MsgShow("BYPASS", frmMessageBox.teButtons.OK, Language.languageDefault.MB_Info, Language.languageDefault.MB_RemoveAllAndHome);

									smm.Next(Cmd.BYPASS_20);
								}
							}
							else
							{
								SM.FMT.FlowClearRange(teFlowTray.LDConv01, teFlowTray.ULDConv35);
								SM.FMP.FlowClearAll();

								smm.Next(Cmd.END_OK);
							}
						}
						break;

					case Cmd.BYPASS_20 :
						{
							if (NDm.Read(eNDM.AutoRunMode) == (int)teAutoRunMode.Bypass)
							{
								break;
							}

							smm.Next(Cmd.END_OK);
						}
						break;

					case Cmd.END_OK:
						{
							if (m_bManual == true  &&  SM.SHM.IsBitData(BIT.AutoRun) == true)			SM.SHM.SetButtonNum(BUTTON.PAUSE);
							smm.Next(Cmd.START);
						}
						break;

					case Cmd.END_NG:
						{
							if (IsRcpChgReady == false)			m_smmRcpChg.Next(Cmd.END_NG);
							if (m_bManual == true  &&  SM.SHM.IsBitData(BIT.AutoRun) == true)			SM.SHM.SetButtonNum(BUTTON.PAUSE);
							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	RcpChg(SMM<Cmd> smm)
		{
			try
			{
				switch (smm.CurStep)
				{
					case Cmd.START:
						{
							if (smm.eInput == InState.run)
							{
								smm.eInput = InState.none;
								smm.Next(Cmd.RCPCHG_READY_10);
								break;
							}
							if (SM.SHM.IsBitData(BIT.RecipeAutoChanging) == true)
							{
								SM.SHM.SetBitData(BIT.RecipeAutoChanging, false);
								SM.SHM.NDmWriteRemoveFlag(eNDM.RejectDoorOpen, (int)teRejctDoorOpen.RejectRemove);
								SM.SHM.MsgClose("RCPCHG");
							}
						}
						break;

					case Cmd.RCPCHG_READY_10:
						{
							SM.SHM.SetBitData(BIT.RecipeAutoChanging, true);

							if (CheckRejectTrayEmpty() == true  &&
								CheckAllEmptyExceptReject() == true)
							{
								smm.Next(Cmd.RCPCHG_CHANGE);
							}
							else
							{
								if (CheckTrayChange(m_nOldRecipeIdx, m_nNewRecipeIdx) == true)
								{
									if (m_bManual == true)
									{
										SM.SHM.MsgShow("RCPCHG", frmMessageBox.teButtons.OKCancelNotClose, Language.languageDefault.MB_Info, Language.languageDefault.MB_WantRemoveAllAuto);

										System.Windows.Forms.DialogResult rt	= SM.SHM.PopupMsgResult("RCPCHG");

										if (rt == System.Windows.Forms.DialogResult.None)			break;
										if (rt == System.Windows.Forms.DialogResult.OK)
										{
											smm.Next(Cmd.RCPCHG_REMOVE_10);
										}
										else
										{
											smm.Next(Cmd.RCPCHG_CHANGE);
										}
									}
									else
									{
										smm.Next(Cmd.RCPCHG_REMOVE_10);
									}
								}
								else
								{
									smm.Next(Cmd.RCPCHG_CHANGE);
								}
							}

							SM.SHM.MsgClose("RCPCHG");
						}
						break;

					case Cmd.RCPCHG_REMOVE_10:
						{
							if		(SM.SHM.IsBitData(BIT.AutoRun) == false  &&
									 SM.SHM.IsBitData(BIT.AllHome) == true)
							{
								SM.SHM.SetButtonNum(BUTTON.START);
							}
							else if (SM.SHM.IsBitData(BIT.AllHome) == false)
							{
								Program.MainUI.Invoke(new System.Windows.Forms.MethodInvoker(delegate()
								{
									System.Windows.Forms.Form fc = System.Windows.Forms.Application.OpenForms["frmAllHomeMonitor"];
									if (fc != null)
									{
										if (fc.WindowState == System.Windows.Forms.FormWindowState.Minimized)
										{  // 폼을 최소화시켜 하단에 내려놓았는지 검사
											fc.WindowState = System.Windows.Forms.FormWindowState.Normal;
										}
										((frmAllHomeMonitor)fc).ResetStatus();
										fc.Activate();
									}
									else
									{
										frmAllHomeMonitor popup = new frmAllHomeMonitor();
										popup.Show();
									}
								}));

								SM.SHM.SetButtonNum(BUTTON.ALLHOME);
								smm.Next(Cmd.RCPCHG_REMOVE_20);
								break;
							}

							smm.Next(Cmd.RCPCHG_REMOVE_40);
						}
						break;

					case Cmd.RCPCHG_REMOVE_20 :
						{
							if (SM.SHM.IsBitData(BIT.AllHoming) == true)
							{
								smm.Next(Cmd.RCPCHG_REMOVE_30);
							}
						}
						break;
					
					case Cmd.RCPCHG_REMOVE_30 :
						{
							if (SM.SHM.IsBitData(BIT.AllHoming) == false)
							{
								if (SM.SHM.IsBitData(BIT.AllHome) == true)
								{
									SM.SHM.SetButtonNum(BUTTON.START);
									smm.Next(Cmd.RCPCHG_REMOVE_40);
								}
								else
								{
									smm.Next(Cmd.END_NG);
								}
							}
						}
						break;
					
					case Cmd.RCPCHG_REMOVE_40:
						{
							if (SM.SHM.IsBitData(BIT.AutoRun) == false)			break;
						
							smm.Next(Cmd.RCPCHG_REMOVE_50);
						}
						break;

					case Cmd.RCPCHG_REMOVE_50:
						{
							if (CheckRejectTrayEmpty() == false)			m_smmRjUnload.eInput	= InState.run;
							if (CheckAllEmptyExceptReject() == false)		m_smmBypass.eInput		= InState.run;
						
							smm.Next(Cmd.RCPCHG_REMOVE_60);
						}
						break;

					case Cmd.RCPCHG_REMOVE_60 :
						{
							if (m_smmRjUnload.IsReady == false  ||
								m_smmBypass.IsReady == false)
							{
								break;
							}

							smm.Next(Cmd.RCPCHG_CHANGE);
						}
						break;

					case Cmd.RCPCHG_CHANGE :
						{
							bool	brt	= false;

							Program.MainUI.Invoke(new System.Windows.Forms.MethodInvoker(delegate()
							{
								brt	= UI.SubpageRecipe.RecipeChange(m_nNewRecipeIdx);
							}));

							if (brt == false)
							{
								SM.SHM.MsgShow("RCPCHG", frmMessageBox.teButtons.OK, Language.languageDefault.MB_Error, Language.languageDefault.MB_SetupRecipe);

								smm.Next(Cmd.END_NG);
								break;
							}

							smm.Next(Cmd.RCPCHG_ALLHOME_10);
						}
						break;

					case Cmd.RCPCHG_ALLHOME_10 :
						{
							if (CheckTrayChange(m_nOldRecipeIdx, m_nNewRecipeIdx) == true  &&
								CheckAllEmpty() == true)
							{
								if (m_bManual == true)
								{
									SM.SHM.MsgShow("RCPCHG", frmMessageBox.teButtons.OKCancelNotClose, Language.languageDefault.MB_Info, Language.languageDefault.MB_AllHome);

									System.Windows.Forms.DialogResult rt	= SM.SHM.PopupMsgResult("RCPCHG");

									if (rt == System.Windows.Forms.DialogResult.None)			break;
									if (rt == System.Windows.Forms.DialogResult.Cancel)
									{
										SM.SHM.SetButtonNum(BUTTON.STOP);
										smm.Next(Cmd.END_NG);
										break;
									}
								}

								SM.SHM.MsgClose("RCPCHG");
								NDm.Write(eNDM.RejectRunMode, (int)teRejectRunMode.Normal);

								if (SM.SHM.IsBitData(BIT.AutoRun) == true  ||
									SM.SHM.IsBitData(BIT.Pause) == true)
								{
									SM.SHM.SetBitData(BIT.AllHome, false);
									SM.SHM.SetBitData(BIT.AutoRun, false);
									SM.SHM.SetBitData(BIT.Pause, false);
									G.seqMain.AutoRunStop();

									smm.Next(Cmd.RCPCHG_ALLHOME_20);
									break;
								}

								smm.Next(Cmd.RCPCHG_ALLHOME_30);
							}
							else if (CheckTrayChange(m_nOldRecipeIdx, m_nNewRecipeIdx) == true  &&
									 CheckAllEmpty() == false)
							{
								SM.SHM.SetButtonNum(BUTTON.STOP);
								smm.Next(Cmd.END_NG);
							}
							else
							{
								smm.Next(Cmd.END_OK);
							}
						}
						break;

					case Cmd.RCPCHG_ALLHOME_20 :
						{
							if (smm.Delayms(1000) == false)			break;

							smm.Next(Cmd.RCPCHG_ALLHOME_30);
						}
						break;

					case Cmd.RCPCHG_ALLHOME_30 :
						{
							Program.MainUI.Invoke(new System.Windows.Forms.MethodInvoker(delegate()
							{
								System.Windows.Forms.Form fc = System.Windows.Forms.Application.OpenForms["frmAllHomeMonitor"];
								if (fc != null)
								{
									if (fc.WindowState == System.Windows.Forms.FormWindowState.Minimized)
									{  // 폼을 최소화시켜 하단에 내려놓았는지 검사
										fc.WindowState = System.Windows.Forms.FormWindowState.Normal;
									}
									((frmAllHomeMonitor)fc).ResetStatus();
									fc.Activate();
								}
								else
								{
									frmAllHomeMonitor popup = new frmAllHomeMonitor();
									popup.Show();
								}
							}));
							
							SM.SHM.SetButtonNum(BUTTON.ALLHOME);

							smm.Next(Cmd.RCPCHG_ALLHOME_40);
						}
						break;

					case Cmd.RCPCHG_ALLHOME_40 :
						{
							if (SM.SHM.IsBitData(BIT.AllHoming) == true)
							{
								smm.Next(Cmd.RCPCHG_ALLHOME_50);
							}
						}
						break;
					
					case Cmd.RCPCHG_ALLHOME_50 :
						{
							if (SM.SHM.IsBitData(BIT.AllHoming) == false)
							{
								if (SM.SHM.IsBitData(BIT.AllHome) == true)
								{
									smm.Next(Cmd.RCPCHG_ALLHOME_60);
								}
								else
								{
									smm.Next(Cmd.END_NG);
								}
							}
						}
						break;

					case Cmd.RCPCHG_ALLHOME_60 :
						{
							SM.SHM.SetBitData(BIT.RecipeAutoChanging, false);

							smm.Next(Cmd.END_OK);
						}
						break;

					case Cmd.END_OK:
						{
							if (m_bManual == false)
							{
						//		SM.SHM.SetButtonNum(BUTTON.START);
								SM.SHM.SetButtonNum(BUTTON.MES_LOT_DATA_RCV);
							}
							smm.Next(Cmd.START);
						}
						break;

					case Cmd.END_NG:
						{
							SM.SHM.SetButtonNum(BUTTON.STOP);

							smm.Next(Cmd.START);
						}
						break;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
