using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
	/// <summary>
	/// Flow List 열거형(첫번쨰)
	/// </summary>
	public enum FlowTray
	{
		Empty,
		LDLift01,
		LDLift02,
		LDLift03,
		LDLift04,
		LDLift05,
		LDLift06,
		LDLift07,
		LDLift08,
		LDLift09,
		LDLift10,
		LDLift11,
		LDLift12,
		LDLift13,
		LDLift14,
		LDLift15,
		LDLift16,
		LDLift17,
		LDLift18,
		LDLift19,
		LDLift20,
		LDLift21,
		LDLift22,
		LDLift23,
		LDLift24,
		LDLift25,
		LDLift26,
		LDLift27,
		LDLift28,
		LDLift29,
		LDLift30,
		LDLift31,
		LDConv01,
		LDConv02,
		LDConv03,
		LDConv04,
		LDConv05,
		LDConv06,
		LDConv07,
		LDConv08,
		LDConv09,
		LDConv10,
		LDConv11,
		LDConv12,
		LDConv13,
		LDConv14,
		LDConv15,
		LDConv16,
		LDConv17,
		LDConv18,
		LDConv19,
		LDConv20,
		LDConv21,
		LDConv22,
		LDConv23,
		LDConv24,
		LDConv25,
		LDConv26,
		LDConv27,
		LDConv28,
		LDConv29,
		LDConv30,
		LDConv31,

		LDTrans,
		LDTable1,
		LDTable2,
		TrayClean,
		ULDTrans,
		ULDTable1,
		ULDTable2,

		ULDConv01,
		ULDConv02,
		ULDConv03,
		ULDConv04,
		ULDConv05,
		ULDConv06,
		ULDConv07,
		ULDConv08,
		ULDConv09,
		ULDConv10,
		ULDConv11,
		ULDConv12,
		ULDConv13,
		ULDConv14,
		ULDConv15,
		ULDConv16,
		ULDConv17,
		ULDConv18,
		ULDConv19,
		ULDConv20,
		ULDConv21,
		ULDConv22,
		ULDConv23,
		ULDConv24,
		ULDConv25,
		ULDConv26,
		ULDConv27,
		ULDConv28,
		ULDConv29,
		ULDConv30,
		ULDConv31,
		ULDLift01,
		ULDLift02,
		ULDLift03,
		ULDLift04,
		ULDLift05,
		ULDLift06,
		ULDLift07,
		ULDLift08,
		ULDLift09,
		ULDLift10,
		ULDLift11,
		ULDLift12,
		ULDLift13,
		ULDLift14,
		ULDLift15,
		ULDLift16,
		ULDLift17,
		ULDLift18,
		ULDLift19,
		ULDLift20,
		ULDLift21,
		ULDLift22,
		ULDLift23,
		ULDLift24,
		ULDLift25,
		ULDLift26,
		ULDLift27,
		ULDLift28,
		ULDLift29,
		ULDLift30,
		ULDLift31,

		LDWaitBuf,
		Reject,
		Tmp1,

		/// <summary>
		/// Enum Total Count
		/// </summary>
		Max,
	}
	
	//Pvi 보고용
	public enum FlowRejectTray
	{
		Empty,
		RejectBuf1,
		RejectBuf2,
		RejectBuf3,
		RejectBuf4,
		RejectBuf5,
		RejectBuf6,
		RejectBuf7,
		RejectBuf8,
		RejectBuf9,
		RejectBuf10,
		RejectBuf11,
		RejectBuf12,
		RejectBuf13,
		RejectBuf14,
		RejectBuf15,
		RejectBuf16,
		RejectBuf17,
		RejectBuf18,
		RejectBuf19,
		RejectBuf20,
		RejectBuf21,
		RejectBuf22,
		RejectBuf23,
		RejectBuf24,
		RejectBuf25,
		RejectBuf26,
		RejectBuf27,
		RejectBuf28,
		RejectBuf29,
		RejectBuf30,
		RejectBuf31,
		Tmp1,

		/// <summary>
		/// Enum Total Count
		/// </summary>
		Max,
	}


	/// <summary>
	/// Flow List 열거형(두번쨰)
	/// </summary>
	public enum FlowPart
	{
		Empty = 0,
		LDPnP,
		LDBuffer1,
		LDBuffer2,
		LDInspPnP,
		Insp1,
		Insp2,
		ULDInspPnP,
		ULDBuffer,
		ULDPnP,
		Max,
	}

	
	public	enum	ProdStatus
	{
		None = 0,
		Unknown = 1,
		NG = 2,
		OK = 3,

		Max,
	}

	public	enum	MESStatus
	{
		NG = 0,
		OK = 1,
		Unknown = 2,

		Max,
	}

	public enum ProdColorResult
	{
		None,
		Unknown,
		NG,
		OK,
		USERNG,
		USEROK,
		MISMATCH
	}

	public	enum	ProcStatus
	{
		Unknown,
		CleanDone,
		TrayEmptyDone,
		TrayFillDone,
		SortRun,
		SortDone,

		Max,
	}
	
	public	struct	stLotCnt
	{
		public	const int	HMax	= 31;

		public	static int Max
		{
			get
			{
				return HMax;
			}
		}
	}

}
