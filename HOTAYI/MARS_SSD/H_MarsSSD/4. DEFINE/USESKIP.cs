using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace H_MarsSSD
{
    /// <summary>
    /// Channel ( Ch00~Ch01 )
    /// Total ( 0 ~ 31 )
    /// </summary>
    public enum USESKIP
    {
        // CH-00
		[DefaultValueAttribute("Wait Until the previous lot is completed")]
		PrevLotEndWait,			// 이전 Lot의 트레이가 모두 배출 될 때까지 다음 Lot을 진행 시키지 않음
		[DefaultValueAttribute("LoadPnPStop")]
		LoadPnPStop,

		[DefaultValueAttribute("Unload Table Aligner")]
		UnloadTableAligner = 8,
		[DefaultValueAttribute("Reject Buffer Aligner")]
		RejectBufferAligner = 9,
		[DefaultValueAttribute("Inspection Offline Job Check")]
		InspOfflineJobCheck = 10,

		[DefaultValueAttribute("The Result of Skipping inspection is NG")]
		InspSkipResultNG = 13,		// 검사 스킵한 제품의 검사결과를 NG로 설정
		[DefaultValueAttribute("The Result of Skipping inspection is Random")]
		InspSkipResultRandom,	// 검사 스킵한 제품의 검사결과를 Random으로 설정
		[DefaultValueAttribute("Inspect Result Always OK")]
        InspResultAlwaysOK,

        // CH-01
		[DefaultValueAttribute("Inspection Debug(Use Plate1)")]
        InspDebugPlate1 = 16,
		[DefaultValueAttribute("Inspection Debug(Use Plate2)")]
        InspDebugPlate2,
		[DefaultValueAttribute("Top Inspection")]
        TopInspection,
		[DefaultValueAttribute("Bottom Inspection")]
        BtmInspection,
		[DefaultValueAttribute("Review Program")]
        SendRejectProdId,
		[DefaultValueAttribute("SECS/GEM")]
        SGEM,
		[DefaultValueAttribute("Barcode Read")]
        BarcodeRead,
		[DefaultValueAttribute("Smart Vision Loader")]
        SmartVisionLoader,
		[DefaultValueAttribute("Smart Vision Table")]
        SmartVisionTable,
		[DefaultValueAttribute("SAFETY (KEY LOCK, DOOR OPEN)")]
        Safety	= 27,


		[DefaultValueAttribute("RunState Log")]
        RunStateLog = 30, 
		[DefaultValueAttribute("Dry Run")]
        DRYRUN = 31,
    }

	public	enum	UsePartName
	{
		[DefaultValueAttribute("Load Picker #1")]
		Picker1,
		[DefaultValueAttribute("Load Picker #2")]
		Picker2,
		[DefaultValueAttribute("Load Picker #3")]
		Picker3,
		[DefaultValueAttribute("Load Picker #4")]
		Picker4,
		[DefaultValueAttribute("Load Table #1")]
		LDTable1,
		[DefaultValueAttribute("Load Table #2")]
		LDTable2,
		[DefaultValueAttribute("Load Buffer #1")]
		LDBuf1,
		[DefaultValueAttribute("Load Buffer #2")]
		LDBuf2,
		[DefaultValueAttribute("Inspection Plate #1")]
		Insp1,
		[DefaultValueAttribute("Inspection Plate #2")]
		Insp2,
		[DefaultValueAttribute("Unload Table #1")]
		ULDTable1,
		[DefaultValueAttribute("Unload Table #2")]
		ULDTable2,
		Max,
	}

	[Flags]
	public	enum	UsePartMode
	{
		Disable	= 0,
		Load	= 1<<1,
		Unload	= 1<<2,
		Enable	= Load|Unload,
	}
}
