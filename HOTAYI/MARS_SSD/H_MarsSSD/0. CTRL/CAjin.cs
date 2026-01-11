using System;


namespace H_MarsSSD
{
    public class CAjin
    {           
        private static bool bDeviceOpened = false;  
        /// <summary>
        /// 아진보드 미장착시 or 오프라인 PC환경에서 사용일떄 변경하여 사용하십시요.
        /// </summary>
        public static bool bNoDevice { get => Properties.Settings.Default.AjinNoDevice; }
        /// <summary>
        /// [CAUTION] 아래와 다른 Mot파일(모션 설정파일)을 사용할 경우 Setting에서 경로를 변경하십시요.
        /// </summary>
        private static string strMotPath { get => Properties.Settings.Default.AjinMotFilePath; }
   
        public static void InitializeFunc()
        {
            bDeviceOpened = Open(7);                        
            if (bDeviceOpened)
                bDeviceOpened = LoadMot(strMotPath);
        }

        public static bool IsAjinDevice()
        {
            if (bNoDevice == true)
                return false;

            if (CAXL.AxlIsOpened() == 1 && bDeviceOpened)
                return true;
            return false;
        }

        public static bool ReDeviceOpen()
        {
            if (bNoDevice == true)
                return (true);

            if (IsAjinDevice() == true)
                return false;

            CAXL.AxlClose();

            bDeviceOpened = Open(7);
            return true;
        }

        private static bool Open(int nIrq)
        {
            if (bNoDevice == true)
                return (true);

            //++ AXL(AjineXtek Library)을 사용가능하게 하고 장착된 보드들을 초기화합니다.
            uint nRet = CAXL.AxlOpen(nIrq);

            if (nRet != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)           
                return (false);            

            return (true);
        }
        
        public static void Close()
        {
            if (bNoDevice == true)
                return;
            
            // 라이브러리 사용 종료.
            CAXL.AxlClose();
        }

        public static void CheckMotionErr(uint ret, int nAxis)
        {
            if (ret != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                AXT_FUNC_RESULT res = (AXT_FUNC_RESULT)ret;
                // 로그 추가
                string strErr = nAxis + " " + ret + "- " + res.ToString();
                GLog.Add(strErr);
            }
        }

        public static void CheckAjinErr(uint ret)
        {
            if (ret != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                AXT_FUNC_RESULT res = (AXT_FUNC_RESULT)ret;
                // 로그 추가
                string strErr = "AJIN ERR " + ret + "- " + res.ToString();
                GLog.Add(strErr);
            }
        }


        private static bool LoadMot(string strPath)
        {
            if (bNoDevice == true)
                return false;

            //++ 지정한 Mot파일의 설정값들로 모션보드의 설정값들을 일괄변경 적용합니다.
            uint nRet = CAXM.AxmMotLoadParaAll(strPath);

            if (nRet != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)            
                return (false);
            
            return (true);
        }

    }
}
