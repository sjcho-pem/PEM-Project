using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	//구조체에 값 추가시 frmFlowMapInfo 의 SetValue에 해당 값을 추가
	public	struct	stProdInfo
	{
		public	bool	Exist;
		public	int		TrayRow;
		public	int		TrayCol;
		public	int		ResultTop;
		public	int		ResultBtm;
		public	int		ResultMES;
		public	bool	VerifyTopDone;
		public	bool	VerifyBtmDone;
		public int HandlingStatus;
		public	int		PickStatus;
		
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
		public  string  KeyValue;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
		public  string  TrayID;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
		public	string	TrayBarcode;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
		public  string  ProdID;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
		public  string  InspTimeTop;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
		public  string  InspTimeBtm;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
		public  string  SerialNumber;
	}

	public	class	InfoTray
	{
		private	const int	MaxProdCnt	= 200;

		#region Struct & Event
		
		//구조체에 값 추가시 frmFlowMapInfo 의 SetValue에 해당 값을 추가, 열거형으로 표현하고 싶으면 m_enumTypeMap에도 추가
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct tsInfo
		{
			public	bool	LastTrayOfLot;
			public	int		AutoRunMode;
			public	int		TrayNo;
			public	int		MaxCntTray;
			public	int		MaxCntProdInTray;
			public	int		MaxCntProdInLot;
			public	int		ProcStatus;
			public	int		ColCnt;
			public	int		RowCnt;
			public	long	TransTime;
			public	long	StartTime;
			public	long	InTime;
			public	long	OutTime;


			
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
			public	string	ModuleType;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
			public	string	ModuleGen;
			
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
			public	string	WorkOrder; //Lot ID
			
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
			public	string	KeyValue;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
			public	string	TrayID;
			
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public	string	ProgramID_Top; //Aoi Recipe

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public	string	ProgramID_Btm; 
			
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public	string	TrayBarcode;

			[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = MaxProdCnt)]
			public	stProdInfo[]	ProdInfo;

			public	tsInfo DeepCopy()
			{
				tsInfo copy = this;
				if (this.ProdInfo != null)
				{
				    copy.ProdInfo = (stProdInfo[])this.ProdInfo.Clone(); // 배열의 깊은 복사
				}

				return copy;
			}
		}
		private	tsInfo	m_Info	= new tsInfo();

		/// <summary>
		/// 구조체 배열 현재 인덱스
		/// </summary>
		private	int		_iIndex;
		public	int		m_iIndex
		{
			set => _iIndex = value;
			get => _iIndex;
		}

		public	delegate void	UpdateEventHandler(int Index);
		public	event			UpdateEventHandler UpdateEvent;
		protected	void	OnUpdateEvent()
		{
			if (UpdateEvent != null)			UpdateEvent(_iIndex);
		}
		#endregion Struct & Event  END

		#region Default Functions
		public			InfoTray(int _idx)
		{
			_iIndex			= _idx;

			InitInfo();
		}

		public	tsInfo	GetInfo()
		{
			return m_Info;
		}

		
		public	void	SetInfo(tsInfo _info)
		{
			m_Info = _info;

			OnUpdateEvent();
		}

		public	void	SetTrayBarcode(string strBarcode)
		{
			m_Info.TrayBarcode = strBarcode;
			for (int i = 0; i < m_Info.ProdInfo.Length; i++)
			{
				m_Info.ProdInfo[i].TrayBarcode = strBarcode;
			}		
			OnUpdateEvent();
		}
		
		
		public	void	SetAutoRunMode(AutoRunMode mode)
		{
			m_Info.AutoRunMode = (int)mode;

			OnUpdateEvent();
		}

		public	void	InitInfo()
		{
			try
			{
				m_Info.LastTrayOfLot	= false;
				m_Info.TrayNo			= 0;
				m_Info.MaxCntTray		= 0;
				m_Info.MaxCntProdInTray	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000 * SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				m_Info.MaxCntProdInLot	= 0;
				m_Info.ProcStatus		= (int)ProcessState.UNKNOWN;
				m_Info.WorkOrder		= "";
				m_Info.KeyValue			= "";
				m_Info.ProgramID_Top	= "";
				m_Info.ProgramID_Btm	= "";
				m_Info.TrayID			= "";
				m_Info.TrayBarcode		= "";
				m_Info.ColCnt			= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
				m_Info.RowCnt			= SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				m_Info.ModuleType		= SM.SHM.GetRecipeData(DEVICE.ModuleType);
				m_Info.ProdInfo			= new stProdInfo[MaxProdCnt];
				m_Info.InTime		= 0;
				m_Info.OutTime		= 0;

				if (_iIndex == 0)
				{
					m_Info.MaxCntProdInTray	= 0;
					m_Info.ColCnt			= 0;
					m_Info.RowCnt			= 0;
				}

				for (int i = 0; i < m_Info.ProdInfo.Length; i++)
				{
					ClearProductData(i);
				}

				OnUpdateEvent();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion Default Functions END


		//------------------------- 자유롭게 함수 추가 -----------------------------------
		#region ADD Functions
		public  ProcStatus ProcStatus
		{
			set
			{
				if (m_Info.ProcStatus == (int)value)	return;
				
				m_Info.ProcStatus = (int)value;
				if (value == ProcStatus.TrayFillDone)
				{
					GLog.Add(string.Format("Tray Proc {0} = {1}", m_Info.TrayID, value.ToString()));
					m_Info.TransTime = DateTime.Now.Ticks;
				}
				OnUpdateEvent();	
			}
			get => (ProcStatus)m_Info.ProcStatus;
		}

		public	void	SetInfo(int nTrayNo, int nCntRow, int nCntCol,string nModuleType, string strWorkOrder, long strStartTime, string strProgramID_Top, string strProgramID_Btm, int nMaxCntProdInLot, int nMaxCntTray, int nCntProdinTray, bool bLidUse , string nModuleGen, string strTrayBarcode)
		{
			try
			{
				TimeSpan time = TimeSpan.FromTicks(strStartTime);
				string ticksString = time.Ticks.ToString();
				m_Info.LastTrayOfLot	= false;
				m_Info.TrayNo			= nTrayNo;
				m_Info.MaxCntTray		= nMaxCntTray;
				m_Info.MaxCntProdInTray	= nCntRow * nCntCol;
				m_Info.MaxCntProdInLot	= nMaxCntProdInLot;
				m_Info.ProcStatus		= (int)ProcStatus.Unknown;
				m_Info.WorkOrder		= strWorkOrder;
				m_Info.KeyValue			= $"{time.Hours.ToString("00")}{time.Minutes.ToString("00")}{time.Seconds.ToString("00")}{time.Milliseconds.ToString("000")}";
				m_Info.ProgramID_Top	= strProgramID_Top;
				m_Info.ProgramID_Btm	= strProgramID_Btm;
				m_Info.TrayID			= string.Format("{0}_{1:00}", strWorkOrder, nTrayNo);
				m_Info.ColCnt			= nCntCol;
				m_Info.RowCnt			= nCntRow;
				m_Info.StartTime		= strStartTime;
				m_Info.ModuleType		= nModuleType;
				m_Info.ModuleGen		= nModuleGen;
				m_Info.TrayBarcode		= strTrayBarcode;
				m_Info.InTime		= strStartTime;
				m_Info.OutTime		= 0;
				m_Info.AutoRunMode		= 0;
				
				for (int i = 0; i < nCntProdinTray; i++)
				{
					int nIdx = SM.SHM.GetOriginToTransIdx(i, m_Info.ModuleType);
					m_Info.ProdInfo[nIdx].Exist				= true;
					m_Info.ProdInfo[nIdx].TrayRow			= nIdx / nCntCol;
					m_Info.ProdInfo[nIdx].TrayCol			= nIdx % nCntCol;
					m_Info.ProdInfo[nIdx].ResultTop			= (int)ProdStatus.Unknown;
					m_Info.ProdInfo[nIdx].ResultBtm			= (int)ProdStatus.Unknown;
					m_Info.ProdInfo[nIdx].ResultMES			= (int)MESStatus.Unknown;
					m_Info.ProdInfo[nIdx].VerifyTopDone		= false;
					m_Info.ProdInfo[nIdx].VerifyBtmDone		= false;
					m_Info.ProdInfo[nIdx].TrayID			= m_Info.TrayID;
					m_Info.ProdInfo[nIdx].KeyValue			= m_Info.KeyValue;
					m_Info.ProdInfo[nIdx].ProdID			= string.Format("{0}_{1}_{2:000}", m_Info.KeyValue, m_Info.TrayID,i+1);
					m_Info.ProdInfo[nIdx].InspTimeTop		= "";
					m_Info.ProdInfo[nIdx].InspTimeBtm		= "";
					m_Info.ProdInfo[nIdx].SerialNumber		= "";
					m_Info.ProdInfo[nIdx].PickStatus		= (int)ProdPickStatus.Normal;
				}

				OnUpdateEvent();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	SetProd(int nIdx, stProdInfo ProdInfo)
		{
			try
			{
				if (nIdx > m_Info.ProdInfo.Length-1)			return false;

				m_Info.ProdInfo[nIdx]	= ProdInfo;

				OnUpdateEvent();

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	void	SetLastOfLot(bool bState)
		{
			try
			{
				m_Info.LastTrayOfLot	= bState;

				OnUpdateEvent();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	ClearProd(int nIdx)
		{
			try
			{
				if (ClearProductData(nIdx) == false)			return false;

				OnUpdateEvent();

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	void	ClearProdAll()
		{
			try
			{
				for (int i = 0; i < m_Info.ProdInfo.Length; i++)
				{
					ClearProductData(i);
				}

				OnUpdateEvent();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	int		GetExistCount()
		{
			try
			{
				int	ncnt	= 0;

				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (m_Info.ProdInfo[i].Exist == true)			ncnt++;
				}
				
				return ncnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}

		
		public	int		GetExistCount(string KeyValue)
		{
			try
			{
				int	ncnt	= 0;

				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (m_Info.ProdInfo[i].Exist == true && m_Info.ProdInfo[i].KeyValue == KeyValue)			ncnt++;
				}
				
				return ncnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}

		public	bool	IsResultExist(ProdStatus inspResult)
		{
			try
			{
				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (m_Info.ProdInfo[i].Exist == true)
					{
						if (inspResult == ProdStatus.OK)
						{
							if (m_Info.ProdInfo[i].ResultTop == (int)inspResult &&
								m_Info.ProdInfo[i].ResultBtm == (int)inspResult)
							{
								//mes 사용시엔 mes도 ok여야 ok로 판정
								if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true )
								{
									if (m_Info.ProdInfo[i].ResultMES == (int)MESStatus.OK)
										return true;
									else
										return false;

								}
								else
									return true;
							}
						}
						else
						{
							//mes 사용시엔 mes ng도 ng로 판정
							if (m_Info.ProdInfo[i].ResultTop == (int)inspResult ||
								m_Info.ProdInfo[i].ResultBtm == (int)inspResult ||
								(SM.SHM.GetUseSkipData(USESKIP.SGEM) == true && m_Info.ProdInfo[i].ResultMES == (int)MESStatus.NG))
							{
								return true;
							}
						}
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		
		public	bool	SetVerifyResult(string strProdID, ProdStatus Result, bool bTop)
		{
			try
			{
				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (m_Info.ProdInfo[i].ProdID == strProdID  )
					{
						//바코드, 피두셜, MES NG인 경우 컨펌 따로 안오는데, 혹시 몰라서 결과 안바뀌도록 처리
						if (m_Info.ProdInfo[i].ResultMES == (int)MESStatus.NG  || 
							m_Info.ProdInfo[i].SerialNumber.ToLower().Contains("error") ==  true)
						{
							m_Info.ProdInfo[i].VerifyTopDone	= true;
							m_Info.ProdInfo[i].VerifyBtmDone	= true;
							return true;
						}
					
						if (bTop == true)
						{
							m_Info.ProdInfo[i].ResultTop		= (int)Result;
							m_Info.ProdInfo[i].VerifyTopDone	= true;
						}
						else
						{
							m_Info.ProdInfo[i].ResultBtm		= (int)Result;
							m_Info.ProdInfo[i].VerifyBtmDone	= true;
						}
						
						OnUpdateEvent();

						return true;
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	int		GetVerifyCount(ProdStatus ProdStatus = ProdStatus.Max)
		{
			try
			{
				int	ncnt	= 0;

				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (ProdStatus == ProdStatus.Max)
					{
						if (m_Info.ProdInfo[i].Exist == true  &&
							CheckVerifyComplete(i) == true)			ncnt++;
					}
					else
					{
						if (GetProdStatus(i) == ProdStatus  &&
							CheckVerifyComplete(i) == true)				ncnt++;
					}
				}
				
				return ncnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}

		public	int		GetVerifyNotCount(ProdStatus ProdStatus = ProdStatus.Max)
		{
			try
			{
				int	ncnt	= 0;

				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (ProdStatus == ProdStatus.Max)
					{
						if (m_Info.ProdInfo[i].Exist == true  &&
							CheckVerifyComplete(i) == false)			ncnt++;
					}
					else
					{
						if (GetProdStatus(i) == ProdStatus  &&
							CheckVerifyComplete(i) == false)				ncnt++;
					}
				}
				
				return ncnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}
		
		public	bool	CheckVerifyComplete(int nIndex = -1)
		{
			try
			{
				if (nIndex < 0)
				{
					for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
					{
						if (m_Info.ProdInfo[i].Exist == true)
						{
							if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true && m_Info.ProdInfo[i].ResultMES == (int)MESStatus.NG )					continue;
							if ((ProdStatus)m_Info.ProdInfo[i].ResultTop == ProdStatus.NG  &&  m_Info.ProdInfo[i].VerifyTopDone == false)			return false;
							if ((ProdStatus)m_Info.ProdInfo[i].ResultBtm == ProdStatus.NG  &&  m_Info.ProdInfo[i].VerifyBtmDone == false)			return false;
						}
					}
				}
				else
				{
					if (m_Info.ProdInfo[nIndex].Exist == true)
					{
						if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true && m_Info.ProdInfo[nIndex].ResultMES == (int)MESStatus.NG )						return true;
						if ((ProdStatus)m_Info.ProdInfo[nIndex].ResultTop == ProdStatus.NG  &&  m_Info.ProdInfo[nIndex].VerifyTopDone == false)			return false;
						if ((ProdStatus)m_Info.ProdInfo[nIndex].ResultBtm == ProdStatus.NG  &&  m_Info.ProdInfo[nIndex].VerifyBtmDone == false)			return false;
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

		
		public	ProdColorResult	GetProdColorResult(int nIdx)
		{
			try
			{
				if (m_Info.ProdInfo[nIdx].Exist == false)			return ProdColorResult.None;
				
				ProdStatus TopResult = (ProdStatus)m_Info.ProdInfo[nIdx].ResultTop;
				ProdStatus BtmResult = (ProdStatus)m_Info.ProdInfo[nIdx].ResultBtm;

				if		(TopResult == ProdStatus.Unknown  ||
						 BtmResult == ProdStatus.Unknown  ||
						 TopResult == ProdStatus.None  ||
						 BtmResult == ProdStatus.None)
				{
					return ProdColorResult.Unknown;
				}
				else if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true && (ProdStatus)m_Info.ProdInfo[nIdx].ResultMES == (int)MESStatus.NG )
				{
					return ProdColorResult.USERNG;
				}
				else if (TopResult == ProdStatus.NG  ||  BtmResult == ProdStatus.NG)
				{
					bool bIsTopDone = TopResult == ProdStatus.OK || (TopResult == ProdStatus.NG && m_Info.ProdInfo[nIdx].VerifyTopDone);
					bool bIsBtmDone = BtmResult == ProdStatus.OK || (BtmResult == ProdStatus.NG && m_Info.ProdInfo[nIdx].VerifyBtmDone);

					if (bIsTopDone && bIsBtmDone)
					{
						return ProdColorResult.USERNG;
					}
					
					return ProdColorResult.NG;
				}
				else
				{	
					if (m_Info.ProdInfo[nIdx].VerifyTopDone == true || m_Info.ProdInfo[nIdx].VerifyBtmDone == true )
					{
						return ProdColorResult.USEROK;
					}
					return ProdColorResult.OK;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return ProdColorResult.Unknown;
			}
		}

		public	int		GetEmptyCount()
		{
			try
			{
				int	ncnt	= 0;

				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (m_Info.ProdInfo[i].Exist == false)			ncnt++;
				}
				
				return ncnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}
		
		public	int		GetExistCount(ProdStatus ProdStatus = ProdStatus.Max)
		{
			try
			{
				int	ncnt	= 0;

				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (ProdStatus == ProdStatus.Max)
					{
						if (m_Info.ProdInfo[i].Exist == true)			ncnt++;
					}
					else
					{
						if (GetProdStatus(i) == ProdStatus )				ncnt++;
					}
				}
				
				return ncnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}

		public	int		GetVerifyDoneCount()
		{
			try
			{
				int		ncnt		= 0;
				bool[]	bverify_done	= new bool[2];

				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{	
					if (m_Info.ProdInfo[i].Exist == true)
					{
						bverify_done[0]	= false;
						bverify_done[1]	= false;

						if (m_Info.ProdInfo[i].VerifyTopDone == true  ||
							m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.OK)			bverify_done[0]	= true;

						if (m_Info.ProdInfo[i].VerifyBtmDone == true  ||
							m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.OK)			bverify_done[1]	= true;

						if (bverify_done[0] == true  &&  bverify_done[1] == true)			ncnt++;
					}
				}

				return ncnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

		public	void	SetVerifyDone(int nIdx, bool bTopDone, bool bBtmDone)
		{
			try
			{
				m_Info.ProdInfo[nIdx].VerifyTopDone	= bTopDone;
				m_Info.ProdInfo[nIdx].VerifyBtmDone	= bBtmDone;

				OnUpdateEvent();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void SetTrayHandlingStatus(ProdHandlingStatus status)
		{
			for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
			{
				if (m_Info.ProdInfo[i].Exist == true)
				{
					m_Info.ProdInfo[i].HandlingStatus = (int)status;
				}
			}
		}

		public bool IsAlignDone()
		{
			for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
			{
				if (m_Info.ProdInfo[i].Exist == true &&
					m_Info.ProdInfo[i].HandlingStatus != (int)ProdHandlingStatus.Aligned)
				{
					return false;
				}
			}
			return true;
		}

		public bool IsAlignDone(int nIdx)
		{
			if (m_Info.ProdInfo[nIdx].Exist == true &&
				m_Info.ProdInfo[nIdx].HandlingStatus != (int)ProdHandlingStatus.Aligned)
			{
				return false;
			}

			return true;
		}

		public	ProdStatus	GetProdStatus(int nIdx)
		{
			try
			{
				if (m_Info.ProdInfo[nIdx].Exist == false)			return ProdStatus.None;

				if		((ProdStatus)m_Info.ProdInfo[nIdx].ResultTop == ProdStatus.Unknown  ||
						 (ProdStatus)m_Info.ProdInfo[nIdx].ResultBtm == ProdStatus.Unknown  ||
						 (ProdStatus)m_Info.ProdInfo[nIdx].ResultTop == ProdStatus.None  ||
						 (ProdStatus)m_Info.ProdInfo[nIdx].ResultBtm == ProdStatus.None)
				{
					return ProdStatus.Unknown;
				}
				else if ((ProdStatus)m_Info.ProdInfo[nIdx].ResultTop == ProdStatus.NG ||
						 (ProdStatus)m_Info.ProdInfo[nIdx].ResultBtm == ProdStatus.NG)
				{
					return ProdStatus.NG;
				}
				else if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true && 
					     (MESStatus)m_Info.ProdInfo[nIdx].ResultMES == MESStatus.NG)
				{
					return ProdStatus.NG;
				}
				else
				{
					return ProdStatus.OK;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return ProdStatus.Unknown;
			}
		}

		public	bool	IsFull()
		{
			try
			{
				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (m_Info.ProdInfo[i].Exist == false)			return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsEmpty()
		{
			try
			{
				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					if (m_Info.ProdInfo[i].Exist == true)			return false;
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		
		public	int	GetVerifyyNGCount()
		{
			try
			{
				int nCount = 0;
				for (int i = 0; i < m_Info.MaxCntProdInTray; i++)
				{
					
					if (m_Info.ProdInfo[i].VerifyBtmDone == true  && 
						m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.NG &&
						m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.OK)
					{
						nCount++;
						continue;
					}
					if (m_Info.ProdInfo[i].VerifyTopDone == true  && 
						m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.NG && 
						m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.OK)
					{
						nCount++;
						continue;
					}
					if (m_Info.ProdInfo[i].VerifyTopDone == true  && 
						m_Info.ProdInfo[i].VerifyBtmDone == true  && 
						m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.OK && 
						m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.NG)
					{
						nCount++;
						continue;
					}
					if (m_Info.ProdInfo[i].VerifyTopDone == true  && 
						m_Info.ProdInfo[i].VerifyBtmDone == true  && 
						m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.NG && 
						m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.OK)
					{
						nCount++;
						continue;
					}
					if (m_Info.ProdInfo[i].VerifyTopDone == true  && 
						m_Info.ProdInfo[i].VerifyBtmDone == true  && 
						m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.NG && 
						m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.NG)
					{
						nCount++;
						continue;
					}
					if (SM.SHM.GetUseSkipData(USESKIP.SGEM)  &&
						m_Info.ProdInfo[i].ResultMES == (int)MESStatus.NG)
					{
						nCount++;
						continue;
					}
				}

				return nCount;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}

		private	bool	ClearProductData(int nIdx)
		{
			try
			{
				if (nIdx > m_Info.ProdInfo.Length-1)			return false;
				
				m_Info.ProdInfo[nIdx].Exist			= false;
				m_Info.ProdInfo[nIdx].TrayRow		= 0;
				m_Info.ProdInfo[nIdx].TrayCol		= 0;
				m_Info.ProdInfo[nIdx].ResultTop		= (int)ProdStatus.Unknown;
				m_Info.ProdInfo[nIdx].ResultBtm		= (int)ProdStatus.Unknown;
				m_Info.ProdInfo[nIdx].ResultMES		= (int)MESStatus.Unknown;
				m_Info.ProdInfo[nIdx].VerifyTopDone	= false;
				m_Info.ProdInfo[nIdx].VerifyBtmDone	= false;
				m_Info.ProdInfo[nIdx].TrayID		= "";
				m_Info.ProdInfo[nIdx].ProdID		= "";
				m_Info.ProdInfo[nIdx].KeyValue		= "";
				m_Info.ProdInfo[nIdx].InspTimeTop	= "";
				m_Info.ProdInfo[nIdx].InspTimeBtm	= "";
				m_Info.ProdInfo[nIdx].SerialNumber	= "";
				m_Info.ProdInfo[nIdx].TrayBarcode	= "";
				m_Info.ProdInfo[nIdx].PickStatus	= (int)ProdPickStatus.Normal;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		#endregion ADD Functions END
	}

	public	class	FlowMngTray : MemMappedFile
	{
		#region Variables

		private	bool[]		m_arrUse;
		private	InfoTray[]	m_arrMap;
		private	EnumArray<FlowTray, int>	m_arrIdx;

		#endregion Variables END

		#region Default Functions
		public				FlowMngTray()
		{
			#region 사이즈 설정, 버퍼 초기화
			SIZE_ENUM	= (int)FlowTray.Max;
			SIZE_STRUCT	= Marshal.SizeOf(new InfoTray.tsInfo());

			m_arrUse	= new bool[SIZE_ENUM + OFFSET];
			m_arrMap	= new InfoTray[SIZE_ENUM + OFFSET];
			m_arrIdx	= new EnumArray<FlowTray, int>(SIZE_ENUM);

			for (int i = 0; i < SIZE_ENUM + OFFSET; i++)
			{
				m_arrUse[i]	= false;
				m_arrMap[i]	= new InfoTray(i);
				m_arrMap[i].UpdateEvent += Update;
			}

			for (int i = 0; i < m_arrIdx.Count; i++)
			{
				m_arrIdx[i] = 0;
			}
			#endregion

			MemCreate();

			m_arrUse[0] = true; // 0번째는 Empty로 디폴트가 True
		}

		public	InfoTray	this[FlowTray eKey]
		{
			get
			{
				return m_arrMap[m_arrIdx[eKey]];
			}
			set
			{
				m_arrMap[m_arrIdx[eKey]] = value;
				WriteInfo(m_arrMap[m_arrIdx[eKey]]);
			}
		}

		private	bool		MemCreate()
		{
			try
			{
				bool	ret	= Create("FlowMapTray");

				for (int i = 0; i < m_arrUse.Length; i++)
				{
					m_arrUse[i]	= ReadUse(i);
					m_arrMap[i].SetInfo(ReadInfo(i));
				}

				for (int i = 0; i < m_arrIdx.Count; i++)
					m_arrIdx[i]	= ReadIdx(i);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}

			return true;
		}



		/// <summary>
		/// Event Call 함수
		/// </summary>
		/// <param name="_Idx"></param>
		private	void	Update(int _Idx)
		{
			WriteInfo(m_arrMap[_Idx]);
		}

		private	void	WriteUse(int nIdx, bool bVal)
		{
			try
			{
				long	lOffset	= nIdx * sizeof(Boolean);

				m_arrUse[nIdx]	= bVal;
				Write(lOffset, bVal);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	WriteInfo(InfoTray _info)
		{
			byte[]	byBuff	= StructParsing.MethodObjectToByteSerializer(_info.GetInfo());
			Write(SIZE_USE_MAP + (_info.m_iIndex * byBuff.Length), byBuff);
		}

		private	void	WriteIdx(FlowTray flow, int nVal)
		{
			try
			{
				long	lSize	= SIZE_USE_MAP + SIZE_MAP + ((int)flow * sizeof(int));

				m_arrIdx[flow]	= nVal;
				Write(lSize, nVal);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	InfoTray.tsInfo ReadInfo(int idx)
		{
			byte[]	byBuff	= Read(SIZE_USE_MAP + (idx * SIZE_STRUCT), SIZE_STRUCT);
			return (InfoTray.tsInfo)(StructParsing.MethodByteToObjectDeserialize(byBuff, typeof(InfoTray.tsInfo)));
		}

		private	bool	ReadUse(int idx)
		{
			long	lSize	= idx * sizeof(Boolean);
			return Read(lSize);
		}

		private	int		ReadIdx(int idx)
		{
			long	lSize	= SIZE_USE_MAP + SIZE_MAP + (idx * sizeof(int));
			return IRead(lSize);
		}

		private	int		GetPushIdx()
		{
			try
			{
				int	nIdx	= Array.IndexOf(m_arrUse, false);
				//if (nIdx == -1) MessageBox.Show("FlowMng : nPush Index -1 Check Please");
				WriteUse(nIdx, true);

				return nIdx;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

		public	bool	Next(FlowTray CurIdx, FlowTray NextIdx)
		{
			try
			{
				if (m_arrIdx[NextIdx] != (int)FlowTray.Empty)			return false;
				
				switch (CurIdx)
				{
					case FlowTray.Empty:
						{
							WriteIdx(NextIdx, GetPushIdx());
							m_arrMap[m_arrIdx[NextIdx]].InitInfo();
						}
						break;
					default:
						{
							 // 없으면 안된다.
							if (m_arrIdx[CurIdx] == (int)FlowTray.Empty)			return false;

							WriteIdx(NextIdx, m_arrIdx[CurIdx]);
							WriteIdx(CurIdx, (int)FlowTray.Empty);
							
							GLog.Add(string.Format("Tray Next {0} -> {1},  ID = {2}",CurIdx, NextIdx,  m_arrMap[m_arrIdx[NextIdx]].GetInfo().TrayID));
						}
						break;
				}
				if (CurIdx >= FlowTray.LDConv01  && CurIdx <= FlowTray.LDConv31)
				{
					var tempInfo	= m_arrMap[m_arrIdx[NextIdx]].GetInfo();
					tempInfo.StartTime	= DateTime.Now.Ticks;
					m_arrMap[m_arrIdx[NextIdx]].SetInfo(tempInfo);
				}
				else if (CurIdx >= FlowTray.ULDConv01  && CurIdx <= FlowTray.ULDConv31)
				{
					var tempInfo	= m_arrMap[m_arrIdx[NextIdx]].GetInfo();
				}
				
				var info	= m_arrMap[m_arrIdx[NextIdx]].GetInfo();
				info.TransTime	= DateTime.Now.Ticks;
				m_arrMap[m_arrIdx[NextIdx]].SetInfo(info);
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		#endregion Default Functions END



		//------------------------- 자유롭게 함수 추가 -----------------------------------
		#region ADD Functions

		public	bool	NextLot(FlowTray CurIdx, FlowTray NextIdx)
		{
			try
			{
				if (m_arrIdx[NextIdx] != (int)FlowTray.Empty)			return false;

				bool	bnext_run	= false;
				int		nmax		= 0;

				switch (CurIdx)
				{
					case FlowTray.Empty:
						{
							switch(NextIdx)
							{
								case FlowTray.LDLift01:
									{
										bnext_run	= false;

										int		ncnt_col		= 0;
										int		ncnt_row		= 0;
										int		nTrayCount		= 0;
										int		nTrayResidue	= 0;
										int		nProdCount		= 0;
										string	strModuleType	= "";
										string	strModuleGen	= "";
										bool	bCapUse			= true;
										long	strStartTime	= DateTime.Now.Ticks;

										string	strWorkOrder		= string.Empty;
										string	strProgramID_Top	= string.Empty;
										string	strProgramID_Btm	= string.Empty;
										string	strCapBarcode		= string.Empty;
										string	strTrayBarcode		= string.Empty;
										string	strInTime			= string.Empty;
										string	strOutTime			= string.Empty;

										if (IsFlow(FlowTray.LDWaitBuf) == true)
										{
											ncnt_row			= SM.FMT[FlowTray.LDWaitBuf].GetInfo().RowCnt;
											ncnt_col			= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ColCnt;
											strWorkOrder		= SM.FMT[FlowTray.LDWaitBuf].GetInfo().WorkOrder;
											strProgramID_Top	= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ProgramID_Top;
											strProgramID_Btm	= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ProgramID_Btm;
											strModuleType		= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ModuleType;
											strModuleGen		= SM.FMT[FlowTray.LDWaitBuf].GetInfo().ModuleGen;
											strTrayBarcode		= SM.FMT[FlowTray.LDWaitBuf].GetInfo().TrayBarcode;
											nProdCount			= m_arrMap[m_arrIdx[(int)FlowTray.LDWaitBuf]].GetInfo().MaxCntProdInLot;
											nTrayCount			= nProdCount / (ncnt_col * ncnt_row) + 1;
											nTrayResidue		= nProdCount % (ncnt_col * ncnt_row);

											if (nTrayResidue > 0)			nTrayCount	+= 1;
										}
										else
										{
											G.AlmMng.ADD(ALM.ERROR_FLOW_MANAGER_CHECK);
										}
										for (int i = 0; i < nTrayCount; i++)
										{
											WriteIdx(NextIdx+i, GetPushIdx());

											if		(i == 0) // 맨위 트레이, 제품이 없는 경우
											{
												m_arrMap[m_arrIdx[NextIdx+i]].InitInfo();
												m_arrMap[m_arrIdx[NextIdx+i]].SetInfo(i, ncnt_row, ncnt_col, strModuleType, strWorkOrder, strStartTime, strProgramID_Top, strProgramID_Btm, nProdCount, nTrayCount, 0,bCapUse, strModuleGen, strTrayBarcode);
											}
											else if (i == 1  &&  nTrayResidue > 0) // 제품이 가득 안찬 경우
											{
												m_arrMap[m_arrIdx[NextIdx+i]].SetInfo(i, ncnt_row, ncnt_col, strModuleType, strWorkOrder, strStartTime, strProgramID_Top, strProgramID_Btm, nProdCount, nTrayCount, nTrayResidue, bCapUse, strModuleGen, strTrayBarcode);
											}
											else // 가득찬 경우
											{
												m_arrMap[m_arrIdx[NextIdx+i]].SetInfo(i, ncnt_row, ncnt_col, strModuleType, strWorkOrder, strStartTime, strProgramID_Top, strProgramID_Btm, nProdCount, nTrayCount, ncnt_row*ncnt_col, bCapUse, strModuleGen, strTrayBarcode);
											}

											if (i == nTrayCount-1)
											{
												var tcinfo	= m_arrMap[m_arrIdx[NextIdx+i]].GetInfo();
												tcinfo.LastTrayOfLot	= true;
												m_arrMap[m_arrIdx[NextIdx+i]].SetInfo(tcinfo);
											}
										}
									}

									if (SM.FMT.IsFlow(FlowTray.LDLift01))
									{
										string str = "";
										if (Program._bSimul_ == true)
										{
											str = "SIM ";
										}
										str = str + string.Format($"LOT INFO WRITE - " +
																	$"Key Value  = {SM.FMT[FlowTray.LDLift01].GetInfo().KeyValue}, " +
																	$"WorkOrder = {SM.FMT[FlowTray.LDLift01].GetInfo().WorkOrder}, " +
																	$"Top ProgramID = {SM.FMT[FlowTray.LDLift01].GetInfo().ProgramID_Top}, " +
																	$"Btm ProgramID = {SM.FMT[FlowTray.LDLift01].GetInfo().ProgramID_Btm}, " +
																	$"Count = {SM.FMT[FlowTray.LDLift01].GetInfo().MaxCntProdInLot}, " +
																	$"ModuleType = {SM.FMT[FlowTray.LDLift01].GetInfo().ModuleType}, " +
																	$"Row = {SM.FMT[FlowTray.LDLift01].GetInfo().RowCnt} ");

										UI.Logs.Write(LOGPART.DATA, str);
									}
									
									SM.FMT.Clear(FlowTray.LDWaitBuf);
									break;
							}
						}
						break;

					case FlowTray.LDLift01:
						{
							if (NextIdx != FlowTray.LDConv01)						return false;
							if (m_arrIdx[CurIdx] == (int)FlowTray.Empty)			return false;

							bnext_run	= true;
							nmax		= stLotCnt.Max;
						}
						break;

					case FlowTray.LDConv01:
						{
							if (NextIdx != FlowTray.LDLift01)						return false;

							bnext_run	= true;
							nmax		= stLotCnt.Max;
						}
						break;
					
					case FlowTray.ULDConv01:
						{
							if (NextIdx != FlowTray.ULDLift01)					return false;
							if (m_arrIdx[CurIdx] == (int)FlowTray.Empty)			return false;

							bnext_run	= true;
							nmax		= stLotCnt.Max;
						}
						break;

					default :
						return false;
				}

				if (bnext_run == true)
				{
					for (int i = 0; i < nmax; i++)
					{
						if (m_arrIdx[CurIdx+i] == (int)FlowTray.Empty)			continue;

						WriteIdx(NextIdx+i, m_arrIdx[CurIdx+i]);
						WriteIdx(CurIdx+i, (int)FlowTray.Empty);
						
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

		public	bool	NextInsert(FlowTray CurIdx, FlowTray NextIdx)
		{
			try
			{
			//	if (m_arrIdx[NextIdx] != (int)teFlowTray.Empty)			return false;

				bool	bnext_run	= false;
				int		nmax		= 0;

				switch (CurIdx)
				{
					case FlowTray.Empty:
						bnext_run	= false;
						break;

					case FlowTray.ULDTrans:
						{	
							if (NextIdx != FlowTray.ULDConv01)					return false;
							if (m_arrIdx[CurIdx] == (int)FlowTray.Empty)			return false;

							bnext_run	= true;
							nmax		= stLotCnt.Max;
						}
						break;

					case FlowTray.LDTrans:
						{	
							if (NextIdx != FlowTray.LDConv01)						return false;
							if (m_arrIdx[CurIdx] == (int)FlowTray.Empty)			return false;

							bnext_run	= true;
							nmax		= stLotCnt.Max;
						}
						break;

					default :
						return false;
				}

				if (bnext_run == true)
				{
					int	ncnt		= 0;
					int ncnt_tray	= 0;

					for (int i = nmax-1; i >= 0; i--)
					{
						if (i == nmax-1  &&
							m_arrIdx[NextIdx+i] != (int)FlowTray.Empty)			break;
						if (m_arrIdx[NextIdx+i] == (int)FlowTray.Empty)			continue;	

						WriteIdx(NextIdx+i+1, m_arrIdx[NextIdx+i]);
						WriteIdx(NextIdx+i, (int)FlowTray.Empty);

						//제일 아래에 있는 트레이의 최대 값
						if (ncnt_tray == 0 ) ncnt_tray = m_arrMap[m_arrIdx[NextIdx+i+1]].GetInfo().MaxCntTray;

						ncnt++;
					}

					WriteIdx(NextIdx, m_arrIdx[CurIdx]);
					WriteIdx(CurIdx, (int)FlowTray.Empty);
					ncnt++;

					//ncnt_tray	= m_arrMap[m_arrIdx[NextIdx]].GetInfo().MaxCntTray;
					//if (ncnt_tray < ncnt)			ncnt_tray	= ncnt;

					for (int i = 0; i < ncnt; i++)
					{
						var info = m_arrMap[m_arrIdx[NextIdx+i]].GetInfo();
						//첫 트레이 예외처리
						if (ncnt_tray != 0)		info.MaxCntTray = ncnt_tray; 

						m_arrMap[m_arrIdx[NextIdx+i]].SetInfo(info);
					}

					if (IsFlow(NextIdx+1) == true)
					{
						if (m_arrMap[m_arrIdx[NextIdx]].GetInfo().KeyValue != m_arrMap[m_arrIdx[NextIdx+1]].GetInfo().KeyValue)
						{
							var info = m_arrMap[m_arrIdx[NextIdx]].GetInfo();
								
							info.WorkOrder		= m_arrMap[m_arrIdx[NextIdx+1]].GetInfo().WorkOrder; 
							info.KeyValue	= m_arrMap[m_arrIdx[NextIdx+1]].GetInfo().KeyValue; 

							m_arrMap[m_arrIdx[NextIdx]].SetInfo(info);
						}
					}
				}
				
				GLog.Add(string.Format("Tray Next {0} -> {1},  ID = {2}",CurIdx, NextIdx,  m_arrMap[m_arrIdx[NextIdx]].GetInfo().TrayID));

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	FlowSwap(FlowTray Idx1, FlowTray Idx2)
		{
			try
			{
				if (m_arrIdx[Idx1] == (int)FlowTray.Empty)			return false;
				if (m_arrIdx[Idx2] == (int)FlowTray.Empty)			return false;

				Next(Idx1, FlowTray.Tmp1);
				Next(Idx2, Idx1);
				Next(FlowTray.Tmp1, Idx2);

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsFlow(FlowTray Flow)
		{
			try
			{
				if (m_arrIdx[Flow] < 0)			m_arrIdx[Flow]	= 0;

				return m_arrIdx[Flow] != (int)FlowTray.Empty;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	int		GetExistCountRange(FlowTray StartTray, FlowTray EndTray, out FlowTray LastExistTray)
		{
			LastExistTray	= FlowTray.Empty;

			try
			{
				int ncnt	= 0;
				
				if (StartTray == FlowTray.Empty)			StartTray	= (FlowTray)1;

				for (FlowTray f = StartTray; f <= EndTray; f++)
				{
					if (m_arrIdx[f] != (int)FlowTray.Empty)
					{
						ncnt++;
						LastExistTray	= f;
					}
				}

				return ncnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

		public	int		GetExistCountRange(FlowTray StartTray, FlowTray EndTray)
		{
			try
			{
				FlowTray	last_exist_tray;
				return GetExistCountRange(StartTray, EndTray, out last_exist_tray);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

		public	bool	IsFlowRange(FlowTray StartTray, FlowTray EndTray)
		{
			try
			{
				if (GetExistCountRange(StartTray, EndTray) > 0)			return true;

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	FlowTray	GetLastExistFlow(FlowTray StartTray, FlowTray EndTray)
		{
			FlowTray	last_exist_tray;
			try
			{
				GetExistCountRange(StartTray, EndTray, out last_exist_tray);

				return last_exist_tray;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return FlowTray.Empty;
			}
		}

		public	FlowTray	GetFirstExistFlow(FlowTray StartTray, FlowTray EndTray)
		{
			try
			{
				if (StartTray == FlowTray.Empty)			StartTray	= (FlowTray)1;

				for (FlowTray f = StartTray; f <= EndTray; f++)
				{
					if (m_arrIdx[f] != (int)FlowTray.Empty)			return f;
				}

				return FlowTray.Empty;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return FlowTray.Empty;
			}
		}

		public	FlowTray	GetFirstEmptyFlow(FlowTray StartTray, FlowTray EndTray)
		{
			try
			{
				if (StartTray == FlowTray.Empty)			StartTray	= (FlowTray)1;

				for (FlowTray f = StartTray; f <= EndTray; f++)
				{
					if (m_arrIdx[f] == (int)FlowTray.Empty)			return f;
				}

				return FlowTray.Empty;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return FlowTray.Empty;
			}
		}

		public	void	Clear(FlowTray Idx)
		{
			try
			{
				if		(m_arrIdx[Idx] == (int)FlowTray.Empty)
				{
					m_arrMap[m_arrIdx[Idx]].InitInfo();
					WriteUse(m_arrIdx[Idx], true);
				}
				else if (m_arrIdx[Idx] != (int)FlowTray.Empty)
				{
					GLog.Add(string.Format("Tray Clear {0}", Idx));
					m_arrMap[m_arrIdx[Idx]].InitInfo();
					WriteUse(m_arrIdx[Idx], false);
				}

				WriteIdx(Idx, (int)FlowTray.Empty);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	FlowClearAll()
		{
			try
			{
				GLog.Add("Tray Clear All");

				for (int i = 1; i < m_arrIdx.Count; i++) // 0은 Empty이므로 1부터 시작한다.
				{
					if (i == (int)FlowTray.LDWaitBuf)			continue;

					if (m_arrIdx[i] != (int)FlowTray.Empty)
					{
						m_arrMap[m_arrIdx[i]].InitInfo();
						WriteUse(m_arrIdx[i], false);
					}
					WriteIdx((FlowTray)i, (int)FlowTray.Empty);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	FlowClearRange(FlowTray StartTray, FlowTray EndTray)
		{
			try
			{
				GLog.Add(string.Format("Tray Clear Rnage : {0} ~ {1}", StartTray.ToString(), EndTray.ToString()));

				for (int i = (int)StartTray; i <= (int)EndTray; i++)
				{
					if (m_arrIdx[i] != (int)FlowTray.Empty)
					{
						m_arrMap[m_arrIdx[i]].InitInfo();
						WriteUse(m_arrIdx[i], false);
					}
					WriteIdx((FlowTray)i, (int)FlowTray.Empty);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion ADD Functions END
	}



	//===================================================================================================================================
	// INFO 2, FLOW MNG 2

	public	class	InfoPart
	{
		private	const int	MaxProdCnt	= 4;

		#region Struct & Event
		
		//구조체에 값 추가시 frmFlowMapInfo 의 SetValue에 해당 값을 추가
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public	struct	tsInfo
		{
			public	bool	LastPieceOfLot;
			public	int		MaxCntTray;
			public	int		MaxCntProdInLot;
			public	int		MaxCntProdInTray;
			public	long	TransTime;
			public	long	StartTime; //값이 큰게 더 나중 시간이므로 해당 값이 작을수록 먼저 동작
			
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 30)]
			public  string  WorkOrder; // Lot Id
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 60)]
			public  string  KeyValue;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public  string  ProgramID_Top; // AOI Recipe
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
			public  string  ProgramID_Btm;

			[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = MaxProdCnt)]
			public	stProdInfo[]	ProdInfo;
		}
		private	tsInfo	m_Info	= new tsInfo();

		/// <summary>
		/// 구조체 배열 현재 인덱스
		/// </summary>
		private	int	_iIndex;
		public	int	m_iIndex
		{
			set => _iIndex = value;
			get => _iIndex;
		}

		public	delegate void	UpdateEventHandler(int Index);
		public	event			UpdateEventHandler UpdateEvent;
		protected	void	OnUpdateEvent()
		{
			if (UpdateEvent != null)			UpdateEvent(_iIndex);
		}
		#endregion Struct & Event  END

		#region Default Functions
		public			InfoPart(int _idx)
		{
			_iIndex				= _idx;

			m_Info.LastPieceOfLot		= false;
			m_Info.TransTime			= 0;
			m_Info.StartTime			= 0;
			m_Info.MaxCntTray			= 0;
			m_Info.MaxCntProdInTray		= 0;
			m_Info.MaxCntProdInLot		= 0;
			m_Info.WorkOrder			= "";
			m_Info.KeyValue				= "";
			m_Info.ProgramID_Top		= "";
			m_Info.ProgramID_Btm		= "";
			m_Info.ProdInfo				= new stProdInfo[MaxProdCnt];
		}

		public	tsInfo	GetInfo()
		{
			return m_Info;
		}

		public	void	SetInfo(tsInfo _info)
		{
			m_Info = _info;
			OnUpdateEvent();
		}

		public	void	InitInfo()
		{
			try
			{
				m_Info.LastPieceOfLot	= false;
				m_Info.TransTime		= 0;
				m_Info.StartTime		= 0;
				m_Info.MaxCntTray		= 0;
				m_Info.MaxCntProdInTray	= 0;
				m_Info.MaxCntProdInLot	= 0;
				m_Info.WorkOrder		= "";
				m_Info.KeyValue			= "";
				m_Info.ProgramID_Top	= "";
				m_Info.ProgramID_Btm	= "";

				m_Info.ProdInfo			= new stProdInfo[MaxProdCnt];

				for (int i = 0; i < m_Info.ProdInfo.Length; i++)
				{
					ClearProductData(i);
				}

				OnUpdateEvent();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion Default Functions END


		//------------------------- 자유롭게 함수 추가 -----------------------------------
		#region ADD Functions
		public	int		GetMaxProdCount()
		{
			try
			{
				return MaxProdCnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 4;
			}
		}

		public	int		GetExistCount()
		{
			try
			{
				int nCount = 0;
				for(int i = 0; i < GetMaxProdCount(); i++)
				{	
					if (m_Info.ProdInfo[i].Exist == true)			 nCount++;
				}

				return nCount;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}
		
		public	int		GetExistCount(ProdStatus Status)
		{
			try
			{

				int nCount = 0;
				
				switch (Status)
				{
					case ProdStatus.None:
						for (int i = 0; i < GetMaxProdCount(); i++)
						{
							if (m_Info.ProdInfo[i].Exist == false) nCount++;
						}
						break;
					case ProdStatus.Unknown:
						for (int i = 0; i < GetMaxProdCount(); i++)
						{
							if (m_Info.ProdInfo[i].Exist == true) 
							{
								if (m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.Unknown  ||
									m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.Unknown)
								{
									nCount++;
								}
							}
						}
						break;
					case ProdStatus.NG:
						for (int i = 0; i < GetMaxProdCount(); i++)
						{
							if (m_Info.ProdInfo[i].Exist == true) 
							{
								if ((m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.NG  &&  m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.OK)  ||
									(m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.OK  &&  m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.NG)  ||
									(m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.NG  &&  m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.NG)  )
								{
									nCount++;
								}
								else if (m_Info.ProdInfo[i].ResultMES == (int)MESStatus.NG)
								{
									nCount++;
								}
							}
						}
						break;
					case ProdStatus.OK:
						
						for (int i = 0; i < GetMaxProdCount(); i++)
						{
							if (m_Info.ProdInfo[i].Exist == true) 
							{
								if ((m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.OK  &&  m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.OK) )
								{
									nCount++;
								}
							}
						}
						break;
					case ProdStatus.Max:
						break;
					default:
						break;
				}

				return nCount;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}

		public	bool[]	GetProdExist()
		{
			try
			{
				bool[] bexist = new bool[GetMaxProdCount()];
				for(int i = 0; i < GetMaxProdCount(); i++)
				{	
					bexist[i]	= m_Info.ProdInfo[i].Exist;
				}

				return bexist;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return new bool[4];
			}

		}
		
		public	ProdStatus	GetProdStatus(int nIdx)
		{
			try
			{
				if (m_Info.ProdInfo[nIdx].Exist == false)			return ProdStatus.None;

				if		((ProdStatus)m_Info.ProdInfo[nIdx].ResultTop == ProdStatus.Unknown  ||
						 (ProdStatus)m_Info.ProdInfo[nIdx].ResultBtm == ProdStatus.Unknown  ||
						 (ProdStatus)m_Info.ProdInfo[nIdx].ResultTop == ProdStatus.None  ||
						 (ProdStatus)m_Info.ProdInfo[nIdx].ResultBtm == ProdStatus.None)
				{
					return ProdStatus.Unknown;
				}
				else if ((ProdStatus)m_Info.ProdInfo[nIdx].ResultTop == ProdStatus.NG  ||
						 (ProdStatus)m_Info.ProdInfo[nIdx].ResultBtm == ProdStatus.NG)
				{
					return ProdStatus.NG;
				}
				else
				{
					return ProdStatus.OK;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return ProdStatus.Unknown;
			}
		}

		public	ProdColorResult	GetProdColorResult(int nIdx)
		{
			try
			{
				if (m_Info.ProdInfo[nIdx].Exist == false)									return ProdColorResult.None;
				if (m_Info.ProdInfo[nIdx].PickStatus == (int)ProdPickStatus.Mismatch)		return ProdColorResult.MISMATCH;

				ProdStatus TopResult = (ProdStatus)m_Info.ProdInfo[nIdx].ResultTop;
				ProdStatus BtmResult = (ProdStatus)m_Info.ProdInfo[nIdx].ResultBtm;

				if (TopResult == ProdStatus.Unknown ||
					BtmResult == ProdStatus.Unknown ||
					TopResult == ProdStatus.None ||
					BtmResult == ProdStatus.None)
				{
					return ProdColorResult.Unknown;
				}
				else if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true && (ProdStatus)m_Info.ProdInfo[nIdx].ResultMES == (int)MESStatus.NG)
				{
					return ProdColorResult.USERNG;
				}
				else if (TopResult == ProdStatus.NG || BtmResult == ProdStatus.NG)
				{
					bool bIsTopDone = TopResult == ProdStatus.OK || (TopResult == ProdStatus.NG && m_Info.ProdInfo[nIdx].VerifyTopDone);
					bool bIsBtmDone = BtmResult == ProdStatus.OK || (BtmResult == ProdStatus.NG && m_Info.ProdInfo[nIdx].VerifyBtmDone);

					if (bIsTopDone && bIsBtmDone)
					{
						return ProdColorResult.USERNG;
					}

					return ProdColorResult.NG;
				}
				else
				{
					if (m_Info.ProdInfo[nIdx].VerifyTopDone == true || m_Info.ProdInfo[nIdx].VerifyBtmDone == true)
					{
						return ProdColorResult.USEROK;
					}
					return ProdColorResult.OK;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return ProdColorResult.Unknown;
			}
		}
		
		public	bool	GetProdExist(int nNo)
		{
			try
			{
				return m_Info.ProdInfo[nNo].Exist;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}

		}

		public	bool	SetProd(int nIdx, stProdInfo ProdInfo)
		{
			try
			{
				if (nIdx > m_Info.ProdInfo.Length-1)			return false;

				m_Info.ProdInfo[nIdx]	= ProdInfo;

				OnUpdateEvent();

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	void	SetLastOfLot(bool bState)
		{
			try
			{
				m_Info.LastPieceOfLot	= bState;

				OnUpdateEvent();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public	bool	ClearProd(int nIdx)
		{
			try
			{
				if (ClearProductData(nIdx) == false)			return false;

				OnUpdateEvent();

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	void	ClearProdAll()
		{
			try
			{
				for (int i = 0; i < m_Info.ProdInfo.Length; i++)
				{
					ClearProductData(i);
				}

				OnUpdateEvent();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	int		IsResultExist(ProdStatus inspResult)
		{
			try
			{
				for(int i = 0; i < GetMaxProdCount(); i++)
				{	
					if (m_Info.ProdInfo[i].Exist == true)
					{
						if		(inspResult == ProdStatus.OK)
						{
							if (m_Info.ProdInfo[i].ResultTop == (int)inspResult  &&
								m_Info.ProdInfo[i].ResultBtm == (int)inspResult)
							{
								//mes 사용시엔 mes도 ok여야 ok로 판정
								if (SM.SHM.GetUseSkipData(USESKIP.SGEM) == true  &&
									m_Info.ProdInfo[i].ResultMES == (int)MESStatus.OK)
								{
									return i;
								}
								else
									return i;
							}
						}
						else
						{
								//mes 사용시엔 mes ng도 ng로 판정
							if (m_Info.ProdInfo[i].ResultTop == (int)inspResult  ||
								m_Info.ProdInfo[i].ResultBtm == (int)inspResult  ||
								(SM.SHM.GetUseSkipData(USESKIP.SGEM) == true  &&  m_Info.ProdInfo[i].ResultMES == (int)MESStatus.NG))
							{
								return i;
							}
						}
					}
				}

				return -1;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}
		
		public	bool	IsNoVerifyExist(int nIndex = -1)
		{
			try
			{
				if (nIndex < 0)
				{
					for (int i = 0; i < GetMaxProdCount(); i++)
					{	
						if (m_Info.ProdInfo[i].Exist == true)
						{
							if (m_Info.ProdInfo[i].ResultTop == (int)ProdStatus.NG  &&  m_Info.ProdInfo[i].VerifyTopDone == false)			return true;
							if (m_Info.ProdInfo[i].ResultBtm == (int)ProdStatus.NG  &&  m_Info.ProdInfo[i].VerifyBtmDone == false)			return true;
							if (m_Info.ProdInfo[i].ResultMES == (int)MESStatus.NG  &&  SM.SHM.GetUseSkipData(USESKIP.SGEM) == true)			return true;
						}
					}
				}
				else
				{
					if (m_Info.ProdInfo[nIndex].Exist == true)
					{
						if (m_Info.ProdInfo[nIndex].ResultTop == (int)ProdStatus.NG  &&  m_Info.ProdInfo[nIndex].VerifyTopDone == false)			return true;
						if (m_Info.ProdInfo[nIndex].ResultBtm == (int)ProdStatus.NG  &&  m_Info.ProdInfo[nIndex].VerifyBtmDone == false)			return true;
						if (m_Info.ProdInfo[nIndex].ResultMES == (int)MESStatus.NG  &&  SM.SHM.GetUseSkipData(USESKIP.SGEM) == true)				return true;
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private	bool	ClearProductData(int nIdx)
		{
			try
			{
				if (nIdx > m_Info.ProdInfo.Length-1)			return false;
				
				m_Info.ProdInfo[nIdx].Exist				= false;
				m_Info.ProdInfo[nIdx].TrayRow			= 0;
				m_Info.ProdInfo[nIdx].TrayCol			= 0;
				m_Info.ProdInfo[nIdx].ResultTop			= (int)ProdStatus.Unknown;
				m_Info.ProdInfo[nIdx].ResultBtm			= (int)ProdStatus.Unknown;
				m_Info.ProdInfo[nIdx].ResultMES			= (int)MESStatus.Unknown;
				m_Info.ProdInfo[nIdx].VerifyTopDone		= false;
				m_Info.ProdInfo[nIdx].VerifyBtmDone		= false;
				m_Info.ProdInfo[nIdx].TrayID			= "";
				m_Info.ProdInfo[nIdx].ProdID			= "";
				m_Info.ProdInfo[nIdx].KeyValue			= "";
				m_Info.ProdInfo[nIdx].InspTimeTop		= "";
				m_Info.ProdInfo[nIdx].InspTimeBtm		= "";
				m_Info.ProdInfo[nIdx].SerialNumber		= "";
				m_Info.ProdInfo[nIdx].TrayBarcode		= "";
				m_Info.ProdInfo[nIdx].PickStatus		= (int)ProdPickStatus.Normal;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public void	SetPickupStatus( ProdPickStatus Status, int nIdx = -1)
		{
			if (nIdx == -1)
			{
				for (int i = 0; i < m_Info.ProdInfo.Count(); i++)
				{	
					m_Info.ProdInfo[i].PickStatus = (int)Status;
				}
			}
			else
			{
				m_Info.ProdInfo[nIdx].PickStatus = (int)Status;
			}
		}
		#endregion ADD Functions END
	}

	public	class	FlowMngPart : MemMappedFile
	{
		public	enum	teDir
		{
			Top,
			Btm
		}

		#region Variables

		private	bool[]		m_arrUse;
		private	InfoPart[]	m_arrMap;
		private	EnumArray<FlowPart, int>	m_arrIdx;

		#endregion Variables END

		#region Default Functions
		public			FlowMngPart()
		{
			#region 사이즈 설정, 버퍼 초기화
			SIZE_ENUM	= (int)FlowPart.Max;
			SIZE_STRUCT	= Marshal.SizeOf(new InfoPart.tsInfo());

			m_arrUse	= new bool[SIZE_ENUM + OFFSET];
			m_arrMap	= new InfoPart[SIZE_ENUM + OFFSET];
			m_arrIdx	= new EnumArray<FlowPart, int>(SIZE_ENUM);

			for (int i = 0; i < SIZE_ENUM + OFFSET; i++)
			{
				m_arrUse[i]	= false;
				m_arrMap[i]	= new InfoPart(i);
				m_arrMap[i].UpdateEvent += Update;
			}

			for (int i = 0; i < m_arrIdx.Count; i++)
				m_arrIdx[i]	= 0;
			#endregion

			MemCreate();
			m_arrUse[0] = true; // 0번째는 Empty로 디폴트가 True
		}

		public	InfoPart	this[FlowPart eKey]
		{
			get
			{
				return m_arrMap[m_arrIdx[eKey]];
			}
			set
			{
				m_arrMap[m_arrIdx[eKey]] = value;
				WriteInfo(m_arrMap[m_arrIdx[eKey]]);
			}
		}

		private	bool	MemCreate()
		{
			try
			{
				bool ret = Create("FlowMapPart");

				for (int i = 0; i < m_arrUse.Length; i++)
				{
					m_arrUse[i] = ReadUse(i);
					m_arrMap[i].SetInfo(ReadInfo(i));
				}

				for (int i = 0; i < m_arrIdx.Count; i++)
					m_arrIdx[i] = ReadIdx(i);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}

			return true;
		}

		/// <summary>
		/// Event Call 함수
		/// </summary>
		/// <param name="_Idx"></param>
		private	void	Update(int _Idx)
		{
			WriteInfo(m_arrMap[_Idx]);
		}

		private	void	WriteUse(int nIdx, bool bVal)
		{
			try
			{
				long	lOffset	= nIdx * sizeof(Boolean);

				m_arrUse[nIdx] = bVal;
				Write(lOffset, bVal);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	WriteInfo(InfoPart _info)
		{
			byte[]	byBuff	= StructParsing.MethodObjectToByteSerializer(_info.GetInfo());
			Write(SIZE_USE_MAP + (_info.m_iIndex * byBuff.Length), byBuff);
		}

		private void	WriteIdx(FlowPart flow, int nVal)
		{
			try
			{
				long	lSize	= SIZE_USE_MAP + SIZE_MAP + ((int)flow * sizeof(int));

				m_arrIdx[flow]	= nVal;
				Write(lSize, nVal);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	InfoPart.tsInfo	ReadInfo(int idx)
		{
			byte[]	byBuff	= Read(SIZE_USE_MAP + (idx * SIZE_STRUCT), SIZE_STRUCT);
			return (InfoPart.tsInfo)(StructParsing.MethodByteToObjectDeserialize(byBuff, typeof(InfoPart.tsInfo)));
		}

		private	bool	ReadUse(int idx)
		{
			long	lSize	= idx * sizeof(Boolean);
			return Read(lSize);
		}

		private	int		ReadIdx(int idx)
		{
			long	lSize	= SIZE_USE_MAP + SIZE_MAP + (idx * sizeof(int));
			return IRead(lSize);
		}

		private	int		GetPushIdx()
		{
			try
			{
				int	nIdx	= Array.IndexOf(m_arrUse, false);
				//if (nIdx == -1) MessageBox.Show("FlowMng : nPush Index -1 Check Please");
				WriteUse(nIdx, true);

				return nIdx;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

		public	bool	Next(FlowPart CurIdx, FlowPart NextIdx)
		{
			try
			{
				if (m_arrIdx[NextIdx] != (int)FlowPart.Empty)			return false;
				
				switch (CurIdx)
				{
					case FlowPart.Empty:
						{
							WriteIdx(NextIdx, GetPushIdx());
							m_arrMap[m_arrIdx[NextIdx]].InitInfo();

							var temp	= m_arrMap[m_arrIdx[NextIdx]].GetInfo();
							temp.StartTime	= DateTime.Now.Ticks;
							m_arrMap[m_arrIdx[NextIdx]].SetInfo(temp);
							
							GLog.Add(string.Format("Prod Create {0}", NextIdx));
						}
						break;

					default:
						{
							if (m_arrIdx[CurIdx] == (int)FlowPart.Empty)			return false;

							WriteIdx(NextIdx, m_arrIdx[CurIdx]);
							WriteIdx(CurIdx, (int)FlowPart.Empty);
							
							string ID = "";
							for (int i = 0; i < m_arrMap[m_arrIdx[NextIdx]].GetMaxProdCount(); i++)
								ID += m_arrMap[m_arrIdx[NextIdx]].GetInfo().ProdInfo[i].ProdID + " ";
							GLog.Add(string.Format("Prod Flow {0} -> {1} . ID = {2}", CurIdx, NextIdx, ID));
						}
						break;
				}
				//자재가 있을때만 CurLotID 변경
				if (NextIdx == FlowPart.ULDPnP)
				{
					for (int i = 0; i < m_arrMap[m_arrIdx[NextIdx]].GetMaxProdCount(); i++)
					{
						if (m_arrMap[m_arrIdx[NextIdx]].GetInfo().ProdInfo[i].Exist == false)			continue;
			
						string strCurKeyValue = m_arrMap[m_arrIdx[NextIdx]].GetInfo().KeyValue;

						if (SDm.Read(eSDM.CurKeyValue).Equals(strCurKeyValue) == false) //언로드 하고 있는 랏이 변경될때
						{
							//SDm.Write(eSDM.CurKeyValue, strCurKeyValue);
							SDm.Write(eSDM.CurTempKeyValue, strCurKeyValue);
			
							if (SM.SHM.IsBitData(BIT.NewDay) == true  &&
								DateTime.Now.Hour >= NDm.Read(eNDM.ResetHourTotalYeild)) //Daily Lot 초기화
							{
								int nOKCount = NDm.Read(eNDM.CurOKProdCount);
								int nNGCount = NDm.Read(eNDM.CurNGProdCount);
								int nUserOKCount = NDm.Read(eNDM.CurUserOKProdCount);
			
								string str = string.Format("DAILY YIELD CLEAR! TOTAL = {0}, OK = {1} , NG = {2} , USER OK = {3}     ", nOKCount + nNGCount + nUserOKCount, nOKCount, nNGCount, nUserOKCount);
								NDm.Write(eNDM.CurOKProdCount, 0);
								NDm.Write(eNDM.CurNGProdCount, 0);
								NDm.Write(eNDM.CurUserOKProdCount, 0);
								SM.SHM.SetBitData(BIT.NewDay, false);
								UI.Logs.Write(LOGPART.DATA, str);
							}
						}

						break;
					}
				}
				
				var info	= m_arrMap[m_arrIdx[NextIdx]].GetInfo();
				info.TransTime	= DateTime.Now.Ticks;
				m_arrMap[m_arrIdx[NextIdx]].SetInfo(info);
				
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		#endregion Default Functions END



		//------------------------- 자유롭게 함수 추가 -----------------------------------
		#region ADD Functions
		public	bool	IsFlow(FlowPart Flow)
		{
			try
			{
				return m_arrIdx[Flow] != (int)FlowPart.Empty;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	IsFlowRange(FlowPart StartPart, FlowPart EndPart)
		{
			try
			{
				if (StartPart == FlowPart.Empty)			StartPart	= (FlowPart)1;

				for (FlowPart f = StartPart; f <= EndPart; f++)
				{
					if (m_arrIdx[f] != (int)FlowPart.Empty)			return true;
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}


		public	void	Clear(FlowPart Idx)
		{
			try
			{
				if		(m_arrIdx[Idx] == (int)FlowPart.Empty)
				{
					m_arrMap[m_arrIdx[Idx]].InitInfo();
					WriteUse(m_arrIdx[Idx], true);
				}
				else if (m_arrIdx[Idx] != (int)FlowPart.Empty)
				{
					GLog.Add(string.Format("Tray Clear {0}", Idx));
					m_arrMap[m_arrIdx[Idx]].InitInfo();
					WriteUse(m_arrIdx[Idx], false);
				}
				WriteIdx(Idx, (int)FlowTray.Empty);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	FlowClearAll()
		{
			try
			{
				GLog.Add("Part Clear All");

				for (int i = 1; i < m_arrIdx.Count; i++) // 0은 Empty이므로 1부터 시작한다.
				{
					if (m_arrIdx[i] != (int)FlowTray.Empty)
					{
						m_arrMap[m_arrIdx[i]].InitInfo();
						WriteUse(m_arrIdx[i], false);
					}
					WriteIdx((FlowPart)i, (int)FlowTray.Empty);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	SetInspTime(teDir Dir, string strProdID, string strInspStartTime)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(strProdID) == true)			return false;

				for (int i = 1; i < m_arrMap.Length; i++)
				{
					for (int j = 0; j < m_arrMap[i].GetMaxProdCount(); j++)
					{
						if (m_arrMap[i].GetInfo().ProdInfo[j].ProdID == strProdID)
						{
							if (Dir == teDir.Top)
							{
								m_arrMap[i].GetInfo().ProdInfo[j].InspTimeTop	= strInspStartTime;
							}
							else
							{
								m_arrMap[i].GetInfo().ProdInfo[j].InspTimeBtm	= strInspStartTime;
							}
							return true;
						}
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	SetInspResult(teDir Dir, string strProdID, ProdStatus Result, string strBarcode)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(strProdID) == true)			return false;

				for (int i = 1; i < m_arrMap.Length; i++)
				{
					for (int j = 0; j < m_arrMap[i].GetMaxProdCount(); j++)
					{
						if (m_arrMap[i].GetInfo().ProdInfo[j].ProdID == strProdID)
						{
							if (Dir == teDir.Top)			m_arrMap[i].GetInfo().ProdInfo[j].ResultTop	= (int)Result;
							else							m_arrMap[i].GetInfo().ProdInfo[j].ResultBtm	= (int)Result;

							//if (Dir == teDir.Top)			m_arrMap[i].GetInfo().ProdInfo[j].SerialNumber	= strBtmBarcode;
							
							//한쪽은 에러, 한쪽은 정상인 경우 예외처리
							//이미 피두셜 에러인 경우, 덮어 씌우지 않음
							if (m_arrMap[i].GetInfo().ProdInfo[j].SerialNumber.ToLower().Contains("fiduerror") == false)
							{
								if (m_arrMap[i].GetInfo().ProdInfo[j].SerialNumber.ToLower().Contains("error") == true)
								{
									//에러였던 경우 피두셜 에러오면 덮어씌움
									if (strBarcode.ToLower().Contains("fiduerror") == true)
									{
										m_arrMap[i].GetInfo().ProdInfo[j].SerialNumber	= strBarcode;
									}
								}
								else //피두셜에러, 에러가 아닌경우만 데이터 넣음, 바텀은 빈칸이 오니깐 예외처리
								{
									if (string.IsNullOrWhiteSpace(strBarcode) == false  &&  m_arrMap[i].GetInfo().ProdInfo[j].SerialNumber	!= strBarcode )
									{
										m_arrMap[i].GetInfo().ProdInfo[j].SerialNumber	= strBarcode;
									}
								}
							}

							return true;
						}
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	bool	SetMesResult(string strSerialNumber, int nResult)
		{
			
			try
			{
				if (string.IsNullOrWhiteSpace(strSerialNumber) == true)		return false;
				//if (nResult != 0  && nResult != 1)							return false;

				for (int i = 1; i < m_arrMap.Length; i++)
				{
					for (int j = 0; j < m_arrMap[i].GetMaxProdCount(); j++)
					{
						if (m_arrMap[i].GetInfo().ProdInfo[j].SerialNumber == strSerialNumber)
						{
							m_arrMap[i].GetInfo().ProdInfo[j].ResultMES	= nResult;
							if (nResult != (int)MESStatus.OK)
							{
								m_arrMap[i].GetInfo().ProdInfo[j].VerifyTopDone = true;
								m_arrMap[i].GetInfo().ProdInfo[j].VerifyBtmDone = true;
							}
							return true;
						}
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		#endregion ADD Functions END
	}

	

	//===================================================================================================================================
	// FLOW MNG 3
	
	public	class	FlowMngRejectTray : MemMappedFile
	{
		#region Variables

		private	bool[]		m_arrUse;
		private	InfoTray[]	m_arrMap;
		private	EnumArray<FlowRejectTray, int>	m_arrIdx;

		#endregion Variables END

		#region Default Functions
		public				FlowMngRejectTray()
		{
			#region 사이즈 설정, 버퍼 초기화
			SIZE_ENUM	= (int)FlowRejectTray.Max;
			SIZE_STRUCT	= Marshal.SizeOf(new InfoTray.tsInfo());

			m_arrUse	= new bool[SIZE_ENUM + OFFSET];
			m_arrMap	= new InfoTray[SIZE_ENUM + OFFSET];
			m_arrIdx	= new EnumArray<FlowRejectTray, int>(SIZE_ENUM);

			for (int i = 0; i < SIZE_ENUM + OFFSET; i++)
			{
				m_arrUse[i]	= false;
				m_arrMap[i]	= new InfoTray(i);
				m_arrMap[i].UpdateEvent += Update;
			}

			for (int i = 0; i < m_arrIdx.Count; i++)
			{
				m_arrIdx[i] = 0;
			}
			#endregion

			MemCreate();

			m_arrUse[0] = true; // 0번째는 Empty로 디폴트가 True
		}

		public	InfoTray	this[FlowRejectTray eKey]
		{
			get
			{
				return m_arrMap[m_arrIdx[eKey]];
			}
			set
			{
				m_arrMap[m_arrIdx[eKey]] = value;
				WriteInfo(m_arrMap[m_arrIdx[eKey]]);
			}
		}

		private	bool		MemCreate()
		{
			try
			{
				bool	ret	= Create("FlowMapRejectTray");

				for (int i = 0; i < m_arrUse.Length; i++)
				{
					m_arrUse[i]	= ReadUse(i);
					m_arrMap[i].SetInfo(ReadInfo(i));
				}

				for (int i = 0; i < m_arrIdx.Count; i++)
					m_arrIdx[i]	= ReadIdx(i);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}

			return true;
		}



		/// <summary>
		/// Event Call 함수
		/// </summary>
		/// <param name="_Idx"></param>
		private	void	Update(int _Idx)
		{
			WriteInfo(m_arrMap[_Idx]);
		}

		private	void	WriteUse(int nIdx, bool bVal)
		{
			try
			{
				long	lOffset	= nIdx * sizeof(Boolean);

				m_arrUse[nIdx]	= bVal;
				Write(lOffset, bVal);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	WriteInfo(InfoTray _info)
		{
			byte[]	byBuff	= StructParsing.MethodObjectToByteSerializer(_info.GetInfo());
			Write(SIZE_USE_MAP + (_info.m_iIndex * byBuff.Length), byBuff);
		}

		private	void	WriteIdx(FlowRejectTray flow, int nVal)
		{
			try
			{
				long	lSize	= SIZE_USE_MAP + SIZE_MAP + ((int)flow * sizeof(int));

				m_arrIdx[flow]	= nVal;
				Write(lSize, nVal);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	InfoTray.tsInfo ReadInfo(int idx)
		{
			byte[]	byBuff	= Read(SIZE_USE_MAP + (idx * SIZE_STRUCT), SIZE_STRUCT);
			return (InfoTray.tsInfo)(StructParsing.MethodByteToObjectDeserialize(byBuff, typeof(InfoTray.tsInfo)));
		}

		private	bool	ReadUse(int idx)
		{
			long	lSize	= idx * sizeof(Boolean);
			return Read(lSize);
		}

		private	int		ReadIdx(int idx)
		{
			long	lSize	= SIZE_USE_MAP + SIZE_MAP + (idx * sizeof(int));
			return IRead(lSize);
		}

		private	int		GetPushIdx()
		{
			try
			{
				int	nIdx	= Array.IndexOf(m_arrUse, false);
				//if (nIdx == -1) MessageBox.Show("FlowMng : nPush Index -1 Check Please");
				WriteUse(nIdx, true);

				return nIdx;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

		public	bool	Next(FlowRejectTray CurIdx, FlowRejectTray NextIdx)
		{
			try
			{
				if (m_arrIdx[NextIdx] != (int)FlowTray.Empty)			return false;
				
				switch (CurIdx)
				{
					case FlowRejectTray.Empty:
						{
							WriteIdx(NextIdx, GetPushIdx());
							m_arrMap[m_arrIdx[NextIdx]].InitInfo();
						}
						break;
					default:
						{
							 // 없으면 안된다.
							if (m_arrIdx[CurIdx] == (int)FlowTray.Empty)			return false;

							WriteIdx(NextIdx, m_arrIdx[CurIdx]);
							WriteIdx(CurIdx, (int)FlowTray.Empty);
							
							GLog.Add(string.Format("Tray Next {0} -> {1},  ID = {2}",CurIdx, NextIdx,  m_arrMap[m_arrIdx[NextIdx]].GetInfo().TrayID));
						}
						break;
				}

				var info	= m_arrMap[m_arrIdx[NextIdx]].GetInfo();
				info.TransTime	= DateTime.Now.Ticks;
				m_arrMap[m_arrIdx[NextIdx]].SetInfo(info);
				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		#endregion Default Functions END



		//------------------------- 자유롭게 함수 추가 -----------------------------------
		#region ADD Functions
		public	bool	IsFlow(FlowRejectTray Flow)
		{
			try
			{
				if (m_arrIdx[Flow] < 0)			m_arrIdx[Flow]	= 0;

				return m_arrIdx[Flow] != (int)FlowRejectTray.Empty;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public	int		GetExistCountRange(FlowRejectTray StartTray, FlowRejectTray EndTray, out FlowRejectTray LastExistTray)
		{
			LastExistTray	= FlowRejectTray.Empty;

			try
			{
				int ncnt	= 0;
				
				if (StartTray == FlowRejectTray.Empty)			StartTray	= (FlowRejectTray)1;

				for (FlowRejectTray f = StartTray; f <= EndTray; f++)
				{
					if (m_arrIdx[f] != (int)FlowRejectTray.Empty)
					{
						ncnt++;
						LastExistTray	= f;
					}
				}

				return ncnt;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

		public	int		GetExistCountRange(FlowRejectTray StartTray, FlowRejectTray EndTray)
		{
			try
			{
				FlowRejectTray	last_exist_tray;
				return GetExistCountRange(StartTray, EndTray, out last_exist_tray);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return 0;
			}
		}

		public	bool	IsFlowRange(FlowRejectTray StartTray, FlowRejectTray EndTray)
		{
			try
			{
				if (GetExistCountRange(StartTray, EndTray) > 0)			return true;

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		public bool		GetMESNGExistID(string strProdID)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(strProdID) == true)			return false;

				for (int i = 1; i < m_arrMap.Length; i++)
				{
					for (int j = 0; j < m_arrMap[i].GetInfo().MaxCntProdInTray; j++)
					{
						if (m_arrMap[i].GetInfo().ProdInfo[j].Exist == false)	continue;
						
						if (m_arrMap[i].GetInfo().ProdInfo[j].ProdID == strProdID  &&
							(m_arrMap[i].GetInfo().ProdInfo[j].ResultMES == (int)MESStatus.NG  || 
							 m_arrMap[i].GetInfo().ProdInfo[j].SerialNumber.ToLower().Contains("error") == true) )

						{
							return true;
						}
					}
				}

				return false;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}

		}

		public	FlowRejectTray	GetLastExistFlow(FlowRejectTray StartTray, FlowRejectTray EndTray)
		{
			FlowRejectTray	last_exist_tray;
			try
			{
				GetExistCountRange(StartTray, EndTray, out last_exist_tray);

				return last_exist_tray;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return FlowRejectTray.Empty;
			}
		}

		public	FlowRejectTray	GetFirstExistFlow(FlowRejectTray StartTray, FlowRejectTray EndTray)
		{
			try
			{
				if (StartTray == FlowRejectTray.Empty)			StartTray	= (FlowRejectTray)1;

				for (FlowRejectTray f = StartTray; f <= EndTray; f++)
				{
					if (m_arrIdx[f] != (int)FlowRejectTray.Empty)			return f;
				}

				return FlowRejectTray.Empty;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return FlowRejectTray.Empty;
			}
		}

		public	FlowRejectTray	GetFirstEmptyFlow(FlowRejectTray StartTray, FlowRejectTray EndTray)
		{
			try
			{
				if (StartTray == FlowRejectTray.Empty)			StartTray	= (FlowRejectTray)1;

				for (FlowRejectTray f = StartTray; f <= EndTray; f++)
				{
					if (m_arrIdx[f] == (int)FlowRejectTray.Empty)			return f;
				}

				return FlowRejectTray.Empty;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return FlowRejectTray.Empty;
			}
		}

		public	void	Clear(FlowRejectTray Idx)
		{
			try
			{
				if		(m_arrIdx[Idx] == (int)FlowRejectTray.Empty)
				{
					m_arrMap[m_arrIdx[Idx]].InitInfo();
					WriteUse(m_arrIdx[Idx], true);
				}
				else if (m_arrIdx[Idx] != (int)FlowRejectTray.Empty)
				{
					GLog.Add(string.Format("Tray Clear {0}", Idx));
					m_arrMap[m_arrIdx[Idx]].InitInfo();
					WriteUse(m_arrIdx[Idx], false);
				}

				WriteIdx(Idx, (int)FlowRejectTray.Empty);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	FlowClearAll()
		{
			try
			{
				GLog.Add("Tray Clear All");

				for (int i = 1; i < m_arrIdx.Count; i++) // 0은 Empty이므로 1부터 시작한다.
				{
					if (m_arrIdx[i] != (int)FlowRejectTray.Empty)
					{
						m_arrMap[m_arrIdx[i]].InitInfo();
						WriteUse(m_arrIdx[i], false);
					}
					WriteIdx((FlowRejectTray)i, (int)FlowRejectTray.Empty);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	FlowClearRange(FlowRejectTray StartTray, FlowRejectTray EndTray)
		{
			try
			{
				GLog.Add(string.Format("Reject Tray Clear Rnage : {0} ~ {1}", StartTray.ToString(), EndTray.ToString()));

				for (int i = (int)StartTray; i <= (int)EndTray; i++)
				{
					if (m_arrIdx[i] != (int)FlowRejectTray.Empty)
					{
						m_arrMap[m_arrIdx[i]].InitInfo();
						WriteUse(m_arrIdx[i], false);
					}
					WriteIdx((FlowRejectTray)i, (int)FlowRejectTray.Empty);
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion ADD Functions END
	}

}

