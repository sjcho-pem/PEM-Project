using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public enum DI
	{
		[DefaultValueAttribute("EMO")]
		EMO,
		[DefaultValueAttribute("START")]
		START,
		[DefaultValueAttribute("PAUSE")]
		PAUSE,
		[DefaultValueAttribute("RESET")]
		RESET,
		[DefaultValueAttribute("Buzzer Off Btn")]
		BuzzerOffBtn,
		[DefaultValueAttribute("Key Lock")]
		KeyLock,
		[DefaultValueAttribute("Main Air")]
		MainAir,
		[DefaultValueAttribute("Cleaner Air")]
		CleanerAir,
		[DefaultValueAttribute("Dust Control Alarm")]
		DustControlAlarm,
		[DefaultValueAttribute("CH00_None09")]
		CH00_None09,
		[DefaultValueAttribute("CH00_None10")]
		CH00_None10,
		[DefaultValueAttribute("CH00_None11")]
		CH00_None11,
		[DefaultValueAttribute("Top Insp End1")]
		TopInspEnd1,
		[DefaultValueAttribute("Top Insp End2")]
		TopInspEnd2,
		[DefaultValueAttribute("Btm Insp End1")]
		BtmInspEnd1,
		[DefaultValueAttribute("Btm Insp End2")]
		BtmInspEnd2,

		[DefaultValueAttribute("Top Front Door Sen")]
		TopFrontDoorSen,
		[DefaultValueAttribute("Top Left Door Sen")]
		TopLeftDoorSen,
		[DefaultValueAttribute("Top Rear Door Sen")]
		TopRearDoorSen,
		[DefaultValueAttribute("Top Right Door Sen")]
		TopRightDoorSen,
		[DefaultValueAttribute("Btm Door Sen")]
		BtmDoorSen,
        [DefaultValueAttribute("CH01_None05")]
        CH01_None05,
        [DefaultValueAttribute("CH01_None06")]
        CH01_None06,
        [DefaultValueAttribute("CH01_None07")]
		CH01_None07,
		[DefaultValueAttribute("CH01_None08")]
		CH01_None08,
		[DefaultValueAttribute("CH01_None09")]
		CH01_None09,
		[DefaultValueAttribute("CH01_None10")]
		CH01_None10,
		[DefaultValueAttribute("CH01_None11")]
		CH01_None11,
		[DefaultValueAttribute("CH01_None12")]
		CH01_None12,
		[DefaultValueAttribute("CH01_None13")]
		CH01_None13,
		[DefaultValueAttribute("CH01_None14")]
		CH01_None14,
		[DefaultValueAttribute("CH01_None15")]
		CH01_None15,

		[DefaultValueAttribute("LD Tray Lift Star tBtn")]
		LDTrayLiftStartBtn,
		[DefaultValueAttribute("LD Tray Lift Up Btn")]
		LDTrayLiftUpBtn,
		[DefaultValueAttribute("LD Tray Lift Dn Btn")]
		LDTrayLiftDnBtn,
		[DefaultValueAttribute("LD Tray Lift Area Sen1")]
		LDTrayLiftAreaSen1,
		[DefaultValueAttribute("LD Tray Lift Area Sen2")]
		LDTrayLiftAreaSen2,
		[DefaultValueAttribute("LD Tray Lift Limit Sen")]
		LDTrayLiftLimitSen,
		[DefaultValueAttribute("LD Tray Lift Exist Sen")]
		LDTrayLiftExistSen,
		[DefaultValueAttribute("LD Tray Lift Dec Sen")]
		LDTrayLiftDecSen,
		[DefaultValueAttribute("LD Tray Lift Count Sen")]
		LDTrayLiftCountSen,
		[DefaultValueAttribute("CH02_None09")]
		CH02_None09,
		[DefaultValueAttribute("CH02_None10")]
		CH02_None10,
		[DefaultValueAttribute("LD Tray Conv Exist Sen")]
		LDTrayConvExistSen,
		[DefaultValueAttribute("LD Tray Conv Dec Sen")]
		LDTrayConvDecSen,
		[DefaultValueAttribute("LD Conv Stopper Sen")]
		LDTrayConvStopperSen,
		[DefaultValueAttribute("LD Conv Stopper Fwd")]
		LDTrayConvStopperFwd,
		[DefaultValueAttribute("LD Conv Stopper Bwd")]
		LDTrayConvStopperBwd,

		[DefaultValueAttribute("LDTrayHolderGrip1Close")]
		LDTrayHolderGrip1Close,
		[DefaultValueAttribute("LDTrayHolderGrip1Open")]
		LDTrayHolderGrip1Open,
		[DefaultValueAttribute("LDTrayHolderGrip2Close")]
		LDTrayHolderGrip2Close,
		[DefaultValueAttribute("LDTrayHolderGrip2Open")]
		LDTrayHolderGrip2Open,
		[DefaultValueAttribute("CH03_None04")]
		CH03_None04,
		[DefaultValueAttribute("LDTrayTable1PadFwdCyl")]
		LDTrayTable1PadFwdCyl,
		[DefaultValueAttribute("LDTrayTable1PadBwdCyl")]
		LDTrayTable1PadBwdCyl,
		[DefaultValueAttribute("LDTrayTable1GripOpenCyl")]
		LDTrayTable1GripOpenCyl,
		[DefaultValueAttribute("LDTrayTable1ExistSen")]
		LDTrayTable1ExistSen,
		[DefaultValueAttribute("LDTrayTable Collide Sen")]
		LDTrayTableCollideSen,
		[DefaultValueAttribute("LDTrayTable2PadFwdCyl")]
		LDTrayTable2PadFwdCyl,
		[DefaultValueAttribute("LDTrayTable2PadBwdCyl")]
		LDTrayTable2PadBwdCyl,
		[DefaultValueAttribute("CH03_None12")]
        CH03_None12,
		[DefaultValueAttribute("CH03_None13")]
        CH03_None13,
		[DefaultValueAttribute("LDTrayTable2GripOpenCyl")]
        LDTrayTable2GripOpenCyl,
		[DefaultValueAttribute("LDTrayTable2ExistSen")]
		LDTrayTable2ExistSen,



		[DefaultValueAttribute("LD Tray Trans Exist Sen")]
		LDTrayTransExistSen,
		[DefaultValueAttribute("LD Tray Trans Pos Sen")]
		LDTrayTransPosSen,
		[DefaultValueAttribute("LD Tray Trans Safety Sen")]
		LDTrayTransSafetySen,
		[DefaultValueAttribute("CH04_None03")]
		CH04_None03,
		[DefaultValueAttribute("CH04_None04")]
		CH04_None04,
		[DefaultValueAttribute("CH04_None05")]
		CH04_None05,
		[DefaultValueAttribute("CH04_None06")]
		CH04_None06,
		[DefaultValueAttribute("CH04_None07")]
		CH04_None07,
		[DefaultValueAttribute("CH04_None08")]
		CH04_None08,
		[DefaultValueAttribute("CH04_None09")]
		CH04_None09,
		[DefaultValueAttribute("CH04_None10")]
		CH04_None10,
		[DefaultValueAttribute("CH04_None11")]
		CH04_None11,
		[DefaultValueAttribute("CH04_None12")]
		CH04_None12,
		[DefaultValueAttribute("CH04_None13")]
		CH04_None13,
		[DefaultValueAttribute("CH04_None14")]
		CH04_None14,
		[DefaultValueAttribute("CH04_None15")]
		CH04_None15,

		[DefaultValueAttribute("Tray Clean Exist Sen")]
		TrayCleanExistSen,
		[DefaultValueAttribute("Tray Clean Grip Open Cyl")]
		TrayCleanGripOpenCyl,
		[DefaultValueAttribute("Tray Clean Rotate0 Cyl")]
		TrayCleanRotate0Cyl,
		[DefaultValueAttribute("Tray Clean Rotate180 Cyl")]
		TrayCleanRotate180Cyl,
		[DefaultValueAttribute("Tray Clean Up Cyl")]
		TrayCleanUpCyl,
		[DefaultValueAttribute("Tray Clean Dn Cyl")]
		TrayCleanDnCyl,
		[DefaultValueAttribute("Tray Clean Air Left Cyl")]
		TrayCleanAirLeftCyl,
		[DefaultValueAttribute("Tray Clean Air Right Cyl")]
		TrayCleanAirRightCyl,
		[DefaultValueAttribute("Tray Clean Dust Open Cyl")]
		TrayCleanDustOpenCyl,
		[DefaultValueAttribute("Tray Clean Dust Close Cyl")]
		TrayCleanDustCloseCyl,
		[DefaultValueAttribute("Tray Clean Filter")]
		TrayCleanFilter,
		[DefaultValueAttribute("Tray Clean Air")]
		TrayCleanAir,
		[DefaultValueAttribute("CH05_None12")]
		CH05_None12,
		[DefaultValueAttribute("CH05_None13")]
		CH05_None13,
		[DefaultValueAttribute("CH05_None14")]
		CH05_None14,
		[DefaultValueAttribute("CH05_None15")]
		CH05_None15,

		[DefaultValueAttribute("LD PnP Up1 Cyl")]
		LDPnPUp1Cyl,
		[DefaultValueAttribute("LD PnP Dn1 Cyl")]
		LDPnPDn1Cyl,
		[DefaultValueAttribute("LD PnP Up2 Cyl")]
		LDPnPUp2Cyl,
		[DefaultValueAttribute("LD PnP Dn2 Cyl")]
		LDPnPDn2Cyl,
		[DefaultValueAttribute("LD PnP Up3 Cyl")]
		LDPnPUp3Cyl,
		[DefaultValueAttribute("LD PnP Dn3 Cyl")]
		LDPnPDn3Cyl,
		[DefaultValueAttribute("LD PnP Up4 Cyl")]
		LDPnPUp4Cyl,
		[DefaultValueAttribute("LD PnP Dn4 Cyl")]
		LDPnPDn4Cyl,
		[DefaultValueAttribute("LD PnP Vac1")]
		LDPnPVac1,
		[DefaultValueAttribute("LD PnP Vac2")]
		LDPnPVac2,
		[DefaultValueAttribute("LD PnP Vac3")]
		LDPnPVac3,
		[DefaultValueAttribute("LD PnP Vac4")]
		LDPnPVac4,
		[DefaultValueAttribute("CH06_None12")]
		CH06_None12,
		[DefaultValueAttribute("CH06_None13")]
		CH06_None13,
		[DefaultValueAttribute("Smart Camera Fwd")]
		SmartCameraFwd,
		[DefaultValueAttribute("Smart Camera Bwd")]
		SmartCameraBwd,

		[DefaultValueAttribute("LD Buf1 Grip1 Open Cyl")]
		LDBuf1Grip1OpenCyl,
		[DefaultValueAttribute("LD Buf1 Grip2 Open Cyl")]
		LDBuf1Grip2OpenCyl,
		[DefaultValueAttribute("LD Buf1 Grip3 Open Cyl")]
		LDBuf1Grip3OpenCyl,
		[DefaultValueAttribute("LD Buf1 Grip4 Open Cyl")]
		LDBuf1Grip4OpenCyl,
		[DefaultValueAttribute("LD Buf2 Grip1 Open Cyl")]
		LDBuf2Grip1OpenCyl,
		[DefaultValueAttribute("LD Buf2 Grip2 Open Cyl")]
		LDBuf2Grip2OpenCyl,
		[DefaultValueAttribute("LD Buf2 Grip3 Open Cyl")]
		LDBuf2Grip3OpenCyl,
		[DefaultValueAttribute("LD Buf2 Grip4 Open Cyl")]
		LDBuf2Grip4OpenCyl,
		[DefaultValueAttribute("LD Buf1 Grip1 Exist Sen")]
		LDBuf1Grip1ExistSen,
		[DefaultValueAttribute("LD Buf1 Grip2 Exist Sen")]
		LDBuf1Grip2ExistSen,
		[DefaultValueAttribute("LD Buf1 Grip3 Exist Sen")]
		LDBuf1Grip3ExistSen,
		[DefaultValueAttribute("LD Buf1 Grip4 Exist Sen")]
		LDBuf1Grip4ExistSen,
		[DefaultValueAttribute("LD Buf2 Grip1 Exist Sen")]
		LDBuf2Grip1ExistSen,
		[DefaultValueAttribute("LD Buf2 Grip2 Exist Sen")]
		LDBuf2Grip2ExistSen,
		[DefaultValueAttribute("LD Buf2 Grip3 Exist Sen")]
		LDBuf2Grip3ExistSen,
		[DefaultValueAttribute("LD Buf2 Grip4 Exist Sen")]
		LDBuf2Grip4ExistSen,

		[DefaultValueAttribute("LDBuf1TableUp1Cyl")]
		LDBuf1TableUp1Cyl,
		[DefaultValueAttribute("LDBuf1TableDown1Cyl")]
		LDBuf1TableDown1Cyl,
		[DefaultValueAttribute("LDBuf1TableUp2Cyl")]
		LDBuf1TableUp2Cyl,
		[DefaultValueAttribute("LDBuf1TableDown2Cyl")]
		LDBuf1TableDown2Cyl,
		[DefaultValueAttribute("LDBuf1CleanUpCyl")]
		LDBuf1CleanUpCyl,
		[DefaultValueAttribute("LDBuf1CleanDnCyl")]
		LDBuf1CleanDnCyl,
		[DefaultValueAttribute("LDBuf1CleanAirFwdCyl")]
		LDBuf1CleanAirFwdCyl,
		[DefaultValueAttribute("LDBuf1CleanAirBwdCyl")]
		LDBuf1CleanAirBwdCyl,
		[DefaultValueAttribute("LDBuf1DuctUpCyl")]
		LDBuf1DuctUpCyl,
		[DefaultValueAttribute("LDBuf1DuctDnCyl")]
		LDBuf1DuctDnCyl,
		[DefaultValueAttribute("LDBuf1DustOpenCyl")]
		LDBuf1DustOpenCyl,
		[DefaultValueAttribute("LDBuf1DustCloseCyl")]
		LDBuf1DustCloseCyl,
		[DefaultValueAttribute("CH08_None12")]
		CH08_None12,
		[DefaultValueAttribute("LDBuf1CleanFilter")]
		LDBuf1CleanFilter,
		[DefaultValueAttribute("LDBuf1Air")]
		LDBuf1Air,
		[DefaultValueAttribute("CH08_None15")]
		CH08_None15,

		[DefaultValueAttribute("LDBuf2TableUp1Cyl")]
		LDBuf2TableUp1Cyl,
		[DefaultValueAttribute("LDBuf2TableDown1Cyl")]
		LDBuf2TableDown1Cyl,
		[DefaultValueAttribute("LDBuf2TableUp2Cyl")]
		LDBuf2TableUp2Cyl,
		[DefaultValueAttribute("LDBuf2TableDown2Cyl")]
		LDBuf2TableDown2Cyl,
		[DefaultValueAttribute("LDBuf2CleanUpCyl")]
		LDBuf2CleanUpCyl,
		[DefaultValueAttribute("LDBuf2CleanDnCyl")]
		LDBuf2CleanDnCyl,
		[DefaultValueAttribute("LDBuf2CleanAirFwdCyl")]
		LDBuf2CleanAirFwdCyl,
		[DefaultValueAttribute("LDBuf2CleanAirBwdCyl")]
		LDBuf2CleanAirBwdCyl,
		[DefaultValueAttribute("LDBuf2DuctUpCyl")]
		LDBuf2DuctUpCyl,
		[DefaultValueAttribute("LDBuf2DuctDnCyl")]
		LDBuf2DuctDnCyl,
		[DefaultValueAttribute("LDBuf2DustOpenCyl")]
		LDBuf2DustOpenCyl,
		[DefaultValueAttribute("LDBuf2DustCloseCyl")]
		LDBuf2DustCloseCyl,
		[DefaultValueAttribute("CH09_None12")]
		CH09_None12,
		[DefaultValueAttribute("LDBuf2CleanFilter")]
		LDBuf2CleanFilter,
		[DefaultValueAttribute("LDBuf2Air")]
		LDBuf2Air,
		[DefaultValueAttribute("CH09_None15")]
		CH09_None15,


		[DefaultValueAttribute("LD Insp PnP Up Cyl")]
		LDInspPnPUpCyl,
		[DefaultValueAttribute("LD Insp PnP Dn Cyl")]
		LDInspPnPDnCyl,
		[DefaultValueAttribute("LD Insp PnP Vac1")]
		LDInspPnPVac1,
		[DefaultValueAttribute("LD Insp PnP Vac2")]
		LDInspPnPVac2,
		[DefaultValueAttribute("LD Insp PnP Vac3")]
		LDInspPnPVac3,
		[DefaultValueAttribute("LD Insp PnP Vac4")]
		LDInspPnPVac4,
		[DefaultValueAttribute("Insp PnP Collide Sen")]
		InspPnPCollideSen,
		[DefaultValueAttribute("CH10_None07")]
		CH10_None07,
		[DefaultValueAttribute("CH10_None08")]
		CH10_None08,
		[DefaultValueAttribute("CH10_None09")]
		CH10_None09,
		[DefaultValueAttribute("CH10_None10")]
		CH10_None10,
		[DefaultValueAttribute("CH10_None11")]
		CH10_None11,
		[DefaultValueAttribute("CH10_None12")]
		CH10_None12,
		[DefaultValueAttribute("CH10_None13")]
		CH10_None13,
		[DefaultValueAttribute("CH10_None14")]
		CH10_None14,
		[DefaultValueAttribute("CH10_None15")]
		CH10_None15,

		[DefaultValueAttribute("Insp Plate1 Grip Close Cyl")]
		InspPlate1GripCloseCyl,
		[DefaultValueAttribute("Insp Plate1 Exist Sen1")]
		InspPlate1ExistSen1,
		[DefaultValueAttribute("Insp Plate1 Exist Sen2")]
		InspPlate1ExistSen2,
		[DefaultValueAttribute("Insp Plate1 Exist Sen3")]
		InspPlate1ExistSen3,
		[DefaultValueAttribute("Insp Plate1 Exist Sen4")]
		InspPlate1ExistSen4,
		[DefaultValueAttribute("CH11_None05")]
		CH11_None05,
		[DefaultValueAttribute("CH11_None06")]
		CH11_None06,
		[DefaultValueAttribute("CH11_None07")]
		CH11_None07,
		[DefaultValueAttribute("Insp Plate2 Grip Close Cyl")]
		InspPlate2GripCloseCyl,
		[DefaultValueAttribute("Insp Plate2 Exist Sen1")]
		InspPlate2ExistSen1,
		[DefaultValueAttribute("Insp Plate2 Exist Sen2")]
		InspPlate2ExistSen2,
		[DefaultValueAttribute("Insp Plate2 Exist Sen3")]
		InspPlate2ExistSen3,
		[DefaultValueAttribute("Insp Plate2 Exist Sen4")]
		InspPlate2ExistSen4,
		[DefaultValueAttribute("CH11_None13")]
		CH11_None13,
		[DefaultValueAttribute("CH11_None14")]
		CH11_None14,
		[DefaultValueAttribute("CH11_None15")]
		CH11_None15,

		[DefaultValueAttribute("MainAirFlow")]
		MainAirFlow,
		[DefaultValueAttribute("CH12_None01")]
		CH12_None01,
		[DefaultValueAttribute("CH12_None02")]
		CH12_None02,
		[DefaultValueAttribute("CH12_None03")]
		CH12_None03,
		[DefaultValueAttribute("CH12_None04")]
		CH12_None04,
		[DefaultValueAttribute("CH12_None05")]
		CH12_None05,
		[DefaultValueAttribute("CH12_None06")]
		CH12_None06,
		[DefaultValueAttribute("CH12_None07")]
		CH12_None07,
		[DefaultValueAttribute("CH12_None08")]
		CH12_None08,
		[DefaultValueAttribute("CH12_None09")]
		CH12_None09,
		[DefaultValueAttribute("CH12_None10")]
		CH12_None10,
		[DefaultValueAttribute("CH12_None11")]
		CH12_None11,
		[DefaultValueAttribute("CH12_None12")]
		CH12_None12,
		[DefaultValueAttribute("CH12_None13")]
		CH12_None13,
		[DefaultValueAttribute("CH12_None14")]
		CH12_None14,
		[DefaultValueAttribute("CH12_None15")]
		CH12_None15,

		[DefaultValueAttribute("BlowAirFlow")]
		BlowAirFlow,
		[DefaultValueAttribute("CH13_None01")]
		CH13_None01,
		[DefaultValueAttribute("CH13_None02")]
		CH13_None02,
		[DefaultValueAttribute("CH13_None03")]
		CH13_None03,
		[DefaultValueAttribute("CH13_None04")]
		CH13_None04,
		[DefaultValueAttribute("CH13_None05")]
		CH13_None05,
		[DefaultValueAttribute("CH13_None06")]
		CH13_None06,
		[DefaultValueAttribute("CH13_None07")]
		CH13_None07,
		[DefaultValueAttribute("CH13_None08")]
		CH13_None08,
		[DefaultValueAttribute("CH13_None09")]
		CH13_None09,
		[DefaultValueAttribute("CH13_None10")]
		CH13_None10,
		[DefaultValueAttribute("CH13_None11")]
		CH13_None11,
		[DefaultValueAttribute("CH13_None12")]
		CH13_None12,
		[DefaultValueAttribute("CH13_None13")]
		CH13_None13,
		[DefaultValueAttribute("CH13_None14")]
		CH13_None14,
		[DefaultValueAttribute("CH13_None15")]
		CH13_None15,

		[DefaultValueAttribute("LDPnPAirFlow")]
		LDPnPAirFlow,
		[DefaultValueAttribute("CH14_None01")]
		CH14_None01,
		[DefaultValueAttribute("CH14_None02")]
		CH14_None02,
		[DefaultValueAttribute("CH14_None03")]
		CH14_None03,
		[DefaultValueAttribute("CH14_None04")]
		CH14_None04,
		[DefaultValueAttribute("CH14_None05")]
		CH14_None05,
		[DefaultValueAttribute("CH14_None06")]
		CH14_None06,
		[DefaultValueAttribute("CH14_None07")]
		CH14_None07,
		[DefaultValueAttribute("CH14_None08")]
		CH14_None08,
		[DefaultValueAttribute("CH14_None09")]
		CH14_None09,
		[DefaultValueAttribute("CH14_None10")]
		CH14_None10,
		[DefaultValueAttribute("CH14_None11")]
		CH14_None11,
		[DefaultValueAttribute("CH14_None12")]
		CH14_None12,
		[DefaultValueAttribute("CH14_None13")]
		CH14_None13,
		[DefaultValueAttribute("CH14_None14")]
		CH14_None14,
		[DefaultValueAttribute("CH14_None15")]
		CH14_None15,

		[DefaultValueAttribute("LDInspPnPAirFlow")]
		LDInspPnPAirFlow,
		[DefaultValueAttribute("CH15_None01")]
		CH15_None01,
		[DefaultValueAttribute("CH15_None02")]
		CH15_None02,
		[DefaultValueAttribute("CH15_None03")]
		CH15_None03,
		[DefaultValueAttribute("CH15_None04")]
		CH15_None04,
		[DefaultValueAttribute("CH15_None05")]
		CH15_None05,
		[DefaultValueAttribute("CH15_None06")]
		CH15_None06,
		[DefaultValueAttribute("CH15_None07")]
		CH15_None07,
		[DefaultValueAttribute("CH15_None08")]
		CH15_None08,
		[DefaultValueAttribute("CH15_None09")]
		CH15_None09,
		[DefaultValueAttribute("CH15_None10")]
		CH15_None10,
		[DefaultValueAttribute("CH15_None11")]
		CH15_None11,
		[DefaultValueAttribute("CH15_None12")]
		CH15_None12,
		[DefaultValueAttribute("CH15_None13")]
		CH15_None13,
		[DefaultValueAttribute("CH15_None14")]
		CH15_None14,
		[DefaultValueAttribute("CH15_None15")]
		CH15_None15,

		[DefaultValueAttribute("ULDPnPAirFlow")]
		ULDPnPAirFlow,
		[DefaultValueAttribute("CH16_None01")]
		CH16_None01,
		[DefaultValueAttribute("CH16_None02")]
		CH16_None02,
		[DefaultValueAttribute("CH16_None03")]
		CH16_None03,
		[DefaultValueAttribute("CH16_None04")]
		CH16_None04,
		[DefaultValueAttribute("CH16_None05")]
		CH16_None05,
		[DefaultValueAttribute("CH16_None06")]
		CH16_None06,
		[DefaultValueAttribute("CH16_None07")]
		CH16_None07,
		[DefaultValueAttribute("CH16_None08")]
		CH16_None08,
		[DefaultValueAttribute("CH16_None09")]
		CH16_None09,
		[DefaultValueAttribute("CH16_None10")]
		CH16_None10,
		[DefaultValueAttribute("CH16_None11")]
		CH16_None11,
		[DefaultValueAttribute("CH16_None12")]
		CH16_None12,
		[DefaultValueAttribute("CH16_None13")]
		CH16_None13,
		[DefaultValueAttribute("CH16_None14")]
		CH16_None14,
		[DefaultValueAttribute("CH16_None15")]
		CH16_None15,

		[DefaultValueAttribute("ULDInspPnPAirFlow")]
		ULDInspPnPAirFlow,
		[DefaultValueAttribute("CH17_None01")]
		CH17_None01,
		[DefaultValueAttribute("CH17_None02")]
		CH17_None02,
		[DefaultValueAttribute("CH17_None03")]
		CH17_None03,
		[DefaultValueAttribute("CH17_None04")]
		CH17_None04,
		[DefaultValueAttribute("CH17_None05")]
		CH17_None05,
		[DefaultValueAttribute("CH17_None06")]
		CH17_None06,
		[DefaultValueAttribute("CH17_None07")]
		CH17_None07,
		[DefaultValueAttribute("CH17_None08")]
		CH17_None08,
		[DefaultValueAttribute("CH17_None09")]
		CH17_None09,
		[DefaultValueAttribute("CH17_None10")]
		CH17_None10,
		[DefaultValueAttribute("CH17_None11")]
		CH17_None11,
		[DefaultValueAttribute("CH17_None12")]
		CH17_None12,
		[DefaultValueAttribute("CH17_None13")]
		CH17_None13,
		[DefaultValueAttribute("CH17_None14")]
		CH17_None14,
		[DefaultValueAttribute("CH17_None15")]
		CH17_None15,

		[DefaultValueAttribute("CH18_None00")]
		CH18_None00,
		[DefaultValueAttribute("CH18_None01")]
		CH18_None01,
		[DefaultValueAttribute("CH18_None02")]
		CH18_None02,
		[DefaultValueAttribute("CH18_None03")]
		CH18_None03,
		[DefaultValueAttribute("CH18_None04")]
		CH18_None04,
		[DefaultValueAttribute("CH18_None05")]
		CH18_None05,
		[DefaultValueAttribute("CH18_None06")]
		CH18_None06,
		[DefaultValueAttribute("CH18_None07")]
		CH18_None07,
		[DefaultValueAttribute("CH18_None08")]
		CH18_None08,
		[DefaultValueAttribute("CH18_None09")]
		CH18_None09,
		[DefaultValueAttribute("CH18_None10")]
		CH18_None10,
		[DefaultValueAttribute("CH18_None11")]
		CH18_None11,
		[DefaultValueAttribute("CH18_None12")]
		CH18_None12,
		[DefaultValueAttribute("CH18_None13")]
		CH18_None13,
		[DefaultValueAttribute("CH18_None14")]
		CH18_None14,
		[DefaultValueAttribute("CH18_None15")]
		CH18_None15,

		[DefaultValueAttribute("CH19_None00")]
		CH19_None00,
		[DefaultValueAttribute("CH19_None01")]
		CH19_None01,
		[DefaultValueAttribute("CH19_None02")]
		CH19_None02,
		[DefaultValueAttribute("CH19_None03")]
		CH19_None03,
		[DefaultValueAttribute("CH19_None04")]
		CH19_None04,
		[DefaultValueAttribute("CH19_None05")]
		CH19_None05,
		[DefaultValueAttribute("CH19_None06")]
		CH19_None06,
		[DefaultValueAttribute("CH19_None07")]
		CH19_None07,
		[DefaultValueAttribute("CH19_None08")]
		CH19_None08,
		[DefaultValueAttribute("CH19_None09")]
		CH19_None09,
		[DefaultValueAttribute("CH19_None10")]
		CH19_None10,
		[DefaultValueAttribute("CH19_None11")]
		CH19_None11,
		[DefaultValueAttribute("CH19_None12")]
		CH19_None12,
		[DefaultValueAttribute("CH19_None13")]
		CH19_None13,
		[DefaultValueAttribute("CH19_None14")]
		CH19_None14,
		[DefaultValueAttribute("CH19_None15")]
		CH19_None15,

		[DefaultValueAttribute("ULD Tray Lift Up Btn")]
		ULDTrayLiftUpBtn,
		[DefaultValueAttribute("ULD Tray Lift Dn Btn")]
		ULDTrayLiftDnBtn,
		[DefaultValueAttribute("ULD Tray Lift Area Sen1")]
		ULDTrayLiftAreaSen1,
		[DefaultValueAttribute("ULD Tray Lift Area Sen2")]
		ULDTrayLiftAreaSen2,
		[DefaultValueAttribute("ULD Tray Lift Exist Sen")]
		ULDTrayLiftExistSen,
		[DefaultValueAttribute("ULD Tray Lift Dec Sen")]
		ULDTrayLiftDecSen,
		[DefaultValueAttribute("ULD Tray Conv Exist Sen")]
		ULDTrayConvExistSen,
		[DefaultValueAttribute("ULD Conv Stopper Fwd Cyl")]
		ULDConvStopperFwdCyl,
		[DefaultValueAttribute("ULD Conv Stopper Bwd Cyl")]
		ULDConvStopperBwdCyl,
		[DefaultValueAttribute("CH20_None09")]
		CH20_None09,
		[DefaultValueAttribute("CH20_None10")]
		CH20_None10,
		[DefaultValueAttribute("CH20_None11")]
		CH20_None11,
		[DefaultValueAttribute("ULD Tray Trans Exist Sen")]
		ULDTrayTransExistSen,
		[DefaultValueAttribute("ULD Tray Trans Pos Sen")]
		ULDTrayTransPosSen,
		[DefaultValueAttribute("ULD Tray Trans Safety Sen")]
		ULDTrayTransSafetySen,
		[DefaultValueAttribute("Tray Trans Collide Sen")]
		TrayTransCollideSen,

		[DefaultValueAttribute("ULD Tray Table1 Pad Fwd Cyl")]
		ULDTrayTable1PadFwdCyl,
		[DefaultValueAttribute("ULD Tray Table1 Pad Bwd Cyl")]
		ULDTrayTable1PadBwdCyl,
		[DefaultValueAttribute("ULD Tray Table1 Grip Open Cyl")]
		ULDTrayTable1GripOpenCyl,
		[DefaultValueAttribute("ULD Tray Table1 Exist Sen")]
		ULDTrayTable1ExistSen,
		[DefaultValueAttribute("CH21_None04")]
		CH21_None04,
		[DefaultValueAttribute("CH21_None05")]
		CH21_None05,
		[DefaultValueAttribute("CH21_None06")]
		CH21_None06,
		[DefaultValueAttribute("CH21_None07")]
		CH21_None07,
		[DefaultValueAttribute("ULD Tray Table2 Pad Fwd Cyl")]
		ULDTrayTable2PadFwdCyl,
		[DefaultValueAttribute("ULD Tray Table2 Pad Bwd Cyl")]
		ULDTrayTable2PadBwdCyl,
		[DefaultValueAttribute("ULD Tray Table2 Grip Open Cyl")]
		ULDTrayTable2GripOpenCyl,
		[DefaultValueAttribute("ULD Tray Table2 Exist Sen")]
		ULDTrayTable2ExistSen,
		[DefaultValueAttribute("CH21_None12")]
		CH21_None12,
		[DefaultValueAttribute("CH21_None13")]
		CH21_None13,
		[DefaultValueAttribute("CH21_None14")]
		CH21_None14,
		[DefaultValueAttribute("CH21_None15")]
		CH21_None15,

		[DefaultValueAttribute("ULD PnP Up 1 Cyl")]
		ULDPnPUp1Cyl,
		[DefaultValueAttribute("ULD PnP Dn 1 Cyl")]
		ULDPnPDn1Cyl,
		[DefaultValueAttribute("ULD PnP Up 2 Cyl")]
		ULDPnPUp2Cyl,
		[DefaultValueAttribute("ULD PnP Dn 2 Cyl")]
		ULDPnPDn2Cyl,
		[DefaultValueAttribute("ULD PnP Up 3 Cyl")]
		ULDPnPUp3Cyl,
		[DefaultValueAttribute("ULD PnP Dn 3 Cyl")]
		ULDPnPDn3Cyl,
		[DefaultValueAttribute("ULD PnP Up 4 Cyl")]
		ULDPnPUp4Cyl,
		[DefaultValueAttribute("ULD PnP Dn 4 Cyl")]
		ULDPnPDn4Cyl,
		[DefaultValueAttribute("ULD PnP Vac Sen1")]
		ULDPnPVac1,
		[DefaultValueAttribute("ULD PnP Vac Sen2")]
		ULDPnPVac2,
		[DefaultValueAttribute("ULD PnP Vac Sen3")]
		ULDPnPVac3,
		[DefaultValueAttribute("ULD PnP Vac Sen4")]
		ULDPnPVac4,
		[DefaultValueAttribute("CH22_None12")]
		CH22_None12,
		[DefaultValueAttribute("CH22_None13")]
		CH22_None13,
		[DefaultValueAttribute("CH22_None14")]
		CH22_None14,
		[DefaultValueAttribute("CH22_None15")]
		CH22_None15,


		[DefaultValueAttribute("ULD Buf Exist Sen1")]
		ULDBufExistSen1,
		[DefaultValueAttribute("ULD Buf Exist Sen2")]
		ULDBufExistSen2,
		[DefaultValueAttribute("ULD Buf Exist Sen3")]
		ULDBufExistSen3,
		[DefaultValueAttribute("ULD Buf Exist Sen4")]
		ULDBufExistSen4,
		[DefaultValueAttribute("CH23_None04")]
		CH23_None04,
		[DefaultValueAttribute("CH23_None05")]
		CH23_None05,
		[DefaultValueAttribute("CH23_None06")]
		CH23_None06,
		[DefaultValueAttribute("CH23_None07")]
		CH23_None07,
		[DefaultValueAttribute("Reject Tray Grip X Open Cyl")]
		RejectTrayGripXOpenCyl,
		[DefaultValueAttribute("Reject Tray Grip Y Open Cyl")]
		RejectTrayGripYOpenCyl,
		[DefaultValueAttribute("Reject Tray Exist Sen")]
		RejectTrayExistSen,
		[DefaultValueAttribute("Reject Tray Size Sen")]
		RejectTraySizeSen,
		[DefaultValueAttribute("CH23_None12")]
		CH23_None12,
		[DefaultValueAttribute("CH23_None13")]
		CH23_None13,
		[DefaultValueAttribute("CH23_None14")]
		CH23_None14,
        [DefaultValueAttribute("Top Right Rear Door Sen")]
        TopRightRearDoorSen,

        [DefaultValueAttribute("ULD Insp PnP Up Cyl")]
		ULDInspPnPUpCyl,
		[DefaultValueAttribute("ULD Insp PnP Dn Cyl")]
		ULDInspPnPDnCyl,
		[DefaultValueAttribute("ULD Insp PnP Vac1")]
		ULDInspPnPVac1,
		[DefaultValueAttribute("ULD Insp PnP Vac2")]
		ULDInspPnPVac2,
		[DefaultValueAttribute("ULD Insp PnP Vac3")]
		ULDInspPnPVac3,
		[DefaultValueAttribute("ULD Insp PnP Vac4")]
		ULDInspPnPVac4,
		[DefaultValueAttribute("CH24_None06")]
		CH24_None06,
		[DefaultValueAttribute("CH24_None07")]
		CH24_None07,
		[DefaultValueAttribute("CH24_None08")]
		CH24_None08,
		[DefaultValueAttribute("CH24_None09")]
		CH24_None09,
		[DefaultValueAttribute("CH24_None10")]
		CH24_None10,
		[DefaultValueAttribute("CH24_None11")]
		CH24_None11,
		[DefaultValueAttribute("CH24_None12")]
		CH24_None12,
		[DefaultValueAttribute("CH24_None13")]
		CH24_None13,
		[DefaultValueAttribute("CH24_None14")]
		CH24_None14,
		[DefaultValueAttribute("CH24_None15")]
		CH24_None15,


		Total,
	}
}