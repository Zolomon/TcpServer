using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace Networking
{
	public class Client
	{
		private BinaryWriter writer { get; set; }
		private BinaryReader reader { get; set; }
		private TcpClient client { get; set; }
		private NetworkStream stream { get; set; }
		private Context context;
		private int bytesReceived;

		private static string ip { get; set; }
		private static int port { get; set; }

		public Client (string address, int portToListen)
		{
			context = new Context (new StatePlayerA ());
			
			Connect ("Hello", address, portToListen);
		}

		public void Connect (string msg, string address, int portToListen)
		{
			using (client = new TcpClient (address, portToListen)) {
				if (client.Connected) {
					Console.WriteLine ("Connection to {0}:{1} established", address, portToListen);
				}
				
				using (stream = client.GetStream ()) {
					writer = new BinaryWriter (stream);
					reader = new BinaryReader (stream);
					
					Update ();
				}
			}
		}

		private void Update ()
		{
			string msg;
				
			while (client.Connected) {
				// Get current state.. 
				Console.WriteLine ("From {0}:{1}: {2}", ip, port, ReceiveMessage ());
				
				// Fetch command
				Console.Write("Enter command ('exit' to exit): ");				
				if ((msg = Console.ReadLine ()) == "exit") { 
					// Exit
					break;
				} else {
					// Send command
					SendMessage(msg);
				}
			}
		}

		public void SendMessage (string msg)
		{
			writer.Write (msg);
			writer.Flush ();
			Console.WriteLine ("Msg sent to {0}:{1}: {1}", ip, port, msg);
		}

		public string ReceiveMessage ()
		{
			return reader.ReadString();
		}
	}
}

