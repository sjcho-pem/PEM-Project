using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace H_MarsSSD
{
    #region [ Enum ]
    /// <summary>
    /// Equipment 장비 구분
    /// </summary>
    public enum EQP
    {        
        CTRL = 0,
        TOP,
        BTM,        
    };
       
    public enum EqpStatus
    {
        None, // 프로그램이 활성화 되어 있지 않은 상태
        Alive, // 프로그램 활성화 상태
        Run, // 프로그램 Run 상태
        Max,
    }

    public enum ServerReceiveCode
    {
        None = 0,
        Complate = 1,
        ApplicationClose = -1,
        TotalTimeOut = -2,
        ExceptionError = -3,
        ReceiveTimeOut = -4
    }

    public enum InspCmd
    {
        None,
        ReqState,
        ReqGrabStart,        
        State,
        Grab,        
    }

    public enum InspRes
    {
        None,
        Ok,
        NG,
    };
    #endregion

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct InspectData
    {
        //초기화
        public InspectData(char bit, int sID)
        {
            headbit = bit;
            EquipID = (EQP)sID;
            ProductID = "";
            InspResult = InspRes.None;
            State = EqpStatus.None;
            eCMD = InspCmd.None;
        }

        // 서버 코드 ( PC 구분 )
        public char headbit;
        public EQP EquipID;

        // GrabStart 명령어 일때만 사용.
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 50)]
        public string ProductID;

        // 검사 결과를 여기에
        public InspRes InspResult;

        // 상태 결과
        public EqpStatus State;

        public InspCmd eCMD;
    }
}

