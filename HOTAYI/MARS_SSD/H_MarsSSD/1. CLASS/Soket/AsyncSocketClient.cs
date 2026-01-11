using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace H_MarsSSD
{
    class AsyncSocketClient
    {
        protected delegate void ReceiveDataHandler(object sender, byte[] receivedData);
        protected event ReceiveDataHandler ReceiveData;
        private void OnReceiveData(byte[] receivedData)
        {
            if (ReceiveData != null)
            {
                ReceiveData(this, receivedData);
            }
        }

        protected delegate void ConnectCompletedHandler(object sender, bool ConnectState);
        protected event ConnectCompletedHandler ConnectComplete;
        private void OnConnectComplete(bool ConnectState)
        {
            if (ConnectComplete != null)
            {
                ConnectComplete(this, ConnectState);
            }
        }

        #region Variables
        private const int BUFFFER_COUNT = 1024;
        private Socket SocketClient;
        private byte[] byBuffer;

        protected int SocketPort { get; set; }
        protected string IpAddress { get; set; }
        protected bool IsConnected
        {
            get
            {
                if (SocketClient == null)
                    return false;

                return SocketClient.Connected;
            }
        }
        #endregion

        protected bool Connect()
        {
            SocketClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IpAddress), SocketPort);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = ipep;
            args.Completed += new EventHandler<SocketAsyncEventArgs>(ConnectCompleted);

            return SocketClient.ConnectAsync(args);
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            SocketClient = sender as Socket;

            if (SocketClient == null)
            {
                OnConnectComplete(false);
                return;
            }

            if (SocketClient.Connected)
            {
                SocketAsyncEventArgs _receiveArgs = new SocketAsyncEventArgs();
                byBuffer = new byte[BUFFFER_COUNT];
                _receiveArgs.SetBuffer(byBuffer, 0, BUFFFER_COUNT);
                _receiveArgs.UserToken = SocketClient;
                OnConnectComplete(true);
                disposed = false;

                _receiveArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ReceiveArgsCompleted);
                SocketClient.ReceiveAsync(_receiveArgs);
            }
            else
            {
                OnConnectComplete(false);
                Dispose();
            }
        }

        private void ReceiveArgsCompleted(object sender, SocketAsyncEventArgs e)
        {
            Socket client = sender as Socket;

            if (client.Connected && e.BytesTransferred > 0)
            {
                byte[] receivedBuffer = e.Buffer;

                OnReceiveData(receivedBuffer);                

                Array.Clear(receivedBuffer, 0, receivedBuffer.Length);
                e.SetBuffer(receivedBuffer, 0, BUFFFER_COUNT);
                client.ReceiveAsync(e);
            }
            else
            {
                OnConnectComplete(false);
                Dispose();
            }
        }

        protected bool Send(string sData)
        {
            try
            {
                if (SocketClient != null && SocketClient.Connected)
                {
                    SocketAsyncEventArgs args = new SocketAsyncEventArgs();

                    byte[] packet = Encoding.Default.GetBytes(sData);
                    args.SetBuffer(packet, 0, packet.Length);
					args.Completed += (sender, e) => OnSendCompleted(e);

					//비동기 작업이 아닌 즉시 완료된 경우 직접 호출하여 결과처리
					bool bAsyncReturn = SocketClient.SendAsync(args);
					if (bAsyncReturn == false)
						OnSendCompleted(args, bAsyncReturn);

					return true;
                }
            }
            catch (System.ArgumentNullException ex)
            {
                string s = "AsyncSocketClient_Send" + ex.Message;
                Console.WriteLine(s);

                return false;
            }

            return false;
        }

        protected bool Send(byte[] byData)
        {
            try
            {
                if (SocketClient != null && SocketClient.Connected)
                {
                    SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                    args.SetBuffer(byData, 0, byData.Length);
					args.Completed += (sender, e) => OnSendCompleted(e);

					//비동기 작업이 아닌 즉시 완료된 경우 직접 호출하여 결과처리
					bool bAsyncReturn = SocketClient.SendAsync(args);
					if (bAsyncReturn == false)
						OnSendCompleted(args, bAsyncReturn);

					return true;
                }
            }
            catch (System.ArgumentNullException ex)
            {
                string s = "AsyncSocketClient_Send" + ex.Message;
                Console.WriteLine(s);

                return false;
            }

            return false;
        }
		
		private void OnSendCompleted(SocketAsyncEventArgs e, bool bAsyncReturn = true)
		{
			try
			{
				e.Dispose();
			}
			catch (System.ArgumentNullException ex)
			{
				string s = "_sendArgs_Completed" + ex.Message;
				Console.WriteLine(s);
			}
		}

        #region Finalizable and Dispose

        private bool disposed = false;
        protected void Dispose()
        {
            this.CleanUp(true);
            GC.SuppressFinalize(this);
        }

        private void CleanUp(bool disposing)
        {
            if (this.disposed == false)
            {
                if (disposing)
                {

                }

                DisconnectAsyncSocketClient();
            }

            disposed = true;
        }

        private void DisconnectAsyncSocketClient()
        {
            if (SocketClient != null)
            {
                if (SocketClient.Connected)
                    SocketClient.Disconnect(false);

                SocketClient.Dispose();
                SocketClient = null;
            }
        }

        //~SocketClientBase()
        //{
        //    this.CleanUp(false);
        //}

        #endregion



    }
}
