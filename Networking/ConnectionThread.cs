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
		private BinaryWriter writer { get; set; }
		private BinaryReader reader { get; set; }
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
			
			writer = new BinaryWriter (client.GetStream ());
			reader = new BinaryReader (client.GetStream ());
			
			Update ();
			
			Disconnect ();
		}
		public void SendMessage (string msg)
		{
			writer.Write (msg);
			writer.Flush ();
			Console.WriteLine ("Sent to {0}: {1}", client.Client.RemoteEndPoint.ToString (), msg);
		}

		public string ReceiveMessage ()
		{
			string msg = reader.ReadString ();
			Console.WriteLine ("From {0}: {1}", client.Client.RemoteEndPoint.ToString (), msg);
			return msg;
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
			// Send welcome message
			SendMessage ("Welcome");
			
			while (client.Connected) {
				
				// Get position
				string position = ReceiveMessage ();
				
				// Send position accepted
				SendMessage (position + " received.");
				if (bytesReceived == 0) {
					break;
				}
			}
		}

		private void Disconnect ()
		{
			reader.Close ();
			writer.Close ();
			stream.Close ();
			client.Close ();
			connections--;
			Console.WriteLine ("Client disconnected: {0} active connections", connections);
		}
	}
}
