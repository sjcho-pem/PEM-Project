using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public enum DO
	{

		[DefaultValueAttribute("Lamp Red")]
		LampRed,
		[DefaultValueAttribute("Lamp Yellow")]
		LampYellow,
		[DefaultValueAttribute("Lamp Green")]
		LampGreen,
		[DefaultValueAttribute("Buzzer")]
		Buzzer,
		[DefaultValueAttribute("Start Btn Lamp")]
		StartBtnLamp,
		[DefaultValueAttribute("Stop Btn Lamp")]
		StopBtnLamp,
		[DefaultValueAttribute("CH00_None06")]
		CH00_None06,
		[DefaultValueAttribute("CH00_None07")]
		CH00_None07,
		[DefaultValueAttribute("Reject Lamp Red")]
		RejectLampRed,
		[DefaultValueAttribute("Reject Buzzer")]
		RejectBuzzer,
		[DefaultValueAttribute("Dust Control")]
		DustControl,
        [DefaultValueAttribute("CH00_None11")]
        CH00_None11,
        [DefaultValueAttribute("Top Insp Ready1")]
        TopInspReady1,
        [DefaultValueAttribute("Top Insp Ready2")]
		TopInspReady2,
		[DefaultValueAttribute("Btm Insp Ready1")]
		BtmInspReady1,
		[DefaultValueAttribute("Btm Insp Ready2")]
		BtmInspReady2,


		[DefaultValueAttribute("LDConvStopperCyl")]
		LDConvStopperCyl,
		[DefaultValueAttribute("LDTrayHolderGripCloseCyl")]
		LDTrayHolderGripCloseCyl,
		[DefaultValueAttribute("LDTrayHolderGripOpenCyl")]
		LDTrayHolderGripOpenCyl,
		[DefaultValueAttribute("CH01_None03")]
		CH01_None03,
		[DefaultValueAttribute("CH01_None04")]
		CH01_None04,
		[DefaultValueAttribute("LDTrayTable1PadCyl")]
		LDTrayTable1PadCyl,
		[DefaultValueAttribute("LDTrayTable1GripCyl")]
		LDTrayTable1GripCyl,
		[DefaultValueAttribute("LDTrayTable2PadCyl")]
		LDTrayTable2PadCyl,
		[DefaultValueAttribute("CH01_None08")]
        CH01_None08,
		[DefaultValueAttribute("LDTrayTable2GripCyl")]
		LDTrayTable2GripCyl,
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




        [DefaultValueAttribute("Tray Clean Grip Open Cyl")]
		TrayCleanGripOpenCyl,
		[DefaultValueAttribute("Tray Clean Grip Close Cyl")]
		TrayCleanGripCloseCyl,
		[DefaultValueAttribute("Tray Clean Rotate0 Cyl")]
		TrayCleanRotate0Cyl,
		[DefaultValueAttribute("Tray Clean Rotate180 Cyl")]
		TrayCleanRotate180Cyl,
		[DefaultValueAttribute("Tray Clean Up Cyl")]
		TrayCleanUpCyl,
		[DefaultValueAttribute("Tray Clean Dn Cyl")]
		TrayCleanDnCyl,
		[DefaultValueAttribute("Tray Clean Air Cyl")]
		TrayCleanAirCyl,
		[DefaultValueAttribute("Tray Clean Dust Cyl")]
		TrayCleanDustCyl,
		[DefaultValueAttribute("Tray Clean Air Blow")]
		TrayCleanAirBlow,
		[DefaultValueAttribute("CH02_None09")]
		CH02_None09,
		[DefaultValueAttribute("CH02_None10")]
		CH02_None10,
		[DefaultValueAttribute("CH02_None11")]
		CH02_None11,
		[DefaultValueAttribute("CH02_None12")]
		CH02_None12,
		[DefaultValueAttribute("CH02_None13")]
		CH02_None13,
		[DefaultValueAttribute("CH02_None14")]
		CH02_None14,
		[DefaultValueAttribute("CH02_None15")]
		CH02_None15,

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
		[DefaultValueAttribute("CH03_None12")]
		CH03_None12,
		[DefaultValueAttribute("Smart Camera Trigger")]
		SmartCameraTrigger,
		[DefaultValueAttribute("Smart Camera Fwd")]
		SmartCameraFwd,
		[DefaultValueAttribute("Smart Camera Bwd")]
		SmartCameraBwd,

		[DefaultValueAttribute("LD Buf1 Grip Open Cyl")]
		LDBuf1GripOpenCyl,
		[DefaultValueAttribute("LD Buf1 Grip Close Cyl")]
		LDBuf1GripCloseCyl,
		[DefaultValueAttribute("LD Buf2 Grip Open Cyl")]
		LDBuf2GripOpenCyl,
		[DefaultValueAttribute("LD Buf2 Grip Close Cyl")]
		LDBuf2GripCloseCyl,
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


		[DefaultValueAttribute("LDBuf1TableUpCyl")]
		LDBuf1TableUpCyl,
		[DefaultValueAttribute("LDBuf1TableDownCyl")]
		LDBuf1TableDownCyl,
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
		[DefaultValueAttribute("LDBuf1BlowIonizer")]
		LDBuf1BlowIonizer,
		[DefaultValueAttribute("LDBuf1CleanAirBlow")]
		LDBuf1CleanAirBlow,
		[DefaultValueAttribute("LDBuf1DustCyl")]
		LDBuf1DustCyl,
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

		[DefaultValueAttribute("LDBuf2TableUpCyl")]
		LDBuf2TableUpCyl,
		[DefaultValueAttribute("LDBuf2TableDownCyl")]
		LDBuf2TableDownCyl,
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
		[DefaultValueAttribute("LDBuf2BlowIonizer")]
		LDBuf2BlowIonizer,
		[DefaultValueAttribute("LDBuf2CleanAirBlow")]
		LDBuf2CleanAirBlow,
		[DefaultValueAttribute("LDBuf2DustCyl")]
		LDBuf2DustCyl,
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
		[DefaultValueAttribute("CH07_None06")]
		CH07_None06,
		[DefaultValueAttribute("CH07_None07")]
		CH07_None07,
		[DefaultValueAttribute("CH07_None08")]
		CH07_None08,
		[DefaultValueAttribute("CH07_None09")]
		CH07_None09,
		[DefaultValueAttribute("CH07_None10")]
		CH07_None10,
		[DefaultValueAttribute("CH07_None11")]
		CH07_None11,
		[DefaultValueAttribute("CH07_None12")]
		CH07_None12,
		[DefaultValueAttribute("CH07_None13")]
		CH07_None13,
		[DefaultValueAttribute("CH07_None14")]
		CH07_None14,
		[DefaultValueAttribute("CH07_None15")]
		CH07_None15,

		[DefaultValueAttribute("Insp Plate1 Grip Open Cyl")]
		InspPlate1GripOpenCyl,
		[DefaultValueAttribute("Insp Plate1 Grip Close Cyl")]
		InspPlate1GripCloseCyl,
		[DefaultValueAttribute("CH08_None02")]
		CH08_None02,
		[DefaultValueAttribute("CH08_None03")]
		CH08_None03,
		[DefaultValueAttribute("CH08_None04")]
		CH08_None04,
		[DefaultValueAttribute("CH08_None05")]
		CH08_None05,
		[DefaultValueAttribute("CH08_None06")]
		CH08_None06,
		[DefaultValueAttribute("CH08_None07")]
		CH08_None07,
		[DefaultValueAttribute("Insp Plate2 Grip Open Cyl")]
		InspPlate2GripOpenCyl,
		[DefaultValueAttribute("Insp Plate2 Grip Close Cyl")]
		InspPlate2GripCloseCyl,
		[DefaultValueAttribute("CH08_None10")]
		CH08_None10,
		[DefaultValueAttribute("CH08_None11")]
		CH08_None11,
		[DefaultValueAttribute("CH08_None12")]
		CH08_None12,
		[DefaultValueAttribute("CH08_None13")]
		CH08_None13,
		[DefaultValueAttribute("CH08_None14")]
		CH08_None14,
		[DefaultValueAttribute("CH08_None15")]
		CH08_None15,


		[DefaultValueAttribute("ULD Conv Stopper Cyl")]
		ULDConvStopperCyl,
		[DefaultValueAttribute("ULD Tray Table1 Pad Cyl")]
		ULDTrayTable1PadCyl,
		[DefaultValueAttribute("ULD Tray Table1 Grip Cyl")]
		ULDTrayTable1GripCyl,
		[DefaultValueAttribute("ULD Tray Table1 Air Vib")]
		ULDTrayTable1AirVib,
		[DefaultValueAttribute("CH09_None04")]
		CH09_None04,
		[DefaultValueAttribute("CH09_None05")]
		CH09_None05,
		[DefaultValueAttribute("CH09_None06")]
		CH09_None06,
		[DefaultValueAttribute("CH09_None07")]
		CH09_None07,
		[DefaultValueAttribute("ULD Tray Table2 Pad Cyl")]
		ULDTrayTable2PadCyl,
		[DefaultValueAttribute("ULD Tray Table2 Grip Cyl")]
		ULDTrayTable2GripCyl,
		[DefaultValueAttribute("ULD Tray Table2 Air Vib")]
        ULDTrayTable2AirVib,
		[DefaultValueAttribute("CH09_None11")]
		CH09_None11,
		[DefaultValueAttribute("CH09_None12")]
		CH09_None12,
		[DefaultValueAttribute("CH09_None13")]
		CH09_None13,
		[DefaultValueAttribute("CH09_None14")]
		CH09_None14,
		[DefaultValueAttribute("CH09_None15")]
		CH09_None15,

		[DefaultValueAttribute("ULD PnP Up1 Cyl")]
		ULDPnPUp1Cyl,
		[DefaultValueAttribute("ULD PnP Dn1 Cyl")]
		ULDPnPDn1Cyl,
		[DefaultValueAttribute("ULD PnP Up2 Cyl")]
		ULDPnPUp2Cyl,
		[DefaultValueAttribute("ULD PnP Dn2 Cyl")]
		ULDPnPDn2Cyl,
		[DefaultValueAttribute("ULD PnP Up3 Cyl")]
		ULDPnPUp3Cyl,
		[DefaultValueAttribute("ULD PnP Dn3 Cyl")]
		ULDPnPDn3Cyl,
		[DefaultValueAttribute("ULD PnP Up4 Cyl")]
		ULDPnPUp4Cyl,
		[DefaultValueAttribute("ULD PnP Dn4 Cyl")]
		ULDPnPDn4Cyl,
		[DefaultValueAttribute("ULD PnP Vac1")]
		ULDPnPVac1,
		[DefaultValueAttribute("ULD PnP Vac2")]
		ULDPnPVac2,
		[DefaultValueAttribute("ULD PnP Vac3")]
		ULDPnPVac3,
		[DefaultValueAttribute("ULD PnP Vac4")]
		ULDPnPVac4,
		[DefaultValueAttribute("CH10_None12")]
		CH10_None12,
		[DefaultValueAttribute("CH10_None13")]
		CH10_None13,
		[DefaultValueAttribute("CH10_None14")]
		CH10_None14,
		[DefaultValueAttribute("CH10_None15")]
		CH10_None15,

		[DefaultValueAttribute("Reject Tray Grip X Cyl")]
		RejectTrayGripXCyl,
		[DefaultValueAttribute("Reject Tray Grip Y Cyl")]
		RejectTrayGripYCyl,
		[DefaultValueAttribute("Reject Tray Air Vib")]
		RejectTrayAirVib,
		[DefaultValueAttribute("CH11_None03")]
		CH11_None03,
		[DefaultValueAttribute("CH11_None04")]
		CH11_None04,
		[DefaultValueAttribute("CH11_None05")]
		CH11_None05,
		[DefaultValueAttribute("CH11_None06")]
		CH11_None06,
		[DefaultValueAttribute("CH11_None07")]
		CH11_None07,
		[DefaultValueAttribute("CH11_None08")]
		CH11_None08,
		[DefaultValueAttribute("CH11_None09")]
		CH11_None09,
		[DefaultValueAttribute("CH11_None10")]
		CH11_None10,
		[DefaultValueAttribute("CH11_None11")]
		CH11_None11,
		[DefaultValueAttribute("CH11_None12")]
		CH11_None12,
		[DefaultValueAttribute("CH11_None13")]
		CH11_None13,
		[DefaultValueAttribute("CH11_None14")]
		CH11_None14,
        [DefaultValueAttribute("Top Right Rear Door")]
        TopRightRearDoor,

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

		Total,
	}

}
