using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	#region Command enum
    
	public enum teEVENT1
	{
		PRODUCT,
		TECHINCIAN,
		QA,
		EQUIPMENT,
		MAX,
	}

	public enum teEVENT2
	{
		REPORTSTATUS,
		REPAIRECHECK,
		REGISTERSTATUS,
		CHECKCOMPLETED,
		LOTSTART,
		LOTEND,
		COMPLETED,
		VERIFY,
		JAM,
		RUN,
		STOP,
		NODEVICE,
		EQUIPMENTCHECK,
		NETWORK,
		FILESTART,
		FILEEND,
		REGISTERREPORT,
        PM,
        USEENGR,
        CONVERSION,
		MAX,
	}

	public enum REQ //send
	{
		[DefaultValueAttribute("ControlStateChanged")]
		ControlStateChanged = 1001, //
		[DefaultValueAttribute("MaterialInputCompletedReport")]
		MaterialInputCompletedReport = 1004,
		[DefaultValueAttribute("MaterialIDReport")]
		MaterialIDReport = 1005,
		[DefaultValueAttribute("ProcessStateChanged")]
		ProcessStateChanged = 1006,  //
		[DefaultValueAttribute("ProcessProgramSelected")]
		ProcessProgramSelected = 1007,
		[DefaultValueAttribute("MaterialProcessStartedReport")]
        MaterialProcessStartedReport = 1008,
		[DefaultValueAttribute("MaterialProcessCompletedReport")]
        MaterialProcessCompletedReport = 1009,
		[DefaultValueAttribute("MaterialOutputCompletedReport")]
		MaterialOutputCompletedReport = 1010,
		[DefaultValueAttribute("AlarmSendReport")]
		AlarmSendReport = 1011,
		[DefaultValueAttribute("AlarmClearReport")]
		AlarmClearReport = 1012,
		[DefaultValueAttribute("CanceledReport")]
		CanceledReport = 1013,
		[DefaultValueAttribute("ROSMaterialProcessStartedReport")]
		ROSMaterialProcessStartedReport = 1014,
		[DefaultValueAttribute("ROSMaterialProcessCompletedReport")]
		ROSMaterialProcessCompletedReport = 1015,
		[DefaultValueAttribute("Open Lid Request")]	
		OpenLidRequest = 1016,
		[DefaultValueAttribute("Inspection Request")]	
		InspectionRequest = 1017,
		[DefaultValueAttribute("Inspection Result")]	
		InspectionResult  = 1018,
		[DefaultValueAttribute("TRAY_VALIDATION")]	
		TRAY_VALIDATION = 1019,
		[DefaultValueAttribute("LOT_END")]	
		LOT_END = 4000,


		UNKWON = 9999,
	}

	public enum REP //recv
	{
		PP_SELECT,
		START,
		STOP,
		REMOTE,
		LOCAL,
		SENDWORKINFO,
		INSP_RESULT_CONFIRM,
    }
    #endregion

    #region Reply Data Name Enum
    public enum CPNAME_PP_SELECT
	{
		PPID,
		MAX,
	}

	public enum CPNAME_START
	{
		
	}

	public enum CPNAME_STOP
	{

	}

	public enum CPNAME_REMOTE
	{

	}

	public enum CPNAME_LOCAL
	{

	}

	public enum CPNAME_SENDWORKINFO
	{
		WO,
		PPID_TOP,
		PPID_BOTTOM,
		TrayID,
		LidID,
		HasLid,
		Column,
		Row,
		TotalQty,
		FormFactor,
		DDRGeneration,
		MAX,
	}

	public enum CPNAME_INSP_RESULT_CONFIRM
	{
		SN1,
		RESULT1,
		REMARK1,
		SN2,
		RESULT2,
		REMARK2,
		SN3,
		RESULT3,
		REMARK3,
		SN4,
		RESULT4,
		REMARK4,
		SN5,
		RESULT5,
		REMARK5,
		SN6,
		RESULT6,
		REMARK6,
		SN7,
		RESULT7,
		REMARK7,
		SN8,
		RESULT8,
		REMARK8,
		MAX,
	}
	public enum ProcessState
	{
		INIT = 1,
		IDLE ,
		SETUP ,
		READY ,
		EXECUTING ,
		PAUSE,
		UNKNOWN,

	}
	
	#endregion
	
    public enum VID
    {
		MDLN,
		SOFTREV,
		user_name,
		
        Clock = 1001,
        ControlState = 1002,
        OperatorID = 1003,
		LaneID = 1004,
		ModelID = 1005,
		OrderType = 1006,
		ProcessProgramID = 1007,
		MaterialInputType = 1008,
		ProcessState = 1010,
		MaterialOutputType = 1011,
		MaterialOutputState = 1012,
		AlarmID = 1013,
		AlarmSet = 1014,
		CancelText = 1015,
		CancelType = 1016,
		MaterialInformation = 2001,
		ArrayMaterialInformation = 2002,
		ArrayMaterialInfo = 2003,
		MaterialID = 3001,
		MaterialENDTYPE = 3002,
		MaterialSIDE = 3003,
		MaterialJudge = 3004,
		ArrayMaterialID = 3005,
		ArrayMaterialEndType = 3006,
		ArrayMaterialPosition = 3007,
		ArrayMaterialSide = 3008,
		ArrayMaterialJudge = 3009,
    }

	public enum AlarmID
	{
		[DefaultValueAttribute("PCB Load Fail")]
		PCBLoadFail = 41001,
		[DefaultValueAttribute("Part Inspection Fail")]
		PartInspectionFail = 41007,
		[DefaultValueAttribute("Error Motor X")]
		ErrorMotorX = 41008,
		[DefaultValueAttribute("Error Motor Y")]
		ErrorMotorY = 41009,
		[DefaultValueAttribute("Emergency Stop")]
		EmergencyStop = 41010,
		[DefaultValueAttribute("Barcode Search Fail")]
		BarcodeSearchFail = 41013,
		[DefaultValueAttribute("PCB Position error")]
		PCBPositionerror = 41016,
		[DefaultValueAttribute("Push up plate error")]
		Pushupplateerror = 41017,
		[DefaultValueAttribute("Rail size error")]
		Railsizeerror = 41020,
		[DefaultValueAttribute("PCB Time out error")]
		PCBTimeouterror = 41021,
		[DefaultValueAttribute("OK PCB inserted in NG rack")]
		OKPCBinsertedinNGrack = 41033,
		[DefaultValueAttribute("NG PCB inserted in OK rack")]
		NGPCBinsertedinNGrack = 41033,
		[DefaultValueAttribute("DB connect fail")]
		DBconnectfail = 41055,
		[DefaultValueAttribute("Conveyor width is different")]
		Conveyorwidthisdifferent = 41057,
		[DefaultValueAttribute("Barcode Fail")]
		BarcodeFail = 41059,
	}
}
