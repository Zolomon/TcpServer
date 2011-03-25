using System;
using System.Threading;
using System.Net;

namespace Networking
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Client c;
			Server s;
			//Server s = new Server();
			if (args[0].Equals ("c")) {
				c = new Client (args[1].ToString (), Convert.ToInt32 (args[2]));
			} else if (args[0].Equals ("s")) {
				s = new Server ();
			}
		}
	}
}

