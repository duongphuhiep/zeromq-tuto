using log4net;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ToolsPack.Displayer;
using ToolsPack.Log4net;

namespace netmqEchoServer
{
	/// <summary>
	/// echo server
	/// </summary>
	class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

		private static void Main(string[] args)
		{
			try
			{
				Log.Info("Start serivce " + args.Display() + "..");
				Log.InfoFormat("Environment.CurrentDirectory = {0}", Environment.CurrentDirectory);
				Log.InfoFormat("Directory.CurrentDirectory = {0}", Directory.GetCurrentDirectory());
				var assemblyLocation = Assembly.GetExecutingAssembly().Location;
				Log.InfoFormat("ExecutingAssembly.Location = {0}", assemblyLocation);
				Log.InfoFormat("ExecutingAssembly.Version = {0}", FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion);

				StartListen();
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}
			Console.ReadLine();
		}

		private static void StartListen()
		{
			var listenerAddr = ConfigReader.Read("ControllerSocket", "tcp://127.0.0.1:12321");

			using (var server = new ResponseSocket(listenerAddr))
			{
				while (true)
				{
					var action = server.ReceiveFrameString();
					string response;
					try
					{
						response = DoWork(action);
					}
					catch (Exception ex)
					{
						Log.Error(ex);
						response = ex.Message;
					}
					server.SendFrame(response);
				}
			}
		}

		private static string DoWork(string action)
		{
			return $"{action} done!";
		}
	}
}