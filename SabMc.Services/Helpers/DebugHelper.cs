using System;

namespace SabMc.Services.Helpers
{
	/// <summary>
	/// Helper class for debugging.
	/// </summary>
	public static class DebugHelper
	{
		/// <summary>
		/// Write's an header to the console
		/// 
		/// Example: === START MESSAGE ===
		/// </summary>
		/// <param name="header">header text</param>
		public static void WriteHeader(string header)
		{
			Write(string.Empty);
			Write(string.Format(" === {0} ===", header.ToUpper()));
		}
		/// <summary>
		/// Write's a log message
		/// 
		/// Example: [LOG]   01-01-11 - message here
		/// </summary>
		/// <param name="message">the log message</param>
		public static void Log(string message)
		{
			Log(message, true);
		}
		/// <summary>
		/// Write's a log message
		/// 
		/// Example: [LOG]   01-01-11 - message here
		/// </summary>
		/// <param name="message">the log message</param>
		/// <param name="withDateTime">append datetime</param>
		public static void Log(string message, bool withDateTime)
		{
			if (withDateTime)
				Write(string.Format("[LOG]   {0} - {1}", DateTime.Now.ToString("dd-MM-yy HH:mm.ss"), message));
			else
				Write(string.Format("[LOG]   - {0}", message));
		}
		/// <summary>
		/// Write's a error message
		/// 
		/// Example: [ERROR]   01-01-11 - message here
		/// </summary>
		/// <param name="message">the error message</param>
		public static void Error(string message)
		{
			Error(message, true);
		}
		/// <summary>
		/// Write's a error message
		/// 
		/// Example: [ERROR]   01-01-11 - message here
		/// </summary>
		/// <param name="message">the error message</param>
		/// <param name="withDateTime">append datetime</param>
		public static void Error(string message, bool withDateTime)
		{
			if (withDateTime)
				Write(string.Format("[ERROR] {0} - {1}", DateTime.Now.ToString("dd-MM-yy HH:mm.ss"), message));
			else
				Write(string.Format("[ERROR] - {0}", message));
		}
		/// <summary>
		/// Write's a info message
		/// 
		/// Example: [INFO]    01-01-11 - message here
		/// </summary>
		/// <param name="message">the error message</param>
		public static void Info(string message)
		{
			Info(message, true);
		}
		/// <summary>
		/// Write's a info message
		/// 
		/// Example: [INFO]    01-01-11 - message here
		/// </summary>
		/// <param name="message">the error message</param>
		/// <param name="withDateTime">append datetime</param>
		public static void Info(string message, bool withDateTime)
		{
			if (withDateTime)
				Write(string.Format("[INFO]  {0} - {1}", DateTime.Now.ToString("dd-MM-yy HH:mm.ss"), message));
			else
				Write(string.Format("[INFO]  - {0}", message));
		}

		/// <summary>
		/// Write's an array
		/// </summary>
		/// <param name="message">message</param>
		/// <param name="array">the array</param>
		public static void WriteArray(string message, string[] array)
		{
			Info(message);
			for (int i = 0; i < array.Length; i++)
			{
				Write(string.Format("[{0:00}] = '{1}'", i, array[i]));
			}
			Write(string.Empty);
		}
		/// <summary>
		/// internal method for writing.. maybe later we could write to an file?
		/// </summary>
		/// <param name="str">string</param>
		internal static void Write(string str)
		{
			Console.WriteLine(str);
		}
	}
}
