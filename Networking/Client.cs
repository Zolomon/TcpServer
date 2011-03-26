using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace Networking
{
	public class Client
	{
		Context context;
		public Client (string address, int portToListen)
		{
			context = new Context(new StatePlayerA());
			Connect ("Hello", address, portToListen);
		}

		public void Connect (string msg, string address, int portToListen)
		{
			using (TcpClient c = new TcpClient (address, portToListen)) {
				if (c.Connected) {
					Console.WriteLine ("Connection to {0}:{1} established", address, portToListen);
				}
				using (NetworkStream s = c.GetStream ()) {
					BinaryWriter w = new BinaryWriter(s);
					BinaryReader r = new BinaryReader(s);
					byte[] data = new byte[1024];
					int recv;
					
					while (c.Connected) {
						byte[] state = new byte[10];
						state[0] = 1;
						state[3] = 0;
						w.Write(state);
						//w.Write();
						w.Flush();
						
						data = new byte[1024];
						recv = s.Read (data, 0, data.Length);
						Console.WriteLine ("From {0}: {1}", c.Client.RemoteEndPoint.ToString (), Encoding.ASCII.GetString (data, 0, recv));
						if (recv == 0) {
							break;
						}
					}
				}
			}
		}
	}
}

