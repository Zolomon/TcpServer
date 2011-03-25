using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Networking
{
	public class Client
	{
		public Client (string address, int portToListen)
		{
			Connect ("Hello", address, portToListen);
		}

		public void Connect (string msg, string address, int portToListen)
		{
			using (TcpClient c = new TcpClient (address, portToListen)) {
				
				using (NetworkStream s = c.GetStream ()) {
//					BinaryWriter w = new BinaryWriter (s);
//					w.Write (Console.ReadLine());
//					w.Flush ();
//					BinaryReader r = new BinaryReader (s);
//					Console.WriteLine (r.ReadString ());
					BinaryWriter w = new BinaryWriter(s);
					BinaryReader r = new BinaryReader(s);
					while (c.Connected) {
						w.Write(Console.ReadLine());
						w.Flush();
						
						data = new byte[1024];
						recv = s.Read (data, 0, data.Length);
						Console.WriteLine ("From {0}: {1}", client.Client.RemoteEndPoint.ToString (), Encoding.ASCII.GetString (data, 0, recv));
						if (recv == 0) {
							break;
						}
					}
				}
			}
		}
	}
}

