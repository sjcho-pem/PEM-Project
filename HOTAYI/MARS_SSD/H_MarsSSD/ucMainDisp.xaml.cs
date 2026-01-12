using PMN.Control.WPF.MachineAni;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static H_MarsSSD.frmProductEditPopup;


namespace H_MarsSSD
{
	/// <summary>
	/// ucMainDisp.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class ucMainDisp : UserControl
	{
		#region Struct
		private	enum	teModuleState
		{
			Unknown	= 0,
			Loaded,
			Unloaded,
		}

		struct ModuleData
		{
			private	CBlinkTimer		AirBlink;
			public	Status			StateColor;
			private	teModule		thisModule;
			public	bool			bAlreadyRun;
			private	IFigure			thisAni;
			private	teModuleState	status;

			public			ModuleData(teModule Module, IFigure Ani, GroupBox ColorState)
			{
				AirBlink	= new CBlinkTimer();
				StateColor	= new Status();
				thisAni		= new ucFigure();
				thisModule	= Module;
				bAlreadyRun = false;
				thisAni		= Ani;
				status		= teModuleState.Unknown;

				if (ColorState != null)  ColorState.DataContext = StateColor;
			}

			public	void	StateUpdate()
			{
				Brush[] temp = new Brush[2];

				switch (thisModule)
				{
					case teModule.TrayClean:
						temp[0] =  SM.SHM.GetInputData(DI.TrayCleanUpCyl) == true ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.LightGray);
						temp[1] =  SM.SHM.GetInputData(DI.TrayCleanDnCyl) == true ? new SolidColorBrush(Colors.Black) : new SolidColorBrush(Colors.LightGray);

						bool bAirBlink = AirBlink.Blink(800, 800);
						//클린 트레이 에어 점멸 업데이트
						if (SM.SHM.GetOutputData(DO.TrayCleanAirBlow) == true)
						{
							if (bAirBlink)			StateColor.Visible = Visibility.Visible;
							else					StateColor.Visible = Visibility.Hidden;
						}
						else
						{
							StateColor.Visible = Visibility.Hidden;
						}

						//트레이 Visible 업데이트
						if (SM.FMT.IsFlow(FlowTray.TrayClean) == true)
						{
							thisAni._SetVisibility(Visibility.Visible);
						}
						else
						{
							if (thisAni._IsRunMove() == false)
							{
								thisAni._SetVisibility(Visibility.Hidden);
							}
						}
						break;
						
				}
				StateColor.CylState = temp;
				StateColor.ModuleState = IsRun == true ? new SolidColorBrush(Colors.Lime) : new SolidColorBrush();
			}

			public	bool	IsRun
			{
				get
				{
					switch (thisModule)
					{
						case teModule.LDTable1:		return G.seqLoadTrayTable1.IsReady == false;
						case teModule.LDTable2:		return G.seqLoadTrayTable2.IsReady == false;
						case teModule.ULDTable1:	return G.seqUnloadTrayTable1.IsReady == false;
						case teModule.ULDTable2:	return G.seqUnloadTrayTable2.IsReady == false;
						case teModule.TrayClean:	return G.seqTrayClean.IsReady == false;
						case teModule.LDLift:		return G.seqTrayLoader.IsLiftReady() == false;
						case teModule.LDConv:		return G.seqTrayLoader.IsConvReady() == false;
						case teModule.ULDLift:		return G.seqTrayUnloader.IsLiftReady() == false;
						case teModule.ULDConv:		return G.seqTrayUnloader.IsConvReady() == false;
					}
					return false;
				}
			}

			public	bool	IsExistChange
			{
				get
				{
					switch (thisModule)
					{
						case teModule.LDTable1:     return CurExist != SM.FMT.IsFlow(FlowTray.LDTable1);
						case teModule.LDTable2:     return CurExist != SM.FMT.IsFlow(FlowTray.LDTable2);
						case teModule.ULDTable1:    return CurExist != SM.FMT.IsFlow(FlowTray.ULDTable1);
						case teModule.ULDTable2:    return CurExist != SM.FMT.IsFlow(FlowTray.ULDTable2);
						case teModule.TrayClean:    return CurExist != SM.FMT.IsFlow(FlowTray.TrayClean);
					}

					return false;
				}
			}

			private	bool	CurExist
			{
				get
				{
					return thisAni._Location != new Point(0, 0);
				}
			}

			public	bool	IsLoading
			{
				get
				{
					switch (thisModule)
					{
						case teModule.LDTable1:			return SM.FMT.IsFlow(FlowTray.LDTable1);
						case teModule.LDTable2:			return SM.FMT.IsFlow(FlowTray.LDTable2);
						case teModule.ULDTable1:		return SM.FMT.IsFlow(FlowTray.ULDTable1);
						case teModule.ULDTable2:		return SM.FMT.IsFlow(FlowTray.ULDTable2);
						case teModule.TrayClean:		return  (SM.FMT.IsFlow(FlowTray.TrayClean) && (G.seqTrayClean.IsActPos(SeqTrayClean.Act.Rotate) == SenPos.FWD)) ;
						
					}

					return false;
				}
			}

			public	bool	IsUnloading
			{
				get
				{
					switch (thisModule)
					{
						case teModule.LDTable1:     return SM.FMT.IsFlow(FlowTray.LDTable1) == false ;
						case teModule.LDTable2:     return SM.FMT.IsFlow(FlowTray.LDTable2) == false;
						case teModule.ULDTable1:    return SM.FMT.IsFlow(FlowTray.ULDTable1) == false;
						case teModule.ULDTable2:    return SM.FMT.IsFlow(FlowTray.ULDTable2) == false;
						case teModule.TrayClean:
							if (SM.FMT.IsFlow(FlowTray.TrayClean))
							{
								return (G.seqTrayClean.IsActPos(SeqTrayClean.Act.Rotate) == SenPos.BWD);
							}
							else return false;
					}

					return false;
				}
			}
			
			public	void	Loading()
			{
				if (thisAni._IsRunMove() == true)
					return;
				if (status == teModuleState.Loaded)
					return;

				status	= teModuleState.Loaded;

				switch (thisModule)
				{
					case teModule.LDTable1:
					case teModule.LDTable2:
					case teModule.ULDTable1:
					case teModule.ULDTable2:
						thisAni._SetVisibility(Visibility.Visible);
						Runmove(new System.Windows.Point(0, -50));
						break;
					case teModule.TrayClean:
						thisAni._RunRotate(tePosMode.Abs, 180, (180 / 50) * 1000);
						break;
				
				}
			}

			public	void	Unloading()
			{
				if (thisAni._IsRunMove() == true)
					return;
				if (status == teModuleState.Unloaded)
					return;

				status	= teModuleState.Unloaded;

				switch (thisModule)
				{
					case teModule.LDTable1:
					case teModule.LDTable2:
					case teModule.ULDTable1:
					case teModule.ULDTable2:
						Runmove(new System.Windows.Point(0, 0));			break;
					case teModule.TrayClean:
						thisAni._RunRotate(tePosMode.Abs, 0, (180 / 50) * 1000);			break;
				}
			}

			private	void	Runmove(Point Dest)
			{
				double Diffrent =  Math.Abs(thisAni._Location.X - Dest.X) > 0 ?  Math.Abs(thisAni._Location.X - Dest.X) : Math.Abs(thisAni._Location.Y - Dest.Y);
				thisAni._RunMove(tePosMode.Abs, Dest, (Diffrent / 50) * 1000);
			}

		}

		struct MotorData
		{
			private	MT			thisMotor;
			private	IFigure	thisAni { get; }
			private	Dictionary<int, Point>	thisDicPos;
			private	bool		m_bCoordX;

			public			MotorData(MT Motor, IFigure Ani)
			{
				thisMotor	= Motor;
				thisAni		= Ani;
				thisDicPos	= new Dictionary<int, Point>();
				m_bCoordX	= true;
			}

			public	void	SetDicPos(Dictionary<int, Point> dicPos)
			{
				thisDicPos	= dicPos;
			}

			public	void	SetLutPos(tsPosLut[] X, tsPosLut[] Y, bool bCoordX)
			{
				if (X.Length > 0)			thisAni._SetLutPosX(X);
				if (Y.Length > 0)			thisAni._SetLutPosY(Y);
				m_bCoordX	= bCoordX;
			}

			public	int		nNextIdx
			{
				get
				{
					if		(thisMotor == MT.VirtualTopInspX)		return  NDm.Read(eNDM.TopInspNextIdx);
					else if (thisMotor == MT.VirtualBtmInspX)		return  NDm.Read(eNDM.BtmInspNextIdx);
					else											return  G.cKernel.Motor(thisMotor).GetCmdIndex();
				}
			}

			public	int		nCurIdx
			{
				get
				{
					if		(thisMotor == MT.VirtualTopInspX)		return NDm.Read(eNDM.TopInspCurIdx);
					else if (thisMotor == MT.VirtualBtmInspX)		return NDm.Read(eNDM.BtmInspCurIdx);
					else											return G.cKernel.Motor(thisMotor).GetNowIndex();
				}
			}

			public	double	Diffrent
			{
				get
				{
					Point Destnation = new Point();
					if (thisDicPos.TryGetValue(nNextIdx, out Destnation) == false)			return 0;

					if (Math.Abs(thisAni._Location.X - Destnation.X) > 0)			return Math.Abs(thisAni._Location.X - Destnation.X);
					else															return Math.Abs(thisAni._Location.Y - Destnation.Y);
				}
			}

			public	void	Run()
			{
				if (thisDicPos == null)						return;
				if (thisAni._IsRunMove() == true)			return;

				Point Destnation = new Point();

				if (thisDicPos.TryGetValue(nNextIdx, out Destnation) == true)
				{
					if (thisAni._Location.X == Destnation.X  &&  thisAni._Location.Y == Destnation.Y)			return;

					thisAni._RunMove(tePosMode.Abs, Destnation, (Diffrent / 600) * 1000);

					if (thisMotor == MT.VirtualTopInspX)			NDm.Write(eNDM.TopInspCurIdx,	nNextIdx);
					if (thisMotor == MT.VirtualBtmInspX)			NDm.Write(eNDM.BtmInspCurIdx,	nNextIdx);
				}
				else
				{
					UI.Logs.Write(LOGPART.EX, string.Format("Animation Motor Run Dictionary ERR : Motor = {0} ,nNextIdx = {1} ", thisMotor.ToString(), nNextIdx) );
				}
			}

			public	void	UpdatePos()
			{
				if (thisAni._IsRunMove() == true)			return;

				STATUS_MOTOR	status	= new STATUS_MOTOR();
				SM.SHM.GetMotorSatusData((uint)thisMotor, ref status);

				Point	pt;
				
				if (m_bCoordX == true)
				{
					pt	= thisAni._PosToPixel(status.nActPos, thisAni._Location.Y);
				}
				else
				{
					pt	= thisAni._PosToPixel(thisAni._Location.X, status.nActPos);
				}

				if (thisAni._Location.X != pt.X  ||  thisAni._Location.Y != pt.Y)
				{
					thisAni._Location	= pt;
				}
			}
			
		}
		#endregion

		#region Variable
		private	ModuleData[]	stModuleData		= new ModuleData[(int)teModule.Max];
		private	MotorData[]		stMotorData			= new MotorData[(int)MT.TOTAL];
		private	Status			TrayCleanCapState	= new Status();
		private	DispatcherTimer	tUpdate				= new DispatcherTimer();
		private	DispatcherTimer	tUpdateFlow			= new DispatcherTimer();
		private	DispatcherTimer	tUpdateRcv			= new DispatcherTimer();
		private	const int		nCapMaxCount		= 5;

		private	CBlinkTimer		StateBlink			= new CBlinkTimer();

		private stProdInfo		stCopyProdInfo		= new stProdInfo();
		private bool			IsCopyDone			= false;
		private	PStopwatch		m_swProcLoadReady	= new PStopwatch();
		
		private	Color			m_NoneColor			= Colors.White;			//제품 없을떄
		private	Color			m_OkColor			= Colors.Lime;			//양품
		private	Color			m_UnknownColor		= Colors.Yellow;		//미검
		private	Color			m_MachineNGColor	= Colors.DeepPink;		//NG
		private	Color			m_UserNGColor		= Colors.Red;			//User NG
		private	Color			m_MisMatchColor		= Colors.Pink;
		private	Color			m_TrayExistColor	= Colors.Black;
		private	Color			m_CapExistColor		= Colors.LightSlateGray;
		private	Color			m_TrayOKColor		= Colors.Lime;
		private	Color			m_TrayEmptyColor	= Colors.LightSlateGray;
		private	Color			m_TrayUnknownColor	= Colors.Yellow;
		#endregion

		#region Enum

		private enum teMotor
		{
			LDBuf1Y,
			LDBuf2Y,
			ULDBufY,
			LDTable2X_1,
			LDTable2X_2,
			LDTable2X_3,
			InspPlateY1,
			InspPlateY2,
			LDInspPnPX,
			ULDInspPnPX,
			LDPnPX,
			ULDPnPX,
			TopInsp,
			BtmInsp,
			LDTrans,
			ULDTrans,
			RejectBufY,
			Max,
		}
		
		private enum teModule
		{
			LDTable1,
			LDTable2,
			ULDTable1,
			ULDTable2,
			TrayClean,
			LDLift,
			LDConv,
			ULDLift,
			ULDConv,

			Max,
		}
		#endregion


		#region - Language Function 

		public	void	SetLanguageText()
		{
			try
			{
				gbLDConveyor.Header				= H_MarsSSD.Language.languageDefault.LDConveyor;
				LDConvReadyState.Margin			= new Thickness(MeasureString(H_MarsSSD.Language.languageDefault.LDConveyor).Width + 5, -13, 0, 0);
				gbLDLift.Header					= H_MarsSSD.Language.languageDefault.LDLift;
				LDLiftReadyState.Margin			= new Thickness(MeasureString(H_MarsSSD.Language.languageDefault.LDLift).Width + 5, -13, 0, 0);
				gbLDTable1.Header				= H_MarsSSD.Language.languageDefault.LDTable1;
				LDTable1ReadyState.Margin		=  new Thickness(MeasureString(H_MarsSSD.Language.languageDefault.LDTable1).Width+5, -13 ,0,0);
				gbLDTable2.Header				= H_MarsSSD.Language.languageDefault.LDTable2;
				LDTable2ReadyState.Margin		= new Thickness(MeasureString(H_MarsSSD.Language.languageDefault.LDTable2).Width + 5, -13, 0, 0);
				gbTrayClean.Header				= H_MarsSSD.Language.languageDefault.TrayClean;
				TrayCleanReadyState.Margin		= new Thickness(MeasureString(H_MarsSSD.Language.languageDefault.TrayClean).Width + 5, -13, 0, 0);
				gbULDConveyor.Header			= H_MarsSSD.Language.languageDefault.ULDConveyor;
				ULDConvReadyState.Margin		= new Thickness(MeasureString(H_MarsSSD.Language.languageDefault.ULDConveyor).Width + 5, -13, 0, 0);
				gbULDLift.Header				= H_MarsSSD.Language.languageDefault.ULDLift; 
				ULDLiftReadyState.Margin		= new Thickness(MeasureString(H_MarsSSD.Language.languageDefault.ULDLift).Width + 5, -13, 0, 0);
				gbULDTable1.Header				= H_MarsSSD.Language.languageDefault.ULDTable1;
				ULDTable1ReadyState.Margin		= new Thickness(MeasureString(H_MarsSSD.Language.languageDefault.ULDTable1).Width + 5, -13, 0, 0);
				gbULDTable2.Header				= H_MarsSSD.Language.languageDefault.ULDTable2;
				ULDTable2ReadyState.Margin		= new Thickness(MeasureString(H_MarsSSD.Language.languageDefault.ULDTable2).Width + 5, -13, 0, 0);
				gbInspection1.Header			= H_MarsSSD.Language.languageDefault.Inspection1;
				gbInspection2.Header			= H_MarsSSD.Language.languageDefault.Inspection2;
				
				tbIdxNG.Text					= H_MarsSSD.Language.languageDefault.MachineNG;
				tbIdxOK.Text					= H_MarsSSD.Language.languageDefault.GOOD + " / " + H_MarsSSD.Language.languageDefault.UserOK;
				tbTrayIdxOK.Text				= H_MarsSSD.Language.languageDefault.GOOD + " / " + H_MarsSSD.Language.languageDefault.UserOK;
				tbIdxUserNG.Text				= H_MarsSSD.Language.languageDefault.UserNG;
				tbIdxUnknown.Text				= H_MarsSSD.Language.languageDefault.Unknown;
				tbTrayIdxUnknown.Text			= H_MarsSSD.Language.languageDefault.Unknown;
				tbLdBuf1.Text					= H_MarsSSD.Language.languageDefault.LdBuf1;
				tbLdBuf2.Text					= H_MarsSSD.Language.languageDefault.LdBuf2;
				tbUldBuf.Text					= H_MarsSSD.Language.languageDefault.UldBuf;
				
				lbLdPnP.Text					= H_MarsSSD.Language.languageDefault.Load     + Environment.NewLine + H_MarsSSD.Language.languageDefault.PnP;
				lbUldPnP.Text					= H_MarsSSD.Language.languageDefault.Unload   + Environment.NewLine + H_MarsSSD.Language.languageDefault.PnP;
				lbLdInspPnP.Text				= H_MarsSSD.Language.languageDefault.Load     + Environment.NewLine + H_MarsSSD.Language.languageDefault.Inspection + Environment.NewLine + H_MarsSSD.Language.languageDefault.PnP;
				lbUldInspPnP.Text				= H_MarsSSD.Language.languageDefault.Unload	  + Environment.NewLine + H_MarsSSD.Language.languageDefault.Inspection + Environment.NewLine + H_MarsSSD.Language.languageDefault.PnP;
				tbRejectBuffer1.Text			= H_MarsSSD.Language.languageDefault.Reject   + Environment.NewLine + H_MarsSSD.Language.languageDefault.Buffer;
				lbLdTrayTrans.Text				= H_MarsSSD.Language.languageDefault.Load     + Environment.NewLine + H_MarsSSD.Language.languageDefault.Transfer;
				lbUldTrayTrans.Text				= H_MarsSSD.Language.languageDefault.Unload   + Environment.NewLine + H_MarsSSD.Language.languageDefault.Transfer;
				
				lbRJTable2.Content				= H_MarsSSD.Language.languageDefault.Reject   + " " + H_MarsSSD.Language.languageDefault.Buffer;
				lbLdTable1.Content				= H_MarsSSD.Language.languageDefault.LDTable1;
				lbLdTable2.Content				= H_MarsSSD.Language.languageDefault.LDTable2;
				lbUldTable1.Content				= H_MarsSSD.Language.languageDefault.ULDTable1;
				lbUldTable2.Content				= H_MarsSSD.Language.languageDefault.ULDTable2;

				gbDoorCloseState.Header			= H_MarsSSD.Language.languageDefault.DoorClose;
				tbDoorTopFront.Text				= H_MarsSSD.Language.languageDefault.DoorTopFront;
				tbDoorTopLeft.Text				= H_MarsSSD.Language.languageDefault.DoorTopLeft;
				tbDoorTopRear.Text				= H_MarsSSD.Language.languageDefault.DoorTopRear;
				tbDoorTopRight.Text				= H_MarsSSD.Language.languageDefault.DoorTopRight;
				tbDoorBottom.Text				= H_MarsSSD.Language.languageDefault.DoorBottom;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	Size	MeasureString(string str)
		{
			var formattedText = new FormattedText(str,
												LanguageManager.GetLanguageCulture(Program.m_Languageidx),
												FlowDirection.LeftToRight,
												new Typeface(gbTrayClean.FontFamily, gbTrayClean.FontStyle, gbTrayClean.FontWeight, gbTrayClean.FontStretch),
												gbTrayClean.FontSize,
												Brushes.Black,
												new NumberSubstitution(),
												TextFormattingMode.Display);

			return new Size(formattedText.Width, formattedText.Height);
		}
		#endregion

		#region Function 

		private	void	Init()
		{
			try
			{
				StModuleDatainit();
				UpdateColor();

				tUpdate.Interval = TimeSpan.FromMilliseconds(20);    //시간간격 설정
				tUpdate.Tick += new EventHandler(Update);
				tUpdate.Start();
				
				tUpdateFlow.Interval = TimeSpan.FromMilliseconds(300);    //시간간격 설정
				tUpdateFlow.Tick += new EventHandler(UpdateFlowMeter);
				tUpdateFlow.Start();

				tUpdateRcv.Interval = TimeSpan.FromMilliseconds(500);    //시간간격 설정
				tUpdateRcv.Tick += new EventHandler(UpdateRcv);
				tUpdateRcv.Start();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		public	void	MotorDispInit(uint nMtNum)
		{
			try
			{
				StMotorDatainit(nMtNum);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	UpdateColor()
		{
			try
			{
				if (string.IsNullOrWhiteSpace(SDm.Read(eSDM.UnknownColor)))		SDm.Write(eSDM.UnknownColor,		$"#{m_UnknownColor.R:X2}{m_UnknownColor.G:X2}{m_UnknownColor.B:X2}");
				if (string.IsNullOrWhiteSpace(SDm.Read(eSDM.NGColor)))			SDm.Write(eSDM.NGColor,				$"#{m_MachineNGColor.R:X2}{m_MachineNGColor.G:X2}{m_MachineNGColor.B:X2}");
				if (string.IsNullOrWhiteSpace(SDm.Read(eSDM.UserNGColor)))		SDm.Write(eSDM.UserNGColor,			$"#{m_UserNGColor.R:X2}{m_UserNGColor.G:X2}{m_UserNGColor.B:X2}");
				if (string.IsNullOrWhiteSpace(SDm.Read(eSDM.OKColor)))			SDm.Write(eSDM.OKColor,				$"#{m_OkColor.R:X2}{m_OkColor.G:X2}{m_OkColor.B:X2}");
				if (string.IsNullOrWhiteSpace(SDm.Read(eSDM.MisMatchColor)))	SDm.Write(eSDM.MisMatchColor,		$"#{m_MisMatchColor.R:X2}{m_MisMatchColor.G:X2}{m_MisMatchColor.B:X2}");
				if (string.IsNullOrWhiteSpace(SDm.Read(eSDM.TrayUnknownColor)))	SDm.Write(eSDM.TrayUnknownColor,	$"#{m_TrayUnknownColor.R:X2}{m_TrayUnknownColor.G:X2}{m_TrayUnknownColor.B:X2}");
				if (string.IsNullOrWhiteSpace(SDm.Read(eSDM.TrayOKColor)))		SDm.Write(eSDM.TrayOKColor,			$"#{m_TrayOKColor.R:X2}{m_TrayOKColor.G:X2}{m_TrayOKColor.B:X2}");
				if (string.IsNullOrWhiteSpace(SDm.Read(eSDM.TrayEmptyColor)))	SDm.Write(eSDM.TrayEmptyColor,		$"#{m_TrayEmptyColor.R:X2}{m_TrayEmptyColor.G:X2}{m_TrayEmptyColor.B:X2}");

				Color newColor = newColor = (Color)ColorConverter.ConvertFromString(SDm.Read(eSDM.UnknownColor));
				if (!(IdxUnknownColor.Fill is SolidColorBrush UnknownBrush) || UnknownBrush.Color != newColor)
				{
					m_UnknownColor = newColor;
					IdxUnknownColor.Fill = new SolidColorBrush(newColor);
				}

				newColor = (Color)ColorConverter.ConvertFromString(SDm.Read(eSDM.NGColor));
				if (!(IdxNGColor.Fill is SolidColorBrush NGBrush) || NGBrush.Color != newColor)
				{
					m_MachineNGColor = newColor;
					IdxNGColor.Fill = new SolidColorBrush(newColor);
				}

				newColor = (Color)ColorConverter.ConvertFromString(SDm.Read(eSDM.UserNGColor));
				if (!(IdxUserNGColor.Fill is SolidColorBrush UserNGBrush) || UserNGBrush.Color != newColor)
				{
					m_UserNGColor = newColor;
					IdxUserNGColor.Fill = new SolidColorBrush(newColor);
				}
				
				newColor = (Color)ColorConverter.ConvertFromString(SDm.Read(eSDM.OKColor));
				if (!(IdxOKColor.Fill is SolidColorBrush OKBrush) || OKBrush.Color != newColor)
				{
					m_OkColor = newColor;
					IdxOKColor.Fill = new SolidColorBrush(newColor);
				}

				newColor = (Color)ColorConverter.ConvertFromString(SDm.Read(eSDM.MisMatchColor));
				if (!(IdxMisMatchColor.Fill is SolidColorBrush MisMatchBrush) || MisMatchBrush.Color != newColor)
				{
					m_MisMatchColor = newColor;
					IdxMisMatchColor.Fill = new SolidColorBrush(newColor);
				}

				newColor = (Color)ColorConverter.ConvertFromString(SDm.Read(eSDM.TrayUnknownColor));
				if (!(TrayIdxUnknownColor.Fill is SolidColorBrush TrayUnknownBrush) || TrayUnknownBrush.Color != newColor)
				{
					m_TrayUnknownColor = newColor;
					TrayIdxUnknownColor.Fill = new SolidColorBrush(newColor);
				}

				newColor = (Color)ColorConverter.ConvertFromString(SDm.Read(eSDM.TrayOKColor));
				if (!(TrayIdxOKColor.Fill is SolidColorBrush TrayOKBrush) || TrayOKBrush.Color != newColor)
				{
					m_TrayOKColor = newColor;
					TrayIdxOKColor.Fill = new SolidColorBrush(newColor);
				}

				newColor = (Color)ColorConverter.ConvertFromString(SDm.Read(eSDM.TrayEmptyColor));
				if (!(TrayIdxEmptyColor.Fill is SolidColorBrush TrayEmptyBrush) || TrayEmptyBrush.Color != newColor)
				{
					m_TrayEmptyColor = newColor;
					TrayIdxEmptyColor.Fill = new SolidColorBrush(newColor);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	StModuleDatainit()
		{
			try
			{
				IFigure tempAni = new ucFigure();
				GroupBox tempGb = new GroupBox();
				for (int i = 0; i < (int)teModule.Max; i++)
				{
					switch ((teModule)i)
					{
						case teModule.LDTable1:		tempAni	= ucLdTable1;		tempGb	= gbLDTable1;	ucLdTable1._FigureArrayRunEndEvent += HomeEndHiddenEvent;		break;
						case teModule.LDTable2:		tempAni	= ucLdTable2;		tempGb	= gbLDTable2;	ucLdTable2._FigureArrayRunEndEvent += HomeEndHiddenEvent;		break;
						case teModule.ULDTable1:	tempAni	= ucUldTable1;		tempGb	= gbULDTable1;	ucUldTable1._FigureArrayRunEndEvent += HomeEndHiddenEvent;		break;
						case teModule.ULDTable2:	tempAni	= ucUldTable2;		tempGb	= gbULDTable2;	ucUldTable2._FigureArrayRunEndEvent += HomeEndHiddenEvent;		break;
						case teModule.TrayClean:	tempAni	= CleanTray;		tempGb	= gbTrayClean;																	break;
						case teModule.LDLift:		tempAni	= null;				tempGb	= gbLDLift;																break;
						case teModule.LDConv:		tempAni	= null;				tempGb	= gbLDConveyor;															break;
						case teModule.ULDLift:		tempAni	= null;				tempGb	= gbULDLift;															break;
						case teModule.ULDConv:		tempAni	= null;				tempGb	= gbULDConveyor;														break;
					}
					stModuleData[i] = new ModuleData((teModule)i, tempAni, tempGb);
				}

			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	StMotorDatainit(uint nMtNum)
		{
			try
			{
				IFigure	tempAni	= null;
				MT		teMT	= MT.TOTAL;
				bool	bCoordX	= true;

				for (int i = 0; i < (int)teMotor.Max; i++)
				{
					switch ((teMotor)i)
					{
						case teMotor.LDBuf1Y:		teMT = MT.LDBuf1Y;			tempAni = ucLDBuf1Y;		bCoordX	= false;	break;
						case teMotor.LDBuf2Y:		teMT = MT.LDBuf2Y;			tempAni = ucLDBuf2Y;		bCoordX	= false;	break;
						case teMotor.ULDBufY:		teMT = MT.ULDBufY;			tempAni = ucUldBufY;		bCoordX	= false;	break;
						case teMotor.LDTable2X_1:	teMT = MT.LDTable2X;		tempAni = ucLdTable2;		bCoordX	= true;		break;
						case teMotor.LDTable2X_2:	teMT = MT.LDTable2X;		tempAni = ucLdTable2Z;		bCoordX	= true;		break;
						case teMotor.LDTable2X_3:	teMT = MT.LDTable2X;		tempAni = ucLdTable2Img;	bCoordX	= true;		break;
						case teMotor.InspPlateY1:	teMT = MT.InspPlate1Y;		tempAni = ucInspPlateY1;	bCoordX	= false;	break;
						case teMotor.InspPlateY2:	teMT = MT.InspPlate2Y;		tempAni = ucInspPlateY2;	bCoordX	= false;	break;
						case teMotor.LDInspPnPX:	teMT = MT.LDInspPnPX;		tempAni = ucLdInspPnPX;		bCoordX	= true;		break;
						case teMotor.ULDInspPnPX:	teMT = MT.ULDInspPnPX;		tempAni = ucUldInspPnPX;	bCoordX	= true;		break;
						case teMotor.LDPnPX:		teMT = MT.LDPnPX;			tempAni = ucLdPnPX;			bCoordX	= true;		break;
						case teMotor.ULDPnPX:		teMT = MT.ULDPnPX;			tempAni = ucUldPnPX;		bCoordX	= true;		break;
						case teMotor.TopInsp:		teMT = MT.VirtualTopInspX;	tempAni = InspTop;			bCoordX	= true;		break;
						case teMotor.BtmInsp:		teMT = MT.VirtualBtmInspX;	tempAni = InspBtm;			bCoordX	= true;		break;
						case teMotor.LDTrans:		teMT = MT.LDTrayTransX;		tempAni = ucLdTrans;		bCoordX	= true;		break;
						case teMotor.ULDTrans:		teMT = MT.ULDTrayTransX;	tempAni = ucUldTrans;		bCoordX	= true;		break;
						case teMotor.RejectBufY:	teMT = MT.RejectBufY;		tempAni = ucRjTable;		bCoordX	= false;	break;
					}

					if (tempAni == null)			continue;

					if		(i == (int)teMotor.TopInsp  ||  i == (int)teMotor.BtmInsp)
					{
						stMotorData[i] = new MotorData(teMT, tempAni);
						SetDicPos((teMotor)i);
					}
					else if (teMT == (MT)nMtNum)
					{
						stMotorData[i]	= new MotorData(teMT, tempAni);
						
						if		(teMT == MT.ULDPnPX)		SetUldPnP();
						else if (teMT == MT.LDPnPX)			SetLdPnP();
						else								SetLutPos((teMotor)i, bCoordX);
					}

					SetZPos((MT)nMtNum);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	SetDicPos(teMotor Motor)
		{
			try
			{
				Dictionary<int, Point>	dicPos	= new Dictionary<int, Point>();

				switch (Motor)
				{
					case teMotor.TopInsp:
						dicPos	= new Dictionary<int, Point>()	{	{ 0,	new Point(-110,0)},	//READY
																	{ 1,	new Point(60,0)},	//INSP1
																	{ 2,	new Point(305,0)},	//INSP2
																	{ 300,	new Point(-110,0)},	//HOME
																};
						break;
					case teMotor.BtmInsp:
						dicPos	= new Dictionary<int, Point>()	{	{ 0,	new Point(230,0)},	//READY
																	{ 1,	new Point(-180,0)},	//INSP1
																	{ 2,	new Point(60,0)},	//INSP2
																	{ 300,	new Point(230,0)},	//HOME
																};
						break;
				}

				stMotorData[(int)Motor].SetDicPos(dicPos);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	SetLutPos(teMotor Motor, bool bCoordX)
		{
			try
			{
				TABLE_MOTOR	table	= new TABLE_MOTOR();;
				tsPosLut[]	tsLutX	= new tsPosLut[0];
				tsPosLut[]	tsLutY	= new tsPosLut[0];

				switch (Motor)
				{
					case teMotor.LDBuf1Y:
						SM.SHM.GetMotorTableData((uint)MT.LDBuf1Y, ref table);
						tsLutY	= new tsPosLut[] {	
													new tsPosLut(-10,	table.nPos[(int)SeqLoadBuffer.PosY.LOAD]),
													new tsPosLut(-250,	table.nPos[(int)SeqLoadBuffer.PosY.UNLOAD]),
													new tsPosLut(-126,	table.nPos[(int)SeqLoadBuffer.PosY.CLEAN]),
												};
						break;

					case teMotor.LDBuf2Y:
						SM.SHM.GetMotorTableData((uint)MT.LDBuf2Y, ref table);
						tsLutY	= new tsPosLut[] {
													new tsPosLut(-10,	table.nPos[(int)SeqLoadBuffer.PosY.LOAD]),
													new tsPosLut(-250,	table.nPos[(int)SeqLoadBuffer.PosY.UNLOAD]),
													new tsPosLut(-126,	table.nPos[(int)SeqLoadBuffer.PosY.CLEAN]),
												};
						break;

					case teMotor.ULDBufY:
						SM.SHM.GetMotorTableData((uint)MT.ULDBufY, ref table);
						tsLutY	= new tsPosLut[] {
													new tsPosLut(0,	table.nPos[(int)SeqUnloadBuffer.PosY.UNLOAD]),
													new tsPosLut(-150,	table.nPos[(int)SeqUnloadBuffer.PosY.LOAD])
												};
						break;

					case teMotor.LDTable2X_1:
					case teMotor.LDTable2X_2:
					case teMotor.LDTable2X_3:
						SM.SHM.GetMotorTableData((uint)MT.LDTable2X, ref table);
						tsLutX	= new tsPosLut[] {
													new tsPosLut(0,		table.nPos[(int)SeqLoadTrayTable2.PosX.READY]),
													new tsPosLut(-140,	table.nPos[(int)SeqLoadTrayTable2.PosX.PnP])
												};
						break;

					case teMotor.InspPlateY1:
						SM.SHM.GetMotorTableData((uint)MT.InspPlate1Y, ref table);
						tsLutY	= new tsPosLut[] {	
													new tsPosLut(5,		table.nPos[(int)SeqInspPlate.PosY.LOAD]),
													new tsPosLut(95,	table.nPos[(int)SeqInspPlate.PosY.INSP])
												};
						break;

					case teMotor.InspPlateY2:
						SM.SHM.GetMotorTableData((uint)MT.InspPlate2Y, ref table);
						tsLutY	= new tsPosLut[] {
													new tsPosLut(5,		table.nPos[(int)SeqInspPlate.PosY.LOAD]),
													new tsPosLut(95,	table.nPos[(int)SeqInspPlate.PosY.INSP])
												};
						break;

					case teMotor.LDInspPnPX:
						SM.SHM.GetMotorTableData((uint)MT.LDInspPnPX, ref table);
						tsLutX	= new tsPosLut[] {	
													new tsPosLut(26,	table.nPos[(int)SeqLoadInspPnP.PosX.BUF1]),
													new tsPosLut(136,	table.nPos[(int)SeqLoadInspPnP.PosX.BUF2]),
													new tsPosLut(508,	table.nPos[(int)SeqLoadInspPnP.PosX.INSP1]),
													new tsPosLut(751,	table.nPos[(int)SeqLoadInspPnP.PosX.INSP2]),
												};
						break;

					case teMotor.ULDInspPnPX:
						SM.SHM.GetMotorTableData((uint)MT.ULDInspPnPX, ref table);
						tsLutX	= new tsPosLut[] {	
													new tsPosLut(0,	table.nPos[(int)SeqUnloadInspPnP.PosX.BUFFER]),
													new tsPosLut(-487,	table.nPos[(int)SeqUnloadInspPnP.PosX.INSP1]),
													new tsPosLut(-245,	table.nPos[(int)SeqUnloadInspPnP.PosX.INSP2])
												};
						break;

					case teMotor.LDTrans:
						SM.SHM.GetMotorTableData((uint)MT.LDTrayTransX, ref table);
						tsLutX	= new tsPosLut[] {	
													new tsPosLut(5,		table.nPos[(int)SeqLoadTransfer.PosX.LOADCONV]),
													new tsPosLut(160,	table.nPos[(int)SeqLoadTransfer.PosX.LOADTABLE1]),
													new tsPosLut(376,	table.nPos[(int)SeqLoadTransfer.PosX.LOADTABLE2]),
													new tsPosLut(606,	table.nPos[(int)SeqLoadTransfer.PosX.TRAYCLEAN])
												};
						break;

					case teMotor.ULDTrans:
						SM.SHM.GetMotorTableData((uint)MT.ULDTrayTransX, ref table);
						tsLutX	= new tsPosLut[] {
													new tsPosLut(-5,	table.nPos[(int)SeqUnloadTransfer.PosX.UNLOADCONV]),
													new tsPosLut(-252,	table.nPos[(int)SeqUnloadTransfer.PosX.UNLOADTABLE2]),
													new tsPosLut(-463,	table.nPos[(int)SeqUnloadTransfer.PosX.UNLOADTABLE1])
												};
						break;

					case teMotor.RejectBufY:
						SM.SHM.GetMotorTableData((uint)MT.RejectBufY, ref table);
						tsLutY	= new tsPosLut[] {
													new tsPosLut(-6,	table.nPos[(int)SeqRejectBuffer.PosY.READY]),
													new tsPosLut(-177,	table.nPos[(int)SeqRejectBuffer.PosY.TRAYCHANGE]),
												};
						break;
				}

				stMotorData[(int)Motor].SetLutPos(tsLutX, tsLutY, bCoordX);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetUldPnP()
		{
			try
			{
				// 레시피 별 언로드PnP 사이즈, 포지션 지정
				int nCol = int.Parse(UI.SubpageRecipe.GetRecipeData((int)DEVICE.TrayColCnt)) / 1000;
				int nTrayPitch;
				
				TABLE_MOTOR	table	= new TABLE_MOTOR();
				tsPosLut[]	tsLutX	= new tsPosLut[0];
				tsPosLut[]	tsLutY	= new tsPosLut[0];

				switch (nCol)
				{
					case 5:		// LPCAMM
						{
							ucUldPnPX._InsideGap	= new Point(2,0);
							ucUldPnPX._InsideMargin	= new Thickness(5,5,5,5);
							ucUldPnPX.Width			= 122;
							ucUldPnPX.Height		= 60;

							SM.SHM.GetMotorTableData((uint)MT.ULDPnPX, ref table);
							nTrayPitch = table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_LAST] - table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST];
							tsLutX	= new tsPosLut[] {
														//new tsPosLut(-50,	table.nPos[(int)SeqUnloadPnP.PosX.READY]),
														new tsPosLut(-330,	table.nPos[(int)SeqUnloadPnP.PosX.BUFFER]),
														new tsPosLut(-602,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE1_PICK_FIRST]),
														new tsPosLut(-488,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE1_PICK_FIRST] + nTrayPitch),
														new tsPosLut(-392,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST]),
														new tsPosLut(-278,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST] + nTrayPitch),
														new tsPosLut(-116,	table.nPos[(int)SeqUnloadPnP.PosX.REJECT_PICK_FIRST]),
														new tsPosLut(-2,	table.nPos[(int)SeqUnloadPnP.PosX.REJECT_PICK_FIRST] + nTrayPitch), // 각 제품별 제품 First, Last 간격이 다름
													};
						}
						break;

					case 8:		// OES 111.49
						{
							ucUldPnPX._InsideGap	= new Point(2,0);
							ucUldPnPX._InsideMargin	= new Thickness(6,5,6,5);
							ucUldPnPX.Width			= 80;
							ucUldPnPX.Height		= 60;

							SM.SHM.GetMotorTableData((uint)MT.ULDPnPX, ref table);
							nTrayPitch = table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_LAST] - table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST];
							tsLutX	= new tsPosLut[] {
														//new tsPosLut(-50,	table.nPos[(int)SeqUnloadPnP.PosX.READY]),
														new tsPosLut(-351,	table.nPos[(int)SeqUnloadPnP.PosX.BUFFER]),
														new tsPosLut(-644,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE1_PICK_FIRST]),
														new tsPosLut(-520,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE1_PICK_FIRST] + nTrayPitch),
														new tsPosLut(-434,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST]),
														new tsPosLut(-311,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST] + nTrayPitch),
														new tsPosLut(-158,	table.nPos[(int)SeqUnloadPnP.PosX.REJECT_PICK_FIRST]),
														new tsPosLut(-34,	table.nPos[(int)SeqUnloadPnP.PosX.REJECT_PICK_FIRST] + nTrayPitch), // 각 제품별 제품 First, Last 간격이 다름
													};
						}
						break;

					case 10:	// OES 30mm, OES 42mm, OES 110mm
						{
							ucUldPnPX._InsideGap	= new Point(2,0);
							ucUldPnPX._InsideMargin	= new Thickness(5,5,5,5);
							ucUldPnPX.Width			= 66;
							ucUldPnPX.Height		= 60;

							SM.SHM.GetMotorTableData((uint)MT.ULDPnPX, ref table);
							nTrayPitch = table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_LAST] - table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST];
							tsLutX	= new tsPosLut[] {
														//new tsPosLut(-50,	table.nPos[(int)SeqUnloadPnP.PosX.READY]),
														new tsPosLut(-358,	table.nPos[(int)SeqUnloadPnP.PosX.BUFFER]),
														new tsPosLut(-658,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE1_PICK_FIRST]),
														new tsPosLut(-532,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE1_PICK_FIRST] + nTrayPitch),
														new tsPosLut(-448,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST]),
														new tsPosLut(-321,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST] + nTrayPitch),
														new tsPosLut(-172,	table.nPos[(int)SeqUnloadPnP.PosX.REJECT_PICK_FIRST]),
														new tsPosLut(-45,	table.nPos[(int)SeqUnloadPnP.PosX.REJECT_PICK_FIRST] + nTrayPitch), // 각 제품별 제품 First, Last 간격이 다름
													};
						}
						break;

					case 12:	// OES 80mm
						{
							ucUldPnPX._InsideGap	= new Point(12,0);
							ucUldPnPX._InsideMargin	= new Thickness(5,5,5,5);
							ucUldPnPX.Width			= 92;
							ucUldPnPX.Height		= 60;

							SM.SHM.GetMotorTableData((uint)MT.ULDPnPX, ref table);
							nTrayPitch = table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_LAST] - table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST];
							tsLutX	= new tsPosLut[] {
														//new tsPosLut(-50,	table.nPos[(int)SeqUnloadPnP.PosX.READY]),
														new tsPosLut(-345,	table.nPos[(int)SeqUnloadPnP.PosX.BUFFER]),
														new tsPosLut(-632,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE1_PICK_FIRST]),
														new tsPosLut(-502,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE1_PICK_FIRST] + nTrayPitch),
														new tsPosLut(-422,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST]),
														new tsPosLut(-292,	table.nPos[(int)SeqUnloadPnP.PosX.TABLE2_PICK_FIRST] + nTrayPitch),
														new tsPosLut(-146,	table.nPos[(int)SeqUnloadPnP.PosX.REJECT_PICK_FIRST]),
														new tsPosLut(-16,	table.nPos[(int)SeqUnloadPnP.PosX.REJECT_PICK_FIRST] + nTrayPitch), // 각 제품별 제품 First, Last 간격이 다름
													};
						}
						break;
				}

				stMotorData[(int)teMotor.ULDPnPX].SetLutPos(tsLutX, tsLutY, true);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	SetLdPnP()
		{
			try
			{
				// 레시피 별 로드PnP 사이즈, 포지션 지정
				int nCol = int.Parse(UI.SubpageRecipe.GetRecipeData((int)DEVICE.TrayColCnt)) / 1000;
				int nTrayPitch;
				
				TABLE_MOTOR	table	= new TABLE_MOTOR();
				tsPosLut[]	tsLutX	= new tsPosLut[0];
				tsPosLut[]	tsLutY	= new tsPosLut[0];

				switch (nCol)
				{
					case 5:		// LPCAMM(트레이 방향이 다르게 투입)
						{
							ucLdPnPX._InsideGap		= new Point(2,0);
							ucLdPnPX._InsideMargin	= new Thickness(5,5,5,5);
							ucLdPnPX.Width			= 122;
							ucLdPnPX.Height			= 60;

							SM.SHM.GetMotorTableData((uint)MT.LDPnPX, ref table);
							nTrayPitch = table.nPos[(int)SeqLoadPnP.PosX.TABLE1_LAST] - table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST];
							tsLutX	= new tsPosLut[] {
														//new tsPosLut(1,		table.nPos[(int)SeqLoadPnP.PosX.READY]),
														new tsPosLut(13,	table.nPos[(int)SeqLoadPnP.PosX.BUFFER1]),
														new tsPosLut(123,	table.nPos[(int)SeqLoadPnP.PosX.BUFFER2]),
														new tsPosLut(261,	table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST]),
														//new tsPosLut(375,	table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST] + nTrayPitch), 
														new tsPosLut(336,	table.nPos[(int)SeqLoadPnP.PosX.TABLE2_FIRST]),
														new tsPosLut(450,	table.nPos[(int)SeqLoadPnP.PosX.TABLE2_FIRST] + nTrayPitch) // 각 제품별 제품 First, Last 간격이 다름
													};
						}
						break;

					case 8:		// OES 111.49
						{
							ucLdPnPX._InsideGap		= new Point(2,0);
							ucLdPnPX._InsideMargin	= new Thickness(6,5,6,5);
							ucLdPnPX.Width			= 80;
							ucLdPnPX.Height			= 60;

							SM.SHM.GetMotorTableData((uint)MT.LDPnPX, ref table);
							nTrayPitch = table.nPos[(int)SeqLoadPnP.PosX.TABLE1_LAST] - table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST];
							tsLutX	= new tsPosLut[] {
														//new tsPosLut(1,		table.nPos[(int)SeqLoadPnP.PosX.READY]),
														new tsPosLut(34,	table.nPos[(int)SeqLoadPnP.PosX.BUFFER1]),
														new tsPosLut(144,	table.nPos[(int)SeqLoadPnP.PosX.BUFFER2]),
														new tsPosLut(261,	table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST]),
														//new tsPosLut(384,	table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST] + nTrayPitch), 
														new tsPosLut(335,	table.nPos[(int)SeqLoadPnP.PosX.TABLE2_FIRST]),
														new tsPosLut(460,	table.nPos[(int)SeqLoadPnP.PosX.TABLE2_FIRST] + nTrayPitch) // 각 제품별 제품 First, Last 간격이 다름
													};
						}
						break;

					case 10:	// OES 30mm, OES 42mm, OES 110mm
						{
							ucLdPnPX._InsideGap		= new Point(2,0);
							ucLdPnPX._InsideMargin	= new Thickness(5,5,5,5);
							ucLdPnPX.Width			= 66;
							ucLdPnPX.Height			= 60;

							SM.SHM.GetMotorTableData((uint)MT.LDPnPX, ref table);
							nTrayPitch = table.nPos[(int)SeqLoadPnP.PosX.TABLE1_LAST] - table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST];
							tsLutX	= new tsPosLut[] {
														//new tsPosLut(1,		table.nPos[(int)SeqLoadPnP.PosX.READY]),
														new tsPosLut(41,	table.nPos[(int)SeqLoadPnP.PosX.BUFFER1]),
														new tsPosLut(151,	table.nPos[(int)SeqLoadPnP.PosX.BUFFER2]),
														new tsPosLut(261,	table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST]),
														//new tsPosLut(389,	table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST] + nTrayPitch),
														new tsPosLut(336,	table.nPos[(int)SeqLoadPnP.PosX.TABLE2_FIRST]),
														new tsPosLut(464,	table.nPos[(int)SeqLoadPnP.PosX.TABLE2_FIRST] + nTrayPitch) // 각 제품별 제품 First, Last 간격이 다름
													};
						
						}
						break;

					case 12:	// OES 80mm
						{
							ucLdPnPX._InsideGap		= new Point(12,0);
							ucLdPnPX._InsideMargin	= new Thickness(5,5,5,5);
							ucLdPnPX.Width			= 92;
							ucLdPnPX.Height			= 60;

							SM.SHM.GetMotorTableData((uint)MT.LDPnPX, ref table);
							nTrayPitch = table.nPos[(int)SeqLoadPnP.PosX.TABLE1_LAST] - table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST];
							tsLutX	= new tsPosLut[] {
														//new tsPosLut(1,		table.nPos[(int)SeqLoadPnP.PosX.READY]),
														new tsPosLut(28,	table.nPos[(int)SeqLoadPnP.PosX.BUFFER1]),
														new tsPosLut(138,	table.nPos[(int)SeqLoadPnP.PosX.BUFFER2]),
														new tsPosLut(261,	table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST]),
														//new tsPosLut(391,	table.nPos[(int)SeqLoadPnP.PosX.TABLE1_FIRST] + nTrayPitch), 
														new tsPosLut(335,	table.nPos[(int)SeqLoadPnP.PosX.TABLE2_FIRST]),
														new tsPosLut(466,	table.nPos[(int)SeqLoadPnP.PosX.TABLE2_FIRST] + nTrayPitch) // 각 제품별 제품 First, Last 간격이 다름
													};
						}
						break;
				}

				stMotorData[(int)teMotor.LDPnPX].SetLutPos(tsLutX, tsLutY, true);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	SetZPos(MT mt)
		{
			try
			{
				TABLE_MOTOR table = new TABLE_MOTOR();

				switch (mt)
				{
					case MT.LDTrayLiftZ:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucLDLiftZ._Location_Z_Down	= table.nPos[(int)SeqTrayLoader.PosZ.DOWN];
							ucLDLiftZ._Location_Z_Up	= table.nPos[(int)SeqTrayLoader.PosZ.UP];
						}
						break;
					case MT.LDTrayTransZ:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucLdTrans._Location_Z_Down	= table.nPos[(int)SeqLoadTransfer.PosZ.LOADCONVLIMIT];
							ucLdTrans._Location_Z_Up	= table.nPos[(int)SeqLoadTransfer.PosZ.READY];
						}
						break;
					case MT.LDPnPZ:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucLdPnPX._Location_Z_Down	= table.nPos[(int)SeqLoadPnP.PosZ.TABLE2];
							ucLdPnPX._Location_Z_Up		= table.nPos[(int)SeqLoadPnP.PosZ.READY];
						}
						break;
					case MT.LDInspPnPZ:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucLdInspPnPX._Location_Z_Down	= table.nPos[(int)SeqLoadInspPnP.PosZ.INSP2];
							ucLdInspPnPX._Location_Z_Up		= table.nPos[(int)SeqLoadInspPnP.PosZ.READY];
						}
						break;
					case MT.LDTable1Z:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucLdTable1Z._Location_Z_Down	= table.nPos[(int)SeqLoadTrayTable1.PosZ.DOWN];
							ucLdTable1Z._Location_Z_Up		= table.nPos[(int)SeqLoadTrayTable1.PosZ.UP];
						}
						break;
					case MT.LDTable2Z:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucLdTable2Z._Location_Z_Down	= table.nPos[(int)SeqLoadTrayTable2.PosZ.DOWN];
							ucLdTable2Z._Location_Z_Up		= table.nPos[(int)SeqLoadTrayTable2.PosZ.UP];
						}
						break;
					case MT.ULDTrayLiftZ:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucULDLiftZ._Location_Z_Down	= table.nPos[(int)SeqTrayUnloader.tePosLift.DOWN];
							ucULDLiftZ._Location_Z_Up	= table.nPos[(int)SeqTrayUnloader.tePosLift.UP];
						}
						break;
					case MT.ULDTrayTransZ:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucUldTrans._Location_Z_Down	= table.nPos[(int)SeqUnloadTransfer.PosZ.UNLOADCONVLIMIT];
							ucUldTrans._Location_Z_Up	= table.nPos[(int)SeqUnloadTransfer.PosZ.READY];
						}
						break;
					case MT.ULDPnPZ:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucUldPnPX._Location_Z_Down	= table.nPos[(int)SeqUnloadPnP.PosZ.TABLE2_PLACE];
							ucUldPnPX._Location_Z_Up	= table.nPos[(int)SeqUnloadPnP.PosZ.READY];
						}
						break;
					case MT.ULDInspPnPZ:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucUldInspPnPX._Location_Z_Down	= table.nPos[(int)SeqUnloadInspPnP.PosZ.INSP2];
							ucUldInspPnPX._Location_Z_Up	= table.nPos[(int)SeqUnloadInspPnP.PosZ.READY];
						}
						break;
					case MT.ULDTable1Z:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucUldTable1Z._Location_Z_Down	= table.nPos[(int)SeqUnloadTrayTable.PosZ.DOWN];
							ucUldTable1Z._Location_Z_Up		= table.nPos[(int)SeqUnloadTrayTable.PosZ.UP];
						}
						break;
					case MT.ULDTable2Z:
						{
							SM.SHM.GetMotorTableData((uint)mt, ref table);
							ucUldTable2Z._Location_Z_Down	= table.nPos[(int)SeqUnloadTrayTable.PosZ.DOWN];
							ucUldTable2Z._Location_Z_Up		= table.nPos[(int)SeqUnloadTrayTable.PosZ.UP];
						}
						break;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		//자재 색상
		private	void	SetProdColor(FlowPart Flow, ucFigureArray uc)
		{
			try
			{
				List<ucFigureArray.tsDrawInfo>	info	= new List<ucFigureArray.tsDrawInfo>();
				Color	color	= Colors.White;
				int		nProdCnt = SM.FMP[Flow].GetMaxProdCount();

				if (SM.FMP.IsFlow(Flow) == true  &&  SM.FMP[Flow].GetExistCount() == 0)
				{
					uc._ColorBack	= Colors.Yellow;
				}
				else
				{
					uc._ColorBack	= Colors.White;
				}

				for (int i = 0; i < nProdCnt; i++)
				{
					switch (SM.FMP[Flow].GetProdColorResult(i))
					{
						case ProdColorResult.None:			color = m_NoneColor;		break;
						case ProdColorResult.Unknown:		color = m_UnknownColor;		break;
						case ProdColorResult.NG:			color = m_MachineNGColor;			break;
						case ProdColorResult.OK:			color = m_OkColor;			break;
						case ProdColorResult.USERNG:		color = m_UserNGColor;		break;
						case ProdColorResult.USEROK:		color = m_OkColor;		break;
						case ProdColorResult.MISMATCH:		color = m_MisMatchColor;		break;
						default:							color = m_NoneColor;		break;
					}

					if (IsPartNotUsed(Flow, i) == true)
					{
						if (color == Colors.White)
						{
							color	= Colors.Gray;
						}
					}

					info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Black, color));
				}

				uc._SetInsideFigure(info);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		//트레이 색상
		private	void	SetTrayColor(FlowTray Flow, ucFigureArray uc)
		{
			try
			{
				List<ucFigureArray.tsDrawInfo>	info	= new List<ucFigureArray.tsDrawInfo>();
				Color	color	= Colors.White;

				IsTrayNotUsed(Flow);

				switch (Flow)
				{
					case FlowTray.LDTrans:
					case FlowTray.ULDTrans:
						{
							if (SM.FMT.IsFlow(Flow) == false)	//트레이 없을때
							{
								color = m_NoneColor;
							}
							else	//트레이 있을 때
							{
								if (SM.FMT[Flow].IsEmpty() == true)
								{
									color = m_TrayEmptyColor;
								}
								else
								{
									for (int i = 0; i < SM.FMT[Flow].GetInfo().MaxCntProdInTray; i++)
									{
										if (SM.FMT[Flow].GetInfo().ProdInfo[i].Exist == false) continue;

										if (SM.FMT[Flow].GetProdColorResult(i) == ProdColorResult.OK ||
											SM.FMT[Flow].GetProdColorResult(i) == ProdColorResult.USEROK)
										{
											color = m_TrayOKColor;
										}
										else
										{
											color = m_TrayUnknownColor;
											break;
										}
									}
								}
							}

							info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Black, color));
						}
						break;
					default:
						{
							if (SM.FMT.IsFlow(Flow) == false)
							{
								stProdInfo[] tempInfo = SM.FMT[Flow].GetInfo().ProdInfo;
								for (int i = 0; i < tempInfo.Length; i++)
								{
									info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Black, Colors.LightGray));
								}
							}
							else
							{
								stProdInfo[]	tsInfo	= SM.FMT[Flow].GetInfo().ProdInfo;

								for (int i = 0; i < tsInfo.Length; i++)
								{
									switch (SM.FMT[Flow].GetProdColorResult(i))
									{
										case ProdColorResult.None:			color = m_NoneColor;		break;
										case ProdColorResult.Unknown:		color = m_UnknownColor;		break;
										case ProdColorResult.NG:			color = m_MachineNGColor;	break;
										case ProdColorResult.OK:			color = m_OkColor;			break;
										case ProdColorResult.USERNG:		color = m_UserNGColor;		break;
										case ProdColorResult.USEROK:		color = m_OkColor;			break;
										default:							color = m_NoneColor;		break;
									}

									info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Black, color));
								}
							}
							
						}
						break;
				}

				uc._SetInsideFigure(info);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private	bool	IsPartNotUsed(FlowPart Flow, int nIndex)
		{
			try
			{
				switch (Flow)
				{
					case FlowPart.LDPnP:
						{
							if (SM.SHM.IsUsePart(UsePartName.Picker1+nIndex, UsePartMode.Enable) == false)			return true;
						}
						break;
					
					case FlowPart.ULDPnP:
						{
							if (SM.SHM.IsUsePart(UsePartName.Picker1 + nIndex, UsePartMode.Enable) == false)			return true;
						}
						break;
					
					case FlowPart.LDBuffer1:
						{
							if		(SM.SHM.IsUsePart(UsePartName.LDBuf1, UsePartMode.Enable) == false)
							{
								ucLDBuf1Y._ColorBack	= Colors.Gray;
								return true;
							}
							else if (SM.FMP.IsFlow(FlowPart.LDBuffer1) == true  &&  SM.FMP[FlowPart.LDBuffer1].GetExistCount() == 0)
							{
								ucLDBuf1Y._ColorBack	= Colors.Yellow;
							}
							else
							{
								ucLDBuf1Y._ColorBack	= Colors.White;
							}
						}
						break;

					case FlowPart.LDBuffer2:
						{
							if		(SM.SHM.IsUsePart(UsePartName.LDBuf2, UsePartMode.Enable) == false)
							{
								ucLDBuf2Y._ColorBack	= Colors.Gray;
								return true;
							}
							else if (SM.FMP.IsFlow(FlowPart.LDBuffer2) == true  &&  SM.FMP[FlowPart.LDBuffer2].GetExistCount() == 0)
							{
								ucLDBuf2Y._ColorBack	= Colors.Yellow;
							}
							else
							{
								ucLDBuf2Y._ColorBack	= Colors.White;
							}
						}
						break;

					case FlowPart.Insp1:
						{
							if		(SM.SHM.IsUsePart(UsePartName.Insp1, UsePartMode.Enable) == false)
							{
								ucInspPlateY1._ColorBack	= Colors.Gray;
								ucInspPlateY1._FigureType	= teFigureArrayType.Rectangle;
								return true;
							}
							else if (SM.FMP.IsFlow(FlowPart.Insp1) == true  &&  SM.FMP[FlowPart.Insp1].GetExistCount() == 0)
							{
								ucInspPlateY1._ColorBack	= Colors.Yellow;
								ucInspPlateY1._FigureType	= teFigureArrayType.Rectangle;
							}
							else
							{
								ucInspPlateY1._ColorBack	= Colors.White;
								ucInspPlateY1._FigureType	= teFigureArrayType.Image;
							}
						}
						break;

					case FlowPart.Insp2:
						{
							if (SM.SHM.IsUsePart(UsePartName.Insp2, UsePartMode.Enable) == false)
							{
								ucInspPlateY2._ColorBack	= Colors.Gray;
								ucInspPlateY2._FigureType	= teFigureArrayType.Rectangle;
								return true;
							}
							else if (SM.FMP.IsFlow(FlowPart.Insp2) == true  &&  SM.FMP[FlowPart.Insp2].GetExistCount() == 0)
							{
								ucInspPlateY2._ColorBack	= Colors.Yellow;
								ucInspPlateY2._FigureType	= teFigureArrayType.Rectangle;
							}
							else
							{
								ucInspPlateY2._ColorBack	= Colors.White;
								ucInspPlateY2._FigureType	= teFigureArrayType.Image;
							}
						}
						break;
				}

				return false;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private	bool	IsTrayNotUsed(FlowTray Flow)
		{
			try
			{
				switch (Flow)
				{
					case FlowTray.LDTable1:
						{
							if (SM.SHM.IsUsePart(UsePartName.LDTable1, UsePartMode.Enable) == false)
							{
								gbLDTable1.Background	= Brushes.Gray;
								return true;
							}
							else
							{
								gbLDTable1.Background	= null;
							}
						}
						break;
					
					case FlowTray.LDTable2:
						{
							if (SM.SHM.IsUsePart(UsePartName.LDTable2, UsePartMode.Enable) == false)
							{
								gbLDTable2.Background	= Brushes.Gray;
								return true;
							}
							else
							{
								gbLDTable2.Background	= null;
							}
						}
						break;

					case FlowTray.ULDTable1:
						{
							if (SM.SHM.IsUsePart(UsePartName.ULDTable1, UsePartMode.Enable) == false)
							{
								gbULDTable1.Background	= Brushes.Gray;
								return true;
							}
							else
							{
								gbULDTable1.Background	= null;
							}
						}
						break;

					case FlowTray.ULDTable2:
						{
							if (SM.SHM.IsUsePart(UsePartName.ULDTable2, UsePartMode.Enable) == false)
							{
								gbULDTable2.Background	= Brushes.Gray;
								return true;
							}
							else
							{
								gbULDTable2.Background	= null;
							}
						}
						break;
				}

				return false;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		//LDLift01 : 리프트 자재 존재 여부 , LDConveyor01 : 컨베이어쪽 자재 존재 여부
		private int		GetTrayExist(FlowTray flow)
		{
			try
			{
				int nTrayMaxCount = stLotCnt.Max;

				//Lift 트레이 Exist
				for (int i = nTrayMaxCount -1; i >= 0; i--)
				{
					if (SM.FMT.IsFlow(flow + i))
					{
						return i;
					}
				}
				return -1;
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return -1;
			}
		}

		private void	CapVisibleChange(ucFigure Cap, bool visible)
		{
			int AniSpeed = 50;
			if (visible)
			{
				if (Cap.Visibility != Visibility.Visible && Cap._IsRunMove() == false)
				{
					Cap.Visibility = Visibility.Visible;
					Cap._RunMove(tePosMode.Abs, new Point(0, 0), (50 / AniSpeed) * 1000);
				}
			}
			else
			{
				if (Cap.Visibility == Visibility.Visible && Cap._IsRunMove() == false)
					Cap._RunMove(tePosMode.Abs, new Point(0, -50), (50 / AniSpeed) * 1000);
			}


		}
		
		private	void	ChangeExistTrayPocket(object sender, tsFigureArrayMouseButtonEventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)			return;

				ucFigureArray		uc			= (ucFigureArray)sender;
				frmListBoxSelect	ltbox		= new frmListBoxSelect();
				List<string>		ltlist		= new List<string>();
				FlowTray			flow		= FlowTray.Empty;
				int					ncnt_row	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;
				int					ncnt_col	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
				int					x			= e.Index % ncnt_col;
				int					y			= e.Index / ncnt_col;

				switch (uc.Name)
				{
					case "ucLdTray1" :
						ltlist	= new List<string>() {"Edit", "None", "Unknown"};
						flow	= FlowTray.LDTable1;
						break;

					case "ucLdTray2" :
						ltlist	= new List<string>() {"Edit", "None", "Unknown"};
						flow	= FlowTray.LDTable2;
						break;

					case "ucUldTray1" :
						ltlist	= new List<string>() {"Edit", "None", "NG", "OK" };
						flow	= FlowTray.ULDTable1;
						break;

					case "ucUldTray2" :
						ltlist	= new List<string>() {"Edit", "None", "NG", "OK" };
						flow	= FlowTray.ULDTable2;
						break;

					case "ucRjTray" :
						ltlist	= new List<string>() {"Edit", "None", "NG", "OK" };
                        flow    = FlowTray.Reject;
						break;

					default :
						return;
				}

				ProdStatus	status				= SM.FMT[flow].GetProdStatus(e.Index);
				string			str_tray_id			= SM.FMT[flow].GetInfo().TrayID;
				string			str_tray_Barcode	= SM.FMT[flow].GetInfo().TrayBarcode;
				string			str_KeyValue		= SM.FMT[flow].GetInfo().KeyValue;
			
				if	(ltbox.ShowDialog(ltlist, status.ToString()) == System.Windows.Forms.DialogResult.OK)
				{
					switch(ltbox.SelItem)
					{
						case "Edit" :
							frmProductEditPopup pePopup = new frmProductEditPopup();
							if(pePopup.ShowDialog(teProductPos.Tray, stCopyProdInfo, IsCopyDone, e.Index , flow, FlowPart.Max) == System.Windows.Forms.DialogResult.Cancel)
							{ 
								if(pePopup.m_IsCopyDone == true)
								{ 
									stCopyProdInfo			= pePopup.m_CopyProdInfo;
									IsCopyDone				= pePopup.m_IsCopyDone;
								}
								
								return;
							}

							break;
						case "None":
						case "Unknown":
						case "OK":
						case "NG":
							status = (ProdStatus)Enum.Parse(typeof(ProdStatus), ltbox.SelItem);
							StatusChangeInTray(flow, e.Index, status);
							
							stProdInfo info = new stProdInfo();
							
							if (status != ProdStatus.None)
							{
								if (SM.FMT[flow].GetInfo().ProdInfo[e.Index].Exist == true)
								{
									info			= SM.FMT[flow].GetInfo().ProdInfo[e.Index];
									info.ResultTop	= (int)status;
									info.ResultBtm	= (int)status;
								}
								else
								{
									info.Exist				= true;
									info.TrayRow			= x;
									info.TrayCol			= y;
									info.ResultTop			= (int)status;
									info.ResultBtm			= (int)status;
									info.VerifyTopDone		= true;
									info.VerifyBtmDone		= true;
									info.TrayID				= str_tray_id;
									info.ProdID				= string.Format("{0}_{1}_{2}",str_KeyValue, str_tray_id, (e.Index+1));
									info.KeyValue			= str_KeyValue;
									info.InspTimeTop		= string.Empty;
									info.InspTimeBtm		= string.Empty;
									info.SerialNumber		= string.Empty;
									info.TrayBarcode		= str_tray_Barcode;
									info.PickStatus			= (int)ProdPickStatus.Normal;
									info.HandlingStatus		= (int)ProdHandlingStatus.Normal;
								}
								
								SM.FMT[flow].SetProd(e.Index, info);
							}
							else
							{
								SM.FMT[flow].ClearProd(e.Index);
							}
							
							break;
					}

					UI.Logs.Write(LOGPART.DATA, string.Format("UI Tray Pocket Flow Map Change. Mode: {0}, Flow: {1}, Index: {2}", ltbox.SelItem, flow.ToString(), e.Index));
				}
			}
			catch (Exception ex)
			{
				 UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ChangeExistProd(object sender, tsFigureArrayMouseButtonEventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)			return;
				if (Program.m_PasswordMode == PW_MODE.OPER)			return;

				ucFigureArray		uc			= (ucFigureArray)sender;
				frmListBoxSelect	ltbox		= new frmListBoxSelect();
				List<string>		ltlist		= new List<string>();
				FlowPart			flow		= FlowPart.Empty;

				switch (uc.Name)
				{
					case "ucLdPnPX" :
						ltlist	= new List<string>() { "None", "Unknown", "Retry Pick"};
						//ltlist	= new List<string>() { "Edit", "None", "Unknown", "Move LDBuffer1", "Move LDBuffer2", "Retry Pick"};
						flow	= FlowPart.LDPnP;
						break;

					case "ucUldPnPX" :
						ltlist	= new List<string>() { "None", "OK", "NG", "Retry Pick Reject"};
						//ltlist	= new List<string>() { "Edit", "None", "OK", "NG", "Move ULDBuffer", "Retry Pick Reject"};
						flow	= FlowPart.ULDPnP;
						break;

					case "ucLDBuf1Y" :
						ltlist	= new List<string>() { "None", "Unknown"};
						//ltlist	= new List<string>() { "Edit", "None", "Unknown", "Move LDPnP", "Move LDInspPnP"};
						flow	= FlowPart.LDBuffer1;
						break;

					case "ucLDBuf2Y" :
						ltlist	= new List<string>() { "None", "Unknown"};
						//ltlist	= new List<string>() { "Edit", "None", "Unknown", "Move LDPnP", "Move LdInspPnP"};
						flow	= FlowPart.LDBuffer2;
						break;

					case "ucUldBufY" :
						ltlist	= new List<string>() { "None", "Unknown", "OK", "NG"};
						//ltlist	= new List<string>() { "Edit", "None", "Unknown", "OK", "NG", "Move ULDInspPnP", "Move ULDPnP"};
						flow	= FlowPart.ULDBuffer;
						break;

					case "ucLdInspPnPX" :
						ltlist	= new List<string>() { "None", "Unknown"};
						//ltlist	= new List<string>() { "Edit", "None", "Unknown", "Move LDBuffer1", "Move LDBuffer2", "Move Insp1", "Move Insp2"};
						flow	= FlowPart.LDInspPnP;
						break;

					case "ucInspPlateY1" :
						ltlist	= new List<string>() { "None", "Unknown", "OK", "NG"};
						//ltlist	= new List<string>() { "Edit", "None", "Unknown", "OK", "NG", "Move LDInspPnP", "Move ULDInspPnP"};
						flow	= FlowPart.Insp1;
						break;

					case "ucInspPlateY2" :
						ltlist	= new List<string>() { "None", "Unknown", "OK", "NG"};
						//ltlist	= new List<string>() { "Edit", "None", "Unknown", "OK", "NG", "Move LDInspPnP", "Move ULDInspPnP"};
						flow	= FlowPart.Insp2;
						break;

					case "ucUldInspPnPX" :
						ltlist	= new List<string>() { "None", "Unknown", "OK", "NG"};
						//ltlist	= new List<string>() { "Edit", "None", "Unknown", "OK", "NG", "Move ULDBuffer", "Move Insp1", "Move Insp2"};
						flow	= FlowPart.ULDInspPnP;
						break;

					default :
						return;
				}

				ProdStatus	status			= SM.FMP[flow].GetProdStatus(e.Index);
				stProdInfo		info			= new stProdInfo();
				string			str_prod_id		= string.Format("{0}_{1}_U_{2}", SM.SHM.GetRunLotInfo(0).KeyValue, SM.SHM.GetRunLotInfo(0).WorkOrder, DateTime.Now.ToString("HHmmss"));
				string			str_tray_sn		= string.Format("{0}_{1}_T_{2}", SM.SHM.GetRunLotInfo(0).KeyValue, SM.SHM.GetRunLotInfo(0).WorkOrder, DateTime.Now.ToString("HHmm"));
				string			str_KeyValue	= string.Format("{0}", SM.SHM.GetRunLotInfo(0).KeyValue);


				if	(ltbox.ShowDialog(ltlist, status.ToString()) == System.Windows.Forms.DialogResult.OK)
				{
					if (status != ProdStatus.None)
					{
						info	= SM.FMP[flow].GetInfo().ProdInfo[e.Index];
					}
					StatusChangeInPart(flow, e.Index, ltbox.SelItem);
					
					switch (ltbox.SelItem)
					{
						case "Edit" :
							frmProductEditPopup pePopup = new frmProductEditPopup();
							if(pePopup.ShowDialog(teProductPos.Part, stCopyProdInfo, IsCopyDone, e.Index , FlowTray.Max, flow) == System.Windows.Forms.DialogResult.Cancel)
							{ 
								if(pePopup.m_IsCopyDone == true)
								{ 
									stCopyProdInfo			= pePopup.m_CopyProdInfo;
									IsCopyDone				= pePopup.m_IsCopyDone;
								}
								
								return;
							}
							break;

						case "None" :
							if (SM.FMP.IsFlow(flow) == false)			return;
							
							SM.FMP[flow].ClearProd(e.Index);
							
							if (SM.FMP[flow].GetExistCount() == 0)			SM.FMP.Clear(flow);
							break;

						case "Unknown" :
							if (SM.FMP.IsFlow(flow) == false)
							{
								SM.FMP.Next(FlowPart.Empty, flow);
								InfoPart.tsInfo	info_p	= SM.FMP[flow].GetInfo();
								info_p.WorkOrder		= SM.SHM.GetRunLotInfo(0).WorkOrder;
								info_p.ProgramID_Top	= SM.SHM.GetRunLotInfo(0).ProgramID_Top;
								info_p.ProgramID_Btm	= SM.SHM.GetRunLotInfo(0).ProgramID_Btm;
								SM.FMP[flow].SetInfo(info_p);
							}

							info.Exist		= true;
							info.ResultTop	= (int)ProdStatus.Unknown;
							info.ResultBtm	= (int)ProdStatus.Unknown;

							if (string.IsNullOrWhiteSpace(info.TrayID) == true)			info.TrayID			= str_tray_sn;
							if (string.IsNullOrWhiteSpace(info.ProdID) == true)			info.ProdID			= str_prod_id;
							if (string.IsNullOrWhiteSpace(info.KeyValue) == true)		info.KeyValue		= str_KeyValue;
							if (string.IsNullOrWhiteSpace(info.SerialNumber) == true)	info.SerialNumber	= "";
							if (string.IsNullOrWhiteSpace(info.TrayBarcode) == true)	info.TrayBarcode	= "";
							if (string.IsNullOrWhiteSpace(info.InspTimeTop) == true)	info.InspTimeTop	= "";
							if (string.IsNullOrWhiteSpace(info.InspTimeBtm) == true)	info.InspTimeBtm	= "";
							
							SM.FMP[flow].SetProd(e.Index, info);
							break;
						
						case "OK" :
							if (SM.FMP.IsFlow(flow) == false)
							{
								SM.FMP.Next(FlowPart.Empty, flow);
								InfoPart.tsInfo	info_p	= SM.FMP[flow].GetInfo();
								info_p.WorkOrder		= SM.SHM.GetRunLotInfo(0).WorkOrder;
								info_p.ProgramID_Top	= SM.SHM.GetRunLotInfo(0).ProgramID_Top;
								info_p.ProgramID_Btm	= SM.SHM.GetRunLotInfo(0).ProgramID_Btm;
								SM.FMP[flow].SetInfo(info_p);
							}
							
							info.Exist		= true;
							info.ResultTop	= (int)ProdStatus.OK;
							info.ResultBtm	= (int)ProdStatus.OK;

							if (string.IsNullOrWhiteSpace(info.TrayID) == true)			info.TrayID	= str_tray_sn;
							if (string.IsNullOrWhiteSpace(info.ProdID) == true)			info.ProdID	= str_prod_id;
							if (string.IsNullOrWhiteSpace(info.KeyValue) == true)		info.KeyValue	= str_KeyValue;
							if (string.IsNullOrWhiteSpace(info.SerialNumber) == true)	info.SerialNumber	= "";
							if (string.IsNullOrWhiteSpace(info.TrayBarcode) == true)	info.TrayBarcode	= "";
							if (string.IsNullOrWhiteSpace(info.InspTimeTop) == true)	info.InspTimeTop	= "";
							if (string.IsNullOrWhiteSpace(info.InspTimeBtm) == true)	info.InspTimeBtm	= "";
							
							SM.FMP[flow].SetProd(e.Index, info);
							break;
						
						case "NG" :
							if (SM.FMP.IsFlow(flow) == false)
							{
								SM.FMP.Next(FlowPart.Empty, flow);
								InfoPart.tsInfo	info_p	= SM.FMP[flow].GetInfo();
								info_p.WorkOrder		= SM.SHM.GetRunLotInfo(0).WorkOrder;
								info_p.ProgramID_Top	= SM.SHM.GetRunLotInfo(0).ProgramID_Top;
								info_p.ProgramID_Btm	= SM.SHM.GetRunLotInfo(0).ProgramID_Btm;
								SM.FMP[flow].SetInfo(info_p);
							}
							
							info.Exist		= true;
							info.ResultTop	= (int)ProdStatus.NG;
							info.ResultBtm	= (int)ProdStatus.NG;
							info.VerifyBtmDone	= true;
							info.VerifyTopDone	= true;

							if (string.IsNullOrWhiteSpace(info.TrayID) == true)			info.TrayID	= str_tray_sn;
							if (string.IsNullOrWhiteSpace(info.ProdID) == true)			info.ProdID	= str_prod_id;
							if (string.IsNullOrWhiteSpace(info.KeyValue) == true)		info.KeyValue	= str_KeyValue;
							if (string.IsNullOrWhiteSpace(info.SerialNumber) == true)	info.SerialNumber	= "";
							if (string.IsNullOrWhiteSpace(info.TrayBarcode) == true)	info.TrayBarcode	= "";
							if (string.IsNullOrWhiteSpace(info.InspTimeTop) == true)	info.InspTimeTop	= "";
							if (string.IsNullOrWhiteSpace(info.InspTimeBtm) == true)	info.InspTimeBtm	= "";
							
							SM.FMP[flow].SetProd(e.Index, info);
							break;
						
						case "Retry Pick" :
							if (SM.SHM.IsManualRunning() == false)		SM.SHM.StartManual();
							
							if (uc.Name == "ucLdPnPX")			SM.SHM.SetManualNum(MANUAL.LoadPnPPick1+e.Index);
							break;

						case "Retry Pick Reject" :
							if (SM.SHM.IsManualRunning() == false)		SM.SHM.StartManual();

							if (uc.Name == "ucUldPnPX")			SM.SHM.SetManualNum(MANUAL.UnloadPnPPickReject1+e.Index);
							break;
						
						default :
							string		strtmp		= ltbox.SelItem.Replace("Move ", "");
							FlowPart	flowNext	= (FlowPart)Enum.Parse(typeof(FlowPart), strtmp);

							if (SM.FMP.IsFlow(flow) == false)			return;
							if (SM.FMP.IsFlow(flowNext) == true)		return;

							SM.FMP.Next(flow, flowNext);
							break;
					}

					UI.Logs.Write(LOGPART.DATA, string.Format("UI Part Flow Map Change. Mode: {0}, Flow: {1}, Index: {2}", ltbox.SelItem, flow.ToString(), e.Index));
				}
			}
			catch (Exception ex)
			{
				 UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	ChangeExistTrans(object sender, tsFigureArrayMouseButtonEventArgs e)
		{
			try
			{
				if (SM.SHM.IsBitData(BIT.AutoRun) == true)			return;

				ucFigureArray		uc		= (ucFigureArray)sender;
				frmListBoxSelect	ltbox	= new frmListBoxSelect();
				List<string>		ltlist	= new List<string>();
				FlowTray			flow	= FlowTray.Empty;

				switch (uc.Name)
				{
					case "ucLdTrans" :
						ltlist	= new List<string>() { "None", "CapUncoverDone", "TrayEmptyDone" };
						flow	= FlowTray.LDTrans;
						break;
					case "ucUldTrans" :
						ltlist	= new List<string>() { "None", "CleanDone", "TrayFillDone", "CapCoverDone", "CapUncover", "CapCover" };
						flow	= FlowTray.ULDTrans;
						break;
					default :
						return;
				}

				string	strStatus	= "";
				string	strID		= string.Format("{0}_0", SM.SHM.GetRunLotInfo(0).WorkOrder);
				var		tcinfo		= SM.FMT[flow].GetInfo();

				if		(SM.FMT.IsFlow(flow) == false)											strStatus	= "None";
				else																			strStatus	= SM.FMT[flow].ProcStatus.ToString();

				if	(ltbox.ShowDialog(ltlist, strStatus) == System.Windows.Forms.DialogResult.OK)
				{
					switch (ltbox.SelItem)
					{
						case "None" :
							if (SM.FMT.IsFlow(flow) == false)			return;

							SM.FMT.Clear(flow);
							break;
						default :
							if (SM.FMT.IsFlow(flow) == false)			SM.FMT.Next(FlowTray.Empty, flow);
							SM.FMT[flow].ProcStatus	= (ProcStatus)Enum.Parse(typeof(ProcStatus), ltbox.SelItem);

							if (string.IsNullOrWhiteSpace(SM.FMT[flow].GetInfo().KeyValue) == true)			tcinfo.KeyValue	= SM.SHM.GetRunLotInfo(0).KeyValue;
							if (string.IsNullOrWhiteSpace(SM.FMT[flow].GetInfo().TrayID) == true)			tcinfo.TrayID	= strID;

							break;
					}

					if (SM.FMT.IsFlow(flow) == true)			SM.FMT[flow].SetInfo(tcinfo);
				}
			}
			catch (Exception ex)
			{
				 UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private void	StatusChangeInTray(FlowTray flow, int nIdx, ProdStatus AfterState)
		{
			ProdColorResult	BeforeState		= SM.FMT[flow].GetProdColorResult(nIdx);	
			string				strKeyValue		= SM.FMT[flow].GetInfo().KeyValue;

			if (SM.SHM.ContainsRunLotInfo(strKeyValue) == false)	return;
			
			stRunLotInfo		_LotInfo		= SM.SHM.GetRunLotInfo(strKeyValue);

			//제품 생성시
			if (BeforeState == ProdColorResult.None  &&  AfterState != ProdStatus.None)
			{
				_LotInfo.ProdCount = ((int.Parse(_LotInfo.ProdCount)) + 1).ToString();
				
				if ((flow == FlowTray.ULDTable1 ||
					flow == FlowTray.ULDTable2 ||
					flow == FlowTray.Reject) )
				{
					int[] Yield = new int[3] { 0, 0, 0 }; // ok , ng , user ok
					Yield = SM.SHM.GetLotYield(strKeyValue);
					switch (AfterState)
					{
						case ProdStatus.NG: Yield[1] += 1; NDm.Write(eNDM.CurNGProdCount, NDm.Read(eNDM.CurNGProdCount)+1);	break;
						case ProdStatus.OK: Yield[0] += 1; NDm.Write(eNDM.CurOKProdCount, NDm.Read(eNDM.CurOKProdCount)+1);	break;
					}
					SM.SHM.SetLotYield(strKeyValue, Yield);
				}
			}
			//제품 삭제 시
			else if (BeforeState != ProdColorResult.None  &&  AfterState == ProdStatus.None)
			{
				_LotInfo.ProdCount = ((int.Parse(_LotInfo.ProdCount)) - 1).ToString();
				
				if ((flow == FlowTray.ULDTable1 ||
					flow == FlowTray.ULDTable2 ||
					flow == FlowTray.Reject) )
				{
					int[] Yield = new int[3] { 0, 0, 0 }; // ok , ng , user ok
					Yield = SM.SHM.GetLotYield(strKeyValue);
					NDm.Write(eNDM.CurNGProdCount, NDm.Read(eNDM.CurNGProdCount)-1);
					switch (BeforeState)
					{
						case ProdColorResult.OK:		Yield[0] -= 1; NDm.Write(eNDM.CurOKProdCount, NDm.Read(eNDM.CurOKProdCount)-1);		break;
						case ProdColorResult.USEROK:	Yield[0] -= 1; NDm.Write(eNDM.CurUserOKProdCount, NDm.Read(eNDM.CurUserOKProdCount)-1);	break;
						case ProdColorResult.NG:		Yield[1] -= 1; NDm.Write(eNDM.CurNGProdCount, NDm.Read(eNDM.CurNGProdCount)-1);		break;
						case ProdColorResult.USERNG:	Yield[1] -= 1; NDm.Write(eNDM.CurNGProdCount, NDm.Read(eNDM.CurNGProdCount)-1);		break;
					}
					SM.SHM.SetLotYield(strKeyValue, Yield);
				}
			}
			else //상태만 변경하는 경우
			{
				if ((flow == FlowTray.ULDTable1 ||
					flow == FlowTray.ULDTable2 ||
					flow == FlowTray.Reject) )
				{
					int[] Yield = new int[3] { 0, 0, 0 }; // ok , ng , user ok
					Yield = SM.SHM.GetLotYield(strKeyValue);
					switch (BeforeState)
					{
						case ProdColorResult.OK:		Yield[0] -= 1; NDm.Write(eNDM.CurOKProdCount, NDm.Read(eNDM.CurOKProdCount)-1);	 break;
						case ProdColorResult.USEROK:	Yield[0] -= 1; NDm.Write(eNDM.CurOKProdCount, NDm.Read(eNDM.CurOKProdCount)-1);	 break;
						case ProdColorResult.NG:		Yield[1] -= 1; NDm.Write(eNDM.CurNGProdCount, NDm.Read(eNDM.CurNGProdCount)-1);	 break;
						case ProdColorResult.USERNG:	Yield[1] -= 1; NDm.Write(eNDM.CurNGProdCount, NDm.Read(eNDM.CurNGProdCount)-1);	 break;
					}
					switch (AfterState)
					{
						case ProdStatus.OK: Yield[0] += 1; NDm.Write(eNDM.CurOKProdCount, NDm.Read(eNDM.CurOKProdCount)+1);	 break;
						case ProdStatus.NG: Yield[1] += 1; NDm.Write(eNDM.CurNGProdCount, NDm.Read(eNDM.CurNGProdCount)+1);	 break;
					}
					SM.SHM.SetLotYield(strKeyValue, Yield);
				}
			}


			
			SM.SHM.SetRunLotInfo(_LotInfo);
			
			GLog.Add($"Manual State Change In Tray({strKeyValue} - {flow},{nIdx}) : {BeforeState} -> {AfterState} ");
		}

		private void	StatusChangeInPart(FlowPart flow, int nIdx, string AfterState)
		{
			ProdColorResult	BeforeState		= SM.FMP[flow].GetProdColorResult(nIdx);	
			string				strKeyValue		= SM.FMP[flow].GetInfo().KeyValue == "" ?  SM.SHM.GetRunLotInfo(0).KeyValue : SM.FMP[flow].GetInfo().KeyValue ;

			if (SM.SHM.ContainsRunLotInfo(strKeyValue) == false)	return;
			
			stRunLotInfo		_LotInfo		= SM.SHM.GetRunLotInfo(strKeyValue);

			//제품 생성시
			if (BeforeState == ProdColorResult.None  &&  AfterState != "None")
			{
				_LotInfo.ProdCount = ((int.Parse(_LotInfo.ProdCount)) + 1).ToString();
			}
			//제품 삭제 시
			else if (BeforeState != ProdColorResult.None  &&  AfterState == "None")
			{
				_LotInfo.ProdCount = ((int.Parse(_LotInfo.ProdCount)) - 1).ToString();
			}
			
			SM.SHM.SetRunLotInfo(_LotInfo);
			
			GLog.Add($"Manual State Change In Part({strKeyValue} - {flow},{nIdx}) : {BeforeState} -> {AfterState} ");
		}

		#endregion

		#region Timer 

		private	void	UpdateModuleAni()
		{
			try
			{
				for (int i = 0; i < (int)teModule.Max; i++)
				{
					if (stModuleData[i].IsLoading == true)
					{
						stModuleData[i].Loading();
					}
					else if (stModuleData[i].IsUnloading == true)
					{
						stModuleData[i].Unloading();
						// Hidden 이벤트 동작했는데도 Visible 상태이면 강제로 Hidden 해준다
						switch ((teModule)i)
						{
							case teModule.LDTable2:
								{
									if (ucLdTable2._IsRunMove() == false && ucLdTable2._Location != new Point(0, 0) && ucLdTable2.Visibility == Visibility.Visible)
									{
										ucLdTable2.Visibility = Visibility.Hidden;
									}
								}
								break;
						}
					}

					stModuleData[i].StateUpdate();
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateExist()
		{
			try
			{
				//SetRejectBufferColor(teFlowTray.Empty, ucRjBuffer);
				
				SetTrayColor(FlowTray.LDTable1,		ucLdTray1);
				SetTrayColor(FlowTray.LDTable2,		ucLdTray2);
				SetTrayColor(FlowTray.ULDTable1,	ucUldTray1);
				SetTrayColor(FlowTray.ULDTable2,	ucUldTray2);
				SetTrayColor(FlowTray.LDTrans,		ucLdTrans);
				SetTrayColor(FlowTray.ULDTrans,		ucUldTrans);
				SetTrayColor(FlowTray.Reject,		ucRjTray);

				SetTrayColor(FlowTray.LDTable1,		ucLdTable1);
				SetTrayColor(FlowTray.LDTable2,		ucLdTable2);
				SetTrayColor(FlowTray.ULDTable1,	ucUldTable1);
				SetTrayColor(FlowTray.ULDTable2,	ucUldTable2);
				SetTrayColor(FlowTray.Reject,		ucRjTable);

				SetProdColor(FlowPart.LDPnP,		ucLdPnPX);
				SetProdColor(FlowPart.ULDPnP,		ucUldPnPX);
				SetProdColor(FlowPart.LDBuffer1,	ucLDBuf1Y);
				SetProdColor(FlowPart.LDBuffer2,	ucLDBuf2Y);
				SetProdColor(FlowPart.ULDBuffer,	ucUldBufY);
				SetProdColor(FlowPart.LDInspPnP,	ucLdInspPnPX);
				SetProdColor(FlowPart.Insp1,		ucInspPlateY1);
				SetProdColor(FlowPart.Insp2,		ucInspPlateY2);
				SetProdColor(FlowPart.ULDInspPnP,	ucUldInspPnPX);
				
				lbRemainFill.Content	= NDm.Read(eNDM.FillDoneRemainCount).ToString();
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateMotor()
		{
			try
			{
				//모터 업데이트 
				stMotorData[(int)teMotor.TopInsp].Run();
				stMotorData[(int)teMotor.BtmInsp].Run();

				for (int i = 0; i < (int)teMotor.Max; i++)
				{
					if (i == (int)teMotor.TopInsp  ||  i == (int)teMotor.BtmInsp)			continue;
					
					stMotorData[i].UpdatePos();
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		   
		}

		private	void	UpdateMotorZ()
		{
			try
			{
				STATUS_MOTOR table = new STATUS_MOTOR();

				for (MT mt = 0; mt < MT.TOTAL; mt++)
				{
					SM.SHM.GetMotorSatusData((uint)mt, ref table);

					switch (mt)
					{
						case MT.LDTrayLiftZ:	if (ucLDLiftZ._Location_Z != table.nActPos)			ucLDLiftZ._Location_Z		= table.nActPos;	break;
						case MT.LDTrayTransZ:	if (ucLdTrans._Location_Z != table.nActPos)			ucLdTrans._Location_Z		= table.nActPos;	break;
						case MT.LDPnPZ:			if (ucLdPnPX._Location_Z != table.nActPos)			ucLdPnPX._Location_Z		= table.nActPos;	break;
						case MT.LDInspPnPZ:		if (ucLdInspPnPX._Location_Z != table.nActPos)		ucLdInspPnPX._Location_Z	= table.nActPos;	break;
						case MT.LDTable1Z:		if (ucLdTable1Z._Location_Z != table.nActPos)		ucLdTable1Z._Location_Z		= table.nActPos;	break;
						case MT.LDTable2Z:		if (ucLdTable2Z._Location_Z != table.nActPos)		ucLdTable2Z._Location_Z		= table.nActPos;	break;
						case MT.ULDTrayLiftZ:	if (ucULDLiftZ._Location_Z != table.nActPos)		ucULDLiftZ._Location_Z		= table.nActPos;	break;
						case MT.ULDTrayTransZ:	if (ucUldTrans._Location_Z != table.nActPos)		ucUldTrans._Location_Z		= table.nActPos;	break;
						case MT.ULDPnPZ:		if (ucUldPnPX._Location_Z != table.nActPos)			ucUldPnPX._Location_Z		= table.nActPos;	break;
						case MT.ULDInspPnPZ:	if (ucUldInspPnPX._Location_Z != table.nActPos)		ucUldInspPnPX._Location_Z	= table.nActPos;	break;
						case MT.ULDTable1Z:		if (ucUldTable1Z._Location_Z != table.nActPos)		ucUldTable1Z._Location_Z	= table.nActPos;	break;
						case MT.ULDTable2Z:		if (ucUldTable2Z._Location_Z != table.nActPos)		ucUldTable2Z._Location_Z	= table.nActPos;	break;
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateLiftState()
		{
			try
			{
				FlowTray[]						flow_t		= new FlowTray[]		{ FlowTray.LDLift01, FlowTray.LDConv01, FlowTray.ULDLift01, FlowTray.ULDConv01 };
				ucFigureArray[]					uc_f		= new ucFigureArray[]	{ ucLDLift, ucLDConv, ucULDLift, ucULDConv };
				TextBlock[]						uc_t		= new TextBlock[]		{ LDLiftCount, LDConvCount, ULDLiftCount, ULDConvCount };
				int[]							nmax		= new int[]				{ stLotCnt.Max, stLotCnt.Max, stLotCnt.Max, stLotCnt.Max };
				int								nstart_tray	= -1;
				int								ncnt		= 0;
				string							str			= "0/0";

				for (int i = 0; i < flow_t.Length; i++)
				{
					//가장 아래에 있는 트레이
					nstart_tray	= GetTrayExist(flow_t[i]);
					ncnt		= 0;

					if (nstart_tray >= 0)
					{
						// 잔여 트레이 수량
						for (int j = 0; j < nmax[i]; j++)
						{
							if (SM.FMT.IsFlow(flow_t[i] + j) == true)
							{
								ncnt++;
							}
						}
						
						uc_f[i]._CountCol	= 1;
						uc_f[i]._CountRow	= SM.FMT[flow_t[i] + nstart_tray].GetInfo().MaxCntTray < 10 ? 10 : SM.FMT[flow_t[i] + nstart_tray].GetInfo().MaxCntTray;
						uc_f[i]._ColorBack	= m_NoneColor;
					}
					else
					{
						uc_f[i]._CountCol	 = 0;
						uc_f[i]._CountRow	 = 0;
						uc_f[i]._ColorBack	 = Colors.Transparent;
					}

					// 트레이 잔여 수량/최대 수량
					str = string.Format("{0}/{1}", ncnt.ToString(), SM.FMT[flow_t[i] + nstart_tray].GetInfo().MaxCntTray.ToString());
					if (uc_t[i].Text.Equals(str) == false)
						uc_t[i].Text = str;
				}

				for (int i = 0; i < flow_t.Length; i++)
				{
					nstart_tray	= GetTrayExist(flow_t[i]);
					List<ucFigureArray.tsDrawInfo> info = new List<ucFigureArray.tsDrawInfo>();

					for (int j = 0; j < uc_f[i]._CountRow; j++)
					{
						// 10개 미만 체크
						if (j < SM.FMT[flow_t[i] + nstart_tray].GetInfo().MaxCntTray)
						{
							FlowTray flow = FlowTray.Empty;
							if (flow_t[i] == FlowTray.ULDLift01 || flow_t[i] == FlowTray.ULDConv01)
							{
								int nEmptyFlow = SM.FMT.GetExistCountRange(flow_t[i], flow_t[i] + stLotCnt.Max);

								if (nEmptyFlow > j)		flow = flow_t[i] + nEmptyFlow - j - 1;
								else					flow = flow_t[i] + j;
							}
							else
							{
								flow = flow_t[i] + SM.FMT[flow_t[i] + nstart_tray].GetInfo().MaxCntTray - j - 1;
							}
							
							if (SM.FMT.IsFlow(flow) == false)	//트레이 없을때
							{
								info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Black, m_NoneColor));
							}
							else	//트레이 있을 때
							{
								if (SM.FMT[flow].IsEmpty() == true)
								{
									info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Black, m_TrayEmptyColor));
								}
								else
								{
									Color color = Colors.White;
									for (int k = 0; k < SM.FMT[flow].GetInfo().MaxCntProdInTray; k++)
									{
										if (SM.FMT[flow].GetInfo().ProdInfo[k].Exist == false) continue;

										if (SM.FMT[flow].GetProdColorResult(k) == ProdColorResult.OK ||
											SM.FMT[flow].GetProdColorResult(k) == ProdColorResult.USEROK)
										{
											color = m_TrayOKColor;
										}
										else
										{
											color = m_TrayUnknownColor;
											break;
										}
									}

									info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Black, color));
								}
							}
						}
						else
						{
							// 10개 미만 시 투명
							info.Add(new ucFigureArray.tsDrawInfo(1, Colors.Transparent, Colors.Transparent));
						}
					}

					uc_f[i]._SetInsideFigure(info);
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateTrayPocket()
		{
			try
			{
				string strModuleTyle = SM.SHM.GetRecipeData(DEVICE.ModuleType);

				int ncnt_col	= SM.SHM.GetRecipeDataToInt(strModuleTyle , DEVICE.TrayColCnt) / 1000;
				int	ncnt_row	= SM.SHM.GetRecipeDataToInt(strModuleTyle , DEVICE.TrayRowCnt) / 1000;

				ucFigureArray[]	tray		= new ucFigureArray[]	{ ucLdTray1, ucLdTray2, ucUldTray1, ucUldTray2, ucRjTray, ucRjTable, ucLdTable1, ucLdTable2, ucUldTable1, ucUldTable2  };
				FlowTray[]		tray_flow	= new FlowTray[]		{ FlowTray.LDTable1, FlowTray.LDTable2, FlowTray.ULDTable1, FlowTray.ULDTable2 , FlowTray.Reject, FlowTray.Reject, FlowTray.LDTable1, FlowTray.LDTable2, FlowTray.ULDTable1, FlowTray.ULDTable2};

				for (int i = 0; i < tray.Length; i++)
				{
					UpdateTrayCountDisplay(tray[i]);
					if (SM.FMT.IsFlow(tray_flow[i]) == false  &&  i < 6)
					{
						if (tray[i]._CountCol != 1)					tray[i]._CountCol = 1;
						if (tray[i]._CountRow != 1)					tray[i]._CountRow = 1;
					}
					else
					{
						switch (tray_flow[i])
						{

							case FlowTray.Reject: // Reject 트레이는 수동으로 적재하기에 트레이 정보로 Row, Col 표기
								{
									if (tray[i]._CountCol != SM.FMT[tray_flow[i]].GetInfo().ColCnt)		tray[i]._CountCol = SM.FMT[tray_flow[i]].GetInfo().ColCnt;
									if (tray[i]._CountRow != SM.FMT[tray_flow[i]].GetInfo().RowCnt)		tray[i]._CountRow = SM.FMT[tray_flow[i]].GetInfo().RowCnt;
								}
								break;
							default:
								{
									if (tray[i]._CountCol != ncnt_col)			tray[i]._CountCol = ncnt_col;
									if (tray[i]._CountRow != ncnt_row)			tray[i]._CountRow = ncnt_row;
								}
								break;
						}
					}
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private void	UpdateTrayCountDisplay(ucFigureArray uc)
		{
			Grid	grdTray		= new Grid();
			int		nColCount	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000;
			int		nRowCount	= SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000;

			Thickness	TopMargin	= new Thickness(-3,3,-3,0);
			Thickness	BtmMargin	= new Thickness(-3,0,-3,1);

			switch (uc.Name)
			{
				case "ucLdTray1":	grdTray	= GrdLdTray1;		break;
				case "ucLdTray2":	grdTray	= GrdLdTray2;		break;
				case "ucUldTray1":	grdTray	= GrdUldTray1;		break;
				case "ucUldTray2":	grdTray	= GrdUldTray2;		break;
				case "ucRjTray":
					{
						// Reject 트레이는 수동으로 적재하기에 트레이 정보로 Row, Col 표기
						grdTray		= GrdRejectTray;
						nColCount	= SM.FMT[FlowTray.Reject].GetInfo().ColCnt == 0 ? SM.SHM.GetRecipeDataToInt(DEVICE.TrayColCnt) / 1000 : SM.FMT[FlowTray.Reject].GetInfo().ColCnt;
						nRowCount	= SM.FMT[FlowTray.Reject].GetInfo().RowCnt == 0 ? SM.SHM.GetRecipeDataToInt(DEVICE.TrayRowCnt) / 1000 : SM.FMT[FlowTray.Reject].GetInfo().RowCnt;
					}
					break;
				default:										return;
			}
			if (grdTray.RowDefinitions.Count == nRowCount  &&  grdTray.ColumnDefinitions.Count == nColCount)			return;

			grdTray.RowDefinitions.Clear();
			grdTray.ColumnDefinitions.Clear();
			grdTray.Children.Clear();

			for (int i = 0; i < nRowCount; i++)
			{
				grdTray.RowDefinitions.Add(new RowDefinition());
			}
			for (int i = 0; i < nColCount; i++)
			{
				grdTray.ColumnDefinitions.Add(new ColumnDefinition());

				if (i == 0  ||  (i == nColCount-1  &&  (i%5) != 0)  ||  (i%5) == 4) // 처음, 끝, 5의 배수
				{
					//상단
					TextBlock tbTop = new TextBlock();
					tbTop.TextAlignment = TextAlignment.Center;
					tbTop.Text = (i+1).ToString();
					Grid.SetColumn(tbTop,i);

					Grid.SetRow(tbTop,0);
					tbTop.VerticalAlignment = VerticalAlignment.Top;
					tbTop.Margin = TopMargin;

					grdTray.Children.Add(tbTop);

					//하단
					TextBlock tbBottom = new TextBlock();
					tbBottom.TextAlignment = TextAlignment.Center;
					tbBottom.Text = (i+1).ToString();
					Grid.SetColumn(tbBottom,i);

					Grid.SetRow(tbBottom,nRowCount);
					tbBottom.VerticalAlignment = VerticalAlignment.Bottom;
					tbBottom.Margin = BtmMargin;

					grdTray.Children.Add(tbBottom);
				}
			}
		}

		private	void	UpdateControlText()
		{
			try
			{
				// Auto Run Mode
				AutoRunMode RunMode = (AutoRunMode)NDm.Read(eNDM.AutoRunMode);
				if (btnRunMode.Content.Equals(EnumHelper.GetDescription(RunMode)) == false)
				{
					switch (RunMode)
					{
						case AutoRunMode.Normal:
							btnRunMode.Background = Brushes.White;
							btnRunMode.Foreground = Brushes.Black;
							break;
						case AutoRunMode.CycleStop:
							btnRunMode.Background = Brushes.Blue;
							btnRunMode.Foreground = Brushes.White;
							break;
						case AutoRunMode.QuitLot:
						case AutoRunMode.EndLot:
							btnRunMode.Background = Brushes.Red;
							btnRunMode.Foreground = Brushes.White;
							break;
						default:
							//NDm.Write(eNDM.AutoRunMode, (int)teAutoRunMode.Normal);
							break;
					}
					btnRunMode.Content =EnumHelper.GetDescription(RunMode);
				}
				
				// 검사 디버그 모드 상태 표시
				if		(SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate1) == true)
				{
					if (lbDebugMode.Visibility == Visibility.Hidden)			lbDebugMode.Visibility	= Visibility.Visible;
					if (lbDebugMode.Text != "INSPECTION DEBUG MODE - #1")		lbDebugMode.Text		= "INSPECTION DEBUG #1";
				}
				else if (SM.SHM.GetUseSkipData(USESKIP.InspDebugPlate2) == true)
				{
					if (lbDebugMode.Visibility == Visibility.Hidden)			lbDebugMode.Visibility	= Visibility.Visible;
					if (lbDebugMode.Text != "INSPECTION DEBUG MODE - #2")		lbDebugMode.Text		= "INSPECTION DEBUG #2";
				}
				else
				{
					if (lbDebugMode.Visibility == Visibility.Visible)			lbDebugMode.Visibility	= Visibility.Hidden;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateDoorClose()
		{
			try
			{
				Ellipse[]	lamps	= new Ellipse[] { LampDoorTopFront, LampDoorTopLeft, LampDoorTopRear, LampDoorTopRight, LampDoorBottom};

				for (int i = 0; i < 5; i++)
				{
					if		(SM.SHM.GetDoorOpen(i) == true  &&  lamps[i].Fill != Brushes.Lime)			lamps[i].Fill	= Brushes.Lime;
					else if (SM.SHM.GetDoorOpen(i) == false  &&  lamps[i].Fill != Brushes.Red)			lamps[i].Fill	= Brushes.Red;
				}
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		private	void	UpdateTooltip()
		{
			try
			{
				for (FlowTray flow = FlowTray.LDTrans; flow <= FlowTray.Reject; flow++)
				{
					object TargetObject = new object();
					switch (flow)
					{
						case FlowTray.LDTrans:			TargetObject = ucLdTrans;		break;
						case FlowTray.LDTable1:			TargetObject = ucLdTable1;		break;
						case FlowTray.LDTable2:			TargetObject = ucLdTable2;		break;
						case FlowTray.TrayClean:		TargetObject = CleanTray;		break;
						case FlowTray.ULDTrans:			TargetObject = ucUldTrans;		break;
						case FlowTray.ULDTable1:		TargetObject = ucUldTable1;		break;
						case FlowTray.ULDTable2:		TargetObject = ucUldTable2;		break;
						case FlowTray.Reject:			TargetObject = ucRjTable;		break;
						default:
							break;
					}
					
					int labelWidth = 16; 
					// TargetObject가 ToolTip을 설정할 수 있는 UI 요소인지 확인합니다.
					if (TargetObject is FrameworkElement targetElement)
					{
						if (targetElement.ToolTip == null)	// ToolTip이 null인 경우 새로 생성, 트레이 없으면 동작 안함
						{
							if (SM.FMT.IsFlow(flow) == false)	continue; 

							ToolTip trayToolTip = new ToolTip();
							StackPanel toolTipContent = new StackPanel();
							toolTipContent.Children.Add(new TextBlock { Text = $"{"LOT ID".PadRight(labelWidth)}: {SM.FMT[flow].GetInfo().WorkOrder}" });
							toolTipContent.Children.Add(new TextBlock { Text = $"{"TOP PROGRAM NAME".PadRight(labelWidth)}: {SM.FMT[flow].GetInfo().ProgramID_Top}" });
							toolTipContent.Children.Add(new TextBlock { Text = $"{"BTM PROGRAM NAME".PadRight(labelWidth)}: {SM.FMT[flow].GetInfo().ProgramID_Btm}" });
							toolTipContent.Children.Add(new TextBlock { Text = $"{"TRAY BARCODE".PadRight(labelWidth)}: {SM.FMT[flow].GetInfo().TrayBarcode}" });
							trayToolTip.Content = toolTipContent;
							targetElement.ToolTip = trayToolTip;
							ToolTipService.SetInitialShowDelay(targetElement, 200);
						}
						else	// ToolTip이 이미 존재하는 경우, 기존 ToolTip의 내용을 수정, 트레이 없으면 삭제
						{
							if (SM.FMT.IsFlow(flow) == false)
							{
								ToolTipService.SetToolTip(targetElement, null);
								continue;
							}

							if (targetElement.ToolTip is ToolTip existingToolTip)
							{
								if (existingToolTip.Content is StackPanel existingContent)
								{
									// 기존 StackPanel의 자식 요소를 수정 (필요시 기존 자식 요소 제거 후 새로 추가 가능)
									((TextBlock)existingContent.Children[0]).FontFamily = new FontFamily("Consolas");
									((TextBlock)existingContent.Children[1]).FontFamily = new FontFamily("Consolas");
									((TextBlock)existingContent.Children[2]).FontFamily = new FontFamily("Consolas");
									((TextBlock)existingContent.Children[3]).FontFamily = new FontFamily("Consolas");
									((TextBlock)existingContent.Children[0]).Text = $"{"LOT ID".PadRight(labelWidth)}: {SM.FMT[flow].GetInfo().WorkOrder}";
									((TextBlock)existingContent.Children[1]).Text = $"{"TOP PROGRAM NAME".PadRight(labelWidth)}: {SM.FMT[flow].GetInfo().ProgramID_Top}";
									((TextBlock)existingContent.Children[2]).Text = $"{"BTM PROGRAM NAME".PadRight(labelWidth)}: {SM.FMT[flow].GetInfo().ProgramID_Btm}";
									((TextBlock)existingContent.Children[3]).Text = $"{"TRAY BARCODE".PadRight(labelWidth)}: {SM.FMT[flow].GetInfo().TrayBarcode}";
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		public	void	Update(object sender, EventArgs e)
		{
			try
			{
				UpdateExist();
				UpdateLiftState();
				UpdateModuleAni();
				UpdateMotor();
				UpdateMotorZ();
				UpdateTrayPocket();
				UpdateDoorClose();
				UpdateControlText();
				UpdateTooltip();
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		public	void	UpdateFlowMeter(object sender, EventArgs e)
		{
			try
			{
				string	strMainAirFlow		=  (Math.Truncate(TypeConversion.GetFlowMeterFromAInput(DI.MainAirFlow)		 *10)/10).ToString("F0");
				string	strBlowAirFlow		=  (Math.Truncate(TypeConversion.GetFlowMeterFromAInput(DI.BlowAirFlow)		 *10)/10).ToString("F0");
				string	strLDPnPAirFlow		=  (Math.Truncate(TypeConversion.GetFlowMeterFromAInput(DI.LDPnPAirFlow)	 *10)/10).ToString("F1");
				string	strLDInspPnPAirFlow	=  (Math.Truncate(TypeConversion.GetFlowMeterFromAInput(DI.LDInspPnPAirFlow) *10)/10).ToString("F1");
				string	strULDPnPAirFlow	=  (Math.Truncate(TypeConversion.GetFlowMeterFromAInput(DI.ULDPnPAirFlow)	 *10)/10).ToString("F1");
				string	strULDInspPnPAirFlow=  (Math.Truncate(TypeConversion.GetFlowMeterFromAInput(DI.ULDInspPnPAirFlow)*10)/10).ToString("F1");
				
				tbMainAir.Text		= strMainAirFlow		== "-1" ? "0" : strMainAirFlow;	
				tbBlowAir.Text		= strBlowAirFlow		== "-1" ? "0" : strBlowAirFlow;
				tbLDPnP.Text		= strLDPnPAirFlow		== "-1.0" ? "0" : strLDPnPAirFlow;
				tbLDInspPnP.Text	= strLDInspPnPAirFlow	== "-1.0" ? "0" : strLDInspPnPAirFlow;
				tbULDPnP.Text		= strULDPnPAirFlow		== "-1.0" ? "0" : strULDPnPAirFlow;
				tbULDInspPnP.Text	= strULDInspPnPAirFlow	== "-1.0" ? "0" : strULDInspPnPAirFlow;
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		public	void	UpdateRcv(object sender, EventArgs e)
		{
			try
			{
				if (SM.FMT.IsFlow(FlowTray.LDWaitBuf) == false)
				{
					btnRcvWorkInfo.Background = new SolidColorBrush(Colors.Transparent);
					btnRcvWorkInfo.Foreground = new SolidColorBrush(Colors.Transparent);
					btnRcvWorkInfo.BorderBrush = new SolidColorBrush(Colors.Transparent);
					return;
				}

				if (((SolidColorBrush)btnRcvWorkInfo.Background).Color != Colors.Lime)
				{
					btnRcvWorkInfo.Background = new SolidColorBrush(Colors.Lime);
					btnRcvWorkInfo.Foreground = new SolidColorBrush(Colors.Black);
					btnRcvWorkInfo.BorderBrush = new SolidColorBrush(Colors.Black);
				}
				else
				{
					btnRcvWorkInfo.Background = new SolidColorBrush(Colors.Transparent);
					btnRcvWorkInfo.Foreground = new SolidColorBrush(Colors.Transparent);
					btnRcvWorkInfo.BorderBrush = new SolidColorBrush(Colors.Transparent);
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
		#endregion

		#region Event

		private void	RunEndHiddenEvent(object sender, tsFigureEventArgs e)
		{
			ucFigure fi = sender as ucFigure;
			if (fi._Location != new Point(0, 0))
			{
				fi.Visibility = Visibility.Hidden;
			}
		}

		private void	HomeEndHiddenEvent(object sender, tsFigureEventArgs e)
		{
			ucFigure fi = sender as ucFigure;
			if (fi._Location == new Point(0, 0))
			{
				fi.Visibility = Visibility.Hidden;
			}
		}

		private void	HomeEndHiddenEvent(object sender, tsFigureArrayEventArgs e)
		{
			ucFigureArray fi = sender as ucFigureArray;
			if (fi._Location == new Point(0, 0))
			{
				fi.Visibility = Visibility.Hidden;
			}
		}

		private void	FigureArrayDoubleClickEvent(object sender, tsFigureArrayMouseButtonEventArgs e)
		{
			try
			{
				if (e.me.ChangedButton == MouseButton.Right)						return;
				if (Program._bSimul_ == false  &&  SM.SHM.IsBitData(BIT.AutoRun) == true)			return;
				if (Program.m_PasswordMode != PW_MODE.ADMIN && Program.m_PasswordMode != PW_MODE.PRO)		return;
				
				ucFigureArray	uc	= (ucFigureArray)sender;

				switch (uc.Name)
				{
					case "ucLdTray1" :
					case "ucLdTray2" :
					case "ucUldTray1" :
					case "ucUldTray2" :
					case "ucRjTray" :
						ChangeExistTrayPocket(sender, e);
						break;
					case "ucLdPnPX" :
					case "ucUldPnPX" :
					case "ucLDBuf1Y" :
					case "ucLDBuf2Y" :
					case "ucUldBufY" :
					case "ucLdInspPnPX" :
					case "ucInspPlateY1" :
					case "ucInspPlateY2" :
					case "ucUldInspPnPX" :
						ChangeExistProd(sender, e);
						break;
					case "ucLdTrans" :
					case "ucUldTrans" :
						ChangeExistTrans(sender, e);
						break;
				}
			}
			catch (Exception ex)
			{
				 UI.Logs.Write(LOGPART.EX, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}
		
		private	void	btnMode_Click(object sender, RoutedEventArgs e)
		{
			if (SM.SHM.IsBitData(BIT.AutoRun) == true)			return;
			//if (Program.m_PasswordMode == PW_MODE.OPER)			return;

			eNDM				ndm		= eNDM.DM255;
			Button				btn		= (Button)sender;
			frmListBoxSelect	popup	= new frmListBoxSelect();
			List<string>		lt		= new List<string>();

			switch (btn.Name)
			{
				case "btnRunMode":
					//if (Program.m_PasswordMode != PW_MODE.ADMIN)
					switch (Program.m_PasswordMode)
					{
						case PW_MODE.OPER:
						case PW_MODE.ENG:
									lt	= new List<string>() {	EnumHelper.GetDescription(AutoRunMode.Normal) ,
																EnumHelper.GetDescription(AutoRunMode.QuitLot)};
							break;
						case PW_MODE.PRO:
						case PW_MODE.ADMIN:
									lt	= new List<string>() {	EnumHelper.GetDescription(AutoRunMode.Normal),
																EnumHelper.GetDescription(AutoRunMode.CycleStop),
																EnumHelper.GetDescription(AutoRunMode.QuitLot) ,
																EnumHelper.GetDescription(AutoRunMode.EndLot) };
							break;
						default:
									lt	= new List<string>() {	EnumHelper.GetDescription(AutoRunMode.Normal) ,
																EnumHelper.GetDescription(AutoRunMode.CycleStop)};
							break;
					}
					ndm	= eNDM.AutoRunMode;
					break;

				case "btnRejectMode":
					lt	= new List<string>() { "Normal", "Reject Unload All" };
					ndm	= eNDM.RejectRunMode;
					break;

				default:
					return;
			}



			if (popup.ShowDialog(lt, NDm.Read(ndm)) == System.Windows.Forms.DialogResult.OK)
			{
				if ((NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.EndLot || NDm.Read(eNDM.AutoRunMode) == (int)AutoRunMode.QuitLot)  &&
					(SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.ULDConv31) == true ||
					 SM.FMT.IsFlow(FlowTray.Reject) == true && SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0 ||
					 SM.FMP.IsFlowRange(FlowPart.LDPnP, FlowPart.ULDPnP) == true))
				{
					string strMsg = $"You cannot manually change the AutoRun mode from EndLot or QuitLot to Normal. {Environment.NewLine} " +
									$"You have to empty all the products in that mode";
					UI.Logs.Write(LOGPART.WARNING, strMsg);
					SM.SHM.SetMsg(SeqMsg.SEQERR, strMsg);
					SM.SHM.MsgShow(SeqMsg.SEQERR);
					return;
				}

				if (ndm == eNDM.AutoRunMode  &&  popup.SelItem == EnumHelper.GetDescription(AutoRunMode.CycleStop))
				{
					NDm.Write(eNDM.OldAutoRunMode, NDm.Read(eNDM.AutoRunMode));
				}

				if (popup.SelItem  == EnumHelper.GetDescription(AutoRunMode.QuitLot))
				{	
					if (SM.FMT.IsFlow(FlowTray.LDTable1) )	SM.FMT[FlowTray.LDTable1].SetAutoRunMode(AutoRunMode.QuitLot);
					if (SM.FMT.IsFlow(FlowTray.LDTable2) )	SM.FMT[FlowTray.LDTable2].SetAutoRunMode(AutoRunMode.QuitLot);
				}

				if (popup.SelItem == EnumHelper.GetDescription(AutoRunMode.QuitLot))			NDm.Write(ndm, (int)AutoRunMode.QuitLot);
				else if (popup.SelItem == EnumHelper.GetDescription(AutoRunMode.Normal))		NDm.Write(ndm, (int)AutoRunMode.Normal);
				else if (popup.SelItem == EnumHelper.GetDescription(AutoRunMode.EndLot))		NDm.Write(ndm, (int)AutoRunMode.EndLot);
				else if (popup.SelItem == EnumHelper.GetDescription(AutoRunMode.CycleStop))	NDm.Write(ndm, (int)AutoRunMode.CycleStop);
				
			}
		}

		
		private bool	IsMachineEmpty()
		{
			//Check : 범위 확인
			try 
			{
				if (SM.FMT.IsFlowRange(FlowTray.LDLift01, FlowTray.ULDConv31) == true)								return false;
				if (SM.FMT.IsFlow(FlowTray.Reject) == true  &&  SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0)	return false;
				if (SM.FMP.IsFlowRange(FlowPart.LDPnP, FlowPart.ULDPnP) == true)									return false;

				return true;
			}
			catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				return false;
			}
		}

		private	void	tbMES_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (SM.SHM.IsBitData(BIT.AutoRun) == true)			return;
	
			frmIPSetting IpSetting = new frmIPSetting();
			IpSetting.Show("tbMes");
		}

		private void	lbRemainFill_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			if (SM.SHM.IsBitData(BIT.AutoRun) == true)			return;
			if (Program.m_PasswordMode == PW_MODE.OPER)			return;

			System.Windows.Forms.Label	lb		= new System.Windows.Forms.Label();
			string						str_val	= NDm.Read(eNDM.FillDoneRemainCount).ToString();		
			
			frmKeyPad	pup	= new frmKeyPad(lb, str_val, KeyPadType.unsigned);
				
			if (pup.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)			return;

			NDm.Write(eNDM.FillDoneRemainCount, int.Parse(lb.Text));

			GLog.Add(string.Format("Tray Remain Count : {0}", NDm.Read(eNDM.FillDoneRemainCount)));
		}
		
		private void btnPickColor_Click(object sender, MouseButtonEventArgs e)
		{
			if (Program.m_PasswordMode != PW_MODE.ADMIN)		return;
				
			Rectangle RecColor	= sender as Rectangle;
			string RecName		= RecColor.Name ;

            using (var colorDialog = new System.Windows.Forms.ColorDialog())
            {
                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 선택된 색상을 string 형식으로 저장
                    var selectedColor = colorDialog.Color;
                    string colorString = $"#{selectedColor.R:X2}{selectedColor.G:X2}{selectedColor.B:X2}";

					switch (RecName)
					{
						case "IdxTrayColor":		SDm.Write(eSDM.TrayColor,			colorString );	break;
						case "IdxCapColor":			SDm.Write(eSDM.CapColor,			colorString );	break;
						case "IdxUnknownColor":		SDm.Write(eSDM.UnknownColor,		colorString );	break;
						case "IdxNGColor":			SDm.Write(eSDM.NGColor,				colorString );	break;
						case "IdxUserNGColor":		SDm.Write(eSDM.UserNGColor,			colorString );	break;
						case "IdxOKColor":			SDm.Write(eSDM.OKColor,				colorString );	break;
						case "IdxMisMatchColor":	SDm.Write(eSDM.MisMatchColor,		colorString );	break;
						case "TrayIdxOKColor":		SDm.Write(eSDM.TrayOKColor,			colorString );	break;
						case "TrayIdxUnknownColor":	SDm.Write(eSDM.TrayUnknownColor,	colorString );	break;
						case "TrayIdxEmptyColor":	SDm.Write(eSDM.TrayEmptyColor,		colorString );	break;
					}
				}
            }

			UpdateColor();
        }
		#endregion

		public			ucMainDisp()
		{
			InitializeComponent();
			Init();
		}

		private void UcRjTable_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			//if (SM.SHM.IsBitData(BIT.AutoRun) == true)	return;
			
			if (SM.FMT.IsFlow(FlowTray.Reject)  == false)
			{
				frmMessageBox pMsg;
				pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, H_MarsSSD.Language.languageDefault.MB_Info, "Please check the Reject Buffer Flow.");
				if (pMsg.ShowDialog() == System.Windows.Forms.DialogResult.OK) return;
			}

			if (G.seqRejectBuffer.IsReady == false)
			{
				frmMessageBox pMsg;
				pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, H_MarsSSD.Language.languageDefault.MB_Info, "Reject Buffer Sequence is already in progress.");
				if (pMsg.ShowDialog() == System.Windows.Forms.DialogResult.OK) return;
			}

            //베리파이 안된게 있으면 안됨
            if (SM.FMT[FlowTray.Reject].GetVerifyNotCount() > 0)
            {
                frmMessageBox pMsg;
                pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, H_MarsSSD.Language.languageDefault.MB_Info, "There is still material left that has not yet been Veirified.");
                if (pMsg.ShowDialog() == System.Windows.Forms.DialogResult.OK) return;
            }

            //양품이 있으면 안됨
            if (SM.FMT[FlowTray.Reject].IsResultExist(ProdStatus.OK) == true)
            {
                frmMessageBox pMsg;
                pMsg = new frmMessageBox(frmMessageBox.teButtons.OK, H_MarsSSD.Language.languageDefault.MB_Info, "There is still material left that Veirified as OK");
                if (pMsg.ShowDialog() == System.Windows.Forms.DialogResult.OK) return;
            }

            frmMessageBox pMsgConfirm;
			pMsgConfirm = new frmMessageBox(frmMessageBox.teButtons.OKCancel, H_MarsSSD.Language.languageDefault.MB_Info, "Would you like to replace the Reject tray?");
			if (pMsgConfirm.ShowDialog() == System.Windows.Forms.DialogResult.Cancel) return;
			
			
			if (SM.SHM.IsManualRunning() == false)		SM.SHM.StartManual();

			SM.SHM.SetManualNum(MANUAL.REJECTBUFFER_CHANGE);
		}

		
		private void BtnRcvWorkInfo_Click(object sender, RoutedEventArgs e)
		{
			UI.BarcodeInfo.ShowDialog();
		}
	}

	public class Status : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected	void	OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,
					new PropertyChangedEventArgs(propertyName));
			}
		}

		private	Visibility	visible;
		public	Visibility	Visible
		{
			get { return visible; }
			set
			{
				visible = value;
				OnPropertyChanged("Visible");
			}
		}

		private	Brush[]		cylState = new SolidColorBrush[2];
		public	Brush[]		CylState
		{
			get { return cylState; }
			set
			{
				for (int i = 0 ; i < cylState.Length; i++)
				{
					if (value[i] == null)
					{
						break;
					}
					if (cylState[i] == null  ||  ((SolidColorBrush)cylState[i]).Color != ((SolidColorBrush)value[i]).Color)
					{
						cylState = value;
						OnPropertyChanged("CylState");
						break;
					}
				}
			}
		}

		//모듈 상태
		private	Brush		moduleState = new SolidColorBrush();
		public	Brush		ModuleState
		{
			get { return moduleState; }
			set
			{
				if (moduleState == null  ||  ((SolidColorBrush)moduleState).Color != ((SolidColorBrush)value).Color)
				{
					moduleState = value;
					OnPropertyChanged("ModuleState");
				}
			}
		}
	}
}