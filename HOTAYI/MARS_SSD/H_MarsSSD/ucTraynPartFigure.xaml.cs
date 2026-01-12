using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PMN.Control.WPF.MachineAni;
using static H_MarsSSD.frmProductEditPopup;

namespace H_MarsSSD
{
	/// <summary>
	/// ucTraynPartFigure.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ucTraynPartFigure : System.Windows.Controls.UserControl
	{
		public Form WinFormInstance { get; set; }
		public ucTraynPartFigure(FlowTray tray, List<int> deleteTargetPart, FlowPart flowpartOnlyDeleteTarget)
		{
			InitializeComponent();
			m_lstDeleteTargetPart = deleteTargetPart;
			m_flowpartOnlyDeleteTarget = flowpartOnlyDeleteTarget;
			m_flowtray = tray;
			SM.FMT.Clear(FlowTray.Tmp1);
			SM.FMT.Next(FlowTray.Empty, FlowTray.Tmp1);
			SM.FMT[FlowTray.Tmp1].SetInfo(SM.FMT[m_flowtray].GetInfo().DeepCopy());
			m_tempTrayInfo = SM.FMT[FlowTray.Tmp1];
		}

		public ucTraynPartFigure(FlowPart part)
		{
			InitializeComponent();

			m_flowpart = part;
			m_tempPartInfo = SM.FMP[m_flowpart];
			SetPartFigure();
		}

		#region [ Variables ]

		private bool		m_bIsCopyDone		= false;
		private stProdInfo	m_stCopyProdInfo	= new stProdInfo();
		private List<int>	m_ltTrayResult		= new List<int>();
		private List<int>	m_ltPartResult		= new List<int>();

		private FlowTray	m_flowtray			= FlowTray.Empty;
		private FlowPart	m_flowpart			= FlowPart.Empty;

		private FlowPart	m_flowpartOnlyDeleteTarget	= FlowPart.Empty;

		private InfoTray	m_tempTrayInfo;
		private InfoPart	m_tempPartInfo;
		private List<int>	m_lstDeleteTargetPart = new List<int>();
		
		#endregion

		#region [ Event Functions ]

		private void FigureArrayDoubleClickEvent(object sender, tsFigureArrayMouseButtonEventArgs e)
		{
			try
			{
				if (e.me.ChangedButton == MouseButton.Right)									return;
				if (Program._bSimul_ == false  &&  SM.SHM.IsBitData(BIT.AutoRun) == true)		return;


				if (Enum.IsDefined(typeof(FlowTray), m_flowtray) == true  &&  m_flowtray != FlowTray.Empty)
				{
					ChangeExistTrayProduct(e.Index);
					UpdateTrayFigure();
				}

				if (Enum.IsDefined(typeof(FlowPart), m_flowpart) == true  &&  m_flowpart != FlowPart.Empty)
				{
					ChangeExistPartProduct(e.Index);
					UpdatePartFigure();
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void FigureArrayMouseLeftDownEvent(object sender, tsFigureArrayMouseButtonEventArgs e)
		{
			try
			{
				if (e.me.ChangedButton == MouseButton.Right)									return;
				if (Program._bSimul_ == false  &&  SM.SHM.IsBitData(BIT.AutoRun) == true)		return;

				SelectProd(e.Index, e.BackColor);
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void FigureFigureArrayMouseEnterEvent(object sender, tsFigureArrayMouseEventArgs e)
		{
			try
			{
				if (e.me.LeftButton == MouseButtonState.Released)								return;
				if (Program._bSimul_ == false  &&  SM.SHM.IsBitData(BIT.AutoRun) == true)		return;

				SelectProd(e.Index, e.BackColor);
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region [ Functions ]

		#region - Public Functions

		public void UpdateTrayFigure()
		{
			try
			{
				SetTrayRowCol();
				SetTrayProdColor();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public void UpdatePartFigure()
		{
			try
			{
				SetPartProdColor();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		#region - Private Functions

		private void SetTrayRowCol()
		{
			try
			{
				int ntray_row = SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				int ntray_col = SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;

				//figure.Width			= 400;
				//figure.Height			= 200;

				figure._CountRow = ntray_row;
				figure._CountCol = ntray_col;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SetTrayProdColor()
		{
			try
			{
				int maxProdCount = figure._CountCol * figure._CountRow;

				if (maxProdCount == 0)			return;

				Color color = Colors.White;

				for (int i = 0; i < maxProdCount ; i++)
				{
					if (SM.FMT.IsFlow(m_flowtray) == false)			return;
                    
                    switch (m_tempTrayInfo.GetProdStatus(i))
                    {
						case ProdStatus.Unknown:                          color = Colors.Yellow;      break;
						case ProdStatus.NG:                               color = Colors.Red;         break;
						case ProdStatus.OK:                               color = Colors.Lime;        break;
						default:                                            color = Colors.White;       break;   //자재없음
                    }
					
					figure._SetInsideText(i, $"{i+1}", TextAlignment.Center ,new tcFont(), Colors.Black );
                    figure._SetInsideFigure(i, 1, Colors.Black, color);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SetPartProdColor()
		{
			try
			{
				Color	color	= Colors.White;
				
				for (int i = 0; i < figure._CountCol * figure._CountRow; i++)
				{
					if (SM.FMP.IsFlow(m_flowpart) == false)
					{
						figure._SetInsideFigure(i, 1, Colors.Black, Colors.White);
						continue;
					}

                    
                    switch (m_tempPartInfo.GetProdStatus(i))
                    {
                        case ProdStatus.Unknown:                          color = Colors.Yellow;      break;
                        case ProdStatus.NG:                               color = Colors.Red;         break;
                        case ProdStatus.OK:                               color = Colors.Lime;        break;
                        default:                                            color = Colors.White;       break;   //자재없음
                    }

					figure._SetInsideFigure(i, 1, Colors.Black, color);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void SetPartFigure()
		{
			try
			{
				figure.Width			= 400;
				figure.Height			= 50;
				figure._CountCol		= 8;
				figure._CountRow		= 1;

				figure._InsideGap		= new Point(10,2);
				figure._InsideMargin	= new Thickness(10);

				//switch (m_flowpart.ToString())
				//{
				//	case "LDPnP2":
				//	case "ULDPnP2":
				//		figure._ColorBack = Colors.Gainsboro;
				//		break;

				//	case "LdPnP1":
				//	case "UldPnP1":
				//		figure.Width			= 200;
				//		figure.Height			= 130;
				//		figure._CountCol		= 4;
				//		figure._CountRow		= 2;
				//		figure._InsideGap		= new Point(4, 4);
				//		figure._InsideMargin	= new Thickness(3, 13, 3, 3);
				//		figure.Margin			= new Thickness(5, 0, 5, 0);
				//		figure._ColorBack		= Colors.Gainsboro;
				//		break;

				//	case "SortPnP":
				//		figure.Width			= 130;
				//		figure.Height			= 200;
				//		figure._CountCol		= 2;
				//		figure._CountRow		= 4;
				//		figure._InsideGap		= new Point(4, 4);
				//		figure.Margin			= new Thickness(5, 0, 5, 0);
				//		figure._InsideMargin	= new Thickness(3, 3, 3, 3);
				//		figure._ColorBack		= Colors.Gainsboro;
				//		break;

				//	case "LdBuf":
				//	case "UldBuf":
				//	case "UldBtmBuf1":
				//	case "UldBtmBuf2":
				//		figure._ColorBack = Colors.DimGray;
				//		break;

				//	case "TopInspStg1":
				//	case "TopInspStg2":
				//	case "BtmInspStg1":
				//	case "BtmInspStg2":
				//		figure._InsideFigureType	= teFigureArrayType.Ellipse;
				//		figure._ColorBack			= Colors.DarkGray;
				//		break;
				//}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
        
		private void SelectProd(int nIndex, Color BackColor)
		{
			try
			{
				Color color = Colors.White;

				if (m_flowpart != FlowPart.Empty)
				{
					if (BackColor == Colors.RoyalBlue)
					{
						switch (m_tempPartInfo.GetProdStatus(nIndex))
						{
							case ProdStatus.Unknown:							color = Colors.Yellow;		break;
							case ProdStatus.NG:								color = Colors.Red;			break;
							case ProdStatus.OK:								color = Colors.Lime;		break;
							default:											color = Colors.White;		break;   //자재없음
						}
						m_ltPartResult.RemoveAt(m_ltPartResult.IndexOf(nIndex));

						figure._SetInsideFigure(nIndex, 1, Colors.Black, color);
					}
					else
					{
						//중복 Index는 생략
						if (m_ltPartResult.IndexOf(nIndex) != nIndex)
						{
							m_ltPartResult.Add(nIndex);
						}
						figure._SetInsideFigure(nIndex, 1, Colors.Black, Colors.RoyalBlue);
					}
				}

				if (m_flowtray != FlowTray.Empty)
				{
					if (BackColor == Colors.RoyalBlue)
					{	
						switch (m_tempTrayInfo.GetProdStatus(nIndex))
						{
							case ProdStatus.Unknown:							color = Colors.Yellow;		break;
							case ProdStatus.NG:								color = Colors.Red;			break;
							case ProdStatus.OK:								color = Colors.Lime;		break;
							default:											color = Colors.White;		break;   //자재없음
						}

						m_ltTrayResult.RemoveAt(m_ltTrayResult.IndexOf(nIndex));

						figure._SetInsideFigure(nIndex, 1, Colors.Black, color);
					}
					else
					{
						//중복 Index는 생략
						if (m_ltTrayResult.IndexOf(nIndex) != nIndex)
						{
							m_ltTrayResult.Add(nIndex);
						}

						figure._SetInsideFigure(nIndex, 1, Colors.Black, Colors.RoyalBlue);
					}
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private stProdInfo SetTrayProductInfo(int index, ProdStatus status)
		{
			stProdInfo info = new stProdInfo();

			try
			{
				int		ncnt_row		= SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				int		ncnt_col		= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
				int		x				= index % ncnt_col;
				int		y				= index / ncnt_col;
				string	str_trayid		= m_tempTrayInfo.GetInfo().TrayID;
				string	str_prodid		= m_tempTrayInfo.GetInfo().ProdInfo[index].ProdID;
				string	str_keyvalue	= m_tempTrayInfo.GetInfo().KeyValue;
				string	str_trayBarcode	= m_tempTrayInfo.GetInfo().TrayBarcode;

				if (status != ProdStatus.None)
				{
					if (m_tempTrayInfo.GetInfo().ProdInfo[index].Exist == true)
					{
						info			= m_tempTrayInfo.GetInfo().ProdInfo[index];
						info.ResultTop	= (int)status;
						info.ResultBtm	= (int)status;
					}
					else
					{
						info.Exist			= true;
						info.TrayRow		= x;
						info.TrayCol		= y;
						info.ResultTop		= (int)status;
						info.ResultBtm		= (int)status;
						info.VerifyTopDone	= false;
						info.VerifyBtmDone	= false;
						info.TrayID			= str_trayid;
						info.KeyValue		= str_keyvalue;
						info.ProdID			= string.Format("{0}_{1}_{2:000}_E", str_keyvalue, str_trayid, index+1);
						info.InspTimeTop	= string.Empty;
						info.InspTimeBtm	= string.Empty;
						info.SerialNumber	= string.Empty;
						info.TrayBarcode	= str_trayBarcode;
						info.PickStatus		= (int)ProdPickStatus.Normal;
						info.HandlingStatus = (int)ProdHandlingStatus.Normal;
					}
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
			return info;
		}

		private void ChangeExistTrayProduct(int nIndex)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)		return;

				frmListBoxSelect	ltbox		= new frmListBoxSelect();
				List<string>		ltlist		= new List<string>();

				
				ltlist = new List<string>() {"None", "Unknown"};
				
			

				ProdStatus status = m_tempTrayInfo.GetProdStatus(nIndex);

				if (ltbox.ShowDialog(ltlist, status.ToString()) == System.Windows.Forms.DialogResult.OK)
				{
					switch (ltbox.SelItem)
					{
						case "None":
						case "Unknown":
						case "OK":
						case "NG":
							status = (ProdStatus)Enum.Parse(typeof(ProdStatus), ltbox.SelItem);

							if (m_ltTrayResult.Count > 0)
							{
								//Product 다중 선택
								for (int i = 0; i < m_ltTrayResult.Count; i++)
								{
									m_tempTrayInfo.SetProd(m_ltTrayResult[i], SetTrayProductInfo(m_ltTrayResult[i], status));
									UI.Logs.Write(LOGPART.DATA, string.Format("UI Tray Pocket Flow Map Change. Mode: {0}, Flow: {1}, Index: {2}", ltbox.SelItem, m_flowtray.ToString(), m_ltTrayResult[i]));
								}
							}
							else
							{
								//Product 단일 선택
								m_tempTrayInfo.SetProd(nIndex, SetTrayProductInfo(nIndex, status));
								UI.Logs.Write(LOGPART.DATA, string.Format("UI Tray Pocket Flow Map Change. Mode: {0}, Flow: {1}, Index: {2}", ltbox.SelItem, m_flowtray.ToString(), nIndex));
							}
							break;
					}
					m_ltTrayResult.Clear();
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private stProdInfo SetPartProductInfo(int index, ProdStatus status)
		{
			stProdInfo info = new stProdInfo();
			try
			{
				string str_trayid = m_tempPartInfo.GetInfo().WorkOrder;
				string str_prodid = m_tempPartInfo.GetInfo().ProdInfo[index].ProdID;

				if (status != ProdStatus.None)
				{
					info = m_tempPartInfo.GetInfo().ProdInfo[index];

					info.Exist		= true;
					info.ResultTop	= (int)status;
					info.ResultBtm	= (int)status;
		
					if (string.IsNullOrWhiteSpace(info.TrayID) == true)		info.TrayID	= str_trayid;
					if (string.IsNullOrWhiteSpace(info.ProdID) == true)		info.ProdID	= str_prodid;
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
			return info;
		}

		private void ChangeExistPartProduct(int nIndex)
		{
			try
			{
				//if (SM.SHM.IsBitData(BIT.AutoRun) == true)			return;
				if (Program.m_PasswordMode == PW_MODE.OPER)			return;

				frmListBoxSelect	ltbox		= new frmListBoxSelect();
				List<string>		ltlist		= new List<string>();
		
				ltlist	= new List<string>() {"None", "Unknown"};	

				ProdStatus	status		= m_tempPartInfo.GetProdStatus(nIndex);
				string			str_trayid	= m_tempPartInfo.GetInfo().ProdInfo[nIndex].TrayID;
				string			str_prodid	= m_tempPartInfo.GetInfo().ProdInfo[nIndex].ProdID;
		
				if (ltbox.ShowDialog(ltlist, status.ToString()) == System.Windows.Forms.DialogResult.OK)
				{
					switch (ltbox.SelItem)
					{
						case "None":
						case "Unknown":
						case "OK":
						case "NG":
							status = (ProdStatus)Enum.Parse(typeof(ProdStatus), ltbox.SelItem);
					
							if (SM.FMP.IsFlow(m_flowpart) == false && ltbox.SelItem != "None")
							{
								SM.FMP.Next(FlowPart.Empty, m_flowpart);
		
								InfoPart.tsInfo part_info	= m_tempPartInfo.GetInfo();
								part_info.WorkOrder				= m_tempPartInfo.GetInfo().WorkOrder;
								
								SM.FMP[m_flowpart].SetInfo(part_info);
							}

							if (m_ltPartResult.Count > 0)
							{
								//Product 다중 선택
								for (int i = 0; i < m_ltPartResult.Count; i++)
								{
									m_tempPartInfo.SetProd(m_ltPartResult[i], SetPartProductInfo(m_ltPartResult[i], status));
									UI.Logs.Write(LOGPART.DATA, string.Format("UI Part Pocket Flow Map Change. Mode: {0}, Flow: {1}, Index: {2}", ltbox.SelItem, m_flowpart.ToString(), m_ltPartResult[i]));
								}
							}
							else
							{
								//Product 단일 선택
								m_tempPartInfo.SetProd(nIndex, SetPartProductInfo(nIndex, status));
								UI.Logs.Write(LOGPART.DATA, string.Format("UI Part Pocket Flow Map Change. Mode: {0}, Flow: {1}, Index: {2}", ltbox.SelItem, m_flowpart.ToString(), nIndex));
							}

							if (m_tempPartInfo.GetExistCount() == 0)		SM.FMP.Clear(m_flowpart);
							break;
						default:
							string		strmp		= ltbox.SelItem.Replace("Move ", "");
							FlowPart	flownext	= (FlowPart)Enum.Parse(typeof(FlowPart), strmp);
		
							if (SM.FMP.IsFlow(m_flowpart) == false)		return;
							if (SM.FMP.IsFlow(flownext) == true)		return;
		
							SM.FMP.Next(m_flowpart, flownext);
							break;
					}
					m_ltPartResult.Clear();
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}

		#endregion

		#endregion

		}

		private void BtnClose_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				SM.FMT.Clear(FlowTray.Tmp1);
				WinFormInstance.Close();
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void BtnDelete_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (m_ltTrayResult.Count > 0)
				{
					//Product 다중 선택
					for (int i = 0; i < m_ltTrayResult.Count; i++)
					{
						m_tempTrayInfo.SetProd(m_ltTrayResult[i], SetTrayProductInfo(m_ltTrayResult[i], ProdStatus.None));
						UI.Logs.Write(LOGPART.DATA, string.Format("UI Tray Pocket Flow Map Change. Mode: {0}, Flow: {1}, Index: {2}", "None", m_flowtray.ToString(), m_ltTrayResult[i]));
					}
					
					if (m_tempTrayInfo.IsEmpty() == true)
					{
						m_tempTrayInfo.ProcStatus = ProcStatus.TrayEmptyDone;
					}

					m_ltTrayResult.Clear();
					UpdateTrayFigure();
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void BtnExist_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (m_ltTrayResult.Count > 0)
				{
					//Product 다중 선택
					for (int i = 0; i < m_ltTrayResult.Count; i++)
					{
						m_tempTrayInfo.SetProd(m_ltTrayResult[i], SetTrayProductInfo(m_ltTrayResult[i], ProdStatus.Unknown));
						UI.Logs.Write(LOGPART.DATA, string.Format("UI Tray Pocket Flow Map Change. Mode: {0}, Flow: {1}, Index: {2}", "Unknown", m_flowtray.ToString(), m_ltTrayResult[i]));
					}

					m_ltTrayResult.Clear();
					UpdateTrayFigure();
				}
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int TrayEditCount = m_tempTrayInfo.GetExistCount() - SM.FMT[m_flowtray].GetExistCount() ;
				int ProdEditCount = m_lstDeleteTargetPart.Count;

				stRunLotInfo tempLotInfo = SM.SHM.GetRunLotInfo(SM.FMT[m_flowtray].GetInfo().KeyValue);
				int tempQty = int.Parse(tempLotInfo.ProdCount);
				
				tempLotInfo.ProdCount = (tempQty + TrayEditCount - ProdEditCount).ToString();
				// QTY 변경
				SM.SHM.SetRunLotInfo(tempLotInfo);
				
				
				if (SM.FMP[m_flowpartOnlyDeleteTarget].GetInfo().LastPieceOfLot == true)
				{
					SM.FMP[m_flowpartOnlyDeleteTarget].SetLastOfLot(false);
					if (SM.FMT.IsFlow(FlowTray.Reject) == true)
					{
						var t_info =	SM.FMT[FlowTray.Reject].GetInfo();
						t_info.KeyValue = "";
						SM.FMT[FlowTray.Reject].SetInfo(t_info);
					}
                    m_tempTrayInfo.SetLastOfLot(true);
				}
				
				stProdInfo		info			= new stProdInfo();

				info.Exist = false;
				for (int i = 0; i < m_lstDeleteTargetPart.Count; i++)
				{
					SM.FMP[m_flowpartOnlyDeleteTarget].SetProd(m_lstDeleteTargetPart[i], info);
				}

				SM.FMT[m_flowtray].SetInfo(m_tempTrayInfo.GetInfo().DeepCopy());
				
				SM.FMT.Clear(FlowTray.Tmp1);


				WinFormInstance.Close();
			}
			catch(Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
	}
}
