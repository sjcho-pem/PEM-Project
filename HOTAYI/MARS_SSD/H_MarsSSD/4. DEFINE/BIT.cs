using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
    /// <summary>
    /// Channel ( Ch00~Ch15 )
    /// Total ( 0 ~ 255 )
    /// </summary>
    public enum BIT
    {
        // CH-00
        AllHome = 0,
        AllHoming,
        Error,
        Warning,
        AutoRun,
        Pause,
        DryRun,
        IDLE,
        EMO,
        ControlDeviceState,
        Buzzer,
        DoorOpen,
        KeyLockAuto,
        MotorJogMode,
		UIAllHomeStopping,
		MotorTeachingMode,

        // CH-01
		TopInspConnect = 16,
		BtmInspConnect,
		ReviewConnect,
		SmartVisionLoaderConnect,
		SmartVisionTableConnect,
		TotalBarcodeConnect,
		Loader1BarcodeConnect,
		Loader2BarcodeConnect,
		Unload1BarcodeConnect,
		Unload2BarcodeConnect,
		Reject1BarcodeConnect,
		Reject2BarcodeConnect,
		UbiGemConnect,
		InspLock,
        RejectWarning,
        RejectBuzzer,
		WarningBuzzer,
    //  VerifyBuzzerOff = 30,
	//	VerifyMsgView	= 31,

        // CH-02
        IsManualReq = 32,
		LotLoadStart,

        // CH-03
		TrayCleanUpSafety = 48,

        // CH-04
        //ReferenceRequestTop1 = 64,
        //ReferenceRequestTop2,
        //ReferenceRequestBtm1,
        //ReferenceRequestBtm2,

        // CH-05
        NewDay = 80,
		RecipeAutoChanging,
		RecipeChangeFail,

        // CH-06
		MainDisplayShow	= 96,
		TrayReplaceMsgShow,

		// CH-07
		Bits71= 112,

        // CH-08
        Bits80 = 128,
        Bits81,

        // CH-09
        Bits90 = 144,
        Bits91,

        // CH-10
        LotEnd = 160,
        BitsA1,

        // CH-11
        BitsB0 = 176,
        BitsB1,

        // CH-12
        BitsC0 = 192,
        BitsC1,

        // CH-13
        UBIGEM_PP_SELECT = 208,
		UBIGEM_SENDWORKINFO,
		UBIGEMCheckingEquipment,
		UBIGEM_INSP_RESULT_CONFIRM,
		UBIGEM_PROCESS_STATE_CHANGE,

        // CH-14
        BitsE0 = 224,
        BitsE1,

        // CH-15
        BitsF0 = 240,
        BitsF1,


        BitsFF = 255
,
    }
}
