using System;
using Events.UnityEvents;
using UnityEngine;

namespace Debugging
{
	[CreateAssetMenu(menuName = "Debug/Printer", fileName = "Printer")]
	public class Printer : ScriptableObject
	{
		#region Singleton

		private const string PRINTER_PATH = "Setup/Debugging/Printer";
		private static Printer _instance;

		private static Printer Instance
		{
			get
			{
				if (!_instance)
				{
					_instance = Resources.Load<Printer>(PRINTER_PATH);
				}

				if (_instance)
					return _instance;
				Debug.Log("No Printer found");
				_instance = CreateInstance<Printer>();

				return _instance;
			}
		}

		#endregion

		private const string INFO_COLOR = "grey";
		private const string LOG_COLOR = "grey";
		private const string WARNING_COLOR = "yellow";
		private const string ERROR_COLOR = "red";

		[SerializeField] private StringUnityEvent onPrint;
		[SerializeField] private bool showTimestamp;
		[SerializeField] private bool showTimeSinceStart;
		[SerializeField] private bool showLogLevel;
		[SerializeField] private LogLevel minimumLogLevel = LogLevel.Info;

		public static void Log(LogLevel logLevel, object message)
		{
			if ((int) logLevel < (int) Instance.minimumLogLevel)
				return;
			var prefix = string.Empty;
			if (Instance.showTimeSinceStart)
				prefix += $"[{Math.Round(Time.time, 2)}s] ";
			if (Instance.showTimestamp)
				prefix += $"[{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}] ";
			if (Instance.showLogLevel)
			{
				var color = string.Empty;
				switch (logLevel)
				{
					case LogLevel.Info:
						color = INFO_COLOR;
						break;
					case LogLevel.Log:
						color = LOG_COLOR;
						break;
					case LogLevel.Warning:
						color = WARNING_COLOR;
						break;
					case LogLevel.Error:
						color = ERROR_COLOR;
						break;
				}
				prefix += $"{logLevel.Colored(color)}: ";
			}

			Instance.onPrint.Invoke(prefix + message);
		}

		public static void Log(object message) => Log(LogLevel.Log, message);
		public static void LogWarning(object message) => Log(LogLevel.Warning, message);
		public static void LogError(object message) => Log(LogLevel.Error, message);
	}

	public static class PrinterHelper
	{
		public static void Log(this object message, LogLevel logLevel = LogLevel.Log) => Printer.Log(logLevel, message);
		public static string Colored(this object message, string color) => $"<color={color}>{message}</color>";
		public static string Bold(this object message) => $"<b>{message}</b>";
		public static string Italic(this object message) => $"<i>{message}</i>";
	}
}