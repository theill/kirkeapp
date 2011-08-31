using System;

namespace dk.kirkeapp {
	public class Log {
		public Log() {
		}

#if DEBUG
		public static void WriteLine(string value) {
			Console.WriteLine(value);
		}

		public static void WriteLine(string format, object[] arg) {
			if (arg == null || arg.Length == 0) {
				Console.WriteLine(format);
			}
			else {
				Console.WriteLine(format, arg);
			}
		}

		public static void WriteLine(string format, object arg0) {
			Console.WriteLine(format, arg0);
		}

		public static void WriteLine(string format, object arg0, object arg1) {
			Console.WriteLine(format, arg0, arg1);
		}

		public static void WriteLine(string format, object arg0, object arg1, object arg2) {
			Console.WriteLine(format, arg0, arg1, arg2);
		}
#else
		public static void WriteLine(string value) {
//			Console.WriteLine(value);
		}

		public static void WriteLine(string format, object[] arg) {
			if (arg == null || arg.Length == 0) {
//				Console.WriteLine(format);
			}
			else {
//				Console.WriteLine(format, arg);
			}
		}

		public static void WriteLine(string format, object arg0) {
//			Console.WriteLine(format, arg0);
		}

		public static void WriteLine(string format, object arg0, object arg1) {
//			Console.WriteLine(format, arg0, arg1);
		}

		public static void WriteLine(string format, object arg0, object arg1, object arg2) {
//			Console.WriteLine(format, arg0, arg1, arg2);
		}
#endif

	}
}

