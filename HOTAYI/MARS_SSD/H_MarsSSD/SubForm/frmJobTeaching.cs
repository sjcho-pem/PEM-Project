using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	public partial class frmJobTeaching : Form
	{
		private int SeqNo = 0;
		private List<string>    lstText     = new List<string>();
		private List<Image>     lstImage    = new List<Image>();

		public ucJobTrayAni TrayAni = new ucJobTrayAni();

		private enum TeachingCase
		{
			LoadPlate1,
			LoadPlate2,
			UnloadPlate1,
			UnloadPlate2,
			Max,
		}

		private TeachingCase m_TeachingCase;

		public frmJobTeaching(string strManual)
		{
			InitializeComponent();

			//job 트레이 정보
			elementHost1.Child = TrayAni;

			m_TeachingCase = TeachingCase.Max;
			SetList();
			switch (strManual)
			{
				case "TeachingLoad1":   m_TeachingCase = TeachingCase.LoadPlate1; break;
				case "TeachingLoad2":   m_TeachingCase = TeachingCase.LoadPlate2; break;
				case "TeachingUnload1": m_TeachingCase = TeachingCase.UnloadPlate1; break;
				case "TeachingUnload2": m_TeachingCase = TeachingCase.UnloadPlate2; break;
			}
		}

		private void frmJobTeaching_Load(object sender, EventArgs e)
		{
			try
			{
				elementHost1.Visible = false;
				btnNext.Visible = true;
				lbText.Visible = true;
				SetList();
				if (m_TeachingCase == TeachingCase.LoadPlate1 || m_TeachingCase == TeachingCase.LoadPlate2)
				{
					SeqNo = 0;
				}
				else
				{
					SeqNo = 1;
				}
				
				
				lbText.Text = lstText[SeqNo];
				pb.Image = lstImage[SeqNo];
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		public void SetList()
		{
			switch (m_TeachingCase)
			{
				case TeachingCase.LoadPlate1:
				case TeachingCase.LoadPlate2:
					{
						lstImage.Clear();
						lstImage.Add(Properties.Resources.Layout_Job_Load_Check);
						lstImage.Add(Properties.Resources.Layout_withNumber);
						lstImage.Add(Properties.Resources.Layout_Job_Load);
						lstImage.Add(Properties.Resources.Layout_Job_Load);
						lstImage.Add(Properties.Resources.Layout_withNumber);
						lstImage.Add(Properties.Resources.Layout_withNumber);

						if (Program.m_Languageidx == (int)LAN_MODE.ENG)
						{
							lstText.Clear();
							lstText.Add("Step 1: Please check that the currently open Recipe is the same as the product you are trying to input.");
							lstText.Add("Step 2: Please Remove All materials.");
							lstText.Add("Step 3: Place the tray on the part shown in Figure (load table-1).");
							lstText.Add("Step 4: Please place the material in the load table as follows.");
							lstText.Add("Step 5: Do you want to start loading materials?");
						}
						else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
						{
							lstText.Clear();
							lstText.Add("Step 1: 현재 열려있는 Recipe가 투입하려는 제품과 동일한지 확인 해 주세요.");
							lstText.Add("Step 2: 자재를 제거해주세요.");
							lstText.Add("Step 3: 그림에 표시된 부분(로드 테이블-1 )에 트레이를 올려주세요.");
							lstText.Add("Step 4: 로드 테이블에 다음과 같은 위치에 자재를 넣어 주세요.");
							lstText.Add("Step 5: 자재 로드를 시작하시겠습니까?");
						}
					}
					break;
				case TeachingCase.UnloadPlate1:
				case TeachingCase.UnloadPlate2:
					{
						lstImage.Clear();
						lstImage.Add(Properties.Resources.Layout_withNumber);
						lstImage.Add(Properties.Resources.Layout_withNumber);
						lstImage.Add(Properties.Resources.Layout_Job_Unload);
						lstImage.Add(Properties.Resources.Layout_Job_Unload);
						lstImage.Add(Properties.Resources.Layout_withNumber);

						if (Program.m_Languageidx == (int)LAN_MODE.ENG)
						{
							lstText.Clear();
							lstText.Add("Step 1: Please Remove unnecessary materials and Tray.");
							lstText.Add("Step 1: Please Remove unnecessary materials and Tray.");
							lstText.Add("Step 2: Place the tray on the part shown in Figure (unload table-19).");
							lstText.Add("Step 3: Do you want to start unloading materials?");
							lstText.Add("Step 4: Remove Tray on the unload table1");
						}
						else if (Program.m_Languageidx == (int)LAN_MODE.KOR)
						{
							lstText.Clear();
							lstText.Add("Step 1: 불필요한 자재를 제거해주세요.");
							lstText.Add("Step 1: 불필요한 자재를 제거해주세요.");
							lstText.Add("Step 2: 그림에 표시된 부분(언로드 테이블-19)에 트레이를 올려주세요.");
							lstText.Add("Step 3: 자재 언로드를 시작하시겠습니까?");
							lstText.Add("Step 4: 트레이를 제거해주세요.");
						}
					}
					break;
				default:
					break;
			}
		}

		private bool MemoryDeleteCheck()
		{
			switch (m_TeachingCase)
			{
				case TeachingCase.LoadPlate1:
					{
						for (int i = 0; i < (int)FlowPart.Max; i++)
						{
							if (i == (int)FlowPart.Insp2)
								continue;
								
							if (SM.FMP.IsFlow((FlowPart)i) == true)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check Memory Exist");
								pMsg.ShowDialog();
								return false;
							}
						}

						if (SM.FMT.IsFlow(FlowTray.LDTable2) == true)
						{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Remove Tray in Load Table2");
								pMsg.ShowDialog();
								return false;
						}
					}
					break;
				case TeachingCase.LoadPlate2:
					{
						for (int i = 0; i < (int)FlowPart.Max; i++)
						{
							if (i == (int)FlowPart.Insp1)
								continue;
								
							if (SM.FMP.IsFlow((FlowPart)i) == true)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check Memory Exist");
								pMsg.ShowDialog();
								return false;
							}
						}

						if (SM.FMT.IsFlow(FlowTray.LDTable2) == true)
						{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Remove Tray in Load Table2");
								pMsg.ShowDialog();
								return false;
						}
					}
					break;
				case TeachingCase.UnloadPlate1:
					{
						for (int i = 0; i < (int)FlowPart.Max; i++)
						{
							if (i == (int)FlowPart.Insp1 || i == (int)FlowPart.Insp2)
								continue;
								
							if (SM.FMP.IsFlow((FlowPart)i) == true)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check Memory Exist");
								pMsg.ShowDialog();
								return false;
							}
						}

						if (SM.FMT.IsFlow(FlowTray.ULDTable2) == true)
						{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Remove Tray in Unload Table2");
								pMsg.ShowDialog();
								return false;
						}
					}
					break;
				case TeachingCase.UnloadPlate2:
					
					{
						for (int i = 0; i < (int)FlowPart.Max; i++)
						{
							if (i == (int)FlowPart.Insp1 || i == (int)FlowPart.Insp2)
								continue;
								
							if (SM.FMP.IsFlow((FlowPart)i) == true)
							{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check Memory Exist");
								pMsg.ShowDialog();
								return false;
							}
						}

						if (SM.FMT.IsFlow(FlowTray.ULDTable2) == true)
						{
								frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Remove Tray in Unload Table2");
								pMsg.ShowDialog();
								return false;
						}
					}
					break;
				default:
					break;
			}

			return true;
		}		

		private bool TrayLoadCheck()
		{
			switch (m_TeachingCase)
			{
				case TeachingCase.LoadPlate1:
				case TeachingCase.LoadPlate2:
					{
						if (G.seqLoadTrayTable2.LoadTable1UpDnSafety() == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Check Load table2 State. Please perform a All home and try again.");
							pMsg.ShowDialog();
							return false;
						}
						if (G.seqLoadTrayTable1.IsUpState() == false)
						{
							SM.SHM.SetManualNum(MANUAL.LOADTABLE1_UP);

							return false;
						}

						if (G.DryRunIsBIn(DI.LDTrayTable1ExistSen, true) == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Put the tray on the load table1.");
							pMsg.ShowDialog();
							return false;
						}
					}
					break;
				case TeachingCase.UnloadPlate1:
				case TeachingCase.UnloadPlate2:
					{
						if (G.seqUnloadTrayTable1.IsUpState() == false)
						{
							SM.SHM.SetManualNum(MANUAL.UNLOADTABLE1_UP);

							return false;
						}

						if (G.DryRunIsBIn(DI.ULDTrayTable1ExistSen, true) == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Put the tray on the Unload table1.");
							pMsg.ShowDialog();
							return false;
						}
					}
					break;
				default:
					break;
			}
			
			return true;

		}
		
		private bool TrayUnloadCheck()
		{
			
			switch (m_TeachingCase)
			{
				case TeachingCase.LoadPlate1:
				case TeachingCase.LoadPlate2:
					{
						return true;
					}
				case TeachingCase.UnloadPlate1:
				case TeachingCase.UnloadPlate2:
					{
						if (G.DryRunIsBIn(DI.ULDTrayTable1ExistSen, false) == true)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Remove the tray on the Unload table1.");
							pMsg.ShowDialog();
							return false;
						}
					}
					break;
				default:
					break;
			}
			
			return true;

		}

		private bool RunStart()
        {
            for (int i = 0; i < (int)MT.TOTAL; i++)
            {
                if (G.cKernel.Motor((MT)i).IsHomeDone() == false)
                {
					switch (m_TeachingCase)
					{
						case TeachingCase.LoadPlate1:
							if ( i == (int)MT.InspPlate2Grip  ||  i == (int)MT.InspPlate2Y)			continue;
							break;;
						case TeachingCase.LoadPlate2:
							if (i == (int)MT.InspPlate1W  ||  i == (int)MT.InspPlate1Y)			continue;
							break;;
					}
                    frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, string.Format("Check Home State\nAXIS : {0}", ((MT)i).ToString()));
                    pMsg.ShowDialog();
                    return true;
                }
            }

			if (G.seqJobTeaching.IsReady == false)
			{
				frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run Manual Job Teaching Seq");
				pMsg.ShowDialog();
				return false;
			}
			
			if (SM.SHM.IsBitData(BIT.Pause) == true || SM.SHM.IsBitData(BIT.AutoRun) == true)
			{
				frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please stop the machine and try again.");
				pMsg.ShowDialog();
				return false;
			}

			if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true ||
				SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true )
			{
				frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check UseSkip State of Inspection Debug Mode.");
				pMsg.ShowDialog();
				return false;
			}

			for (UsePartName part_name = 0; part_name < UsePartName.Max; part_name++)
			{
				if(SM.SHM.GetUsePart(part_name) != UsePartMode.Enable )
				{
					frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check Use Part State. Please Change All Part to Enable ");
					pMsg.ShowDialog();
					return false;
				}
			}
			
			switch (m_TeachingCase)
			{
				case TeachingCase.LoadPlate1:
					{
						if (G.seqLoadPnP.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run Load PnP");
							pMsg.ShowDialog();
							return false;
						}
						if (G.seqLoadBuffer1.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run LoadBuffer1");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqLoadInspPnP.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run LoadInspPnP");
							pMsg.ShowDialog();
							return false;
						}

						if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Uncheck UseSkip State of InspDebug");
							pMsg.ShowDialog();
							return false;
						}

						if (SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Load) == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check UseSkip State of Plate1");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqInspPlate1.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqInspPlate1");
							pMsg.ShowDialog();
							return false;
						}
						if (SM.FMP.IsFlow(FlowPart.Insp1) == true  ||
						   G.cKernel.IsAIn(DI.InspPlate1ExistSen1) == true  ||
						   G.cKernel.IsAIn(DI.InspPlate1ExistSen2) == true  ||
						   G.cKernel.IsAIn(DI.InspPlate1ExistSen3) == true  ||
						   G.cKernel.IsAIn(DI.InspPlate1ExistSen4) == true)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Remove product on the seqInspPlate1");
							pMsg.ShowDialog();
							return false;
						}
					}
					break;
				case TeachingCase.LoadPlate2:
					{
						
						if (G.seqLoadPnP.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run Load PnP");
							pMsg.ShowDialog();
							return false;
						}
						if (G.seqLoadBuffer1.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run LoadBuffer1");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqLoadInspPnP.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run LoadInspPnP");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqInspPlate2.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqInspPlate2");
							pMsg.ShowDialog();
							return false;
						}
						
						if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Uncheck UseSkip State of InspDebug");
							pMsg.ShowDialog();
							return false;
						}
						
						if (SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Load) == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check UseSkip State of Plate2");
							pMsg.ShowDialog();
							return false;
						}
						
						if (SM.FMP.IsFlow(FlowPart.Insp2) == true)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please Remove product on the seqInspPlate2");
							pMsg.ShowDialog();
							return false;
						}
					}
					break;
				case TeachingCase.UnloadPlate1:
					{
						if (G.seqInspPlate1.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqInspPlate1");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqUnloadBuffer.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqUnloadBuffer");
							pMsg.ShowDialog();
							return false;
						}

						
						if (SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Unload) == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check UseSkip State of Plate1");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqUnloadPnP.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqUnloadPnP");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqUnloadInspPnP.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqUnloadInspPnP");
							pMsg.ShowDialog();
							return false;
						}
						
						if (SM.FMP.IsFlow(FlowPart.Insp1) == false )
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "seqInspPlate1 don't Exist Product");
							pMsg.ShowDialog();
							return false;
						}
					}
					break;
				case TeachingCase.UnloadPlate2: 
					{
						if (G.seqInspPlate2.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqInspPlate2");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqUnloadBuffer.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqUnloadBuffer");
							pMsg.ShowDialog();
							return false;
						}
						
						if (SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Unload) == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Check UseSkip State of Plate2");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqUnloadPnP.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqUnloadPnP");
							pMsg.ShowDialog();
							return false;
						}

						if (G.seqUnloadInspPnP.IsReady == false)
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run seqUnloadInspPnP");
							pMsg.ShowDialog();
							return false;
						}
						if (SM.FMP.IsFlow(FlowPart.Insp2) == false )
						{
							frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "seqInspPlate2 don't Exist Product");
							pMsg.ShowDialog();
							return false;
						}
					}
					break;
			}
			
			switch (m_TeachingCase)
			{
				case TeachingCase.LoadPlate1:   G.seqJobTeaching.TeachingLoadStart(0);    break;
				case TeachingCase.LoadPlate2:   G.seqJobTeaching.TeachingLoadStart(1);    break;
				case TeachingCase.UnloadPlate1: G.seqJobTeaching.TeachingUnloadStart(0);    break;
				case TeachingCase.UnloadPlate2: G.seqJobTeaching.TeachingUnloadStart(1);    break;
			}
			
			return true;
		}
	   
		private void btnNext_Click(object sender, EventArgs e)
		{
			try
			{
				if (G.seqJobTeaching.IsReady == false)
				{
					frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Already Run Manual Job Teaching Seq");
					pMsg.ShowDialog();
					return;
				}

				tmrTableUp.Stop();

				if (m_TeachingCase == TeachingCase.LoadPlate1  &&  G.seqLoadTrayTable1.IsUpState() == false)
				{
                    if (G.seqLoadTrayTable2.LoadTable1UpDnSafety() == false ||
                        G.seqLoadTransfer.LoadTableUpDnSafety(0) == false)
                    {
                        frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please try again after performing all home actions.");
                        pMsg.ShowDialog();
                        return;
                    }
					SM.SHM.SetManualNum(MANUAL.LOADTABLE1_UP);
				}

				//if (m_TeachingCase == TeachingCase.LoadPlate2  &&  G.seqLoadTrayTable2.IsUpState() == false)
				//{
				//	SM.SHM.SetManualNum(MANUAL.LOADTABLE2_UP);
				//}

				if (m_TeachingCase == TeachingCase.UnloadPlate1  &&  G.seqUnloadTrayTable1.IsUpState() == false)
                {
                    if (G.seqUnloadTransfer.UnloadTableUpDnSafety(0) == false ||
                        G.seqLoadTransfer.UnloadTableUpDnSafety() == false)
                    {
                        frmMessageBox pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, Language.languageDefault.MB_Warnning, "Please try again after performing all home actions.");
                        pMsg.ShowDialog();
                        return;
                    }
                    SM.SHM.SetManualNum(MANUAL.UNLOADTABLE1_UP);
				}

				//if (m_TeachingCase == TeachingCase.UnloadPlate1  &&  G.seqUnloadTrayTable2.IsUpState() == false)
				//{
				//	SM.SHM.SetManualNum(MANUAL.UNLOADTABLE2_UP);
				//}

				SeqNo++;
				bool bResult = false;
				switch (SeqNo)
				{
					case 1:
						{
							bResult = true;
						}
						break;
					case 2:
						{
							bResult = MemoryDeleteCheck();
						}
						break;
					case 3:
						{
							bResult = TrayLoadCheck();

							if (bResult == true)
							{
								pb.Visible = false;
								elementHost1.Visible = true;
								if (m_TeachingCase == TeachingCase.LoadPlate1  ||  m_TeachingCase == TeachingCase.LoadPlate2)
									TrayAni.UpdateTray(System.Windows.Media.Colors.Yellow);
								else
									TrayAni.UpdateTray(System.Windows.Media.Colors.White);
							}
						}
						break;
					case 4:
						{
							elementHost1.Visible = false;
							pb.Visible = true;

							if (m_TeachingCase == TeachingCase.UnloadPlate1  ||  m_TeachingCase == TeachingCase.UnloadPlate2)
								bResult = RunStart();
							else
								bResult= true;
							
						}
						break;
					case 5:
						{						
							if (m_TeachingCase == TeachingCase.LoadPlate1  ||  m_TeachingCase == TeachingCase.LoadPlate2)
							{
								bResult = RunStart();
								this.Close();
								return;
							}
							else
							{
								bResult = TrayUnloadCheck();
								this.Close();
								return;
							}
						}
					default:
						bResult = true;
						break;
				}

				if (bResult == false)
				{
					SeqNo--;
					return;
				}
				lbText.Text		= lstText[(int)SeqNo];
				pb.Image		= lstImage[(int)SeqNo];
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}

		}

		private void frmJobTeaching_VisibleChanged(object sender, EventArgs e)
		{
			if (this.Visible == true)
			{
				tmrTableUp.Start();
			}
			else
			{
				tmrTableUp.Stop();
			}
		}

		private void tmrTableUp_Tick(object sender, EventArgs e)
		{
			if (SM.SHM.IsManualRunning() == false)			SM.SHM.StartManual();

			//SM.SHM.SetManualNum(MANUAL.ALL_TABLE_UP);

			//if (G.seqLoadTrayTable1.IsUpState() == true  &&
			//	G.seqLoadTrayTable2.IsUpState() == true  &&
			//	G.seqUnloadTrayTable1.IsUpState() == true  &&
			//	G.seqUnloadTrayTable2.IsUpState() == true)
			//{
			//	tmrTableUp.Stop();
			//}
		}
	}
}
