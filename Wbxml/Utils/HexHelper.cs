using System;
using System.Collections.Generic;
using System.Text;

namespace Comtech.Utils
{
	public class HexHelper
	{
		public static string ToHexString(byte[] buffer)
		{
			return ToHexString(buffer, 0, buffer.Length);
		}

		public static string ToHexString(byte[] buffer, int offset, int count)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				byte b = buffer[offset + i];
				sb.Append(b.ToString("X2"));
				sb.Append(' ');
			}
			if (sb.Length > 0)
			{
				sb.Length--; // remove last space
			}
			return sb.ToString();
		}

		public static string ToHexDump(byte[] buffer, int offset, int count)
		{
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < count; i++)
			{
				byte b = buffer[offset + i];
				sb.Append(b.ToString("X2"));
				sb.Append(' ');
				if (i % 16 == 15)
				{
					// end of 16-byte line: print ISO chars
					for (int j = i - 15; j <= i; j++)
					{
						char c = (char)buffer[offset + j]; // ISO-8859-1
						if (Char.IsControl(c))
							c = '.';
						sb.Append(c);
					}
					sb.Append("\r\n");
				}
			}
			if (count % 16 != 0)
			{
				// last line is incomplete
				sb.Append(' ', 3 * (16 - (count % 16))); // print spaces instead of missing bytes
				// print ISO chars at the end 
				for (int i = count - (count % 16); i < count; i++)
				{
					char c = (char)buffer[offset + i]; // ISO-8859-1
					if (Char.IsControl(c))
						c = '.';
					sb.Append(c);
				}
			}
			else
			{
				sb.Length -= 2; // remove extra line break
			}
			return sb.ToString();
		}

		public static string ToHexDump(byte[] buffer)
		{
			return ToHexDump(buffer, 0, buffer.Length);
		}

		public static byte[] HexStringToByteArray(string src)
		{
			src = System.Text.RegularExpressions.Regex.Replace(src, @"\s", ""); // remove whitespace
			if (src.Length % 2 != 0)
			{
				throw new ArgumentException("hex string must contain an even number of non-whitespace characters");
			}
			byte[] result = new byte[src.Length / 2];
			for (int i = 0; i < result.Length; i++)
			{
				string hex = null;
				try
				{
					hex = src.Substring(i * 2, 2);
					result[i] = Convert.ToByte(hex, 16);
				}
				catch (Exception)
				{
					throw new ArgumentException("non-parsable hex value \"" + hex + "\" at " + (i * 2));
				}
			}
			return result;
		}


	}
}
