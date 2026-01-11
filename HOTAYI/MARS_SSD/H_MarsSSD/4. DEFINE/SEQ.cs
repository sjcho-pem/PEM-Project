using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace H_MarsSSD
{
    public enum Seq
    {
		TrayLoader,
		TrayClean,
		LoadTransfer,
		LoadTrayTable1,
		LoadTrayTable2,
		LoadPnP,
		LoadBuffer1,
		LoadBuffer2,
		LoadInspPnP,
		InspPlate1,
		InspPlate2,
		UnloadInspPnP,
		UnloadBuffer,
		RejectBuffer,
		UnloadPnP,
		UnloadTrayTable1,
		UnloadTrayTable2,
		UnloadTransfer,
		Unloader,

		Max,
    }

	public	enum	SeqID
	{
		
		[DefaultValueAttribute("LOADER/HOME")]
		LOADER_HOME,
		[DefaultValueAttribute("LOADER/LOAD")]
		LOADER_LOAD,
		[DefaultValueAttribute("LOADER/UNLOAD")]
		LOADER_UNLOAD,
		[DefaultValueAttribute("LOADER/DOWN")]
		LOADER_DOWN,
		[DefaultValueAttribute("LOADER/UP")]
		LOADER_UP,
		
        [DefaultValueAttribute("TRAYCLEAN/HOME")]
        TRAYCLEAN_HOME,
        [DefaultValueAttribute("TRAYCLEAN/CLEAN")]
        TRAYCLEAN_CLEAN,
        [DefaultValueAttribute("TRAYCLEAN/GRIP")]
        TRAYCLEAN_GRIP,
        [DefaultValueAttribute("TRAYCLEAN/RELEASE")]
        TRAYCLEAN_RELEASE,

        [DefaultValueAttribute("LDTRANS/HOME")]
		LDTRANS_HOME,
		[DefaultValueAttribute("LDTRANS/PICK")]
		LDTRANS_PICK,
		[DefaultValueAttribute("LDTRANS/PICK CONV")]
		LDTRANS_PICKCONV,
		[DefaultValueAttribute("LDTRANS/PLACE")]
		LDTRANS_PLACE,
		[DefaultValueAttribute("LDTRANS/PLACE CONV")]
		LDTRANS_PLACECONV,
		[DefaultValueAttribute("LDTRANS/PNP")]
		LDTRANS_PNP,

		[DefaultValueAttribute("LOADTRAY1/HOME")]
		LOADTABLE1_HOME,
		[DefaultValueAttribute("LOADTRAY2/HOME")]
		LOADTABLE2_HOME,
		[DefaultValueAttribute("LOADTRAY1/UP")]
		LOADTABLE1_UP,
		[DefaultValueAttribute("LOADTRAY2/UP")]
		LOADTABLE2_UP,
		[DefaultValueAttribute("LOADTRAY1/DOWN")]
		LOADTABLE1_DOWN,
		[DefaultValueAttribute("LOADTRAY2/DOWN")]
		LOADTABLE2_DOWN,
		[DefaultValueAttribute("LOADTRAY1/GRIP")]
		LOADTABLE1_GRIP,
		[DefaultValueAttribute("LOADTRAY2/GRIP")]
		LOADTABLE2_GRIP,
		[DefaultValueAttribute("LOADTRAY1/RELEASE")]
		LOADTABLE1_RELEASE,
		[DefaultValueAttribute("LOADTRAY2/RELEASE")]
		LOADTABLE2_RELEASE,
		[DefaultValueAttribute("LOADTRAY2/MOVEX")]
		LOADTABLE2_MOVEX,

		[DefaultValueAttribute("LOADPNP/HOME")]
		LOADPNP_HOME,
		[DefaultValueAttribute("LOADPNP/PICK TRAY")]
		LOADPNP_PICKTRAY,
		[DefaultValueAttribute("LOADPNP/PLACE BUFFER")]
		LOADPNP_PLACEBUFFER,
		[DefaultValueAttribute("LOADPNP/PNP")]
		LOADPNP_PNP,
		[DefaultValueAttribute("LOADPNP/RETRY PICK")]
		LOADPNP_RETRY_PICK,
		[DefaultValueAttribute("LOADPNP/EMPTY CHECK")]
		LOADPNP_EMPTYCHECK,

		[DefaultValueAttribute("LDBUFFER1/HOME")]
		LDBUFFER1_HOME,
		[DefaultValueAttribute("LDBUFFER2/HOME")]
		LDBUFFER2_HOME,
		[DefaultValueAttribute("LDBUFFER1/LOADWAIT")]
		LDBUFFER1_LOADWAIT,
		[DefaultValueAttribute("LDBUFFER2/LOADWAIT")]
		LDBUFFER2_LOADWAIT,
		[DefaultValueAttribute("LDBUFFER1/CLEAN")]
		LDBUFFER1_CLEAN,
		[DefaultValueAttribute("LDBUFFER2/CLEAN")]
		LDBUFFER2_CLEAN,
		[DefaultValueAttribute("LDBUFFER1/UNLOADWAIT")]
		LDBUFFER1_UNLOADWAIT,
		[DefaultValueAttribute("LDBUFFER2/UNLOADWAIT")]
		LDBUFFER2_UNLOADWAIT,

		[DefaultValueAttribute("LDINSPPNP/HOME")]
		LDINSPPNP_HOME,
		[DefaultValueAttribute("LDINSPPNP/PICK")]
		LDINSPPNP_PICK,
		[DefaultValueAttribute("LDINSPPNP/PLACE")]
		LDINSPPNP_PLACE,
		[DefaultValueAttribute("LDINSPPNP/PNP")]
		LDINSPPNP_PNP,
		[DefaultValueAttribute("LDINSPPNP/READY POS")]
		LDINSPPNP_READYPOS,

		[DefaultValueAttribute("INSPPLATE1/HOME")]
		INSPPLATE1_HOME,
		[DefaultValueAttribute("INSPPLATE2/HOME")]
		INSPPLATE2_HOME,
		[DefaultValueAttribute("INSPPLATE1/LOADWAIT")]
		INSPPLATE1_LOADWAIT,
		[DefaultValueAttribute("INSPPLATE2/LOADWAIT")]
		INSPPLATE2_LOADWAIT,
		[DefaultValueAttribute("INSPPLATE1/UNLOADWAIT")]
		INSPPLATE1_UNLOADWAIT,
		[DefaultValueAttribute("INSPPLATE2/UNLOADWAIT")]
		INSPPLATE2_UNLOADWAIT,
		[DefaultValueAttribute("INSPPLATE1/INSPWAIT")]
		INSPPLATE1_INSPWAIT,
		[DefaultValueAttribute("INSPPLATE2/INSPWAIT")]
		INSPPLATE2_INSPWAIT,

		[DefaultValueAttribute("ULDINSPPNP/HOME")]
		ULDINSPPNP_HOME,
		[DefaultValueAttribute("ULDINSPPNP/PICK")]
		ULDINSPPNP_PICK,
		[DefaultValueAttribute("ULDINSPPNP/PLACE")]
		ULDINSPPNP_PLACE,
		[DefaultValueAttribute("ULDINSPPNP/PNP")]
		ULDINSPPNP_PNP,
		[DefaultValueAttribute("ULDINSPPNP/READY POS")]
		ULDINSPPNP_READYPOS,

		[DefaultValueAttribute("ULDBUFFER/HOME")]
		ULDBUFFER_HOME,
		[DefaultValueAttribute("ULDBUFFER/LOADWAIT")]
		ULDBUFFER_LOADWAIT,
		[DefaultValueAttribute("ULDBUFFER/UNLOADWAIT")]
		ULDBUFFER_UNLOADWAIT,

		[DefaultValueAttribute("REJECTBUFFER/HOME")]
		REJECTBUFFER_HOME,
		[DefaultValueAttribute("REJECTBUFFER/READY")]
		REJECTBUFFER_READY,
		[DefaultValueAttribute("REJECTBUFFER/CHANGE")]
		REJECTBUFFER_CHANGE,
		[DefaultValueAttribute("REJECTBUFFER/VIBRATE")]
		REJECTBUFFER_VIBRATE,

		[DefaultValueAttribute("UNLOADPNP/HOME")]
		UNLOADPNP_HOME,
		[DefaultValueAttribute("UNLOADPNP/PICK BUFFER")]
		UNLOADPNP_PICKBUFFER,
		[DefaultValueAttribute("UNLOADPNP/PICK REJECT")]
		UNLOADPNP_PICKREJECT,
		[DefaultValueAttribute("UNLOADPNP/PLACE")]
		UNLOADPNP_PLACE,
		[DefaultValueAttribute("UNLOADPNP/PNP")]
		UNLOADPNP_PNP,
		[DefaultValueAttribute("UNLOADPNP/READY")]
		UNLOADPNP_READY,
		[DefaultValueAttribute("UNLOADPNP/SORT")]
		UNLOADPNP_SORT,
		[DefaultValueAttribute("UNLOADPNP/PICK REJECT RETRY")]
		UNLOADPNP_PICKREJECTRETRY,

		[DefaultValueAttribute("UNLOADTRAY1/HOME")]
		UNLOADTABLE1_HOME,
		[DefaultValueAttribute("UNLOADTRAY2/HOME")]
		UNLOADTABLE2_HOME,
		[DefaultValueAttribute("UNLOADTRAY1/UP")]
		UNLOADTABLE1_UP,
		[DefaultValueAttribute("UNLOADTRAY2/UP")]
		UNLOADTABLE2_UP,
		[DefaultValueAttribute("UNLOADTRAY1/DOWN")]
		UNLOADTABLE1_DOWN,
		[DefaultValueAttribute("UNLOADTRAY2/DOWN")]
		UNLOADTABLE2_DOWN,
		[DefaultValueAttribute("UNLOADTRAY1/GRIP")]
		UNLOADTABLE1_GRIP,
		[DefaultValueAttribute("UNLOADTRAY2/GRIP")]
		UNLOADTABLE2_GRIP,
		[DefaultValueAttribute("UNLOADTRAY1/RELEASE")]
		UNLOADTABLE1_RELEASE,
		[DefaultValueAttribute("UNLOADTRAY2/RELEASE")]
		UNLOADTABLE2_RELEASE,

		[DefaultValueAttribute("ULDTRANS/HOME")]
		ULDTRANS_HOME,
		[DefaultValueAttribute("ULDTRANS/PICK")]
		ULDTRANS_PICK,
		[DefaultValueAttribute("ULDTRANS/PLACE")]
		ULDTRANS_PLACE,
		[DefaultValueAttribute("ULDTRANS/PLACE CONV")]
		ULDTRANS_PLACECONV,
		[DefaultValueAttribute("ULDTRANS/PNP")]
		ULDTRANS_PNP,
		
        [DefaultValueAttribute("UNLOADER/HOME")]
		UNLOADER_HOME,
		[DefaultValueAttribute("UNLOADER/UNLOAD")]
		UNLOADER_UNLOAD,
		[DefaultValueAttribute("UNLOADER/LIFTUP")]
		UNLOADER_LIFTUP,
		[DefaultValueAttribute("UNLOADER/LIFTDOWN")]
		UNLOADER_LIFTDOWN,
		[DefaultValueAttribute("UNLOADER/ALIGN")]
		UNLOADER_ALIGN,

		//UbiGEM Seq 관련
		[DefaultValueAttribute("SECS/GEM TRAY VALIDATION")]
		UBIGEM_TRAY_VALIDATION,
		[DefaultValueAttribute("SECS/GEM ALARM SET")]
		UBIGEM_ALARM_SET,

		[DefaultValueAttribute("JOB TEACHING/LOAD")]
		JOBTEACHING_LOAD,
		[DefaultValueAttribute("JOB TEACHING/UNLOAD")]
		JOBTEACHING_UNLOAD,
			
		[DefaultValueAttribute("INSPECTION TOP/HOME")]
		INSP_TOP_HOME,
		[DefaultValueAttribute("INSPECTION BOTTOM/HOME")]
		INSP_BTM_HOME,

		Max,
	}

	public	enum	Tact
	{
		[DefaultValueAttribute("LOAD CONVEYOR START")]
		LOAD_CONVEYOR_START,
		[DefaultValueAttribute("TRAY LOAD START")]
		TRAY_LOAD_START,
		[DefaultValueAttribute("LOAD PnP START")]
		LOAD_PnP_START,
		[DefaultValueAttribute("UNLOAD PnP START")]
		UNLOAD_PnP_START,
		[DefaultValueAttribute("TRAY UNLOAD START")]
		TRAY_UNLOAD_START,
		[DefaultValueAttribute("UNLOAD CONVEYOR START")]
		UNLOAD_CONVEYOR_START,
	}

	public	enum	ProdExist
	{
		None,
		Mismatch,
		Exist,
	}
	
	public	enum	ProdPickStatus
	{
		Normal,
		Mismatch,
	}

	public	enum	AutoRunMode
	{
		[DefaultValueAttribute("NORMAL")]
		Normal,
		[DefaultValueAttribute("CYCLE STOP")]
		CycleStop,//피커가 자재를 안집고 있도록 하고 멈춤
		[DefaultValueAttribute("QUIT LOT")]
		QuitLot,//현재까지 진행했던 제품은 분류해서 배출, 그외에 제품은 검사 없이 배출
		[DefaultValueAttribute("END LOT")]
		EndLot,//cap 을 벗긴 제품까지는 검사 진행 후 배출
	}
	
	public	enum	LotEndMode
	{
		Normal,
		LotEnd,//언로드 PnP가 마지막 자재를 내려놓을떄
	}

	public	enum	ModuleType
	{
		[DefaultValueAttribute("UNKNOWN")]
		UNKNOWN,
		[DefaultValueAttribute("OES30")] 
		OES30,
		[DefaultValueAttribute("OES42")]
		OES42,
		[DefaultValueAttribute("OES80")]
		OES80,
		[DefaultValueAttribute("OES110")]
		OES110,
		[DefaultValueAttribute("OES111.49")]
		OES111,
		[DefaultValueAttribute("LPCAMM")]
		LPCAMM,
	}
	
	public	enum	ModuleGeneration
	{
		[DefaultValueAttribute("UNKNOWN")]
		UNKNOWN,
		[DefaultValueAttribute("DDR4")] //추후 필요시 SO-DIMM (25X2) 등으로 row/col갯수에 따라 추가
		DDR4,
		[DefaultValueAttribute("DDR5")]
		DDR5,
	}

	public	enum	RejectRunMode
	{
		Normal,
		VerifyReq,
		VerifyComplete,
	}
	
	public	enum	SeqMsg
	{
		Unknown,
		VerifyReqSel,	//시뮬레이션 용
		VerifyReq,		//베리파이 진행 요구
		KeyLockErr,		
		RejectNone,
		UnloadNone,
		TraySize,
		InspPause,
		BypassByNG,
		BypassByContinueNG,
		RejectTrayChange,
		ManualBarcode,
		TrayBarcodeMisMatch,	//언로드에서 읽었을때, 이전과 다른 경우
		LidBarcodeSuccess,		//Lid를 사용하지 않는데 바코드가 읽힌 경우
		LidBarcodeFail,			//Lid를 사용하는데, 바코드가 읽히지 않은 경우
		TrayBarcodeFail,		//트레이 바코드를 읽지 못한 경우
		TrayBarcode_RegexMatchFail,		//MES에서 받은 정규식과 매치 실패한 경우
		LidBarcode_RegexMatchFail,		//MES에서 받은 정규식과 매치 실패한 경우
		PCBMisMatch,
		PCBMisMatch_FirstTray,
		TrayMisMatch,		//로드 컨베이어에 트레이가 있어야하는데 없거나, 없어야 하는데 있는 경우
		SGEMConnect,		
		MESNG,
		JOBCHECKNG,
		SEQERR,
		SEQINFO,

		//Enum 추가시 터미널 앞에 추가 (get Key Count 관련)
		Terminal1,		//MES에서 받은 터미널 메세지 출력
		Terminal2,
		Terminal3,
		Terminal4,
		Terminal5,
	}
	
	public enum		RunStateType
	{
		NormalRun,			// 일반 런 상태
		CameraIdle,			// 카메라 검사를 위해 보류 중인 PCB가 없는 경우
		VerifyAlarm,		//제어 PC가 확인 알람을 표시하여 OP에게 RPS에서 보류 중인 판단을 완료하도록 상기시키는 경우
		RejectTrayAlarm,	// 제어 PC가 Reject 트레이 알람을 표시하여 OP에게 새로운 빈 Reject 트레이를 변경하도록 상기시키는 경우
		NormalStop,			//일반 일시정지/정지 상태
		ErrorStop			//SAOI에 오류 알람이 발생하여 일시 중지 모드로 변경되면 Error Time은 오류 알람 시작(PAUSE mode)부터 알람이 해제될 때까지(Production mode) 기록된 시간
	}

	public enum ProdHandlingStatus
	{
		Normal,
		Mismatch,
		Aligned,
	}

	public enum		UPHStep
	{
		FirstInsp,
		LastInsp,
	}
	
	public	struct	stPocket
	{
		public	int		nCol;
		public	int		nRow;
		public	int		nColCnt;
		public	bool	bFromReject;

		public	stPocket(int _nCol, int _nRow, int _nColCnt, bool _bFromReject)
		{
			nCol		= _nCol;
			nRow		= _nRow;
			nColCnt		= _nColCnt;
			bFromReject	= _bFromReject;
		}

		public	void	Set(int _nCol, int _nRow, int _nColCnt, bool _bFromReject)
		{
			nCol		= _nCol;
			nRow		= _nRow;
			nColCnt		= _nColCnt;
			bFromReject	= _bFromReject;
		}

		public	void	Reset()
		{
			nCol		= -1;
			nRow		= 0;
			nColCnt		= 0;
			bFromReject	= false;
		}

		public	int		PocketNo
		{
			get
			{
				return nCol + nRow * nColCnt;
			}
		}
	}

	public	struct	stActPicker
	{
		public	stPocket[]	Picker;

		public	int			Count
			{
				get
				{
					if (Picker == null)			return 0;
					else						return Picker.Length;
				}
			}


		public	stActPicker(int nPickerCnt)
		{
			Picker	= new stPocket[nPickerCnt];

			for (int i = 0; i < nPickerCnt; i++)
			{
				Picker[i]	= new stPocket();
				Picker[i].Reset();
			}
		}

		public	void	SetAll(int _nCol, int _nRow, int _nColCnt, bool _bFromReject = false)
		{
			if (Picker == null)			return;

			for (int i = 0; i < Picker.Length; i++)
			{
				Picker[i].Set(_nCol, _nRow, _nColCnt, _bFromReject);
			}
		}

		public	void	Set(int nIndex, int _nCol, int _nRow, int _nColCnt, bool _bFromReject = false)
		{
			if (Picker == null)						return;
			if (Picker.Length-1 < nIndex)			return;

			Picker[nIndex].Set(_nCol, _nRow, _nColCnt, _bFromReject);
		}

		public	stPocket	Get(int nIndex)
		{
			if (Picker == null)						return new stPocket();
			if (Picker.Length-1 < nIndex)			return new stPocket();

			return Picker[nIndex];
		}

		public	void	ResetAll()
		{
			if (Picker == null)			return;

			for (int i = 0; i < Picker.Length; i++)
			{
				Picker[i].Reset();
			}
		}

		public	void	Reset(int nIndex)
		{
			if (Picker == null)						return;
			if (Picker.Length-1 < nIndex)			return;

			Picker[nIndex].Reset();
		}

		public	int		GetPocketNo(int nIndex)
		{
			if (Picker == null)						return -1;
			if (Picker.Length-1 < nIndex)			return -1;

			return Picker[nIndex].PocketNo;
		}

		public	int		CountExist()
		{
			if (Picker == null)						return 0;

			int	ncnt	= 0;

			for (int i = 0; i < Picker.Length; i++)
			{
				if (Picker[i].PocketNo > -1)			ncnt++;
			}

			return ncnt;
		}

		public	int[]	GetPocketNoAll()
		{
			if (Picker == null)						return new int[0];
				
			int[] nno	= new int[Picker.Length];

			for (int i = 0; i < Picker.Length; i++)
			{
				nno[i]	= Picker[i].PocketNo;
			}

			return nno;
		}

		public	bool	ContainsPocket(int nSlot)
		{
			if (Picker == null)						return false;

			for (int i = 0; i < Picker.Length; i++)
			{
				if (Picker[i].PocketNo == nSlot)			return true;
			}

			return false;
		}
	}

	[Serializable]
	public	struct stRunLotInfo
	{
		public	string	UserID;
		public	string	WorkOrder;
		public	string	KeyValue;
		public	string	ProgramID_Top;
		public	string	ProgramID_Btm;
		public	string	ProdCount;
		public	string	StartTime;
		public	bool	RecipeCheck;
	}
}
