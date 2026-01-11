	using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public enum MT
	{
		LDTrayLiftZ = 0,
		LDConv1,
		LDConv2,
		LDConvStopper,
		LDConvWidth,
		LDTrayHolder1Z,
		LDTrayHolder2Z,
		LDTrayTransX,
		LDTrayTransZ,
		LDTrayTransGrip,
		LDPnPX,
		LDPnPY,
		LDPnPZ,
		LDPnPW,
		LDInspPnPX,
		LDInspPnPZ,
		LDBuf1Y,
		LDBuf1Grip,
		LDBuf1R,
		LDBuf2Y,
		LDBuf2Grip,
		LDBuf2R,
		LDTable1Z,
		LDTable2Z,
		LDTable2X,
		LDTable1GripY,
		LDTable2GripY,
		InspPlate1W,
		InspPlate1Y,
		InspPlate1Grip,
		InspPlate1Z,

		ULDTrayLiftZ,
		ULDConv1,
		ULDConv2,
		ULDConvWidth,

		ULDTrayTransX,
		ULDTrayTransZ,
		ULDTrayTransGrip,

		ULDPnPX,
		ULDPnPY,
		ULDPnPZ,
		ULDPnPW,

		ULDInspPnPX,
		ULDInspPnPZ,
		ULDBufY,
		ULDBufGripY,
		ULDBufGripX,

		RejectBufY,

		ULDTable1Z,
		ULDTable2Z,
		ULDTable1GripY,
		ULDTable2GripY,
		InspPlate2W,
		InspPlate2Y,
		InspPlate2Grip,
		InspPlate2Z,

		TOTAL,

		// TOTAL 이후는 미사용 모터
		VirtualTopInspX,
		VirtualBtmInspX,
	}

	public enum VelocityRate
	{
		[DefaultValueAttribute("100%")]
		P100 = 0,
		[DefaultValueAttribute("80%")]
		P80,
		[DefaultValueAttribute("50%")]
		P50,
		[DefaultValueAttribute("30%")]
		P30,
		[DefaultValueAttribute("10%")]
		P10,   
		
		XML_INDEX = 59,
	}

	public enum VELOCITY
	{        
		JOG = 95,
		ACC = 96,
		DEC = 97,
		HomeFind = 98,
		HomeSearch = 99,
	}


	#region - Motor Position Index ( --> 해당 Part Class 에서 선언하여 사용 가능.. )
	public enum LDPickerPos
	{
		READY,
		LDST,
	}

	public enum ULDPickerPos
	{
		READY,
		OKST,
		NGST,
	}
	#endregion


	public class MtInfo
	{
		public MT eMt = MT.TOTAL;
		public int nIdx = 99;
		public int nDist = 0;
		public bool bFlag = false;
		public int nVel = 0;

		public MtInfo()
		{
			Init();
		}        

		public void Init()
		{
			eMt = MT.TOTAL;
			nIdx = 99;
			nDist = 0;
			bFlag = false;
			nVel = 0;
		}
	}

}
