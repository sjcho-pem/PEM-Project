using System;
using System.IO;

namespace H_MarsSSD
{
    class CSVFile
    {
        public CSVFile(string path)
        {
            filePath = path;
        }

        #region [ private, public variables ]
        private const string dirPath = @"C:\BIN\DATA\";
        private string filePath;
        private string _strMsg = "";
        public string fullPath
        {
            get { return filePath; }
            set { filePath = value; }
        }        
        public string MSG
        {
            set { _strMsg = value; }
            get { return _strMsg; }
        }        
        #endregion

        #region [ Function ]
        public bool IsFileExist()
        {
            try
            {
                FileInfo fi = new FileInfo(filePath);
                return fi.Exists;
            }
            catch (Exception ex)
            {
                _strMsg = string.Format("EXCEPTION = {0}", ex.Message);
                return false;
            }
        }
        
        public bool CreateFile()
        {
            try
            {
                FileInfo fi = new FileInfo(filePath);
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                if (!dir.Exists) dir.Create();
                fi.CreateText();
                return true;
            }
            catch (Exception ex)
            {
                _strMsg = string.Format("EXCEPTION = {0}", ex.Message);
                return false;
            }
        }

        public bool AllClearLine()
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                if (!dir.Exists) dir.Create();
                FileInfo fi = new FileInfo(filePath);
                if (!fi.Exists) return false;

                StreamReader sr = new StreamReader(filePath);
                sr.DiscardBufferedData();
                sr.Close();

            }
            catch (Exception ex)
            {
                _strMsg = string.Format("EXCEPTION = {0}", ex.Message);
                return false;
            }

            return true;
        }

        public bool ReadLine(ref string text)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                if (!dir.Exists) dir.Create();
                FileInfo fi = new FileInfo(filePath);
                if (!fi.Exists) return false;
                
                StreamReader sr = new StreamReader(filePath);
                text = sr.ReadLine();                
                sr.Close();

            }
            catch (Exception ex)
            {
                _strMsg = string.Format("EXCEPTION = {0}", ex.Message);
                return false;
            }

            return true;
        }

        public string[] ReadAllLine()
        {                        
            return File.ReadAllLines(filePath);            
        }

        public bool WriteLine(string msg)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                if (!dir.Exists) dir.Create();
                StreamWriter sw = new StreamWriter(filePath, true);
                sw.WriteLine(msg);
                sw.Close();                
            }
            catch (Exception ex)
            {
                _strMsg = string.Format("EXCEPTION = {0}", ex.Message);
                return false;
            }

            return true;
        }

        public bool WriteAllLine(string[] msg)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                if (!dir.Exists) dir.Create();
                StreamWriter sw = new StreamWriter(filePath, false);

                foreach (string s in msg)
                {
                    sw.WriteLine(s);
                }
                
                sw.Close();
            }
            catch (Exception ex)
            {
                _strMsg = string.Format("EXCEPTION = {0}", ex.Message);
                return false;
            }

            return true;
        }
        #endregion
    }
}
