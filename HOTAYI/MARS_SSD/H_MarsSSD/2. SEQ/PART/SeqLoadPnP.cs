using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace H_MarsSSD
{
	public class SeqLoadPnP : SeqBase<SeqLoadPnP.Act, SeqLoadPnP.Axis, SeqLoadPnP.Cmd>
	{
		#region [ Enum ]
		public enum Act
		{
			PnPUpDn1,
			PnPUpDn2,
			PnPUpDn3,
			PnPUpDn4,
			Vac1,
			Vac2,
			Vac3,
			Vac4,
			SmartCam,
			Max,
		}

		public enum Axis
		{
			X,
			Y,
			Z,
			W,
			Max,
		}

		public enum Cmd
		{
			START	= 10,
			END		= 99,

			HOME_10	= 100,
			HOME_20,
			HOME_30,
			HOME_40,
			HOME_50,
			HOME_60,
			HOME_70,
			HOME_80,
			HOME_90,
			HOME_100,

			PICK_TRAY_10,
			PICK_TRAY_20,
			PICK_TRAY_30, // Z 모터 실린더를 위로 모두 향하게 변경함
			PICK_TRAY_40,
			PICK_TRAY_50,
			PICK_TRAY_60,
			PICK_TRAY_70,
			PICK_TRAY_71,
			PICK_TRAY_72,
			PICK_TRAY_73,
			PICK_TRAY_74,
			PICK_TRAY_80,
			PICK_TRAY_90,
			PICK_TRAY_100,
			PICK_TRAY_110,
			PICK_TRAY_120,
			PICK_TRAY_130,
			PICK_TRAY_140,
			PICK_TRAY_150,
			PICK_TRAY_160,
			PICK_TRAY_170,
			PICK_TRAY_180,
			PICK_TRAY_190,
			PICK_TRAY_200,
			PICK_TRAY_210,
			PICK_TRAY_220,
			PICK_TRAY_230,
			PICK_TRAY_240,
			PICK_TRAY_250,
			PICK_TRAY_260,
			PICK_TRAY_270,
			PICK_TRAY_280,

			PICK_CHECKEMPTY_10,
			PICK_CHECKEMPTY_20,
			PICK_CHECKEMPTY_30,
			PICK_CHECKEMPTY_40,
			PICK_CHECKEMPTY_50,
			PICK_CHECKEMPTY_60,

			PICK_RETRY_10,
			PICK_RETRY_20,
			PICK_RETRY_30,
			PICK_RETRY_40,
			PICK_RETRY_50,
			PICK_RETRY_60,
			PICK_RETRY_70,
			PICK_RETRY_80,
			PICK_RETRY_90,


			PLACE_BUF_10,
			PLACE_BUF_20,
			PLACE_BUF_30,
			PLACE_BUF_40,
			PLACE_BUF_50,
			PLACE_BUF_60,
            PLACE_BUF_61,
            PLACE_BUF_70,
			PLACE_BUF_80,
			PLACE_BUF_90,
			PLACE_BUF_100,
			PLACE_BUF_110,
			PLACE_BUF_120,
			PLACE_BUF_130,

			PnP_10,
			PnP_20,
			PnP_30,
			PnP_40,
			PnP_50,
			PnP_60,
			
			EMPTY_CHECK_10,
			EMPTY_CHECK_20,
			EMPTY_CHECK_30,
			EMPTY_CHECK_40,
			EMPTY_CHECK_50,
			EMPTY_CHECK_60,
			EMPTY_CHECK_70,
			EMPTY_CHECK_80,
			EMPTY_CHECK_90,
		}

		public enum PosX
		{
			READY,
			BUFFER1,
			BUFFER2,
			TABLE1_FIRST,
			TABLE1_LAST,
			TABLE2_FIRST,
		}

		public enum PosY
		{
			READY,
			BUFFER,
			TABLE1_FIRST,
			TABLE1_LAST,
			TABLE2_FIRST,
		}

		public enum PosZ
		{
			READY,
			BUFFER,
			TABLE1,
			TABLE2,
		}
		public enum PosW
		{
			READY,
			BUFFER,
			TABLE,
		}

		private enum Dir
		{
			Right,
			Left,
		}
		#endregion

		#region [Variables]
		private	int	m_PickerCnt		= 4;
		private	int	m_PickerMaxCol	= 4;

		private SMM<Cmd> m_smmHome			= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADPNP_HOME));
		private SMM<Cmd> m_smmPickTray		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADPNP_PICKTRAY));
		private SMM<Cmd> m_smmPlaceBuf		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADPNP_PLACEBUFFER));
		private SMM<Cmd> m_smmPnP				= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADPNP_PNP));
		private SMM<Cmd> m_smmEmptyCheck		= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADPNP_EMPTYCHECK));
		private SMM<Cmd> m_smmPickTrayRetry	= new SMM<Cmd>(Cmd.START, EnumHelper.GetDescription(SeqID.LOADPNP_RETRY_PICK));

		public	Cmd	smmHome				{ get => m_smmHome.CurStep; }
		public	Cmd	smmPickTray			{ get => m_smmPickTray.CurStep; }
		public	Cmd	smmPlaceBuf			{ get => m_smmPlaceBuf.CurStep; }
		public	Cmd	smmPnP				{ get => m_smmPnP.CurStep; }
		public	Cmd	smmEmptyCheck		{ get => m_smmEmptyCheck.CurStep; }
		public	Cmd	smmPickTrayRetry	{ get => m_smmPickTrayRetry.CurStep; }

		private int		m_nCntTrayCol	= 0;
		private int		m_nCntTrayRow	= 0;
		private	bool	m_IsMtHome		= true;
		
		//Load PnP
		private	int		m_nProdCol	= 0;
		private	int		m_nProdRow	= 0;
		private	int		m_nAbsPosX	= 0;
		private	int		m_nAbsPosY	= 0;

		private PosX  m_nEmptyCheckPosX;
		private PosY  m_nEmptyCheckStartPosY	;
	

		private	int		m_nCurTable			= -1; // 가능한 테이블이 존재하지 않을 경우 -1
		private	int		m_nCurBuffer		= -1; // 가능한 버퍼가 존재하지 않을 경우 -1
		private Dir	    m_TrayDirection		= Dir.Left;

		// Pick 함수에서만 사용
		private	int		m_nStartPicker		= 0;
		private	int		m_nSeqUsingTable	= -1;	// 시퀀스 중에는 m_nCurTable 정보가 바뀌어도 계속 유지하기 위해 사용
		private	int		m_nPickRetryNo		= -1;

		// place 함수에서만 사용
		private	bool	m_nPlaceDone		 = false;
        private int     m_nPlaceBuffer       = -1;
        private int     m_nBeforePlaceBuffer = -1;

        private	int		m_nEmptyCheckTray	= -1; // 가능한 트레이가 존재하지 않을 경우 -1
		private	int		m_smmFTVInspRetry	= -1; 
		
		private	stActPicker	m_ActPicker		= new stActPicker(8);
		
		private List<int>			m_lstmissmatchProdInPicker = new List<int>();

		private	PosZ	m_TrayZPos
		{
			get
			{
				switch (m_nSeqUsingTable)
				{
					case 0: return PosZ.TABLE1;
					case 1: return PosZ.TABLE2;
				}
				return PosZ.TABLE1;
			}
		}
		#endregion

		#region Function
		public	void	Init()
		{
			try
			{
				lstSMM.Add(m_smmHome);
				lstSMM.Add(m_smmPickTray);
				lstSMM.Add(m_smmPlaceBuf);
				lstSMM.Add(m_smmPnP);
				lstSMM.Add(m_smmEmptyCheck);
				lstSMM.Add(m_smmPickTrayRetry);


				#region - Motor
				arrMt = new EnumArray<Axis, CMotor>(Axis.Max);
				arrMt[Axis.X] = G.cKernel.Motor(MT.LDPnPX); 
				arrMt[Axis.Y] = G.cKernel.Motor(MT.LDPnPY);
				arrMt[Axis.Z] = G.cKernel.Motor(MT.LDPnPZ);
				arrMt[Axis.W] = G.cKernel.Motor(MT.LDPnPW);
				#endregion

				#region - Actuator
				arrAct = new EnumArray<Act, Actuator>(Act.Max);
				
				arrAct[Act.Vac1] = new CSingleNoSen();
				arrAct[Act.Vac1].SetSol(DO.LDPnPVac1);
				arrAct[Act.Vac1].Setting(ALM.ERROR_LOAD_PNP_VAC1_VAC, ALM.ERROR_LOAD_PNP_VAC1_BLOW, ErrSenPos.Ori, SenPos.NONE, 1000, 2000);
				
				arrAct[Act.Vac2] = new CSingleNoSen();
				arrAct[Act.Vac2].SetSol(DO.LDPnPVac2);
				arrAct[Act.Vac2].Setting(ALM.ERROR_LOAD_PNP_VAC2_VAC, ALM.ERROR_LOAD_PNP_VAC2_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac3] = new CSingleNoSen();
				arrAct[Act.Vac3].SetSol(DO.LDPnPVac3);
				arrAct[Act.Vac3].Setting(ALM.ERROR_LOAD_PNP_VAC3_VAC, ALM.ERROR_LOAD_PNP_VAC3_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);
				
				arrAct[Act.Vac4] = new CSingleNoSen();
				arrAct[Act.Vac4].SetSol(DO.LDPnPVac4);
				arrAct[Act.Vac4].Setting(ALM.ERROR_LOAD_PNP_VAC4_VAC, ALM.ERROR_LOAD_PNP_VAC4_BLOW, ErrSenPos.Ori, SenPos.NONE, 200, 2000);

				arrAct[Act.PnPUpDn1] = new CDoubleSen();
				arrAct[Act.PnPUpDn1].SetSenser1(new List<DI>(new DI[] { DI.LDPnPDn1Cyl }));
				arrAct[Act.PnPUpDn1].SetSenser2(new List<DI>(new DI[] { DI.LDPnPUp1Cyl }));
				arrAct[Act.PnPUpDn1].SetSol(DO.LDPnPDn1Cyl, DO.LDPnPUp1Cyl);
				arrAct[Act.PnPUpDn1].Setting(ALM.ERROR_LOAD_PNP_UPDN1_CYL_DOWN, ALM.ERROR_LOAD_PNP_UPDN1_CYL_UP, ErrSenPos.BWD, SenPos.NONE, 600);

				arrAct[Act.PnPUpDn2] = new CDoubleSen();
				arrAct[Act.PnPUpDn2].SetSenser1(new List<DI>(new DI[] { DI.LDPnPDn2Cyl }));
				arrAct[Act.PnPUpDn2].SetSenser2(new List<DI>(new DI[] { DI.LDPnPUp2Cyl }));
				arrAct[Act.PnPUpDn2].SetSol(DO.LDPnPDn2Cyl, DO.LDPnPUp2Cyl);
				arrAct[Act.PnPUpDn2].Setting(ALM.ERROR_LOAD_PNP_UPDN2_CYL_DOWN, ALM.ERROR_LOAD_PNP_UPDN2_CYL_UP, ErrSenPos.BWD, SenPos.NONE, 600);
				
				arrAct[Act.PnPUpDn3] = new CDoubleSen();
				arrAct[Act.PnPUpDn3].SetSenser1(new List<DI>(new DI[] { DI.LDPnPDn3Cyl }));
				arrAct[Act.PnPUpDn3].SetSenser2(new List<DI>(new DI[] { DI.LDPnPUp3Cyl }));
				arrAct[Act.PnPUpDn3].SetSol(DO.LDPnPDn3Cyl, DO.LDPnPUp3Cyl);
				arrAct[Act.PnPUpDn3].Setting(ALM.ERROR_LOAD_PNP_UPDN3_CYL_DOWN, ALM.ERROR_LOAD_PNP_UPDN3_CYL_UP, ErrSenPos.BWD, SenPos.NONE, 600);

				arrAct[Act.PnPUpDn4] = new CDoubleSen();
				arrAct[Act.PnPUpDn4].SetSenser1(new List<DI>(new DI[] { DI.LDPnPDn4Cyl }));
				arrAct[Act.PnPUpDn4].SetSenser2(new List<DI>(new DI[] { DI.LDPnPUp4Cyl }));
				arrAct[Act.PnPUpDn4].SetSol(DO.LDPnPDn4Cyl, DO.LDPnPUp4Cyl);
				arrAct[Act.PnPUpDn4].Setting(ALM.ERROR_LOAD_PNP_UPDN4_CYL_DOWN, ALM.ERROR_LOAD_PNP_UPDN4_CYL_UP, ErrSenPos.BWD, SenPos.NONE, 600);

				arrAct[Act.SmartCam] = new CDoubleSen();
				arrAct[Act.SmartCam].SetSenser1(new List<DI>(new DI[] { DI.SmartCameraFwd }));
				arrAct[Act.SmartCam].SetSenser2(new List<DI>(new DI[] { DI.SmartCameraBwd }));
				arrAct[Act.SmartCam].SetSol(DO.SmartCameraFwd, DO.SmartCameraBwd);
				arrAct[Act.SmartCam].Setting(ALM.ERROR_SMART_CAMERA_MOVE_CYL_FORWARD, ALM.ERROR_SMART_CAMERA_MOVE_CYL_BACKWARD, ErrSenPos.BWD, SenPos.NONE, 600, 5000);

				#endregion

				m_nCntTrayCol	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
				m_nCntTrayRow	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				
				m_PickerCnt		= SM.FMP[FlowPart.Empty].GetMaxProdCount();
				m_PickerMaxCol	= SM.FMP[FlowPart.Empty].GetMaxProdCount();

				base.Initialize();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		public bool HomeStart(bool IsMtHome = true) { m_IsMtHome = IsMtHome; return SetSeqRun(ref m_smmHome); }

		public bool PickTrayStart() { return SetSeqRun(ref m_smmPickTray); }

		public bool PlaceBufStart() { return SetSeqRun(ref m_smmPlaceBuf); }

		public bool PnPStart() { return SetSeqRun(ref m_smmPnP); }

		public bool EmptyCheckStart(FlowTray eTray)
		{
			switch (eTray)
			{
				case FlowTray.LDTable1:
					m_nEmptyCheckTray = 0;
					break;
				case FlowTray.LDTable2:
					m_nEmptyCheckTray = 1;
					break;
				default:
					return false;
			}
			return SetSeqRun(ref m_smmEmptyCheck);
		}

		public bool PickRetryStart(int nPickNo)
		{
			m_nPickRetryNo = nPickNo;

			m_nCntTrayCol = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
			m_nCntTrayRow = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;

			m_smmPickTrayRetry.eInput = InState.run;

			return true;
		}

		public bool IsHomeReady() { return m_smmHome.IsReady; }

		public bool IsPickTrayReady() { return m_smmPickTray.IsReady; }

		public bool IsPlaceBufReady() { return m_smmPlaceBuf.IsReady; }

		public bool IsEmptyCheckReady() { return m_smmEmptyCheck.IsReady; }

		public bool IsPickTrayRetryReady() { return m_smmPickTrayRetry.IsReady && SM.SHM.IsBitData(BIT.Error) == false; }

		public bool IsPnPReady() { return m_smmPnP.IsReady; }

		public bool IsPlaceBuffer(int nBufNo)
		{
			if (m_nPlaceDone == true && nBufNo == m_nPlaceBuffer)
			{
				m_nPlaceDone = false;
				return true;
			}

			return false;
		}

		public int IsPlaceGrip()
		{
			if (m_smmPlaceBuf.CurStep == Cmd.PLACE_BUF_90) return m_nPlaceBuffer;
			else return -1;
		}

		private bool SetSeqRun(ref SMM<Cmd> smm)
		{
			try
			{
				if (this.IsReady == false) return false;

				m_nCntTrayCol = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
				m_nCntTrayRow = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;

				smm.eInput = InState.run;
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private void UpdateCurTable()
		{
			try
			{
				int ntable_no = 0;

				if (m_nCurTable == -1)
				{
					if (SM.FMT.IsFlow(FlowTray.LDTable1) == true &&
						SM.FMT.IsFlow(FlowTray.LDTable2) == true &&
						G.seqLoadTrayTable1.IsPnPEnable() ==  true &&
						G.seqLoadTrayTable2.IsPnPEnable() ==  true &&
						SM.SHM.IsUsePart(UsePartName.LDTable1, UsePartMode.Unload) == true &&
						SM.SHM.IsUsePart(UsePartName.LDTable2, UsePartMode.Unload) == true)
					{
						if (SM.FMT[FlowTray.LDTable1].GetInfo().TransTime <= SM.FMT[FlowTray.LDTable2].GetInfo().TransTime)
						{
							ntable_no = 0;
						}
						else
						{
							ntable_no = 1;
						}

						if (SearchProd(ntable_no, Dir.Left) == true)
						{
							m_nCurTable = ntable_no;
							return;
						}
					}

					if (SM.FMT.IsFlow(FlowTray.LDTable1) == true &&
						G.seqLoadTrayTable1.IsPnPEnable() ==  true &&
						SM.SHM.IsUsePart(UsePartName.LDTable1, UsePartMode.Unload) == true)
					{
						if (SearchProd(0, Dir.Left) == true)
						{
							m_nCurTable = 0;
							return;
						}
					}

					if (SM.FMT.IsFlow(FlowTray.LDTable2) == true &&
						G.seqLoadTrayTable2.IsPnPEnable() ==  true &&
						SM.SHM.IsUsePart(UsePartName.LDTable2, UsePartMode.Unload) == true)
					{
						if (SearchProd(1, Dir.Left) == true)
						{
							m_nCurTable = 1;
							return;
						}
					}
				}

                if (m_nCurTable == 0 && G.seqLoadTrayTable1.IsPnPEnable() == false)
                {
                    m_nCurTable = -1;
                }
                else if (m_nCurTable == 1 && G.seqLoadTrayTable2.IsPnPEnable() == false)
                {
                    m_nCurTable = -1;
                }
				if (SearchProd(m_nCurTable, Dir.Left) == false)
				{
					m_nCurTable = -1;
				}
				else
				{
					return;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void UpdateCurBuffer()
		{
			try
			{
				if (m_nCurBuffer < 0)
				{
					if (G.seqLoadBuffer1.IsLDWait() == true) m_nCurBuffer = 0;
					if (G.seqLoadBuffer2.IsLDWait() == true) m_nCurBuffer = 1;
				}

				if (m_nCurBuffer == 0 && G.seqLoadBuffer1.IsLDWait() == false)			m_nCurBuffer = -1;
				else if (m_nCurBuffer == 1 && G.seqLoadBuffer2.IsLDWait() == false)		m_nCurBuffer = -1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool GetPnPPos(int nTableNo, int nPickerNo, int nCol, int nRow, ref int nXPos, ref int nYPos)
		{
			try
			{
				int ndist_x = 0;
				int ndist_y = 0;
				int npicker_dif_x = 0;
				int ncnt_row = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				int ncnt_col = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;

				npicker_dif_x = GetPickerGap(nPickerNo);
				ndist_x = (arrMt[Axis.X].GetIndexPos((int)PosX.TABLE1_LAST) - arrMt[Axis.X].GetIndexPos((int)PosX.TABLE1_FIRST)) / (ncnt_col - 1);
				ndist_y = (arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE1_LAST) - arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE1_FIRST)) / (ncnt_row - 1);

				if (nTableNo == 0)
				{
					nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.TABLE1_FIRST);
					nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE1_FIRST);
				}
				else if (nTableNo == 1)
				{
					nXPos = arrMt[Axis.X].GetIndexPos((int)PosX.TABLE2_FIRST);
					nYPos = arrMt[Axis.Y].GetIndexPos((int)PosY.TABLE2_FIRST);
				}
				else
				{
					return false;
				}

				nXPos += (nCol * ndist_x) - npicker_dif_x;
				nYPos += (nRow * ndist_y);

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private FlowTray GetFlow(int nTableNo)
		{
			try
			{
				if (nTableNo == 0) return FlowTray.LDTable1;
				else return FlowTray.LDTable2;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return FlowTray.Empty;
			}


		}

		private bool SearchProd(int nTableNo, Dir FromSearchDir, ref int nCol, ref int nRow, int nStartOffsetCol = 0)
		{
			try
			{
				FlowTray flow = FlowTray.LDTable1;
				int nindex = 0;

				if (nTableNo == 0) flow = FlowTray.LDTable1;
				else flow = FlowTray.LDTable2;

				if (SM.FMT.IsFlow(flow) == false) return false;

				
				for (int nrow = 0; nrow < m_nCntTrayRow; nrow++)
				{
					for (int ncol = nStartOffsetCol; ncol < m_nCntTrayCol; ncol++)
					{
						nindex = nrow * m_nCntTrayCol + ncol;

						if (SM.FMT[flow].GetInfo().ProdInfo[nindex].Exist == true)
						{
							nCol = ncol;
							nRow = nrow;

							return true;
						}
					}
				}

				//if (FromSearchDir == Dir.Left)
				//{
				//	for (int nrow = 0; nrow < m_nCntTrayRow; nrow++)
				//	{
				//		for (int ncol = nStartOffsetCol; ncol < m_nCntTrayCol; ncol++)
				//		{
				//			nindex = nrow * m_nCntTrayCol + ncol;
				//
				//			if (SM.FMT[flow].GetInfo().ProdInfo[nindex].Exist == true)
				//			{
				//				nCol = ncol;
				//				nRow = nrow;
				//
				//				return true;
				//			}
				//		}
				//	}
				//}
				//else
				//{
				//	for (int nrow = 0; nrow < m_nCntTrayRow; nrow++)
				//	{
				//		for (int ncol = m_nCntTrayCol - 1 - nStartOffsetCol; ncol >= 0; ncol--)
				//		{
				//			nindex = nrow * m_nCntTrayCol + ncol;
				//
				//			if (SM.FMT[flow].GetInfo().ProdInfo[nindex].Exist == true)
				//			{
				//				nCol = ncol;
				//				nRow = nrow;
				//
				//				return true;
				//			}
				//		}
				//	}
				//}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool SearchProd(int nTableNo, Dir FromSearchDir)
		{
			try
			{
				int ncol = 0;
				int nrow = 0;
				return SearchProd(nTableNo, FromSearchDir, ref ncol, ref nrow);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool CheckGapValid(int nCol, int nPickerNo)
		{
			try
			{
				 if (nPickerNo == 0) return true;

				//int nstep = SM.SHM.GetRecipeDataToInt(DEVICE.GripStepCnt) / 1000;

				//정상 동작 확인 필요
				double nDistPicker	= TrayPnPRecipe.PnPInfo.LoadPicker.ColsGap[nPickerNo];
				double nDistTray	= TrayPnPRecipe.PnPInfo.Tray.ColsGap[nCol];
				double nstep = nDistPicker / nDistTray;
				//정수로 떨어져야될거같은데...안떨어지면 알람? 여기 말고 set하는 곳에서 알람?

				double dsum_gap = 0;

				for (int i = 0; i < nstep; i++)
				{
					if ((nCol - i) < 0) return false;
					dsum_gap += TrayPnPRecipe.PnPInfo.Tray.ColsGap[nCol - i];
				}

				if (dsum_gap != TrayPnPRecipe.PnPInfo.LoadPicker.ColsGap[nPickerNo]) return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private int GetPickerGap(int nPickerNo)
		{
			try
			{
				double dsum_gap = 0;
				for (int i = 0; i <= nPickerNo; i++)
				{
					if (i > SM.FMP[FlowPart.LDPnP].GetMaxProdCount()) return -1;
					dsum_gap += (TrayPnPRecipe.PnPInfo.LoadPicker.ColsGap[i] * 1000);
				}
				return (int)dsum_gap;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}

        //		// 실린더 컨트롤 (SenPos.FWD = 실린더 아래로 Push, SenPos.BWD = 실린더 위로 Pull)
        public void RunCylUpDown(SenPos eSenPos, int nProd = -1)
        {
            try
            {
                if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true && eSenPos == SenPos.PICKER_DOWN) return;

                if (nProd == -1)
                {
                    for (int i = 0; i < SM.FMP[FlowPart.LDPnP].GetMaxProdCount(); i++)
                    {
                        if (eSenPos == SenPos.PICKER_DOWN &&
                            SM.FMP[FlowPart.LDPnP].GetProdExist(i) == false &&
                            SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Enable) == false)
                        {
                            continue;
                        }

                        arrAct[Act.PnPUpDn1 + i].SetPos(eSenPos);
                    }
                }
                else
                {
                    if (eSenPos == SenPos.PICKER_DOWN &&
                        SM.SHM.IsUsePart(UsePartName.Picker1 + nProd, UsePartMode.Enable) == true)
                    {
                        arrAct[Act.PnPUpDn1 + nProd].SetPos(eSenPos);
                    }
                }
            }
            catch (Exception ex)
            {
                G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
            }
        }

        public bool CheckCylUpDown(SenPos eSenPos, int nProd = -1)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true && eSenPos == SenPos.PICKER_DOWN) return true;


                bool res = true;

                if (nProd == -1)
                {
                    for (int i = 0; i < SM.FMP[FlowPart.LDPnP].GetMaxProdCount(); i++)
                    {
                        if (eSenPos == SenPos.PICKER_DOWN &&  //Up은 체크할거임
                            SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Enable) == false)
                        {
                            continue;
                        }

                        if (arrAct[Act.PnPUpDn1 + i].IsPos(eSenPos) == false)
                        {
                            res = false;
                            return res;
                        }
                    }
                }
                else
                {
                    if (eSenPos == SenPos.PICKER_DOWN &&
                        SM.SHM.IsUsePart(UsePartName.Picker1 + nProd, UsePartMode.Enable) == false)
                    {
                        return true;
                    }
                    else if (arrAct[Act.PnPUpDn1 + nProd].IsPos(eSenPos) == false)
                    {
                        res = false;
                    }
                }

                return res;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private bool CheckCylUpDown(SenPos eSenPos, stActPicker actPicker)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true && eSenPos == SenPos.PICKER_DOWN) return true;

				int[] nno = actPicker.GetPocketNoAll();

				for (int i = 0; i < nno.Length; i++)
				{
					if (nno[i] < 0) continue;

					if (CheckCylUpDown(eSenPos, i) == false) return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public void RunEjector(SenPos eSenPos, int nProd = -1)
		{
			try
			{
				if (nProd == -1)
				{
					for (int i = 0; i < SM.FMP[FlowPart.LDPnP].GetMaxProdCount(); i++)
					{
						arrAct[Act.Vac1 + i].SetPos(eSenPos);
					}
				}
				else
				{
					arrAct[Act.Vac1 + nProd].SetPos(eSenPos);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		
		private	void	RunCylUpDown(SenPos eSenPos, stActPicker actPicker)
		{
			try
			{
				if (SM.SHM.GetUseSkipData(USESKIP.DRYRUN) == true  &&  eSenPos == SenPos.PICKER_DOWN)			return;

				int[] nno	= actPicker.GetPocketNoAll();

				for (int i = 0; i < nno.Length; i++)
				{
					if (nno[i] < 0)			continue;

					RunCylUpDown(eSenPos, i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}


		public bool CheckEjector(SenPos eSenPos, int nProd = -1, bool bAlarm = false)
		{
			try
			{
				if (nProd == -1)
				{
					for (int i = 0; i < SM.FMP[FlowPart.LDPnP].GetMaxProdCount(); i++)
					{
						if (arrAct[Act.Vac1 + i].GetPos() != eSenPos ||
						G.DryRunIsAIn(DI.LDPnPVac1 + i, eSenPos == SenPos.VAC) != (eSenPos == SenPos.VAC))
                        {
                            if (bAlarm == true)
                            {
                                SM.FMP[FlowPart.LDPnP].SetPickupStatus(ProdPickStatus.Mismatch, i);
                                AddAlarm(ALM.ERROR_LOAD_PNP_VAC1_VAC + i);
                            }
                            else                    return false;
                        }
					}
				}
				else
				{
					if (arrAct[Act.Vac1 + nProd].GetPos() != eSenPos  ||
						G.DryRunIsAIn(DI.LDPnPVac1 + nProd, eSenPos == SenPos.VAC) != (eSenPos == SenPos.VAC))
                    {
                        if (bAlarm == true) 
                        {
                            SM.FMP[FlowPart.LDPnP].SetPickupStatus(ProdPickStatus.Mismatch, nProd);
                            AddAlarm(ALM.ERROR_LOAD_PNP_VAC1_VAC + nProd);
                        }
                        else                        return false;
                    }
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private void RunPickerEjector(SenPos eSenPos, stActPicker actPicker)
		{
			try
			{
				int[] nno = actPicker.GetPocketNoAll();

				for (int i = 0; i < nno.Length; i++)
				{
					if (nno[i] < 0) continue;

					RunEjector(eSenPos, i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private bool CheckPickerEjector(SenPos eSenPos, stActPicker actPicker, bool bAlarm = false)
		{
			try
			{
				int[] nno = actPicker.GetPocketNoAll();

				for (int i = 0; i < nno.Length; i++)
				{
					if (nno[i] < 0) continue;

					if (CheckEjector(eSenPos, i, bAlarm) == false) return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		

		public bool CheckLoadPnPFull()
		{
			try
			{
				if (SM.FMP[FlowPart.LDPnP].GetExistCount() == SM.FMP[FlowPart.LDPnP].GetMaxProdCount()) return true;

				bool[] bexist = SM.FMP[FlowPart.LDPnP].GetProdExist();

				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] == false && SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Disable) == false)
					{
						return false;
					}
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}


		private void FlowMngMoveToPicker(int nTableNo, int nTrayPocketNo, int nPickerNo)
		{
			try
			{
				if (SM.FMP.IsFlow(FlowPart.LDPnP) == false)
				{
					SM.FMP.Next(FlowPart.Empty, FlowPart.LDPnP);
				}

				InfoPart.tsInfo info_p = SM.FMP[FlowPart.LDPnP].GetInfo();
				InfoTray.tsInfo info_t = SM.FMT[FlowTray.LDTable1 + nTableNo].GetInfo();

				info_p.MaxCntTray = info_t.MaxCntTray;
				info_p.MaxCntProdInTray = info_t.MaxCntProdInTray;
				info_p.MaxCntProdInLot = info_t.MaxCntProdInLot;
				info_p.WorkOrder = info_t.WorkOrder;
				info_p.KeyValue = info_t.KeyValue;
				info_p.ProgramID_Top = info_t.ProgramID_Top;
				info_p.ProgramID_Btm = info_t.ProgramID_Btm;
				info_p.ProdInfo[nPickerNo] = info_t.ProdInfo[nTrayPocketNo];

				SM.FMP[FlowPart.LDPnP].SetInfo(info_p);

				GLog.Add(string.Format($"Prod Flow {FlowTray.LDTable1 + nTableNo} -> {FlowPart.LDPnP} - {nPickerNo} . ID = {info_p.ProdInfo[nPickerNo].ProdID}"));

				SM.FMT[FlowTray.LDTable1 + nTableNo].ClearProd(nTrayPocketNo);
				if (SM.SHM.m_dicLotPickTime.ContainsKey(info_p.KeyValue) == false)
					SM.SHM.m_dicLotPickTime.Add(info_p.KeyValue, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void FlowMngMoveToPicker(int nTableNo, stActPicker ActPicker)
		{
			try
			{
				for (int i = 0; i < ActPicker.Picker.Length; i++)
				{
					if (ActPicker.GetPocketNo(i) == -1) continue;

					FlowMngMoveToPicker(nTableNo, ActPicker.GetPocketNo(i), i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void FlowMngMoveToTray(int nTableNo, int nTrayPocketNo, int nPickerNo)
		{
			try
			{
				InfoPart.tsInfo info_p = SM.FMP[FlowPart.LDPnP].GetInfo();
				InfoTray.tsInfo info_t = SM.FMT[FlowTray.LDTable1 + nTableNo].GetInfo();

				info_t.MaxCntTray = info_p.MaxCntTray;
				info_t.MaxCntProdInTray = info_p.MaxCntProdInTray;
				info_t.MaxCntProdInLot = info_p.MaxCntProdInLot;
				info_t.WorkOrder = info_p.WorkOrder;
				info_t.KeyValue = info_p.KeyValue;
				info_t.ProgramID_Top = info_p.ProgramID_Top;
				info_t.ProgramID_Btm = info_p.ProgramID_Btm;
				info_t.ProdInfo[nTrayPocketNo] = info_p.ProdInfo[nPickerNo];

				SM.FMT[FlowTray.LDTable1 + nTableNo].SetInfo(info_t);

				SM.FMP[FlowPart.LDPnP].ClearProd(nPickerNo);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void FlowMngMoveToTray(int nTableNo, stActPicker ActPicker)
		{
			try
			{
				for (int i = 0; i < ActPicker.Picker.Length; i++)
				{
					if (ActPicker.GetPocketNo(i) == -1) continue;

					FlowMngMoveToTray(nTableNo, ActPicker.GetPocketNo(i), i);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private ProdExist CheckProdExist()
		{
			try
			{
				FlowPart flowPart = FlowPart.LDPnP;
				DI ExistSenStart = DI.LDPnPVac1;

				bool[] bexist = SM.FMP[flowPart].GetProdExist();
				bool[] sen = new bool[bexist.Length];

				for (int i = 0; i < bexist.Length; i++)
					sen[i] = G.DryRunIsAIn(ExistSenStart + i, bexist[i]);

				// 완전 없음
				if (sen.All(s => s == false) && SM.FMP[flowPart].GetExistCount() == 0)
					return ProdExist.None;

				bool isMismatch = false;
				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] != sen[i])
					{
						SM.FMP[flowPart].SetPickupStatus(ProdPickStatus.Mismatch, i);
						isMismatch = true;
					}
					else
					{
						SM.FMP[flowPart].SetPickupStatus(ProdPickStatus.Normal, i);
					}
				}

				if (isMismatch)
				{
					string dataStr = string.Join(", ", bexist.Select(b => b.ToString()));
					string sensorStr = string.Join(", ", sen.Select(s => s.ToString()));

					GLog.AddErr($"{flowPart.ToString()} Mismatching. Data: {dataStr}, Sensor: {sensorStr}");
					return ProdExist.Mismatch;
				}

				// 모두 정상
				return ProdExist.Exist;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, $"Message : {ex.Message}\nStack trace : {ex.StackTrace}");
				return ProdExist.Mismatch;
			}
		}


		private List<int> CheckPnpProdExistIndexList()
		{
			try
			{
				bool[] bexist = SM.FMP[FlowPart.LDPnP].GetProdExist();
				List<int> lstresult = new List<int>();
		
				for (int i = 0; i < bexist.Length; i++)
				{
					if (bexist[i] != G.DryRunIsAIn(DI.LDPnPVac1 + i, bexist[i]))
					{
						lstresult.Add(i);
					}
				}

				return lstresult;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return null;
			}
		}
		#endregion



		public void Run()
		{
			try
			{
				UpdateCurTable();
				UpdateCurBuffer();
				UpdateAct(Seq.LoadPnP);

				// Edit 창을 띄우기 위해 사용
				if (SM.SHM.IsBitData(BIT.Error) == true)
				{
					switch (SM.SHM.PopupMsgResult(SeqMsg.PCBMisMatch))
					{
						case "EDIT":
							m_lstmissmatchProdInPicker = CheckPnpProdExistIndexList();
							frmProductSelect SeqProductSelect = new frmProductSelect();
							SeqProductSelect.ShowDialog(GetFlow(m_nSeqUsingTable), CheckPnpProdExistIndexList(), FlowPart.LDPnP, "Edit Tray");
							break;
						case "SKIP":
							break;
					}
				}


				if (SM.SHM.IsBitData(BIT.AutoRun) == true)
				{
					if (m_smmPickTrayRetry.IsReady == false) m_smmPickTrayRetry.ResetStep();
				}
				else
				{
					PickTrayRetry(m_smmPickTrayRetry);
				}

				SM.SHM.SetSeqStatus(lstSMM);

				if (SM.SHM.IsBitData(BIT.Error) == true || SM.SHM.IsBitData(BIT.Pause) == true) return;
				if (SM.SHM.IsBitData(BIT.AutoRun) == true && IsReady == true && CheckCylUpDown(SenPos.PICKER_UP) && CheckProdExist() == ProdExist.Mismatch) AddAlarm(ALM.ERROR_SEQ_LOAD_PnP_PROD_MISMATCH);

				Home(m_smmHome);
				PickTray(m_smmPickTray);
				PlaceBuffer(m_smmPlaceBuf);
				PnP(m_smmPnP);
				EmptyCheck(m_smmEmptyCheck);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void Home(SMM<Cmd> smm)
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

								smm.Next(Cmd.HOME_10);
							}
						}
						break;

					case Cmd.HOME_10:
						{
							if (SM.FMP.IsFlow(FlowPart.LDPnP) == false)
							{
								if (CheckEjector(SenPos.BLOW) == false)
								{
									RunEjector(SenPos.BLOW);
									break;
								}
							}
							else
							{
								for (int i = 0; i < SM.FMP[FlowPart.LDPnP].GetMaxProdCount(); i++)
								{
									if (SM.FMP[FlowPart.LDPnP].GetInfo().ProdInfo[i].Exist == false)
									{
										if (CheckEjector(SenPos.BLOW, i) == false)
										{
											RunEjector(SenPos.BLOW, i);
											break;
										}
									}
									else
									{
                                        if (smm.Delayms(NDm.Read(eNDM.LoadPnPVacDelay)) == true)
                                        {
                                            smm.Next(Cmd.HOME_20);
                                            break;
                                        }

                                        if (CheckEjector(SenPos.VAC,i) == false)
										{
											RunEjector(SenPos.VAC,i);
											break;
										}
									}
								}
							}

							smm.Next(Cmd.HOME_20);
						}
						break;

					case Cmd.HOME_20:
                        {
                            if (smm.Delayms(NDm.Read(eNDM.LoadPnPVacDelay)) == false) break;

                            if (CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}

							smm.Next(Cmd.HOME_30);
						}
						break;

					case Cmd.HOME_30:
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false)					break;

							if (CheckProdExist() == ProdExist.Mismatch)
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_PnP_PROD_MISMATCH);
								smm.Next(Cmd.END);
								break;
							}
							if (m_IsMtHome == false)
							{
								smm.Next(Cmd.HOME_70);
								break;
							}


							smm.Next(Cmd.HOME_40);
						}
						break;

					case Cmd.HOME_40:
						{
							arrMt[Axis.Z].ClearHome();
							arrMt[Axis.Z].StartHoming();

							smm.Next(Cmd.HOME_50);
						}
						break;

					case Cmd.HOME_50:
						{
                            if (smm.Delayms(100) == false) break;
                            
							if (arrMt[Axis.Z].IsHomeDone() == false || arrMt[Axis.Z].IsRdy() == false) break;
							
							arrMt[Axis.X].ClearHome();
							arrMt[Axis.X].StartHoming();
							
							arrMt[Axis.Y].ClearHome();
							arrMt[Axis.Y].StartHoming();


							smm.Next(Cmd.HOME_60);
						}
						break;

					case Cmd.HOME_60:
						{
							if (arrMt[Axis.X].IsHomeDone() == false || arrMt[Axis.X].IsRdy() == false) break;
							if (arrMt[Axis.Y].IsHomeDone() == false || arrMt[Axis.Y].IsRdy() == false) break;

                            arrMt[Axis.W].ClearHome();
                            arrMt[Axis.W].StartHoming();

                            smm.Next(Cmd.HOME_70);
						}
						break;

					case Cmd.HOME_70:
                        {
                            if (arrMt[Axis.W].IsHomeDone() == false || arrMt[Axis.W].IsRdy() == false) break;

                            if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}

							smm.Next(Cmd.HOME_80);
						}
						break;

					case Cmd.HOME_80:
						{
							if (arrMt[Axis.X].IsRdy((int)PosX.READY) == false ||
								arrMt[Axis.Y].IsRdy((int)PosY.READY) == false ||
								arrMt[Axis.W].IsRdy((int)PosW.READY) == false)
							{
								arrMt[Axis.X].MoveIndex((int)PosX.READY);
								arrMt[Axis.Y].MoveIndex((int)PosY.READY);
								arrMt[Axis.W].MoveIndex((int)PosW.READY);
								break;
							}
							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
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

		private void PickTray(SMM<Cmd> smm)
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

								smm.Next(Cmd.PICK_TRAY_10);
							}
						}
						break;

					case Cmd.PICK_TRAY_10:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOAD_PnP_PROD_MISMATCH);
									return;
								default:
									smm.Next(Cmd.PICK_TRAY_20);
									break;
							}
						}
						break;

					case Cmd.PICK_TRAY_20:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}



							smm.Next(Cmd.PICK_TRAY_30);
						}
						break;

					case Cmd.PICK_TRAY_30:
						{
							if (arrMt[Axis.W].IsRdy((int)PosW.TABLE) == false )
							{
								arrMt[Axis.W].MoveIndex((int)PosW.TABLE);
								break;
							}

							smm.Next(Cmd.PICK_TRAY_40);
						}
						break;

					case Cmd.PICK_TRAY_40:
						{
							if (m_nCurTable == -1) break;

							m_nSeqUsingTable = m_nCurTable;

							smm.Next(Cmd.PICK_TRAY_50);
						}
						break;

					case Cmd.PICK_TRAY_50:
						{
							m_nStartPicker = -1;
							for (int i = 0; i < SM.FMP[FlowPart.LDPnP].GetMaxProdCount(); i++)
							{
								if (SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Disable) == true) continue;

								if (SM.FMP[FlowPart.LDPnP].GetProdExist(i) == false)
								{
									m_nStartPicker = i;
									break;
								}
							}
							if (m_nStartPicker == -1)
							{
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.PICK_TRAY_60);
						}
						break;

					case Cmd.PICK_TRAY_60:
						{
							//LotNo가 변경되면
							if (SM.FMP.IsFlow(FlowPart.LDPnP) == true &&
								SM.FMP[FlowPart.LDPnP].GetInfo().KeyValue != SM.FMT[GetFlow(m_nSeqUsingTable)].GetInfo().KeyValue)
							{
								SM.FMP[FlowPart.LDPnP].SetLastOfLot(true);
								smm.Next(Cmd.END);
								break;
							}

							if (SM.FMP[FlowPart.LDPnP].GetInfo().LastPieceOfLot == true)
							{
								smm.Next(Cmd.END);
								break;
							}

							int nstart_offet_col = 0;
							//자재가 있는 pocket 확인
							if (SearchProd(m_nSeqUsingTable, m_TrayDirection, ref m_nProdCol, ref m_nProdRow, nstart_offet_col) == false)
							{
								if (SM.FMT[GetFlow(m_nSeqUsingTable)].GetInfo().LastTrayOfLot == true)
								{
									SM.FMP[FlowPart.LDPnP].SetLastOfLot(true);
									SM.FMT[GetFlow(m_nSeqUsingTable)].SetLastOfLot(false);
								}

								smm.Next(Cmd.END);
								break;
							}
							

							smm.Next(Cmd.PICK_TRAY_70);
						}
						break;

					case Cmd.PICK_TRAY_70:
						{
							int ncol = 0;
							int ncnt = 0;
							int npocket = 0;
							int nstep = SM.SHM.GetRecipeDataToInt(DEVICE.GripStepCnt) / 1000;

							m_ActPicker.ResetAll();

							int nend_picker = (m_nStartPicker / m_PickerMaxCol) * m_PickerMaxCol + m_PickerMaxCol;
							for (int i = m_nStartPicker; i < nend_picker; i++)
							{
								if (SM.SHM.IsUsePart(UsePartName.Picker1 + i, UsePartMode.Disable) == true)
								{
									ncnt++;
									continue;
								}

								ncol = m_nProdCol + ncnt + nstep * ncnt;

								//if (m_TrayDirection == Dir.Right)     ncol = m_nProdCol - ncnt - nstep * ncnt;
								//else                                    ncol = m_nProdCol + ncnt + nstep * ncnt;

								npocket = m_nProdRow * m_nCntTrayCol + ncol;

								if (ncol < m_nCntTrayCol &&
									i > m_nStartPicker &&
									ncol > 0)
								{
									if (CheckGapValid(ncol, i % m_PickerMaxCol) == false) break;
								}

								if (ncol < m_nCntTrayCol &&
									ncol >= 0 &&
									SM.FMP[FlowPart.LDPnP].GetProdExist(i) == false &&
									m_ActPicker.ContainsPocket(npocket) == false &&
									SM.FMT[GetFlow(m_nSeqUsingTable)].GetInfo().ProdInfo[npocket].Exist == true)
								{
									m_ActPicker.Set(i, ncol, m_nProdRow, m_nCntTrayCol);
								}

								ncnt++;
							}

							GetPnPPos(m_nSeqUsingTable, m_nStartPicker, m_nProdCol, m_nProdRow, ref m_nAbsPosX, ref m_nAbsPosY);
							GLog.Add($"TEST X : {m_nAbsPosX} / Y: {m_nAbsPosY}");
							smm.Next(Cmd.PICK_TRAY_80);
						}
						break;

					case Cmd.PICK_TRAY_80:
						{
							if (arrMt[Axis.X].IsRdyPos(m_nAbsPosX) == false ||
								arrMt[Axis.Y].IsRdyPos(m_nAbsPosY) == false)
							{
								arrMt[Axis.X].MoveABS(m_nAbsPosX, (int)PosX.TABLE1_FIRST);
								arrMt[Axis.Y].MoveABS(m_nAbsPosY, (int)PosY.TABLE1_FIRST);
								break;
							}
							
							smm.Next(Cmd.PICK_TRAY_90);
						}
						break;

					case Cmd.PICK_TRAY_90:
						{
							if (arrMt[Axis.Z].IsRdy((int)m_TrayZPos) == false ||
								CheckCylUpDown(SenPos.PICKER_DOWN, m_ActPicker) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)m_TrayZPos);
								RunCylUpDown(SenPos.PICKER_DOWN, m_ActPicker);
								break;
							}

							smm.Next(Cmd.PICK_TRAY_100);
						}
						break;

					case Cmd.PICK_TRAY_100:
						{
							if (smm.Delayms(100) == false) break;

                            RunPickerEjector(SenPos.VAC, m_ActPicker);
                            smm.Next(Cmd.PICK_TRAY_110);
						}
						break;

					case Cmd.PICK_TRAY_110:
						{
                            if (smm.Delayms(NDm.Read(eNDM.LoadPnPVacDelay)) == true)
                            {
                                smm.Next(Cmd.PICK_TRAY_120);
                                break;
                            }

                            if (CheckPickerEjector(SenPos.VAC, m_ActPicker) == false)
							{
								RunPickerEjector(SenPos.VAC, m_ActPicker);
                                break;
							}

							smm.Next(Cmd.PICK_TRAY_120);
						}
						break;

					case Cmd.PICK_TRAY_120:
						{
							RunCylUpDown(SenPos.PICKER_UP);

							FlowMngMoveToPicker(m_nSeqUsingTable, m_ActPicker);

							if (SM.FMT[GetFlow(m_nSeqUsingTable)].GetExistCount() == 0 &&
								SM.FMT[GetFlow(m_nSeqUsingTable)].GetInfo().LastTrayOfLot == true)
							{
								SM.FMP[FlowPart.LDPnP].SetLastOfLot(true);
								SM.FMT[GetFlow(m_nSeqUsingTable)].SetLastOfLot(false);
							}

							smm.Next(Cmd.PICK_TRAY_130);
						}
						break;

					case Cmd.PICK_TRAY_130:
						{

							if (SM.FMP[FlowPart.LDPnP].GetProdExist().Contains(false) == false)
							{
								if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
									arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
								{
									RunCylUpDown(SenPos.PICKER_UP);
									arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
									break;
								}
							}
							else
							{
								if (CheckCylUpDown(SenPos.PICKER_UP) == false)
								{
									RunCylUpDown(SenPos.PICKER_UP);
									break;
								}
							}

							smm.Next(Cmd.PICK_TRAY_140);
						}
						break;

					case Cmd.PICK_TRAY_140:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOAD_PnP_PROD_MISMATCH);
									SM.SHM.MsgShow(SeqMsg.PCBMisMatch);
									smm.Next(Cmd.END);//끝내고 다시 시작해야 피커 그리퍼 초기화 가능
									break;
								default:
									smm.Next(Cmd.PICK_TRAY_150);
									break;
							}
						}
						break;

					case Cmd.PICK_TRAY_150:
						{
							if (SM.FMT[GetFlow(m_nSeqUsingTable)].GetExistCount() == 0)
							{
								if (SM.FMT[GetFlow(m_nSeqUsingTable)].GetInfo().LastTrayOfLot == true)
								{
									SM.FMP[FlowPart.LDPnP].SetLastOfLot(true);
									SM.FMT[GetFlow(m_nSeqUsingTable)].SetLastOfLot(false);
								}

								smm.Next(Cmd.PICK_CHECKEMPTY_10);
							}
							else if (SM.FMP[FlowPart.LDPnP].GetProdExist().Contains(false) == true)
							{
								smm.Next(Cmd.PICK_TRAY_50);
							}
							else
							{
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.PICK_CHECKEMPTY_10:
						{
							if (IsEmptyCheckReady() == false) break;
							if (m_nSeqUsingTable == 1 && G.seqLoadTrayTable2.IsEmptyCheckEnable() == false)  break;
							
							m_nEmptyCheckTray = m_nSeqUsingTable;

							m_smmEmptyCheck.eInput = InState.run;

							smm.Next(Cmd.PICK_CHECKEMPTY_20);
						}
						break;

					case Cmd.PICK_CHECKEMPTY_20:
						{
							if (IsEmptyCheckReady() == false) break;

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
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

		private void PlaceBuffer(SMM<Cmd> smm)
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

								smm.Next(Cmd.PLACE_BUF_10);
							}
						}
						break;

					case Cmd.PLACE_BUF_10:
						{
							if (arrMt[Axis.Z].IsRdy() == false) break;

							smm.Next(Cmd.PLACE_BUF_20);
						}
						break;

					case Cmd.PLACE_BUF_20:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false ||
								CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}

							smm.Next(Cmd.PLACE_BUF_30);
						}
						break;

					case Cmd.PLACE_BUF_30:
						{
							switch (CheckProdExist())
							{
								case ProdExist.Mismatch:
									AddAlarm(ALM.ERROR_SEQ_LOAD_PnP_PROD_MISMATCH);
									break;
								case ProdExist.None:
									AddAlarm(ALM.ERROR_SEQ_LOAD_PnP_PROD_NONE);
									break;
								default:
									smm.Next(Cmd.PLACE_BUF_40);
									break;
							}
						}
						break;

					case Cmd.PLACE_BUF_40:
						{
							switch (m_nCurBuffer)
							{
								case 0:
								case 1:
									{

										if (arrMt[Axis.X].IsRdy((int)PosX.BUFFER1 + m_nCurBuffer) == false ||
											arrMt[Axis.Y].IsRdy((int)PosY.BUFFER) == false||
											arrMt[Axis.W].IsRdy((int)PosW.BUFFER) == false)
										{
											arrMt[Axis.X].MoveIndex((int)PosX.BUFFER1 + m_nCurBuffer);
											arrMt[Axis.Y].MoveIndex((int)PosY.BUFFER);
											arrMt[Axis.W].MoveIndex((int)PosW.BUFFER);
											break;
										}

										m_nPlaceBuffer = m_nCurBuffer;

                                        smm.Next(Cmd.PLACE_BUF_50);
									}
									break;
								case -1:
                                    {
                                        // 준비된 버퍼가 있을 때까지 대기
                                        if (m_nBeforePlaceBuffer == -1)
                                        {
                                            if (arrMt[Axis.X].IsRdy((int)PosX.READY) == false ||
                                                arrMt[Axis.Y].IsRdy((int)PosY.BUFFER) == false ||
                                                arrMt[Axis.W].IsRdy((int)PosW.BUFFER) == false)
                                            {
                                                arrMt[Axis.X].MoveIndex((int)PosX.READY);
                                                arrMt[Axis.Y].MoveIndex((int)PosY.BUFFER);
                                                arrMt[Axis.W].MoveIndex((int)PosW.BUFFER);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //이전에 대기한곳의 반대에서 대기
                                            if (arrMt[Axis.X].IsRdy((int)PosX.BUFFER1 + m_nBeforePlaceBuffer) == false ||
                                                arrMt[Axis.Y].IsRdy((int)PosY.BUFFER) == false ||
                                                arrMt[Axis.W].IsRdy((int)PosW.BUFFER) == false)
                                            {
                                                arrMt[Axis.X].MoveIndex((int)PosX.BUFFER1 + m_nBeforePlaceBuffer);
                                                arrMt[Axis.Y].MoveIndex((int)PosY.BUFFER);
                                                arrMt[Axis.W].MoveIndex((int)PosW.BUFFER);
                                                break;
                                            }
                                        }

										m_nPlaceBuffer = m_nCurBuffer;
									}
									break;
							}
						}
						break;

					case Cmd.PLACE_BUF_50:
						{
							if (arrMt[Axis.Z].IsRdy((int)PosZ.BUFFER) == false ||
								CheckCylUpDown(SenPos.PICKER_DOWN) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)PosZ.BUFFER);
								RunCylUpDown(SenPos.PICKER_DOWN);
								break;
							}


                            m_nBeforePlaceBuffer = m_nPlaceBuffer == 0 ? 1 : 0;
                            smm.Next(Cmd.PLACE_BUF_60);
						}
						break;

                    case Cmd.PLACE_BUF_60:
                        {
                            if (smm.Delayms(50) == false) break;
                            
                            if (CheckEjector(SenPos.BLOW) == false)
                            {
                                RunEjector(SenPos.BLOW);
                                break;
                            }
                            smm.Next(Cmd.PLACE_BUF_61);
                        }
                        break;
                    case Cmd.PLACE_BUF_61:
                        {
                            FlowPart flow = FlowPart.Empty;
                            if (m_nPlaceBuffer == 0) flow = FlowPart.LDBuffer1;
                            else flow = FlowPart.LDBuffer2;
                            SM.FMP.Next(FlowPart.LDPnP, flow);

                            m_nPlaceDone = true;
                            smm.Next(Cmd.PLACE_BUF_70);
                        }
                        break;

                    case Cmd.PLACE_BUF_70:
                        {
                            if (m_nPlaceBuffer == 0 && G.seqLoadBuffer1.IsBufferGrip() == false)        break;
                            if (m_nPlaceBuffer == 1 && G.seqLoadBuffer2.IsBufferGrip() == false)        break;

                            if (CheckCylUpDown(SenPos.PICKER_UP) == false ||
								arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
								break;
							}


							smm.Next(Cmd.PLACE_BUF_80);
						}
						break;

					case Cmd.PLACE_BUF_80:
						{
							if (CheckProdExist() == ProdExist.Mismatch)
							{
								AddAlarm(ALM.ERROR_SEQ_LOAD_PnP_PROD_MISMATCH);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
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

		private void PickTrayRetry(SMM<Cmd> smm)
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

								smm.Next(Cmd.PICK_RETRY_10);
							}
						}
						break;

					case Cmd.PICK_RETRY_10:
						{
							if (SM.FMP[FlowPart.LDPnP].GetProdExist()[m_nPickRetryNo] == false &&
									 G.cKernel.IsAIn(DI.LDPnPVac1 + m_nPickRetryNo) == false)
							{
								smm.Next(Cmd.PICK_RETRY_20);
							}
							else if (SM.FMP[FlowPart.LDPnP].GetProdExist()[m_nPickRetryNo] == true &&
									 G.cKernel.IsAIn(DI.LDPnPVac1 + m_nPickRetryNo) == false)
							{
								frmMessageBox msg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, "CHECK", "Sensors are not detected, only data is present.\nDo you want to move the data to the tray?");

								if (msg.ShowDialog() == DialogResult.Cancel) smm.Next(Cmd.END);
								else smm.Next(Cmd.PICK_RETRY_20);
							}
							else
							{
								frmMessageBox msg = new frmMessageBox(frmMessageBox.teButtons.OK, "ERROR", "This is not a condition to pick.");
								msg.ShowDialog();
								smm.Next(Cmd.END);
							}
						}
						break;

					case Cmd.PICK_RETRY_20:
						{
							// 중요 : Pick 시퀀스에서 m_ActPicker.Get(m_nPickRetryNo).PocketNo 값이 남아 있어야 재시도 가능
							if (m_ActPicker.Get(m_nPickRetryNo).PocketNo < 0)
							{
								frmMessageBox msg = new frmMessageBox(frmMessageBox.teButtons.OK, "ERROR", "Tray Pocket number is missing.");
								msg.ShowDialog();
								smm.Next(Cmd.END);
							}
							else
							{
								FlowMngMoveToTray(m_nSeqUsingTable, m_ActPicker.GetPocketNo(m_nPickRetryNo), m_nPickRetryNo);

								smm.Next(Cmd.PICK_RETRY_30);
							}
						}
						break;

					case Cmd.PICK_RETRY_30:
						{
							
							if (CheckEjector(SenPos.BLOW, m_nPickRetryNo) == false)
							{
								RunEjector(SenPos.BLOW, m_nPickRetryNo);
								break;
							}

							smm.Next(Cmd.PICK_RETRY_40);
						}
						break;

					case Cmd.PICK_RETRY_40:
						{	
							if (arrMt[Axis.Z].IsRdy((int)m_TrayZPos) == false ||
								CheckCylUpDown(SenPos.PICKER_DOWN, m_nPickRetryNo) == false)
							{
								arrMt[Axis.Z].MoveIndex((int)m_TrayZPos);
								RunCylUpDown(SenPos.PICKER_DOWN, m_nPickRetryNo);
								break;
							}

							smm.Next(Cmd.PICK_RETRY_50);
						}
						break;

					case Cmd.PICK_RETRY_50:
						{
							if (smm.Delayms(100) == false) break;

                            if (smm.Delayms(NDm.Read(eNDM.LoadPnPVacDelay)) == true)
                            {
                                smm.ResetDelayms();
                                CheckEjector(SenPos.VAC, m_nPickRetryNo, true);
                                break;
                            }

							if (CheckEjector(SenPos.VAC, m_nPickRetryNo) == false)
							{
								RunEjector(SenPos.VAC, m_nPickRetryNo);
								break;
							}

							smm.Next(Cmd.PICK_RETRY_60);
						}
						break;

					case Cmd.PICK_RETRY_60:
						{
							//if (smm.Delayms(NDm.Read(eNDM.LoadPnPVacDelay)) == false) break;

							RunCylUpDown(SenPos.PICKER_UP);

							FlowMngMoveToPicker(m_nSeqUsingTable, m_ActPicker.GetPocketNo(m_nPickRetryNo), m_nPickRetryNo);

							smm.Next(Cmd.PICK_RETRY_70);
						}
						break;

					case Cmd.PICK_RETRY_70:
						{
							if (CheckCylUpDown(SenPos.PICKER_UP) == false)
							{
								RunCylUpDown(SenPos.PICKER_UP);
								break;
							}

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
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

		private void PnP(SMM<Cmd> smm)
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

								smm.Next(Cmd.PnP_10);
							}
						}
						break;

					case Cmd.PnP_10:
						{
							if (IsPickTrayReady() == false) break;

							m_smmPickTray.eInput = InState.run;

							smm.Next(Cmd.PnP_20);
						}
						break;

					case Cmd.PnP_20:
						{
							if (IsPickTrayReady() == false) break;
							if (CheckLoadPnPFull() == false)
							{
								smm.Next(Cmd.END);
								break;
							}

							// UPH 측정 시작
							if (SM.SHM.IsRunUPH() == true)
							{
								DateTime dt = DateTime.Now;
								SM.SHM.SetUPHTime(UPHStep.FirstInsp, dt);
							}
							m_smmPlaceBuf.eInput = InState.run;

							smm.Next(Cmd.PnP_30);
						}
						break;

					case Cmd.PnP_30:
						{
							if (IsPlaceBufReady() == false) break;

							smm.Next(Cmd.END);
						}
						break;

					case Cmd.END:
						{
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

		private void EmptyCheck(SMM<Cmd> smm)
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

								smm.Next(Cmd.EMPTY_CHECK_10);
							}
						}
						break;

					case Cmd.EMPTY_CHECK_10:
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SmartVisionTable) == false)
							{
								FlowTray Table;
								if (m_nEmptyCheckTray == 0) Table = FlowTray.LDTable1;
								else Table = FlowTray.LDTable2;

								SM.FMT[Table].ProcStatus = ProcStatus.TrayEmptyDone;
								smm.Next(Cmd.END);
								break;
							}

                            RunCylUpDown(SenPos.PICKER_UP);
                            if (arrMt[Axis.Z].IsRdy((int)PosZ.READY) == false)
                            {
                                arrMt[Axis.Z].MoveIndex((int)PosZ.READY);
                                break;
                            }


                            if (m_nEmptyCheckTray == 0)
                            {
                                m_nEmptyCheckPosX = PosX.BUFFER1;
                                m_nEmptyCheckStartPosY = PosY.TABLE2_FIRST;

                            }
                            else
                            {
                                m_nEmptyCheckPosX = PosX.BUFFER1;
                                m_nEmptyCheckStartPosY = PosY.TABLE1_FIRST;
                            }

                            smm.Next(Cmd.EMPTY_CHECK_20);
						}
						break;

					case Cmd.EMPTY_CHECK_20:
						{
							if (m_nEmptyCheckTray == 0)
							{
                                if (arrMt[Axis.Y].IsRdy((int)m_nEmptyCheckStartPosY) == false ||
                                    arrMt[Axis.X].IsRdy((int)m_nEmptyCheckPosX) == false ||
                                    arrAct[Act.SmartCam].IsPos(SenPos.BWD) == false)
                                {
                                    arrMt[Axis.Y].MoveIndex((int)m_nEmptyCheckStartPosY);
                                    arrMt[Axis.X].MoveIndex((int)m_nEmptyCheckPosX);
                                    arrAct[Act.SmartCam].SetPos(SenPos.BWD);

                                    break;
                                }
							}
							else
							{
                                if (arrMt[Axis.Y].IsRdy((int)m_nEmptyCheckStartPosY) == false ||
                                    arrMt[Axis.X].IsRdy((int)m_nEmptyCheckPosX) == false ||
                                    arrAct[Act.SmartCam].IsPos(SenPos.FWD) == false)
                                {
                                    arrMt[Axis.Y].MoveIndex((int)m_nEmptyCheckStartPosY);
                                    arrMt[Axis.X].MoveIndex((int)m_nEmptyCheckPosX);
                                    arrAct[Act.SmartCam].SetPos(SenPos.FWD);

                                    break;
                                }
							}

							smm.Next(Cmd.EMPTY_CHECK_30);
						}
						break;

					case Cmd.EMPTY_CHECK_30:
						{
							m_smmFTVInspRetry = 0;
							smm.Next(Cmd.EMPTY_CHECK_40);
						}
						break;

					case Cmd.EMPTY_CHECK_40:
						{
                            if (smm.Delayms(500) == false) break;

							G.commFTVTable.Send(CommFTV.IV4Command.TriggerAndGetResult);

							smm.Next(Cmd.EMPTY_CHECK_50);
						}
						break;

					case Cmd.EMPTY_CHECK_50:
						{
							//스캔 확인
							if (G.commFTVTable.TriggerRcvCheck == false)
							{
								if (smm.Delayms(1000) == false) break;

								smm.Next(Cmd.EMPTY_CHECK_60);

								break;
							}

							switch (G.commFTVTable.RstEmptyCheck)
							{
								case CommFTV.FTVResult.DATA_NG:
									{
										AddAlarm(ALM.ERROR_COMM_FTV_DATA);
										smm.Next(Cmd.END);
										return;
									}
								case CommFTV.FTVResult.RESULT_NG:
                                    {
                                        if (m_nEmptyCheckTray == 0) AddAlarm(ALM.ERROR_SEQ_LOAD_PnP_PRODUCT_EXIST_IN_EMPTY_TRAY1_ROW1);
                                        else                        AddAlarm(ALM.ERROR_SEQ_LOAD_PnP_PRODUCT_EXIST_IN_EMPTY_TRAY2_ROW1);
                                        
										smm.Next(Cmd.END);
										return;
									}
								case CommFTV.FTVResult.OK:
									{
										FlowTray Table;
										if (m_nEmptyCheckTray == 0 ) Table = FlowTray.LDTable1;
										else						 Table = FlowTray.LDTable2;

										SM.FMT[Table].ProcStatus = ProcStatus.TrayEmptyDone;
										smm.Next(Cmd.END);
									}
									break;
							}
						}
						break;
					case Cmd.EMPTY_CHECK_60:
						{
							m_smmFTVInspRetry++;
							if (m_smmFTVInspRetry >= 3) // 검사결과 체크 3회 실패시 알람후 종료.
							{
								AddAlarm(ALM.ERROR_MAIN_SEQ_FTV_INSP_RESULT_CHECK);
								smm.Next(Cmd.END);
								break;
							}

							smm.Next(Cmd.EMPTY_CHECK_40);
						}
						break;


					case Cmd.END:
						{

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
