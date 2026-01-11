using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace H_MarsSSD
{
    public class TypeConversion
    {
        #region [ Return : byte ]
        /// <summary>
        /// Short Array -> Byte Array
        /// </summary>
        /// <param name="data">short Array </param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromShortArray(short[] shortArray)
        {
            byte[] data = new byte[shortArray.Length * 2];
            for (int i = 0; i < shortArray.Length; i++)
                Array.Copy(BitConverter.GetBytes(shortArray[i]), 0, data, i * 2, 2);
            return data;
        }

        /// <summary>
        /// Short -> Byte Array
        /// </summary>
        /// <param name="data">short Array </param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromShort(short shortArray)
        {
            byte[] data = new byte[2];
            byte[] _src = BitConverter.GetBytes(shortArray);
            //             for (int i = 0; i < data.Length; i++)
            //                 System.Buffer.BlockCopy(_src, 0, data, i, 1);
            return _src;
        }

        /// <summary>
        /// short Array -> byte Array
        /// </summary>
        /// <param name="shortArray"></param>
        /// <param name="start"></param>
        /// <param name="_size"></param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromShort(short[] shortArray, int start, int _size)
        {
            byte[] _dst = new byte[_size * 2];

            try
            {
                for (int i = 0; i < _size; i++)
                    System.Buffer.BlockCopy(BitConverter.GetBytes(shortArray[start + i]), 0, _dst, i * 2, 2);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }

            return _dst;
        }

        /// <summary>
        /// short Array -> byte Array
        /// </summary>
        /// <param name="shortArray"></param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromShortArray_UseBlockCopy(short[] shortArray)
        {
            byte[] temp = new byte[shortArray.Length * sizeof(short)];
            Buffer.BlockCopy(shortArray, 0, temp, 0, temp.Length);

            return temp;
        }

        /// <summary>
        /// UShort Array -> Byte Array
        /// </summary>
        /// <param name="data"> UShort Array </param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromUShortArray(ushort[] ushortArray)
        {
            byte[] data = new byte[ushortArray.Length * 2];
            for (int i = 0; i < ushortArray.Length; i++)
                Array.Copy(BitConverter.GetBytes(ushortArray[i]), 0, data, i * 2, 2);
            return data;
        }

        /// <summary>
        /// UShort -> Byte Array
        /// </summary>
        /// <param name="data"> UShort </param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromUShort(ushort value)
        {
            byte[] data = new byte[2];
            Array.Copy(BitConverter.GetBytes(value), 0, data, 0, 2);

            return data;
        }

        /// <summary>
        /// uint array -> byte array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromUintArray(uint[] data)
        {
            byte[] dstbyte = new byte[data.Length * 4];
            for (int i = 0; i < data.Length; i++)
            {
                byte[] srcbyte = BitConverter.GetBytes(data[i]);
                Array.Copy(srcbyte, 0, dstbyte, i * 4, 4);
            }

            return dstbyte;
        }

        /// <summary>
        /// Bit Array -> Byte Array
        /// </summary>
        /// <param name="data">Bit Array </param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromBitArray(BitArray bits)
        {
            int numBytes = bits.Count / 8;
            if (bits.Count % 8 != 0) numBytes++;

            byte[] bytes = new byte[numBytes];
            int byteIndex = 0, bitIndex = 0;

            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

                bitIndex++;
                if (bitIndex == 8)
                {
                    bitIndex = 0;
                    byteIndex++;
                }
            }

            return bytes;
        }

        /// <summary>
        /// String -> Byte Array
        /// </summary>
        /// <param name="data"> String </param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromString(string str)
        {
            byte[] StrByte = Encoding.ASCII.GetBytes(str);
            return StrByte;
        }

        /// <summary>
        /// String -> Byte Array
        /// </summary>
        /// <param name="data"> String </param>
        /// <returns></returns>
        public static byte[] GetByteArrayFromString(string str, int byteSize)
        {
            byte[] byResult = new byte[byteSize];
            Array.Clear(byResult, 0, byteSize);
            byte[] byData = Encoding.ASCII.GetBytes(str);
            for (int i = 0; i < byData.Length; i++)
                byResult[i] = byData[i];
            return byResult;
        }

        /// <summary>
        /// Byte Reverse
        /// </summary>
        /// <param name="data"> Byte </param>
        /// <returns></returns>
        public static byte ReverseByte(byte originalByte)
        {
            int result = 0;
            for (int i = 0; i < 8; i++)
            {
                result = result << 1;
                result += originalByte & 1;
                originalByte = (byte)(originalByte >> 1);
            }

            return (byte)result;
        }
        #endregion

        #region [ Return : short ]
        /// <summary>
        /// Byte Array -> Short Array
        /// </summary>
        /// <param name="data">Byte Array </param>
        /// <returns></returns>
        public static short[] GetShortArrayFromByteArray(byte[] byteArray)
        {
            int size = Convert.ToInt32(Math.Ceiling(byteArray.Length / (double)2));

            short[] shortArray = new short[size];
            byte[] byDst = new byte[size * 2];

            for (int i = 0; i < byDst.Length; i++)
            {
                if (i < byteArray.Length) byDst[i] = byteArray[i];
                else byDst[i] = (byte)0;
            }

            for (int i = 0; i < byteArray.Length; i += 2)
                shortArray[i / 2] = BitConverter.ToInt16(byDst, i);
            return shortArray;
        }

        public static short[] GetShortArrayFromByteArray_UseBlockCopy(byte[] byteArray)
        {
            short[] temp = new short[byteArray.Length / 2];
            Buffer.BlockCopy(byteArray, 0, temp, 0, temp.Length);

            return temp;
        }

        //         public static short[] GetShortArrayFromByteArray(byte[] byteArray)
        //         {
        //             int size = Convert.ToInt32(Math.Ceiling(byteArray.Length / (double)2));
        // 
        //             short[] shortArray = new short[size];
        //             for (int i = 0; i < byteArray.Length; i += 2)
        //                 shortArray[i / 2] = BitConverter.ToInt16(byteArray, i);
        //             return shortArray;
        //         }

        /// <summary>
        /// Byte Array -> Short
        /// </summary>
        /// <param name="data">Byte Array </param>
        /// <returns></returns>
        public static short GetShortFromByteArray(byte[] byteArray)
        {
            short sh = new short();
            if (byteArray.Length > 2) return sh;
            sh = BitConverter.ToInt16(byteArray, 0);
            return sh;
        }

        /// <summary>
        /// Byte Array -> UShort Array
        /// </summary>
        /// <param name="data">Byte Array </param>
        /// <returns></returns>
        public static ushort[] GetUShortArrayFromByteArray(byte[] byteArray, int nChOffset = 0)
        {
            ushort[] ushortArray = new ushort[byteArray.Length / 2];
            for (int i = 0; i < byteArray.Length - nChOffset; i += 2)
            {
                ushortArray[i / 2] = BitConverter.ToUInt16(byteArray, i + nChOffset);
            }

            return ushortArray;
        }

        /// <summary>
        /// Short Array -> UShort Array
        /// </summary>
        /// <param name="data">Short Array </param>
        /// <returns></returns>
        public static ushort[] GetUshortArrayFromShortArray(short[] shortArray)
        {
            byte[] byteArray = GetByteArrayFromShortArray(shortArray);
            ushort[] ushortArray = GetUShortArrayFromByteArray(byteArray);

            return ushortArray;
        }

        /// <summary>
        /// int -> UShort
        /// </summary>
        /// <param name="data"> INT </param>
        /// <returns></returns>
        public static ushort GetUshortFromINT(int nData)
        {
            byte[] bytes = BitConverter.GetBytes(nData);
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// UShort Array -> Short Array
        /// </summary>
        /// <param name="data">UShort Array </param>
        /// <returns></returns>
        public static short[] GetShortArrayFromUShortArray(ushort[] ushortArray)
        {
            byte[] byteArray = GetByteArrayFromUShortArray(ushortArray);
            short[] shortArray = GetShortArrayFromByteArray(byteArray);

            return shortArray;
        }

        public static short[] GetShortArrayFromBitArray(BitArray bits)
        {
            byte[] bytes = new byte[bits.Length / 8];
            bits.CopyTo(bytes, 0);
            return GetShortArrayFromByteArray(bytes);
        }

        public static short GetShortFromBitArray(BitArray bits)
        {
            byte[] bytes = new byte[bits.Length / 8];
            bits.CopyTo(bytes, 0);
            return GetShortFromByteArray(bytes);
        }

        /// <summary>
        /// string -> short Array
        /// </summary>
        /// <param name="data"> string </param>
        /// <returns></returns>
        public static short[] GetshortArrayFromstring(string str, int size)
        {
            byte[] byDst = Encoding.Default.GetBytes(str);
            short[] sh = TypeConversion.GetShortArrayFromByteArray(byDst);
            short[] shDst = new short[size / 2];
            for (int i = 0; i < shDst.Length; i++)
            {
                if (i < sh.Length) shDst[i] = sh[i];
                else shDst[i] = (short)0;
            }
            return shDst;
        }

        /// <summary>
        /// string -> short 
        /// </summary>
        /// <param name="data"> string </param>
        /// <returns></returns>
        public static short GetshortFromstring(string str, int size)
        {
            byte[] byDst = Encoding.Default.GetBytes(str.Substring(0, size));

            return BitConverter.ToInt16(byDst, 0);
        }
        #endregion

        #region [ Return : string ]
        /// <summary>
        /// Byte Array -> String
        /// </summary>
        /// <param name="data"> Byte Array </param>
        /// <returns></returns>
        public static string GetStringFromByteArray(byte[] strByte)
        {
            string str = "";
            try
            {
                str = Encoding.Default.GetString(strByte);
                return str;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return str;
        }

        /// <summary>
        /// Byte Array -> string
        /// </summary>
        /// <param name="data"> Byte Array </param>
        /// <returns></returns>
        public static string GetStringFromByteArray2(byte[] _byte)
        {
            string src = "";
            string dst = "";

            for (int i = 0; i < _byte.Length; i++)
            {
                src = string.Format("{0:C}", Convert.ToChar(_byte[i]));
                dst += src;
            }

            return dst;
        }
        #endregion

        #region [ Return : int ]
        /// <summary>
        /// BCD Array -> Ind
        /// </summary>
        /// <param name="data"> BCD Array </param>
        /// <returns></returns>
        public static int GetIntFromBCD(byte[] bcds)
        {
            int result = 0;
            int i = 0;
            foreach (byte bcd in bcds)
            {
                result |= (bcd << 4 * (3 - i));
                i++;
            }

            return result;
        }

        /// <summary>
        /// string -> uint array
        /// </summary>
        /// <param name="sTemp"> sorce string </param>
        /// <param name="nSize"> string length </param>
        /// <returns></returns>
        public static uint[] GetUintArrayFromString(string sTemp, int nSize = Define.MAX_STRING_LENGTH)
        {
            int T = sTemp.Length;
            char[] pdata = new char[T];

            for (int i = 0; i < T; i++)
                pdata[i] = sTemp[i];

            uint[] nData = new uint[nSize];

            if (T < 4)
            {
                for (int i = 0; i < T; i++)
                    nData[0] |= (uint)pdata[i] << (i * 8);

                return nData;
            }

            int nCnt = 0;
            for (int i = 0; i < (T / 4); i++)
            {
                nData[i] |= (uint)pdata[nCnt];
                nData[i] |= (uint)(pdata[nCnt + 1] << 8);
                nData[i] |= (uint)(pdata[nCnt + 2] << 16);
                nData[i] |= (uint)(pdata[nCnt + 3] << 24);
                nCnt += 4;
            }

            // 나머지
            for (int k = 0; k < (T % 4); k++)
                nData[T / 4] |= (uint)pdata[nCnt + k] << (k * 8);

            return nData;
        }
        #endregion

        #region [ Return : BitArray ]
        /// <summary>
        /// ushort array -> BitArray
        /// </summary>
        /// <param name="ush"></param>
        /// <returns></returns>
        public static BitArray GetBitArrayToUShortArray(ushort[] ush)
        {
            return new BitArray(TypeConversion.GetByteArrayFromUShortArray(ush));
        }
		#endregion


		#region [ Sorting Data : bit Convert, Struct Convert ]
		public static short BitArrayToShort(BitArray bit)
        {//Saved used
            try
            {
                byte[] by = new byte[bit.Length / 8];
                bit.CopyTo(by, 0);
                return BitConverter.ToInt16(by, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public static BitArray ShortToBitArray(short sh)
        {//Load used
            try
            {
                byte[] by = new byte[sizeof(short)];
                by = BitConverter.GetBytes(sh);
                return new BitArray(by);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BitArray(sizeof(short), false);
            }
        }

        //         public static byte[] GetSortingbyteArrayToStruct(SORTING_DATA[] stSort)
        //         {            
        //             byte[] byData = new byte[SHMDEF.DUMP_BUF_SIZE * sizeof(int)];
        //             int nSize = byData.Length / Define.MAX_SORTING_DATA;
        //             for (int i = 0; i < stSort.Length; i++)
        //             {
        //                 IntPtr pTemp = Marshal.AllocHGlobal(nSize);
        //                 Marshal.StructureToPtr(stSort[i], pTemp, true);
        //                 Marshal.Copy(pTemp, byData, i * nSize, nSize);
        //                 Marshal.FreeHGlobal(pTemp);
        //             }
        //             
        //             IntPtr pAddr = Marshal.AllocHGlobal(byData.Length);
        //             Marshal.Copy(byData, 0, pAddr, byData.Length);           
        //             Marshal.FreeHGlobal(pAddr);
        //             return byData;
        //         }                
        #endregion

        #region [ C++ To C# Time_Ticks Convert ]        
        /// <summary>
        /// C# DateTime Ticks -> c++ time_t
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public static long GetTimeTSecondsFrom(long ticks)
        {
            try
            {
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return (long)(new DateTime(ticks) - epoch).TotalSeconds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return 0;
        }

        /// <summary>
        /// C++ time_t -> c# DateTime Ticks
        /// </summary>
        /// <param name="time_t"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeTicksFrom(long time_t)
        {
            try
            {
                return new DateTime(((time_t * 10000000) + 0x089f7ff5f7b58000) + 324000000000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return new DateTime(0);
        }
        #endregion
		
		#region Alnalog Input 유량계
		public static Double GetFlowMeterFromAInput(DI eNum)
		{
			int		nValueMin		= 0;		//아날로그 입력 범위 (16bit)
			int		nValueMax		= 27648;
			double	dVoltageMin		= 0;	
			double	dVoltageMax		= 0;
			double	dFlowMeterMin	= 0;
			double	dFlowMeterMax	= 0;

			switch (eNum) //장비별 스펙에 따라 전압 범위/ 유량 범위 설정
			{
				case DI.MainAirFlow :
				case DI.BlowAirFlow :
					dVoltageMin		= 1.0;		//아날로그 입력 전압 범위 (1V ~ 5V)
					dVoltageMax		= 5.0;
					dFlowMeterMin	= 20;//20.0;		//유량계 유량 범위 (20 ~ 2000 L/Min)
					dFlowMeterMax	= 2000.0;
					break;
					
				case DI.LDPnPAirFlow :
				case DI.LDInspPnPAirFlow :
				case DI.ULDPnPAirFlow :
				case DI.ULDInspPnPAirFlow :
					dVoltageMin		= 4.0;		//아날로그 입력 전압 범위 (4V ~ 20V)
					dVoltageMax		= 20.0;
					dFlowMeterMin	= 0;//0.5;		//유량계 유량 범위 (0.5 ~ 50.0 L/Min)
					dFlowMeterMax	= 50.0;
					break;
				default:
					GLog.Add("GetFlowMeterFromAInput Error - Insert Invalid Input ");
					return 0;
			}

			//바이드 뮬러 메뉴얼 참조(S7 Format)
			//22.81 mA	32511	0x7EFF Overloading
			//20 mA		27648	0x6C00
			//12 mA		13824	0x3600 
			//4 mA		0		0x0000
			//1.19 mA	-4864	0xED00
			
			//16bit Bool형을 정수로 전환
			int nValue = 0;
			for (int i = 0; i < 15; i++)
			{
				if (G.cKernel.IsAIn(eNum + i))
					nValue |= (1 << i);     //nValue |= (1 << (15 - i));
			}
			//msb는 부호
			if (G.cKernel.IsAIn(eNum + 15)) // 부호 비트가 1이면 음수 처리
			{
				nValue = -(~nValue + 1); // 2의 보수로 음수 변환
			}

			double dVoltage =0;
			if (nValue > 0 && nValue < nValueMax) //정상 범위
			{
				dVoltage = dVoltageMin + ((double)nValue / nValueMax) * (dVoltageMax - dVoltageMin);
			}
			else if (nValue > 27648) //과부하
			{
				return -1;
				//dVoltage = 4.0;
			}
			else if ( nValue < 0) // 언더로드
			{	
				return -1;
				//dVoltage =  4.0;
			}

			// 전압을 유량으로 변환
			if (dVoltage > dVoltageMax )	dVoltage = dVoltageMax; 
			if (dVoltage < dVoltageMin )	dVoltage = dVoltageMin; 
			double dFlowMeter = dFlowMeterMin + ((dVoltage - dVoltageMin) / (dVoltageMax - dVoltageMin)) * (dFlowMeterMax - dFlowMeterMin);
			
			return dFlowMeter;
		}

		#endregion

		#region 시간관련
		public static string GetFormatTimeFromTicks(long timeTicks)
		{
		    DateTime dateTime = new DateTime(timeTicks);
		    return dateTime.ToString("yyyyMMddHHmmssff");
		}
		long GetTicksFromFormattedTime(string formattedTime)
		{
		    // formattedTime 문자열을 "yyyyMMddHHmmssff" 형식으로 파싱하여 DateTime으로 변환
		    DateTime dateTime = DateTime.ParseExact(formattedTime, "yyyyMMddHHmmssff", null);
		    return dateTime.Ticks;
		}

		#endregion
	}
}
