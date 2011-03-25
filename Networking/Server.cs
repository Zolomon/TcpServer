using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text;

namespace Networking
{
	public class Server
	{
		TcpListener listener;

		public Server ()
		{
			Start ();
			
			HandleRequests ();
			
			Stop ();
		}

		public void Start ()
		{
			listener = new TcpListener (IPAddress.Any, 51111);
			listener.Start ();
			Console.WriteLine (String.Format ("Listening to {0}:{1}", IPAddress.Any.ToString (), 51111));
		}
		public void Stop ()
		{
			listener.Stop ();
		}

		public void HandleRequests ()
		{
			while (true) {
				while (!listener.Pending ()) {
					Thread.Sleep (1000);
				}
				
				ConnectionThread newConnection = new ConnectionThread ();
				newConnection.ThreadListener = this.listener;
				Thread newThread = new Thread (new ThreadStart (newConnection.HandleConnection));
				newThread.Start();
			}
		}

		private class ConnectionThread
		{
			public TcpListener ThreadListener { get; set; }
			private static int connections = 0;

			public ConnectionThread ()
			{
				
			}

			public void HandleConnection ()
			{
				int recv;
				byte[] data = new byte[1024];
				
				TcpClient client = ThreadListener.AcceptTcpClient ();
				NetworkStream ns = client.GetStream ();
				connections++;
				
				Console.WriteLine ("New client accepted: {0} active connections", connections.ToString ());
				
				string mod = "Welcome to my test server";
				data = Encoding.ASCII.GetBytes (mod);
				ns.Write (data, 0, data.Length);
				
				while (client.Connected) {
					data = new byte[1024];
					recv = ns.Read (data, 0, data.Length);
					Console.WriteLine("From {0}: {1}", 
					                  client.Client.RemoteEndPoint.ToString(), 
					                  Encoding.ASCII.GetString(data, 0, recv));
					if (recv == 0) {
						break;
					}
					
					ns.Write (data, 0, recv);
				}
				ns.Close ();
				client.Close ();
				connections--;
				Console.WriteLine ("Client disconnected: {0} active connections", connections);
			}
		}
	}
}
