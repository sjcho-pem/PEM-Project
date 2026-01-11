using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace H_MarsSSD
{
    /// <summary>
    /// Recipe Data Index : 0 ~ 255
    /// </summary>
    public enum DEVICE
    {
        [DefaultValueAttribute("Module Type")]
        ModuleType,
		[DefaultValueAttribute("Tray Column Slot Count")]
        TrayColCnt,
		[DefaultValueAttribute("Tray Row Slot Count")]
        TrayRowCnt,
		[DefaultValueAttribute("Grip Step Count")]
		GripStepCnt,
		[DefaultValueAttribute("Tray Height")]
		TrayHeight,
		[DefaultValueAttribute("Tray Stacked Height")]
		TrayStackedHeight,

        [DefaultValueAttribute("Load Transfer Down Distance")]
        LDTransDnDist,
        [DefaultValueAttribute("Unload Transfer Down Distance")]
        ULDTransDnDist,

		[DefaultValueAttribute("Tray Size Check Sensor #1 Use [ Not Use(0), Use(1) ]")]
        TraySizeChkSen1Use,
		[DefaultValueAttribute("Tray Size Check Sensor #3 Use [ Not Use(0), Use(1) ]")]
        TraySizeChkSen3Use,
		[DefaultValueAttribute("Tray Size Check Sensor #4 Use [ Not Use(0), Use(1) ]")]
        TraySizeChkSen4Use,
		
		[DefaultValueAttribute("PnP Tray First Direction [ X Left(L), X Right(R), Y Top(T), Y Bottom(B) ]")]
		TrayFirstDir,
		[DefaultValueAttribute("PnP Tray Second Direction [ X Left(L), X Right(R), Y Top(T), Y Bottom(B) ]")]
		TraySecondDir,
		
		[DefaultValueAttribute("Count - Reject Tray Change")]
		RejectTrayChangeCount,
		[DefaultValueAttribute("Count - Reject Tray Verify")]
		RejectTrayVerifyCount,

		
		[DefaultValueAttribute("Smart Vision Job Number")]
		FTVJob,

		[DefaultValueAttribute("Delay - Tray Loader Conv Load Delay")]
		TrayLoaderConvLoadDelay,
		[DefaultValueAttribute("Delay - Tray Loader Conv Unload Delay")]
		TrayLoaderConvUnloadDelay,
		[DefaultValueAttribute("Delay - Tray Unloader Conv Unload Delay")]
		TrayUnloaderConvUnloadDelay,

        [DefaultValueAttribute("Loadlift - Tray Start Position Offset Value")]
        CountSensorDistance,

        RecipeData255 = 255,
    }
}
