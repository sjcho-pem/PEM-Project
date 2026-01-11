using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Windows;

namespace H_MarsSSD
{
	public class AsyncSocketServer : IDisposable
	{
		#region Internal Member Variables

		private const int BUFFFER_COUNT = 1024;

		private Socket ServerSocket;
		private List<Socket> ClientSocketList;
		private byte[] byBuffer;
		#endregion


		public delegate void DataReceiveStrHandler(object sender, string strIP, int nPort, string receivedData);
		public event DataReceiveStrHandler ServerDataStrReceived;

		public delegate void DataReceiveByHandler(object sender, string strIP, int nPort, byte[] receivedData, int nDataLength);
		public event DataReceiveByHandler ServerDataByReceived;

		public delegate void ConnectReceiveHandler(object sender, string strIP, int nPort, bool IsConnect);
		public event ConnectReceiveHandler ConnectReceive;

		public AsyncSocketServer()
		{
			ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			ClientSocketList = new List<Socket>();
		}

		public void Bind(int nPort)
		{
			try
			{
				if (ServerSocket != null)
				{
					IPEndPoint point = new IPEndPoint(IPAddress.Any, /*SocketPort*/nPort);

					if (ServerSocket.LocalEndPoint == null)
						ServerSocket.Bind(point);
				}
			}
			catch (System.ArgumentNullException ex)
			{
				UI.Logs.Write(LOGPART.EX , ex.Message);
				Console.WriteLine(ex.Message);
			}
			catch (System.Net.Sockets.SocketException ex)
			{
				UI.Logs.Write(LOGPART.EX , ex.Message);
				Console.WriteLine(ex.Message);
			}
			catch (System.ObjectDisposedException ex)
			{
				UI.Logs.Write(LOGPART.EX , ex.Message);
				Console.WriteLine(ex.Message);
			}
			catch (System.Security.SecurityException ex)
			{
				UI.Logs.Write(LOGPART.EX , ex.Message);
				Console.WriteLine(ex.Message);
			}
		}

		public void Listen(int backlog)
		{
			try
			{
				ServerSocket.Listen(backlog);
				SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();

				eventArgs.Completed += new EventHandler<SocketAsyncEventArgs>(ClientAccept_Completed);
				ServerSocket.AcceptAsync(eventArgs);
			}
			catch (Exception ex)
			{
				UI.Logs.Write(LOGPART.EX , ex.Message);
				Console.WriteLine(ex.Message);
			}
		}

		#region Socket Event

		private void ClientAccept_Completed(object sender, SocketAsyncEventArgs e)
		{
			try
			{
				Socket client  = e.AcceptSocket;
				//int    nindex  = -1;

				if		(client.RemoteEndPoint == null  ||  client.LocalEndPoint == null)
				{
                    if (ServerSocket.Connected ==false)  return;

					e.AcceptSocket = null;
					ServerSocket.AcceptAsync(e);
					return;
				}
				else if (ClientSocketList != null)
				{
					ClientSocketList.Add(client);

					SocketAsyncEventArgs args = new SocketAsyncEventArgs();
					byBuffer = new byte[BUFFFER_COUNT];
					args.SetBuffer(byBuffer, 0, BUFFFER_COUNT);
					args.UserToken = ClientSocketList;

					args.Completed += new EventHandler<SocketAsyncEventArgs>(Receive_Completed);
					client.ReceiveAsync(args);

					string	strip	= ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
					int		nPort	= ((IPEndPoint)client.LocalEndPoint).Port;
					OnConnectReceive(strip, nPort, true);         // 연결 상태 갱신
				}
			}
			catch (System.Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				Console.WriteLine(ex.Message);
			}

			e.AcceptSocket = null;
			ServerSocket.AcceptAsync(e);
		}

		private void Receive_Completed(object sender, SocketAsyncEventArgs e)
		{
			try
			{
				Socket client = sender as Socket;

				if (client.Connected && e.BytesTransferred > 0)
				{
					int		nDataLength		= Convert.ToInt32(e.BytesTransferred);
					byte[]	receivedBuffer	= e.Buffer;
					string	tempData		= Encoding.Default.GetString(receivedBuffer);
					string	strIP			= ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
					int		nPort			= ((IPEndPoint)client.LocalEndPoint).Port;
				
					OnServerDataReceived(strIP, nPort, receivedBuffer, nDataLength);
					OnServerDataReceived(strIP, nPort, tempData);

					for (int i = 0; i < receivedBuffer.Length; i++)
					{
						receivedBuffer[i] = 0;
					}

					e.SetBuffer(receivedBuffer, 0, BUFFFER_COUNT);
					client.ReceiveAsync(e);
				}
				else
				{
					if (client== null)
					{
						return;
					}

                    if (client.Connected == true )
                    {
                        string strIp = ((IPEndPoint)client.RemoteEndPoint).Address.ToString();
                        int nPort = ((IPEndPoint)client.LocalEndPoint).Port;
                        OnConnectReceive(strIp, nPort, false);

                        client.Disconnect(false);
                    }
                    client.Dispose();

                    if (ClientSocketList.Count > 0 && ClientSocketList.Contains(client) == true)
                        ClientSocketList.Remove(client);

                }
            }
            catch (Exception ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
			}
		}

		#endregion

		public bool Send(Socket	Client, byte[] byData)
		{
			try
			{
				if (Client.Connected == false)
				{
					string	strIp   = ((IPEndPoint)Client.RemoteEndPoint).Address.ToString();
					int		nPort	= ((IPEndPoint)Client.LocalEndPoint).Port;

					Client.Disconnect(false);
					Client.Dispose();

					if (ClientSocketList.Count > 0)			ClientSocketList.Remove(Client);
					OnConnectReceive(strIp, nPort, false);
					return false;
				}
				else
				{
					lock (Client)
					{
						SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
						_sendArgs.SetBuffer(byData, 0, byData.Length);
						_sendArgs.Completed += (sender, e) => OnSendCompleted(e);

						//비동기 작업이 아닌 즉시 완료된 경우 직접 호출하여 결과처리
						bool bAsyncReturn = Client.SendAsync(_sendArgs);
						if (bAsyncReturn == false)
							OnSendCompleted(_sendArgs, bAsyncReturn);
						
						return true;
					}
				}
			}
			catch (System.ArgumentNullException ex)
			{
				string s = "AsyncSocketServer_Send" + ex.Message;
				Console.WriteLine(s);
				return false;
			}
		}

		public void Send(string sData)
		{
			try
			{
				byte[] packet = Encoding.Default.GetBytes(sData);
				Send(packet);
			}
			catch (System.ArgumentNullException ex)
			{
				string s = "AsyncSocketServer_Send" + ex.Message;
				Console.WriteLine(s);
			}
		}

		public void Send(byte[] byData)
		{
			try
			{
				foreach (Socket client in new List<Socket>(ClientSocketList))
				{
					Send(client, byData);
				}
			}
			catch (System.ArgumentNullException ex)
			{
				string s = "AsyncSocketServer_Send" + ex.Message;
				Console.WriteLine(s);
			}
		}

		public bool Send(string strIP, byte[] byData)
		{
			try
			{
				foreach (Socket client in new List<Socket>(ClientSocketList))
				{
					string strIpPort	= string.Format("{0}:{1}", ((IPEndPoint)client.RemoteEndPoint).Address, ((IPEndPoint)client.LocalEndPoint).Port);

					if (strIpPort == strIP)
					{
						return Send(client, byData);
					}
				}
			}
			catch (System.ArgumentNullException ex)
			{
				G.AlmMng.ADD(ALM.ERROR_EXCEPTION, string.Format("Message : {0}\nStack trace : {1}", ex.Message, ex.StackTrace));
				string s = "AsyncSocketServer_Send" + ex.Message;
				Console.WriteLine(s);
			}
			
			return false;
		}

		public bool Send(string strIP, string sData)
		{
			try
			{
				foreach (Socket client in new List<Socket>(ClientSocketList))
				{
					if (((IPEndPoint)client.RemoteEndPoint).Address.ToString() == strIP)
					{
						byte[] packet = Encoding.Default.GetBytes(sData);
						return Send(client, packet);
					}
				}
			}
			catch (System.ArgumentNullException ex)
			{
				string s = "AsyncSocketServer_Send" + ex.Message;
				Console.WriteLine(s);
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

		protected void OnServerDataReceived(string strIP, int nPort, string receivedData)
		{
			if (ServerDataStrReceived != null)
			{
				ServerDataStrReceived(this, strIP, nPort, receivedData);
			}
		}

		protected void OnServerDataReceived(string strIP, int nPort, byte[] receivedData, int nLength)
		{
			if (ServerDataByReceived != null)
			{
				ServerDataByReceived(this, strIP, nPort, receivedData, nLength);
			}
		}

		protected void OnConnectReceive(string strIP, int nPort, bool Isconnect)
		{
			if (ConnectReceive != null)
			{
				ConnectReceive(this, strIP, nPort, Isconnect);
			}
		}

		#region Finalizable and Dispose

		private bool disposed = false;
		public void Dispose()
		{
			this.CleanUp(true);
			GC.SuppressFinalize(this);
		}

		private void CleanUp(bool disposing)
		{
			try
			{
				if (this.disposed == false)
				{
					if (disposing)
					{

					}

					foreach (Socket client in new List<Socket>(ClientSocketList))
					{

						client.Disconnect(false);
						client.Dispose();
					}

					//for (int i = 0; i < ClientSocketList.Count; i++)
					//{
					//	if (ClientSocketList[i] == null)
					//		return;

					//	if (ClientSocketList[i].Connected)
					//	{
					//		ClientSocketList[i].Shutdown(SocketShutdown.Both);
					//		ClientSocketList[i].Disconnect(false);
					//	}
					//	ClientSocketList[i].Dispose();
					//}

					if (ServerSocket == null)
						return;
					
					ServerSocket.Close(50);
					ServerSocket.Dispose();
				}

				disposed = true;
			}
			catch (Exception ex)
			{
				string s = "AsyncSocketServer_CleanUp" + ex.Message;
				Console.WriteLine(s);
				//throw;
			}
			
		}

		~AsyncSocketServer()
		{
			//this.CleanUp(false);
		}

		#endregion
	}
}