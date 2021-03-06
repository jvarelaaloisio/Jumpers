using Events.UnityEvents;
using UnityEngine;

namespace Debug
{
	[CreateAssetMenu(menuName = "Debug/Printer", fileName = "Printer")]
	public class Printer : ScriptableObject
	{
		#region Singleton

		const string PRINTER_PATH = "Setup/Debug/Printer";
		private static Printer _instance;

		private static Printer Instance
		{
			get
			{
				if (!_instance)
				{
					_instance = Resources.Load<Printer>(PRINTER_PATH);
				}

				if (!_instance)
				{
					UnityEngine.Debug.Log("No Printer found");
					_instance = CreateInstance<Printer>();
				}

				return _instance;
			}
		}

		#endregion

		[SerializeField] private StringUnityEvent onPrint;

		public static void Log(object message) => Instance.onPrint.Invoke(message.ToString());
	}

	public static class PrinterHelper
	{
		public static void Log(this object message) => Printer.Log(message);
		public static string Colored(this object message, string color) => $"<color={color}>{message}</color>";
		public static string Bold(this object message) => $"<b>{message}</b>";
		public static string Italic(this object message) => $"<i>{message}</i>";
	}
}