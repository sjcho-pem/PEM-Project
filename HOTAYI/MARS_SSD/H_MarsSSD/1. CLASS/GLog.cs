using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace H_MarsSSD
{
    public class GLog
    {
        protected static LogBase seqLog = new LogBase(@"D:\" + @"Log\Sequence", "SeqLog");
		//protected static LogBase mesLog = new LogBase(@"D:\" + @"Log\MES", "MESLog");

        public static void Dispose()
        {
            seqLog.Dispose();
        }
        public static string GetLogFilePath()
        {
            return @"D:\"+ @"Log";
        }

        public static void Add(string sMsg)
        {
            try
            {
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1, true);
                if (seqLog == null)
                    return;

                string	tmpComp = string.Empty;
				int		nLine	= 0;

                if (sf != null)
                {
					nLine	= sf.GetFileLineNumber();

					if (string.IsNullOrEmpty(sf.GetFileName()) == false)
					{
						string[] fn = sf.GetFileName().Split(new char[] { '\\' });

						if (fn.Count() > 0)			tmpComp = fn[fn.Count() - 1];
						else						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
					else
					{
						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
                }
				sMsg = string.Format("[{0}],[{1}],{2}", tmpComp, nLine, sMsg);

				seqLog.Add(sMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void AddSeq(string sMsg)
        {
            try
            {
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(2, true);
                if (seqLog == null)
                    return;

                string	tmpComp = string.Empty;
				int		nLine	= 0;

                if (sf != null)
                {
					nLine	= sf.GetFileLineNumber();

					if (string.IsNullOrEmpty(sf.GetFileName()) == false)
					{
						string[] fn = sf.GetFileName().Split(new char[] { '\\' });

						if (fn.Count() > 0)			tmpComp = fn[fn.Count() - 1];
						else						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
					else
					{
						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
                }

				sMsg = string.Format("[{0}],[{1}],{2}", tmpComp, nLine, sMsg);

                seqLog.Add((int)LogBase.LOG_LEVEL.SEQ, sMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
		
// 		public static void AddMES(string sMsg, bool bXml = false)
//         {
//             try
//             {
//                 System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(2, true);
//                 if (mesLog == null)
//                     return;
// 
//                 string	tmpComp = string.Empty;
// 				int		nLine	= 0;
// 				string	tmpMsg	= "";
// 
// 				if(bXml == true)	tmpMsg = sMsg.Replace("<", "\n\t<");
// 				else				tmpMsg = sMsg;
// 
//                 if (sf != null)
//                 {
// 					nLine	= sf.GetFileLineNumber();
// 
// 					if (string.IsNullOrEmpty(sf.GetFileName()) == false)
// 					{
// 						string[] fn = sf.GetFileName().Split(new char[] { '\\' });
// 
// 						if (fn.Count() > 0)			tmpComp = fn[fn.Count() - 1];
// 						else						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
// 					}
// 					else
// 					{
// 						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
// 					}
//                 }
// 
// 				tmpMsg = string.Format("[{0}],[{1}],{2}", tmpComp, nLine, tmpMsg);
// 
//                 mesLog.Add((int)LogBase.LOG_LEVEL.MES, tmpMsg);
//             }
//             catch (Exception ex)
//             {
//                 Console.WriteLine(ex.Message);
//             }
//         }

        public static void AddSeq(Enum em)
        {
            try
            {
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1, true);
                string sMsg = string.Empty;
                if (seqLog == null)
                    return;

                string	tmpComp = string.Empty;
				int		nLine	= 0;

                if (sf != null)
                {
					nLine	= sf.GetFileLineNumber();

					if (string.IsNullOrEmpty(sf.GetFileName()) == false)
					{
						string[] fn = sf.GetFileName().Split(new char[] { '\\' });

						if (fn.Count() > 0)			tmpComp = fn[fn.Count() - 1];
						else						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
					else
					{
						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
                }
				sMsg = string.Format("[{0}],[{1}],{2}", tmpComp, nLine, em.ToString());

                seqLog.Add((int)LogBase.LOG_LEVEL.SEQ, sMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void AddErr(string sMsg)
        {
            try
            {
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1, true);
                if (seqLog == null)
                    return;

                string	tmpComp = string.Empty;
				int		nLine	= 0;

                if (sf != null)
                {
					nLine	= sf.GetFileLineNumber();

					if (string.IsNullOrEmpty(sf.GetFileName()) == false)
					{
						string[] fn = sf.GetFileName().Split(new char[] { '\\' });

						if (fn.Count() > 0)			tmpComp = fn[fn.Count() - 1];
						else						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
					else
					{
						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
                }
				sMsg = string.Format("[{0}],[{1}],{2},{3}", tmpComp, nLine, SM.SHM.GetRunLotInfo(0).WorkOrder, sMsg);

                seqLog.Add((int)LogBase.LOG_LEVEL.ERR, sMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void AddErr(Enum em)
        {
            try
            {
                System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(3, true);
                string sMsg = string.Empty;
                if (seqLog == null)
                    return;

                string	tmpComp = string.Empty;
				int		nLine	= 0;

                if (sf != null)
                {
					nLine	= sf.GetFileLineNumber();

					if (string.IsNullOrEmpty(sf.GetFileName()) == false)
					{
						string[] fn = sf.GetFileName().Split(new char[] { '\\' });

						if (fn.Count() > 0)			tmpComp = fn[fn.Count() - 1];
						else						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
					else
					{
						tmpComp = string.Format("{0}.{1}", sf.GetMethod().ReflectedType.Name, sf.GetMethod().Name);
					}
                }
				sMsg = string.Format("[{0}],[{1}],{2},{3}", tmpComp, nLine, SM.SHM.GetRunLotInfo(0).WorkOrder, em.ToString());

                seqLog.Add((int)LogBase.LOG_LEVEL.ERR, sMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}