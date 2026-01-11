using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.InteropServices;
using System.IO.MemoryMappedFiles;
using System.IO;
using System.Threading;

namespace H_MarsSSD
{
	public class PVI
	{
		public	enum	WriteResult
		{
			Doing,
			EndOK,
			EndNG
		}

		private	const string	m_RootPath		= @"D:\DATASHARE\PVI\";
		private	Thread			m_ThreadWrite;
		private	Thread			m_ThreadRejectWrite;
		private	WriteResult		m_Result			= WriteResult.EndNG;
		private	WriteResult		m_RejectResult	= WriteResult.EndNG;

		public					PVI()
		{
			m_ThreadWrite	= new Thread(WritePVIData);
		}

		
		public	void			WritePVIStart()
		{
			try
			{
				if (m_Result == WriteResult.Doing  ||
					m_ThreadWrite.IsAlive == true)
				{
					return;
				}

				m_Result		= WriteResult.Doing;

				m_ThreadWrite	= new Thread(WritePVIData);
				m_ThreadWrite.Start();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				m_Result	= WriteResult.EndNG;
			}
		}
		
		public	WriteResult	WritePVIResult()
		{
			try
			{
				if (m_Result == WriteResult.EndNG)
				{
					G.AlmMng.ADD(ALM.ERROR_PVI_DATA_WRITE);
				}

				return m_Result;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return WriteResult.EndNG;
			}
		}
		
		private	string			GetDirectoryName() // 마지막 Map TrayID로 폴더생성
		{
			try
			{
				return DateTime.Now.ToString("yyyyMMddHHmmss0");
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return "ExceptionError";
			}
		}

		private string			GetHeaderInfo()
		{
			try
			{
				string	str_lot_no		= SM.FMT[FlowTray.ULDConv01].GetInfo().WorkOrder;
				string	strTopProgram	= SM.FMT[FlowTray.ULDConv01].GetInfo().ProgramID_Top;
				strTopProgram = strTopProgram.Replace('@', '|');
				return string.Format($"{SDm.Read(eSDM.PVI_MachineName)},{strTopProgram},{str_lot_no}");
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return "ExceptionError";
			}
		}

		private	void			WritePVIData()
		{
			try
			{
				m_Result	= WriteResult.Doing;

				string	str_dir_name		= GetDirectoryName();
				string	str_completed_path	= string.Format("{0}\\{1}", m_RootPath, str_dir_name);
				string	str_pvi_path		= string.Format("{0}\\{1}\\{2}", m_RootPath, str_dir_name, str_dir_name);
				string	str_date_time		= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff");

				if (WriteInspPvIFile(str_pvi_path, str_date_time) == false  ||
					WriteInspPvIMapFile(str_pvi_path, str_date_time) == false  ||
					WriteCompletedFile(str_completed_path, 1) == false)
				{
					m_Result	= WriteResult.EndNG;
				}

				m_Result	= WriteResult.EndOK;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				m_Result	= WriteResult.EndNG;
			}
		}

		private	bool			WriteCompletedFile(string strMainPath, int nAutoReport)
		{
			try
			{
				string str_file_path	= strMainPath + "\\pvi_completed.txt";

				if (Directory.Exists(strMainPath) == false)			Directory.CreateDirectory(strMainPath);

				if (SM.SHM.GetUseSkipData(USESKIP.SGEM) ==false)
				{
					SDm.Write(eSDM.SUMMARY_QUALITY_NAME, "SUMMARY_QUALITY_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx");
				}

				using (StreamWriter sw = new StreamWriter(str_file_path, false))
				{
					sw.WriteLine(GetHeaderInfo()+string.Format(",{0}\n{1}", nAutoReport, SDm.Read(eSDM.SUMMARY_QUALITY_NAME)));
					sw.Close();
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private	bool			WriteInspPvIFile(string strMainPath, string strDateTime)
		{
			try
			{
				string		str_file_path	= strMainPath + "\\insp_pvi.txt";
				string[]	str_dataes		= new string[13];
				string		str_WorkOrder		= "";
				string		str_KeyValue	= "";
				if (Directory.Exists(strMainPath) == false)			Directory.CreateDirectory(strMainPath);

				//* 데이터 베이스 설계서에 순번중 오타 있음
				str_dataes[0]	= DateTime.Now.Month.ToString();	//생성 달
				str_dataes[1]	= strDateTime;	//생성 시간
				str_dataes[2]	= "0";	//0	(Tray Pos)
				str_dataes[3]	= "0";	//아웃풋 트레이 넘버()
				str_dataes[4]	= "0";	//랏 넘버
				str_dataes[5]	= "0";	//0	(NO)
				str_dataes[6]	= "0";	//Col갯수
				str_dataes[7]	= "0";	//Row갯수
				str_dataes[8]	= "0";	//제품 개수
				str_dataes[9]	= "0";	//0	(NG)
				str_dataes[10]	= "0";	//0	(USEROK)
				str_dataes[11]	= "0";	//0	(GOOD)
				str_dataes[12]	= SM.SHM.GetRunLotInfo(0).UserID;	//작업자 아이디

				using (StreamWriter sw = new StreamWriter(str_file_path, false))
				{
					sw.WriteLine(GetHeaderInfo());

					for (FlowTray i = FlowTray.ULDConv01; i <= FlowTray.ULDConv31; i++)
					{
						if (SM.FMT.IsFlow(i) == false)			continue;
						
						InfoTray.tsInfo	info	= SM.FMT[i].GetInfo();
						str_WorkOrder			= info.WorkOrder;
						str_KeyValue			= info.KeyValue;

						if (SM.FMT[i].GetExistCount() == 0)		continue;


						str_dataes[3]	= info.TrayID;
						str_dataes[4]	= info.WorkOrder;
						str_dataes[6]	= info.ColCnt.ToString();
						str_dataes[7]	= info.RowCnt.ToString();
						str_dataes[8]	= info.MaxCntProdInTray.ToString();
						string	str_tmp	= string.Empty;
						foreach (string str in str_dataes)
						{
							if (string.IsNullOrEmpty(str_tmp) == false)			str_tmp	+= ",";

							str_tmp	+= str;
						}

						sw.WriteLine(str_tmp);
					}

					for (FlowRejectTray i = FlowRejectTray.RejectBuf1; i <= FlowRejectTray.RejectBuf15; i++)
					{
						if (SM.FMR.IsFlow(i) == false)						continue;
						if (SM.FMR[i].GetExistCount(str_KeyValue) == 0)		continue;

						InfoTray.tsInfo info = SM.FMR[i].GetInfo();

						str_dataes[3] = i.ToString();
						str_dataes[4] = str_WorkOrder;
						str_dataes[6] = info.ColCnt.ToString();
						str_dataes[7] = info.RowCnt.ToString();
						str_dataes[8] = info.MaxCntProdInTray.ToString();

						string str_tmp = string.Empty;
						foreach (string str in str_dataes)
						{
							if (string.IsNullOrEmpty(str_tmp) == false) str_tmp += ",";

							str_tmp += str;
						}

						sw.WriteLine(str_tmp);
					}

					if (SM.FMT.IsFlow(FlowTray.Reject) == true  && SM.FMT[FlowTray.Reject].GetExistCount(str_KeyValue) != 0)
					{
						InfoTray.tsInfo info = SM.FMT[FlowTray.Reject].GetInfo();

						str_dataes[3] = FlowTray.Reject.ToString();
						str_dataes[4] = str_WorkOrder;
						str_dataes[6] = info.ColCnt.ToString();
						str_dataes[7] = info.RowCnt.ToString();
						str_dataes[8] = info.MaxCntProdInTray.ToString();

						string str_tmp = string.Empty;
						foreach (string str in str_dataes)
						{
							if (string.IsNullOrEmpty(str_tmp) == false) str_tmp += ",";

							str_tmp += str;
						}

						sw.WriteLine(str_tmp);
					}

					sw.Close();
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private	bool			WriteInspPvIMapFile(string strMainPath, string strDateTime)
		{
			try
			{
				string		str_tmp			= string.Empty;
				string		str_file_path	= strMainPath + "\\insp_pvi_map.txt";
				string[]	str_dataes		= new string[23];
				int			npos_idx		= 1;
				string		str_KeyValue	= "";
				string		str_WorkOrder	= "";

				if (Directory.Exists(strMainPath) == false)			Directory.CreateDirectory(strMainPath);

				str_dataes[0]	= DateTime.Now.Month.ToString();	//생성 달
				str_dataes[1]	= strDateTime;	//생성 시간
				str_dataes[2]	= "0";	//Lot No
				str_dataes[3]	= "0";	//0 (Tray Pos)
				str_dataes[4]	= "0";	//Output_Tray No
				str_dataes[5]	= "0";	//순서(ex: 1,2,3)
				str_dataes[6]	= "0";	//SURFACE   1 (상부)/ 2 (하부)
				str_dataes[7]	= "0";	//생성 달 (위와 동일)
				str_dataes[8]	= "0";	//생성 시간 (위와 동일)
				str_dataes[9]	= "0";	//0 (Module ID)
				str_dataes[10]	= "0";	//0 (Part ID)
				str_dataes[11]	= "0";	//Serial No
				str_dataes[12]	= "0";	//0 (Pass Code)
				str_dataes[13]	= "0";	//0 (Defect Code)
				str_dataes[14]	= "0";	//0 (Defect Wind)
				str_dataes[15]	= "0";	//0 (Algo Kind)
				str_dataes[16]	= "0";	//Output_Tray_Column
				str_dataes[17]	= "0";	//Output_Tray_Row
				str_dataes[18]	= "";	//Confirm ID
				str_dataes[19]	= "";	//Confirm Time
				str_dataes[20]	= "";	//Temp1 -> PassCode가 0이 아닌 경우 원인 문자열
				str_dataes[21]	= "";	
				str_dataes[22]	= "";	

				using (StreamWriter sw = new StreamWriter(str_file_path, false))
				{
					sw.WriteLine(GetHeaderInfo());

					for (FlowTray i = FlowTray.ULDConv01; i <= FlowTray.ULDConv31; i++)
					{
						if (SM.FMT.IsFlow(i) == false)			continue;

						InfoTray.tsInfo	info	= SM.FMT[i].GetInfo();
						str_KeyValue	= info.KeyValue;
						str_WorkOrder	= info.WorkOrder;

						if (SM.FMT[i].GetExistCount() == 0)		continue;

						for (int j = 0; j < info.MaxCntProdInTray; j++)
						{
							if (info.ProdInfo[j].Exist == false)			continue;

							// 상부 결과
							str_dataes[2]	= info.WorkOrder;
							str_dataes[4]	= info.TrayID;
							str_dataes[5]	= npos_idx.ToString();
							str_dataes[7]	= GetMonthFromDateTime(info.ProdInfo[j].InspTimeTop);
							str_dataes[8]	= info.ProdInfo[j].InspTimeTop;
							str_dataes[11]	= info.ProdInfo[j].ProdID;
							str_dataes[16]	= info.ProdInfo[j].TrayCol.ToString();
							str_dataes[17]	= info.ProdInfo[j].TrayRow.ToString();
							str_tmp	= string.Empty;
							foreach (string str in str_dataes)
							{
								if (string.IsNullOrEmpty(str_tmp) == false)			str_tmp	+= ",";

								str_tmp	+= str;
							}

							sw.WriteLine(str_tmp);

							npos_idx++;

							// 하부 결과
							str_dataes[5]	= npos_idx.ToString();
							str_dataes[7]	= GetMonthFromDateTime(info.ProdInfo[j].InspTimeBtm);
							str_dataes[8]	= info.ProdInfo[j].InspTimeBtm;

							str_tmp	= string.Empty;
							foreach (string str in str_dataes)
							{
								if (string.IsNullOrEmpty(str_tmp) == false)			str_tmp	+= ",";

								str_tmp	+= str;
							}

							sw.WriteLine(str_tmp);

							npos_idx++;
						}
					}
					npos_idx = 1;
					for (FlowRejectTray i = FlowRejectTray.RejectBuf1; i <= FlowRejectTray.RejectBuf15; i++)
					{
						if (SM.FMR.IsFlow(i) == false)			continue;
						if (SM.FMR[i].GetExistCount() == 0)		continue;

						InfoTray.tsInfo info = SM.FMR[i].GetInfo();

						for (int j = 0; j < info.MaxCntProdInTray; j++)
						{
							if (info.ProdInfo[j].Exist == false)			continue;
							if (info.ProdInfo[j].KeyValue != str_KeyValue)	continue;

							// 상부 결과
							str_dataes[2] = str_WorkOrder;
							str_dataes[4] = i.ToString();
							str_dataes[5] = npos_idx.ToString();
							str_dataes[6] = "1"; //SURFACE : 1 (상부)
							str_dataes[7] = GetMonthFromDateTime(info.ProdInfo[j].InspTimeTop);
							str_dataes[8] = info.ProdInfo[j].InspTimeTop;
							str_dataes[11] = info.ProdInfo[j].ProdID;
							str_dataes[16] = info.ProdInfo[j].TrayCol.ToString();
							str_dataes[17] = info.ProdInfo[j].TrayRow.ToString();
							
							if (info.ProdInfo[j].ResultMES == (int)MESStatus.NG)
							{
								str_dataes[12] = "22";   //(Validation Fail -> MES NG 발생 시 코드)
								str_dataes[20] =  "Reject MES NG";
							}

							str_tmp = string.Empty;
							foreach (string str in str_dataes)
							{
								if (string.IsNullOrEmpty(str_tmp) == false) str_tmp += ",";

								str_tmp += str;
							}

							sw.WriteLine(str_tmp);

							npos_idx++;

							// 하부 결과
							str_dataes[5] = npos_idx.ToString();
							str_dataes[6] = "2"; //SURFACE : 2 (하부)
							str_dataes[7] = GetMonthFromDateTime(info.ProdInfo[j].InspTimeBtm);
							str_dataes[8] = info.ProdInfo[j].InspTimeBtm;

							str_tmp = string.Empty;
							foreach (string str in str_dataes)
							{
								if (string.IsNullOrEmpty(str_tmp) == false) str_tmp += ",";

								str_tmp += str;
							}

							sw.WriteLine(str_tmp);

							npos_idx++;
						}
					}

					if (SM.FMT.IsFlow(FlowTray.Reject) == true  && SM.FMT[FlowTray.Reject].GetExistCount() != 0)
					{
						InfoTray.tsInfo info = SM.FMT[FlowTray.Reject].GetInfo();

						for (int j = 0; j < info.MaxCntProdInTray; j++)
						{
							if (info.ProdInfo[j].Exist == false) continue;
							if (info.ProdInfo[j].KeyValue != str_KeyValue) continue;
							
							// 상부 결과
							str_dataes[2] = str_WorkOrder;
							str_dataes[4] = FlowTray.Reject.ToString();
							str_dataes[5] = npos_idx.ToString();
							str_dataes[7] = GetMonthFromDateTime(info.ProdInfo[j].InspTimeTop);
							str_dataes[8] = info.ProdInfo[j].InspTimeTop;
							str_dataes[11] = info.ProdInfo[j].ProdID;
							str_dataes[16] = info.ProdInfo[j].TrayCol.ToString();
							str_dataes[17] = info.ProdInfo[j].TrayRow.ToString();
							
							if (info.ProdInfo[j].ResultMES == (int)MESStatus.NG)
							{
								str_dataes[12] = "22";   //(Validation Fail -> MES NG 발생 시 코드)
								str_dataes[20] =  "Reject MES NG";
							}

							str_tmp = string.Empty;
							foreach (string str in str_dataes)
							{
								if (string.IsNullOrEmpty(str_tmp) == false) str_tmp += ",";

								str_tmp += str;
							}

							sw.WriteLine(str_tmp);

							npos_idx++;

							// 하부 결과
							str_dataes[5] = npos_idx.ToString();
							str_dataes[7] = GetMonthFromDateTime(info.ProdInfo[j].InspTimeBtm);
							str_dataes[8] = info.ProdInfo[j].InspTimeBtm;

							str_tmp = string.Empty;
							foreach (string str in str_dataes)
							{
								if (string.IsNullOrEmpty(str_tmp) == false) str_tmp += ",";

								str_tmp += str;
							}

							sw.WriteLine(str_tmp);

							npos_idx++;
						}

						sw.WriteLine(str_tmp);
					}
					sw.Close();
					SM.FMR.FlowClearAll();
				}

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		
		private	string			GetMonthFromDateTime(string strDateTime)
		{
			try
			{
				if (strDateTime == null)			strDateTime	= string.Empty;
				string[]	str_slpit	= strDateTime.Split('-');
				int			nmonth		= 0;

				if (str_slpit.Length < 2)										return DateTime.Now.Month.ToString();
				if (int.TryParse(str_slpit[1], out nmonth) == false)			return DateTime.Now.Month.ToString();
				if (nmonth < 1  ||  nmonth > 12)								return DateTime.Now.Month.ToString();

				return nmonth.ToString();
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return DateTime.Now.Month.ToString();
			}
		}
	}
}
