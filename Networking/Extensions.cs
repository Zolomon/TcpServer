using System;

namespace Networking
{
	public static class Extensions
	{
		public static string ConvertToString (this byte[] bytes)
		{
			string s = "[";
			for (int i = 0; i < bytes.Length - 1; i++) {
				s += bytes[i].ToString () + ", ";
			}
			return s + bytes[bytes.Length - 1].ToString() + "]";
		}
	}
}

