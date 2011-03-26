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
			while (client.Connected) {
				SendMessage (new byte[] { 1, 0, 0, 0, 0, 1, 0, 0, 0, 0 });
				Console.WriteLine ("From {0}:{1}: {2}", ip, port, GetMsg (ReceiveMessage ()));
				Console.Write("'Exit' to quit: ");
				if (Console.ReadLine () == "exit") {
					break;
				}
			}
		}

		public void SendMessage (byte[] msgInBytes)
		{
			writer.Write (msgInBytes);
			writer.Flush ();
			Console.WriteLine ("Msg sent to {0}:{1}: {1}", ip, port, Encoding.ASCII.GetString (msgInBytes));
		}

		public byte[] ReceiveMessage ()
		{
			byte[] data = new byte[1024];
			bytesReceived = stream.Read (data, 0, data.Length);
			return data;
		}

		public string GetMsg (byte[] msg)
		{
			return Encoding.ASCII.GetString (msg);
		}
	}
}

