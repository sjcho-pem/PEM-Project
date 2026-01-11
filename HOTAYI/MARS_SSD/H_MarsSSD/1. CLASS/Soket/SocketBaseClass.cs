using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace H_MarsSSD
{
    public class SocketBaseClass
    {
        #region [ Variables ]
        public bool ApplicationClose = false;

        internal int nSocketTimeOut = 2000;

        //Server Port        
        internal const ushort ServerRecvPort1 = 20000;  // 검사제어가 융착기로부터 받기
        internal const ushort ServerRecvPort2 = 30000; // 검사제어가 검사Top 으로부터 받기
        internal const ushort ServerRecvPort3 = 40000; // 검사제어가 검사Btm 으로부터 받기

        internal const ushort ServerSendPort1 = 21000; // 검사제어가 융착기로 보내기
        internal const ushort ServerSendPort2 = 31000; // 검사제어가 검사Top으로 보내기
        internal const ushort ServerSendPort3 = 41000; // 검사제어가 검사Btm으로 보내기

        //Client Port        
        internal const ushort ClientRecvPort1 = 21000;  // 융착기가 검사제어 로부터 받기       
        internal const ushort ClientSendPort1 = 20000;  // 융착기가 검사제어로 보내기

        internal IPAddress BROADCAST = IPAddress.Parse("192.168.0.255");

        internal delegate void ReceiveMessageHandler(object sender, ushort Port);
        internal event ReceiveMessageHandler ReceiveMessageHandled;
        #endregion


        internal virtual void MethodReceiveServer(ushort Port, int TimeOutValue)
        {
            ApplicationClose = false;
            int recv = 0;
            byte[] data = new byte[1024];
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);

            Socket server = null;
            while (true)
            {
                Thread.Sleep(1);

                server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                server.Bind(localEndPoint);
                EndPoint ep = (EndPoint)localEndPoint;

                try
                {
                    while (true)
                    {
                        Thread.Sleep(1);

                        if (this.ApplicationClose)
                        {
                            break;
                        }
                        if (server.Available <= 0)
                        {
                            continue;
                        }

                        data = new byte[1024];
                        try
                        {
                            recv = server.ReceiveFrom(data, ref ep);
                        }
                        catch (SocketException sex)
                        {
                            if (sex.SocketErrorCode == SocketError.TimedOut)
                                continue;
                        }

                        if (data[0] != '^') //Start byte 체크
                            continue;

                        if (ReceiveMessageHandled != null)
                        {
                            ReceiveMessageHandled(data, Port);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("MethodReceiveServer Exception: [{0}]", ex.StackTrace));
                }

                server.Close();
                server = null;

                if (this.ApplicationClose)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SendData">전송 메세지</param>
        /// <param name="SendPort">전송 포트번호</param>
        /// <returns></returns>
        internal bool MethodSendData(string SendData, ushort SendPort)
        {
            IPEndPoint endPoint = new IPEndPoint(BROADCAST, SendPort);

            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //s.EnableBroadcast = true;
            byte[] msg = Encoding.UTF8.GetBytes(SendData);
            s.SendTo(msg, endPoint);
            s.Close();

            return true;
        }

        internal bool MethodSendData(byte[] SendData, ushort SendPort)
        {
            if (SendData == null)
                return false;

            try
            {
                IPEndPoint endPoint = new IPEndPoint(BROADCAST, SendPort);

                Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                s.EnableBroadcast = true;
                s.SendTo(SendData, endPoint);
                s.Close();
              
                return true;
            }
            catch (SocketException exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }

        }
    }
}

