using System;
using Events.UnityEvents;
using UnityEngine;

namespace Debugging
{
	[CreateAssetMenu(menuName = "Debug/Printer", fileName = "Printer")]
	public class Printer : ScriptableObject
	{
		#region Singleton

		private const string PrinterPath = "Setup/Debugging/Printer";
		private static Printer _instance;

		private static Printer Instance
		{
			get
			{
				if (!_instance)
				{
					_instance = Resources.Load<Printer>(PrinterPath);
				}
		
				if (_instance)
					return _instance;
				Debug.Log("No Printer found");
				_instance = CreateInstance<Printer>();
		
				return _instance;
			}
		}

		#endregion

		private const string InfoColor = "grey";
		private const string LOGColor = "grey";
		private const string WarningColor = "yellow";
		private const string ErrorColor = "red";

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
						color = InfoColor;
						break;
					case LogLevel.Log:
						color = LOGColor;
						break;
					case LogLevel.Warning:
						color = WarningColor;
						break;
					case LogLevel.Error:
						color = ErrorColor;
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