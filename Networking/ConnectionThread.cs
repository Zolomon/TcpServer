using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace Networking
{
	public class ConnectionThread
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
			Console.WriteLine ("Sent to {0}: {1}", client.Client.RemoteEndPoint.ToString (), enc.GetString (msgInBytes));
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
								
				data = ReceiveMessage ();
				string msg = enc.GetString(data);
				Console.WriteLine ("From {0}: {1}",
				                   client.Client.RemoteEndPoint.ToString(),
				                   msg );
				SendMessage(enc.GetBytes( msg + " back at you!"));
				if (bytesReceived == 0) {
					break;
				}
				
				Console.WriteLine (data.ConvertToString ());
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
