using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
	/// <summary>
	/// Only 대문자 사용
	/// 디스크립션 규칙 : "<ko>한글모드<en>영문모드<cn>중국어모드"
	/// </summary>
	public enum ALM
	{
		NORMAL = 0,
		NOT_ALARM = 0,
		#region - System
#if _WMX3_
		[DefaultValueAttribute("<ko>WMX3 디바이스 연결이 해제 되었습니다.<en>WMX3 CONTROL DEVICE DISCONECTED.")]
#elif _AJIN_
		[DefaultValueAttribute("<ko>AJIN 디바이스 연결이 해제 되었습니다.<en>AJIN CONTROL DEVICE DISCONECTED.")]
#else
		[DefaultValueAttribute("<ko>디바이스 연결이 해제 되었습니다.<en>CONTROL DEVICE DISCONECTED.")]
#endif
		ERROR_CONTROL_DEVICE_DISCONECTED,
		[DefaultValueAttribute("<ko>EMO 알람 발생.<en>EMO CHECK")]
		ERROR_EMO,
		[DefaultValueAttribute("<ko>문이 열려 있습니다.<en>THE DOOR IS OPEN.")]
		ERROR_DOOR_OPEN,
		[DefaultValueAttribute("<ko>문이 열려 있습니다.<en>THE TOP FRONT DOOR IS OPEN.")]
		ERROR_TOP_FRONT_DOOR_OPEN,
		[DefaultValueAttribute("<ko>문이 열려 있습니다.<en>THE TOP LEFT DOOR IS OPEN.")]
		ERROR_TOP_LEFT_DOOR_OPEN,
		[DefaultValueAttribute("<ko>문이 열려 있습니다.<en>THE TOP REAR DOOR IS OPEN.")]
		ERROR_TOP_REAR_DOOR_OPEN,
		[DefaultValueAttribute("<ko>문이 열려 있습니다.<en>THE TOP RIGHT DOOR IS OPEN.")]
		ERROR_TOP_RIGHT_DOOR_OPEN,
		[DefaultValueAttribute("<ko>문이 열려 있습니다.<en>THE TOP DOOR IS OPEN.")]
		ERROR_TOP_DOOR_OPEN,
		[DefaultValueAttribute("<ko>문이 열려 있습니다.<en>THE BOTTOM DOOR IS OPEN.")]
		ERROR_BOTTOM_DOOR_OPEN,
		[DefaultValueAttribute("<ko>문이 열려 있습니다.<en>THE TOP RIGHT REAR DOOR IS OPEN.")]
		ERROR_TOP_RIGHT_REAR_DOOR_OPEN,
		[DefaultValueAttribute("<ko>메인 공압 확인해주세요.<en>MAIN AIR CHECK")]
		ERROR_MAIN_AIR,
		[DefaultValueAttribute("<ko>클리너 메인 공압 확인해주세요.<en>CLEANER MAIN AIR CHECK")]
		ERROR_CLEANER_MAIN_AIR,
		[DefaultValueAttribute("<ko>리셋 버튼 확인해주세요.<en>RESET CHECK")]
		ERROR_RESET,
		[DefaultValueAttribute("<ko>알람 초기화 TIME OUT이 발생 했습니다. 모터 전원, 모터 드라이버의 상태를 확인 해 주세요.<en>TIME OUT of the alarm initialization has occurred. Check the motor power and motor driver status.")]
		ERROR_ALARMCLEAR_TIMEOUT,
		[DefaultValueAttribute("<ko>키 락을 확인해주세요.<en>KEYLOCK CHECK")]
		ERROR_KEYLOCK,
		#endregion

		#region - Motor
		/// <summary>
		/// 모터 알람 관련
		/// </summary>
		ERROR_MT_DRIVER_START = 20,
		[DefaultValueAttribute("<ko>로드 트레이 리프트 Z 모터 드라이버 알람.<en>LD TRAY LIFT Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>로드 컨베이어1 모터 드라이버 알람.<en>LD CONV1 MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_CONV1,
		[DefaultValueAttribute("<ko>로드 컨베이어2 모터 드라이버 알람.<en>LD CONV2 MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_CONV2,
		[DefaultValueAttribute("<ko>로드 컨베이어 스토퍼 모터 드라이버 알람.<en>LD CONV STOPPER MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_CONV_STOPPER,
		[DefaultValueAttribute("<ko>로드 컨베이어 WIDTH 모터 드라이버 알람.<en>LD CONV WIDTH MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>로드 트레이 홀더1 Z 모터 드라이버 알람.<en>LD TRAY HOLDER1 Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TRAY_HOLDER1_Z,
		[DefaultValueAttribute("<ko>로드 트레이 홀더2 Z 모터 드라이버 알람.<en>LD TRAY HOLDER2 Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TRAY_HOLDER2_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 X 모터 드라이버 알람.<en>LD TRAY TRANS X MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 Z 모터 드라이버 알람.<en>LD TRAY TRANS Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 GRIP 모터 드라이버 알람.<en>LD TRAY TRANS GRIP MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>로드 PnP X 모터 드라이버 알람.<en>LD PNP X MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_PNP_X,
		[DefaultValueAttribute("<ko>로드 PnP Y 모터 드라이버 알람.<en>LD PNP Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_PNP_Y,
		[DefaultValueAttribute("<ko>로드 PnP Z 모터 드라이버 알람.<en>LD PNP Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_PNP_Z,
		[DefaultValueAttribute("<ko>로드 PnP W 모터 드라이버 알람.<en>LD PNP W MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_PNP_W,
		[DefaultValueAttribute("<ko>로드 검사 PnP X 모터 드라이버 알람.<en>LD INSP PNP X MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>로드 검사 PnP Z 모터 드라이버 알람.<en>LD INSP PNP Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>로드 버퍼1 Y 모터 드라이버 알람.<en>LD BUF1 Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_BUF1_Y,
		[DefaultValueAttribute("<ko>로드 버퍼1 GRIP 모터 드라이버 알람.<en>LD BUF1 GRIP MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_BUF1_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼1 R 모터 드라이버 알람.<en>LD BUF1 R MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_BUF1_R,
		[DefaultValueAttribute("<ko>로드 버퍼2 Y 모터 드라이버 알람.<en>LD BUF2 Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_BUF2_Y,
		[DefaultValueAttribute("<ko>로드 버퍼2 GRIP 모터 드라이버 알람.<en>LD BUF2 GRIP MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_BUF2_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼2 R 모터 드라이버 알람.<en>LD BUF2 R MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_BUF2_R,
		[DefaultValueAttribute("<ko>로드 테이블1 Z 모터 드라이버 알람.<en>LD TABLE1 Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TABLE1_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 Z 모터 드라이버 알람.<en>LD TABLE2 Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TABLE2_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 X 모터 드라이버 알람.<en>LD TABLE2 X MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TABLE2_X,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 드라이버 알람.<en>LD TABLE1 GRIP Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 드라이버 알람.<en>LD TABLE2 GRIP Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_LD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 W 모터 드라이버 알람.<en>INSP PLATE1 W MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_INSP_PLATE1_W,
		[DefaultValueAttribute("<ko>검사 플레이트1 Y 모터 드라이버 알람.<en>INSP PLATE1 Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_INSP_PLATE1_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 GRIP 모터 드라이버 알람.<en>INSP PLATE1 GRIP MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_INSP_PLATE1_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트1 Z 모터 드라이버 알람.<en>INSP PLATE1 Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_INSP_PLATE1_Z,
		[DefaultValueAttribute("<ko>언로드 리프트 Z 모터 드라이버 알람.<en>ULD TRAY LIFT Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>언로드 컨베이어1 모터 드라이버 알람.<en>ULD CONV1 MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_CONV1,
		[DefaultValueAttribute("<ko>언로드 컨베이어2 모터 드라이버 알람.<en>ULD CONV2 MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_CONV2,
		[DefaultValueAttribute("<ko>언로드 컨베이어 WIDTH 모터 드라이버 알람.<en>ULD CONV WIDTH MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 X 모터 드라이버 알람.<en>ULD TRAY TRANS X MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 Z 모터 드라이버 알람.<en>ULD TRAY TRANS Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 GRIP 모터 드라이버 알람.<en>ULD TRAY TRANS GRIP MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>언로드 PnP X 모터 드라이버 알람.<en>ULD PNP X MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_PNP_X,
		[DefaultValueAttribute("<ko>언로드 PnP Y 모터 드라이버 알람.<en>ULD PNP Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_PNP_Y,
		[DefaultValueAttribute("<ko>언로드 PnP Z 모터 드라이버 알람.<en>ULD PNP Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 PnP W 모터 드라이버 알람.<en>ULD PNP W MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_PNP_W,
		[DefaultValueAttribute("<ko>언로드 검사 PnP X 모터 드라이버 알람.<en>ULD INSP PNP X MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>언로드 검사 PnP Z 모터 드라이버 알람.<en>ULD INSP PNP Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 버퍼 Y 모터 드라이버 알람.<en>ULD BUF Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP Y 모터 드라이버 알람.<en>ULD BUF GRIP Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_BUF_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP X 모터 드라이버 알람.<en>ULD BUF GRIP X MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_BUF_GRIP_X,
		[DefaultValueAttribute("<ko>리젝트 버퍼 Y 모터 드라이버 알람.<en>REJECT BUF Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_REJECT_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 테이블1 Z 모터 드라이버 알람.<en>ULD TABLE1 Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_TABLE1_Z,
		[DefaultValueAttribute("<ko>언로드 테이블2 Z 모터 드라이버 알람.<en>ULD TABLE2 Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_TABLE2_Z,
		[DefaultValueAttribute("<ko>언로드 테이블1 GRIP Y 모터 드라이버 알람.<en>ULD TABLE1 GRIP Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 테이블2 GRIP Y 모터 드라이버 알람.<en>ULD TABLE2 GRIP Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_ULD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 W 모터 드라이버 알람.<en>INSP PLATE2 W MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_INSP_PLATE2_W,
		[DefaultValueAttribute("<ko>검사 플레이트2 Y 모터 드라이버 알람.<en>INSP PLATE2 Y MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_INSP_PLATE2_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 GRIP 모터 드라이버 알람.<en>INSP PLATE2 GRIP MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_INSP_PLATE2_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트2 Z 모터 드라이버 알람.<en>INSP PLATE2 Z MOTOR DRIVER ALARM.")]
		ERROR_MT_DRIVER_INSP_PLATE2_Z,


		ERROR_MT_POWER_START,
		[DefaultValueAttribute("<ko>로드 트레이 리프트 Z 모터 전원 알람.<en>LD TRAY LIFT Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>로드 컨베이어1 모터 전원 알람.<en>LD CONV1 MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_CONV1,
		[DefaultValueAttribute("<ko>로드 컨베이어2 모터 전원 알람.<en>LD CONV2 MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_CONV2,
		[DefaultValueAttribute("<ko>로드 컨베이어 스토퍼 모터 전원 알람.<en>LD CONV STOPPER MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_CONV_STOPPER,
		[DefaultValueAttribute("<ko>로드 컨베이어 WIDTH 모터 전원 알람.<en>LD CONV WIDTH MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>로드 트레이 홀더1 Z 모터 전원 알람.<en>LD TRAY HOLDER1 Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TRAY_HOLDER1_Z,
		[DefaultValueAttribute("<ko>로드 트레이 홀더2 Z 모터 전원 알람.<en>LD TRAY HOLDER2 Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TRAY_HOLDER2_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 X 모터 전원 알람.<en>LD TRAY TRANS X MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 Z 모터 전원 알람.<en>LD TRAY TRANS Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 GRIP 모터 전원 알람.<en>LD TRAY TRANS GRIP MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>로드 PnP X 모터 전원 알람.<en>LD PNP X MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_PNP_X,
		[DefaultValueAttribute("<ko>로드 PnP Y 모터 전원 알람.<en>LD PNP Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_PNP_Y,
		[DefaultValueAttribute("<ko>로드 PnP Z 모터 전원 알람.<en>LD PNP Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_PNP_Z,
		[DefaultValueAttribute("<ko>로드 PnP W 모터 전원 알람.<en>LD PNP W MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_PNP_W,
		[DefaultValueAttribute("<ko>로드 검사 PnP X 모터 전원 알람.<en>LD INSP PNP X MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>로드 검사 PnP Z 모터 전원 알람.<en>LD INSP PNP Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>로드 버퍼1 Y 모터 전원 알람.<en>LD BUF1 Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_BUF1_Y,
		[DefaultValueAttribute("<ko>로드 버퍼1 GRIP 모터 전원 알람.<en>LD BUF1 GRIP MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_BUF1_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼1 R 모터 전원 알람.<en>LD BUF1 R MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_BUF1_R,
		[DefaultValueAttribute("<ko>로드 버퍼2 Y 모터 전원 알람.<en>LD BUF2 Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_BUF2_Y,
		[DefaultValueAttribute("<ko>로드 버퍼2 GRIP 모터 전원 알람.<en>LD BUF2 GRIP MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_BUF2_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼2 R 모터 전원 알람.<en>LD BUF2 R MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_BUF2_R,
		[DefaultValueAttribute("<ko>로드 테이블1 Z 모터 전원 알람.<en>LD TABLE1 Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TABLE1_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 Z 모터 전원 알람.<en>LD TABLE2 Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TABLE2_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 X 모터 전원 알람.<en>LD TABLE2 X MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TABLE2_X,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 전원 알람.<en>LD TABLE1 GRIP Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 전원 알람.<en>LD TABLE2 GRIP Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_LD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 W 모터 전원 알람.<en>INSP PLATE1 W MOTOR POWER ALARM.")]
		ERROR_MT_POWER_INSP_PLATE1_W,
		[DefaultValueAttribute("<ko>검사 플레이트1 Y 모터 전원 알람.<en>INSP PLATE1 Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_INSP_PLATE1_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 GRIP 모터 전원 알람.<en>INSP PLATE1 GRIP MOTOR POWER ALARM.")]
		ERROR_MT_POWER_INSP_PLATE1_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트1 Z 모터 전원 알람.<en>INSP PLATE1 Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_INSP_PLATE1_Z,
		[DefaultValueAttribute("<ko>언로드 리프트 Z 모터 전원 알람.<en>ULD TRAY LIFT Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>언로드 컨베이어1 모터 전원 알람.<en>ULD CONV1 MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_CONV1,
		[DefaultValueAttribute("<ko>언로드 컨베이어2 모터 전원 알람.<en>ULD CONV2 MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_CONV2,
		[DefaultValueAttribute("<ko>언로드 컨베이어 WIDTH 모터 전원 알람.<en>ULD CONV WIDTH MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 X 모터 전원 알람.<en>ULD TRAY TRANS X MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 Z 모터 전원 알람.<en>ULD TRAY TRANS Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 GRIP 모터 전원 알람.<en>ULD TRAY TRANS GRIP MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>언로드 PnP X 모터 전원 알람.<en>ULD PNP X MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_PNP_X,
		[DefaultValueAttribute("<ko>언로드 PnP Y 모터 전원 알람.<en>ULD PNP Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_PNP_Y,
		[DefaultValueAttribute("<ko>언로드 PnP Z 모터 전원 알람.<en>ULD PNP Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 PnP W 모터 전원 알람.<en>ULD PNP W MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_PNP_W,
		[DefaultValueAttribute("<ko>언로드 검사 PnP X 모터 전원 알람.<en>ULD INSP PNP X MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>언로드 검사 PnP Z 모터 전원 알람.<en>ULD INSP PNP Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 버퍼 Y 모터 전원 알람.<en>ULD BUF Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP Y 모터 전원 알람.<en>ULD BUF GRIP Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_BUF_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP X 모터 전원 알람.<en>ULD BUF GRIP X MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_BUF_GRIP_X,
		[DefaultValueAttribute("<ko>리젝트 버퍼 Y 모터 전원 알람.<en>REJECT BUF Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_REJECT_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 테이블1 Z 모터 전원 알람.<en>ULD TABLE1 Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_TABLE1_Z,
		[DefaultValueAttribute("<ko>언로드 테이블2 Z 모터 전원 알람.<en>ULD TABLE2 Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_TABLE2_Z,
		[DefaultValueAttribute("<ko>언로드 테이블1 GRIP Y 모터 전원 알람.<en>ULD TABLE1 GRIP Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 테이블2 GRIP Y 모터 전원 알람.<en>ULD TABLE2 GRIP Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_ULD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 W 모터 전원 알람.<en>INSP PLATE2 W MOTOR POWER ALARM.")]
		ERROR_MT_POWER_INSP_PLATE2_W,
		[DefaultValueAttribute("<ko>검사 플레이트2 Y 모터 전원 알람.<en>INSP PLATE2 Y MOTOR POWER ALARM.")]
		ERROR_MT_POWER_INSP_PLATE2_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 GRIP 모터 전원 알람.<en>INSP PLATE2 GRIP MOTOR POWER ALARM.")]
		ERROR_MT_POWER_INSP_PLATE2_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트2 Z 모터 전원 알람.<en>INSP PLATE2 Z MOTOR POWER ALARM.")]
		ERROR_MT_POWER_INSP_PLATE2_Z,


		ERROR_MT_LIMIT_START,
		[DefaultValueAttribute("<ko>로드 트레이 리프트 Z 모터 LIMIT 알람.<en>LD TRAY LIFT Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>로드 컨베이어1 모터 LIMIT 알람.<en>LD CONV1 MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_CONV1,
		[DefaultValueAttribute("<ko>로드 컨베이어2 모터 LIMIT 알람.<en>LD CONV2 MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_CONV2,
		[DefaultValueAttribute("<ko>로드 컨베이어 스토퍼 모터 LIMIT 알람.<en>LD CONV STOPPER MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_CONV_STOPPER,
		[DefaultValueAttribute("<ko>로드 컨베이어 WIDTH 모터 LIMIT 알람.<en>LD CONV WIDTH MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>로드 트레이 홀더1 Z 모터 LIMIT 알람.<en>LD TRAY HOLDER1 Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TRAY_HOLDER1_Z,
		[DefaultValueAttribute("<ko>로드 트레이 홀더2 Z 모터 LIMIT 알람.<en>LD TRAY HOLDER2 Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TRAY_HOLDER2_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 X 모터 LIMIT 알람.<en>LD TRAY TRANS X MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 Z 모터 LIMIT 알람.<en>LD TRAY TRANS Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 GRIP 모터 LIMIT 알람.<en>LD TRAY TRANS GRIP MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>로드 PnP X 모터 LIMIT 알람.<en>LD PNP X MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_PNP_X,
		[DefaultValueAttribute("<ko>로드 PnP Y 모터 LIMIT 알람.<en>LD PNP Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_PNP_Y,
		[DefaultValueAttribute("<ko>로드 PnP Z 모터 LIMIT 알람.<en>LD PNP Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_PNP_Z,
		[DefaultValueAttribute("<ko>로드 PnP W 모터 LIMIT 알람.<en>LD PNP W MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_PNP_W,
		[DefaultValueAttribute("<ko>로드 검사 PnP X 모터 LIMIT 알람.<en>LD INSP PNP X MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>로드 검사 PnP Z 모터 LIMIT 알람.<en>LD INSP PNP Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>로드 버퍼1 Y 모터 LIMIT 알람.<en>LD BUF1 Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_BUF1_Y,
		[DefaultValueAttribute("<ko>로드 버퍼1 GRIP 모터 LIMIT 알람.<en>LD BUF1 GRIP MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_BUF1_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼1 R 모터 LIMIT 알람.<en>LD BUF1 R MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_BUF1_R,
		[DefaultValueAttribute("<ko>로드 버퍼2 Y 모터 LIMIT 알람.<en>LD BUF2 Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_BUF2_Y,
		[DefaultValueAttribute("<ko>로드 버퍼2 GRIP 모터 LIMIT 알람.<en>LD BUF2 GRIP MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_BUF2_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼2 R 모터 LIMIT 알람.<en>LD BUF2 R MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_BUF2_R,
		[DefaultValueAttribute("<ko>로드 테이블1 Z 모터 LIMIT 알람.<en>LD TABLE1 Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TABLE1_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 Z 모터 LIMIT 알람.<en>LD TABLE2 Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TABLE2_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 X 모터 LIMIT 알람.<en>LD TABLE2 X MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TABLE2_X,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 LIMIT 알람.<en>LD TABLE1 GRIP Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 LIMIT 알람.<en>LD TABLE2 GRIP Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_LD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 W 모터 LIMIT 알람.<en>INSP PLATE1 W MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_INSP_PLATE1_W,
		[DefaultValueAttribute("<ko>검사 플레이트1 Y 모터 LIMIT 알람.<en>INSP PLATE1 Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_INSP_PLATE1_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 GRIP 모터 LIMIT 알람.<en>INSP PLATE1 GRIP MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_INSP_PLATE1_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트1 Z 모터 LIMIT 알람.<en>INSP PLATE1 Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_INSP_PLATE1_Z,
		[DefaultValueAttribute("<ko>언로드 리프트 Z 모터 LIMIT 알람.<en>ULD TRAY LIFT Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>언로드 컨베이어1 모터 LIMIT 알람.<en>ULD CONV1 MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_CONV1,
		[DefaultValueAttribute("<ko>언로드 컨베이어2 모터 LIMIT 알람.<en>ULD CONV2 MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_CONV2,
		[DefaultValueAttribute("<ko>언로드 컨베이어 WIDTH 모터 LIMIT 알람.<en>ULD CONV WIDTH MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 X 모터 LIMIT 알람.<en>ULD TRAY TRANS X MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 Z 모터 LIMIT 알람.<en>ULD TRAY TRANS Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 GRIP 모터 LIMIT 알람.<en>ULD TRAY TRANS GRIP MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>언로드 PnP X 모터 LIMIT 알람.<en>ULD PNP X MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_PNP_X,
		[DefaultValueAttribute("<ko>언로드 PnP Y 모터 LIMIT 알람.<en>ULD PNP Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_PNP_Y,
		[DefaultValueAttribute("<ko>언로드 PnP Z 모터 LIMIT 알람.<en>ULD PNP Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 PnP W 모터 LIMIT 알람.<en>ULD PNP W MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_PNP_W,
		[DefaultValueAttribute("<ko>언로드 검사 PnP X 모터 LIMIT 알람.<en>ULD INSP PNP X MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>언로드 검사 PnP Z 모터 LIMIT 알람.<en>ULD INSP PNP Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 버퍼 Y 모터 LIMIT 알람.<en>ULD BUF Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP Y 모터 LIMIT 알람.<en>ULD BUF GRIP Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_BUF_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP X 모터 LIMIT 알람.<en>ULD BUF GRIP X MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_BUF_GRIP_X,
		[DefaultValueAttribute("<ko>리젝트 버퍼 Y 모터 LIMIT 알람.<en>REJECT BUF Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_REJECT_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 테이블1 Z 모터 LIMIT 알람.<en>ULD TABLE1 Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_TABLE1_Z,
		[DefaultValueAttribute("<ko>언로드 테이블2 Z 모터 LIMIT 알람.<en>ULD TABLE2 Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_TABLE2_Z,
		[DefaultValueAttribute("<ko>언로드 테이블1 GRIP Y 모터 LIMIT 알람.<en>ULD TABLE1 GRIP Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 테이블2 GRIP Y 모터 LIMIT 알람.<en>ULD TABLE2 GRIP Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_ULD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 W 모터 LIMIT 알람.<en>INSP PLATE2 W MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_INSP_PLATE2_W,
		[DefaultValueAttribute("<ko>검사 플레이트2 Y 모터 LIMIT 알람.<en>INSP PLATE2 Y MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_INSP_PLATE2_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 GRIP 모터 LIMIT 알람.<en>INSP PLATE2 GRIP MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_INSP_PLATE2_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트2 Z 모터 LIMIT 알람.<en>INSP PLATE2 Z MOTOR LIMIT ALARM.")]
		ERROR_MT_LIMIT_INSP_PLATE2_Z,


		ERROR_MT_HOME_TIME_OUT_START,
		[DefaultValueAttribute("<ko>로드 트레이 리프트 Z 모터 HOME 알람.<en>LD TRAY LIFT Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>로드 컨베이어1 모터 HOME 알람.<en>LD CONV1 MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_CONV1,
		[DefaultValueAttribute("<ko>로드 컨베이어2 모터 HOME 알람.<en>LD CONV2 MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_CONV2,
		[DefaultValueAttribute("<ko>로드 컨베이어 스토퍼 모터 HOME 알람.<en>LD CONV STOPPER MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_CONV_STOPPER,
		[DefaultValueAttribute("<ko>로드 컨베이어 WIDTH 모터 HOME 알람.<en>LD CONV WIDTH MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>로드 트레이 홀더1 Z 모터 HOME 알람.<en>LD TRAY HOLDER1 Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TRAY_HOLDER1_Z,
		[DefaultValueAttribute("<ko>로드 트레이 홀더2 Z 모터 HOME 알람.<en>LD TRAY HOLDER2 Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TRAY_HOLDER2_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 X 모터 HOME 알람.<en>LD TRAY TRANS X MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 Z 모터 HOME 알람.<en>LD TRAY TRANS Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 GRIP 모터 HOME 알람.<en>LD TRAY TRANS GRIP MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>로드 PnP X 모터 HOME 알람.<en>LD PNP X MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_PNP_X,
		[DefaultValueAttribute("<ko>로드 PnP Y 모터 HOME 알람.<en>LD PNP Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_PNP_Y,
		[DefaultValueAttribute("<ko>로드 PnP Z 모터 HOME 알람.<en>LD PNP Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_PNP_Z,
		[DefaultValueAttribute("<ko>로드 PnP W 모터 HOME 알람.<en>LD PNP W MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_PNP_W,
		[DefaultValueAttribute("<ko>로드 검사 PnP X 모터 HOME 알람.<en>LD INSP PNP X MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>로드 검사 PnP Z 모터 HOME 알람.<en>LD INSP PNP Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>로드 버퍼1 Y 모터 HOME 알람.<en>LD BUF1 Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_BUF1_Y,
		[DefaultValueAttribute("<ko>로드 버퍼1 GRIP 모터 HOME 알람.<en>LD BUF1 GRIP MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_BUF1_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼1 R 모터 HOME 알람.<en>LD BUF1 R MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_BUF1_R,
		[DefaultValueAttribute("<ko>로드 버퍼2 Y 모터 HOME 알람.<en>LD BUF2 Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_BUF2_Y,
		[DefaultValueAttribute("<ko>로드 버퍼2 GRIP 모터 HOME 알람.<en>LD BUF2 GRIP MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_BUF2_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼2 R 모터 HOME 알람.<en>LD BUF2 R MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_BUF2_R,
		[DefaultValueAttribute("<ko>로드 테이블1 Z 모터 HOME 알람.<en>LD TABLE1 Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TABLE1_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 Z 모터 HOME 알람.<en>LD TABLE2 Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TABLE2_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 X 모터 HOME 알람.<en>LD TABLE2 X MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TABLE2_X,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 HOME 알람.<en>LD TABLE1 GRIP Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 HOME 알람.<en>LD TABLE2 GRIP Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_LD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 W 모터 HOME 알람.<en>INSP PLATE1 W MOTOR HOME ALARM.")]
		ERROR_MT_HOME_INSP_PLATE1_W,
		[DefaultValueAttribute("<ko>검사 플레이트1 Y 모터 HOME 알람.<en>INSP PLATE1 Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_INSP_PLATE1_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 GRIP 모터 HOME 알람.<en>INSP PLATE1 GRIP MOTOR HOME ALARM.")]
		ERROR_MT_HOME_INSP_PLATE1_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트1 Z 모터 HOME 알람.<en>INSP PLATE1 Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_INSP_PLATE1_Z,
		[DefaultValueAttribute("<ko>언로드 리프트 Z 모터 HOME 알람.<en>ULD TRAY LIFT Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>언로드 컨베이어1 모터 HOME 알람.<en>ULD CONV1 MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_CONV1,
		[DefaultValueAttribute("<ko>언로드 컨베이어2 모터 HOME 알람.<en>ULD CONV2 MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_CONV2,
		[DefaultValueAttribute("<ko>언로드 컨베이어 WIDTH 모터 HOME 알람.<en>ULD CONV WIDTH MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 X 모터 HOME 알람.<en>ULD TRAY TRANS X MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 Z 모터 HOME 알람.<en>ULD TRAY TRANS Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 GRIP 모터 HOME 알람.<en>ULD TRAY TRANS GRIP MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>언로드 PnP X 모터 HOME 알람.<en>ULD PNP X MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_PNP_X,
		[DefaultValueAttribute("<ko>언로드 PnP Y 모터 HOME 알람.<en>ULD PNP Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_PNP_Y,
		[DefaultValueAttribute("<ko>언로드 PnP Z 모터 HOME 알람.<en>ULD PNP Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 PnP W 모터 HOME 알람.<en>ULD PNP W MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_PNP_W,
		[DefaultValueAttribute("<ko>언로드 검사 PnP X 모터 HOME 알람.<en>ULD INSP PNP X MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>언로드 검사 PnP Z 모터 HOME 알람.<en>ULD INSP PNP Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 버퍼 Y 모터 HOME 알람.<en>ULD BUF Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP Y 모터 HOME 알람.<en>ULD BUF GRIP Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_BUF_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP X 모터 HOME 알람.<en>ULD BUF GRIP X MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_BUF_GRIP_X,
		[DefaultValueAttribute("<ko>리젝트 버퍼 Y 모터 HOME 알람.<en>REJECT BUF Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_REJECT_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 테이블1 Z 모터 HOME 알람.<en>ULD TABLE1 Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_TABLE1_Z,
		[DefaultValueAttribute("<ko>언로드 테이블2 Z 모터 HOME 알람.<en>ULD TABLE2 Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_TABLE2_Z,
		[DefaultValueAttribute("<ko>언로드 테이블1 GRIP Y 모터 HOME 알람.<en>ULD TABLE1 GRIP Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 테이블2 GRIP Y 모터 HOME 알람.<en>ULD TABLE2 GRIP Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_ULD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 W 모터 HOME 알람.<en>INSP PLATE2 W MOTOR HOME ALARM.")]
		ERROR_MT_HOME_INSP_PLATE2_W,
		[DefaultValueAttribute("<ko>검사 플레이트2 Y 모터 HOME 알람.<en>INSP PLATE2 Y MOTOR HOME ALARM.")]
		ERROR_MT_HOME_INSP_PLATE2_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 GRIP 모터 HOME 알람.<en>INSP PLATE2 GRIP MOTOR HOME ALARM.")]
		ERROR_MT_HOME_INSP_PLATE2_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트2 Z 모터 HOME 알람.<en>INSP PLATE2 Z MOTOR HOME ALARM.")]
		ERROR_MT_HOME_INSP_PLATE2_Z,


		ERROR_MT_MOVE_TIME_OUT_START,
		[DefaultValueAttribute("<ko>로드 트레이 리프트 Z 모터 이동 시간 초과 알람.<en>LD TRAY LIFT Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>로드 컨베이어1 모터 이동 시간 초과 알람.<en>LD CONV1 MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_CONV1,
		[DefaultValueAttribute("<ko>로드 컨베이어2 모터 이동 시간 초과 알람.<en>LD CONV2 MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_CONV2,
		[DefaultValueAttribute("<ko>로드 컨베이어 스토퍼 모터 이동 시간 초과 알람.<en>LD CONV STOPPER MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_CONV_STOPPER,
		[DefaultValueAttribute("<ko>로드 컨베이어 WIDTH 모터 이동 시간 초과 알람.<en>LD CONV WIDTH MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>로드 트레이 홀더1 Z 모터 이동 시간 초과 알람.<en>LD TRAY HOLDER1 Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TRAY_HOLDER1_Z,
		[DefaultValueAttribute("<ko>로드 트레이 홀더2 Z 모터 이동 시간 초과 알람.<en>LD TRAY HOLDER2 Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TRAY_HOLDER2_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 X 모터 이동 시간 초과 알람.<en>LD TRAY TRANS X MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 Z 모터 이동 시간 초과 알람.<en>LD TRAY TRANS Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 GRIP 모터 이동 시간 초과 알람.<en>LD TRAY TRANS GRIP MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>로드 PnP X 모터 이동 시간 초과 알람.<en>LD PNP X MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_PNP_X,
		[DefaultValueAttribute("<ko>로드 PnP Y 모터 이동 시간 초과 알람.<en>LD PNP Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_PNP_Y,
		[DefaultValueAttribute("<ko>로드 PnP Z 모터 이동 시간 초과 알람.<en>LD PNP Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_PNP_Z,
		[DefaultValueAttribute("<ko>로드 PnP W 모터 이동 시간 초과 알람.<en>LD PNP W MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_PNP_W,
		[DefaultValueAttribute("<ko>로드 검사 PnP X 모터 이동 시간 초과 알람.<en>LD INSP PNP X MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>로드 검사 PnP Z 모터 이동 시간 초과 알람.<en>LD INSP PNP Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>로드 버퍼1 Y 모터 이동 시간 초과 알람.<en>LD BUF1 Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_BUF1_Y,
		[DefaultValueAttribute("<ko>로드 버퍼1 GRIP 모터 이동 시간 초과 알람.<en>LD BUF1 GRIP MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_BUF1_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼1 R 모터 이동 시간 초과 알람.<en>LD BUF1 R MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_BUF1_R,
		[DefaultValueAttribute("<ko>로드 버퍼2 Y 모터 이동 시간 초과 알람.<en>LD BUF2 Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_BUF2_Y,
		[DefaultValueAttribute("<ko>로드 버퍼2 GRIP 모터 이동 시간 초과 알람.<en>LD BUF2 GRIP MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_BUF2_GRIP,
		[DefaultValueAttribute("<ko>로드 버퍼2 R 모터 이동 시간 초과 알람.<en>LD BUF2 R MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_BUF2_R,
		[DefaultValueAttribute("<ko>로드 테이블1 Z 모터 이동 시간 초과 알람.<en>LD TABLE1 Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TABLE1_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 Z 모터 이동 시간 초과 알람.<en>LD TABLE2 Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TABLE2_Z,
		[DefaultValueAttribute("<ko>로드 테이블2 X 모터 이동 시간 초과 알람.<en>LD TABLE2 X MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TABLE2_X,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 이동 시간 초과 알람.<en>LD TABLE1 GRIP Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>로드 테이블1 GRIP Y 모터 이동 시간 초과 알람.<en>LD TABLE2 GRIP Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_LD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 W 모터 이동 시간 초과 알람.<en>INSP PLATE1 W MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_INSP_PLATE1_W,
		[DefaultValueAttribute("<ko>검사 플레이트1 Y 모터 이동 시간 초과 알람.<en>INSP PLATE1 Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_INSP_PLATE1_Y,
		[DefaultValueAttribute("<ko>검사 플레이트1 GRIP 모터 이동 시간 초과 알람.<en>INSP PLATE1 GRIP MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_INSP_PLATE1_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트1 Z 모터 이동 시간 초과 알람.<en>INSP PLATE1 Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_INSP_PLATE1_Z,
		[DefaultValueAttribute("<ko>언로드 리프트 Z 모터 이동 시간 초과 알람.<en>ULD TRAY LIFT Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_TRAY_LIFT_Z,
		[DefaultValueAttribute("<ko>언로드 컨베이어1 모터 이동 시간 초과 알람.<en>ULD CONV1 MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_CONV1,
		[DefaultValueAttribute("<ko>언로드 컨베이어2 모터 이동 시간 초과 알람.<en>ULD CONV2 MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_CONV2,
		[DefaultValueAttribute("<ko>언로드 컨베이어 WIDTH 모터 이동 시간 초과 알람.<en>ULD CONV WIDTH MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_CONV_WIDTH,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 X 모터 이동 시간 초과 알람.<en>ULD TRAY TRANS X MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_TRAY_TRANS_X,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 Z 모터 이동 시간 초과 알람.<en>ULD TRAY TRANS Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_TRAY_TRANS_Z,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼 GRIP 모터 이동 시간 초과 알람.<en>ULD TRAY TRANS GRIP MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_TRAY_TRANS_GRIP,
		[DefaultValueAttribute("<ko>언로드 PnP X 모터 이동 시간 초과 알람.<en>ULD PNP X MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_PNP_X,
		[DefaultValueAttribute("<ko>언로드 PnP Y 모터 이동 시간 초과 알람.<en>ULD PNP Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_PNP_Y,
		[DefaultValueAttribute("<ko>언로드 PnP Z 모터 이동 시간 초과 알람.<en>ULD PNP Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 PnP W 모터 이동 시간 초과 알람.<en>ULD PNP W MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_PNP_W,
		[DefaultValueAttribute("<ko>언로드 검사 PnP X 모터 이동 시간 초과 알람.<en>ULD INSP PNP X MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_INSP_PNP_X,
		[DefaultValueAttribute("<ko>언로드 검사 PnP Z 모터 이동 시간 초과 알람.<en>ULD INSP PNP Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_INSP_PNP_Z,
		[DefaultValueAttribute("<ko>언로드 버퍼 Y 모터 이동 시간 초과 알람.<en>ULD BUF Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP Y 모터 이동 시간 초과 알람.<en>ULD BUF GRIP Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_BUF_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 버퍼 GRIP X 모터 이동 시간 초과 알람.<en>ULD BUF GRIP X MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_BUF_GRIP_X,
		[DefaultValueAttribute("<ko>리젝트 버퍼 Y 모터 이동 시간 초과 알람.<en>REJECT BUF Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_REJECT_BUF_Y,
		[DefaultValueAttribute("<ko>언로드 테이블1 Z 모터 이동 시간 초과 알람.<en>ULD TABLE1 Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_TABLE1_Z,
		[DefaultValueAttribute("<ko>언로드 테이블2 Z 모터 이동 시간 초과 알람.<en>ULD TABLE2 Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_TABLE2_Z,
		[DefaultValueAttribute("<ko>언로드 테이블1 GRIP Y 모터 이동 시간 초과 알람.<en>ULD TABLE1 GRIP Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_TABLE1_GRIP_Y,
		[DefaultValueAttribute("<ko>언로드 테이블2 GRIP Y 모터 이동 시간 초과 알람.<en>ULD TABLE2 GRIP Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_ULD_TABLE2_GRIP_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 W 모터 이동 시간 초과 알람.<en>INSP PLATE2 W MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_INSP_PLATE2_W,
		[DefaultValueAttribute("<ko>검사 플레이트2 Y 모터 이동 시간 초과 알람.<en>INSP PLATE2 Y MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_INSP_PLATE2_Y,
		[DefaultValueAttribute("<ko>검사 플레이트2 GRIP 모터 이동 시간 초과 알람.<en>INSP PLATE2 GRIP MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_INSP_PLATE2_GRIP,
		[DefaultValueAttribute("<ko>검사 플레이트2 Z 모터 이동 시간 초과 알람.<en>INSP PLATE2 Z MOTOR MOVE TIME OUT ALARM.")]
		ERROR_MT_TIME_OUT_INSP_PLATE2_Z,

		#endregion

		#region - Cylinder
		/// <summary>
		/// 실린더 알람 관련
		/// </summary>
		/// 
		[DefaultValueAttribute("<ko>LOAD LIFT STOPPER CYL 전진 센서 확인.<en>LOAD LIFT STOPPER CYL FORWARD SENSOR CHECK.")]
		ERROR_LOAD_LIFT_STOPPER_CYL_FORWARD,
		[DefaultValueAttribute("<ko>LOAD LIFT STOPPER CYL 후진 센서 확인.<en>LOAD LIFT STOPPER CYL BACKWARD SENSOR CHECK.")]
		ERROR_LOAD_LIFT_STOPPER_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD CONV TRAY HOLDER1 CYL 그립 센서 확인.<en>LOAD CONV TRAY HOLDER1 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_CONV_TRAY_HOLDER1_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD CONV TRAY HOLDER1 CYL 릴리즈 센서 확인.<en>LOAD CONV TRAY HOLDER1 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_CONV_TRAY_HOLDER1_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD CONV TRAY HOLDER2 CYL 그립 센서 확인.<en>LOAD CONV TRAY HOLDER2 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_CONV_TRAY_HOLDER2_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD CONV TRAY HOLDER2 CYL 릴리즈 센서 확인.<en>LOAD CONV TRAY HOLDER2 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_CONV_TRAY_HOLDER2_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD TABLE1 PAD1 CYL 전진 센서 확인.<en>LOAD TABLE1 PAD1 CYL FORWARD SENSOR CHECK.")]
		ERROR_LOAD_TABLE1_PAD1_CYL_FORWARD,
		[DefaultValueAttribute("<ko>LOAD TABLE1 PAD1 CYL 후진 센서 확인.<en>LOAD TABLE1 PAD1 CYL BACKWARD SENSOR CHECK.")]
		ERROR_LOAD_TABLE1_PAD1_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD TABLE1 PAD2 CYL 전진 센서 확인.<en>LOAD TABLE1 PAD2 CYL FORWARD SENSOR CHECK.")]
		ERROR_LOAD_TABLE1_PAD2_CYL_FORWARD,
		[DefaultValueAttribute("<ko>LOAD TABLE1 PAD2 CYL 후진 센서 확인.<en>LOAD TABLE1 PAD2 CYL BACKWARD SENSOR CHECK.")]
		ERROR_LOAD_TABLE1_PAD2_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD TABLE1 GRIP CYL 그립 센서 확인.<en>LOAD TABLE1 GRIP CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_TABLE1_GRIP_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD TABLE1 GRIP CYL 릴리즈 센서 확인.<en>LOAD TABLE1 GRIP CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_TABLE1_GRIP_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD TABLE2 PAD1 CYL 전진 센서 확인.<en>LOAD TABLE2 PAD1 CYL FORWARD SENSOR CHECK.")]
		ERROR_LOAD_TABLE2_PAD1_CYL_FORWARD,
		[DefaultValueAttribute("<ko>LOAD TABLE2 PAD1 CYL 후진 센서 확인.<en>LOAD TABLE2 PAD1 CYL BACKWARD SENSOR CHECK.")]
		ERROR_LOAD_TABLE2_PAD1_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD TABLE2 PAD2 CYL 전진 센서 확인.<en>LOAD TABLE2 PAD2 CYL FORWARD SENSOR CHECK.")]
		ERROR_LOAD_TABLE2_PAD2_CYL_FORWARD,
		[DefaultValueAttribute("<ko>LOAD TABLE2 PAD2 CYL 후진 센서 확인.<en>LOAD TABLE2 PAD2 CYL BACKWARD SENSOR CHECK.")]
		ERROR_LOAD_TABLE2_PAD2_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD TABLE2 GRIP CYL 그립 센서 확인.<en>LOAD TABLE2 GRIP CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_TABLE2_GRIP_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD TABLE2 GRIP CYL 릴리즈 센서 확인.<en>LOAD TABLE2 GRIP CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_TABLE2_GRIP_CYL_RELEASE,
		[DefaultValueAttribute("<ko>TRAY CLEAN GRIP CYL 그립 센서 확인.<en>TRAY CLEAN GRIP CYL GRIP SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_GRIP_CYL_GRIP,
		[DefaultValueAttribute("<ko>TRAY CLEAN GRIP CYL 릴리즈 센서 확인.<en>TRAY CLEAN GRIP CYL RELEASE SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_GRIP_CYL_RELEASE,
		[DefaultValueAttribute("<ko>TRAY CLEAN ROTATE CYL 전진 센서 확인.<en>TRAY CLEAN ROTATE CYL FORWARD SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_ROTATE_CYL_FORWARD,
		[DefaultValueAttribute("<ko>TRAY CLEAN ROTATE CYL 후진 센서 확인.<en>TRAY CLEAN ROTATE CYL BACKWARD SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_ROTATE_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>TRAY CLEAN UP CYL 업 센서 확인.<en>TRAY CLEAN UP CYL UP SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_UP_CYL_UP,
		[DefaultValueAttribute("<ko>TRAY CLEAN UP CYL 다운 센서 확인.<en>TRAY CLEAN UP CYL DOWN SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_UP_CYL_DOWN,
		[DefaultValueAttribute("<ko>TRAY CLEAN AIR CYL 전진 센서 확인.<en>TRAY CLEAN AIR CYL FORWARD SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_AIR_CYL_FORWARD,
		[DefaultValueAttribute("<ko>TRAY CLEAN AIR CYL 후진 센서 확인.<en>TRAY CLEAN AIR CYL BACKWARD SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_AIR_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>TRAY CLEAN DUST CYL 전진 센서 확인.<en>TRAY CLEAN DUST CYL FORWARD SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_DUST_CYL_FORWARD,
		[DefaultValueAttribute("<ko>TRAY CLEAN DUST CYL 후진 센서 확인.<en>TRAY CLEAN DUST CYL BACKWARD SENSOR CHECK.")]
		ERROR_TRAY_CLEAN_DUST_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD PNP UPDN1 CYL 업 센서 확인.<en>LOAD PNP UPDN1 CYL UP SENSOR CHECK.")]
		ERROR_LOAD_PNP_UPDN1_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD PNP UPDN1 CYL 다운 센서 확인.<en>LOAD PNP UPDN1 CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_PNP_UPDN1_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD PNP UPDN2 CYL 업 센서 확인.<en>LOAD PNP UPDN2 CYL UP SENSOR CHECK.")]
		ERROR_LOAD_PNP_UPDN2_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD PNP UPDN2 CYL 다운 센서 확인.<en>LOAD PNP UPDN2 CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_PNP_UPDN2_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD PNP UPDN3 CYL 업 센서 확인.<en>LOAD PNP UPDN3 CYL UP SENSOR CHECK.")]
		ERROR_LOAD_PNP_UPDN3_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD PNP UPDN3 CYL 다운 센서 확인.<en>LOAD PNP UPDN3 CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_PNP_UPDN3_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD PNP UPDN4 CYL 업 센서 확인.<en>LOAD PNP UPDN4 CYL UP SENSOR CHECK.")]
		ERROR_LOAD_PNP_UPDN4_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD PNP UPDN4 CYL 다운 센서 확인.<en>LOAD PNP UPDN4 CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_PNP_UPDN4_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD PNP VAC1 Ejector 흡착 센서 확인.<en>LOAD PNP VAC1  VAC SENSOR CHECK.")]
		ERROR_LOAD_PNP_VAC1_VAC,
        [DefaultValueAttribute("<ko>LOAD PNP VAC2 Ejector 흡착 센서 확인.<en>LOAD PNP VAC2  VAC SENSOR CHECK.")]
        ERROR_LOAD_PNP_VAC2_VAC,
        [DefaultValueAttribute("<ko>LOAD PNP VAC3 Ejector 흡착 센서 확인.<en>LOAD PNP VAC3  VAC SENSOR CHECK.")]
        ERROR_LOAD_PNP_VAC3_VAC,
        [DefaultValueAttribute("<ko>LOAD PNP VAC4 Ejector 흡착 센서 확인.<en>LOAD PNP VAC4  VAC SENSOR CHECK.")]
        ERROR_LOAD_PNP_VAC4_VAC,
        [DefaultValueAttribute("<ko>LOAD PNP VAC1 Ejector 파기 센서 확인.<en>LOAD PNP VAC1  BLOW SENSOR CHECK.")]
		ERROR_LOAD_PNP_VAC1_BLOW,
		[DefaultValueAttribute("<ko>LOAD PNP VAC2 Ejector 파기 센서 확인.<en>LOAD PNP VAC2  BLOW SENSOR CHECK.")]
		ERROR_LOAD_PNP_VAC2_BLOW,
		[DefaultValueAttribute("<ko>LOAD PNP VAC3 Ejector 파기 센서 확인.<en>LOAD PNP VAC3  BLOW SENSOR CHECK.")]
		ERROR_LOAD_PNP_VAC3_BLOW,
		[DefaultValueAttribute("<ko>LOAD PNP VAC4 Ejector 파기 센서 확인.<en>LOAD PNP VAC4  BLOW SENSOR CHECK.")]
		ERROR_LOAD_PNP_VAC4_BLOW,
		[DefaultValueAttribute("<ko>SMART CAMERA MOVE CYL 전진 센서 확인.<en>SMART CAMERA MOVE CYL FORWARD SENSOR CHECK.")]
		ERROR_SMART_CAMERA_MOVE_CYL_FORWARD,
		[DefaultValueAttribute("<ko>SMART CAMERA MOVE CYL 후진 센서 확인.<en>SMART CAMERA MOVE CYL BACKWARD SENSOR CHECK.")]
		ERROR_SMART_CAMERA_MOVE_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 GRIP1 CYL 그립 센서 확인.<en>LOAD BUFFER1 GRIP1 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP1_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 GRIP1 CYL 릴리즈 센서 확인.<en>LOAD BUFFER1 GRIP1 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP1_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 GRIP2 CYL 그립 센서 확인.<en>LOAD BUFFER1 GRIP2 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP2_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 GRIP2 CYL 릴리즈 센서 확인.<en>LOAD BUFFER1 GRIP2 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP2_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 GRIP3 CYL 그립 센서 확인.<en>LOAD BUFFER1 GRIP3 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP3_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 GRIP3 CYL 릴리즈 센서 확인.<en>LOAD BUFFER1 GRIP3 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP3_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 GRIP4 CYL 그립 센서 확인.<en>LOAD BUFFER1 GRIP4 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP4_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 GRIP4 CYL 릴리즈 센서 확인.<en>LOAD BUFFER1 GRIP4 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP4_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 GRIP1 CYL 그립 센서 확인.<en>LOAD BUFFER2 GRIP1 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP1_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 GRIP1 CYL 릴리즈 센서 확인.<en>LOAD BUFFER2 GRIP1 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP1_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 GRIP2 CYL 그립 센서 확인.<en>LOAD BUFFER2 GRIP2 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP2_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 GRIP2 CYL 릴리즈 센서 확인.<en>LOAD BUFFER2 GRIP2 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP2_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 GRIP3 CYL 그립 센서 확인.<en>LOAD BUFFER2 GRIP3 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP3_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 GRIP3 CYL 릴리즈 센서 확인.<en>LOAD BUFFER2 GRIP3 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP3_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 GRIP4 CYL 그립 센서 확인.<en>LOAD BUFFER2 GRIP4 CYL GRIP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP4_CYL_GRIP,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 GRIP4 CYL 릴리즈 센서 확인.<en>LOAD BUFFER2 GRIP4 CYL RELEASE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP4_CYL_RELEASE,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 TABLE UPDN CYL 업 센서 확인.<en>LOAD BUFFER1 TABLE UPDN CYL UP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_TABLE_UPDN_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 TABLE UPDN CYL 다운 센서 확인.<en>LOAD BUFFER1 TABLE UPDN CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_TABLE_UPDN_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 TABLE GRIP CYL CLOSE 센서 확인.<en>LOAD BUFFER1 TABLE GRIP CYL CLOSE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP_CYL_CLOSE,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 TABLE GRIP CYL OPEN 센서 확인.<en>LOAD BUFFER1 TABLE GRIP CYL OPEN SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_GRIP_CYL_OPEN,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 CLEAN UPDN CYL 업 센서 확인.<en>LOAD BUFFER1 CLEAN UPDN CYL UP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_CLEAN_UPDN_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 CLEAN UPDN CYL 다운 센서 확인.<en>LOAD BUFFER1 CLEAN UPDN CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_CLEAN_UPDN_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 CLEAN AIR CYL 전진 센서 확인.<en>LOAD BUFFER1 CLEAN AIR CYL FORWARD SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_CLEAN_AIR_CYL_FORWARD,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 CLEAN AIR CYL 후진 센서 확인.<en>LOAD BUFFER1 CLEAN AIR CYL BACKWARD SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_CLEAN_AIR_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 DUCT UPDN CYL 업 센서 확인.<en>LOAD BUFFER1 DUCT UPDN CYL UP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_DUCT_UPDN_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 DUCT UPDN CYL 다운 센서 확인.<en>LOAD BUFFER1 DUCT UPDN CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_DUCT_UPDN_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 DUST CYL 전진 센서 확인.<en>LOAD BUFFER1 DUST CYL FORWARD SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_DUST_CYL_FORWARD,
		[DefaultValueAttribute("<ko>LOAD BUFFER1 DUST CYL 후진 센서 확인.<en>LOAD BUFFER1 DUST CYL BACKWARD SENSOR CHECK.")]
		ERROR_LOAD_BUFFER1_DUST_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 TABLE UPDN CYL 업 센서 확인.<en>LOAD BUFFER2 TABLE UPDN CYL UP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_TABLE_UPDN_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 TABLE UPDN CYL 다운 센서 확인.<en>LOAD BUFFER2 TABLE UPDN CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_TABLE_UPDN_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 TABLE GRIP CYL CLOSE 센서 확인.<en>LOAD BUFFER2 TABLE GRIP CYL CLOSE SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP_CYL_CLOSE,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 TABLE GRIP CYL OPEN 센서 확인.<en>LOAD BUFFER2 TABLE GRIP CYL OPEN SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_GRIP_CYL_OPEN,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 CLEAN UPDN CYL 업 센서 확인.<en>LOAD BUFFER2 CLEAN UPDN CYL UP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_CLEAN_UPDN_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 CLEAN UPDN CYL 다운 센서 확인.<en>LOAD BUFFER2 CLEAN UPDN CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_CLEAN_UPDN_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 CLEAN AIR CYL 전진 센서 확인.<en>LOAD BUFFER2 CLEAN AIR CYL FORWARD SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_CLEAN_AIR_CYL_FORWARD,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 CLEAN AIR CYL 후진 센서 확인.<en>LOAD BUFFER2 CLEAN AIR CYL BACKWARD SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_CLEAN_AIR_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 DUCT UPDN CYL 업 센서 확인.<en>LOAD BUFFER2 DUCT UPDN CYL UP SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_DUCT_UPDN_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 DUCT UPDN CYL 다운 센서 확인.<en>LOAD BUFFER2 DUCT UPDN CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_DUCT_UPDN_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 DUST CYL 전진 센서 확인.<en>LOAD BUFFER2 DUST CYL FORWARD SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_DUST_CYL_FORWARD,
		[DefaultValueAttribute("<ko>LOAD BUFFER2 DUST CYL 후진 센서 확인.<en>LOAD BUFFER2 DUST CYL BACKWARD SENSOR CHECK.")]
		ERROR_LOAD_BUFFER2_DUST_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>LOAD INSP PNP UPDN CYL 업 센서 확인.<en>LOAD INSP PNP UPDN CYL UP SENSOR CHECK.")]
		ERROR_LOAD_INSP_PNP_UPDN_CYL_UP,
		[DefaultValueAttribute("<ko>LOAD INSP PNP UPDN CYL 다운 센서 확인.<en>LOAD INSP PNP UPDN CYL DOWN SENSOR CHECK.")]
		ERROR_LOAD_INSP_PNP_UPDN_CYL_DOWN,
		[DefaultValueAttribute("<ko>LOAD INSP PNP VAC1 Ejector 흡착 센서 확인.<en>LOAD INSP PNP VAC1  VAC SENSOR CHECK.")]
		ERROR_LOAD_INSP_PNP_VAC1_VAC,
        [DefaultValueAttribute("<ko>LOAD INSP PNP VAC2 Ejector 흡착 센서 확인.<en>LOAD INSP PNP VAC2  VAC SENSOR CHECK.")]
        ERROR_LOAD_INSP_PNP_VAC2_VAC,
        [DefaultValueAttribute("<ko>LOAD INSP PNP VAC3 Ejector 흡착 센서 확인.<en>LOAD INSP PNP VAC3  VAC SENSOR CHECK.")]
        ERROR_LOAD_INSP_PNP_VAC3_VAC,
        [DefaultValueAttribute("<ko>LOAD INSP PNP VAC4 Ejector 흡착 센서 확인.<en>LOAD INSP PNP VAC4  VAC SENSOR CHECK.")]
        ERROR_LOAD_INSP_PNP_VAC4_VAC,
        [DefaultValueAttribute("<ko>LOAD INSP PNP VAC1 Ejector 파기 센서 확인.<en>LOAD INSP PNP VAC1  BLOW SENSOR CHECK.")]
		ERROR_LOAD_INSP_PNP_VAC1_BLOW,
		[DefaultValueAttribute("<ko>LOAD INSP PNP VAC2 Ejector 파기 센서 확인.<en>LOAD INSP PNP VAC2  BLOW SENSOR CHECK.")]
		ERROR_LOAD_INSP_PNP_VAC2_BLOW,
		[DefaultValueAttribute("<ko>LOAD INSP PNP VAC3 Ejector 파기 센서 확인.<en>LOAD INSP PNP VAC3  BLOW SENSOR CHECK.")]
		ERROR_LOAD_INSP_PNP_VAC3_BLOW,
		[DefaultValueAttribute("<ko>LOAD INSP PNP VAC4 Ejector 파기 센서 확인.<en>LOAD INSP PNP VAC4  BLOW SENSOR CHECK.")]
		ERROR_LOAD_INSP_PNP_VAC4_BLOW,
		[DefaultValueAttribute("<ko>INSP PLATE1 GRIP CYL 그립 센서 확인.<en>INSP PLATE1 GRIP CYL GRIP SENSOR CHECK.")]
		ERROR_INSP_PLATE1_GRIP_CYL_GRIP,
		[DefaultValueAttribute("<ko>INSP PLATE1 GRIP CYL 릴리즈 센서 확인.<en>INSP PLATE1 GRIP CYL RELEASE SENSOR CHECK.")]
		ERROR_INSP_PLATE1_GRIP_CYL_RELEASE,
		[DefaultValueAttribute("<ko>INSP PLATE2 GRIP CYL 그립 센서 확인.<en>INSP PLATE2 GRIP CYL GRIP SENSOR CHECK.")]
		ERROR_INSP_PLATE2_GRIP_CYL_GRIP,
		[DefaultValueAttribute("<ko>INSP PLATE2 GRIP CYL 릴리즈 센서 확인.<en>INSP PLATE2 GRIP CYL RELEASE SENSOR CHECK.")]
		ERROR_INSP_PLATE2_GRIP_CYL_RELEASE,
		[DefaultValueAttribute("<ko>UNLOAD LIFT STOPPER CYL 전진 센서 확인.<en>UNLOAD LIFT STOPPER CYL FORWARD SENSOR CHECK.")]
		ERROR_UNLOAD_LIFT_STOPPER_CYL_FORWARD,
		[DefaultValueAttribute("<ko>UNLOAD LIFT STOPPER CYL 후진 센서 확인.<en>UNLOAD LIFT STOPPER CYL BACKWARD SENSOR CHECK.")]
		ERROR_UNLOAD_LIFT_STOPPER_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>UNLOAD TABLE1 PAD CYL 전진 센서 확인.<en>UNLOAD TABLE1 PAD CYL FORWARD SENSOR CHECK.")]
		ERROR_UNLOAD_TABLE1_PAD_CYL_FORWARD,
		[DefaultValueAttribute("<ko>UNLOAD TABLE1 PAD CYL 후진 센서 확인.<en>UNLOAD TABLE1 PAD CYL BACKWARD SENSOR CHECK.")]
		ERROR_UNLOAD_TABLE1_PAD_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>UNLOAD TABLE1 GRIP CYL 그립 센서 확인.<en>UNLOAD TABLE1 GRIP CYL GRIP SENSOR CHECK.")]
		ERROR_UNLOAD_TABLE1_GRIP_CYL_GRIP,
		[DefaultValueAttribute("<ko>UNLOAD TABLE1 GRIP CYL 릴리즈 센서 확인.<en>UNLOAD TABLE1 GRIP CYL RELEASE SENSOR CHECK.")]
		ERROR_UNLOAD_TABLE1_GRIP_CYL_RELEASE,
		[DefaultValueAttribute("<ko>UNLOAD TABLE2 PAD CYL 전진 센서 확인.<en>UNLOAD TABLE2 PAD CYL FORWARD SENSOR CHECK.")]
		ERROR_UNLOAD_TABLE2_PAD_CYL_FORWARD,
		[DefaultValueAttribute("<ko>UNLOAD TABLE2 PAD CYL 후진 센서 확인.<en>UNLOAD TABLE2 PAD CYL BACKWARD SENSOR CHECK.")]
		ERROR_UNLOAD_TABLE2_PAD_CYL_BACKWARD,
		[DefaultValueAttribute("<ko>UNLOAD TABLE2 GRIP CYL 그립 센서 확인.<en>UNLOAD TABLE2 GRIP CYL GRIP SENSOR CHECK.")]
		ERROR_UNLOAD_TABLE2_GRIP_CYL_GRIP,
		[DefaultValueAttribute("<ko>UNLOAD TABLE2 GRIP CYL 릴리즈 센서 확인.<en>UNLOAD TABLE2 GRIP CYL RELEASE SENSOR CHECK.")]
		ERROR_UNLOAD_TABLE2_GRIP_CYL_RELEASE,
		[DefaultValueAttribute("<ko>UNLOAD PNP UPDN1 CYL 업 센서 확인.<en>UNLOAD PNP UPDN1 CYL UP SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_UPDN1_CYL_UP,
		[DefaultValueAttribute("<ko>UNLOAD PNP UPDN1 CYL 다운 센서 확인.<en>UNLOAD PNP UPDN1 CYL DOWN SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_UPDN1_CYL_DOWN,
		[DefaultValueAttribute("<ko>UNLOAD PNP UPDN2 CYL 업 센서 확인.<en>UNLOAD PNP UPDN2 CYL UP SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_UPDN2_CYL_UP,
		[DefaultValueAttribute("<ko>UNLOAD PNP UPDN2 CYL 다운 센서 확인.<en>UNLOAD PNP UPDN2 CYL DOWN SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_UPDN2_CYL_DOWN,
		[DefaultValueAttribute("<ko>UNLOAD PNP UPDN3 CYL 업 센서 확인.<en>UNLOAD PNP UPDN3 CYL UP SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_UPDN3_CYL_UP,
		[DefaultValueAttribute("<ko>UNLOAD PNP UPDN3 CYL 다운 센서 확인.<en>UNLOAD PNP UPDN3 CYL DOWN SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_UPDN3_CYL_DOWN,
		[DefaultValueAttribute("<ko>UNLOAD PNP UPDN4 CYL 업 센서 확인.<en>UNLOAD PNP UPDN4 CYL UP SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_UPDN4_CYL_UP,
		[DefaultValueAttribute("<ko>UNLOAD PNP UPDN4 CYL 다운 센서 확인.<en>UNLOAD PNP UPDN4 CYL DOWN SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_UPDN4_CYL_DOWN,
		[DefaultValueAttribute("<ko>UNLOAD PNP VAC1 Ejector 흡착 센서 확인.<en>UNLOAD PNP VAC1  VAC SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_VAC1_VAC,
        [DefaultValueAttribute("<ko>UNLOAD PNP VAC2 Ejector 흡착 센서 확인.<en>UNLOAD PNP VAC2  VAC SENSOR CHECK.")]
        ERROR_UNLOAD_PNP_VAC2_VAC,
        [DefaultValueAttribute("<ko>UNLOAD PNP VAC3 Ejector 흡착 센서 확인.<en>UNLOAD PNP VAC3  VAC SENSOR CHECK.")]
        ERROR_UNLOAD_PNP_VAC3_VAC,
        [DefaultValueAttribute("<ko>UNLOAD PNP VAC4 Ejector 흡착 센서 확인.<en>UNLOAD PNP VAC4  VAC SENSOR CHECK.")]
        ERROR_UNLOAD_PNP_VAC4_VAC,
        [DefaultValueAttribute("<ko>UNLOAD PNP VAC1 Ejector 파기 센서 확인.<en>UNLOAD PNP VAC1  BLOW SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_VAC1_BLOW,
		[DefaultValueAttribute("<ko>UNLOAD PNP VAC2 Ejector 파기 센서 확인.<en>UNLOAD PNP VAC2  BLOW SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_VAC2_BLOW,
		[DefaultValueAttribute("<ko>UNLOAD PNP VAC3 Ejector 파기 센서 확인.<en>UNLOAD PNP VAC3  BLOW SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_VAC3_BLOW,
		[DefaultValueAttribute("<ko>UNLOAD PNP VAC4 Ejector 파기 센서 확인.<en>UNLOAD PNP VAC4  BLOW SENSOR CHECK.")]
		ERROR_UNLOAD_PNP_VAC4_BLOW,
		[DefaultValueAttribute("<ko>REJECT TABLE GRIP X CYL 그립 센서 확인.<en>REJECT TABLE GRIP X CYL GRIP SENSOR CHECK.")]
		ERROR_REJECT_TABLE_GRIP_X_CYL_GRIP,
		[DefaultValueAttribute("<ko>REJECT TABLE GRIP X CYL 릴리즈 센서 확인.<en>REJECT TABLE GRIP X CYL RELEASE SENSOR CHECK.")]
		ERROR_REJECT_TABLE_GRIP_X_CYL_RELEASE,
		[DefaultValueAttribute("<ko>REJECT TABLE GRIP Y CYL 그립 센서 확인.<en>REJECT TABLE GRIP Y CYL GRIP SENSOR CHECK.")]
		ERROR_REJECT_TABLE_GRIP_Y_CYL_GRIP,
		[DefaultValueAttribute("<ko>REJECT TABLE GRIP Y CYL 릴리즈 센서 확인.<en>REJECT TABLE GRIP Y CYL RELEASE SENSOR CHECK.")]
		ERROR_REJECT_TABLE_GRIP_Y_CYL_RELEASE,
		[DefaultValueAttribute("<ko>UNLOAD INSP PNP UPDN CYL 업 센서 확인.<en>UNLOAD INSP PNP UPDN CYL UP SENSOR CHECK.")]
		ERROR_UNLOAD_INSP_PNP_UPDN_CYL_UP,
		[DefaultValueAttribute("<ko>UNLOAD INSP PNP UPDN CYL 다운 센서 확인.<en>UNLOAD INSP PNP UPDN CYL DOWN SENSOR CHECK.")]
		ERROR_UNLOAD_INSP_PNP_UPDN_CYL_DOWN,
		[DefaultValueAttribute("<ko>UNLOAD INSP PNP VAC1 Ejector 흡착 센서 확인.<en>UNLOAD INSP PNP VAC1  VAC SENSOR CHECK.")]
		ERROR_UNLOAD_INSP_PNP_VAC1_VAC,
        [DefaultValueAttribute("<ko>UNLOAD INSP PNP VAC2 Ejector 흡착 센서 확인.<en>UNLOAD INSP PNP VAC2  VAC SENSOR CHECK.")]
        ERROR_UNLOAD_INSP_PNP_VAC2_VAC,
        [DefaultValueAttribute("<ko>UNLOAD INSP PNP VAC3 Ejector 흡착 센서 확인.<en>UNLOAD INSP PNP VAC3  VAC SENSOR CHECK.")]
        ERROR_UNLOAD_INSP_PNP_VAC3_VAC,
        [DefaultValueAttribute("<ko>UNLOAD INSP PNP VAC4 Ejector 흡착 센서 확인.<en>UNLOAD INSP PNP VAC4  VAC SENSOR CHECK.")]
        ERROR_UNLOAD_INSP_PNP_VAC4_VAC,
        [DefaultValueAttribute("<ko>UNLOAD INSP PNP VAC1 Ejector 파기 센서 확인.<en>UNLOAD INSP PNP VAC1  BLOW SENSOR CHECK.")]
		ERROR_UNLOAD_INSP_PNP_VAC1_BLOW,
		[DefaultValueAttribute("<ko>UNLOAD INSP PNP VAC2 Ejector 파기 센서 확인.<en>UNLOAD INSP PNP VAC2  BLOW SENSOR CHECK.")]
		ERROR_UNLOAD_INSP_PNP_VAC2_BLOW,
		[DefaultValueAttribute("<ko>UNLOAD INSP PNP VAC3 Ejector 파기 센서 확인.<en>UNLOAD INSP PNP VAC3  BLOW SENSOR CHECK.")]
		ERROR_UNLOAD_INSP_PNP_VAC3_BLOW,
		[DefaultValueAttribute("<ko>UNLOAD INSP PNP VAC4 Ejector 파기 센서 확인.<en>UNLOAD INSP PNP VAC4  BLOW SENSOR CHECK.")]
		ERROR_UNLOAD_INSP_PNP_VAC4_BLOW,



		#endregion

		#region - SEQ Loader
		ERROR_SEQ_LOADER_LIFT_START = 500,
		[DefaultValueAttribute("<ko>로더 리프트의 언로드 준비상태를 확인 해 주세요.<en>Please check the loader lift's ready to unload.")]
		ERROR_SEQ_LOADER_LIFT_UNLOAD_ENABLE_CHECK,
		[DefaultValueAttribute("<ko>로더 리프트의 에어리어 센서1이 감지되었습니다.<en>Area sensor 1 on loader lift detected.")]
		ERROR_SEQ_LOADER_LIFT_AREA_SENSOR1_CHECK,
		[DefaultValueAttribute("<ko>로더 리프트의 에어리어 센서2가 감지되었습니다.<en>Area sensor 2 on loader lift detected.")]
		ERROR_SEQ_LOADER_LIFT_AREA_SENSOR2_CHECK,
		[DefaultValueAttribute("<ko>로더 리프트에 트레이가 없습니다. 트레이가 컨베이어 중간에 있는 경우 READY 동작 후 실행해주세요.<en>There is no tray in the loader lift. If the tray is in the middle of the conveyor, run after the READY action.")]
		ERROR_SEQ_LOADER_LIFT_TRAY_NONE,
		[DefaultValueAttribute("<ko>로더 리프트에 트레이가 있습니다.<en>There is a tray on the loader lift.")]
		ERROR_SEQ_LOADER_LIFT_TRAY_EXIST,
		[DefaultValueAttribute("<ko>로더 리프트의 트레이 정보와 센서가 상태가 다릅니다.<en>The loader lift's tray information and sensor are in different condition.")]
		ERROR_SEQ_LOADER_LIFT_TRAY_MISMATCH,
		[DefaultValueAttribute("<ko>로더 리프트의 트레이 최대 갯수를 초과하였습니다.<en>The maximum number of trays on the loader lift has been exceeded.")]
		ERROR_SEQ_LOADER_LIFT_LIMIT_SENSOR_CHECK,


		ERROR_SEQ_LOADER_CONV_START = 510,
		[DefaultValueAttribute("<ko>로더 컨베이어의 트레이 감지 센서가 감지되었습니다.<en>Tray detection sensor on loader conveyor detected.")]
		ERROR_SEQ_LOADER_CONV_EXIST_SENSOR_CHECK,
		[DefaultValueAttribute("<ko>로더 컨베이어에 트레이가 없습니다.<en>There are no trays in the loader conveyor.")]
		ERROR_SEQ_LOADER_CONV_TRAY_NONE,
		[DefaultValueAttribute("<ko>로더 컨베이어에 트레이가 있습니다.<en>There is a tray on the loader conveyor.")]
		ERROR_SEQ_LOADER_CONV_TRAY_EXIST,
		[DefaultValueAttribute("<ko>로더 컨베이어의 트레이 정보와 센서가 상태가 다릅니다.<en>The tray information of the loader conveyor and the sensor are in different condition.")]
		ERROR_SEQ_LOADER_CONV_TRAY_MISMATCH,
		[DefaultValueAttribute("<ko>로더 컨베이어의 트레이 크기가 레시피와 다릅니다.<en>The tray size of the loader conveyor is different from the recipe.")]
		ERROR_SEQ_LOADER_CONV_TRAY_SIZE_DIFFERENT_RECIPE,
		#endregion

		#region - SEQ Load Tray Table
		ERROR_SEQ_LOAD_TRAY_TABLE1_START = 520,
		[DefaultValueAttribute("<ko>로드 트레이 테이블1에 트레이가 존재하여 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Tray exists in Load Tray Table1. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_EXIST,
		[DefaultValueAttribute("<ko>로드 트레이 테이블1에 트레이 정보와 센서가 일치하지 않습니다. 확인해주세요.<en>Tray information and sensor mismatch in load tray table 1. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_MISMATCH,
		[DefaultValueAttribute("<ko>로드 트레이 테이블1에 트레이가 존재하지 않아 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Tray Not exists in Load Tray Table1. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE1_TRAY_NONE,
		[DefaultValueAttribute("<ko>로드 트레이 테이블1가 아래쪽에 있는 상태라 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Load Tray Table1 is at the bottom. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE1_UP_POSITION,
		[DefaultValueAttribute("<ko>로드 트레이 테이블1에 PAD실린더가 전진상태가 아닙니다. 확인해주세요.<en>Load Tray Table1 Pad Cylinder is Not in Forward State. Please check it. ")]
		ERROR_SEQ_LOAD_TRAY_TABLE1_PAD_NOT_FWD,
		[DefaultValueAttribute("<ko>로드 트레이 테이블1의 그립이 해제되어 있지 않습니다.확인해주세요<en>Load Tray Table1 is Not Released. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE1_NOT_RELEASE,

		ERROR_SEQ_LOAD_TRAY_TABLE2_START = 530,
		[DefaultValueAttribute("<ko>로드 트레이 테이블2에 트레이가 존재하여 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Tray exists in Load Tray Table2. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE2_TRAY_EXIST,
		[DefaultValueAttribute("<ko>로드 트레이 테이블2에 트레이 정보와 센서가 일치하지 않습니다. 확인해주세요.<en>Tray information and sensor mismatch in load tray table 2. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE2_TRAY_MISMATCH,
		[DefaultValueAttribute("<ko>로드 트레이 테이블2에 트레이가 존재하지 않아 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Tray Not exists in Load Tray Table2. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE2_TRAY_NONE,
		[DefaultValueAttribute("<ko>로드 트레이 테이블2가 아래쪽에 있는 상태라 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Load Tray Table2 is at the bottom. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE2_UP_POSITION,
		[DefaultValueAttribute("<ko>로드 트레이 테이블2에 PAD실린더가 전진상태가 아닙니다. 확인해주세요.<en>Load Tray Table2 Pad Cylinder is Not in Forward State. Please check it. ")]
		ERROR_SEQ_LOAD_TRAY_TABLE2_PAD_NOT_FWD,
		[DefaultValueAttribute("<ko>로드 트레이 테이블2의 그립이 해제되어 있지 않습니다.확인해주세요<en>Load Tray Table1 is Not Released. Please check it.")]
		ERROR_SEQ_LOAD_TRAY_TABLE2_NOT_RELEASE,
		#endregion

		#region - SEQ Load Transfer
		ERROR_SEQ_LOAD_TRANSFER_START = 540,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 트레이가 감지됩니다.<en>The tray is detected in the Load transfer.")]
		ERROR_SEQ_LOAD_TRANSFER_TRAY_EXIST,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 트레이가 감지되지 않습니다.<en>The tray is Undetected in the Load transfer.")]
		ERROR_SEQ_LOAD_TRANSFER_TRAY_NONE,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 트레이 리밋센서를 확인해 주세요.<en>Please check the tray limit sensor in Load transfer.")]
		ERROR_SEQ_LOAD_TRANSFER_TRAY_LIMIT_CHECK,
		[DefaultValueAttribute("<ko>로드 트랜스퍼 PICK, PLACE 위치를 확인해 주세요.(개발팀 로그 송부)<en>Please check the location of Pick and Place of Load transfer.")]
		ERROR_SEQ_LOAD_TRANSFER_PNP_POSITION,

		#endregion
		

		#region - SEQ Tray Cleaner
		ERROR_SEQ_TRAY_CLEAN_START = 550,
		[DefaultValueAttribute("<ko>트레이 클린에 트레이가 이미 존재합니다.<en>The tray already exists in the Tray Clean.")]
		ERROR_SEQ_TRAY_CLEAN_TRAY_EXIST,
		[DefaultValueAttribute("<ko>트레이 클린에 트레이가 감지되지않습니다.<en>There's no tray in the Tray Clean.")]
		ERROR_SEQ_TRAY_CLEAN_TRAY_NONE,
		[DefaultValueAttribute("<ko>트레이 클리너에 메모리 데이터와 센서 감지가 일치하지 않았습니다.<en>Memory Data and Sensor Detection Did Not Match in the Tray Cleaner.")]
		ERROR_SEQ_TRAY_CLEAN_TRAY_MISMATCH,
		[DefaultValueAttribute("<ko>트레이 클리너의 이오나이저에서 알람이 발생했습니다.<en>An alarm has occurred in the ionizer of the tray cleaner.")]
		ERROR_SEQ_TRAY_CLEAN_IONIZER_RIGHT_ALARM,
		[DefaultValueAttribute("<ko>트레이 클리너 공압 확인해주세요.<en>Please check the Tray Cleaner Air ")]
		ERROR_SEQ_TRAY_CLEAN_CLEANER_AIR,
		[DefaultValueAttribute("<ko>트레이 클리너 필터를 확인해주세요.<en>Please check the Tray Cleaner filter.")]
		ERROR_SEQ_TRAY_CLEAN_CLEANER_FILTER,
		#endregion

		#region - SEQ LOADPNP
		ERROR_SEQ_LOAD_PnP_START = 560,
		[DefaultValueAttribute("<ko>Load PnP에 메모리가 감지되었습니다.<en>Memory detected in Load PnP.")]
		ERROR_SEQ_LOAD_PnP_PROD_EXIST,
		[DefaultValueAttribute("<ko>Load PnP에 메모리가 감지되지 않았습니다.<en>Memory Undetected in Load PnP.")]
		ERROR_SEQ_LOAD_PnP_PROD_NONE,
		[DefaultValueAttribute("<ko>Load PnP에 메모리 데이터와 센서 감지가 일치하지 않았습니다.<en>Memory Data and Sensor Detection Did Not Match in Load PnP.")]
		ERROR_SEQ_LOAD_PnP_PROD_MISMATCH,
		[DefaultValueAttribute("<ko>로드된 트레이가 존재하지 않습니다. 확인해주세요<en>Loaded tray does not exist. Please check it.")]
		ERROR_SEQ_LOAD_PnP_TRAY_NONE,
		[DefaultValueAttribute("<ko>Load PnP에서 그립완료 시간을 초과 하였습니다.<en>Load PnP grip timeout occurred.")]
		ERROR_SEQ_LOAD_PnP_GRIP_TIME_OUT,
		[DefaultValueAttribute("<ko>1번 트레이의 1줄에 제품이 존재 합니다. 확인해주세요.<en>Product Exist in row1 of Empty tray1 . Please check it.")]
		ERROR_SEQ_LOAD_PnP_PRODUCT_EXIST_IN_EMPTY_TRAY1_ROW1,
		[DefaultValueAttribute("<ko>1번 트레이의 2줄에 제품이 존재 합니다. 확인해주세요.<en>Product Exist in row2 of Empty tray1 . Please check it.")]
		ERROR_SEQ_LOAD_PnP_PRODUCT_EXIST_IN_EMPTY_TRAY1_ROW2,
		[DefaultValueAttribute("<ko>1번 트레이의 3줄에 제품이 존재 합니다. 확인해주세요.<en>Product Exist in row3 of Empty tray1 . Please check it.")]
		ERROR_SEQ_LOAD_PnP_PRODUCT_EXIST_IN_EMPTY_TRAY1_ROW3,
		[DefaultValueAttribute("<ko>2번 트레이의 1줄에 제품이 존재 합니다. 확인해주세요.<en>Product Exist in row1 of Empty tray2 . Please check it.")]
		ERROR_SEQ_LOAD_PnP_PRODUCT_EXIST_IN_EMPTY_TRAY2_ROW1,
		[DefaultValueAttribute("<ko>2번 트레이의 2줄에 제품이 존재 합니다. 확인해주세요.<en>Product Exist in row2 of Empty tray2 . Please check it.")]
		ERROR_SEQ_LOAD_PnP_PRODUCT_EXIST_IN_EMPTY_TRAY2_ROW2,
		[DefaultValueAttribute("<ko>2번 트레이의 3줄에 제품이 존재 합니다. 확인해주세요.<en>Product Exist in row3 of Empty tray2 . Please check it.")]
		ERROR_SEQ_LOAD_PnP_PRODUCT_EXIST_IN_EMPTY_TRAY2_ROW3,
		[DefaultValueAttribute("<ko>로드 PnP 공압 확인해주세요.<en>Please check the Load PnP Air ")]
		ERROR_SEQ_LOAD_PnP_AIR,
		[DefaultValueAttribute("<ko>제품이 존재 합니다. 첫번째 트레이는 비어있어야 합니다. 확인해주세요.<en>There is a product . The first tray must be empty. Please check.")]
		ERROR_SEQ_FIRST_TRAY_NEED_EMPTY,



		#endregion

		#region - SEQ Load Buffer
		ERROR_SEQ_LOAD_BUFFER1_START = 580,
		[DefaultValueAttribute("<ko>로드 버퍼1에 제품이 메모리와 일치하지 않습니다. 센서를 확인해 주세요.<en>The product Mismatch in load buffer1. Please check the sensor.")]
		ERROR_SEQ_LOAD_BUFFER1_PROD_MISMATCH,
		[DefaultValueAttribute("<ko>로드 버퍼1에 제품이 존재하지 않습니다. 센서를 확인해 주세요.<en>The product Not exists in load buffer1. Please check the sensor.")]
		ERROR_SEQ_LOAD_BUFFER1_PROD_NONE,
		[DefaultValueAttribute("<ko>로드 버퍼1에 제품이 존재합니다. 센서를 확인해 주세요.<en>The product exists in load buffer1. Please check the sensor.")]
		ERROR_SEQ_LOAD_BUFFER1_PROD_EXIST,
		[DefaultValueAttribute("<ko>로드버퍼 #1 공압 확인해주세요.<en>LOAD BUFFER #1 AIR CHECK")]
		ERROR_SEQ_LOAD_BUFFER1_AIR,
		[DefaultValueAttribute("<ko>로드버퍼 #1 클리너 필터를 확인해주세요.<en>Please check the Load Buffer #1 cleaner filter.")]
		ERROR_SEQ_LOAD_BUFFER1_CLEANER_FILTER,
		[DefaultValueAttribute("<ko>로드버퍼 #1 이오나이저를 확인해주세요.<en>Please check the Load Buffer #1 Ionizer.")]
		ERROR_SEQ_LOAD_BUFFER1_IONIZER,

		ERROR_SEQ_LOAD_BUFFER2_START = 590,
		[DefaultValueAttribute("<ko>로드 버퍼2에 제품이 메모리와 일치하지 않습니다. 센서를 확인해 주세요.<en>The product Mismatch in load buffer2. Please check the sensor.")]
		ERROR_SEQ_LOAD_BUFFER2_PROD_MISMATCH,
		[DefaultValueAttribute("<ko>로드 버퍼2에 제품이 존재하지 않습니다. 센서를 확인해 주세요.<en>The product Not exists in load buffer2. Please check the sensor.")]
		ERROR_SEQ_LOAD_BUFFER2_PROD_NONE,
		[DefaultValueAttribute("<ko>로드 버퍼2에 제품이 존재합니다. 센서를 확인해 주세요.<en>The product exists in load buffer2. Please check the sensor.")]
		ERROR_SEQ_LOAD_BUFFER2_PROD_EXIST,
		[DefaultValueAttribute("<ko>로드버퍼 #2 공압 확인해주세요.<en>LOAD BUFFER #2 AIR CHECK")]
		ERROR_SEQ_LOAD_BUFFER2_AIR,
		[DefaultValueAttribute("<ko>로드버퍼 #2 클리너 필터를 확인해주세요.<en>Please check the Load Buffer #2 cleaner filter.")]
		ERROR_SEQ_LOAD_BUFFER2_CLEANER_FILTER,
		[DefaultValueAttribute("<ko>로드버퍼 #2 이오나이저를 확인해주세요.<en>Please check the Load Buffer #2 Ionizer.")]
		ERROR_SEQ_LOAD_BUFFER2_IONIZER,

		#endregion

		#region - SEQ Load Insp Pnp
		ERROR_SEQ_LOADINSP_PnP_START = 600,
		[DefaultValueAttribute("<ko>Load Inspector PnP에 메모리가 감지되었습니다.<en>Memory detected in Load Inspector.")]
		ERROR_SEQ_LOADINSP_PnP_PROD_EXIST,
		[DefaultValueAttribute("<ko>Load Inspector PnP에 메모리가 감지되지 않았습니다.<en>Memory Undetected in Load Inspector.")]
		ERROR_SEQ_LOADINSP_PnP_PROD_NONE,
		[DefaultValueAttribute("<ko>Load Inspector PnP에 메모리 데이터와 센서 감지가 일치하지 않았습니다.<en>Memory Data and Sensor Detection Did Not Match in Load Inspector.")]
		ERROR_SEQ_LOADINSP_PnP_PROD_MISMATCH,
		[DefaultValueAttribute("<ko>로드 검사 PnP 공압 확인해주세요.<en>Please check the Load Insp PnP Air ")]
		ERROR_SEQ_LOAD_INSP_PnP_AIR,
		#endregion

		#region - SEQ Inspection Plate
		ERROR_SEQ_INSP_PLATE_START = 610,
		[DefaultValueAttribute("<ko>Inspection Plate #1에 메모리가 감지되었습니다.<en>Memory detected on Inspection Plate #1.")]
		ERROR_SEQ_INSP_PLATE1_RAM_EXIST,
		[DefaultValueAttribute("<ko>Inspection Plate #2에 메모리가 감지되었습니다.<en>Memory detected on Inspection Plate #2.")]
		ERROR_SEQ_INSP_PLATE2_RAM_EXIST,
		[DefaultValueAttribute("<ko>Inspection Plate #1에 메모리가 감지되지 않았습니다.<en>Memory Undetected on Inspection Plate #1.")]
		ERROR_SEQ_INSP_PLATE1_RAM_NONE,
		[DefaultValueAttribute("<ko>Inspection Plate #2에 메모리가 감지되지 않았습니다.<en>Memory Undetected on Inspection Plate #2.")]
		ERROR_SEQ_INSP_PLATE2_RAM_NONE,
		[DefaultValueAttribute("<ko>Inspection Plate #1에 메모리 데이터와 센서 감지가 일치하지 않았습니다.<en>The memory data and sensor detection did not match on Inspection Plate #1.")]
		ERROR_SEQ_INSP_PLATE1_RAM_MISMATCH,
		[DefaultValueAttribute("<ko>Inspection Plate #2에 메모리 데이터와 센서 감지가 일치하지 않았습니다.<en>The memory data and sensor detection did not match on Inspection Plate #2.")]
		ERROR_SEQ_INSP_PLATE2_RAM_MISMATCH,
		[DefaultValueAttribute("<ko>상부 검사 PC가 자동 운전 상태가 아닙니다.<en>The Top inspection PC is not on AutoRun for inspection.")]
		ERROR_SEQ_INSP_TOP_PC_NOT_AUTORUN,
		[DefaultValueAttribute("<ko>하부 검사 PC가 자동 운전 상태가 아닙니다.<en>The Bottom inspection PC is not on AutoRun for inspection.")]
		ERROR_SEQ_INSP_BTM_PC_NOT_AUTORUN,
		[DefaultValueAttribute("<ko>상부 검사 PC의 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Top inspection PC has been exceeded.(Timeout)")]
		ERROR_SEQ_INSP_TOP_PC_COMM_TIMEOUT,
		[DefaultValueAttribute("<ko>상부 검사 PC의 자동 운전 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Top inspection PC has been exceeded.(Auto Run Comm Timeout)")]
		ERROR_SEQ_INSP_TOP_PC_AUTORUN_COMM_TIMEOUT,
		[DefaultValueAttribute("<ko>상부 검사 PC의 검사 상태 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Top inspection PC has been exceeded.(Insp State Comm Timeout)")]
		ERROR_SEQ_INSP_TOP_PC_INSPSTATE_COMM_TIMEOUT,
		[DefaultValueAttribute("<ko>상부 검사 PC의 상태 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Top inspection PC has been exceeded.(Run State Comm Timeout)")]
		ERROR_SEQ_INSP_TOP_PC_RUNSTATE_COMM_TIMEOUT,
		[DefaultValueAttribute("<ko>상부 검사 PC의 확인 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Top inspection PC has been exceeded.(Confirm Timeout)")]
		ERROR_SEQ_INSP_TOP_PC_CONFIRM_COMM_TIMEOUT,

		[DefaultValueAttribute("<ko>하부 검사 PC의 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Bottom inspection PC has been exceeded.(Timeout)")]
		ERROR_SEQ_INSP_BTM_PC_COMM_TIMEOUT,
		[DefaultValueAttribute("<ko>하부 검사 PC의 자동 운전 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Bottom inspection PC has been exceeded.(Auto Run Comm Timeout)")]
		ERROR_SEQ_INSP_BTM_PC_AUTORUN_COMM_TIMEOUT,
		[DefaultValueAttribute("<ko>하부 검사 PC의 검사 상태 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Bottom inspection PC has been exceeded.(Insp State Comm Timeout)")]
		ERROR_SEQ_INSP_BTM_PC_INSPSTATE_COMM_TIMEOUT,
		[DefaultValueAttribute("<ko>하부 검사 PC의 상태 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Bottom inspection PC has been exceeded.(Run State Comm Timeout)")]
		ERROR_SEQ_INSP_BTM_PC_RUNSTATE_COMM_TIMEOUT,
		[DefaultValueAttribute("<ko>하부 검사 PC의 확인 응답 시간이 초과 되었습니다.(Timeout)<en>The response time of the Bottom inspection PC has been exceeded.(Confirm Timeout)")]
		ERROR_SEQ_INSP_BTM_PC_CONFIRM_COMM_TIMEOUT,

		[DefaultValueAttribute("<ko>상부 검사 PC가 확인 오류를 보내왔습니다.<en>The Top inspection PC sent a confirmation error.")]
		ERROR_SEQ_INSP_TOP_PC_CONFIRM_NG,
		[DefaultValueAttribute("<ko>하부 검사 PC가 확인 오류를 보내왔습니다.<en>The Bottom inspection PC sent a confirmation error.")]
		ERROR_SEQ_INSP_BTM_PC_CONFIRM_NG,
		[DefaultValueAttribute("<ko>검사 시간이 초과 되었습니다.(Timeout)<en>The inspection time has been exceeded.(Timeout)")]
		ERROR_SEQ_INSP_INSPECTION_TIMEOUT,
		[DefaultValueAttribute("<ko>상부 검사 PC의 검사결과를 받지 못했습니다. 상부 PC를 확인해주세요.(Timeout)<en>The result from the Top Inspection was not received. Please check the Top PC.(Timeout)")]
		ERROR_SEQ_INSP_TOP_PC_ISNPECTION_TIMEOUT,
		[DefaultValueAttribute("<ko>하부 검사 PC의 검사결과를 받지 못했습니다. 상부 PC를 확인해주세요.(Timeout)<en>The result from the Bottom Inspection was not received. Please check the Top PC.(Timeout)")]
		ERROR_SEQ_INSP_BTM_PC_ISNPECTION_TIMEOUT,
		[DefaultValueAttribute("<ko>상부와 하부의 그룹명이 다릅니다.<en>The group names for the top and bottom are different.")]
		ERROR_SEQ_INSP_TOP_BTM_PC_GROUP_NAME_DIFFERENT,

		[DefaultValueAttribute("<ko>상부 검사 PC의 이동이 실패했습니다. 상부 검사 PC를 확인해주세요.<en>Failed to move the Top inspection PC. Please check the Top inspection PC")]
		ERROR_SEQ_INSP_TOP_PC_MOVE_FAIL,
		[DefaultValueAttribute("<ko>하부 검사 PC의 이동이 실패했습니다. 하부 검사 PC를 확인해주세요.<en>Failed to move the Bottom inspection PC. Please check the Bottom inspection PC.")]
		ERROR_SEQ_INSP_BTM_PC_MOVE_FAIL,
		#endregion

		#region - SEQ Unloader
		ERROR_SEQ_UNLOADER_LIFT_START = 640,
		[DefaultValueAttribute("<ko>언로더 리프트의 언로드 준비상태를 확인 해 주세요.<en>Please check the status of the unloader lift.")]
		ERROR_SEQ_UNLOADER_LIFT_UNLOAD_ENABLE_CHECK,
		[DefaultValueAttribute("<ko>언로더 리프트의 에어리어 센서1가 감지되었습니다.<en>Area sensor 1 on the unloader lift detected.")]
		ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR1_CHECK,
		[DefaultValueAttribute("<ko>언로더 리프트의 에어리어 센서2가 감지되었습니다.<en>Area sensor 2 on the unloader lift detected.")]
		ERROR_SEQ_UNLOADER_LIFT_AREA_SENSOR2_CHECK,
		[DefaultValueAttribute("<ko>언로더 리프트의 트레이 감지 센서가 감지되었습니다.<en>Tray detection sensor on unloader lift detected.")]
		ERROR_SEQ_UNLOADER_LIFT_EXIST_SENSOR_CHECK,
		[DefaultValueAttribute("<ko>언로더 리프트에 트레이가 없습니다. 트레이가 컨베이어 중간에 있는 경우 READY 동작 후 실행해주세요.<en>There are no trays in the unloader lift.If the tray is in the middle of the conveyor, run after the READY action.")]
		ERROR_SEQ_UNLOADER_LIFT_TRAY_NONE,
		[DefaultValueAttribute("<ko>언로더 리프트에 트레이가 있습니다.<en>There is a tray on the unloader lift.")]
		ERROR_SEQ_UNLOADER_LIFT_TRAY_EXIST,
		[DefaultValueAttribute("<ko>언로드 리프트의 트레이 최대 갯수를 초과하였습니다.<en>The maximum number of trays on the unload lift has been exceeded.")]
		ERROR_SEQ_UNLOADER_LIFT_LIMIT_SENSOR_CHECK,
		[DefaultValueAttribute("<ko>언로더 리프트의 모터 위치를 확인 해 주세요.<en>Please check the motor position of the unloader lift.")]
		ERROR_SEQ_UNLOADER_LIFT_MOTOR_POSITION_CHECK,

		ERROR_SEQ_UNLOADER_CONV_START = 650,
		[DefaultValueAttribute("<ko>언로더 컨베이어의 트레이 감지 센서가 감지되었습니다.<en>Tray detection sensor on unloader conveyor detected.")]
		ERROR_SEQ_UNLOADER_CONV_EXIST_SENSOR_CHECK,
		[DefaultValueAttribute("<ko>언로더 컨베이어에 트레이가 없습니다.<en>There are no trays in the unloader conveyor.")]
		ERROR_SEQ_UNLOADER_CONV_TRAY_NONE,
		[DefaultValueAttribute("<ko>언로더 컨베이어에 트레이가 있습니다.<en>There is a tray on the unloader conveyor.")]
		ERROR_SEQ_UNLOADER_CONV_TRAY_EXIST,
		[DefaultValueAttribute("<ko>언로더 컨베이어에 트레이가 가득 찼습니다.<en>The unloader conveyor is full of trays.")]
		ERROR_SEQ_UNLOADER_CONV_TRAY_FULL,
		#endregion

		#region - SEQ Unload Transfer

		ERROR_SEQ_UNLOAD_TRANSFER_START = 660,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼에서 트레이가 감지됩니다.<en>The tray is detected in the unload transfer.")]
		ERROR_SEQ_UNLOAD_TRANSFER_TRAY_EXIST,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼에서 트레이가 감지되지않습니다.<en>The tray is Undetected in the unload transfer.")]
		ERROR_SEQ_UNLOAD_TRANSFER_TRAY_NONE,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼에서 리드가 감지되지않습니다.<en>The lid is Undetected in the unload transfer.")]
		ERROR_SEQ_UNLOAD_TRANSFER_LID_NONE,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼의 트레이 리밋센서를 확인해 주세요.<en>Please check the tray limit sensor in unload transfer.")]
		ERROR_SEQ_UNLOAD_TRANSFER_TRAY_LIMIT_CHECK,
		[DefaultValueAttribute("<ko>언로드 트랜스퍼의 PICK, PLACE 위치를 확인해 주세요.(개발팀 로그 송부)<en>Please check the location of Pick and Place of unload transfer.")]
		ERROR_SEQ_UNLOAD_TRANSFER_PNP_POSITION,
		[DefaultValueAttribute("<ko>PICK 할 위치의 Z축 정보가 없습니다.<en>There is no Z axis information for the location you want to pick.")]
		ERROR_SEQ_UNLOAD_TRANSFER_PICK_Z_UNKNOWN,
		[DefaultValueAttribute("<ko>PLACE 할 위치의 Z축 정보가 없습니다.<en>There is no Z axis information for the location you want to place.")]
		ERROR_SEQ_UNLOAD_TRANSFER_PLACE_Z_UNKNOWN,
		#endregion

		#region - SEQ Unload Tray Table
		ERROR_SEQ_UNLOAD_TRAY_TABLE1_START = 680,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블1에 트레이가 존재하여 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Tray exists in Unload Tray Table1. Please check it.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_EXIST,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블1에 트레이가 존재하지 않아 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Tray Not exists in Unload Tray Table1. Please check it.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE1_TRAY_NONE,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블1가 아래쪽에 있는 상태라 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Unload Tray Table1 is at the bottom. Please check it.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE1_UP_POSITION,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블1에 PAD실린더가 전진상태가 아닙니다. 확인해주세요.<en>Unload Tray Table1 Pad Cylinder is Not in Forward State. Please check it. ")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE1_PAD_NOT_FWD,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블1의 그립이 해제되어 있지 않습니다.확인해주세요<en>Unload Tray Table1 is Not Released. Please check it.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE1_NOT_RELEASE,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블1의 전면 이오나이저에서 알람이 발생했습니다.<en>An alarm has occurred in the front ionizer of the unload tray table #1.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE1_IONIZER_FRONT_ALARM,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블1의 후면 이오나이저에서 알람이 발생했습니다.<en>An alarm has occurred in the rear ionizer of the unload tray table #1.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE1_IONIZER_REAR_ALARM,

		ERROR_SEQ_UNLOAD_TRAY_TABLE2_START = 690,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블2에 트레이가 존재하여 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Tray exists in Unload Tray Table2. Please check it.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE2_TRAY_EXIST,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블2에 트레이가 존재하지 않아 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Tray Not exists in Unload Tray Table2. Please check it.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE2_TRAY_NONE,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블2가 아래쪽에 있는 상태라 시퀀스 진행이 불가능합니다. 확인해주세요.<en>The sequence cannot proceed because the Unload Tray Table2 is at the bottom. Please check it.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE2_UP_POSITION,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블2에 PAD실린더가 전진상태가 아닙니다. 확인해주세요.<en>Unload Tray Table2 Pad Cylinder is Not in Forward State. Please check it. ")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE2_PAD_NOT_FWD,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블2의 그립이 해제되어 있지 않습니다.확인해주세요<en>Unload Tray Table2 is Not Released. Please check it.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE2_NOT_RELEASE,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블2의 전면 이오나이저에서 알람이 발생했습니다.<en>An alarm has occurred in the front ionizer of the unload tray table #2.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE2_IONIZER_FRONT_ALARM,
		[DefaultValueAttribute("<ko>언로드 트레이 테이블2의 후면 이오나이저에서 알람이 발생했습니다.<en>An alarm has occurred in the rear ionizer of the unload tray table #2.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE2_IONIZER_REAR_ALARM,

		[DefaultValueAttribute("<ko>NG 제품 개수가 트레이를 초과하였습니다. <en>The number of NG products exceeded the tray.")]
		ERROR_SEQ_UNLOAD_TRAY_TABLE_NG_COUNT_OVER,
		#endregion
		
		#region - SEQ Unload PnP
		ERROR_SEQ_UNLOAD_PnP_START = 710,
		[DefaultValueAttribute("<ko>Unload PnP에 메모리가 감지되었습니다.<en>Memory detected in Unload PnP .")]
		ERROR_SEQ_UNLOAD_PnP_PROD_EXIST,
		[DefaultValueAttribute("<ko>Unload PnP에 메모리가 감지되지 않았습니다.<en>Memory Undetected in Unload PnP.")]
		ERROR_SEQ_UNLOAD_PnP_PROD_NONE,
		[DefaultValueAttribute("<ko>Unload PnP에 메모리 데이터와 센서 감지가 일치하지 않았습니다.<en>Memory Data and Sensor Detection Did Not Match in Unload PnP.")]
		ERROR_SEQ_UNLOAD_PnP_PROD_MISMATCH,
		[DefaultValueAttribute("<ko>Unload PnP 제품센서가 감지되어있습니다. 남아있는 제품이 있는지 또는 센서를 확인해 주세요<en>REJECT BUFFER product sensor is detected. Please check if there are any remaining products or sensors.")]
		ERROR_SEQ_UNLOAD_PnP_PROD_REJECTBUFFER_PROD_CHECK,
		[DefaultValueAttribute("<ko>리뷰 PC와의 통신이 시간 초과하였습니다.<en>Communication with the review PC has timeout.")]
		ERROR_SEQ_UNLOAD_PnP_COMM_TIMEOUT,
		[DefaultValueAttribute("<ko>리뷰 PC와의 통신 데이터에 오류가 있습니다.<en>There is an error in the communication data with the review PC.")]
		ERROR_SEQ_UNLOAD_PnP_COMM_DATA_WRONG,
		[DefaultValueAttribute("<ko>언로드 PnP 공압 확인해주세요.<en>Please check the Unload PnP Air ")]
		ERROR_SEQ_UNLOAD_PnP_AIR,
		[DefaultValueAttribute("<ko>Unload PnP가 메모리를 제대로 인계하지 못했습니다.<en>Unload PnP unsucess to memory.")]
		ERROR_SEQ_UNLOAD_PnP_PROD_PLACE_NOT_SUCESS,
		#endregion

		#region - SEQ Unload Buffer
		ERROR_SEQ_UNLOAD_BUFFER_START = 720,
		[DefaultValueAttribute("<ko>Unload Buffer에 메모리가 감지되었습니다.<en>Memory detected in Unload Buffer.")]
		ERROR_SEQ_UNLOAD_BUFFER_PROD_EXIST,
		[DefaultValueAttribute("<ko>Unload Buffer에 메모리가 감지되지 않았습니다.<en>Memory Undetected in Unload Buffer.")]
		ERROR_SEQ_UNLOAD_BUFFER_PROD_NONE,
		[DefaultValueAttribute("<ko>Unload Buffer에 메모리 데이터와 센서 감지가 일치하지 않았습니다.<en>Memory Data and Sensor Detection Did Not Match in Unload Buffer.")]
		ERROR_SEQ_UNLOAD_BUFFER_PROD_MISMATCH,
		#endregion

		#region - SEQ Reject Buffer

		ERROR_SEQ_REJECT_BUF_START = 730,
		[DefaultValueAttribute("<ko>Reject Buffer에 빈 슬롯이 없습니다.<en>There are no empty slots in the reject buffer.")]
		ERROR_SEQ_REJECT_BUF_NONE_EMPTY,
		[DefaultValueAttribute("<ko>Reject Buffer에 불량품이 남아 있습니다.<en>The reject buffer has a defective product left.")]
		ERROR_SEQ_REJECT_BUF_EXIST_NG,
		[DefaultValueAttribute("<ko>Reject Buffer에 양품이 없습니다.<en>There is no OK product in Reject Buffer.")]
		ERROR_SEQ_REJECT_BUF_NONE_OK,
		[DefaultValueAttribute("<ko>Reject Buffer에 불량품이 없습니다.<en>There is no NG product in Reject Buffer.")]
		ERROR_SEQ_REJECT_BUF_NONE_NG,
		[DefaultValueAttribute("<ko>Reject Buffer의 이동 시퀀스에서 오류가 발생했습니다.<en>An error occurred in the movement sequence of Reject Buffer.")]
		ERROR_SEQ_REJECT_BUF_MOVE_SEQ_NG,
		[DefaultValueAttribute("<ko>Reject Buffer Row1에 제품이 남아 있습니다.<en>Product remains in Reject Buffer Row1.")]
		ERROR_SEQ_REJECT_BUF_ROW1_EXIST,
		[DefaultValueAttribute("<ko>Reject Buffer Row2에 제품이 남아 있습니다.<en>Product remains in Reject Buffer Row2.")]
		ERROR_SEQ_REJECT_BUF_ROW2_EXIST,
		[DefaultValueAttribute("<ko>Reject Buffer Row3에 제품이 남아 있습니다.<en>Product remains in Reject Buffer Row3.")]
		ERROR_SEQ_REJECT_BUF_ROW3_EXIST,
		[DefaultValueAttribute("<ko>Reject Buffer Row4에 제품이 남아 있습니다.<en>Product remains in Reject Buffer Row4.")]
		ERROR_SEQ_REJECT_BUF_ROW4_EXIST,
		[DefaultValueAttribute("<ko>Reject Buffer의 크기 세팅을 확인 해 주세요..<en>Please Check Reject Buffer's table Size.")]
		ERROR_SEQ_REJECT_BUF_TABLE_NOT_LARGE_SIZE,
		[DefaultValueAttribute("<ko>Reject Buffer의 크기 세팅을 확인 해 주세요.<en>Please Check Reject Buffer's table Size.")]
		ERROR_SEQ_REJECT_BUF_TABLE_NOT_SMALL_SIZE,
		[DefaultValueAttribute("<ko>리젝버퍼에 NG 제품 개수가 트레이 용량을 초과하였습니다. <en>The number of NG products in the reject buffer exceeded the tray capacity.")]
		ERROR_SEQ_REJECT_BUF_NG_COUNT_OVER,
		[DefaultValueAttribute("<ko>리젝트 트레이 테이블 X 그립 실린더 열림 센서를 확인해주세요. <en>Reject Tray Table X Grip Cylinder Open Sensor Check")]
		ERROR_REJECT_TRAY_TABLE_GRIP_X_CYL_OPEN,
		[DefaultValueAttribute("<ko>리젝트 트레이 테이블 Y 그립 실린더 열림 센서를 확인해주세요. <en>Reject Tray Table Y Grip Cylinder Open Sensor Check")]
		ERROR_REJECT_TRAY_TABLE_GRIP_Y_CYL_OPEN,
		[DefaultValueAttribute("<ko>리젝트 트레이 테이블 FLOW를 확인해주세요. <en>Reject Tray Table Flow Manager Error Check")]
		ERROR_REJECT_TRAY_TABLE_FLOWMNG,
		[DefaultValueAttribute("<ko>리젝트 트레이 테이블 트레이 방향을 확인해주세요. <en>Pease check Reject Tray Table - Tray Direction")]
		ERROR_REJECT_TRAY_TABLE_TRAY_DIRECTION,
		[DefaultValueAttribute("<ko>연속 불량이 발생했습니다.검사 Job을 확인해주세요.<en>Continuous NG have occurred. Please check the inspection job.")]
		ERROR_CONTINUOUS_NG,
		[DefaultValueAttribute("<ko>불량이 너무 많이 발생했습니다.검사 Job을 확인해주세요.<en>There have been too many defects. Please check the inspection job.")]
		ERROR_TOO_MANY_NG,
		#endregion

		#region - SEQ Unload Insp Pnp
		ERROR_SEQ_UNLOADINSP_PnP_START = 760,
		[DefaultValueAttribute("<ko>Unload Inspector PnP에 메모리가 감지되었습니다.<en>Memory detected in Unload Inspector.")]
		ERROR_SEQ_UNLOADINSP_PnP_PROD_EXIST,
		[DefaultValueAttribute("<ko>Unload Inspector PnP에 메모리가 감지되지 않았습니다.<en>Memory Undetected in Unload Inspector.")]
		ERROR_SEQ_UNLOADINSP_PnP_PROD_NONE,
		[DefaultValueAttribute("<ko>Unload Inspector PnP에 메모리 데이터와 센서 감지가 일치하지 않았습니다.<en>Memory Data and Sensor Detection Did Not Match in Unload Inspector.")]
		ERROR_SEQ_UNLOADINSP_PnP_PROD_MISMATCH,
		[DefaultValueAttribute("<ko>언로드 검사 PnP 공압 확인해주세요.<en>Please check the Unload Insp PnP Air ")]
		ERROR_SEQ_UNLOAD_INSP_PnP_AIR,
		#endregion

		#region - SEQ Main
		ERROR_SEQ_MAIN_START = 770,
		[DefaultValueAttribute("<ko>Reject Buffer에 있는 제품의 검사 결과를 재 확인 하십시요.<en>Please recheck the inspection results of the product in Reject Buffer.")]
		ERROR_SEQ_MAIN_VERIFY_REQUEST,
		#endregion

		#region - All Home
		ERROR_SEQ_ALLHOME_START = 780,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. It is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 로드 테이블1이 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Load Tray Table1 is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRAY_TABLE1,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 로드 테이블2이 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Load Tray Table2 is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRAY_TABLE2,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 언로드 테이블1이 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Unload Tray Table1 is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRAY_TABLE1,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 언로드 테이블2이 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Unload Tray Table2 is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRAY_TABLE2,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 로드 PnP가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Load PnP is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_PNP,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 언로드 PnP가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Unload PnP is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_PNP,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 로드버퍼1이 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Load Buffer1 is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_BUFFER1,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 로드버퍼2이 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Load Buffer2 is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_BUFFER2,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 트레이 로더가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Load Lift is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_LOADER,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 로드 트랜스퍼가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Load Transfer is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_TRANSFER,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 리드 언커버가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Lid Uncover is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LID_UNCOVER,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 트레이 클린이 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Tray Clean is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_CLEAN,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 리드 버퍼가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Lid Buffer is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LID_BUFFER,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 리드 커버가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Lid Cover is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LID_COVER,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 트레이 언로더가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Unload Lift is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_TRAY_UNLOADER,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 언로드 트랜스퍼가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Unload Transfer is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_TRANSFER,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 로드 INSP PnP가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Load Insp PnP is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_LOAD_INSP_PNP,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 언로드 INSP PNP가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Unload Insp PnP is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_INSP_PNP,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 검사 플레이트1이 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Insp Plate1 is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_INSP_PLATE1,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 검사 플레이트2 이 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Insp Plate2 is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_INSP_PLATE2,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 언로드 버퍼가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Unload Buffer is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_UNLOAD_BUFFER,
		[DefaultValueAttribute("<ko>전체 원점을 진행할 수 없는 상태 입니다. 리젝트 버퍼가 동작 중이거나 오류가 존재합니다.<en>It is not possible to proceed with the All Home. Reject Buffer is operating or there is an error.")]
		ERROR_SEQ_ALLHOME_CANT_ALL_HOME_REJECT_BUFFER,
		[DefaultValueAttribute("<ko>전체 원점을 진행하십시오.<en> Please proceed to the All Home")]
		ERROR_SEQ_ALLHOME,
		[DefaultValueAttribute("<ko>전체 원점의 Timeout이 발생하였습니다.<en>All Home Timeout has occurred.")]
		ERROR_SEQ_ALLHOME_TIMEOUT,
		#endregion

		#region - SEQ Interlock

		ERROR_SEQ_INTERLOCK_START = 820,
		[DefaultValueAttribute("<ko>Tray Transfer의 충돌 방지 센서가 감지 되었습니다.<en>The anti-collision sensor of the Tray Transfer was detected.")]
		ERROR_SEQ_INTERLOCK_TRANSFER_COLLISION_SENSOR,
		[DefaultValueAttribute("<ko>Load PnP와 Unload PnP의 충돌이 감지되었습니다.<en>A conflict between Load PnP and Unload PnP has been detected.")]
		ERROR_SEQ_INTERLOCK_LOAD_PnP_COLLISION_SENSOR,
		[DefaultValueAttribute("<ko>Load Inspection PnP와 Unload Inspection PnP의 충돌이 감지되었습니다.<en>A conflict between Load Inspection PnP and Unload Inspection PnP has been detected.")]
		ERROR_SEQ_INTERLOCK_LOADINSP_PnP_COLLISION_SENSOR,

		#endregion
		#region System Error

		ERROR_SYSYEM_START = 830,
		[DefaultValueAttribute("<ko>집진기에서 알람이 발생하였습니다.<en>The dust collector has triggered an alarm.")]
		ERROR_DUST_CONTROL_ALARM,

		#endregion

		#region FlowMng
		ERROR_FLOW_MANAGER_START = 840,
		[DefaultValueAttribute("<ko>플로우 매니저 정보를 확인해 주세요.(개발팀 로그 송부)<en>Please check the flow manager information.")]
		ERROR_FLOW_MANAGER_CHECK,

		#endregion

		#region COMM 
		ERROR_COMM_START = 850,
		[DefaultValueAttribute("<ko>검사 PC와의 통신 데이터에 데이터 오류가 있습니다.<en>There is a data error in the communication data with the inspection PC.")]
		ERROR_COMM_INSP_DATA,
		[DefaultValueAttribute("<ko>상부 검사 PC와의 통신이 끊겼습니다.<en>Communication with the Top inspection PC was cut off.")]
		ERROR_COMM_TOP_INSP_CONNECT,
		[DefaultValueAttribute("<ko>하부 검사 PC와의 통신이 끊겼습니다.<en>Communication with the Bottom inspection PC was cut off.")]
		ERROR_COMM_BOTTOM_INSP_CONNECT,
		[DefaultValueAttribute("<ko>리뷰 PC와의 통신이 끊겼습니다.<en>Communication with the Review PC was cut off.")]
		ERROR_COMM_REVIEW_CONNECT,
		[DefaultValueAttribute("<ko>검사 PC에서 알람이 발생했습니다.<en>An alarm has occurred on the Inspection PC.")]
		ERROR_COMM_INSP_ALARM,
		[DefaultValueAttribute("<ko>상부 검사 PC에 해당 하는 잡이 존재 하지 않습니다.잡을 확인해주세요.<en>The job of the TOP inspection PC does not exist. Please check the job.")]
		ERROR_COMM_TOP_JOB_EXIST,
		[DefaultValueAttribute("<ko>하부 검사 PC에 해당 하는 잡이 존재 하지 않습니다.잡을 확인해주세요.<en>The job of the BOTTOM inspection PC does not exist. Please check the job.")]
		ERROR_COMM_BOTTOM_JOB_EXIST,
		[DefaultValueAttribute("<ko>언로드 버퍼에 상부 검사 결과를 받지 못했습니다.<en>Unload Buffer did not recive result data from Top Inspection PC.")]
		ERROR_COMM_UNLOAD_BUFFER_DONT_RECIVE_INSP_RESULT_TOP,
		[DefaultValueAttribute("<ko>언로드 버퍼에 하부 검사 결과를 받지 못했습니다.<en>Unload Buffer did not recive result data from Bottom Inspection PC.")]
		ERROR_COMM_UNLOAD_BUFFER_DONT_RECIVE_INSP_RESULT_BTM,
		[DefaultValueAttribute("<ko>상부 검사 프로그램이 JOB 업데이트에 실패했습니다. <en>The top inspection program failed to update the JOB.")]
		ERROR_COMM_TOP_JOB_UPDATE_FAIL,
		[DefaultValueAttribute("<ko>하부 검사 프로그램이 JOB 업데이트에 실패했습니다. <en>The bottom inspection program failed to update the JOB.")]
		ERROR_COMM_BOTTOM_JOB_UPDATE_FAIL,
		
		[DefaultValueAttribute("<ko>FTV와의 통신 데이터에 데이터 오류가 있습니다.<en>There is a data error in the communication data with FTV.<ja>FTVとの通信データにデータエラーがあります。")]
        ERROR_COMM_FTV_DATA,
		[DefaultValueAttribute("<ko>FTV에 에러가 발생했습니다. 확인해주세요.<en>An error occurred in FTV. Please check.<ja>FTVでエラーが発生しました。確認してください。")]
		ERROR_FTV_ERROR_CHECK,
		[DefaultValueAttribute("<ko>FTV에 워닝 알람이 발생했습니다. 확인해주세요.<en>A warning alarm has occurred on FTV. Please check.<ja>FTVで警告アラームが発生しました。確認してください。")]
		ERROR_FTV_WARNING_CHECK,
		[DefaultValueAttribute("<ko>FTV가 검사 가능한 상태가 아닙니다. FTV 프로그램 실행 후 확인해주세요.<en>FTV is not in an inspectable state. Please run the FTV program and check again.<ja>FTVは検査可能な状態ではありません。FTVプログラムを実行してから再確認してください。")]
		ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK,
		[DefaultValueAttribute("<ko>FTV 검사 상태를 확인 중 시간 초과가 발생했습니다.<en>Timeout occurred while checking FTV inspection status.<ja>FTV検査状態の確認中にタイムアウトが発生しました。")]
		ERROR_MAIN_SEQ_FTV_INSP_FTV_STATE_CHECK_TIME_OUT,
		[DefaultValueAttribute("<ko>FTV 검사 결과가 없습니다.<en>There are no FTV inspection results.<ja>FTV検査結果がありません。")]
		ERROR_MAIN_SEQ_FTV_INSP_RESULT_CHECK,
		[DefaultValueAttribute("<ko>FTV와 통신 연결을 확인해주세요.<en>Please check your communication connection with FTV.<ja>FTVとの通信接続を確認してください。")]
		ERROR_MAIN_SEQ_FTV_INSP_CONNECT_CHECK,
		[DefaultValueAttribute("<ko>FTV의 레시피가 일치하지 않습니다.<en>The recipe on FTV does not match.<ja>FTVのレシピが一致しません。")]
		ERROR_MAIN_SEQ_FTV_INSP_RECIPE_CHECK,
		[DefaultValueAttribute("<ko>FTV 검사 중 시간 초과가 발생했습니다.<en>Timeout occurred while FTV inspection <ja>FTV検査状態の確認中にタイムアウトが発生しました。")]
		ERROR_MAIN_SEQ_FTV_INSP_FTV_TIME_OUT,
		#endregion


		#region - ETC : 기타 추가

		ERROR_ETC_START = 880,
		[DefaultValueAttribute("<ko>마지막 자재 진행 중에 디버깅 모드가 켜져있습니다. 해당 옵션을 꺼주세요. <en>Debugging mode is on during the last Product Inspection. Please turn off that option.")]
		ERROR_DEBUG_MODE_IN_LAST_PRODUCT,
		[DefaultValueAttribute("<ko>PVI Data 저장 오류가 발생하였습니다.<en>PVI Data save error occurred.")]
		ERROR_PVI_DATA_WRITE,
		[DefaultValueAttribute("<ko>입력한 조건과 일치하는 레시피를 찾을 수 없습니다.<en>No recipes were found that match the conditions you entered.")]
		ERROR_FIND_RECIPE,
		[DefaultValueAttribute("<ko>레시피 변경에 실패했습니다. 로그를 확인해주세요.<en>Recipe change failed. Please check the log.")]
		ERROR_CHANGE_RECIPE,
		[DefaultValueAttribute("<ko>피두셜을 반복적으로 찾지 못하고 있습니다. 피두셜 티칭을 확인해주세요.<en>Fiducial could not be detected repeatedly. Please check fiducial teachings.")]
		ERROR_CAN_NOT_FIND_FIDUCIAL_REPEATLY,

		[DefaultValueAttribute("<ko>Unload PnP의 PICKER 1번 제품 적재에 실패했습니다.해당 위치의 제품을 확인해주세요.<en>Failed to place the product on PICKER 1 of the Unload PnP. Please check the product at that location.")]
		ERROR_SEQ_UNLOAD_PnP_PICKER1_PROD_PLACE_FAIL,
		[DefaultValueAttribute("<ko>Unload PnP의 PICKER 2번 제품 적재에 실패했습니다.해당 위치의 제품을 확인해주세요.<en>Failed to place the product on PICKER 2 of the Unload PnP. Please check the product at that location.")]
		ERROR_SEQ_UNLOAD_PnP_PICKER2_PROD_PLACE_FAIL,
		[DefaultValueAttribute("<ko>Unload PnP의 PICKER 3번 제품 적재에 실패했습니다.해당 위치의 제품을 확인해주세요.<en>Failed to place the product on PICKER 3 of the Unload PnP. Please check the product at that location.")]
		ERROR_SEQ_UNLOAD_PnP_PICKER3_PROD_PLACE_FAIL,
		[DefaultValueAttribute("<ko>Unload PnP의 PICKER 4번 제품 적재에 실패했습니다.해당 위치의 제품을 확인해주세요.<en>Failed to place the product on PICKER 4 of the Unload PnP. Please check the product at that location.")]
		ERROR_SEQ_UNLOAD_PnP_PICKER4_PROD_PLACE_FAIL,

		[DefaultValueAttribute("<ko>로더 트랜스퍼의 트레이 바코드 리더기와의 연결을 실패했습니다.<en>Connection with Tray barcode reader in Loader Transfer failed.")]
		ERROR_SEQ_LOAD_TRANSFER_TRAY_BARCODE_READER_CONNECT_FAIL,
		[DefaultValueAttribute("<ko>로더 트랜스퍼의 트레이 바코드 읽기를 실패했습니다.<en>Fail to Read Tray Barcode in Loader Transfer. Please Check Tray Barcode")]
		ERROR_SEQ_LOAD_TRANSFER_TRAY_BARCODE_READ_FAIL,
		[DefaultValueAttribute("<ko>로더 트랜스퍼가 읽은 트레이 바코드가 정규식 매치에 실패했습니다.<en>The Tray barcode value read by the load transfer failed the regex match")]
		ERROR_SEQ_LOAD_TRANSFER_TRAY_BARCODE_REGEX_MATCH_FAIL,

		[DefaultValueAttribute("<ko>언로더 트랜스퍼의 트레이 바코드 리더기와의 연결을 실패했습니다.<en>Connection with Tray barcode reader in Unload Transfer failed.")]
		ERROR_SEQ_UNLOAD_TRANSFER_BARCODE_READER_CONNECT_FAIL,
		[DefaultValueAttribute("<ko>언로더 트랜스퍼의 트레이 바코드 읽기를 실패했습니다.<en>Fail to Read Tray Barcode in Unload Transfer. Please Check Tray Barcode")]
		ERROR_SEQ_UNLOAD_TRANSFER_BARCODE_READ_FAIL,
		[DefaultValueAttribute("<ko>언로더 트랜스퍼의 바코드가 저장된 DATA와 다릅니다.확인해 주세요.<en>The barcode of the Unload transfer does not match the stored data. Please verify")]
		ERROR_SEQ_UNLOAD_TRANSFER_BARCODE_MISMATCH,
		[DefaultValueAttribute("<ko>언로더 트랜스퍼가 읽은 트레이 바코드가 정규식 매치에 실패했습니다.<en>The Tray barcode value read by the Unload transfer failed the regex match")]
		ERROR_SEQ_UNLOAD_TRANSFER_BARCODE_REGEX_MATCH_FAIL,
		
		[DefaultValueAttribute("<ko>리젝 버퍼1의 바코드 리더기와의 연결을 실패했습니다.<en>Connection with barcode reader in Reject Buffer1 failed.")]
		ERROR_SEQ_REJECT_BUFFER1_BARCODE_READER_CONNECT_FAIL,
		[DefaultValueAttribute("<ko>리젝 버퍼1의 바코드 읽기를 실패했습니다.<en>Fail to Read Barcode in Reject Buffer1. Please Check Reject Buffer1 Tray")]
		ERROR_SEQ_REJECT_BUFFER1_BARCODE_READ_FAIL,
		[DefaultValueAttribute("<ko>리젝 버퍼1가 읽은 트레이 바코드가 정규식 매치에 실패했습니다.<en>The Tray barcode value read by the Reject Buffer1 failed the regex match")]
		ERROR_SEQ_REJECT_BUFFER1_BARCODE_REGEX_MATCH_FAIL,

		[DefaultValueAttribute("<ko>리젝 버퍼2의 바코드 리더기와의 연결을 실패했습니다.<en>Connection with barcode reader in Reject Buffer2 failed.")]
		ERROR_SEQ_REJECT_BUFFER2_BARCODE_READER_CONNECT_FAIL,
		[DefaultValueAttribute("<ko>리젝 버퍼2의 바코드 읽기를 실패했습니다.<en>Fail to Read Barcode in Reject Buffer2. Please Check Reject Buffer Tray")]
		ERROR_SEQ_REJECT_BUFFER2_BARCODE_READ_FAIL,
		[DefaultValueAttribute("<ko>리젝 버퍼2가 읽은 트레이 바코드가 정규식 매치에 실패했습니다.<en>The Tray barcode value read by the Reject Buffer2 failed the regex match")]
		ERROR_SEQ_REJECT_BUFFER2_BARCODE_REGEX_MATCH_FAIL,
		
		[DefaultValueAttribute("<ko>로드테이블1이 올라갈 수 있는 상태가 아닙니다. 전체 홈 진행 후 다시 시도해주세요.<en>The load table 1 is not in a condition to up. Please perform a All home and try again.")]
		ERROR_SEQ_LOAD_TABLE1_UP_SAFETY,





		#endregion

		#region - Exception Error
		[DefaultValueAttribute("<ko>예외처리 발생 메세지를 확인해 주세요.<en>Please check the exception message.")]
		ERROR_EXCEPTION = 915,
		#endregion
		#region SECS/GEM

		ERROR_SESCGEM_START = 920,
		[DefaultValueAttribute("<ko>SECS/GEM 통신 TIME OUT이 발생 했습니다. 연결 상태를 확인 해 주세요.<en>SECS/GEM communication TIME OUT occurred. Please check the connection status.")]
		ERROR_SESCGEM_TIMEOUT,
		[DefaultValueAttribute("<ko>SECS/GEM 통신 Data에 이상이 있습니다.로그를 확인 해 주세요.<en>There is an error in SECS/GEM communication data. Please check the log.")]
		ERROR_SESCGEM_DATA,
		[DefaultValueAttribute("<ko>SECS/GEM 통신 Data Length에 이상이 있습니다.로그를 확인 해 주세요.<en>There is an error in SECS/GEM communication data Length. Please check the log.")]
		ERROR_SESCGEM_DATA_LENGTH,
		[DefaultValueAttribute("<ko>SECS/GEM 통신 Status에 이상이 있습니다.로그를 확인 해 주세요.<en>There is an error in SECS/GEM communication Status. Please check the log.")]
		ERROR_SESCGEM_STATUS,
		[DefaultValueAttribute("<ko>SECS/GEM 통신 Driver에 이상이 있습니다.로그를 확인 해 주세요.<en>There is an error in SECS/GEM communication Driver Status. Please check the log.")]
		ERROR_SESCGEM_DRIVER,
		[DefaultValueAttribute("<ko>SECS/GEM 통신 연결에 이상이 있습니다.연결 상태를 확인 해 주세요.<en>There is an error in SECS/GEM connection Status. Please check the connection status..")]
		ERROR_SESCGEM_CONNECT,
		[DefaultValueAttribute("<ko>(SECS/GEM) 통신 바코드 정보에 이상이 있습니다. 확인 해 주세요.<en>There is an error in SECS/GEM Barcode Data. Please check the Barcode")]
		ERROR_SESCGEM_BARCODE_DATA,
		[DefaultValueAttribute("<ko>(SECS/GEM) 등록되지 않은 사번입니다. 확인 해 주세요.<en>This User ID is not registered. Please check the User ID")]
		ERROR_SESCGEM_USER_DATA,
		[DefaultValueAttribute("<ko>(SECS/GEM) 이미 등록된 LOT ID입니다. 확인 해 주세요.<en>It is already registered LOT ID. Please check the LOT ID")]
		ERROR_SESCGEM_LOT_DATA,

		[DefaultValueAttribute("<ko>(SECS/GEM) OPEN LID REQEUST 의 ACK이 문제가 있습니다. 확인 해 주세요.<en>There is a problem with the ACK of OPEN LID REQUEST. Please check")]
		ERROR_SESCGEM_OPEN_LID_REQ_ACK_ERROR,
		[DefaultValueAttribute("<ko>(SECS/GEM) INSPECTION RESULT 의 ACK이 문제가 있습니다. 확인 해 주세요.<en>There is a problem with the ACK of INSPECTION RESULT. Please check")]
		ERROR_SESCGEM_INSPECTION_RESULT_ACK_ERROR,
		[DefaultValueAttribute("<ko>(SECS/GEM) TRAY VALIDATION 의 ACK이 문제가 있습니다. 확인 해 주세요.<en>There is a problem with the ACK of VALIDATION. Please check")]
		ERROR_SESCGEM_VALIDATION_ACK_ERROR,
		[DefaultValueAttribute("<ko>(SECS/GEM) PROCESS STATE CHANGE 의 ACK이 문제가 있습니다. 확인 해 주세요.<en>There is a problem with the ACK of PROCESS STATE CHANGE. Please check")]
		ERROR_SESCGEM_PROCESS_STATE_ACK_ERROR,
		[DefaultValueAttribute("<ko>(SECS/GEM) ALARM SET 의 ACK이 문제가 있습니다. 확인 해 주세요.<en>There is a problem with the ACK of ALARM SET. Please check")]
		ERROR_SESCGEM_ALARM_SET_ACK_ERROR,

		#endregion
		#region - Warning 950~
		/// <summary>
		/// 워닝 알람 950 ~
		/// </summary> 
		WARNING_START = 950,
		[DefaultValueAttribute("<ko>Warning Dump File.<en>Warning Dump File.")]
		WARNING_DUMP_FILE,
		[DefaultValueAttribute("<ko>키 락이 매뉴얼 상태입니다.<en>The Key-Lock is in Manual-Mode.")]
		WARNING_KEYLOCK_MANUAL,
		[DefaultValueAttribute("<ko>랏 정보를 입력 해주세요<en>Please enter the lot information.")]
		WARNING_LOT_INFO,
		[DefaultValueAttribute("<ko>장비 점검 상태입니다. 점검을 완료해야 동작할 수 있습니다.<en>Equipment check status. It can be operated only after inspection is completed.")]
		WARNING_CHECKING_EQUIPMENT,
		[DefaultValueAttribute("<ko>Reject Buffer에 있는 제품의 검사 결과를 재 확인 하십시요(Verify).<en>Please recheck the inspection results of the product in Reject Buffer.(Verify)")]
		WARNING_VERIFY_REQUEST,
		[DefaultValueAttribute("<ko>트레이 배출이 완료 되었습니다.<en>Trays has been Unload.")]
		WARNING_TRAY_UNLOAD_END,
		[DefaultValueAttribute("<ko>QUIT LOT의 트레이 배출이 완료 되었습니다.<en>QUIT LOT Trays has been Unload.")]
		WARNING_TRAY_UNLOAD_END_QUIT_LOT,
		[DefaultValueAttribute("<ko>연속 불량이 발생했습니다.검사 Job을 확인해주세요.<en>Continuous NG have occurred. Please check the inspection job.")]
		WARNING_CONTINUOUS_NG,
		[DefaultValueAttribute("<ko>불량이 너무 많이 발생했습니다.검사 Job을 확인해주세요.<en>There have been too many defects. Please check the inspection job.")]
		WARNING_TOO_MANY_NG,
		[DefaultValueAttribute("<ko>리젝트 트레이를 교체해주세요. <en>Please replace the reject tray.")]
		WARNING_REJECT_TRAY_REPLACE,
		[DefaultValueAttribute("<ko>(SECS/GEM) TRAY VALIDATION 의 ACK이 문제가 있습니다. 확인 해 주세요.<en>There is a problem with the ACK of VALIDATION. Please check")]
		WARNING_SESCGEM_VALIDATION_ACK_ERROR,
		[DefaultValueAttribute("<ko>(SECS/GEM) INSPECTION RESULT CONFIRM - SECS/GEM NG가 발생했습니다.<en>INSPECTION RESULT CONFIRM - SECS/GEM NG has occurred")]
		WARNING_SESCGEM_INPSECTION_RESULT_CONFIRM_SECSGEM_NG,
		[DefaultValueAttribute("<ko> NG가 대량으로 발생하였습니다.시퀀스 진행을 위해 빈트레이를 우선 배출합니다.<en>A large number of NGs have occurred. To proceed with the sequence, the empty tray will be Unload first.")]
		WARNING_SEQ_EMPTY_TRAY_UNLOAD,
		#endregion
		#region AOI 알람
		[DefaultValueAttribute("<ko>원인 모를 알람 발생.<en>Alarm_that_is_not_known_has_occurred_Please_restart_to_reset_the_alarm")]
		WARNING_AOI_ALARM_THAT_IS_NOT_KNOWN_HAS_OCCURRED_PLEASE_RESTART_TO_RESET_THE_ALARM = 1000,
		[DefaultValueAttribute("<ko>PCB 반입 실패 시 알람 발생.<en>PCB_Load_Fail")]
		WARNING_AOI_PCB_LOAD_FAIL,
		[DefaultValueAttribute("<ko>PCB 반출 실패 시 알람 발생.<en>PCB_UnLoad_Fail")]
		WARNING_AOI_PCB_UNLOAD_FAIL,
		[DefaultValueAttribute("<ko>피듀셜 검사 실패 시 알람 발생.<en>There_is_no_fiducial")]
		WARNING_AOI_THERE_IS_NO_FIDUCIAL,
		[DefaultValueAttribute("<ko>피듀셜 매칭 실패하여 검사 실패 시 알람 발생.<en>Fiducial_Search_Fail_Matching_error!_(low_matching)")]
		WARNING_AOI_FIDUCIAL_SEARCH_FAIL_MATCHING_ERROR,
		[DefaultValueAttribute("<ko>피듀셜 Blob이 너무 지저분 해서 검사 실패 시 알람 발생.<en>Fiducial_Search_Fail_a_lot_of_blob")]
		WARNING_AOI_FIDUCIAL_SEARCH_FAIL_A_LOT_OF_BLOB,
		[DefaultValueAttribute("<ko>피듀셜 찾지 못해서 검사 실패 시 알람 발생.<en>Fiducial_Search_Fail_Not_found_fiducial_mark")]
		WARNING_AOI_FIDUCIAL_SEARCH_FAIL_NOT_FOUND_FIDUCIAL_MARK,
		[DefaultValueAttribute("<ko>파트 검사 실패 시 알람 발생.<en>Part_Inspection_Fail")]
		WARNING_AOI_PART_INSPECTION_FAIL,
		[DefaultValueAttribute("<ko>모터 X 축 애러 시 알람 발생.<en>Error_Motor_X")]
		WARNING_AOI_ERROR_MOTOR_X,
		[DefaultValueAttribute("<ko>모터 Y 축 애러 시 알람 발생.<en>Error_Motor_Y")]
		WARNING_AOI_ERROR_MOTOR_Y,
		[DefaultValueAttribute("<ko>비상 정지 시 알람 발생.<en>Emergency_Stop")]
		WARNING_AOI_EMERGENCY_STOP,
		[DefaultValueAttribute("<ko>Under Door 열려 있을 시 알람 발생.<en>Open_Under_Door")]
		WARNING_AOI_OPEN_UNDER_DOOR,
		[DefaultValueAttribute("<ko>Upper Door 열려 있을 시 알람 발생.<en>Open_Upper_Door")]
		WARNING_AOI_OPEN_UPPER_DOOR,
		[DefaultValueAttribute("<ko>바코드 검사 실패 시 알람 발생.<en>Barcode_Search_Fail")]
		WARNING_AOI_BARCODE_SEARCH_FAIL,
		[DefaultValueAttribute("<ko>컨베이어 에러 시 알람 발생.<en>Error_Conveyor")]
		WARNING_AOI_ERROR_CONVEYOR = 1022,
		[DefaultValueAttribute("<ko>공압 부족 시 알람 발생.<en>Air_supply_error")]
		WARNING_AOI_AIR_SUPPLY_ERROR = 1033,
		[DefaultValueAttribute("<ko>PCB 위치 이상 시 알람 발생.<en>PCB_Position_error")]
		WARNING_AOI_PCB_POSITION_ERROR,
		[DefaultValueAttribute("<ko>Push up plate 에러 시 알람 발생.<en>Push_up_plate_error")]
		WARNING_AOI_PUSH_UP_PLATE_ERROR,
		[DefaultValueAttribute("<ko>센서 에러 시 알람 발생.<en>Sensor_sequence_error")]
		WARNING_AOI_SENSOR_SEQUENCE_ERROR,
		[DefaultValueAttribute("<ko>워크 스토퍼 에러 시 알람 발생.<en>Work_stopper_error")]
		WARNING_AOI_WORK_STOPPER_ERROR,
		[DefaultValueAttribute("<ko>레일 폭 맞지 않을 시 알람 발생.<en>Rail_size_error")]
		WARNING_AOI_RAIL_SIZE_ERROR,
		[DefaultValueAttribute("<ko>PCB 반입 반출 지연 시 알람 발생.<en>PCB_Time_out_error")]
		WARNING_AOI_PCB_TIME_OUT_ERROR,
		[DefaultValueAttribute("<ko>벤더 파트 검사 실패시 알람 발생.<en>VenderPart_Inspect_error")]
		WARNING_AOI_VENDER_PART_INSPECT_ERROR,
		[DefaultValueAttribute("<ko>머신 리셋 되어있지 않으면 알람 발생.<en>Not_Reset")]
		WARNING_AOI_NOT_RESET,
		[DefaultValueAttribute("<ko>AreaSensor 에러 시 알람 발생.<en>Raised_area_sensor_error")]
		WARNING_AOI_RAISED_AREA_SENSOR_ERROR,
		[DefaultValueAttribute("<ko>최초 실행 시 X, Y 축 Motor 사용 불가 시 알람 발생.<en>Servo_Off_Press_EMO_and_then_reset_switch")]
		WARNING_AOI_SERVO_OFF_PRESS_EMO_AND_THEN_RESET_SWITCH,
		[DefaultValueAttribute("<ko>보드 전체 파트 중 NG 파트 비율 값이 설정 값보다 높으면 알람 발생.<en>This_board_has_many_NG_parts_Check_the_board")]
		WARNING_AOI_THIS_BOARD_HAS_MANY_NG_PARTS_CHECK_THE_BOARD,
		[DefaultValueAttribute("<ko>리페어 스테이션 가득 찼을 시 알람 발생.<en>RepairStation_is_full_Check_the_RepairStation")]
		WARNING_AOI_REPAIR_STATION_IS_FULL_CHECK_THE_REPAIR_STATION,
		[DefaultValueAttribute("<ko>디스크 용량 1G 미만 시 알람 발생.<en>Not_Enough_Disk_Free_Space")]
		WARNING_AOI_NOT_ENOUGH_DISK_FREE_SPACE,
		[DefaultValueAttribute("<ko>BarcodeCom 통신 실패 시 알람 발생.<en>BarcodeCom_receive_fail!!")]
		WARNING_AOI_BARCODE_COM_RECEIVE_FAIL,
		[DefaultValueAttribute("<ko>Rear 단 PCB 반입 반출 지연 시 에러 발생.<en>REAR_PCB_Time_out_error")]
		WARNING_AOI_REAR_PCB_TIME_OUT_ERROR,
		[DefaultValueAttribute("<ko>2 Step 모드에서 Board 위치가 OUT 단에 없으면 알람 발생.<en>PCB_Second_Load_Position_Fail")]
		WARNING_AOI_PCB_SECOND_LOAD_POSITION_FAIL,
		[DefaultValueAttribute("<ko>NetIO 통신 실패 시 알람 발생.<en>NetIO_Communication_Error")]
		WARNING_AOI_NET_IO_COMMUNICATION_ERROR,
		[DefaultValueAttribute("<ko>Euresys CXP Camera Cable 불량 시 알람 발생.<en>Image_acquisition_fail,_check_the_camera_cable_!!!")]
		WARNING_AOI_IMAGE_ACQUISITION_FAIL_CHECK_THE_CAMERA_CABLE,
		[DefaultValueAttribute("<ko>Euresys CXP Camera 트리거 지연 시 알람 발생.<en>Image_acquisition_fail,_Timeout")]
		WARNING_AOI_IMAGE_ACQUISITION_FAIL_TIMEOUT,
		[DefaultValueAttribute("<ko>Euresys CXP API Grab buffer push 에러 시 알람 발생.<en>Image_acquisition_fail,_CXP_API_Grab_buffer_push_error")]
		WARNING_AOI_IMAGE_ACQUISITION_FAIL_CXP_API_GRAB_BUFFER_PUSH_ERROR,
		[DefaultValueAttribute("<ko>Euresys CXP Camera 연결 실패 시 알람 발생.<en>Image_acquisition_fail,_CXP_camera_connection_error")]
		WARNING_AOI_IMAGE_ACQUISITION_FAIL_CXP_CAMERA_CONNECTION_ERROR,
		[DefaultValueAttribute("<ko>Euresys CXP Camera 트리거 실패 시 알람 발생.<en>Image_acquisition_fail,_Trigger_loss")]
		WARNING_AOI_IMAGE_ACQUISITION_FAIL_TRIGGER_LOSS,
		[DefaultValueAttribute("<ko>Euresys CXP 일 때, Live 중 CRC 에러 시 알람 발생.<en>Image_acquisition_fail,_check_the_camera_cable_(CrcError)")]
		WARNING_AOI_IMAGE_ACQUISITION_FAIL_CHECK_THE_CAMERA_CABLE_CRC_ERROR,
		[DefaultValueAttribute("<ko>NGBuffer OK PCB 가 NG 랙으로 들어갈 때 알람 발생.<en>OK_PCB_inserted_NG_rack")]
		WARNING_AOI_OK_PCB_INSERTED_NG_RACK,
		[DefaultValueAttribute("<ko>NGBuffer NG PCB 가 OK 랙으로 들어갈 때 알람 발생.<en>NG_PCB_inserted_OK_rack")]
		WARNING_AOI_NG_PCB_INSERTED_OK_RACK,
		[DefaultValueAttribute("<ko>NGBuffer NG PCB 가 NG 랙으로 들어갈 때 알람 발생.<en>Overloaded_in_NG_rack_NG_PCB_to_NG_rack")]
		WARNING_AOI_OVERLOADED_IN_NG_RACK_NG_PCB_TO_NG_RACK,
		[DefaultValueAttribute("<ko>NGBuffer NG PCB 가 OK Confirm 혹은 NG Confirm 랙으로 들어갈 때 알람 발생.<en>Overloaded_in_NG_rack_NG_PCB_to_confirmed_rack")]
		WARNING_AOI_OVERLOADED_IN_NG_RACK_NG_PCB_TO_CONFIRMED_RACK,
		[DefaultValueAttribute("<ko>NGBuffer 연동시 일정 시간동안 NG버퍼로부터 IN 신호 못받으면 알람 발생.<en>No_'IN'_message_from_NG_Buffer")]
		WARNING_AOI_NO_IN_MESSAGE_FROM_NG_BUFFER,
		[DefaultValueAttribute("<ko>CALLING인 PCB 에 NG Buffer로부터 OUT_ 신호가 와서 없어지면 알람 발생.<en>CALLING_PCB_is_out_from_NG_Buffer_rack_Please_check_NG_Buffer's_rack")]
		WARNING_AOI_CALLING_PCB_IS_OUT_FROM_NG_BUFFER_RACK_PLEASE_CHECK_NG_BUFFERS_RACK,
		[DefaultValueAttribute("<ko>NG인 PCB 에 NG Buffer로부터 OUT_ 신호가 와서 없어지면 알람 발생.<en>NG(No_confirm)_PCB_is_out_from_NG_Buffer_rack_Please_check_NG_Buffer's_rack")]
		WARNING_AOI_NG_NO_CONFIRM_PCB_IS_OUT_FROM_NG_BUFFER_RACK_PLEASE_CHECK_NG_BUFFERS_RACK,
		[DefaultValueAttribute("<ko>NG Confirm인 PCB 에 NG Buffer로부터 OUT_ 신호가 와서 없어지면 알람 발생.<en>NG_confirmed_PCB_is_out_from_NG_Buffer_rack_Please_check_NG_Buffer's_rack")]
		WARNING_AOI_NG_CONFIRMED_PCB_IS_OUT_FROM_NG_BUFFER_RACK_PLEASE_CHECK_NG_BUFFERS_RACK,
		[DefaultValueAttribute("<ko>CALLED된 PCB가 있는데 CALLOK 신호가 온 경우 알람 발생.<en>Already_exist_called_PCB_Lost_the_data_on_previous_called_PCB")]
		WARNING_AOI_ALREADY_EXIST_CALLED_PCB_LOST_THE_DATA_ON_PREVIOUS_CALLED_PCB,
		[DefaultValueAttribute("<ko>NGBuffer로부터 ALARM_ 메시지 올 경우 알람 발생.<en>Received_'ALARM_'_command_from_NG_Buffer_Please_check_NG_Buffer")]
		WARNING_AOI_RECEIVED_ALARM_COMMAND_FROM_NG_BUFFER_PLEASE_CHECK_NG_BUFFER,
		[DefaultValueAttribute("<ko>NGBuffer로부터 규약에 없는 메시지 올 경우 알람 발생.<en>Received_unknown_command_from_NG_Buffer_Please_check_NG_Buffer")]
		WARNING_AOI_RECEIVED_UNKNOWN_COMMAND_FROM_NG_BUFFER_PLEASE_CHECK_NG_BUFFER,
		[DefaultValueAttribute("<ko>NGBuffer 랙 Full 이면 알람 발생.<en>NG_buffer_rack_is_full")]
		WARNING_AOI_NG_BUFFER_RACK_IS_FULL,
		[DefaultValueAttribute("<ko>잡에 저장된 바코드 라인 수와 실제 읽은 바코드 라인 수가 같지 않을 때 알람 발생.<en>Read_barcode_counts_are_different_from_job's_barcode_counts")]
		WARNING_AOI_READ_BARCODE_COUNTS_ARE_DIFFERENT_FROM_JOBS_BARCODE_COUNTS,
		[DefaultValueAttribute("<ko>검사 시작 시 동일 파트의 동일 디펙에 대한 트랜드 알람 발생.<en>Trend_Alarm!")]
		WARNING_AOI_TREND_ALARM,
		[DefaultValueAttribute("<ko>모든 모듈이 배드마크 처리되었을 때 알람 발생.<en>All_bad_mark_detected")]
		WARNING_AOI_ALL_BAD_MARK_DETECTED,
		[DefaultValueAttribute("<ko>MES 파일 저장 실패 시 알람 발생.<en>MES_File_Save_Fail")]
		WARNING_AOI_MES_FILE_SAVE_FAIL,
		[DefaultValueAttribute("<ko>(BOSE) 하단 바코드 검사 실패 시 알람 발생.<en>Bose_Barcode_Overlap_or_Exception")]
		WARNING_AOI_BOSE_BARCODE_OVERLAP_OR_EXCEPTION,
		[DefaultValueAttribute("<ko>MES UseSystem 사용 중 에러 시 알람 발생.<en>Error_occur_while_using_MES_system")]
		WARNING_AOI_ERROR_OCCUR_WHILE_USING_MES_SYSTEM,
		[DefaultValueAttribute("<ko>하단 바코드 Read 실패 시 알람 발생.<en>BOT_Barcorde_Search_Fail")]
		WARNING_AOI_BOT_BARCODE_SEARCH_FAIL,
		[DefaultValueAttribute("<ko>상단 바코드 Read 실패 시 알람 발생.<en>TOP_Barcorde_Search_Fail")]
		WARNING_AOI_TOP_BARCODE_SEARCH_FAIL,
		[DefaultValueAttribute("<ko>NG Part Image 저장 실패 시 알람 발생.<en>NG_Part_Image_Save_Function_Error")]
		WARNING_AOI_NG_PART_IMAGE_SAVE_FUNCTION_ERROR,
		[DefaultValueAttribute("<ko>(Tyco) tc 파일 Read 실패 시 알람 발생.<en>DB_DATA_EXCEPTION")]
		WARNING_AOI_DB_DATA_EXCEPTION,
		[DefaultValueAttribute("<ko>Offline에서 저장한 데이터 로드 실패 시 알람 발생.<en>Part_is_not_exists")]
		WARNING_AOI_PART_IS_NOT_EXISTS,
		[DefaultValueAttribute("<ko>(LG_IVI) 검사 전 설정 항목이 체크 되어 있지 않으면 알람 발생.<en>Business_Rules_validate_fails")]
		WARNING_AOI_BUSINESS_RULES_VALIDATE_FAILS,
		[DefaultValueAttribute("<ko>바코드에 Essential Word 포함되지 않을 시 알람 발생.<en>There_is_no_word_on_the_barcode")]
		WARNING_AOI_THERE_IS_NO_WORD_ON_THE_BARCODE,
		[DefaultValueAttribute("<ko>외부 카메라 신호 없을 시 알람 발생.<en>There_is_no_signal_from_the_external_camera")]
		WARNING_AOI_THERE_IS_NO_SIGNAL_FROM_THE_EXTERNAL_CAMERA,
		[DefaultValueAttribute("<ko>외부 카메라에 유효하지 않은 신호 발생 시 알람 발생.<en>There_is_an_invalid_signal_from_the_external_camera")]
		WARNING_AOI_THERE_IS_AN_INVALID_SIGNAL_FROM_THE_EXTERNAL_CAMERA,
		[DefaultValueAttribute("<ko>CI 반전기 반전 시컨스 전 보드가 두장이면 알람 발생(In2 , W3 동시 감지).<en>There_is_two_board_in_the_machine_")]
		WARNING_AOI_THERE_IS_TWO_BOARD_IN_THE_MACHINE,
		[DefaultValueAttribute("<ko>바코드에 의한 잡 변경 시 Job Manager에 접속 못했으면 알람 발생.<en>DB_connect_fail_")]
		WARNING_AOI_DB_CONNECT_FAIL,
		[DefaultValueAttribute("<ko>바코드에 의한 잡 변경 시 잡이 없으면 알람 발생.<en>Not_found_job!")]
		WARNING_AOI_NOT_FOUND_JOB,
		[DefaultValueAttribute("<ko>셔틀 레인 컨베이어 폭이 다르면 알람 발생.<en>Conveyor_width_is_different")]
		WARNING_AOI_CONVEYOR_WIDTH_IS_DIFFERENT,
		[DefaultValueAttribute("<ko>검사 결과 파일 생성 실패 시 알람 발생.<en>Failed_to_output_result_file!!")]
		WARNING_AOI_FAILED_TO_OUTPUT_RESULT_FILE,
		[DefaultValueAttribute("<ko>바코드 실패 시 알람 발생.<en>Barcode_Fail")]
		WARNING_AOI_BARCODE_FAIL,
		[DefaultValueAttribute("<ko>바코드 실패 시 알람 발생 (오프라인).<en>Barcode_Fail_Offline")]
		WARNING_AOI_BARCODE_FAIL_OFFLINE,
		[DefaultValueAttribute("<ko>알람 초기화 용도.<en>None")]
		WARNING_AOI_NONE,
		[DefaultValueAttribute("<ko>잡 변경 시 AOI Loader로부터 Model No 전달 받지 못할 시 알람 발생.<en>No_'MODELOK'_message_from_Loader")]
		WARNING_AOI_NO_MODELOK_MESSAGE_FROM_LOADER,
		[DefaultValueAttribute("<ko>파트에 의한 잡 변경 시 파트 검사 실패하면 알람 발생.<en>Job_Change_by_Part_:_Exceeded_count")]
		WARNING_AOI_JOB_CHANGE_BY_PART_EXCEEDED_COUNT,
		[DefaultValueAttribute("<ko>바코드에 의한 잡 변경 시 바코드 검사 실패하면 알람 발생.<en>Job_Change_by_Barcode_:_Barcode_Inspection_Fail")]
		WARNING_AOI_JOB_CHANGE_BY_BARCODE_BARCODE_INSPECTION_FAIL,
		[DefaultValueAttribute("<ko>배드마크 비교 실패 시 알람 발생.<en>Compare_Fail")]
		WARNING_AOI_COMPARE_FAIL,
		[DefaultValueAttribute("<ko>검사 전 설정 항목이 체크 되어 있지 않으면 알람 발생.<en>Check_items_before_inspection")]
		WARNING_AOI_CHECK_ITEMS_BEFORE_INSPECTION,
		[DefaultValueAttribute("<ko>불량마스터 검사인데 검사결과가 OK 나올 시 알람 발생.<en>Please_fine_tuning_because_the_NG_PCB_result_is_OK!")]
		WARNING_AOI_PLEASE_FINE_TUNING_BECAUSE_THE_NG_PCB_RESULT_IS_OK,
		[DefaultValueAttribute("<ko>양품마스터 검사인데 디펙뷰어에서 NG컨펌 시 알람 발생.<en>Please_fine_tuning_because_the_OK_PCB_result_is_NG!")]
		WARNING_AOI_PLEASE_FINE_TUNING_BECAUSE_THE_OK_PCB_RESULT_IS_NG,
		[DefaultValueAttribute("<ko>검사한 바코드가 잡에 저장된 양품, 불량 마스터 바코드와 같지 않으면 알람 발생.<en>Inspected_barcode_is_different_from_master_barcode!")]
		WARNING_AOI_INSPECTED_BARCODE_IS_DIFFERENT_FROM_MASTER_BARCODE,
		[DefaultValueAttribute("<ko>양품마스터 바코드가 비었을 시 알람 발생.<en>OK_master_barcode_is_empty!")]
		WARNING_AOI_OK_MASTER_BARCODE_IS_EMPTY,
		[DefaultValueAttribute("<ko>불량마스터 바코드가 비었을 시 알람 발생.<en>NG_master_barcode_is_empty!")]
		WARNING_AOI_NG_MASTER_BARCODE_IS_EMPTY,
		[DefaultValueAttribute("<ko>마스터 바코드 사용 기간 만료 시 알람 발생.<en>The_period_for_which_the_master_barcode_is_available_has_expired!")]
		WARNING_AOI_THE_PERIOD_FOR_WHICH_THE_MASTER_BARCODE_IS_AVAILABLE_HAS_EXPIRED,
		[DefaultValueAttribute("<ko>NG마스터 검사할 때 NG파트 등록하여 등록된 파트들이 하나라도 검출되지 않으면 알람 발생.<en>It's_not_the_ng_part!")]
		WARNING_AOI_ITS_NOT_THE_NG_PART,
		[DefaultValueAttribute("<ko>FPY값이 사용자가 설정한 값 이하로 내려갈 시 알람 발생.<en>The_FPY_value_is_lower_than_the_set_value")]
		WARNING_AOI_THE_FPY_VALUE_IS_LOWER_THAN_THE_SET_VALUE,
		[DefaultValueAttribute("<ko>Strip Map data 파일 읽어 BadMark처리할 때, 파일이 없으면 알람 발생.<en>Not_exist_the_map_data_file!")]
		WARNING_AOI_NOT_EXIST_THE_MAP_DATA_FILE,
		[DefaultValueAttribute("<ko>Strip Map data 파일 읽어 BadMark처리할 때, Module 같지 않으면 알람 발생.<en>Module_count_not_the_same_from_current_job_and_map_data_file!")]
		WARNING_AOI_MODULE_COUNT_NOT_THE_SAME_FROM_CURRENT_JOB_AND_MAP_DATA_FILE,
		[DefaultValueAttribute("<ko>Vacuum Air Level 낮으면 알람 발생.<en>Vacuum_Air_Level_Low")]
		WARNING_AOI_VACUUM_AIR_LEVEL_LOW,
		[DefaultValueAttribute("<ko>Vacuum Air - 21번 IO 에러 시 알람 발생.<en>Vacuum_Sensor_Error")]
		WARNING_AOI_VACUUM_SENSOR_ERROR,
		[DefaultValueAttribute("<ko>PCB 끝단 검사 실패 시 알람 발생.<en>PCB_Stop_Position_Inspection_Fail")]
		WARNING_AOI_PCB_STOP_POSITION_INSPECTION_FAIL,
		[DefaultValueAttribute("<ko>Stad 전송 실패 시 알람 발생.<en>Stad_sending_fail")]
		WARNING_AOI_STAD_SENDING_FAIL,
		[DefaultValueAttribute("<ko>Stad 연결 지연 시 알람 발생.<en>Stad_Time_out")]
		WARNING_AOI_STAD_TIME_OUT,
		[DefaultValueAttribute("<ko>최초 실행 시 컨베이어 연결 실패 시 알람 발생.<en>Conveyor_connection_fail")]
		WARNING_AOI_CONVEYOR_CONNECTION_FAIL,
		[DefaultValueAttribute("<ko>시작 위치 오류(Lot NO) 알람 발생.<en>This_is_Wrong_Start_Position(LotNo)")]
		WARNING_AOI_START_POSITION_FAIL_LOT,
		[DefaultValueAttribute("<ko>시작 위치 오류(Board ID) 알람 발생.<en>This_is_Wrong_Start_Position(Board ID)")]
		WARNING_AOI_START_POSITION_FAIL_BID,
		[DefaultValueAttribute("<ko>자동 조명 조정 중 문제 발생 시 알람 발생.<en>Please_Check_the_ColorSet")]
		WARNING_AOI_PLEASE_CHECK_THE_COLOR_SET,
		[DefaultValueAttribute("<ko>.<en>Shutter_Close_Alarm_!!!")]
		WARNING_AOI_SHUTTER_CLOSE_ALARM,
		[DefaultValueAttribute("<ko>M.S.L 서버에서 에러 발생 시 알람 발생.<en>Error_from_MSL_Server")]
		WARNING_AOI_ERROR_FROM_MSL_SERVER,
		[DefaultValueAttribute("<ko>검사 전 여유 메모리 체크 3G 이하 시 알람 발생.<en>Not_Enough_Memory_Free_Space")]
		WARNING_AOI_NOT_ENOUGH_MEMORY_FREE_SPACE,
		[DefaultValueAttribute("<ko>CI Inline 검사 중 AreaSensor 에러 시 알람 발생 (센서 방향 : Left).<en>Raised_area_sensor_error[Left]")]
		WARNING_AOI_RAISED_AREA_SENSOR_ERROR_LEFT,
		[DefaultValueAttribute("<ko>CI Inline 검사 중 AreaSensor 에러 시 알람 발생 (센서 방향 : Right).<en>Raised_area_sensor_error[Right]")]
		WARNING_AOI_RAISED_AREA_SENSOR_ERROR_RIGHT,
		[DefaultValueAttribute("<ko>AI 모듈 실패 알람 발생.<en>AI_Module_Failure_:_")]
		WARNING_AOI_AI_MODULE_ALARM,
		[DefaultValueAttribute("<ko>제어에게 받은 데이터 오류 알람 발생.<en>Crtl_Data_Error")]
		WARNING_AOI_CTRL_DATA_ERROR,
		[DefaultValueAttribute("<ko>SECSGEM 전송 타임아웃 알람 발생.<en>SECSGEM_Report_Transmission_TIME_OUT")]
		WARNING_AOI_SECSGEM_TIMEOUT,
		[DefaultValueAttribute("<ko>SECSGEM NG 알람 발생.<en>SSECSGEM_CONFIRM_NG")]
		WARNING_AOI_SECSGEM_CONFIRM_NG,
		[DefaultValueAttribute("<ko>2pc 사용 중 실패 시, Master PC에 알람 발생.<en>Slave_PC_communication_Failure_:_")]
		WARNING_AOI_SLAVE_PC_COMMUNICATION_FAILURE,
		[DefaultValueAttribute("<ko>2pc 사용 중 실패 시, Slave PC에 알람 발생.<en>Slave_PC_Failure_:_")]
		WARNING_AOI_SLAVE_PC_FAILURE,
		[DefaultValueAttribute("<ko>다음 랏 처리 처리 없음 오류 발생.<en>No_next_lot_to_proceed")]
		WARNING_AOI_NO_NEXT_LOT_TO_PROCEED,
		[DefaultValueAttribute("<ko>SUB_ID 확인 실패.<en>SUB_ID Validation Fail")]
		WARNING_AOI_VALIDATION_FAIL,
		[DefaultValueAttribute("<ko>NG PCB 제거 요청 알람 발생.<en>Please Remove NG PCB.")]
		WARNING_AOI_REMOVE_NG_PCB,
		[DefaultValueAttribute("<ko>오프라인에디터 실패.<en>OffEditControl Failure : ")]
		WARNING_AOI_OFFEDITCONTROL_ALARM,
		[DefaultValueAttribute("<ko>드라이브 연결 실패.<en>Check Connection Drive.")]
		WARNING_AOI_DRIVECONNECTION_FAIL,
		[DefaultValueAttribute("<ko>PCB 표면 컬러 검색 실패.<en>Auto PCB Surface Color Searching Fail.")]
		WARNING_AOI_PCB_SURFACE_INSPECTION_FAIL,
		[DefaultValueAttribute("<ko>보드수량 비동기 패킷 전송 실패.<en>Send BoardCounter Async Packet Fail.")]
		WARNING_AOI_BOARD_COUNTER_ASYNC_FAIL,
		[DefaultValueAttribute("<ko>제어 SAFETY 알람 발생.<en>Check Control Safety Alarm.")]
		WARNING_AOI_CONTROL_SAFETY_ALARM,
		[DefaultValueAttribute("<ko>제어 알람 발생.<en>Check Control Alarm.")]
		WARNING_AOI_CONTROL_ALARM,
		[DefaultValueAttribute("<ko>보드 데이터 쓰기는 비밀번호와 결함코드의 불일치로 금지되어 있습니다.<en>Board Data Writing is Forbided. Due to Mismatch in Passcode and Defectcode")]
		WARNING_AOI_WRITING_ODD_BOARD_DATA,
		[DefaultValueAttribute("<ko>모듈 수 데이터 불일치로 데이터 쓰기가 금지됩니다.<en>Module Count Data Writing is Forbided. Due to Mismatch in Module Count Data")]
		WARNING_AOI_WRITING_ODD_MODULE_DATA,
		[DefaultValueAttribute("<ko>SUB PROCESS 상태 체크.<en>Check SubProcess State.")]
		WARNING_AOI_SUBPROCESS_NOT_ALIVE,
		[DefaultValueAttribute("<ko>S현재 Library의 연결 상태가 Http가 아닙니다.<en>The current connection status of Library is not Http.")]
		WARNING_AOI_LIBRARY_NOT_HTTP,
		[DefaultValueAttribute("<ko>부품 3D 계산 시간이 초과되었습니다.<en>Part 3D calculation timed out.")]
		WARNING_AOI_CALC_3D_PART_TIMEOUT,
		[DefaultValueAttribute("<ko>부품 3D 계산 시간이 초과되었습니다.<en>Foreign 3D calculation timed out.")]
		WARNING_AOI_CALC_3D_FR_TIMEOUT,
		[DefaultValueAttribute("<ko>외부 AI 실패.<en>External AI Failure : ")]
		WARNING_AOI_EXTERNAL_AI_ALRAM,
		[DefaultValueAttribute("<ko>서버와 연결 끊어짐 오류.<en>Lost connection with the server.")]
		WARNING_AOI_LIBRARY_SERVER_SYNC,
		[DefaultValueAttribute("<ko>라이브러리와 연결 끊어짐 오류.<en>Lost connection with the Library.")]
		WARNING_AOI_LIBRARY_CONNECTION_FAIL,
		[DefaultValueAttribute("<ko>SPC로 보내는 InspStartTime의 형식이 잘못되었습니다.<en>The InspStartTime send to SPC is in the wrong format.")]
		WARNING_AOI_SPC_INSP_TIME_WRONG,
		[DefaultValueAttribute("<ko>Http 버전이 로컬 버전보다 낮습니다.<en>Http Version is lower than Local Version.")]
		WARNING_AOI_LIBRARY_LOW_VERSION,
		[DefaultValueAttribute("<ko>오븐컨트롤러와 연결 실패.<en>Oven not connected.")]
		WARNING_AOI_OVEN_NOT_CONNECTED,
		[DefaultValueAttribute("<ko>오븐컨트롤러와 통신 오류.<en>Oven communication error.")]
		WARNING_AOI_OVEN_COMM_ERROR,
		[DefaultValueAttribute("<ko>이오나이저 실패.<en>Ionizer fail.")]
		WARNING_AOI_IONIZER_ERROR,
		[DefaultValueAttribute("<ko>Out Stage 센서가 시간 초과를 감지했습니다.<en>Unload, Out Stage Sensor detected timed out.")]
		WARNING_AOI_UNLOAD_OUT_SENSOR_DETECTED_TIMEOUT,
		[DefaultValueAttribute("<ko>풀맵 이미지 경로 설정 실패.<en>Set Fullmap Image Path Fail.")]
		WARNING_AOI_FULLMAP_IMAGE_PATH_SET_FAIL,
		[DefaultValueAttribute("<ko>TwinTransfer 실패.<en>TwinTransfer Failure : ")]
		WARNING_AOI_TWINTRANSFER_ALARM,
		[DefaultValueAttribute("<ko>보드 크기 오류.<en>BoardSize Error.")]
		WARNING_AOI_BOARD_SIZE_ALARM,
		[DefaultValueAttribute("<ko>바코드 수정 오류.<en>Edit Barcode Fail.")]
		WARNING_AOI_EDIT_BARCODE_ALARM,
		[DefaultValueAttribute("<ko>오프라인 리페어 또는 리페어스테이션 종료되어 있어 오류 발생.<en>Offline Repair or Repair Station Off.")]
		WARNING_AOI_NSD_OFFLINE_OFF_ALARM,
		[DefaultValueAttribute("<ko>3D 계산 실패하여 강제 정지.<en>Calculation 3D is Fail. Force Stop.")]
		WARNING_AOI_CALC_3D_FAIL_FORCE_STOP,
		[DefaultValueAttribute("<ko>3D 계산 실패. 파트 리밋 참고.<en>Calculation 3D is Fail. Time Limit Reference Part.")]
		WARNING_AOI_CALC_3D_FAIL_TIME_LIMIT_REF_PART,
		[DefaultValueAttribute("<ko>3D 계산 실패. 바닥색이 충분하지 않음<en>alculation 3D is Fail. Not Enough Bottom Color.")]
		WARNING_AOI_CALC_3D_FAIL_NOT_ENOUGH_BTM_COLOR,
		[DefaultValueAttribute("<ko>Verify H/W 실패(그레이)<en>Verification HW is Fail.(Gray Verification)")]
		WARNING_AOI_VERIFICATION_HW_FAIL,
		[DefaultValueAttribute("<ko>색상 조정이 실패했습니다.(최대 강도: 63)<en>Color Adjust is Fail.(Max Intensity : 63)")]
		WARNING_AOI_ADJUST_MAX_FAIL,
		[DefaultValueAttribute("<ko>프로젝션 조정이 실패했습니다.(최대 강도: 63)<en>Projection Adjust is Fail.(Max Intensity : 63)")]
		WARNING_AOI_PROJECTION_ADJUST_MAX_FAIL,
		[DefaultValueAttribute("<ko>이오나이저 전원 알람(전면).<en>Ionizer Power Alarm. (Front)")]
		WARNING_AOI_FRONT_IONIZER_POWER_ALARM,
		[DefaultValueAttribute("<ko>이오나이저 컨트롤 릴레이 알람(전면).<en>Ionizer Control Relay Alarm. (Front)")]
		WARNING_AOI_FRONT_IONIZER_CTRL_RELAY_ALARM,
		[DefaultValueAttribute("<ko>이오나이저 컨트롤 릴레이 알람(후면).<en>Ionizer Power Alarm. (Rear)")]
		WARNING_AOI_REAR_IONIZER_POWER_ALARM,
		[DefaultValueAttribute("<ko>이오나이저 컨트롤 릴레이 알람(후면).<en>Ionizer Control Relay Alarm. (Rear)")]
		WARNING_AOI_REAR_IONIZER_CTRL_RELAY_ALARM,
		[DefaultValueAttribute("<ko>이오나이저 GLS 도어 알람.<en>Ionizer GLS Door Alarm.")]
		WARNING_AOI_IONIZER_GLS_DOOR_ALRAM,
		[DefaultValueAttribute("<ko>모터 오류.(P3M)<en>Motor Error. (P3M)")]
		WARNING_AOI_ERROR_MOTOR_P3M,
		[DefaultValueAttribute("<ko>모터 오류.(Z1)<en>Motor Error. (Z1)")]
		WARNING_AOI_ERROR_MOTOR_Z1,
		[DefaultValueAttribute("<ko>모터 오류.(Z2)<en>Motor Error. (Z2)")]
		WARNING_AOI_ERROR_MOTOR_Z2,
		[DefaultValueAttribute("<ko>컨베이어 레일 도어 열림.<en>Open Conveyor Rail Door.")]
		WARNING_AOI_OPEN_RAIL_DOOR,
		[DefaultValueAttribute("<ko>검사 프로그램 응답 없음.<en>No response from Insp")]
		WARNING_AOI_NO_RESPONSE_INSP = 1500,
		[DefaultValueAttribute("<ko>Job 오픈 실패.<en>Failed to opening the job.")]
		WARNING_AOI_JOB_OPEN_FAIL,
		[DefaultValueAttribute("<ko>P3M 시퀀스 Error 발생.<en>[P3M] sequence error.")]
		WARNING_AOI_P3M_SEQ_ERROR,
		[DefaultValueAttribute("<ko>P3M 제어 없는 모드 ON.<en>[P3M] No Control Mode.")]
		WARNING_AOI_P3M_NO_CTRL_MODE,
		[DefaultValueAttribute("<ko>P3M 모션 초기화가 필요합니다. <en>[P3M] Please Initialize Motion.")]
		WARNING_AOI_P3M_MOTION_INIT = 1600,
		[DefaultValueAttribute("<ko>P3M 모션 명령 실패.<en>[P3M] Motion cmd error.")]
		WARNING_AOI_P3M_MOTION_CMD,


		#endregion
		UNKNOWN = 1989,
		TOTAL = 1990,
	}
}