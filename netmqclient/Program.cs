using log4net;
using NetMQ;
using NetMQ.Sockets;
using System;

namespace netmqclient
{
	internal class Program
	{
		public static readonly ILog Log = LogManager.GetLogger(typeof(Program));

		private static void Main(string[] args)
		{
			try
			{
				var targetAddr = ReadParameter(args, 0);
				var action = ReadParameter(args, 1);

				Log.Info($"Requesting {targetAddr} for {action}...");

				var response = SendRequest(targetAddr, action);

				Log.Info($"Got:\n{response}");
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}
		}

		private static readonly TimeSpan TIME_OUT = TimeSpan.FromSeconds(15);

		private static string SendRequest(string targetAddr, string action)
		{
			using (var client = new RequestSocket(targetAddr))
			{
				client.SendFrame(action);

				string resp;
				client.TryReceiveFrameString(TIME_OUT, out resp);

				return resp;
			}
		}

		private static string ReadParameter(string[] args, int i)
		{
			if (args.Length <= i)
			{
				PrintHelp();
				throw new ArgumentException("Invalid argument " + i);
			}

			if (string.IsNullOrWhiteSpace(args[i]))
			{
				PrintHelp();
				throw new ArgumentException("Invalid argument " + i);
			}

			return args[i].Trim();
		}

		private static void PrintHelp()
		{
			Console.WriteLine("netmqclient target action\n");
			Console.WriteLine("Example:");
			Console.WriteLine("netmqclient tcp://127.0.0.1:12321 sendkyc\n\n");
		}
	}
}