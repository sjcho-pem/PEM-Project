using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace H_MarsSSD
{
    public class XML
    {
        public struct CompareData
        {
            public int index;
            public string colname;
            public string nowvalue;
            public string oldvalue;

            public CompareData(int _index, string _colname, string _value, string _oldvalue)
            {
                index = _index;
                colname = _colname;
                nowvalue = _value;
                oldvalue = _oldvalue;
            }
        }

        #region [ Variables ]        
        private string sDataFolderPath = Program.DataDirectory;
        private DataTable m_DataTable;
        private DataTable m_OldDataTable;
        private DataSet m_DataSet;
        private XmlDocument xmlDocMember = new XmlDocument();
        private Aes aesKey;
        private string strFileName;

        private string m_strFileName
        {
            set { strFileName = value + ".XML"; }
            get { return strFileName; }
        }

        private Dictionary<string, CompareData> m_CompData = new Dictionary<string, CompareData>();
        #endregion

        private bool Initializ()
        {            
            try
            {  
                FileInfo tempFileInfo = new FileInfo(sDataFolderPath + m_strFileName);

                if (tempFileInfo.Exists)
                {
                    if (m_strFileName == "Member.XML")  
                        return InitEncryptedData();

                    m_DataSet = new DataSet();
                    m_DataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);
                    m_DataTable = m_DataSet.Tables[0];

                    DataSet m_OldDataSet = new DataSet();
                    m_OldDataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);
                    m_OldDataTable = m_OldDataSet.Tables[0];
                }
                else
                {
                    frmMessageBox pMsg = new frmMessageBox(false, "Xml File Check", "[" + m_strFileName + "] " + "Xml Documents is not file!!");
                    pMsg.ShowDialog();                    
                    Application.Exit();                    
                    return false; 
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public bool Initializ(string strTableName)
        {
            m_strFileName = strTableName;

            return Initializ();
        }

        public bool Initializ(string strTableName, int nFolderNum)
        {
            sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(nFolderNum);                    
            if (Directory.Exists(sDataFolderPath) == false)                        
                Directory.CreateDirectory(sDataFolderPath);

            m_strFileName = strTableName;

            return Initializ();
        }

        public bool Initializ(string strTableName, int BeforeIndex = -1, int TargetIndex = -1, int nFolderNum = -1)
        {
            try
            {
                if (nFolderNum != -1)
                {
                    sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(nFolderNum);

                    if (Directory.Exists(sDataFolderPath) == false)
                        Directory.CreateDirectory(sDataFolderPath);
                } 
                this.m_strFileName = strTableName;
                FileInfo tempFileInfo = new FileInfo(sDataFolderPath + m_strFileName);
                
                if (tempFileInfo.Exists)
                {
                    this.m_DataSet = new DataSet();
                    
                    this.m_DataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);

                    this.m_DataTable = this.m_DataSet.Tables[0];
                }
                else
                {
                    if (this.m_strFileName.IndexOf("RecipeData") != -1 && BeforeIndex != -1 && TargetIndex != -1)
                    {
                        /// 레시피 현재 변경하기 전의 레시피 기준으로 새로 만듦 

                        if (BeforeIndex == -2)
                        {
                            // D:DATA//Recipe_000 에 있는 DATA를 복사
                            m_strFileName = string.Format("RecipeData000.XML");
                            if (nFolderNum != -1)
                                sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(0);
                            this.m_DataSet = new DataSet();
                            this.m_DataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);

                            m_strFileName = string.Format("RecipeData{0:D3}.XML", TargetIndex + 1);
                            if (nFolderNum != -1)
                                sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(TargetIndex + 1);
                            this.m_DataSet.WriteXml(sDataFolderPath + m_strFileName);

                            // 레시피 현재 변경하기 전의 레시피 기준으로 모터 레시피 새로 만듦
                            for (int i = 0; i < 6; i++)
                            {
                                m_strFileName = string.Format("MotorData000_{0:D1}0.XML", i + 1);
                                if (nFolderNum != -1)
                                    sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(0);
                                this.m_DataSet = new DataSet();
                                this.m_DataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);

                                m_strFileName = string.Format("MotorData{0:D3}_{1:D1}0.XML", TargetIndex + 1, i + 1);
                                if (nFolderNum != -1)
                                    sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(TargetIndex + 1);
                                this.m_DataSet.WriteXml(sDataFolderPath + m_strFileName);
                            }

                            m_strFileName = string.Format("RecipeData{0:D3}.XML", TargetIndex + 1);

                            this.m_DataSet = new DataSet();
                            this.m_DataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);
                            this.m_DataTable = this.m_DataSet.Tables[0];
                        }
                        else
                        {
                            m_strFileName = string.Format("RecipeData{0:D3}.XML", BeforeIndex + 1);
                            this.m_DataSet = new DataSet();
                            if (nFolderNum != -1)
                                sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(BeforeIndex + 1);
                            this.m_DataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);

                            m_strFileName = string.Format("RecipeData{0:D3}.XML", TargetIndex + 1);
                            if (nFolderNum != -1)
                                sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(TargetIndex + 1);
                            this.m_DataSet.WriteXml(sDataFolderPath + m_strFileName);

                            // 레시피 현재 변경하기 전의 레시피 기준으로 모터 레시피 새로 만듦
                            for (int i = 0; i < 6; i++)
                            {
                                m_strFileName = string.Format("MotorData{0:D3}_{1:D1}0.XML", BeforeIndex + 1, i + 1);
                                this.m_DataSet = new DataSet();
                                if (nFolderNum != -1)
                                    sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(BeforeIndex + 1);

                                this.m_DataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);

                                m_strFileName = string.Format("MotorData{0:D3}_{1:D1}0.XML", TargetIndex + 1, i + 1);
                                if (nFolderNum != -1)
                                    sDataFolderPath = Program.DataDirectory + GetRecipeFolderPath(TargetIndex + 1);

                                this.m_DataSet.WriteXml(sDataFolderPath + m_strFileName);
                            }

                            m_strFileName = string.Format("RecipeData{0:D3}.XML", TargetIndex + 1);

                            this.m_DataSet = new DataSet();
                            this.m_DataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);
                            this.m_DataTable = this.m_DataSet.Tables[0];
                        }
                    }
                    else
                    {
                        return false;
                    }
                        
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        /// <summary>
        /// D:DATA//Recipy_xxx 폴더 명으로 변경
        /// Folder 주소값 변경 함수
        /// </summary>
        private string GetRecipeFolderPath(int strFolderAddr)
        {
            string result = string.Format("Recipe_{0:D3}//", strFolderAddr);

            return result;
        }

        // maybe not use
        public bool InsertData(DataRow drnew)
        {
            try
            {
                this.m_DataTable.Rows.Add(drnew);                
                this.m_DataTable.Clear();                                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // Data Table 내용을 실제 연결된 Database에 적용 시킴.
        public bool UpdateData()
        {
            try
            {
                this.m_DataSet = new DataSet();
                this.m_DataSet.Merge(this.m_DataTable);
                this.m_DataSet.WriteXml(sDataFolderPath + m_strFileName);

                                   
                DataSet m_OldDataSet = new DataSet();                    
                m_OldDataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);                    
                m_OldDataTable = m_OldDataSet.Tables[0];
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // db data adapter 로 부터 데이터 테이블을 다시 채운다.
        public bool RefillData()
        {
            try
            {
                this.m_DataSet.Clear();
                this.m_DataSet = new DataSet();
                this.m_DataSet.ReadXml(sDataFolderPath + m_strFileName, XmlReadMode.InferSchema);

                this.m_DataTable = this.m_DataSet.Tables[0];

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        // Data Table 에 내용을 바꾼다. 데이터는 모두 string 으로.../ 해당 매소드 실행 후 UpdateDatabase() 해주어야 Database에 적용됨.
        public bool WriteDataTable(int No, string ColName, string strValue)
        {
            try
            {
                SetCompareData(No, ColName, strValue);

                this.m_DataTable.Rows[No][ColName] = strValue;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public bool WriteDataTable(int No, string ColName, int nValue)
        {
            try
            {
                SetCompareData(No, ColName, nValue.ToString());
                     
                this.m_DataTable.Rows[No][ColName] = nValue;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }
		
		public void		RemoveDataTable(int No, string ColName)
        {
            try
            {
				if (m_DataTable.Rows.Count <= No)						return;
				if (m_DataTable.Columns.IndexOf(ColName) < 0)			return;

				this.m_DataTable.Rows[No][ColName]	= "";
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // * 마지막에 추가만 가능. 중간에 추가 불가능 
        public bool AddDataTable(int No, string ColName, string strValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ColName) == true) return false;

                if (m_DataTable.Columns.IndexOf("ID") < 0)
                {
                    m_OldDataTable.Columns.Add("ID", typeof(string));
                    m_DataTable.Columns.Add("ID", typeof(string));
                }
                if (m_DataTable.Columns.IndexOf(ColName) < 0)
                {
                    m_OldDataTable.Columns.Add(ColName, typeof(string));
                    m_DataTable.Columns.Add(ColName, typeof(string));
                }

                int nno = 0;

                while (m_DataTable.Rows.Count <= No)
                {
                    m_DataTable.TableName = "Table";
                    m_DataTable.Rows.Add();

                    nno = m_DataTable.Rows.Count;

                    m_DataTable.Rows[nno - 1]["ID"] = nno.ToString();


                    m_OldDataTable.TableName = "Table";
                    m_OldDataTable.Rows.Add();

                    nno = m_OldDataTable.Rows.Count;

                    m_OldDataTable.Rows[nno - 1]["ID"] = nno.ToString();
                }

                this.m_DataTable.Rows[No][ColName] = strValue;

                return true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("File : {0}\nMessage : {1}\nStack Trace : {2}", m_strFileName, ex.Message, ex.StackTrace), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool AddDataTable(int No, string ColName, int nValue)
        {
            try
            {
                return AddDataTable(No, ColName, nValue.ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("File : {0}\nMessage : {1}\nStack Trace : {2}", m_strFileName, ex.Message, ex.StackTrace), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public string ReadDataTable(int No, string ColName)
        {
            try
            {
                return (this.m_DataTable.Rows[No][ColName].ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return null;
        }

        /// <summary>
        /// return : -1은 실패
        /// </summary>
        /// <param name="No"></param>
        /// <param name="ColName"></param>
        /// <returns></returns>
        public int iReadDataTable(int No, string ColName)
        {
            try
            {
                return int.Parse(this.m_DataTable.Rows[No][ColName].ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return -1;
        }

        public int GetDataTableCount()
        {
            try
            {
                return (this.m_DataTable.Rows.Count);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return 0;
        }
        
        public Dictionary<string, CompareData> GetCompareData() { return m_CompData; }
        public void CompareDataClear() { m_CompData.Clear(); }
        public void SetCompareData(int No, string ColName, string strValue)
        {
            try
            {
                string key = No.ToString() + ColName;
                if (m_OldDataTable.Rows[No][ColName].Equals(strValue) == false)
                {             
                    if(m_CompData.ContainsKey(key) == false)
                        m_CompData.Add(key, new CompareData(No, ColName, strValue, m_OldDataTable.Rows[No][ColName].ToString()));
                    else
                        m_CompData[key] = new CompareData(No, ColName, strValue, m_OldDataTable.Rows[No][ColName].ToString()); 
                }
                else
                {
                    if(m_CompData.ContainsKey(key) == true)
                        m_CompData.Remove(key);
                }                
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }
        }

        #region 로그인 기능 추가

        private void Encrypt(string strElementName)
        {
            try
            {
                XmlElement xmlElementToEncrypt = xmlDocMember.GetElementsByTagName(strElementName)[0] as XmlElement;
                //해당하는 ElementName(strElementName)이 없다면 예외
                if (xmlElementToEncrypt == null)   throw new XmlException("The specified element was not found");

                EncryptedXml exEncryptedXml = new EncryptedXml();
                //aesKey 방식으로 암호화하여 암호화된 바이트 배열을 저장
                byte[] byEncryptedElement = exEncryptedXml.EncryptData(xmlElementToEncrypt, aesKey, false);

                EncryptedData edEncryptedData = new EncryptedData();
                edEncryptedData.Type = EncryptedXml.XmlEncElementUrl;

                string strEncryptionMethod;
                strEncryptionMethod = EncryptedXml.XmlEncAES256Url;

                edEncryptedData.EncryptionMethod = new EncryptionMethod(strEncryptionMethod);
                edEncryptedData.CipherData.CipherValue = byEncryptedElement;
                //xmlDocMember 메모리에 암호화된 데이터(edEncryptedData)를 저장
                EncryptedXml.ReplaceElement(xmlElementToEncrypt, edEncryptedData, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Location : Member.XML [ Encrypt Fail ] \nMsg : " + ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Decrypt()
        {
            try
            {
                XmlElement xmlEncryptedElement = xmlDocMember.GetElementsByTagName("EncryptedData")[0] as XmlElement;

                if (xmlEncryptedElement == null)   throw new XmlException("The EncryptedData element was not found.");

                EncryptedData edEncryptedData = new EncryptedData();
                //암호화된 xmlEncryptedElement -> edEncryptedData Load
                edEncryptedData.LoadXml(xmlEncryptedElement);

                EncryptedXml exEncryptedXml = new EncryptedXml();
                //동일한 key로 edEncryptedData를 해독하고 바이트 배열에 저장
                byte[] byDecryptElement = exEncryptedXml.DecryptData(edEncryptedData, aesKey);

                exEncryptedXml.ReplaceData(xmlEncryptedElement, byDecryptElement);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Location : Member.XML [ Decrypt Fail ] \nMsg : " + ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool InitEncryptedData()
        {
            try
            {
                //XML파일을 XmlDocument로 Load
                xmlDocMember.Load(sDataFolderPath + m_strFileName);

                aesKey = Aes.Create();
                aesKey.Key = Encoding.UTF8.GetBytes("pemtronpasswordpemtronpassword01");

                //암호화 m_XmlDocument -> 복호화 DataTable
                Decrypt();
                using ( MemoryStream ms = new MemoryStream())
                {
                    xmlDocMember.Save(ms);

                    ms.Position = 0;
                    this.m_DataSet = new DataSet();
                    this.m_DataSet.ReadXml(ms, XmlReadMode.InferSchema);
                    this.m_DataTable = m_DataSet.Tables["Table"];

                    ms.Position = 0;
                    DataSet m_OldDataSet = new DataSet();
                    m_OldDataSet.ReadXml(ms, XmlReadMode.InferSchema);
                    m_OldDataTable = m_OldDataSet.Tables["Table"];
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void UpdateEncryptData()
        {
            try
            {
                // 복호화 DataTable -> 암호화 XmlDocument
                UpdateData();
                xmlDocMember.Load(sDataFolderPath + m_strFileName);
                Encrypt("NewDataSet");
                xmlDocMember.Save(sDataFolderPath + m_strFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Location : {0}\nMsg : {1}", m_strFileName, ex.Message), "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion 로그인 기능 추가 (End)
    }
}
