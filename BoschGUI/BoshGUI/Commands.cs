using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using System.Net;
using System.Net.Sockets;

namespace BoshGUI {
	public static class Commands {
		public static UdpClient client;

		public static void Start () {
			client = new UdpClient ();
			client.Client.Bind (new IPEndPoint (IPAddress.Any, 2069));
			ar = client.BeginReceive (Wolololo, client);
		}
		private static IAsyncResult ar;
		private static void Wolololo (IAsyncResult _ar) {
			UdpClient client = (UdpClient)_ar.AsyncState;
			IPEndPoint end = new IPEndPoint (IPAddress.Any, 2069);
			byte[] buf = client.EndReceive (_ar, ref end);
			System.Diagnostics.Debug.WriteLine (BitConverter.ToDouble (buf, 0));


			ar = client.BeginReceive (Wolololo, client);

		}
		public static void Init (int nothing) {

		}
		public static void Turn (int r) {

		}

		public static void Stop (int nothing) {

		}

		public static void Speed (int s) {

		}

		public static void Pause (int delay) {
			Task.Delay (delay * 1000);
		}

		public static void Base (int nothing) {

		}

	}
}
