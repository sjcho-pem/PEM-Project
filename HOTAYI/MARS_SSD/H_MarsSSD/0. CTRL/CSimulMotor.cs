using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace H_MarsSSD
{
	public class CSimulMotor : CMotor
	{
		#region -Variables, Events        
		private	struct	PosProfile
		{
			public	double	Target;
			public	double	Velocity;
			public	double	Acc;
			public	double	Dec;
		}

		private	struct	JogProfile
		{
			public	double	Velocity;
			public	double	Acc;
			public	double	Dec;
		}
		private struct SimulProfile
		{
			public OpState opMode;
			public int nPos;
			public int nVel;
		}

		private	PosProfile		m_stPosProfile;
		private	JogProfile		m_stJogProfile;
		private	Thread			m_thread_move	= null;
		private bool			m_bMoveShould	= false;
		private SimulProfile	m_stSimulProfile;
		double					m_dStep			= 0.0;
		double					m_dCurPos	    = 0.0;
		#endregion

		public CSimulMotor(int _nAxis)
		{
			nAxis = _nAxis;

			MotionCommand_Init();
		}

		#region [ Motion Functions ]
		private void MotionCommand_Init()
		{
			try
			{
				if (m_thread_move != null  &&  m_thread_move.IsAlive)
					return;
				
				m_bMoveShould = true;
				m_thread_move	= new Thread(Thread_Move);
				m_thread_move.Start();
				m_thread_move.IsBackground	= true;
		
				if (NDm.Read(eNDM.SimMotorSpeedRate) == 0)			NDm.Write(eNDM.SimMotorSpeedRate, 10);
			}
			catch
			{

			}
		}

		private void SetMovePos(float fTarget)
		{
			m_stPosProfile.Target = fTarget;

			SetTargetPos(fTarget);
		}

		private void SetMoveSpeed(int nVel, int nAcc, int nDec, bool bDir = true)
		{
			if (bDir == false)
				nVel *= -1;

			if (nVel > 100)
				m_stPosProfile.Velocity = nVel * VelPercent;
			else
				m_stPosProfile.Velocity = nVel;

			m_stPosProfile.Acc = nAcc;
			m_stPosProfile.Dec = nDec;
		}

		private void SetJogSpeed(int nVel, int nAcc, int nDec, bool bDir = true)
		{
			if (bDir == false)
				nVel *= -1;

			if (nVel > 100)
				m_stJogProfile.Velocity = nVel * VelPercent;
			else
				m_stJogProfile.Velocity = nVel;

			m_stJogProfile.Acc = nAcc;
			m_stJogProfile.Dec = nDec;
		}
		#endregion

		#region 모터 행동 함수
		public override void Run()
		{
			TorqueParam();
			ReadBitStatus();
			ErrorProc();
		}

		private void TorqueParam()
		{
			
		}

		private void ReadBitStatus()
		{                            
			SetRdy(IsSVON() && !IsAlarm() && IsHomeDone() && IsInPos() && !IsCommand());

			if (IsCommand() && IsHomeDone())
			{
				if (CheckCommandTimeOut() && !IsInPos()) // !IsPSET() 대체가능한지 확인 필요
					OnAddAlarmEvent((int)ALM.ERROR_MT_MOVE_TIME_OUT_START + nAxis + 1);

				if (Math.Abs(GetLocalPos() - GetTargetPos()) > InposRange)
				{
					return;
				}
				else
				{
					OffCommand();
					SetNowIndex(GetCmdIndex());
				}
			}
		}

		void ErrorProc()
		{
			if (IsAlarm())                      
				OnAddAlarmEvent((int)ALM.ERROR_MT_DRIVER_START + nAxis + 1);
			if (!IsSVON())                      
				OnAddAlarmEvent((int)ALM.ERROR_MT_POWER_START + nAxis + 1);
			if (IsPosLSen() || IsNegLSen())     
				OnAddAlarmEvent((int)ALM.ERROR_MT_LIMIT_START + nAxis + 1);
		}

		public override bool SetServoOn(bool bOn)
		{
			m_stMt.ServoOn	= bOn;			

			return true;
		}


		public override bool AlarmReset()
		{
			m_stMt.Alarm		= false;
			m_stMt.AlarmCode	= 0;

			return true;
		}

		public override bool Stop()
		{
			MoveStop();

			OffCommand();
			SetCmdIndex(300);
			SetNowIndex(300);

			return true;
		}

		public override bool EStop()
		{
			MoveStop();
			OffCommand();

			return true;
		}

		public override bool TorqueStop()
		{
			MoveStop();
			return true;
		}

		public override void ClearHome()
		{
			SetCmdIndex(300);
			SetNowIndex(300);

			m_stMt.HomeDone	= true;
		}

		public override void StartHoming()
		{
			SetCmdIndex(300);
			SetNowIndex(300);
			SetMovePos(0);

			int FastVel = GetVel((int)VELOCITY.HomeSearch);
			int SlowVel = GetVel((int)VELOCITY.HomeFind);

			SetMoveSpeed(FastVel, GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));

			MoveStart(OpState.Home, (int)m_stPosProfile.Velocity, 0);
		}

		public override void MoveJog(bool dir, int nVel = 0)
		{
			if (nVel == 0)
				nVel = GetVel((int)VELOCITY.JOG);

			SetJogSpeed(nVel, GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC), dir);
			SetCmdIndex(300);

			MoveStart(OpState.Jog, (int)m_stJogProfile.Velocity, 0);            
		}

		public override void MoveREL(int nDist, int nVel = 0)
		{
			if (nVel == 0)
				nVel = GetVel((int)VELOCITY.JOG);

			SetMovePos(GetLocalPos() + nDist);
			SetMoveSpeed(nVel, GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
			SetCmdIndex(300);

			OnCommand(m_stPosProfile.Velocity);

			MoveStart(OpState.Pos, (int)m_stPosProfile.Velocity, (int)m_stPosProfile.Target);
		}

		public override void MoveABS(int nPos)
		{
			if (!IsRdy() || IsCommand() || GetLocalPos() == nPos)
				return;

			SetMovePos(nPos);
			SetMoveSpeed(GetVel((int)VELOCITY.JOG), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
			SetCmdIndex(300);

			OnCommand(m_stPosProfile.Velocity);

			MoveStart(OpState.Pos, (int)m_stPosProfile.Velocity, (int)m_stPosProfile.Target);       
		}

		public override void MoveABS(int nPos, int nIdxVel)
		{
			if (!IsRdy() || IsCommand() || GetLocalPos() == nPos)
				return;

			SetMovePos(nPos);
			SetMoveSpeed(GetVel(nIdxVel), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
			SetCmdIndex(300);

			OnCommand(m_stPosProfile.Velocity);

			MoveStart(OpState.Pos, (int)m_stPosProfile.Velocity, (int)m_stPosProfile.Target);
		}

		public override void MoveIndex(int eNum)
		{
			if (!IsRdy() || IsCommand() || GetNowIndex() == eNum)
				return;

			SetMovePos(GetIndexPos(eNum));
			SetMoveSpeed(GetVel(eNum), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
			SetCmdIndex(eNum);

			OnCommand(m_stPosProfile.Velocity);

			MoveStart(OpState.Pos, (int)m_stPosProfile.Velocity, (int)m_stPosProfile.Target);
		}

		public override void MoveIndex(int eNum, int nIdxVel)
		{
			if (!IsRdy() || IsCommand() || GetNowIndex() == eNum)
				return;

			SetMovePos(GetIndexPos(eNum));
			SetMoveSpeed(GetVel(nIdxVel), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
			SetCmdIndex(eNum);

			OnCommand(m_stPosProfile.Velocity);

			MoveStart(OpState.Pos, (int)m_stPosProfile.Velocity, (int)m_stPosProfile.Target);
		}

		public override void TMoveIndex(int eNum, int nTMt, int nRemainDist, int nSecVel)
		{
			SetMovePos(GetIndexPos(eNum));
			SetMoveSpeed(GetVel(eNum), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
			SetCmdIndex(eNum);

			//OnCommand(m_TPosCom.Profile.Velocity);

			MoveStart(OpState.Pos, (int)m_stPosProfile.Velocity, (int)m_stPosProfile.Target);
		}

		public override void MoveTorque(int eNum)            
		{
			SetMovePos(GetIndexPos(eNum));
			SetMoveSpeed(GetVel(eNum), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
			SetCmdIndex(eNum);

			MoveStart(OpState.Pos, (int)m_stPosProfile.Velocity, (int)m_stPosProfile.Target);
		}

		public override void OverrideVel(int nVel)
		{
			SetMoveSpeed(nVel, GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
		}

		public void MoveIndexREL(int eNum, bool dir = true)
		{
			int direction = -1;
			if (dir == true)
				direction = 1;  

			SetMovePos(GetLocalPos() + direction * GetIndexPos(eNum));
			SetMoveSpeed(GetVel(eNum), GetVel((int)VELOCITY.ACC), GetVel((int)VELOCITY.DEC));
			SetCmdIndex(300);
			
			OnCommand(m_stPosProfile.Velocity);

			MoveStart(OpState.Pos, (int)m_stPosProfile.Velocity, (int)m_stPosProfile.Target);
		}
		#endregion

		#region Simulation Function
		public void             Dispose()
		{             
			m_bMoveShould = false;

			Thread.Sleep(100);
			if (m_thread_move != null)
				m_thread_move.Abort();
		}
		private	void			MoveStart(OpState Mode, int nVel, int nPos)
		{
			try
			{
				if (m_thread_move == null  ||  m_thread_move.IsAlive == false)			
					return;
				if (nVel == 0)			
					return;
				
				m_stSimulProfile.opMode = Mode;
				m_stSimulProfile.nPos = nPos;
				m_stSimulProfile.nVel = nVel;

				// 시뮬레이션 속도 증가
				if		(NDm.Read(eNDM.SimMotorSpeedRate) < 0)				 NDm.Write(eNDM.SimMotorSpeedRate, 1);
				else if (NDm.Read(eNDM.SimMotorSpeedRate) > 100)			 NDm.Write(eNDM.SimMotorSpeedRate, 100);

				m_dStep		= m_stSimulProfile.nVel * NDm.Read(eNDM.SimMotorSpeedRate) / 20.0f;
				//fStep		= stSimulProfile.nVel / 20.0f;
				m_dCurPos 	= m_stMt.CmdPos;

				if (m_stSimulProfile.opMode == OpState.Home)
				{
					int ntmp	= m_stSimulProfile.nPos - m_stMt.CmdPos;
					m_dCurPos	= ntmp / 20.0f;
				}

				m_stMt.CurOpState	= m_stSimulProfile.opMode;
				m_stMt.Inpos		= false;
			}
			catch
			{
			}
		}

		private	void			MoveStop()
		{
			try
			{
				m_stMt.CurOpState = m_stSimulProfile.opMode	= OpState.Stop;

				Thread.Sleep(1);

				m_stMt.CurOpState = m_stSimulProfile.opMode	= OpState.Idle;
				m_stMt.Inpos		= true;
			}
			catch
			{
			}
		}


		private	void			Thread_Move()
		{
			try
			{	
				bool brun = true;

				while (m_bMoveShould)
				{
					Thread.Sleep(50);

					switch (m_stSimulProfile.opMode)
					{
						case OpState.Jog :
							m_dCurPos		+= m_dStep;
							m_stMt.CmdPos	= (int)m_dCurPos;
							m_stMt.ActPos	= (int)m_dCurPos;
							break;

						case OpState.Pos :
							{
								int ntmp = m_stMt.CmdPos - m_stSimulProfile.nPos;
								if (Math.Abs(ntmp) <= Math.Abs(m_dStep))
								{
									m_stMt.CmdPos = m_stSimulProfile.nPos;
									m_stMt.ActPos = m_stSimulProfile.nPos;
									brun = false;
									break;
								}

								if (0 < ntmp) m_dCurPos -= m_dStep;
								else m_dCurPos += m_dStep;
								m_stMt.CmdPos = (int)m_dCurPos;
								m_stMt.ActPos = (int)m_dCurPos;
							}
							break;

						case OpState.Home :
							{
								int ntmp = m_stMt.CmdPos - m_stSimulProfile.nPos;
								if (Math.Abs(ntmp) <= Math.Abs(m_dStep))
								{
									m_stMt.CmdPos = m_stSimulProfile.nPos;
									m_stMt.ActPos = m_stSimulProfile.nPos;
									m_stMt.HomeDone = true;
									m_stMt.eHomeState = HomeState.Idle;
									brun = false;
									break;
								}
								m_stMt.eHomeState = HomeState.HomeSearch;
								if (0 < ntmp) m_dCurPos -= m_dStep;
								else m_dCurPos += m_dStep;
								m_stMt.CmdPos = (int)m_dCurPos;
								m_stMt.ActPos = (int)m_dCurPos;
							}
							break;
					}

					if (brun == false)
					{
						brun = true;
						m_stSimulProfile.opMode = m_stMt.CurOpState	= OpState.Idle;
						m_stMt.Inpos		= true;
					}
				}
			}
			catch (ThreadInterruptedException)
			{
			}
			catch (Exception)
			{
			}
			
		}
		#endregion


	}
}