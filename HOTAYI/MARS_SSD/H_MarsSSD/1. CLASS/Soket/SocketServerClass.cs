using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace H_MarsSSD
{
    public class SocketServerClass : SocketBaseClass
    {
        #region [ Variables ]
        public delegate void EventClientDataClassHandler(object sender, ServerReceiveCode code);
        public event EventClientDataClassHandler EventClientDataClassHandled;
        
        private Thread thServer1 = null;
        private Thread thServer2 = null;
        #endregion

        public SocketServerClass(ushort Port1, ushort Port2)
        {
            this.ReceiveMessageHandled += new ReceiveMessageHandler(SocketServerClass_ReceiveMessageHandled);
            thServer1 = new Thread(() => base.MethodReceiveServer(Port1, nSocketTimeOut));
            thServer1.Start();
            thServer2 = new Thread(() => base.MethodReceiveServer(Port2, nSocketTimeOut));
            thServer2.Start();
        }

        public bool MethodCommandSendToClient(InspectData Data, EQP Equip)
        {
            /// Port 변경 요망
            ///
            if(Equip == EQP.TOP)
                MethodSendData(StructParsing.MethodObjectToByteSerializer(Data), ServerSendPort1);
            else if(Equip == EQP.BTM)
                MethodSendData(StructParsing.MethodObjectToByteSerializer(Data), ServerSendPort2);
            return true;
        }

        void SocketServerClass_ReceiveMessageHandled(object sender, ushort Port)
        {
            if (EventClientDataClassHandled == null)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("SocketServerClass_ReceiveMessageHandled EventDataHandled: [{0}={1}]", sender.GetType(), sender));
                return;
            }

            if (sender is byte[])
            {
                byte[] bytes = (byte[])sender;
                object obj;

                if (Port == ServerRecvPort1)                
                    obj = StructParsing.MethodByteToObjectDeserialize(bytes, typeof(InspectData));                
                else
                    obj = null;

                if (obj == null)
                    return;

                EventClientDataClassHandled(obj, ServerReceiveCode.None);
            }
            else
            {
                System.Diagnostics.Trace.WriteLine(string.Format("SocketServerClass_ReceiveMessageHandled: Not Byte[] [{0}={1}]", sender.GetType(), sender));
            }
        }
        
        public void Dispose()
        {
            thServer1.Abort();
            thServer2.Abort();
        }
    }
}

