using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace H_MarsSSD
{
    // NDM(Int형 DM)
    public enum eNDM
    {
		[DefaultValueAttribute("Main Seq Thread Ticks")]
        Ticks = 0,  // Main Seq Thread Ticks
		[DefaultValueAttribute("Log Storage Period")] // 0 : 파일 삭제 안함
        LogStoragePeriod, 
		
        [DefaultValueAttribute("Delay - LoadLift Conv Load Delay")]
        LoadLiftConvLoadDelay = 20,
		[DefaultValueAttribute("Delay - Tray Clean Grip")]
        TrayCleanGripDelay,
		[DefaultValueAttribute("Delay - Load Lift Align")]
        LDLiftAlignDelay,
		[DefaultValueAttribute("Delay - Unload Lift Align")]
        ULDLiftAlignDelay,
		[DefaultValueAttribute("Delay - Tray Table Grip")]
		TrayTableGripDelay,
		
		[DefaultValueAttribute("Delay - Load PnP Vac Delay")]
		LoadPnPVacDelay = 37,
		[DefaultValueAttribute("Delay - Load Insp PnP Vac Delay")]
		LoadInspPnPVacDelay,
		[DefaultValueAttribute("Delay - Unload Insp PnP Vac Delay")]
		UnloadInspPnPVacDelay,
		[DefaultValueAttribute("Delay - Unload PnP Vac Delay")]
		UnloadPnPVacDelay,

		[DefaultValueAttribute("Delay - Auto Logout, 0 = Not used, Min Value = 30000")]
		AutoLogoutDelay = 44,
		[DefaultValueAttribute("Delay - UnloadLiftWarning, 0 = Not used")]
		ULDWarningDelay,
		
		[DefaultValueAttribute("Delay - InspSkip Inspection Delay")]
		InspSkipInspDelay,
		[DefaultValueAttribute("Delay - InspWait TimeOut(In Unload Buffer)")]
		InspWaitTimeOut,
        [DefaultValueAttribute("Delay - Inspection Communication TimeOut Delay")]
        InspCommTimeOut,
		[DefaultValueAttribute("Delay - SecsGem Communication TimeOut Delay")]
		SecsGemCommTimeOut,
		[DefaultValueAttribute("Delay - Insp Home TimeOut")]
		InspHomeTimeOut,
        [DefaultValueAttribute("Delay - RPS Communication TimeOut Delay")]
        RpsCommTimeOut,

        [DefaultValueAttribute("Value - Table AirVibrate Time Value")]
        AirVibrateTime = 55,


        [DefaultValueAttribute("Count - Tray Clean Repeat")]
        TrayCleanCount = 60,
		[DefaultValueAttribute("Count - Memory Clean Repeat")]
        MemoryCleanCount,
		[DefaultValueAttribute("Count - Total NG Threshold in Lot")]
		TotalNGCount,
		[DefaultValueAttribute("Count - Continuous NG Threshold")]
		ContinuousNGCount,
		[DefaultValueAttribute("Count - Fidu Error Threshold")]
		FiduErrorThreshold,

		[DefaultValueAttribute("Count - Uph Average")]
		UphAverageCount = 70,
		[DefaultValueAttribute("Count - Remaining FillDone")]
		FillDoneRemainCount,
		[DefaultValueAttribute("Count - Cur Continuous NG")]
		CurContinuousNGCount,
		[DefaultValueAttribute("Count - UserOK Product")]
		CurUserOKProdCount ,
		[DefaultValueAttribute("Count - OK product")]
		CurOKProdCount,
		[DefaultValueAttribute("Count - NG Product")]
		CurNGProdCount,



        AnimationSpeed = 80,
        TopInspCurIdx,			// 0 : 준비위치,홈위치, 1: 1번검사 테이블, 2 : 2번 검사 테이블 
        TopInspNextIdx,			// 시퀀스에서는 Next만 수정
        BtmInspCurIdx,
        BtmInspNextIdx,
		SimTrayCnt,
		SimMotorSpeedRate,

		TimeoutAllHome	= 90,
		TimeoutInspection,

		
		
		AutoRunSWDown = 100,
		[DefaultValueAttribute("Reset Hour of Total Yeild. (0~23)")]
		ResetHourTotalYeild,

		OldAutoRunMode = 150,
		AutoRunMode,
		RejectRunMode,
		LotEndMode, // 0: normal, 1: Lotend 

		ManualMotorMoveDir,
		WritePvi,//0: Don't Write,  1: Write 
		RecentlyAlarmCode,
		DateTime = 160,


		OldRecipeIdx,
		[DefaultValueAttribute("Recipe Change")]
		RecipeChange,//0일땐 변경 X, 그외엔 해당하는 index로 체인지

        DM255 = 255,
    }
}
