using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace H_MarsSSD
{
	#region [ Shared Memory Define ]
	public static class SHMDEF
	{
		public const int BIT_BUF_NUM = 16;
		public const int USESKIP_BUF_NUM = 2;
		public const int DM_BUF_NUM = 256;
		public const int RECIPE_BUF_NUM = 256;
		public const int MAX_MOTOR_NUM = 60;
		public const int MAX_MOTOR_TABLE_NUM = 100;        
		public const int INOUT_BUF_NUM = 725;        //-- [16->64->725] 725ch로 변경 함. 
		public const int ERRCODE_BUF_NUM = 10;
		public const int DUMP_BUF_SIZE = 10000;
		public const int BUZZER_ROW_SIZE = 800;

		public const uint EXEC_SUCCESSED = 0;
		public const uint ER_NOCONNECT_SHARED_MEM = 0xffffffff;
		public const uint ER_MUTEX_TIME_OVER = 0xfffffff0;
		public const uint ER_SEQ_RUNNING = 0xfffffff1;
		public const uint ER_MMI_RUNNING = 0xfffffff2;

		public const uint ER_QUEUE_FULL = 0xffffff00;
		public const uint ER_QUEUE_EMPTY = 0xffffff01;

		public const uint ER_MOTOR_COUNT_OVER = 0xffffff10;
		public const uint ER_MOTOR_NUM_OVER = 0xffffff11;

		public const uint ER_INPUT_COUNT_OVER = 0xffffff20;
		public const uint ER_OUTPUT_COUNT_OVER = 0xffffff21;
	}
	#endregion

	#region [ Structures ]
	[StructLayoutAttribute(LayoutKind.Sequential)]
	public struct SVSTAT_STRUCT
	{
		/// ALM : 1
		///WARNG : 1
		///CMDRDY : 1
		///SVON : 1
		///PON : 1
		///MLOCK : 1
		///ZPOINT : 1
		///PSET : 1
		///DEN : 1
		///T_LIM : 1
		///L_CMP : 1
		///NEAR_ : 1
		///P_SOT : 1
		///N_SOT : 1
		///INPOS : 1
		///HOME : 1

		public ushort bitvector1;

		private bool alm;
		public bool ALM
		{
			get { return alm; }
			set { alm = value; }
		}

		private bool warng;
		public bool WARNG
		{
			get { return warng; }
			set { warng = value; }
		}

		private bool cmdrdy;
		public bool CMDRDY
		{
			get { return cmdrdy; }
			set { cmdrdy = value; }
		}

		private bool svon;
		public bool SVON
		{
			get { return svon; }
			set { svon = value; }
		}

		private bool rdy;
		public bool Rdy
		{
			get { return rdy; }
			set { rdy = value; }
		}

		private bool mlock;
		public bool MLOCK
		{
			get { return mlock; }
			set { mlock = value; }
		}

		private bool zpoint;
		public bool ZPOINT
		{
			get { return zpoint; }
			set { zpoint = value; }
		}

		private bool pset;
		public bool PSET
		{
			get { return pset; }
			set { pset = value; }
		}

		private bool den;
		public bool DEN
		{
			get { return den; }
			set { den = value; }
		}

		private bool t_lim;
		public bool T_LIM
		{
			get { return t_lim; }
			set { t_lim = value; }
		}

		private bool l_cmp;
		public bool L_CMP
		{
			get { return l_cmp; }
			set { l_cmp = value; }
		}

		private bool near_;
		public bool NEAR_
		{
			get { return near_; }
			set { near_ = value; }
		}

		private bool p_sot;
		public bool P_SOT
		{
			get { return p_sot; }
			set { p_sot = value; }
		}

		private bool n_sot;
		public bool N_SOT
		{
			get { return n_sot; }
			set { n_sot = value; }
		}

		private bool inpos;
		public bool INPOS
		{
			get { return inpos; }
			set { inpos = value; }
		}

		private bool home;
		public bool HOME
		{
			get { return home; }
			set { home = value; }
		}
	}


	[StructLayoutAttribute(LayoutKind.Sequential)]
	public struct SVIO_STRUCT
	{

		///P_OT : 1
		///N_OT : 1
		///DEC : 1
		///PA : 1
		///PB : 1
		///PC : 1
		///EXT1 : 1
		///EXT2 : 1
		///EXT3 : 1
		///BRK : 1
		///NA10 : 1
		///NA11 : 1
		///IO12 : 1
		///IO13 : 1
		///IO14 : 1
		///IO15 : 1

		private bool p_ot;
		public bool P_OT
		{
			get { return p_ot; }
			set { p_ot = value; }
		}

		private bool n_ot;
		public bool N_OT
		{
			get { return n_ot; }
			set { n_ot = value; }
		}

		public bool dec;
		public bool DEC
		{
			get { return dec; }
			set { dec = value; }
		}

		public bool pa;
		public bool PA
		{
			get { return pa; }
			set { pa = value; }
		}

		private bool pb;
		public bool PB
		{
			get { return pb; }
			set { pb = value; }
		}

		private bool pc;
		public bool PC
		{
			get { return pc; }
			set { pc = value; }
		}

		private bool org;
		public bool Org
		{
			get { return org; }
			set { org = value; }
		}

		private bool ext2;
		public bool EXT2
		{
			get { return ext2; }
			set { ext2 = value; }
		}

		private bool ext3;
		public bool EXT3
		{
			get { return ext3; }
			set { ext3 = value; }
		}

		private bool brk;
		public bool BRK
		{
			get { return brk; }
			set { brk = value; }
		}

		private bool na10;
		public bool NA10
		{
			get { return na10; }
			set { na10 = value; }
		}

		private bool na11;
		public bool NA11
		{
			get { return na11; }
			set { na11 = value; }
		}

		private bool io12;
		public bool IO12
		{
			get { return io12; }
			set { io12 = value; }
		}

		private bool io13;
		public bool IO13
		{
			get { return io13; }
			set { io13 = value; }
		}

		private bool io14;
		public bool IO14
		{
			get { return io14; }
			set { io14 = value; }
		}

		private bool io15;
		public bool IO15
		{
			get { return io15; }
			set { io15 = value; }
		}
	}


	[StructLayoutAttribute(LayoutKind.Sequential)]
	public struct STATUS_MOTOR
	{

		/// USHORT->unsigned short
		public SVSTAT_STRUCT svStatus;

		/// USHORT->unsigned short
		public ushort ushAlarm;

		/// USHORT->unsigned short
		public ushort ushCurIndex;

		/// USHORT->unsigned short
		public ushort ushNextIndex;

		/// USHORT->unsigned short
		public SVIO_STRUCT ushIO;

		/// USHORT->unsigned short
		public ushort ushReserved;

		/// INT->int
		public int nCurPos;

		public int nActPos;

		/// UINT->unsigned int
		public uint nReserved1;

		/// UINT->unsigned int
		public uint nReserved2;

		/// UINT->unsigned int
		public uint nReserved3;

		/// UINT->unsigned int
		public uint nReserved4;

		/// UINT->unsigned int
		public uint nReserved5;

		/// UINT->unsigned int
		public uint nReserved6;
	}

	[StructLayoutAttribute(LayoutKind.Sequential)]
	public struct TABLE_MOTOR
	{

		/// INT[100]
		[System.Runtime.InteropServices.MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = SHMDEF.MAX_MOTOR_TABLE_NUM, ArraySubType = UnmanagedType.I4)]
		public int[] nPos;

		/// UINT[100]
		[System.Runtime.InteropServices.MarshalAsAttribute(UnmanagedType.ByValArray, SizeConst = SHMDEF.MAX_MOTOR_TABLE_NUM, ArraySubType = UnmanagedType.U4)]
		public uint[] nSpeed;
	}
	#endregion

	#region [ Shared Memory : imports, variables, function ]
	public class SharedMemory
	{

		#region 클래스 변수들
		private bool bInitComp = false;
		#endregion

		#region 값 전달 변수들
		/// <summary>
		/// 공유 메모리 상용시 값 전달 및 설정을 위한 주요 변수
		/// 외부 클래스에서는 해당 변수를 읽고 쓰도록 한다.
		/// 그리고 해당 클래스를 new한 클래스에서만 업데이트를 진행한다.
		/// </summary>
		public	Dictionary<string, bool>	m_dicIsReady		= new Dictionary<string, bool>();
		public	Dictionary<string, string>	m_dicStep			= new Dictionary<string, string>();
		public	Dictionary<string, int[]>	m_dicInspYield	    = new Dictionary<string, int[]>(); // index 0-OK, 1-NG  //베리파이 전 섹젬 보고 용

		private	Dictionary<string, long>	m_dicSeqTact		= new Dictionary<string, long>();
		private	Dictionary<string, long>	m_dicSeqLastTact	= new Dictionary<string, long>();
		private	Dictionary<Seq, Dictionary<int, SenPos>>	m_dicActPos		= new Dictionary<Seq, Dictionary<int, SenPos>>();
		private	Dictionary<SeqID, string>	m_dicSeqID			= new Dictionary<SeqID, string>();
        public  Dictionary<string,string>   m_dicLotPickTime     = new Dictionary<string, string>();

		private ushort[]		shBit		= new ushort[SHMDEF.BIT_BUF_NUM];  // 특정 동작을 하게 하는 값
		private ushort[]		shUseSkip	= new ushort[SHMDEF.USESKIP_BUF_NUM]; // 스킵을 가능하게 하는 값
		private int[]			nDM			= new int[SHMDEF.DM_BUF_NUM];
        private string[]		strRecipe	= new string[SHMDEF.RECIPE_BUF_NUM];
		private int[]			iRecipe		= new int[SHMDEF.RECIPE_BUF_NUM];
		private double[]		dDM			= new double[SHMDEF.DM_BUF_NUM];
		private STATUS_MOTOR[]	stMt		= new STATUS_MOTOR[SHMDEF.MAX_MOTOR_NUM];
		private TABLE_MOTOR[]	stMtTable	= new TABLE_MOTOR[SHMDEF.MAX_MOTOR_NUM];
		
		private ushort[]		shInput		= new ushort[SHMDEF.INOUT_BUF_NUM];
		private ushort[]		shOutput	= new ushort[SHMDEF.INOUT_BUF_NUM];
		private DO				uiOutput	= DO.Total;
		private DI				uiInput		= DI.Total;
		private int[]			iErrCode	= new int[SHMDEF.ERRCODE_BUF_NUM];        
		private int[]			iBuzzerDump	= new int[SHMDEF.DUMP_BUF_SIZE];

		private MANUAL			emNum	= MANUAL.NotManual;
		private BUTTON			ebtnNum = BUTTON.NotButton;
		private MtInfo			curMt	= new MtInfo();

		private object[]		m_manualParams;
		private	object[]		m_buttonParams;
		#endregion

		#region 초기화 관련 함수들
		public bool Initialization()
		{
			try
			{
				InitObject();
				InitSeqID();
				bInitComp = true;
				InitUPH();
				StateTimeLoadFromDat();
				LotYieldDataOpen(Program.DataDirectory + "LotYield.dat");
				RunLotInfoDataOpen(Program.DataDirectory + "RunLotInfo.dat");
				XmlModelUpdate();
			}
			catch (Exception)
			{
				bInitComp = false;
			}
			
			return bInitComp;
		}

		private void InitObject()
		{
			int i = 0;

			for (i = 0; i < SHMDEF.MAX_MOTOR_NUM; i++)
			{
				stMtTable[i].nPos = new int[SHMDEF.MAX_MOTOR_TABLE_NUM];
				stMtTable[i].nSpeed = new uint[SHMDEF.MAX_MOTOR_TABLE_NUM];
			}
		}

		private void InitSeqID()
		{
			m_dicSeqID.Clear();

			for (int i = 0; i < (int)SeqID.Max; i++)
			{
				m_dicSeqID.Add((SeqID)i, EnumHelper.GetDescription((SeqID)i));
			}
		}

		public void SeqInitializ()
		{
			G.InitControl();
			
		}

		public void SeqDipose()
		{
			G.CtrlDispose();
		}
		#endregion

		#region BIT 관련 함수들

		// Only Seq 사용
		public void SetBitData(BIT bit, bool bOn)
		{
			if (!bInitComp) return;
			if (bit == BIT.InspLock)
				GLog.Add(string.Format("BIT! {0} = {1}", bit.ToString(), bOn.ToString()));

			int ch = (int)bit / SHMDEF.BIT_BUF_NUM;
			int nBit = (int)bit % SHMDEF.BIT_BUF_NUM;

			if (!bOn)
			{
				shBit[ch] &= Convert.ToUInt16((~(0x0001 << nBit)) & 0xFFFF);
			}
			else
			{
				shBit[ch] |= Convert.ToUInt16(0x0001 << nBit);
			}
		}

		// Only Seq 사용
		public bool IsBitData(BIT bit)
		{
			if (!bInitComp) return false;

			int ch = (int)bit / SHMDEF.BIT_BUF_NUM;
			int nBit = (int)bit % SHMDEF.BIT_BUF_NUM;

			if (((shBit[ch] >> nBit) & 0x0001) == 0x0001)
				return true;
			else
				return false;
		}

		// Only UI 사용
		public bool SetBitData(uint uiCh, ushort ushData)
		{
			if (!bInitComp) return false;

			if (uiCh >= SHMDEF.BIT_BUF_NUM) return false;

			shBit[uiCh] = (ushort)ushData;

			return true;
		}

		// Only UI 사용 (UI 업데이트용)
		public bool GetBitData(ref ushort[] ushbuf)
		{
			if (!bInitComp) return false;

			if (ushbuf.Length > SHMDEF.BIT_BUF_NUM)
				return false;

			for (int i = 0; i < ushbuf.Length; i++)
				ushbuf[i] = (ushort)shBit[i];

			return true;
		}

		#endregion

		#region USE/SKIP 관련 함수들

		public bool SetUseSkipData(USESKIP uiCh, bool ushData)
		{
			int num = (int)uiCh;
			if (!bInitComp) return false;

			int buffnum = 0;
			int selectNum = num;

			buffnum = (int)(num / 16);
			selectNum = num - 16 * buffnum;

			//if (uiCh >= SHMDEF.USESKIP_BUF_NUM) return false;
			if(ushData)
				shUseSkip[buffnum] |= Convert.ToUInt16(0x0001 << (int)selectNum);
			else
				shUseSkip[buffnum] &= Convert.ToUInt16((~(0x0001 << (int)selectNum)) & 0xFFFF);

			return true;
		}

		public bool SetUseSkipData(ushort[] ushBuf)
		{
			if (!bInitComp) return false;

			ushort[] shBuf = new ushort[ushBuf.Length];
			for (int i = 0; i < ushBuf.Length; i++)
				shBuf[i] = ushBuf[i];

			Array.Copy(shBuf, shUseSkip, shBuf.Length);
			return true;
		}

		public bool GetUseSkipData(USESKIP uiCh)
		{
			int num = (int)uiCh;
			if (!bInitComp) return false;

			int buffnum = 0;
			int selectNum = num;

			buffnum = (int)(num / 16);
			selectNum = num - 16 * buffnum;

			//if (uiCh >= SHMDEF.USESKIP_BUF_NUM)
			//    uiCh = SHMDEF.USESKIP_BUF_NUM - 1;

			if (((shUseSkip[buffnum] >> (int)selectNum) & 0x0001) == 0x0001)
			{
				return true;
			}
			else
				return false;
		}

		public bool GetUseSkipData(ref ushort[] ushbuf)
		{
			if (!bInitComp) return false;

			if (ushbuf.Length > SHMDEF.USESKIP_BUF_NUM)
				return false;

			for (int i = 0; i < ushbuf.Length; i++)
				ushbuf[i] = (ushort)shUseSkip[i];

			return true;
		}

		//public bool ReadSHM_UseSkip()
		//{
		//    if (!bInitComp) return false;

		//    IntPtr pAddr = Marshal.AllocHGlobal(SHMDEF.USESKIP_BUF_NUM * sizeof(ushort));

		//    if (GetUseSkip(pAddr) != SHMDEF.EXEC_SUCCESSED)
		//    {
		//        Marshal.FreeHGlobal(pAddr);
		//        return false;
		//    }

		//    Marshal.Copy(pAddr, shUseSkip, 0, shUseSkip.Length);

		//    Marshal.FreeHGlobal(pAddr);

		//    return true;
		//}

		//public bool WriteSHM_UseSkip()
		//{
		//    //if (!bInitComp) return false;

		//    //IntPtr pAddr = Marshal.AllocHGlobal(SHMDEF.USESKIP_BUF_NUM * sizeof(ushort));

		//    //Marshal.Copy(shUseSkip, 0, pAddr, shUseSkip.Length);

		//    //if (SetUseSkip(pAddr) != SHMDEF.EXEC_SUCCESSED)
		//    //{
		//    //    Marshal.FreeHGlobal(pAddr);
		//    //    return false;
		//    //}

		//    //Marshal.FreeHGlobal(pAddr);

		//    return true;
		//}

		#endregion

		#region DM 관련 함수들
		
		// Only NDM 사용(NDM)
		public bool SetNDMData(int[] iBuf)
		{
			if (!bInitComp) return false;

			Array.Copy(iBuf, nDM, iBuf.Length);
			return true;
		}

		// Only DDM 사용(DDM)
		public bool SetDDMData(double[] dBuf)
		{
			if (!bInitComp) return false;

			Array.Copy(dBuf, dDM, dBuf.Length);
			return true;
		}

		//// NDM
		//public int GetNDMData(uint uiNum)
		//{
		//    if (!bInitComp) return 0;

		//    if (uiNum >= SHMDEF.DM_BUF_NUM)
		//        uiNum = SHMDEF.DM_BUF_NUM - 1;

		//    return nDM[uiNum];
		//}
		//// NDM
		//public int GetNDMData(eNDM uiNum)
		//{
		//    int n = (int)uiNum;
		//    if (!bInitComp) return 0;

		//    if (n >= SHMDEF.DM_BUF_NUM)
		//        n = SHMDEF.DM_BUF_NUM - 1;

		//    return nDM[n];
		//}

		//// DDM
		//public double GetDDMData(uint uiNum)
		//{
		//    if (!bInitComp) return 0;

		//    if (uiNum >= SHMDEF.DM_BUF_NUM)
		//        uiNum = SHMDEF.DM_BUF_NUM - 1;

		//    return dDM[uiNum];
		//}
		//// DDM
		//public double GetDDMData(eDDM uiNum)
		//{
		//    int n = (int)uiNum;
		//    if (!bInitComp) return 0;

		//    if (n >= SHMDEF.DM_BUF_NUM)
		//        n = SHMDEF.DM_BUF_NUM - 1;

		//    return dDM[n];
		//}

		// Only UI 사용(NDM)
		public bool SetNDMData(uint uiNum, int iData)
		{
			if (!bInitComp) return false;

			if (uiNum >= SHMDEF.DM_BUF_NUM) return false;

			nDM[uiNum] = iData;            

			return true;
		}

		// Only UI 사용(DDM)
		public bool SetDDMData(uint uiNum, double dData)
		{
			if (!bInitComp) return false;

			if (uiNum >= SHMDEF.DM_BUF_NUM) return false;

			dDM[uiNum] = dData;

			return true;
		}

		// Only UI 사용(NDM)
		public bool GetNDMData(ref int[] ibuf)
		{
			if (!bInitComp) return false;

			if (ibuf.Length > SHMDEF.DM_BUF_NUM)
				return false;

			for (int i = 0; i < ibuf.Length; i++)
				ibuf[i] = nDM[i];

			return true;
		}

		// Only UI 사용(DDM)
		public bool GetDDMData(ref double[] dbuf)
		{
			if (!bInitComp) return false;

			if (dbuf.Length > SHMDEF.DM_BUF_NUM)
				return false;

			for (int i = 0; i < dbuf.Length; i++)
				dbuf[i] = dDM[i];

			return true;
		}

		// Only UI 사용(SDM)
		public bool GetSDMData(ref string[] sbuf)
		{
			if (!bInitComp) return false;

			if (sbuf.Length > SHMDEF.DM_BUF_NUM)
				return false;

			for (int i = 0; i < sbuf.Length; i++)
				sbuf[i] = SDm.Read((eSDM)i);

			return true;
		}
		#endregion

		#region RECIPE(DEVICE) 관련 함수들
		/// <summary>
		/// Only Seq 사용
		/// </summary>
		/// <param name="num"></param>
		/// <returns></returns>
		/// 
        public string GetRecipeData(DEVICE num)
        {
            return strRecipe[(int)num];
        }

		public int GetRecipeDataToInt(DEVICE num)
		{
            double dTemp = 0;
            double.TryParse(strRecipe[(int)num],out dTemp);
            //dTemp = dTemp * 1000;
            return (int)dTemp;
		}
		public string GetRecipeData(int nRecipeNo, DEVICE num)
        {
			if (nRecipeNo < 0  ||  nRecipeNo > 255)			return null;

			XML	xml_recipe	= UI.SubpageRecipe.GetXMLRecipData(nRecipeNo);

			if (xml_recipe == null)			return null;

            return xml_recipe.ReadDataTable((int)num, "RecipeDataValue");
        }


		/// <summary>
		///  Module Type에 Module과 동일한 레시피를 찾고 해당 Device정보를 리턴해주는 함수
		/// </summary>
		/// <param name="Module"></param>
		/// <param name="num"></param>
		/// <returns></returns>
		public string GetRecipeData(string Module, DEVICE num)
		{
		
			int nRecipe = -1;
			for (int i = 0; i < Define.MAX_RECIPE_NUM; i++)
			{
				if (SM.SHM.GetRecipeData(i, DEVICE.ModuleType) == Module)
				{
					nRecipe = i;
					break;
				}
			}
			if (nRecipe == -1)
			{
				return null;
			}

			XML	xml_recipe	= UI.SubpageRecipe.GetXMLRecipData(nRecipe);
			return  xml_recipe.ReadDataTable((int)num, "RecipeDataValue");
		}
		
		public int GetRecipeDataToInt(string Module, DEVICE num)
		{
		
			int nRecipe = -1;
			for (int i = 0; i < Define.MAX_RECIPE_NUM; i++)
			{
				if (SM.SHM.GetRecipeData(i, DEVICE.ModuleType) == Module)
				{
					nRecipe = i;
					break;
				}
			}
			if (nRecipe == -1)
			{
				return -1;
			}

			XML	xml_recipe	= UI.SubpageRecipe.GetXMLRecipData(nRecipe);
            return int.Parse(xml_recipe.ReadDataTable((int)num, "RecipeDataValue"));
		}

		public int GetRecipeDataToInt(int nRecipeNo, DEVICE num)
        {
			if (nRecipeNo < 0  ||  nRecipeNo > 255)			return 0;

			XML	xml_recipe	= UI.SubpageRecipe.GetXMLRecipData(nRecipeNo);

			if (xml_recipe == null)			return 0;

            return int.Parse(xml_recipe.ReadDataTable((int)num, "RecipeDataValue"));
        }

		/// <summary>
		/// Only UI 사용
		/// </summary>
		/// <param name="uiNum"></param>
		/// <param name="iData"></param>
		/// <returns></returns>
        public bool SetRecipeData(uint uiNum, string strData)
        {
            if (!bInitComp) return false;

            if (uiNum >= SHMDEF.RECIPE_BUF_NUM) return false;

            strRecipe[uiNum] = strData;

            return true;
        }
        
        public bool SetRecipeData(uint uiNum, int nData)
        {
            if (!bInitComp) return false;

            if (uiNum >= SHMDEF.RECIPE_BUF_NUM) return false;

            strRecipe[uiNum] = nData.ToString();

            return true;
        }
		/// <summary>
		/// Only UI 사용
		/// </summary>
		/// <param name="iBuf"></param>
		/// <returns></returns>
		public bool SetRecipeData(string[] iBuf)
		{
			if (!bInitComp) return false;

			Array.Copy(iBuf, strRecipe, iBuf.Length);
			return true;
		}

		/// <summary>
		/// Only Seq 사용
		/// </summary>
		/// <param name="uiNum"></param>
		/// <returns></returns>
		public string GetRecipeData(uint uiNum)
		{
			if (!bInitComp)		return "";

			if (uiNum >= SHMDEF.RECIPE_BUF_NUM)
				uiNum = SHMDEF.RECIPE_BUF_NUM - 1;

			return strRecipe[uiNum];
		}

		#endregion

		#region MOTOR STATUS 관련 함수들

		/// <summary>
		/// Only UI 사용
		/// </summary>
		/// <param name="nMtNum"></param>
		/// <param name="pStatus"></param>
		/// <returns></returns>
		public bool GetMotorSatusData(uint nMtNum, ref STATUS_MOTOR pStatus)
		{
			if (!bInitComp) return false;

			if (nMtNum > (uint)MT.TOTAL)
				return false;

			
			pStatus = stMt[nMtNum];

			return true;
		}
		
		/// <summary>
		/// Only UI 사용
		/// </summary>
		/// <param name="nMtNum"></param>
		/// <param name="pStatus"></param>
		/// <returns></returns>
		public int GetMotorLocalPos(MT nMtNum)
		{
			if (!bInitComp) 
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION);
				return -1;
			}

			if (nMtNum > MT.TOTAL)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION);
				return -1;
			}

			return stMt[(uint)nMtNum].nCurPos;
		}
		#endregion

		#region MOTOR TABLE 관련 함수들

		/// <summary>
		/// Only UI 사용
		/// </summary>
		/// <param name="nMtNum"></param>
		/// <param name="pTable"></param>
		/// <returns></returns>
		public bool GetMotorTableData(uint nMtNum, ref TABLE_MOTOR pTable)
		{
			if (!bInitComp) return false;

			if (nMtNum > SHMDEF.MAX_MOTOR_NUM)
				return false;

			pTable = stMtTable[nMtNum];

			return true;
		}

		/// <summary>
		/// Only UI 사용
		/// </summary>
		/// <param name="nMtNum"></param>
		/// <param name="stTable"></param>
		/// <returns></returns>
		public bool SetMotorTableData(uint nMtNum, TABLE_MOTOR stTable)
		{
			if (!bInitComp) return false;

			if (nMtNum > SHMDEF.MAX_MOTOR_NUM)
				return false;

			stMtTable[nMtNum] = stTable;

			UI.PageMainDisp.wpfMainDisp.MotorDispInit(nMtNum);

			return true;
		}

		public int GetMotorPos(MT nMtNum ,int eNum)
		{
			return stMtTable[(uint)nMtNum].nPos[(uint)eNum];
		}

		public int GetMotorVel(MT nMtNum, int eNum)
		{
			return (int)stMtTable[(uint)nMtNum].nSpeed[(uint)eNum];
		}

		/// <summary>
		/// Seq Only 사용
		/// </summary>
		/// <param name="nMtNum"></param>
		/// <param name="stMtState"></param>
		/// <param name="bRdy"></param>
		public void SetMotorState(uint nMtNum, MotorStatus stMtState, bool bRdy)
		{
			stMt[nMtNum].ushAlarm = (ushort)stMtState.AlarmCode;
			stMt[nMtNum].svStatus.HOME = stMtState.HomeDone;
			stMt[nMtNum].svStatus.ALM = stMtState.Alarm;
			stMt[nMtNum].svStatus.Rdy = bRdy;
			stMt[nMtNum].svStatus.SVON = stMtState.ServoOn;
			stMt[nMtNum].svStatus.PSET = stMtState.PosSet;
			stMt[nMtNum].svStatus.DEN = stMtState.CmdDistributionEnd;
			stMt[nMtNum].svStatus.INPOS = stMtState.Inpos;
			stMt[nMtNum].ushIO.P_OT = stMtState.PosLSen;
			stMt[nMtNum].ushIO.N_OT = stMtState.NegLSen;
			stMt[nMtNum].ushIO.Org = stMtState.HomeSen;
			stMt[nMtNum].ushCurIndex = (ushort)stMtState.CurIndex;
			stMt[nMtNum].nCurPos = stMtState.CmdPos;
			stMt[nMtNum].nActPos = stMtState.ActPos;
		}

		#endregion

		#region INPUT 관련 함수들
		/// <summary>
		/// Only UI Use
		/// </summary>
		/// <param name="nInNum"></param>
		/// <param name="ushIn"></param>
		/// <returns></returns>
		public bool GetInputData(uint nInNum, ref ushort ushIn)
		{
			if (!bInitComp) return false;

			if (nInNum >= SHMDEF.INOUT_BUF_NUM)
				return false;

			ushIn = shInput[nInNum];

			return true;
		}
		/// <summary>
		/// Only UI Use
		/// </summary>
		/// <param name="nInNum"></param>
		/// <param name="ushIn"></param>
		/// <returns></returns>
		public bool GetInputData(DI eInNum)
		{
			if (!bInitComp)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION);
				return false;
			}

			uint nInNum = (uint)eInNum;
			if (nInNum >= SHMDEF.INOUT_BUF_NUM)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION);
				return false;
			}
		
			int channelIndex = (int)eInNum / 16; //채널별 16개의 input
			int bitIndex = (int)eInNum % 16;

			ushort channelValue = shInput[channelIndex];

			return (channelValue & (1 << bitIndex)) != 0;
			//return Convert.ToBoolean(shInput[nInNum]);
		}
		/// <summary>
		/// Only UI Use
		/// </summary>
		/// <param name="nInNum"></param>
		/// <param name="ushIn"></param>
		/// <returns></returns>
		public bool GetInputData(DI eInNum, ref bool bIn)
		{
			if (!bInitComp) return false;

			uint nInNum = (uint)eInNum;
			if (nInNum >= SHMDEF.INOUT_BUF_NUM)
				return false;

			int channelIndex = (int)eInNum / 16; //채널별 16개의 input
			int bitIndex = (int)eInNum % 16;

			ushort channelValue = shInput[channelIndex];

			bIn = (channelValue & (1 << bitIndex)) != 0;
			
			return true;
		}
		/// <summary>
		/// Only UI Use
		/// </summary>
		/// <param name="ushInbuf"></param>
		/// <returns></returns>
		public bool GetInputData(ref ushort[] ushInbuf)
		{
			if (!bInitComp) return false;

			if (ushInbuf.Length > SHMDEF.INOUT_BUF_NUM)
				return false;

			Array.Copy(shInput, ushInbuf, ushInbuf.Length);

			return true;
		}
		/// <summary>
		/// Only Seq Use
		/// </summary>
		/// <param name="shInbuff"></param>
		/// <returns></returns>
		public bool SetInputData(ushort[] shInbuff)
		{
			if (!bInitComp) return false;

			if (shInbuff.Length > SHMDEF.INOUT_BUF_NUM)
				return false;

			Array.Copy(shInbuff, shInput, shInbuff.Length);              

			return true;
		}

		/// <summary>
		/// 시뮬레이터용. UI에서 Input 출력 사용
		/// </summary>
		/// <param name="eIn"></param>
		public void SetInputData(DI eIn)
		{
			uiInput = eIn;
		}
		/// <summary>
		/// 시뮬레이터용. SEQ에서 Input 출력 체크 함수
		/// </summary>
		/// <param name="eOut"></param>
		/// <returns></returns>
		public bool IsUiInputChange(ref DI eOut)
		{
			if (uiInput != DI.Total)
			{
				eOut = uiInput;
				uiInput = DI.Total;
				return true;
			}

			return false;
		}
		#endregion

		#region OUTPUT 관련 함수들
		/// <summary>
		/// Only UI Use
		/// </summary>
		/// <param name="nOutNum"></param>
		/// <param name="ushOut"></param>
		/// <returns></returns>
		public bool GetOutputData(uint nOutNum, ref ushort ushOut)
		{
			if (!bInitComp) return false;

			if (nOutNum >= SHMDEF.INOUT_BUF_NUM)
				return false;

			ushOut = shOutput[nOutNum];

			return true;
		}
		/// <summary>
		/// Only UI Use
		/// </summary>
		/// <param name="nOutNum"></param>
		/// <returns></returns>
		public bool GetOutputData(DO eOutNum)
		{
			if (!bInitComp)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION);
				return false;
			}

			uint nOutNum = (uint)eOutNum;
			if (nOutNum >= SHMDEF.INOUT_BUF_NUM)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION);
				return false;
			}

			return Convert.ToBoolean(shOutput[nOutNum]);
		}
		/// <summary>
		/// Only UI Use
		/// </summary>
		/// <param name="ushOutbuf"></param>
		/// <returns></returns>
		public bool GetOutputData(ref ushort[] ushOutbuf)
		{
			if (!bInitComp) return false;

			if (ushOutbuf.Length > SHMDEF.INOUT_BUF_NUM)
				return false;

			Array.Copy(shOutput, ushOutbuf, ushOutbuf.Length);

			return true;
		}        
		/// <summary>
		/// Only UI Use
		/// </summary>
		/// <param name="nOutNum"></param>
		/// <param name="ushOut"></param>
		/// <returns></returns>
		public bool SetOutputData(uint nOutNum, ushort ushOut)
		{
			if (!bInitComp) return false;
			
			if (nOutNum >= SHMDEF.INOUT_BUF_NUM)
				return false;

			shOutput[nOutNum] = ushOut;

			return true;
		}
		/// <summary>
		/// Only Seq Use
		/// </summary>
		/// <param name="ushOutbuf"></param>
		/// <returns></returns>
		public bool SetOutputData(ushort[] ushOutbuf)
		{
			if (!bInitComp) return false;

			if (ushOutbuf.Length > SHMDEF.INOUT_BUF_NUM)
				return false;

			Array.Copy(ushOutbuf, shOutput, ushOutbuf.Length);                   

			return true;
		}
		/// <summary>
		/// Only UI Use
		/// </summary>
		/// <param name="eOut"></param>
		public void SetOutputData(DO eOut)
		{
			uiOutput = eOut;
		}
		/// <summary>
		/// Output Checking Function
		/// </summary>
		/// <param name="eOut"></param>
		/// <returns></returns>
		public bool IsUiOutputChange(ref DO eOut)
		{
			if (uiOutput != DO.Total)
			{
				eOut = uiOutput;
				uiOutput = DO.Total;
				return true;
			}

			return false;
		}

		#endregion

		#region ERROR CODE 관련 함수들

		public bool GetErrCodeData(ref uint[] uibuf)
		{
			if (!bInitComp) return false;

			if (uibuf.Length > SHMDEF.ERRCODE_BUF_NUM)
				return false;

			for (int i = 0; i < uibuf.Length; i++)
				uibuf[i] = (uint)iErrCode[i];

			return true;
		}

		public void SetErrCodeData(int nIdx, int nCode)
		{
			iErrCode[nIdx] = nCode;
		}

		public int GetErrorCode()
		{
			if (!bInitComp)				return -1;
			if (iErrCode.Count() < 0)	return -1;
			
			return iErrCode[0];
		}

		public void ErrorInit()
		{
			iErrCode.Initialize();
			iErrCode = new int[SHMDEF.ERRCODE_BUF_NUM];
		}
		#endregion

		#region Dump 관련 함수들

		#region -Buzzer Function
		public void SetBuzzerData(int idx, int value)
		{
			iBuzzerDump[idx] = value;            
		}
		public int[] GetBuzzerData()
		{
			return iBuzzerDump;
		}

		#endregion


		#endregion

		#region Manual 관련 함수들
		public void StartManual() { G.seqMnl.Start(); }
		public void StopManual() { G.seqMnl.Stop(); }
		public bool IsManualRunning() { return G.seqMnl.IsRunning(); }
		public void SetManualNum(MANUAL No)
		{
			emNum = No;
		}
		public void SetManualParams(params object[] Param)
		{
			m_manualParams	= Param;
		}

		public void ClearManualNum()
		{
			emNum = MANUAL.NotManual;
		}
		public MANUAL GetManualNum()
		{
			return emNum;
		}
		public object GetManualParam(int nIndex)
		{
			if (m_manualParams == null)			return null;

			return m_manualParams[nIndex];
		}
		public string GetCurStep(SeqID SeqID)
		{
			if (m_dicStep.ContainsKey(m_dicSeqID[SeqID]) == false)			return "-";
			else															return	m_dicStep[m_dicSeqID[SeqID]];
		}
		public Color GetReadyState(SeqID SeqID)
		{
			Color StateColor = Color.Yellow;

			if (m_dicIsReady.ContainsKey(m_dicSeqID[SeqID]) == true  &&  m_dicIsReady[m_dicSeqID[SeqID]] == true)			StateColor = Color.White;

			return StateColor;
		}
		#endregion

		#region Button SEQ 관련 함수들
		public void SetButtonNum(BUTTON No)
		{
			ebtnNum = No;
		}
		public void SetButtonParams(params object[] Param)
		{
			m_buttonParams	= Param;
		}
		public object[] GetButtonParams()
		{
			return m_buttonParams;
		}
		public object GetButtonParam(int nIndex)
		{
			return m_buttonParams[nIndex];
		}
		public void SetButtonNum(BUTTON No, MtInfo mtInfo)
		{
			ebtnNum = No;
			curMt = mtInfo;
		}
		/// <summary>
		/// Motor Jog,Step,IndexMove 사용
		/// </summary>
		/// <returns></returns>
		public MtInfo GetCurMT()
		{            
			return curMt;
		}
   
		public void ClearButtonNum()
		{
			ebtnNum = BUTTON.NotButton;
		}
		public BUTTON GetButtonNum()
		{
			return ebtnNum;
		}
		#endregion

		#region Sequence 관련 함수
		public	void	SetSeqStatus<T>(List<SMM<T>> lstSMM)
		{
			try
			{
				string strkey	= "";
				for (int i = 0; i < lstSMM.Count; i++)
				{
					strkey	= lstSMM[i].AppendLog;

					if (m_dicStep.ContainsKey(strkey) == false)				m_dicStep.Add(strkey, lstSMM[i].CurStep.ToString());
					else													m_dicStep[strkey] = lstSMM[i].CurStep.ToString();

					if (m_dicIsReady.ContainsKey(strkey) == false)			m_dicIsReady.Add(strkey, lstSMM[i].IsReady);
					else													m_dicIsReady[strkey] = lstSMM[i].IsReady;

					if (m_dicSeqTact.ContainsKey(strkey) == false)			m_dicSeqTact.Add(strkey, lstSMM[i].Tact);
					else													m_dicSeqTact[strkey] = lstSMM[i].Tact;

					if (m_dicSeqLastTact.ContainsKey(strkey) == false)		m_dicSeqLastTact.Add(strkey, lstSMM[i].LastTact);
					else													m_dicSeqLastTact[strkey] = lstSMM[i].LastTact;
				}
			}
			catch
			{

			}
		}

		public	void	SetSeqTact(string strKey, long lTact)
		{
			try
			{
				if (m_dicSeqTact.ContainsKey(strKey) == false)			m_dicSeqTact.Add(strKey, lTact);
				else													m_dicSeqTact[strKey]	= lTact;
			}
			catch
			{
			}
		}

		public	void	SetSeqLastTact(string strKey, long lTact)
		{
			try
			{
				if (m_dicSeqLastTact.ContainsKey(strKey) == false)			m_dicSeqLastTact.Add(strKey, lTact);
				else														m_dicSeqLastTact[strKey]	= lTact;
			}
			catch
			{
			}
		}

		public	long	GetSeqTact(string strKey)
		{
			try
			{
				if (m_dicSeqTact.ContainsKey(strKey) == false)			return 0;
				else													return m_dicSeqTact[strKey];
			}
			catch
			{
				return 0;
			}
		}

		public	long	GetSeqLastTact(string strKey)
		{
			try
			{
				if (m_dicSeqLastTact.ContainsKey(strKey) == false)			return 0;
				else														return m_dicSeqLastTact[strKey];
			}
			catch
			{
				return 0;
			}
		}
		#endregion
		
		#region Actuator 관련 함수
	//	public	void	SetActPos<T>(teSeq Seq, string[] stract, EnumArray<T, Actuator> arrAct)
	//	{
	//		string	strkey	= "";
	//		for (int i = 0; i < arrAct.Count; i++)
	//        {
	//			strkey	= string.Format("{0}{1}", Seq, stract[i]);
	//            if (m_dicActPos.ContainsKey(strkey) == false)			m_dicActPos.Add(strkey, arrAct[i].GetPos());
	//            else													m_dicActPos[strkey] = arrAct[i].GetPos();
	//        }
	//	}

		public	void	SetActPos(Seq Seq, int nIndex, SenPos senPos)
		{
			try
			{
				if (m_dicActPos.ContainsKey(Seq) == false)
				{
					m_dicActPos.Add(Seq, new Dictionary<int, SenPos>());
				}

				if (m_dicActPos[Seq].ContainsKey(nIndex) == false)			m_dicActPos[Seq].Add(nIndex, senPos);
				else														m_dicActPos[Seq][nIndex] = senPos;
			}
			catch
			{

			}
		}

		public	SenPos	GetActPos(Seq Seq, int nIndex)
		{
			try
			{
				if (m_dicActPos.ContainsKey(Seq) == false)					return	SenPos.UNKNOWN;
				if (m_dicActPos[Seq].ContainsKey(nIndex) == true)			return	m_dicActPos[Seq][nIndex];
				else														return	SenPos.UNKNOWN;
			}
			catch
			{
				return SenPos.UNKNOWN;
			}
				
		}
		#endregion

		#region SeqMessage
		public void		SetMsg(SeqMsg SeqMsg, string Msg)
		{
			UI.SeqMessageBox.SetMessage(SeqMsg,Msg);
		}

		public void		MsgShow(SeqMsg SeqMsg)
		{
			UI.SeqMessageBox.Show(SeqMsg);
		}
		
		public void		MsgClose(SeqMsg SeqMsg)
		{
			UI.SeqMessageBox.Close(SeqMsg);
		}

		public	string	PopupMsgResult(SeqMsg SeqMsg)
		{
			return UI.SeqMessageBox.GetResult(SeqMsg);
		}
		public	bool	IsMsgPopUp(SeqMsg SeqMsg)
		{
			return UI.SeqMessageBox.IsVisible(SeqMsg); 
		}

		public	void	MsgAllClose()
		{
			UI.SeqMessageBox.AllClose();
		}
		#endregion

		#region InterLock 메세지 관련
		private Dictionary<string, string>	m_dicInterlock	= new Dictionary<string, string>();
		public	Dictionary<string, string>	DicInterlock { get => m_dicInterlock; }

		private	Dictionary<MT, string>	m_dicInterlockMot	= new Dictionary<MT, string>();
		public	Dictionary<MT, string>	DicInterlockMot { get => m_dicInterlockMot; }
		
		private	object	m_LockItk		= new object();
		private	object	m_LockItkMot	= new object();

		public bool InterlockMsgAdd(string strFormat, params object[] args)
		{
			try
			{
				System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1, true);
				if (strFormat == null)			return false;

				string	str_msg		= string.Format(strFormat, args);
				string	tmp_comp	= string.Empty;

				if (sf != null)
				{
					tmp_comp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
				}

				lock(m_LockItk)
				{
					if (m_dicInterlock.ContainsKey(tmp_comp) == false)			m_dicInterlock.Add(tmp_comp, str_msg);
					else														m_dicInterlock[tmp_comp]	= str_msg;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
			}

			return false;
		}

		public bool InterlockMsgAdd(int nPartNo, string strMsg)
		{
			try
			{
				System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1, true);
				if (strMsg == null)			return false;

				string	tmpComp = string.Empty;

				if (sf != null)
				{
					tmpComp = string.Format("{0}_{2}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name, nPartNo);
				}

				lock(m_LockItk)
				{
					if (m_dicInterlock.ContainsKey(tmpComp) == false)			m_dicInterlock.Add(tmpComp, strMsg);
					else														m_dicInterlock[tmpComp]	= strMsg;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
			}

			return false;
		}

		public bool InterlockMsgReset()
		{
			try
			{
				System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1, true);

				string	tmpComp = string.Empty;

				if (sf != null)
				{
					tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
				}

				lock(m_LockItk)
				{
					if (m_dicInterlock.ContainsKey(tmpComp) == true)			m_dicInterlock.Remove(tmpComp);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
			}

			return true;
		}

		public bool InterlockMsgReset(int nPartNo)
		{
			try
			{
				System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1, true);

				string	tmpComp = string.Empty;

				if (sf != null)
				{
					tmpComp = string.Format("{0}_{2}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name, nPartNo);
				}

				lock(m_LockItk)
				{
					if (m_dicInterlock.ContainsKey(tmpComp) == true)			m_dicInterlock.Remove(tmpComp);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
			}

			return true;
		}

		public bool InterlockMsgMotAdd(MT Motor, string strMsg)
		{
			try
			{
				if (strMsg == null)			return false;

				lock(m_LockItkMot)
				{
					if (m_dicInterlockMot.ContainsKey(Motor) == false)			m_dicInterlockMot.Add(Motor, strMsg);
					else														m_dicInterlockMot[Motor]	= strMsg;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
			}

			return false;
		}

		public bool InterlockMsgMotReset(MT Motor)
		{
			try
			{
				lock(m_LockItkMot)
				{
					if (m_dicInterlockMot.ContainsKey(Motor) == true)			m_dicInterlockMot.Remove(Motor);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
			}

			return false;
		}

		#endregion

		#region	NDm Flag
		public void	NDmWriteAddFlag(eNDM eNo, int nFlag)
		{
			int	nval	= NDm.Read(eNo);
			nval	|= nFlag;
			NDm.Write(eNo, nval);
		}

		public	void	NDmWriteRemoveFlag(eNDM eNo, int nFlag)
		{
			int	nval	= NDm.Read(eNo);
			nval	&= ~nFlag;
			NDm.Write(eNo, nval);
		}
		#endregion

		#region Use Part
		private	Dictionary<UsePartName, UsePartMode>	m_dicUsePart	= new Dictionary<UsePartName, UsePartMode>();

		public	void	InitUsePart()
		{
			for (UsePartName part_name = 0; part_name < UsePartName.Max; part_name++)
			{
				if (m_dicUsePart.ContainsKey(part_name) == false)			m_dicUsePart.Add(part_name, UsePartMode.Enable);
				else														m_dicUsePart[part_name]	= UsePartMode.Enable; 
			}
		}

		public	void	SetUsePart(UsePartName PartName, UsePartMode UseMode)
		{
			if (m_dicUsePart.ContainsKey(PartName) == false)			m_dicUsePart.Add(PartName, UseMode);
			else														m_dicUsePart[PartName]	= UseMode;
		}

		public	UsePartMode	GetUsePart(UsePartName PartName)
		{
			if (m_dicUsePart.ContainsKey(PartName) == false)			return UsePartMode.Enable;

			return m_dicUsePart[PartName];
		}

		public	bool	IsUsePart(UsePartName PartName, UsePartMode PartMode)
		{
			if (m_dicUsePart.ContainsKey(PartName) == false)			m_dicUsePart.Add(PartName, UsePartMode.Enable);

			if (PartMode == UsePartMode.Disable)			return m_dicUsePart[PartName] == UsePartMode.Disable;
			else											return m_dicUsePart[PartName].HasFlag(PartMode);
		}
		#endregion
		
		#region Door Close State
		private	bool[]	m_bDoorOpen	= new bool[5];

		public	void	SetDoorOpen(int nIndex, bool bState)
		{
			try
			{
				if (nIndex < 0  ||  nIndex > 4)			return;

				m_bDoorOpen[nIndex]	= bState;
				
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	bool	GetDoorOpen(int nIndex)
		{
			try
			{
				if (nIndex < 0  ||  nIndex > 4)			return false;

				return m_bDoorOpen[nIndex];
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}
		#endregion
		
		#region	m_dicLotYield
		private	Dictionary<string, int[]>	m_dicLotYield		= new Dictionary<string, int[]>(); // index 0-OK, 1-NG , 2-User OK
		private	Dictionary<string, bool>	m_TooManyNGBypass	= new Dictionary<string, bool>(); // index 0-OK, 1-NG , 2-User OK
		private	Dictionary<string, int>		m_dicFiduError		= new Dictionary<string, int>(); 
		private	object	m_LotYieldFileLock	= new object();

		public	void	ClearLotYield()
		{
			m_dicLotYield.Clear();

			LotYieldDataSave(Program.DataDirectory +"LotYield.dat");
		}

		public	void	RemoveLotYield(string strKeyValue)
		{
			if (m_dicLotYield.ContainsKey(strKeyValue) == false)			return;

			m_dicLotYield.Remove(strKeyValue);

			if (m_TooManyNGBypass.ContainsKey(strKeyValue) == true)		
				m_TooManyNGBypass.Remove(strKeyValue);
		
			LotYieldDataSave(Program.DataDirectory +"LotYield.dat");
		}

		public	void	SetLotYield(string strKeyValue, int[] nValue)
		{
			if (m_dicLotYield.ContainsKey(strKeyValue) == false)			m_dicLotYield.Add(strKeyValue, nValue);
			else															m_dicLotYield[strKeyValue]	= nValue;

			LotYieldDataSave(Program.DataDirectory +"LotYield.dat");
		}

		public	int[]	GetLotYield(string strKeyValue)
		{
			if (m_dicLotYield.ContainsKey(strKeyValue) == false)			return new int[3]{0, 0, 0 };
			else															return m_dicLotYield[strKeyValue];
		}
		
		public	bool	GetLotYield(string strKeyValue, ref int[] nYield)
		{
			if (m_dicLotYield.ContainsKey(strKeyValue) == false)
			{
				return false;
			}
			else
			{
				nYield	= m_dicLotYield[strKeyValue];
				return true;
			}
		}

		private	void	LotYieldDataSave(string strFileName)
		{
			lock (m_LotYieldFileLock)
			{
				FileStream	fs	= new FileStream(strFileName, FileMode.Create);
				try
				{
					BinaryFormatter	Formatter	= new BinaryFormatter();
					Formatter.Serialize(fs, m_dicLotYield);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
				}
				fs.Close();
			}
		}

		private	void	LotYieldDataOpen(string strFileName)
		{
			FileInfo	fi	= new FileInfo(strFileName);
			if (fi.Exists == false)
			{
				m_dicLotYield.Clear();
				return;
			}

			lock (m_LotYieldFileLock)
			{
				FileStream	fs	= new FileStream(strFileName, FileMode.Open);
				try
				{
					BinaryFormatter	Formatter	= new BinaryFormatter();
					m_dicLotYield				= (Dictionary<string, int[]>)Formatter.Deserialize(fs);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
				}
				fs.Close();
			}
		}

		public bool		GetManyNGBypass(string strKeyValue)
		{
			if (m_TooManyNGBypass.ContainsKey(strKeyValue) == false)	m_TooManyNGBypass.Add(strKeyValue, true);
			
			return m_TooManyNGBypass[strKeyValue];
		}

		public void		SetManyNGBypass(string strKeyValue, bool bBypass)
		{
			if (m_TooManyNGBypass.ContainsKey(strKeyValue) == false)	m_TooManyNGBypass.Add(strKeyValue, bBypass);
			
			 m_TooManyNGBypass[strKeyValue] = bBypass;
		}

		public int		GetFiduErrorCount(string strKeyValue)
		{
			if (m_dicFiduError.ContainsKey(strKeyValue) == false)	m_dicFiduError.Add(strKeyValue, 0);
			
			return m_dicFiduError[strKeyValue];
		}

		public void		AddFiduErrorCount(string strKeyValue)
		{
			if (m_dicFiduError.ContainsKey(strKeyValue) == false)	m_dicFiduError.Add(strKeyValue, 0);

			m_dicFiduError[strKeyValue] = m_dicFiduError[strKeyValue] + 1;
		}

		public void		ClearFiduErrorCount(string strKeyValue)
		{
			if (m_dicFiduError.ContainsKey(strKeyValue) == false) return;

			m_dicFiduError.Remove(strKeyValue);
		}
		
		public void		AllClearFiduErrorCount()
		{
			m_dicFiduError.Clear();
		}
		#endregion
		
		#region	Run Lot Info
		private List<stRunLotInfo>	m_ltRunLotInfo	= new List<stRunLotInfo>();
		private	object	m_RunLotInfoFileLock	= new object();

		private	stRunLotInfo	NewtsRunLotInfo()
		{
			stRunLotInfo	tsinfo	= new stRunLotInfo();

			tsinfo.UserID			= string.Empty;
			tsinfo.WorkOrder		= string.Empty;
			tsinfo.KeyValue			= string.Empty;
			tsinfo.ProgramID_Top	= string.Empty;
			tsinfo.ProgramID_Btm	= string.Empty;
			tsinfo.ProdCount		= "0";
			tsinfo.StartTime		= string.Empty;

			return tsinfo;
		}

		public	void	ClearRunLotInfo()
		{
			m_ltRunLotInfo.Clear();

			RunLotInfoDataSave(Program.DataDirectory +"RunLotInfo.dat");
		}

		public	void	RemoveRunLotInfo(int nIndex)
		{
			if (m_ltRunLotInfo.Count < nIndex+1)			return;

			m_ltRunLotInfo.RemoveAt(nIndex);

			RunLotInfoDataSave(Program.DataDirectory +"RunLotInfo.dat");
		}

		public	void	SetRunLotInfo(stRunLotInfo tsValue)
		{
			int	nindex	= m_ltRunLotInfo.FindIndex(v => v.KeyValue == tsValue.KeyValue);

			if (nindex >= 0)			m_ltRunLotInfo[nindex]	= tsValue;
			else						m_ltRunLotInfo.Add(tsValue);

			RunLotInfoDataSave(Program.DataDirectory +"RunLotInfo.dat");
		}

		public	stRunLotInfo	GetRunLotInfo(int nIndex)
		{
			if (m_ltRunLotInfo.Count < nIndex+1)			return NewtsRunLotInfo();
			else											return m_ltRunLotInfo[nIndex];
		}

		public	stRunLotInfo	GetRunLotInfo(string strKeyValue)
		{
			int	nindex	= m_ltRunLotInfo.FindIndex(v => v.KeyValue == strKeyValue);

			if (nindex >= 0)			return m_ltRunLotInfo[nindex];
			else						return NewtsRunLotInfo();
		}

		public	bool	ContainsRunLotInfo(string strKeyValue)
		{
			int	nindex	= m_ltRunLotInfo.FindIndex(v => v.KeyValue == strKeyValue);

			if (nindex >= 0)			return true;
			else						return false;
		}
		
		private	void	RunLotInfoDataSave(string strFileName)
		{
			lock (m_RunLotInfoFileLock)
			{
				FileStream	fs	= new FileStream(strFileName, FileMode.Create);
				try
				{
					BinaryFormatter	Formatter	= new BinaryFormatter();
					Formatter.Serialize(fs, m_ltRunLotInfo);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
				}
				fs.Close();
			}
		}

		private	void	RunLotInfoDataOpen(string strFileName)
		{
			FileInfo	fi	= new FileInfo(strFileName);
			if (fi.Exists == false)
			{
				m_ltRunLotInfo.Clear();
				return;
			}

			lock (m_RunLotInfoFileLock)
			{
				FileStream	fs	= new FileStream(strFileName, FileMode.Open);
				try
				{
					BinaryFormatter	Formatter	= new BinaryFormatter();
					m_ltRunLotInfo				= (List<stRunLotInfo>)Formatter.Deserialize(fs);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message + "\n\r" + ex.StackTrace);
				}
				fs.Close();
			}
		}
		#endregion

		#region GapSize Info
		public	bool	GapSizeOpen(int nRecipeNo)
		{
			string str_file			= string.Format("{0}Recipe_{1:000}\\TrayPnPRecipe.Dat", Program.DataDirectory, nRecipeNo);
			tcTrayPnPInfo	info	= TrayPnPRecipe.FileOpen(str_file);

			if (info == null)			return false;

			return true;
		}
		#endregion

		#region UPH 계산
		[Serializable]
		public class tcUPHInfo
		{
			public	Dictionary<UPHStep, DateTime>	m_dicUPHTime	= new Dictionary<UPHStep, DateTime>();
			public	int								m_nCntProd		= 0;
			public	bool							m_RunUPH		= false;
			public	PStopwatch						m_pswDownTime	= new PStopwatch();
		}

		private tcUPHInfo	m_UPHInfo	= new tcUPHInfo();
		private	object		m_FileLock	= new object();
		private	string		m_strUPH	= Program.DataDirectory +"UPH.dat";

		private void InitUPH()
		{
			SDmDataOpen(m_strUPH);

			m_UPHInfo.m_pswDownTime	= new PStopwatch();
		}

		public void StartUPH()
		{
			m_UPHInfo.m_RunUPH	= true;
		}

		public void StopUPH()
		{
			m_UPHInfo.m_RunUPH	= false;
		}

		public bool IsRunUPH()
		{
			return m_UPHInfo.m_RunUPH;
		}

		public void SetUPHTime(UPHStep Step, DateTime dt)
		{
			if (Step == UPHStep.LastInsp)
			{
				if (m_UPHInfo.m_dicUPHTime.ContainsKey(Step) == false)			m_UPHInfo.m_dicUPHTime.Add(Step,dt);
				else															m_UPHInfo.m_dicUPHTime[Step]	= dt;
				SDmDataSave(m_strUPH);
			}
			else
			{
				if (m_UPHInfo.m_dicUPHTime.ContainsKey(Step) == true)				return;
				m_UPHInfo.m_dicUPHTime.Add(Step,dt);
				SDmDataSave(m_strUPH);
			}
		}

		public DateTime GetUPHTime(UPHStep Step)
		{
			if (m_UPHInfo.m_dicUPHTime.ContainsKey(Step) == true)			return	m_UPHInfo.m_dicUPHTime[Step];
			else															return 	DateTime.MinValue;
		}

		public void StartDown()
		{
			if (m_UPHInfo.m_pswDownTime.IsRunning == true)			return;

			m_UPHInfo.m_pswDownTime.Start();
		}

		public void StopDown()
		{
			if (m_UPHInfo.m_pswDownTime.IsRunning == false)			return;

			m_UPHInfo.m_pswDownTime.Stop();
		}

		public long GetDownTime()
		{
			if (m_UPHInfo.m_pswDownTime == null)			return 0;

			return m_UPHInfo.m_pswDownTime.ElapsedTicks;
		}

		public bool IsUPHTime(UPHStep Step)
		{
			if (m_UPHInfo.m_dicUPHTime.ContainsKey(Step) == true)			return true;
			else															return false;
		}

		public void ClearUPHTime()
		{
			m_UPHInfo.m_dicUPHTime.Clear();
			m_UPHInfo.m_nCntProd	= 0;
			m_UPHInfo.m_pswDownTime.Stop();
			m_UPHInfo.m_pswDownTime.Reset();
		}

		public void AddProd()
		{
			m_UPHInfo.m_nCntProd	+= 4;
		}

		public int GetProdCnt()
		{
			return m_UPHInfo.m_nCntProd;
		}

		private void SDmDataSave(string strFileName)
		{
			lock (m_FileLock)
			{
				FileStream	fs	= new FileStream(strFileName, FileMode.Create);
				if (fs == null)			return;
				try
				{
					BinaryFormatter	Formatter	= new BinaryFormatter();
					Formatter.Serialize(fs, m_UPHInfo);
				}
				catch
				{
					fs.Close();
				}
				fs.Close();
			}
		}

		private void SDmDataOpen(string strFileName)
		{
			FileInfo	fi	= new FileInfo(strFileName);
			if (fi.Exists == false)
			{
				return;
			}

			lock (m_FileLock)
			{
				FileStream	fs	= new FileStream(strFileName, FileMode.Open);
				try
				{
					BinaryFormatter	Formatter	= new BinaryFormatter();
					m_UPHInfo					= (tcUPHInfo)Formatter.Deserialize(fs);
				}
				catch
				{
					fs.Close();
					File.Delete(strFileName);	//파일이 망가져 읽지 못한 경우 삭제
				}
				fs.Close();
			}
		}
		#endregion

		#region Machine State Time Log
		private readonly object m_Lock = new object();
		private string Date { get; set; }
		private TimeSpan NormalRun ;
		private TimeSpan CameraIdle;
		private TimeSpan VerifyAlarm;
		private TimeSpan RejectTrayAlarm;
		private TimeSpan NormalStop;
		private TimeSpan ErrorStop ;

		private readonly string _savePath = @"D:\DATA(HOTAYI_MARS_SSD)\RunState.dat"; // 확장자만 바꿈
		private RunStateType	currentState	= RunStateType.NormalStop; // 초기 상태
		private DateTime		_currentLogDate = DateTime.Now.Date;
		private DateTime		lastStateStart	= DateTime.Now;

		public RunStateType GetCurrentState() { return currentState; }
		//상태 변경 시 호출
		public void		ChangeState(RunStateType newState)
		{
			try
			{
				lock (m_Lock)
				{
					if (newState == currentState) return;

					var now = DateTime.Now;
					TimeSpan elapsed = now - lastStateStart;

					AddTime(currentState, elapsed);

					currentState = newState;
					lastStateStart = now;

					GLog.Add($"State Change : {newState.ToString()}");
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, $"[ChangeState] Message: {ex.Message}\nStackTrace: {ex.StackTrace}");
			}
		}

		private void	AddTime(RunStateType state, TimeSpan time)
		{
			switch (state)
			{
				case RunStateType.NormalRun: NormalRun += time; break;
				case RunStateType.CameraIdle: CameraIdle += time; break;
				case RunStateType.VerifyAlarm: VerifyAlarm += time; break;
				case RunStateType.RejectTrayAlarm: RejectTrayAlarm += time; break;
				case RunStateType.NormalStop: NormalStop += time; break;
				case RunStateType.ErrorStop: ErrorStop += time; break;
			}
		}
		//data파일로 저장
		public void		SaveToDat()
		{
			try
			{
				var lines = new List<string>
				{
					_currentLogDate.ToString("yyyy-MM-dd"),
					NormalRun.ToString(),
					CameraIdle.ToString(),
					VerifyAlarm.ToString(),
					RejectTrayAlarm.ToString(),
					NormalStop.ToString(),
					ErrorStop.ToString()
				};

				File.WriteAllLines(_savePath, lines);
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, $"[SaveToDat] {ex.Message}\n{ex.StackTrace}");
			}
		}
		//data파일 불러오기
		public void		StateTimeLoadFromDat()
		{
			try
			{
				if (!File.Exists(_savePath))
					return;

				string[] lines = File.ReadAllLines(_savePath);
				if (lines.Length < 7)
					return;

				if (!DateTime.TryParse(lines[0], out DateTime savedDate))
					return;

				//저장된 시간이 현재와 동일한 날짜면 시간 이어가기
				lock (m_Lock)
				{
					_currentLogDate = savedDate;
					NormalRun		= TimeSpan.Parse(lines[1]);
					CameraIdle		= TimeSpan.Parse(lines[2]);
					VerifyAlarm		= TimeSpan.Parse(lines[3]);
					RejectTrayAlarm = TimeSpan.Parse(lines[4]);
					NormalStop		= TimeSpan.Parse(lines[5]);
					ErrorStop		= TimeSpan.Parse(lines[6]);
				}
				//다른 경우 로그쓰고 초기화
				if (savedDate != DateTime.Now.Date)
				{
					LogDaily(savedDate);

					_currentLogDate = DateTime.Now.Date;
					lastStateStart = DateTime.Now;
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, $"[LoadFromDat] {ex.Message}\n{ex.StackTrace}");
			}
		}

		public void		UpdateDailyStateTime()
		{
			try
			{
				lock (m_Lock)
				{
					var now = DateTime.Now;
					if (now.Date != _currentLogDate) //다음날인 경우
					{
						TimeSpan elapsed = now - lastStateStart;
						AddTime(currentState, elapsed); //현재 상태추가

						LogDaily(_currentLogDate);

						_currentLogDate = now.Date;
						lastStateStart = now;
					}
					SaveToDat();
				}
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, $"[LogAndResetIfMidnight] {ex.Message}\n{ex.StackTrace}");
			}
		}



		private void	LogDaily(DateTime logDate)
		{
			try
			{
				string str = "";
				str += $"[Daily Operation Log] {logDate:yyyy-MM-dd}{Environment.NewLine}";
				str += $"   {"Operation Time",-18} : {FormatTime(NormalRun + CameraIdle + VerifyAlarm + RejectTrayAlarm)}{Environment.NewLine}";
				str += $"   {"Non Operation Time",-18} : {FormatTime(NormalStop + ErrorStop)}{Environment.NewLine}";
				str += $"   {"NormalRun",-18} : {FormatTime(NormalRun)}{Environment.NewLine}";
				str += $"   {"CameraIdle",-18} : {FormatTime(CameraIdle)}{Environment.NewLine}";
				str += $"   {"VerifyAlarm",-18} : {FormatTime(VerifyAlarm)}{Environment.NewLine}";
				str += $"   {"RejectTrayAlarm",-18} : {FormatTime(RejectTrayAlarm)}{Environment.NewLine}";
				str += $"   {"NormalStop",-18} : {FormatTime(NormalStop)}{Environment.NewLine}";
				str += $"   {"ErrorStop",-18} : {FormatTime(ErrorStop)}{Environment.NewLine}";
				str += $"   {"TotalTime",-18} : {FormatTime(NormalRun + CameraIdle + VerifyAlarm + RejectTrayAlarm + NormalStop + ErrorStop)}";

				UI.Logs.Write(LOGPART.DATA, str);
				
				NormalRun = CameraIdle = VerifyAlarm = RejectTrayAlarm = TimeSpan.Zero;
				NormalStop = ErrorStop = TimeSpan.Zero;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, $"[LogDaily] {ex.Message}\n{ex.StackTrace}");
			}
		}

		private string FormatTime(TimeSpan t)
		{
			return $"{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}.{t.Milliseconds / 10:D2}";
		}
		#endregion



		#region 트레이 좌표 계산
		private enum Dir
		{
			Right,  // 오른쪽 → 좌→우 방향
			Left,   // 왼쪽 → 우→좌 방향
			Top,    // 위쪽 → 상→하 방향
			Bottom  // 아래쪽 → 하→상 방향
		}


		
		private int GetValue(int nIdx, string Module , int Rows, int Cols)
		{
			Dir FirstDir;
			Dir SecondDir;

			switch (SM.SHM.GetRecipeData(Module, DEVICE.TrayFirstDir).ToUpper())
			{
				case "T": FirstDir = Dir.Top; break;
				case "B": FirstDir = Dir.Bottom; break;
				case "L": FirstDir = Dir.Left; break;
				case "R": FirstDir = Dir.Right; break;
				default: FirstDir = Dir.Top; break;
			}

			switch (SM.SHM.GetRecipeData(Module,DEVICE.TraySecondDir).ToUpper())
			{
				case "T": SecondDir = Dir.Top; break;
				case "B": SecondDir = Dir.Bottom; break;
				case "L": SecondDir = Dir.Left; break;
				case "R": SecondDir = Dir.Right; break;
				default:  SecondDir = Dir.Top; break;
			}

			int r = nIdx / Cols;
			int c = nIdx % Cols;

			int tr = 0, tc = 0;

			// FirstDir이 주 반복 방향
			bool firstIsRow = (FirstDir == Dir.Top || FirstDir == Dir.Bottom);
			bool firstReverse = (FirstDir == Dir.Bottom || FirstDir == Dir.Right);
			bool secondReverse = (SecondDir == Dir.Bottom || SecondDir == Dir.Right);

			if (firstIsRow)
			{
				// 행 주 반복
				tr = firstReverse ? Rows - 1 - r : r;
				tc = secondReverse ? Cols - 1 - c : c;
				return tr + tc * Rows; // 열 우선 배치
			}
			else
			{
				// 열 주 반복
				tc = firstReverse ? Cols - 1 - c : c;
				tr = secondReverse ? Rows - 1 - r : r;
				return tc + tr * Cols; // 행 우선 배치
			}
		}
		
		public int GetOriginToTransIdx(int nIdx, string Module)
		{
			int Rows =  GetRecipeDataToInt(Module, DEVICE.TrayRowCnt) / 1000;
			int Cols =  GetRecipeDataToInt(Module, DEVICE.TrayColCnt) / 1000;
			
			for (int row = 0; row < Rows; row++)
			{
				for (int col = 0; col < Cols; col++)
				{
					int val = row * Cols + col;
					if (GetValue(val, Module, Rows , Cols) == nIdx)
						return val;
				}
			}
			return -1;
		}

		
		public int GetTransToOriginIdx(int transformedIdx, string Module)
		{
			int Rows =  GetRecipeDataToInt(Module, DEVICE.TrayRowCnt) / 1000;
			int Cols =  GetRecipeDataToInt(Module, DEVICE.TrayColCnt) / 1000;

			for (int row = 0; row < Rows; row++)
			{
				for (int col = 0; col < Cols; col++)
				{
					int val = row * Cols + col;
					if (GetOriginToTransIdx(val, Module) == transformedIdx)
						return val;
				}
			}
			return -1;
		}

		#endregion


		#region model xml 

		public const string DataDirectory = "D:\\DATA(HOTAYI_MARS_SSD)\\Model.xml";
		private XDocument _xmlModel;

		public string FindModelByRegex(string input)
		{
			if (string.IsNullOrEmpty(input))
				return "Unknown";

			var items = _xmlModel.Descendants("Item")
				.Select(item => new
				{
					Num = item.Attribute("num")?.Value,
					Model = item.Attribute("model")?.Value
				})
				.Where(x => !string.IsNullOrEmpty(x.Num));

			var matchedItem = items
				.Where(x => Regex.IsMatch(input, "^" + Regex.Escape(x.Num)))
				.OrderByDescending(x => x.Num.Length)
				.FirstOrDefault();

			return matchedItem?.Model ?? "Unknown";
		}

		//시뮬용
		public string FindRegexByModel(string input)
		{
			if (string.IsNullOrEmpty(input))
				return "Unknown";

			var items = _xmlModel.Descendants("Item")
				.Select(item => new
				{
					Pattern = new Regex("^" + Regex.Escape(item.Attribute("model")?.Value ?? "")),
					Num = item.Attribute("num")?.Value
				});

			var matchedItem = items
				.Where(item => item.Pattern.IsMatch(input))
				.OrderByDescending(item => item.Pattern.ToString().Length)
				.FirstOrDefault();

			return matchedItem?.Num ?? "Unknown";
		}

		public void XmlModelUpdate()
		{
			try
			{
				if (!File.Exists(DataDirectory))
				{
					frmMessageBox msg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.Error, $"Can not Find Model.xml : {DataDirectory}");
					msg.Show();
					return;
				}
				_xmlModel = XDocument.Load(DataDirectory);
			}
			catch (Exception ex)
			{
				frmMessageBox msg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.Error, $"Model.xml Load Fail : {DataDirectory}");
				msg.Show();
			}
		}



		#endregion


	}


	#endregion


}
