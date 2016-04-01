using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsPack.Log4net;
using ZeroMQ;

namespace zmqclient
{
	class Program
	{
		public static readonly ILog Log = LogManager.GetLogger(typeof(Program));

		static void Main(string[] args)
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
			using (var requester = new ZSocket(ZSocketType.REQ))
			{
				requester.SetOption(ZSocketOption.LINGER, 0);


				requester.Connect(targetAddr);

				using (var frameRequest = new ZFrame(action))
				{
					requester.Send(frameRequest);
				}

				var poller = ZPollItem.CreateReceiver();
				ZMessage incoming;
				ZError error;

				var resu = new StringBuilder();
				if (requester.PollIn(poller, out incoming, out error, TIME_OUT))
				{
					foreach (var reply in incoming)
					{
						resu.Append(reply.ReadString());
					}
				}
				else
				{
					if (error == ZError.ETERM)
						return $"Error {error.Number} {error.Name}: {error.Text}"; // Interrupted
					if (error != ZError.EAGAIN)
						throw new ZException(error);
				}

				return resu.ToString();
			}
		}

		static string ReadParameter(string[] args, int i)
		{
			if (args.Length <= i) {
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


		static void PrintHelp()
		{
			Console.WriteLine("zmqclient target action\n");
			Console.WriteLine("Example:");
			Console.WriteLine("zmqclient tcp://127.0.0.1:12321 sendkyc\n\n");
		}
	}
}
