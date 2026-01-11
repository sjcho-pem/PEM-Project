using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static H_MarsSSD.InfoTray;

namespace H_MarsSSD
{
	public partial class frmFlowMapInfo : Form
	{
		#region Variable
		private object				m_lockProdListAdd	= new object();
		private	DataGridView		m_SelGridTray;
		private	DataGridView		m_SelGridPart;
		private	bool				m_bLoaded			= false;
		private	bool				m_bSetValueUpdated	= true;
		Dictionary<string, Type>	m_enumTypeMap = new Dictionary<string, Type>
		{
			{ "ProcStatus", typeof(ProcStatus) },
			//{ "ModuleType", typeof(ModuleType) },
			//{ "ModuleGen", typeof(ModuleGeneration) },
			{ "AutoRunMode", typeof(AutoRunMode) },
			{ "ResultTop", typeof(ProdStatus) },
			{ "ResultBtm", typeof(ProdStatus) },
			{ "ResultMes", typeof(MESStatus) },
			{ "PickStatus", typeof(ProdPickStatus) }
		};

		#endregion

		public			frmFlowMapInfo()
		{
			InitializeComponent();
		}

		private	void	frmFlowMapInfo_Load(object sender, EventArgs e)
		{
			try
			{
				Init();

				dgvValue_CellClick(dgvTrayList, null);
				dgvValue_CellClick(dgvPartList, null);

				tmrUpdateInfo.Start();

				m_bLoaded	= true;

				m_SelGridTray	= dgvTrayList;
				m_SelGridPart	= dgvPartList;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	Init()
		{
			try
			{
				SetLanguageText();
				//Tab1 : 트레이 정보
				//dgvTrayList  				트레이 리스트
				//dgvTrayValue  			트레이 정보
				//dgvTrayProdList  			프로드 리스트 : UpdateProdList에서 트레이,파트 선택에 따라 매번 초기화
				//dgvTraySelectSSDValue  	프로드 정보
				dgvTrayList.Rows.Clear();
				for (int i = 0; i < (int)FlowTray.Max; i++)
				{
					dgvTrayList.Rows.Add(i, (FlowTray)i, SM.FMT.IsFlow((FlowTray)i));
				}
				
				InitDataGridView(typeof(InfoTray.tsInfo),dgvTrayValue );
				InitDataGridView(typeof(stProdInfo),dgvTraySelectProdValue );
				
				//Tab2 : 파트 정보
				//dgvPartList  				파트 리스트
				//dgvPartValue 				파트
				//dgvPartProdList 			프로드 리스트 : UpdateProdList에서 트레이,파트 선택에 따라 매번 초기화
				//dgvPartSelectProdValue  	프로드

				dgvPartList.Rows.Clear();
				for (int i = 0; i < (int)FlowPart.Max; i++)
				{
					dgvPartList.Rows.Add(i, (FlowPart)i, SM.FMP.IsFlow((FlowPart)i));
				}
				
				InitDataGridView(typeof(InfoPart.tsInfo),dgvPartValue );
				InitDataGridView(typeof(stProdInfo),dgvPartSelectProdValue );

				UpdateSetValue();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	InitDataGridView(Type structType, DataGridView dgv) 
		{
			dgv.Rows.Clear();
			var fields = structType.GetFields();
			foreach (var field in fields)
			{
				// ProdInfo 필드를 제외
				if (field.Name == "ProdInfo")	continue;
				// 필드 이름을 가져옴
				string fieldName = field.Name;
				// DataGridView에 추가 (필드 이름 삽입)
				dgv.Rows.Add(dgv.Rows.Count + 1, fieldName, "", "");
			}
		}
		
		private	void	SetLanguageText()
		{
			try
			{
				chkTrayModify.Text		= Language.languageDefault.CHK_Modify;
				btnTrayClearAll.Text	= Language.languageDefault.BTN_ClearAll;
				btnTrayClear.Text		= Language.languageDefault.BTN_Clear;
				btnTrayMove.Text		= Language.languageDefault.BTN_Move;
				btnTrayMake.Text		= Language.languageDefault.BTN_Make;
				btnTraySet.Text			= Language.languageDefault.BTN_Set;

				chkPartModify.Text		= Language.languageDefault.CHK_Modify;
				btnPartClearAll.Text	= Language.languageDefault.BTN_ClearAll;
				btnPartClear.Text		= Language.languageDefault.BTN_Clear;
				btnPartMove.Text		= Language.languageDefault.BTN_Move;
				btnPartMake.Text		= Language.languageDefault.BTN_Make;
				btnPartSet.Text			= Language.languageDefault.BTN_Set;

                btnClose.Text			= Language.languageDefault.BTN_Close;

				lbPartInfo.Text			= Language.languageDefault.LB_PartInfo;
				lbPartList.Text			= Language.languageDefault.LB_PartList;
				lbTrayInfo.Text			= Language.languageDefault.LB_TrayInfo;
				lbTrayList.Text			= Language.languageDefault.LB_TrayList;
				lbProdInfo_InPart.Text	= Language.languageDefault.LB_ProdInfo;
				lbProdInfo_InTray.Text	= Language.languageDefault.LB_ProdInfo;
				lbProdList_InPart.Text	= Language.languageDefault.LB_ProdList;
				lbProdList_InTray.Text	= Language.languageDefault.LB_ProdList;

				chkAllFlowMapModify.Text	= Language.languageDefault.CHK_Modify;
				btnAllFlowMapClear.Text		= Language.languageDefault.BtnAllFlowMapClear;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#region Event
		private void	tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{
				UpdateSetValue();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private	void	btn_Click(object sender, EventArgs e)
		{
			try
			{
				Button	btn = (Button)sender;

				frmMessageBox		pMsg;
				frmSelectMessageBox	pSelMsg;
				frmComboBoxSelect	popup;

				FlowTray		flow_t	= FlowTray.Empty;
				FlowPart		flow_p	= FlowPart.Empty;
				FlowTray		flow_r	= FlowTray.Reject;

				if (dgvTrayList.SelectedRows.Count > 0)			flow_t	= (FlowTray)dgvTrayList.SelectedRows[0].Index;
				if (dgvPartList.SelectedRows.Count > 0)			flow_p	= (FlowPart)dgvPartList.SelectedRows[0].Index;


				UI.Logs.Write(LOGPART.DATA, string.Format("{0} Button Click", btn.Name));

				switch (btn.Name)
				{
					#region [ Tray ]
					case "btnTrayClearAll":
						pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_AllDelete);
						if (pMsg.ShowDialog() == DialogResult.Cancel)			return;
					
						SM.FMT.FlowClearAll();
						SM.SHM.ClearRunLotInfo();
						NDm.Write(eNDM.FillDoneRemainCount, 0);
						
						break;

					case "btnTrayClear":
						if (m_SelGridTray.SelectedRows.Count < 1)			return;

						if (m_SelGridTray == dgvTrayList)
						{
							pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_Delete);
							if (pMsg.ShowDialog() == DialogResult.Cancel)				return;

							foreach (DataGridViewRow row in m_SelGridTray.SelectedRows)
							{
								if (row.Index > 0  &&  SM.FMT.IsFlow((FlowTray)row.Index) == false)			continue;
								
								SM.FMT.Clear((FlowTray)row.Index);

								UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}", btn.Name, ((FlowTray)row.Index).ToString()));
							}

						}

						if (m_SelGridTray == dgvTrayProdList)
						{
							if (SM.FMT.IsFlow(flow_t) == false)			return;

							pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_Delete);
							if (pMsg.ShowDialog() == DialogResult.Cancel)				return;

							foreach (DataGridViewRow row in m_SelGridTray.SelectedRows)
							{
								if (SM.FMT[flow_t].GetProdStatus(row.Index) == ProdStatus.None)			continue;

								SM.FMT[flow_t].ClearProd(row.Index);

								UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}, Index: {2}", btn.Name, flow_t.ToString(), row.Index));
							}
						}

						break;

					case "btnTrayMove":
						if (m_SelGridTray != dgvTrayList)					return;
						if (m_SelGridTray.SelectedRows.Count != 1)			return;
						if (SM.FMT.IsFlow(flow_t) == false)					return;

						popup = new frmComboBoxSelect();
						if (popup.ShowDialog(Enum.GetNames(typeof(FlowTray)), ComboBoxStyle.DropDown, 0, true) == DialogResult.Cancel)			return;
						if (popup.SelNo == 0)			return;

						pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\n{1} => {2}", Language.languageDefault.MB_Move, flow_t, (FlowTray)popup.SelNo));
						if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

						SM.FMT.Next(flow_t, (FlowTray)popup.SelNo);

						UI.Logs.Write(LOGPART.DATA, string.Format("{0}, {1} -> {2}", btn.Name, flow_t.ToString(), ((FlowTray)popup.SelNo).ToString()));
						break;

					case "btnTrayMake":
						if (m_SelGridTray.SelectedRows.Count < 1)			return;

						List<string>	lt_btn;

						if (m_SelGridTray == dgvTrayList)
						{
							lt_btn	= new List<string> { "Cancel", "Random" };
							
							pSelMsg	= new frmSelectMessageBox(lt_btn, Language.languageDefault.MB_Question, Language.languageDefault.MB_Make, 0);

							pSelMsg.ShowDialog();

							if		(pSelMsg.Result == "Cancel")
							{
								return;
							}
							else if (pSelMsg.Result == "Random")
							{
								int	nrow_cnt	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
								int	ncol_cnt	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;

								foreach (DataGridViewRow row in m_SelGridTray.SelectedRows)
								{
									if (row.Index == 0)											continue;
									if (SM.FMT.IsFlow((FlowTray)row.Index) == true)			continue;

									SM.FMT.Next(FlowTray.Empty, (FlowTray)row.Index);

									tsInfo	ts			= SM.FMT[(FlowTray)row.Index].GetInfo();
									ts.WorkOrder		= SM.SHM.GetRunLotInfo(0).WorkOrder;
									ts.KeyValue			= SM.SHM.GetRunLotInfo(0).KeyValue;
									ts.ProgramID_Top	= SM.SHM.GetRunLotInfo(0).ProgramID_Top;
									ts.ProgramID_Btm	= SM.SHM.GetRunLotInfo(0).ProgramID_Btm;
									ts.TrayID			= string.Format("{0}{1:00}", ts.WorkOrder, row.Index);
									ts.TrayBarcode		="tempTrayBarcode";
									ts.RowCnt			= nrow_cnt;
									ts.ColCnt			= ncol_cnt;
									ts.TrayNo			= -1;
									ts.ModuleType		= SM.SHM.GetRecipeData(DEVICE.ModuleType);
									ts.MaxCntProdInTray	= nrow_cnt * ncol_cnt;
									SM.FMT[(FlowTray)row.Index].SetInfo(ts);

									UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}, Tray Random", btn.Name, ((FlowTray)row.Index).ToString()));
								}
							}
						}

						if (m_SelGridTray == dgvTrayProdList)
						{
							if (SM.FMT.IsFlow(flow_t) == false)			return;

							lt_btn	= new List<string> { "Cancel", "Random" };
							
							pSelMsg	= new frmSelectMessageBox(lt_btn, Language.languageDefault.MB_Question, Language.languageDefault.MB_Make, 0);

							pSelMsg.ShowDialog();

							if		(pSelMsg.Result == "Cancel")
							{
								return;
							}
							else if (pSelMsg.Result == "Random")
							{						
								stProdInfo[]	ts	= SM.FMT[flow_t].GetInfo().ProdInfo;
								int ncol_cnt		= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
								foreach (DataGridViewRow row in m_SelGridTray.SelectedRows)
								{
									if (SM.FMT[flow_t].GetProdStatus(row.Index) != ProdStatus.None)			continue;
									
									ts[row.Index].TrayCol	= row.Index / ncol_cnt; // 1: col
									ts[row.Index].TrayRow	= row.Index % ncol_cnt; // 0: row
									ts[row.Index].TrayID	= SM.FMT[flow_t].GetInfo().TrayID;
									ts[row.Index].ProdID	= string.Format("{0}{1:00}",  DateTime.Now.Ticks, row.Index);
									ts[row.Index].KeyValue	= string.Format("{0}{1:00}",  DateTime.Now.Ticks, row.Index);
									ts[row.Index].Exist		= true;
									ts[row.Index].PickStatus= (int)ProdPickStatus.Normal;
							
									SM.FMT[flow_t].SetProd(row.Index, ts[row.Index]);
							
									UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}, Index: {2}, Product Random", btn.Name, flow_t, row.Index));
								}
							}
						}

						break;

					case "btnTraySet":
						if (m_SelGridTray.SelectedCells.Count != 1)			return;
						if (SM.FMT.IsFlow(flow_t) == false)					return;

						pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\n{1}", Language.languageDefault.MB_SetValue, flow_t));
						if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

						if (m_SelGridTray == dgvTrayValue)
						{
							SetValue(dgvTrayValue, dgvTrayValue.CurrentCell.RowIndex);
							UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}", btn.Name, flow_t.ToString()));
						}

						if (m_SelGridTray == dgvTraySelectProdValue)
						{
							SetValue(dgvTraySelectProdValue, dgvTraySelectProdValue.CurrentCell.RowIndex);
							UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}, Index: {2}", btn.Name, flow_t.ToString(), dgvTrayProdList.SelectedRows[0].Index));
						}

						break;
					#endregion

					#region [ PART ]
					case "btnPartClearAll":
						pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_AllDelete);
						if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

						SM.FMP.FlowClearAll();
						break;

					case "btnPartClear":
						if (dgvPartList.SelectedRows.Count < 1)			return;

						if (m_SelGridPart == dgvPartList)
						{
							pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation,  Language.languageDefault.MB_Delete);
							if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

							foreach (DataGridViewRow row in m_SelGridPart.SelectedRows)
							{
								if (row.Index > 0  &&  SM.FMP.IsFlow((FlowPart)row.Index) == false)			continue;

								SM.FMP.Clear((FlowPart)row.Index);
								UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}", btn.Name, ((FlowPart)row.Index).ToString()));
							}
						}

						if (m_SelGridPart == dgvPartProdList)
						{
							if (SM.FMP.IsFlow(flow_p) == false)			return;
							
							pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation,  Language.languageDefault.MB_Delete);
							if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

							foreach (DataGridViewRow row in m_SelGridPart.SelectedRows)
							{
								if (SM.FMP[flow_p].GetProdStatus(row.Index) == ProdStatus.None)			continue;

								SM.FMP[flow_p].ClearProd(row.Index);
								UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}, Index: {2}", btn.Name, flow_p, row.Index));
							}
						}

						break;

					case "btnPartMove":
						if (dgvPartList.SelectedRows.Count != 1)			return;
						if (SM.FMP.IsFlow(flow_p) == false)					return;

						popup = new frmComboBoxSelect();
						if (popup.ShowDialog(Enum.GetNames(typeof(FlowPart)), 0, true) == DialogResult.Cancel)			return;
						if (popup.SelNo == 0)			return;

						pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\n{1} => {2}", Language.languageDefault.MB_Move, flow_p, (FlowPart)popup.SelNo));
						if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

						SM.FMP.Next(flow_p, (FlowPart)popup.SelNo);

						UI.Logs.Write(LOGPART.DATA, string.Format("{0}, {1} -> {2}", btn.Name, flow_p.ToString(), ((FlowPart)popup.SelNo).ToString()));
						break;

					case "btnPartMake":
						if (dgvPartList.SelectedRows.Count < 1)			return;

						if (m_SelGridPart == dgvPartList)
						{
							lt_btn	= new List<string> { "Cancel", "Random" };
							pSelMsg	= new frmSelectMessageBox(lt_btn, Language.languageDefault.MB_Question, Language.languageDefault.MB_Make, 0);

							pSelMsg.ShowDialog();

							if (pSelMsg.Result == "Cancel")			return;

							foreach (DataGridViewRow row in m_SelGridPart.SelectedRows)
							{
								if (row.Index == 0)											continue;
								if (SM.FMP.IsFlow((FlowPart)row.Index) == true)			continue;

								SM.FMP.Next(FlowPart.Empty, (FlowPart)row.Index);

								InfoPart.tsInfo	ts	= SM.FMP[(FlowPart)row.Index].GetInfo();
								ts.WorkOrder		= SM.SHM.GetRunLotInfo(0).WorkOrder;
								ts.ProgramID_Top	= SM.SHM.GetRunLotInfo(0).ProgramID_Top;
								ts.ProgramID_Btm	= SM.SHM.GetRunLotInfo(0).ProgramID_Btm;
								SM.FMP[(FlowPart)row.Index].SetInfo(ts);
							}
						}

						if (m_SelGridPart == dgvPartProdList)
						{
							if (SM.FMP.IsFlow(flow_p) == false)			return;

						//	if (m_bMesDataLoaded == true)			lt_btn	= new List<string> { "Cancel", "Random", "From File" };
						//	else									lt_btn	= new List<string> { "Cancel", "Random" };
							lt_btn	= new List<string> { "Cancel", "Random" };
							
							pSelMsg	= new frmSelectMessageBox(lt_btn, Language.languageDefault.MB_Question, Language.languageDefault.MB_Make, 0);

							pSelMsg.ShowDialog();

							if		(pSelMsg.Result == "Cancel")
							{
								return;
							}
							else if (pSelMsg.Result == "Random")
							{
								foreach (DataGridViewRow row in m_SelGridPart.SelectedRows)
								{
									InfoPart.tsInfo info = SM.FMP[flow_p].GetInfo();
									stProdInfo prod_info = info.ProdInfo[row.Index];
									if (SM.FMP[flow_p].GetProdStatus(row.Index) != ProdStatus.None)			continue;
								
									prod_info.TrayCol	= row.Index;
									prod_info.TrayRow	= 0;
									prod_info.ProdID	= string.Format("{0}{1:00}",  DateTime.Now.Ticks, row.Index);
									prod_info.Exist		= true;
									prod_info.PickStatus= (int)ProdPickStatus.Normal;
									SM.FMP[flow_p].SetProd(row.Index, prod_info);

									UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}, Index: {2}, Random", btn.Name, flow_p, row.Index));
								}
							}
						}

						break;

					case "btnPartSet":
						if (dgvPartList.SelectedRows.Count < 1)				return;
						if (m_SelGridPart.SelectedCells.Count != 1)			return;
						if (SM.FMP.IsFlow(flow_p) == false)					return;

						pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, string.Format("{0}\n{1}", Language.languageDefault.MB_SetValue, flow_p));
						if (pMsg.ShowDialog() == DialogResult.Cancel)			return;

						if (m_SelGridPart == dgvPartValue)
						{
							SetValue(dgvPartValue, dgvPartValue.CurrentCell.RowIndex);
							UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}", btn.Name, flow_p.ToString()));
						}

						if (m_SelGridPart == dgvPartSelectProdValue)
						{
							SetValue(dgvPartSelectProdValue, dgvPartSelectProdValue.CurrentCell.RowIndex);
							UI.Logs.Write(LOGPART.DATA, string.Format("{0}, Flow: {1}, Index: {2}", btn.Name, flow_p.ToString(), dgvPartProdList.SelectedRows[0].Index));
						}
					
						break;
						#endregion

				}

				UpdateSetValue();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnAllFlowMapClear_Click(object sender, EventArgs e)
		{
			try
			{
				frmMessageBox		pMsg;

				UI.Logs.Write(LOGPART.DATA, "btnAllFlowMapClear Button Click");

				pMsg = new frmMessageBox(frmMessageBox.teButtons.OKCancel, Language.languageDefault.MB_Confirmation, Language.languageDefault.MB_AllDelete);
				if (pMsg.ShowDialog() == DialogResult.Cancel)			return;
					
				SM.FMT.FlowClearAll();
				SM.FMP.FlowClearAll();
				SM.FMT[FlowTray.Reject].ClearProdAll();

				SM.SHM.ClearRunLotInfo();
				NDm.Write(eNDM.FillDoneRemainCount, 0);

				UpdateSetValue();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	btnClose_Click(object sender, EventArgs e)
		{
			try
			{
				this.Close();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	chkModify_CheckedChanged(object sender, EventArgs e)
		{
			try
			{
				CheckBox	chk			= (CheckBox)sender;
				bool		bchecked	= chk.Checked;

				switch (chk.Name)
				{
					case "chkTrayModify"	:
						btnTrayClearAll.Enabled		= bchecked;
						btnTrayClear.Enabled		= bchecked;
						btnTrayMove.Enabled			= bchecked;
						btnTrayMake.Enabled			= bchecked;
						btnTraySet.Enabled			= bchecked;
						break;

					case "chkPartModify"	:
						btnPartClearAll.Enabled		= bchecked;
						btnPartClear.Enabled		= bchecked;
						btnPartMove.Enabled			= bchecked;
						btnPartMake.Enabled			= bchecked;
						btnPartSet.Enabled			= bchecked;
						break;

					case "chkAllFlowMapModify"	:
						btnAllFlowMapClear.Enabled	= bchecked;
						break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		#endregion

		#region DataGridView Event
		
		private void	dgvValue_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				DataGridView	dgv	= (DataGridView)sender;

				SelEditGrid(dgv);

				switch (dgv.Name)
				{
					case "dgvTrayValue"				:	SetValueInput(e.RowIndex, e.ColumnIndex,typeof(InfoTray.tsInfo),	dgvTrayValue);				break;
					case "dgvPartValue"				:	SetValueInput(e.RowIndex, e.ColumnIndex,typeof(InfoPart.tsInfo),	dgvPartValue);				break;
					case "dgvTraySelectProdValue"	:	SetValueInput(e.RowIndex, e.ColumnIndex,typeof(stProdInfo),		dgvTraySelectProdValue);		break;
					case "dgvPartSelectProdValue"	:	SetValueInput(e.RowIndex, e.ColumnIndex,typeof(stProdInfo),		dgvPartSelectProdValue);		break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	dgv_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				SelEditGrid((DataGridView)sender);

				UpdateSetValue();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	dgvValue_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				SelEditGrid((DataGridView)sender);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	dgvValue_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				DataGridView	dgv	= (DataGridView)sender;

				switch (e.KeyCode)
				{
					case Keys.F2 :
						int nrow	= dgv.SelectedCells[0].RowIndex;
						int	ncol	= dgv.SelectedCells[0].ColumnIndex;
						switch (dgv.Name)
						{
							case "dgvTrayValue"				:	SetValueInput(nrow, ncol,typeof(InfoTray.tsInfo),	dgvTrayValue);				break;
							case "dgvPartValue"				:	SetValueInput(nrow, ncol,typeof(InfoPart.tsInfo),	dgvPartValue);				break;
							case "dgvTraySelectProdValue"	:	SetValueInput(nrow, ncol,typeof(stProdInfo),		dgvTraySelectProdValue);		break;
							case "dgvPartSelectProdValue"	:	SetValueInput(nrow, ncol,typeof(stProdInfo),		dgvPartSelectProdValue);		break;
						}

						break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private	void	SelEditGrid(DataGridView dgv)
		{
			try
			{
				if (m_bLoaded == false)			return;

				int	row	= 0;

				switch (dgv.Name)
				{
					case "dgvTrayList"	:
						if (dgvTrayList.Focused == false)			return;

						dgvTrayProdList.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvTrayValue.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvTraySelectProdValue.DefaultCellStyle.SelectionBackColor	= Color.LightSkyBlue;
						dgvTrayList.DefaultCellStyle.SelectionBackColor				= SystemColors.Highlight;
						m_SelGridTray	= dgv;
						break;

					case "dgvTrayValue"	:
						if (dgvTrayValue.Focused == false)			return;

						if (dgvTrayList.SelectedRows.Count > 1)
						{
							row	= dgvTrayList.SelectedRows[0].Index;
							dgvTrayList.ClearSelection();
							dgvTrayList.Rows[row].Selected	= true;
						}

						dgvTrayList.DefaultCellStyle.SelectionBackColor				= Color.LightSkyBlue;
						dgvTrayProdList.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvTraySelectProdValue.DefaultCellStyle.SelectionBackColor	= Color.LightSkyBlue;
						dgvTrayValue.DefaultCellStyle.SelectionBackColor			= SystemColors.Highlight;
						m_SelGridTray	= dgv;
						break;

					case "dgvTrayProdList"	:
						if (dgvTrayProdList.Focused == false)			return;

						if (dgvTrayList.SelectedRows.Count > 1)
						{
							row	= dgvTrayList.SelectedRows[0].Index;
							dgvTrayList.ClearSelection();
							dgvTrayList.Rows[row].Selected	= true;
						}

						dgvTrayList.DefaultCellStyle.SelectionBackColor				= Color.LightSkyBlue;
						dgvTrayValue.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvTraySelectProdValue.DefaultCellStyle.SelectionBackColor	= Color.LightSkyBlue;
						dgvTrayProdList.DefaultCellStyle.SelectionBackColor			= SystemColors.Highlight;
						m_SelGridTray	= dgv;
						break;

					case "dgvTraySelectProdValue"	:
						if (dgvTraySelectProdValue.Focused == false)			return;

						if (dgvTrayList.SelectedRows.Count > 1)
						{
							row	= dgvTrayList.SelectedRows[0].Index;
							dgvTrayList.ClearSelection();
							dgvTrayList.Rows[row].Selected	= true;
						}

						if (dgvTrayProdList.SelectedRows.Count > 1)
						{
							row	= dgvTrayProdList.SelectedRows[0].Index;
							dgvTrayProdList.ClearSelection();
							dgvTrayProdList.Rows[row].Selected	= true;
						}

						dgvTrayList.DefaultCellStyle.SelectionBackColor				= Color.LightSkyBlue;
						dgvTrayProdList.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvTrayValue.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvTraySelectProdValue.DefaultCellStyle.SelectionBackColor	= SystemColors.Highlight;
						m_SelGridTray	= dgv;
						break;

					case "dgvPartList"	:
						if (dgvPartList.Focused == false)			return;

						dgvPartProdList.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvPartValue.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvPartSelectProdValue.DefaultCellStyle.SelectionBackColor	= Color.LightSkyBlue;
						dgvPartList.DefaultCellStyle.SelectionBackColor				= SystemColors.Highlight;
						m_SelGridPart	= dgv;
						break;
					
					case "dgvPartValue"	:
						if (dgvPartValue.Focused == false)			return;

						if (dgvPartList.SelectedRows.Count > 1)
						{
							row	= dgvPartList.SelectedRows[0].Index;
							dgvPartList.ClearSelection();
							dgvPartList.Rows[row].Selected	= true;
						}

						dgvPartList.DefaultCellStyle.SelectionBackColor				= Color.LightSkyBlue;
						dgvPartProdList.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvPartSelectProdValue.DefaultCellStyle.SelectionBackColor	= Color.LightSkyBlue;
						dgvPartValue.DefaultCellStyle.SelectionBackColor			= SystemColors.Highlight;
						m_SelGridPart	= dgv;
						break;

					case "dgvPartProdList"	:
						if (dgvPartProdList.Focused == false)				return;

						if (dgvPartList.SelectedRows.Count > 1)
						{
							row	= dgvPartList.SelectedRows[0].Index;
							dgvPartList.ClearSelection();
							dgvPartList.Rows[row].Selected	= true;
						}

						dgvPartList.DefaultCellStyle.SelectionBackColor				= Color.LightSkyBlue;
						dgvPartValue.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvPartSelectProdValue.DefaultCellStyle.SelectionBackColor	= Color.LightSkyBlue;
						dgvPartProdList.DefaultCellStyle.SelectionBackColor			= SystemColors.Highlight;
						m_SelGridPart	= dgv;
						break;

					case "dgvPartSelectProdValue"	:
						if (dgvPartSelectProdValue.Focused == false)			return;
					
						if (dgvPartList.SelectedRows.Count > 1)
						{
							row	= dgvPartList.SelectedRows[0].Index;
							dgvPartList.ClearSelection();
							dgvPartList.Rows[row].Selected	= true;
						}

						if (dgvPartProdList.SelectedRows.Count > 1)
						{
							row	= dgvPartProdList.SelectedRows[0].Index;
							dgvPartProdList.ClearSelection();
							dgvPartProdList.Rows[row].Selected	= true;
						}

						dgvPartList.DefaultCellStyle.SelectionBackColor				= Color.LightSkyBlue;
						dgvPartProdList.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvPartValue.DefaultCellStyle.SelectionBackColor			= Color.LightSkyBlue;
						dgvPartSelectProdValue.DefaultCellStyle.SelectionBackColor	= SystemColors.Highlight;
						m_SelGridPart	= dgv;
						break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetValueInput(int RowIndex, int ColumnIndex, Type structType,  DataGridView dgv)
		{
			try
			{
				if (ColumnIndex < 3)			return;
				if (RowIndex < 0)				return;

				frmComboBoxSelect	popup_cbs;
				frmKeyboard			popup_key;
				frmKeyPad			popup_num;

				bool				bool_old	= false;

				Type valueType = typeof(string);
				
				var fields = structType.GetFields();
				foreach (var field in fields)
				{
					//Rows 추가시 field.Name을 사용했으므로, 동일한 field.Name을 찾아서 타입 확인
					if (field.Name == dgv.Rows[RowIndex].Cells[1].Value.ToString())
					{
						valueType = field.FieldType;
						break;
					}
				}

				//기본타입은 그냥 사용, Enum형식인 경우에만 타입 수정
				if (m_enumTypeMap.ContainsKey(dgv.Rows[RowIndex].Cells[1].Value.ToString()))
				{ 
					valueType = m_enumTypeMap[dgv.Rows[RowIndex].Cells[1].Value.ToString()];
				}

				if (valueType == typeof(int) || valueType == typeof(long))
				{
					Label lb = new Label();
					popup_num = new frmKeyPad(lb, dgv.Rows[RowIndex].Cells[3].Value.ToString());
					popup_num.TopMost = true;
					popup_num.ShowInTaskbar = false;
					if (popup_num.ShowDialog() == DialogResult.Cancel) return;

					dgv.Rows[RowIndex].Cells[3].Value = lb.Text;
				}
				else if (valueType == typeof(string))
				{
					popup_key	= new frmKeyboard(dgv.Rows[RowIndex].Cells[3].Value.ToString());
					if (popup_key.ShowDialog() == DialogResult.Cancel)			return;

					dgv.Rows[RowIndex].Cells[3].Value = popup_key.Value;
				}
				else if (valueType == typeof(bool))
				{
					popup_cbs	= new frmComboBoxSelect();
					bool_old	= bool.Parse(dgv.Rows[RowIndex].Cells[3].Value.ToString());
					if (popup_cbs.ShowDialog(frmComboBoxSelect.teType.True) == DialogResult.Cancel)			return;

					dgv.Rows[RowIndex].Cells[3].Value = Convert.ToBoolean(popup_cbs.SelNo).ToString();
				}
				else // 위에서 수정한 Enum타입은 이쪽으로 
				{
					popup_cbs = new frmComboBoxSelect();
					int nsel_no = (int)Enum.Parse(valueType, dgv.Rows[RowIndex].Cells[3].Value.ToString());

					// Enum 이름 배열을 가져와서 팝업을 띄움
					if (popup_cbs.ShowDialog(Enum.GetNames(valueType), nsel_no) == DialogResult.Cancel)
						return;

					// 선택된 값을 다시 DataGridView 셀에 설정
					dgv.Rows[RowIndex].Cells[3].Value = Enum.GetName(valueType, popup_cbs.SelNo);
				}
				
			
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	SetValue(DataGridView dgv, int rowIndex)
		{
			if (dgv == dgvTrayValue)
			{
				bool isNewValue = false;
				FlowTray	flow	= (FlowTray)dgvTrayList.SelectedRows[0].Index;
				if (SM.FMT.IsFlow(flow) == false) // 만약 데이터가 없을 경우
				{ 
					SM.FMT.Next(FlowTray.Empty, flow);
					isNewValue = true;
				}
				
				InfoTray.tsInfo temp	= new InfoTray.tsInfo();
				for (int i = 0; i < dgv.Rows.Count; i++)
				{
					var cellValue = dgv.Rows[i].Cells[3].Value?.ToString(); 
					var fieldName = dgv.Rows[i].Cells[1].Value?.ToString(); 

					if (cellValue != null)
					{
						var fieldInfo = typeof(InfoTray.tsInfo).GetField(fieldName);
						var fieldType = fieldInfo.FieldType;
						
						// 열거형 타입인 경우 처리
						if (m_enumTypeMap.ContainsKey(fieldName)) 
						{
							Type enumType = m_enumTypeMap[fieldName];
							cellValue = ((int)Enum.Parse(enumType, cellValue)).ToString();
						}
				

						switch (fieldName)
						{
							case "LastTrayOfLot" :		temp.LastTrayOfLot		=  bool.Parse(cellValue);	break;
							case "AutoRunMode" :		temp.AutoRunMode		=  int.Parse(cellValue);	break;
							case "TrayNo" :				temp.TrayNo				=  int.Parse(cellValue);	break;
							case "MaxCntTray" :			temp.MaxCntTray			=  int.Parse(cellValue);	break;
							case "MaxCntProdInTray" :	temp.MaxCntProdInTray	=  int.Parse(cellValue);	break;
							case "MaxCntProdInLot" :	temp.MaxCntProdInLot	=  int.Parse(cellValue);	break;
							case "ProcStatus" :			temp.ProcStatus			=  int.Parse(cellValue);	break;
							case "ColCnt" :				temp.ColCnt				=  int.Parse(cellValue);	break;
							case "RowCnt" :				temp.RowCnt				=  int.Parse(cellValue);	break;
							case "TransTime" :			temp.TransTime			=  long.Parse(cellValue);	break;
							case "StartTime" :			temp.StartTime			=  long.Parse(cellValue);	break;
							case "ModuleType" :			temp.ModuleType			=  cellValue;				break;
							case "ModuleGen" :			temp.ModuleGen			=  cellValue;				break;
							case "InTime" :				temp.InTime				=  long.Parse(cellValue);	break;
							case "OutTime" :			temp.OutTime			=  long.Parse(cellValue);	break;
							case "WorkOrder" :			temp.WorkOrder			=  cellValue;				break;
							case "KeyValue" :			temp.KeyValue			=  cellValue;				break;
							case "TrayID" :				temp.TrayID				=  cellValue;				break;
							case "ProgramID_Top" :		temp.ProgramID_Top		=  cellValue;				break;
							case "ProgramID_Btm" :		temp.ProgramID_Btm		=  cellValue;				break;
							case "TrayBarcode" :		temp.TrayBarcode		=  cellValue;				break;
							default:
								break;
						}
					}
				}
				
				 // 만약 이미 데이터가 있을 경우
				if (isNewValue == false)		temp.ProdInfo	= SM.FMT[flow].GetInfo().ProdInfo;
				else							temp.ProdInfo	= new stProdInfo[200];

				SM.FMT[flow].SetInfo(temp);
			}
			else if (dgv == dgvPartValue)
			{
				bool isNewValue = false;
				FlowPart flow = (FlowPart)dgvPartList.SelectedRows[0].Index;

				if (SM.FMP.IsFlow(flow) == false) // 만약 데이터가 있을 경우
				{
					SM.FMP.Next(FlowPart.Empty, flow);
					isNewValue = true;
				}
				
				InfoPart.tsInfo	temp	= new InfoPart.tsInfo();
				
				for (int i = 0; i < dgv.Rows.Count; i++)
				{
					var cellValue = dgv.Rows[i].Cells[3].Value?.ToString(); 
					var fieldName = dgv.Rows[i].Cells[1].Value?.ToString(); 

					if (cellValue != null)
					{
						var fieldInfo = typeof(InfoPart.tsInfo).GetField(fieldName);
						var fieldType = fieldInfo.FieldType;

						switch (fieldName)
						{
							case "LastPieceOfLot" :		temp.LastPieceOfLot		=  bool.Parse(cellValue);	break;
							case "MaxCntTray" :			temp.MaxCntTray			=  int.Parse(cellValue);	break;
							case "MaxCntProdInTray" :	temp.MaxCntProdInTray	=  int.Parse(cellValue);	break;
							case "MaxCntProdInLot" :	temp.MaxCntProdInLot	=  int.Parse(cellValue);	break;
							case "TransTime" :			temp.TransTime			=  long.Parse(cellValue);	break;
							case "StartTime" :			temp.StartTime			=  long.Parse(cellValue);	break;
							case "WorkOrder" :			temp.WorkOrder			=  cellValue;				break;
							case "KeyValue" :			temp.KeyValue			=  cellValue;				break;
							case "ProgramID_Top" :		temp.ProgramID_Top		=  cellValue;				break;
							case "ProgramID_Btm" :		temp.ProgramID_Btm		=  cellValue;				break;
							default:
								break;
						}
					}
				}
				
				 // 만약 이미 데이터가 있을 경우
				if (isNewValue == false)		temp.ProdInfo	= SM.FMP[flow].GetInfo().ProdInfo;
				else							temp.ProdInfo	= new stProdInfo[SM.FMP[flow].GetMaxProdCount()];

				SM.FMP[flow].SetInfo(temp);
			}
			else if	(dgv == dgvTraySelectProdValue)
			{
				FlowTray		flow_t	= FlowTray.Empty;
				int				nidx;

				flow_t = (FlowTray)dgvTrayList.SelectedRows[0].Index;
				nidx = dgvTrayProdList.SelectedRows[0].Index;

				if (SM.FMT.IsFlow(flow_t) == false) return;

				stProdInfo temp = new stProdInfo();
			
				for (int i = 0; i < dgv.Rows.Count; i++)
				{
					var cellValue = dgv.Rows[i].Cells[3].Value?.ToString(); 
					var fieldName = dgv.Rows[i].Cells[1].Value?.ToString(); 

					if (cellValue != null)
					{
						var fieldInfo = typeof(stProdInfo).GetField(fieldName);
						var fieldType = fieldInfo.FieldType;
						
						// 열거형 타입인 경우 처리
						if (m_enumTypeMap.ContainsKey(fieldName)) 
						{
							Type enumType = m_enumTypeMap[fieldName];
							cellValue = ((int)Enum.Parse(enumType, cellValue)).ToString();
						}
				
						switch (fieldName)
						{
							case "Exist" :				temp.Exist				=  bool.Parse(cellValue);	break;
							case "TrayRow" :			temp.TrayRow			=  int.Parse(cellValue);	break;
							case "TrayCol" :			temp.TrayCol			=  int.Parse(cellValue);	break;
							case "ResultTop" :			temp.ResultTop			=  int.Parse(cellValue);	break;
							case "ResultBtm" :			temp.ResultBtm			=  int.Parse(cellValue);	break;
							case "ResultMES" :			temp.ResultMES			=  int.Parse(cellValue);	break;
							case "VerifyTopDone" :		temp.VerifyTopDone		=  bool.Parse(cellValue);	break;
							case "VerifyBtmDone" :		temp.VerifyBtmDone		=  bool.Parse(cellValue);	break;
							case "PickStatus" :			temp.PickStatus			=  int.Parse(cellValue);	break;
							case "KeyValue" :			temp.KeyValue			=  cellValue;				break;
							case "TrayID" :				temp.TrayID				=  cellValue;				break;
							case "TrayBarcode" :		temp.TrayBarcode		=  cellValue;				break;
							case "ProdID" :				temp.ProdID				=  cellValue;				break;
							case "InspTimeTop" :		temp.InspTimeTop		=  cellValue;				break;
							case "InspTimeBtm" :		temp.InspTimeBtm		=  cellValue;				break;
							case "SerialNumber" :		temp.SerialNumber		=  cellValue;				break;
							default:
								break;
						}
					}
				}
				
				SM.FMT[flow_t].SetProd(nidx, temp);
			}
			else if (dgv == dgvPartSelectProdValue)
			{
				FlowPart		flow_p	= FlowPart.Empty;
				int				nidx;

				flow_p = (FlowPart)dgvPartList.SelectedRows[0].Index;
				nidx = dgvPartProdList.SelectedRows[0].Index;

				if (SM.FMP.IsFlow(flow_p) == false) return;
				
				stProdInfo temp = new stProdInfo();
			
				for (int i = 0; i < dgv.Rows.Count; i++)
				{
					var cellValue = dgv.Rows[i].Cells[3].Value?.ToString(); 
					var fieldName = dgv.Rows[i].Cells[1].Value?.ToString(); 

					if (cellValue != null)
					{
						var fieldInfo = typeof(stProdInfo).GetField(fieldName);
						var fieldType = fieldInfo.FieldType;
						
						// 열거형 타입인 경우 처리
						if (m_enumTypeMap.ContainsKey(fieldName)) 
						{
							Type enumType = m_enumTypeMap[fieldName];
							cellValue = ((int)Enum.Parse(enumType, cellValue)).ToString();
						}
				
						switch (fieldName)
						{
							case "Exist" :				temp.Exist				=  bool.Parse(cellValue);	break;
							case "TrayRow" :			temp.TrayRow			=  int.Parse(cellValue);	break;
							case "TrayCol" :			temp.TrayCol			=  int.Parse(cellValue);	break;
							case "ResultTop" :			temp.ResultTop			=  int.Parse(cellValue);	break;
							case "ResultBtm" :			temp.ResultBtm			=  int.Parse(cellValue);	break;
							case "ResultMES" :			temp.ResultMES			=  int.Parse(cellValue);	break;
							case "VerifyTopDone" :		temp.VerifyTopDone		=  bool.Parse(cellValue);	break;
							case "VerifyBtmDone" :		temp.VerifyBtmDone		=  bool.Parse(cellValue);	break;
							case "PickStatus" :			temp.PickStatus			=  int.Parse(cellValue);	break;
							case "KeyValue" :			temp.KeyValue			=  cellValue;				break;
							case "TrayID" :				temp.TrayID				=  cellValue;				break;
							case "TrayBarcode" :		temp.TrayBarcode		=  cellValue;				break;
							case "ProdID" :				temp.ProdID				=  cellValue;				break;
							case "InspTimeTop" :		temp.InspTimeTop		=  cellValue;				break;
							case "InspTimeBtm" :		temp.InspTimeBtm		=  cellValue;				break;
							case "SerialNumber" :		temp.SerialNumber		=  cellValue;				break;	
						
							default:
								break;
						}
					}
				}
					SM.FMP[flow_p].SetProd(nidx, temp);
			}
			else
			{
				return;
			}

		}
		#endregion

		#region Function
		
		private void	tmrUpdateInfo_Tick(object sender, EventArgs e)
		{
			try
			{
				UpdateValue();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		//현재 들어있는 값, 지속적으로 업데이트
		private	void	UpdateValue()
		{
			try
			{
				if (m_bLoaded == false)						return;

				UpdateProdList();

				#region [ TRAY ]
				if (tabControl1.SelectedIndex == 0)
				{
					//트레이 리스트 정보 업데이트
					for (int i = 0; i < (int)FlowTray.Max; i++)
					{
						bool	bexist	= SM.FMT.IsFlow((FlowTray)i);
						
						dgvTrayList.Rows[i].Cells[2].Value	= bexist;
						
						if (bexist == true)			dgvTrayList.Rows[i].Cells[2].Style.BackColor	= Color.Yellow;
						else						dgvTrayList.Rows[i].Cells[2].Style.BackColor	= Color.White;
					}
					
					//선택된 트레이 정보 업데이트
					if (dgvTrayList.SelectedRows.Count < 1)			return;
					FlowTray	flow				= (FlowTray)dgvTrayList.SelectedRows[0].Index;
					UpdateDataGridViewValue(SM.FMT[flow].GetInfo(), dgvTrayValue);


					//제품 리스트 정보 업데이트
					stProdInfo[] ProdList_InTray = SM.FMT[flow].GetInfo().ProdInfo;
					for (int i = 0; i < dgvTrayProdList.Rows.Count; i++)
					{
						dgvTrayProdList.Rows[i].Cells[1].Value	= ProdList_InTray[i].ProdID;
						dgvTrayProdList.Rows[i].Cells[2].Value	= ProdList_InTray[i].TrayCol;
						dgvTrayProdList.Rows[i].Cells[3].Value	= ProdList_InTray[i].TrayRow;
						dgvTrayProdList.Rows[i].Cells[4].Value	= ProdList_InTray[i].Exist == true ? "True" : "False";
						
						if (ProdList_InTray[i].Exist == true)			dgvTrayProdList.Rows[i].Cells[4].Style.BackColor = Color.Yellow;
						else											dgvTrayProdList.Rows[i].Cells[4].Style.BackColor = Color.White;
					}
					
					//선택된 제품 정보 업데이트
					if (dgvTrayProdList.SelectedRows.Count < 1)
					{
						for (int i = 0; i < dgvTraySelectProdValue.Rows.Count; i++)
						{
							dgvTraySelectProdValue.Rows[i].Cells[2].Value = "";
						}
						return;
					}
					int			nidx	= dgvTrayProdList.SelectedRows[0].Index;
					stProdInfo	tsinfo	= SM.FMT[flow].GetInfo().ProdInfo[nidx];
					UpdateDataGridViewValue(tsinfo, dgvTraySelectProdValue);
				}
				#endregion

				#region [ PART ]
				if (tabControl1.SelectedIndex == 1)
				{
					if (dgvPartList.SelectedRows.Count < 1)			return;
					
					//파트 리스트 정보 업데이트
					FlowPart	flow		= (FlowPart)dgvPartList.SelectedRows[0].Index;
					InfoPart	part_info	= SM.FMP[flow];

					for (int i = 0; i < (int)FlowPart.Max; i++)
					{
						bool bexist	= SM.FMP.IsFlow((FlowPart)i);

						dgvPartList.Rows[i].Cells[2].Value	= bexist;

						if (bexist == true)			dgvPartList.Rows[i].Cells[2].Style.BackColor	= Color.Yellow;
						else						dgvPartList.Rows[i].Cells[2].Style.BackColor	= Color.White;
					}
					
					//파트 정보 업데이트
					UpdateDataGridViewValue(part_info.GetInfo(), dgvPartValue);
					
					//제품 리스트 정보 업데이트
					stProdInfo[] ProdList_InPart = part_info.GetInfo().ProdInfo;
					for (int i = 0; i < part_info.GetMaxProdCount(); i++)
					{
						dgvPartProdList.Rows[i].Cells[1].Value	= ProdList_InPart[i].ProdID;
						dgvPartProdList.Rows[i].Cells[2].Value	= ProdList_InPart[i].TrayCol;
						dgvPartProdList.Rows[i].Cells[3].Value	= ProdList_InPart[i].TrayRow;
						dgvPartProdList.Rows[i].Cells[4].Value	= ProdList_InPart[i].Exist == true ? "True" : "False";
						
						if (ProdList_InPart[i].Exist == true)			dgvPartProdList.Rows[i].Cells[4].Style.BackColor = Color.Yellow;
						else											dgvPartProdList.Rows[i].Cells[4].Style.BackColor = Color.White;
					}
					
					//선택된 제품 정보 업데이트
					if (dgvPartProdList.SelectedRows.Count < 1)
					{
						for (int i = 0; i < dgvPartSelectProdValue.Rows.Count; i++)
						{
							dgvPartSelectProdValue.Rows[i].Cells[2].Value	= "";
						}
					}
					int			nidx	= dgvPartProdList.SelectedRows[0].Index;
					stProdInfo	tsinfo	= part_info.GetInfo().ProdInfo[nidx];
					UpdateDataGridViewValue(tsinfo, dgvPartSelectProdValue);
					
				}
				#endregion

				#region Modify Check Box Status
				if((SM.SHM.IsBitData(BIT.AutoRun) == true || SM.SHM.IsBitData(BIT.AllHoming) == true) &&
					SM.SHM.IsBitData(BIT.Pause) == false)
				{
					chkTrayModify.Checked = false;
					chkPartModify.Checked = false;

					chkTrayModify.Enabled = false;
					chkPartModify.Enabled = false;
				}
				else
				{
					chkTrayModify.Enabled = true;
					chkPartModify.Enabled = true;
				}

				#endregion
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private void	UpdateDataGridViewValue<T>(T info, DataGridView dgv) where T : struct
		{
			try
			{
				var fields = typeof(T).GetFields();
				// DataGridView의 각 행에 대해 필드 값을 업데이트
				for (int i = 0; i < fields.Length; i++)
				{
					object fieldValue = fields[i].GetValue(info);
					if (i < dgv.Rows.Count)
					{
						//기본타입은 그냥 사용, Enum형식인 경우에만 타입 수정
						if (m_enumTypeMap.ContainsKey(dgv.Rows[i].Cells[1].Value.ToString()))
						{
							Type enumType = m_enumTypeMap[dgv.Rows[i].Cells[1].Value.ToString()];
							fieldValue = Enum.ToObject(enumType, (int)fieldValue);
						}

						dgv.Rows[i].Cells[2].Value = fieldValue?.ToString();
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}
		

		//사용자가 바꿀 수 있는 값, 셀이나 탭 선택시에만 업데이트
		private	void	UpdateSetValue()
		{
			try
			{
				if (m_bLoaded == false)						return;
				if (m_bSetValueUpdated == false)			return;
				
				m_bSetValueUpdated	= false;

				InfoPart part_info;
				
				UpdateProdList();

				#region [ TRAY ]
				if (tabControl1.SelectedIndex == 0)
				{
					if (dgvTrayList.SelectedRows.Count < 1)				return;
					//트레이 정보 업데이트
					FlowTray	flow				= (FlowTray)dgvTrayList.SelectedRows[0].Index;
					UpdateDataGridViewSetValue( SM.FMT[flow].GetInfo(), dgvTrayValue);
					
					if (dgvTrayProdList.SelectedRows.Count < 1)
					{
						for (int i = 0; i < dgvTraySelectProdValue.RowCount; i++)
						{
							dgvTraySelectProdValue.Rows[i].Cells[3].Value	= "";
						}
						return;
					}
					
					//제품 정보 업데이트
					int			nidx	= dgvTrayProdList.SelectedRows[0].Index;
					stProdInfo	tsinfo	= SM.FMT[flow].GetInfo().ProdInfo[nidx];
					UpdateDataGridViewSetValue(tsinfo, dgvTraySelectProdValue);
				
				}
				#endregion

				#region [ PART ]
				if (tabControl1.SelectedIndex == 1)
				{
					if (dgvPartList.SelectedRows.Count < 1)				return;
					//파트 정보 업데이트
					part_info = SM.FMP[(FlowPart)dgvPartList.SelectedRows[0].Index];
					UpdateDataGridViewSetValue(part_info.GetInfo(), dgvPartValue);
					
					if(dgvPartProdList.SelectedRows.Count < 1)
					{
						for (int i = 0; i < dgvPartSelectProdValue.RowCount; i++)
						{
							dgvPartSelectProdValue.Rows[i].Cells[3].Value	= "";
						}
						return;
					}
					
					//제품 정보 업데이트
					int nidx = dgvPartProdList.SelectedRows[0].Index;
					stProdInfo tsinfo = part_info.GetInfo().ProdInfo[nidx];
					UpdateDataGridViewSetValue(tsinfo, dgvPartSelectProdValue);
				}

				#endregion
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
			finally
			{
				m_bSetValueUpdated	= true;
			}
		}
		
		private void	UpdateDataGridViewSetValue<T>(T info, DataGridView dgv) where T : struct
		{
			var fields = typeof(T).GetFields();
			// DataGridView의 각 행에 대해 필드 값을 업데이트
			for (int i = 0; i < fields.Length; i++)
			{
				object fieldValue = fields[i].GetValue(info);
				if (i < dgv.Rows.Count)
				{
					//기본타입은 그냥 사용, Enum형식인 경우에만 타입 수정
					if (m_enumTypeMap.ContainsKey(dgv.Rows[i].Cells[1].Value.ToString()))
					{ 
						Type enumType = m_enumTypeMap[dgv.Rows[i].Cells[1].Value.ToString()];
						fieldValue = Enum.ToObject(enumType, (int)fieldValue);
					}
					
					// 해당 행의 필드 값을 업데이트
					dgv.Rows[i].Cells[3].Value = fieldValue?.ToString();
				}
			}
		}


		//선택된 트레이, 파트에 따라 해당 하는 프로드 리스트 업데이트
		private	void	UpdateProdList()
		{
			try
			{
				lock (m_lockProdListAdd)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						if (dgvTrayList.SelectedRows.Count < 1)				return;

						FlowTray	flow				= (FlowTray)dgvTrayList.SelectedRows[0].Index;
						int			ProdMaxCntInTray	= SM.FMT[flow].GetInfo().MaxCntProdInTray;

						if (dgvTrayProdList.Rows.Count != ProdMaxCntInTray)
						{
							dgvTrayProdList.ClearSelection();
							dgvTrayProdList.Rows.Clear();
							
							dgvTrayProdList.SelectionChanged	-= dgv_SelectionChanged;
							
							for(int i = 0; i < ProdMaxCntInTray; i++)
							{
								dgvTrayProdList.Rows.Add(i, "", "", "", "");
							}

							dgvTrayProdList.SelectionChanged	+= dgv_SelectionChanged;
						}
					}

					if (tabControl1.SelectedIndex == 1)
					{
						if (dgvPartList.SelectedRows.Count < 1)				return;
						if (dgvPartList.SelectedRows.Count > 0)
						{
							InfoPart part_info = SM.FMP[(FlowPart)dgvPartList.SelectedRows[0].Index];

							if (dgvPartProdList.Rows.Count == 0)
							{
								dgvPartProdList.ClearSelection();
								dgvPartProdList.Rows.Clear();
								
								dgvPartProdList.SelectionChanged	-= dgv_SelectionChanged;

								for(int i = 0; i < part_info.GetMaxProdCount(); i++)
								{
									dgvPartProdList.Rows.Add(i, "", "", "", "");
								}

								dgvPartProdList.SelectionChanged	+= dgv_SelectionChanged;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

	}
}
