using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace H_MarsSSD
{
    // SDM(String형 DM)
    public enum eSDM
    {
        ID,
        PW,
		
        CurKeyValue, //연속랏 진행 시, 현재 언로드 하고 있는 LOT ID를 알 수 있도록  -ndm으로
        CurTempKeyValue, //연속랏 진행 시, 랏앤드중에 바뀌지 않도록 업데이트해둿다가 다른곳에서 랏앤드 이후 업데이트

		ReportCause	= 10, 

		SUMMARY_QUALITY_NAME,
		PVI_MachineName,
		PVI_GroupName,

		WO,
		PROGRAMID_TOP,
		PROGRAMID_BOTTOM,
		TRAYID,
		LIDID,
		HASLID,
		COLUMN,
		ROW,
		TOTALQTY,
		FORMFACTOR,
		DDRGENERATION,
		
		SmartVision1IP = 30,
		SmartVision1Port ,
		SmartVision2IP ,
		SmartVision2Port ,

		TrayColor = 50,
		CapColor,
		UnknownColor,
		NGColor,
		UserNGColor,
		OKColor,
		MisMatchColor,
		TrayOKColor,
		TrayUnknownColor,
		TrayEmptyColor,
		
		DM255	= 255,
    }
}
