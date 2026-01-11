using System;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace H_MarsSSD
{
    public enum LOGPART
    {
        [DefaultValueAttribute("SYSTEM")]
        SYS,
        [DefaultValueAttribute("PASSWORD")]
        PW,
        [DefaultValueAttribute("ERROR")]
        ERR,
        [DefaultValueAttribute("EXCEPTION")]
        EX,
        [DefaultValueAttribute("TENKEY")]
        TK,
        [DefaultValueAttribute("DATA")]
        DATA,
        [DefaultValueAttribute("MOTOR")]
        MT,
        [DefaultValueAttribute("HISTORY")]
        HIS,
        [DefaultValueAttribute("WARNING")]
        WARNING,
    }

    public class UILog
    {        
        #region 변수 및 변수초기값

        public delegate void LogMessageHandler(string msg);
        public event LogMessageHandler LogMessageHandled;
        
        private string strLogBasePath = @"D:\";//AppDomain.CurrentDomain.BaseDirectory; // 기본값
        private string strLogFolder = @"LOG\MMI\";
        private string strLogExt = @".log";
        private string strFilePreName = "";
        private string strNowLog = "";

        #endregion

        #region 로그 파일 속성 변경 함수들

        // XXX_yyyyMMdd.qqq 에서 XXX 부분을 바꾸어 줌. 디폴트는 "".
        public void ChangeLogFilePreName(string strName)
        {
            strFilePreName = strName;
        }

        // 기본되는 로그 파일 생성 경로를 변경한다. 디폴트는 프로그램이 실행되고 있는 폴더 경로.
        public void ChangeLogBasePath(string strPath)
        {
            strLogBasePath = strPath;
        }

        // 로그 파일이 생성되는 폴더의 이름을 바꾼다. 디폴트는 "LOG"
        public void ChangeLogFolderName(string strFolderName)
        {
            strLogFolder = strFolderName + @"\";
        }

        // XXX_yyyyMMdd.qqq 에서 qqq 부분을 바꾸어 줌. 디폴트는 "log".
        public void ChangeLogExt(string strExt)
        {
            strLogExt = @"." + strExt;
        }

        #endregion

        #region 속성값 읽는 함수들

        public string GetLogFilePrename()
        {
            return strFilePreName;
        }

        public string GetLogBasePath()
        {
            return strLogBasePath;
        }

        public string GetLogFolderName()
        {
            return strLogFolder;
        }

        public string GetLogExt()
        {
            return strLogExt;
        }

        #endregion

        #region WriteLog 함수들

        private void LogMessage(string msg)
        {
            if (LogMessageHandled != null)
            {
                LogMessageHandled(msg);
            }
        }
        
        // 실제로 로그 파일에 내용을 쓰는 함수
        // 날짜 변경, 폴더 이름 변경, 파일 이름 변경시에 새로 생성하고 로그를 기록한다.
        public void Write(string strPart, string strMsg)
        {
            try
            {
                DateTime dtNow = DateTime.Now; // time update
                string strDate = dtNow.ToString("yyyyMMdd");
                string strPath = string.Format("{0}{3}_{1}{2}", strLogBasePath + strLogFolder, strDate, strLogExt, strFilePreName);
                string strDir = Path.GetDirectoryName(strPath);

                DirectoryInfo dirInfo = new DirectoryInfo(strDir);

                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                    dirInfo = new DirectoryInfo(strDir);
                }

                if (dirInfo.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(strPath, true))
                    {
                        //sw = File.AppendText(strPath);                                        
                        strNowLog = string.Format("[{0}] <{2}> : {1}", dtNow.ToString("yyyy-MM-dd HH:mm:ss"), strMsg, strPart.ToUpper());

                        sw.WriteLine(strNowLog);
                        sw.Close();
                    }

                    LogMessage(strNowLog);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e.Message);
            }
        }

        // 실제로 로그 파일에 내용을 쓰는 함수
        // 날짜 변경, 폴더 이름 변경, 파일 이름 변경시에 새로 생성하고 로그를 기록한다.
        public void Write(LOGPART ePart, string strMsg)
        {
            try
            {
                DateTime dtNow = DateTime.Now; // time update
                string strDate = dtNow.ToString("yyyyMMdd");
                string strPath = string.Format("{0}{3}_{1}{2}", strLogBasePath + strLogFolder, strDate, strLogExt, strFilePreName);
                string strDir = Path.GetDirectoryName(strPath);

                DirectoryInfo dirInfo = new DirectoryInfo(strDir);

                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                    dirInfo = new DirectoryInfo(strDir);
                }

                if (dirInfo.Exists)
                {
                    using (StreamWriter sw = new StreamWriter(strPath, true))
                    {
                        //sw = File.AppendText(strPath);                                        
                        strNowLog = string.Format("[{0}] <{2}> : {1}", 
                            dtNow.ToString("yyyy-MM-dd HH:mm:ss"), 
                            strMsg, EnumHelper.GetDescription(ePart).ToUpper());

                        sw.WriteLine(strNowLog);
                        sw.Close();
                    }

                    LogMessage(strNowLog);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}", e.Message);
            }
        }


        public void WriteOutput(string[] strMsg, int LineNum)
        {
            try
            {
                DateTime dtNow = DateTime.Now; // time update
                string strDate = dtNow.ToString("yyyyMMdd");
                string strPath = string.Format("{0}{3}_{1}{2}", strLogBasePath + strLogFolder, strDate, @".out", strFilePreName);
                string strDir = Path.GetDirectoryName(strPath);

                DirectoryInfo dirInfo = new DirectoryInfo(strDir);

                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                    dirInfo = new DirectoryInfo(strDir);
                }

                if (dirInfo.Exists)
                {
                    //strNowLog = string.Format("[{0:D2}:{1:D2}:{2:D2}:{3:D3}] <MESSAGE> : {4}", dtNow.Hour, dtNow.Minute, dtNow.Second, dtNow.Millisecond, strMsg);
                    //strNowLog = string.Format("[{0}] <MESSAGE> : {1}", dtNow.ToString(), strMsg);
                    using (StreamWriter swStream = File.AppendText(strPath))
                    {
                        for (int i = 0; i < LineNum; i++)
                        {
                            swStream.WriteLine(strMsg[i]);
                        }
                        swStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
            }
        }

        #endregion

    }
}
