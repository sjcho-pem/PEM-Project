using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace H_MarsSSD
{
    #region LogItem
    [Serializable]
    public class LogItem : ICloneable
    {
        public LogItem(string description)
        {
            this.code = 0;
            this.description = description;
            this.timeStamp = DateTime.Now;
        }

        public LogItem(uint code, string description)
        {
            this.code = code;
            this.description = description;
            this.timeStamp = DateTime.Now;
        }

        public LogItem(uint code, string description, uint AlarmCode)
        {
            this.code = code;
            this.description = description;
            this.timeStamp = DateTime.Now;
            this.alarmcode = AlarmCode;
        }

        private uint alarmcode;
        public uint AlarmCode
        {
            get { return alarmcode; }
        }

        private uint code;
        public uint Code
        {
            get { return code; }
        }

        private string description;
        public string Description
        {
            get { return description; }
        }

        private DateTime timeStamp;
        public DateTime TimeStamp
        {
            get { return timeStamp; }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    #endregion

    #region Log Events
    //Event argument class
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(LogItem item)
        {
            this.item = item;
        }

        private LogItem item;
        public LogItem Item
        {
            get { return item; }
        }
    }
    #endregion

    #region Text Utils
    static public class TextUtils
    {
        static public string Line(string s, int n)
        {
            string[] sa = s.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            return (n >= 0 && sa.Length > n) ? sa[n] : "";
        }

        static public string ExMessage(Exception ex)
        {
            StringBuilder s = new StringBuilder(ex.Message);
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                s.AppendFormat(":\r\n{0}", ex.Message);
            }
            if (ex.StackTrace != null)
            {
                string[] sa = ex.StackTrace.Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s1 in sa)
                {
                    if (!s1.Contains(":line") && !s1.Contains(":줄"))
                        continue;

                    s.AppendFormat("\r\n{0}", s1.Trim());
                    break;
                }
            }
            return s.ToString();
        }
    }
    #endregion


    public class LogBase
    {
        public enum LOG_LEVEL : uint
        {
            INFO,
            SEQ,
            WARN,
            ERR,
			MES,
        }
        
        private string logFolder;
        private string logName;
        private string logFile;

        private DateTime startTime;
        private StreamWriter swLog;

        public LogBase(string sLogFolder, string sLogName)
        {
            if (Directory.Exists(sLogFolder) == false)
                Directory.CreateDirectory(sLogFolder);
            logFolder = sLogFolder;
            logName = sLogName;

            logFile = string.Format("{0}\\{1}_{2}{3}", logFolder, logName, DateTime.Now.ToString("yyyyMMdd"), ".csv");

            if (File.Exists(logFile) == false)
            {
                OpenCSV(logFile);

                if (sLogName.Contains("SeqLog"))
                {
                    WriteCSV( new string[] { "Level", "Date", "Location", "Line", "Message" });                    
                }
            }
            else
            {
                OpenCSV(logFile);
            }

            FileInfo fi = new FileInfo(logFile);
            startTime = fi.CreationTime;
        }

        public void Dispose()
        {
            CloseCSV();
        }           

        private void Add(LogItem e)
        {
            lock (this)
            {              
                DateTime CurrentTime = DateTime.Now;
                if (CurrentTime.Day != startTime.Day)
                {
					//0으로 설정 시, 로그  파일 삭제 안함.
                    if ( NDm.Read(eNDM.LogStoragePeriod) != 0 )
                    {
						OldFileDelete();
					}

                    CloseCSV();

                    startTime = DateTime.Now;
                    logFile = string.Format("{0}\\{1}_{2}{3}", logFolder, logName, DateTime.Now.ToString("yyyyMMdd"), ".csv");

                    OpenCSV(logFile);
                    WriteCSV(new string[] { "Level", "Date", "Location", "Line", "Message" });

                    FileInfo fi = new FileInfo(logFile);
                    startTime = fi.CreationTime;
                }

                WriteCSV(new string[] { ((LOG_LEVEL)e.Code).ToString(), e.TimeStamp.ToString("dd-MM-yyyy HH:mm:ss.fff"), e.Description });
            }            
        }


        public void Add(string s)
        {
            Add(new LogItem(s));
        }

        public void Add(int level, string s)
        {
            Add(new LogItem((uint)level, s));
        }

        //----------------------------------------------------------------
        //public void Add(Exception ex, string sHead, bool bMain = false)
        //{
        //    string sMsg = TextUtils.ExMessage(ex);
        //    if (!string.IsNullOrEmpty(sHead))
        //        sMsg = sHead + sMsg;

        //    Add(new LogItem((int)LOG_LEVEL.WARN, sMsg, bMain));
        //}
        //----------------------------------------------------------------        


        private void OpenCSV(string fileName)
        {
            try
            {
                swLog = new StreamWriter(fileName, true, System.Text.Encoding.UTF8);
                swLog.AutoFlush = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("Close the Sequence log file.\n{0}", ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CloseCSV()
        {
            swLog.Close();
        }

        private void WriteCSV(string[] val)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("{0}\r\n", string.Join(",", val));
                swLog.Write(sb.ToString());
            }
            catch
            {
            }
        }
        public string GetLogFilePath()
        {
            return logFile;
        }
        
        private void OldFileDelete()
        {
            try
            {
                DateTime dtNow = DateTime.Now;
                DateTime dtLog;
                string[] lstLogPath = new string[] { @"D:\" + @"Log\Sequence", @"D:\" + @"Log\MMI", @"D:\" + @"Log\MES"};
                for (int i = 0; i < lstLogPath.Length; i++)
                {
                    if (System.IO.Directory.Exists(lstLogPath[i]))
                    {
                        System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(lstLogPath[i]);
                        foreach (var item in di.GetFiles())
                        {
                            string[] strtemp = item.Name.Split('_', '.');
                            if (strtemp.Length != 3)
                            {
                                 UI.Logs.Write(LOGPART.DATA, string.Format("FILE DELETE ERROR(Length) - File Name : {0}", item.Name));
                                continue;
                            }
                            if (DateTime.TryParseExact(strtemp[1], "yyyyMMdd", new CultureInfo("en-US"), DateTimeStyles.None, out dtLog) == false)
                            {
                                UI.Logs.Write(LOGPART.DATA, string.Format("FILE DELETE ERROR(Date Time Parse) - File Name : {0}", item.Name));
                                continue;
                            }
                            TimeSpan ts = dtNow.Subtract(dtLog);

                            int nStoragePeriod = NDm.Read(eNDM.LogStoragePeriod) > 10 ? NDm.Read(eNDM.LogStoragePeriod) : 10;
                            if (ts.TotalDays > nStoragePeriod)
                            {
                                item.Delete();
                                UI.Logs.Write(LOGPART.DATA, string.Format("FILE DELETE SUCCESS - File Name : {0}", item.Name));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                throw;
            }
            
        }
    }
}

