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
					BinaryWriter w = new BinaryWriter (s);
					w.Write (msg);
					w.Flush ();
					BinaryReader r = new BinaryReader (s);
					Console.WriteLine (r.ReadString ());
				}
			}
		}
	}
}

