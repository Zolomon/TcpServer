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
		
	}
}
