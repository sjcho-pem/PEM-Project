using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace H_MarsSSD
{
    public class SocketClientClass : SocketBaseClass
    {
        #region [ Variables ]
        public delegate void EventClientSocketClassHandler(object sender);
        public event EventClientSocketClassHandler EventClientSocketClassHandled;

        private Thread thClient = null;
        #endregion



        public SocketClientClass()
        {            
            this.ReceiveMessageHandled += new ReceiveMessageHandler(SocketClientClass_ReceiveMessageHandled);
            thClient = new Thread(() => base.MethodReceiveServer(ClientRecvPort1, nSocketTimeOut));
            thClient.Start();
        }

        /// <summary>
        /// 융착기제어 ->> 검사제어 전송
        /// </summary>
        /// <param name="command"></param>
        public bool MethodCommandSendToServer(object Data)
        {
            if(Data is InspectData)
            {
                InspectData ppdc = (InspectData)Data;
                MethodSendData(StructParsing.MethodObjectToByteSerializer(ppdc), ClientSendPort1);
            }

            return true;
        }

        void SocketClientClass_ReceiveMessageHandled(object sender, ushort Port)
        {
            if (EventClientSocketClassHandled == null)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("SocketClientClass_ReceiveMessageHandled EventDataHandled: [{0}={1}]", sender.GetType(), sender));
                return;
            }

            if (sender is byte[])
            {
                byte[] bytes = (byte[])sender;
                string recvData = Encoding.UTF8.GetString(bytes, 0, bytes.Length);                
                object obj = StructParsing.MethodByteToObjectDeserialize(bytes, typeof(InspectData));

               
                EventClientSocketClassHandled(obj);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(string.Format("SocketClientClass_ReceiveMessageHandled: Not Byte[] [{0}={1}]", sender.GetType(), sender));
            }
        }
    }
}
