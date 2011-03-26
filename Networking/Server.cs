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
		Context context;

		public Server ()
		{
			context = new Context (new StatePlayerA ());
			
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
				newThread.Start ();
			}
		}

		private class ConnectionThread
		{
			public TcpListener ThreadListener { get; set; }
			private TcpClient client { get; set; }
			private NetworkStream stream { get; set; }
			private string ip { get; set; }
			private int port { get; set; }
			private byte[] data { get; set; }
			private int bytesReceived { get; set; }
			private static int connections = 0;
			private Encoding enc { get; set; }

			public ConnectionThread ()
			{
				enc = Encoding.ASCII;
			}

			public void HandleConnection ()
			{
				Connect ();
				
				Update ();
				
				Disconnect ();
			}

			private void MOD ()
			{
				string mod = "Welcome to my test server";
				data = Encoding.ASCII.GetBytes (mod);
				stream.Write (data, 0, data.Length);
			}

			public void SendMessage (byte[] msgInBytes)
			{
				stream.Write (msgInBytes, 0, msgInBytes.Length);
				stream.Flush ();
				Console.WriteLine ("Sent to {0}: {1}", client.Client.RemoteEndPoint.ToString (), enc.GetString(msgInBytes));
			}

			public byte[] ReceiveMessage ()
			{
				byte[] data = new byte[1024];
				bytesReceived = stream.Read (data, 0, data.Length);
				return data;
			}

			private void Connect ()
			{
				client = ThreadListener.AcceptTcpClient ();
				stream = client.GetStream ();
				connections++;
				Console.WriteLine ("New client accepted: {0} active connections", connections.ToString ());
			}

			private void Update ()
			{
				Encoding enc = Encoding.ASCII;
				
				while (client.Connected) {
					//Console.WriteLine ("From {0}: {1}", 
					//                   client.Client.RemoteEndPoint.ToString (), 
					//                   enc.GetString (ReceiveMessage ()));
					//Console.ReadLine ();
					
					data = ReceiveMessage();					
					if (bytesReceived == 0) {
						break;
					}
					
					Console.WriteLine (data.BytesToString());
				}
			}

			private void Disconnect ()
			{
				stream.Close ();
				client.Close ();
				connections--;
				Console.WriteLine ("Client disconnected: {0} active connections", connections);
			}
		}
	}
}
